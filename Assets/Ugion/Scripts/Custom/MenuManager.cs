using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//                 _ooOoo_
//                o8888888o
//                88" . "88
//                (| -_- |)
//                O\  =  /O
//             ____/`---'\____
//           .'  \\|     |//  `.
//          /  \\|||  :  |||//  \
//         / _ ||||| -:- ||||| - \
//         |   | \\\  -  /// |   |
//         | \_|  ''\---/ '' |_/ |
//         \  .-\__  `-`  __/-.  /
//       ___`. .'  /--.--\  `. . ___
//    ."" '<  `.___\_<|>_/___.'  > '"".
//   | | :  `- \`.;`\ _ /`;.`/ -`  : | |
//   \  \ `-.   \_ __\ /__ _/   .-` /  /
//====`-.____`-.___\_____/___.-`____.-'======
//                 `=---='
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//          拜赛博佛祖       积电子功德

namespace Launch
{
    public class MenuManager : MonoBehaviour
    {
        private Transform trans_Parent;

        [BoxGroup("必要预制")] public GameObject pre_SpreadOption;
        [BoxGroup("必要预制")] public GameObject pre_SubMenu;

        [BoxGroup("配置项")] public ProcedureData procedureData;
        [BoxGroup("配置项")][SerializeField] private bool _IsDefaultEnter;

        private int currentMainIndex = -1;
        private int currentSubIndex = -1;
        private List<GameObject> spreadOptions = new List<GameObject>();
        private Dictionary<OptionBase, List<OptionBase>> options = new Dictionary<OptionBase, List<OptionBase>>();

        [ReadOnly][ShowInInspector][BoxGroup("实时追踪数据")] private OptionBase currentOption;
        [ReadOnly][ShowInInspector][BoxGroup("实时追踪数据")] private ProcedureConfig currentProcedure;

        [ReadOnly][ShowInInspector][BoxGroup("动态对象")] private List<ProcedureConfig> validProcedureConfigs = new List<ProcedureConfig>();

        // 步骤索引标识
        private int mainIndex = 0;
        private int subIndex = 0;

        public void Initialize()
        {
            trans_Parent = GameObject.Find("Main Canvas/Panel_MenuForm/Scroll Vertical/Viewport/Content").transform;
            InitOptions(procedureData);
        }

        private void InitOptions(ProcedureData procedureData)
        {
            currentMainIndex = -1;
            currentSubIndex = -1;

            UnLoadAllOptions();
            LoadAllOptions(procedureData);

            if (_IsDefaultEnter)
            {
                EnterProcedure(options.Keys.First());
            }
        }

        private void LoadAllOptions(ProcedureData procedureData)
        {
            mainIndex = 0;
            subIndex = 0;

            procedureData.Procedures.ForEach(i =>
            {
                CreateOption(i, trans_Parent);
            });
        }

        private void CreateOption(ProcedureInfo procedureInfo, Transform parent)
        {
            GameObject spreadOption = Instantiate(pre_SpreadOption, parent);
            spreadOption.name = $"Option_{procedureInfo.ProcedureConfig.procedureTitle}";
            spreadOptions.Add(spreadOption);

            OptionBase option = spreadOption.GetComponentInChildren<OptionBase>();
            option.gameObject.name = $"Main_{procedureInfo.ProcedureConfig.procedureTitle}";
            option.Initialize(this, procedureInfo, mainIndex++);

            options.Add(option, new List<OptionBase>());

            Transform subSpread = spreadOption.transform.Find("Sub Spread");

            if (procedureInfo.hasExtension)
            {
                subSpread.GetComponent<FlexSubOptions>().preferredHeight = option.procedureInfo.extendedProcedures.Count * pre_SubMenu.GetComponent<RectTransform>().rect.height; ;

                foreach (var config in option.procedureInfo.extendedProcedures)
                {
                    OptionBase subOption = Instantiate(pre_SubMenu, subSpread).GetComponent<OptionBase>();
                    RectTransform subRect = subOption.GetComponent<RectTransform>();
                    subOption.gameObject.name = $"Sub_{config.procedureTitle}";

                    if (subSpread.childCount > 1)
                    {
                        Vector3 lastPos = subSpread.GetChild(subSpread.childCount - 2).GetComponent<RectTransform>().anchoredPosition;
                        Vector3 newPos = new Vector3(lastPos.x, lastPos.y - subRect.rect.height, 0);
                        subRect.anchoredPosition = newPos;
                    }

                    ProcedureInfo info = new ProcedureInfo(config, false, null);
                    validProcedureConfigs.Add(config);
                    subOption.Initialize(this, info, subIndex++);

                    if (options.ContainsKey(option))
                    {
                        options[option].Add(subOption);
                    }
                    else
                    {
                        options.Add(option, new List<OptionBase>() { subOption });
                    }
                }
            }
            else
            {
                validProcedureConfigs.Add(procedureInfo.ProcedureConfig);
            }
        }

        public void UnLoadAllOptions()
        {
            // 使用HashSet避免重复销毁
            var allOptions = new HashSet<GameObject>();

            // 所有子选项
            foreach (var subList in options.Values)
            {
                if (subList == null) continue;

                foreach (var option in subList)
                {
                    if (option != null) allOptions.Add(option.gameObject);
                }
            }

            // 所有主选项
            foreach (var mainOption in options.Keys)
            {
                if (mainOption != null) allOptions.Add(mainOption.gameObject);
            }

            // 全步骤
            foreach (var spreadOption in spreadOptions)
            {
                if (spreadOption != null) allOptions.Add(spreadOption);
            }

            // 统一销毁
            foreach (var option in allOptions)
            {
                if (option.gameObject == null) continue;

                if (Application.isPlaying)
                {
                    Destroy(option.gameObject);
                }
                else
                {
                    DestroyImmediate(option.gameObject);
                }
            }

            options.Clear();
            spreadOptions.Clear();

            currentOption = null;
            currentProcedure = new ProcedureConfig();
            validProcedureConfigs.Clear();
        }

        public void ChangeState(OptionBase currOption)
        {
            bool isMain = JudgeMainProcedure(currOption, out OptionBase mainProcedure);

            // 关闭其他已选中的流程
            if (isMain)
            {
                // 不可重复选
                foreach (var option in options.Keys.Where(o => o == currOption && o.Bool_IsOn))
                {
                    return;
                }

                // 模式锁环节
                if (Main.Global.GameMode == GameMode.Exam && currOption.index < currentMainIndex) return;

                // 关闭其他主流程及其子流程
                foreach (var option in options.Keys.Where(o => o != currOption && o.Bool_IsOn))
                {
                    CloseOptionWithSubs(option);
                }
            }
            else
            {
                // 不可重复选
                foreach (var option in options[mainProcedure].Where(o => o == currOption && o.Bool_IsOn))
                {
                    return;
                }

                if (Main.Global.GameMode == GameMode.Exam && currOption.index < currentSubIndex) return;

                // 关闭当前主流程下的其他子流程
                foreach (var option in options[mainProcedure].Where(o => o != currOption && o.Bool_IsOn))
                {
                    CloseOption(option);
                }
            }

            // 主流程到子流程出问题
            // 启用当前选项
            currOption.ChangeState(true);
            currentOption = currOption;
            // 启用拓展第一项
            if (currOption.procedureInfo.hasExtension)
            {
                options[mainProcedure].First().ChangeState(true);
                currentOption = options[mainProcedure].First();
                currentSubIndex = options[mainProcedure].First().index;
            }

            if (isMain)
                currentMainIndex = currOption.index;
            else
                currentSubIndex = currOption.index;
        }

        private void CloseOptionWithSubs(OptionBase option)
        {
            if (option.procedureInfo.hasExtension)
            {
                options[option].ForEach(o =>
                {
                    if (o.Bool_IsOn)
                    {
                        CloseOption(o);
                    }
                });
            }

            CloseOption(option);
        }

        private void CloseOption(OptionBase option)
        {
            option.ChangeState(false);
        }

        private bool JudgeMainProcedure(OptionBase option, out OptionBase mainProcedure)
        {
            // 检查是否是主流程(字典键)
            if (options.TryGetValue(option, out _))
            {
                mainProcedure = option;
                return true;
            }

            // 是子流程，out子流程的主流程
            mainProcedure = options.FirstOrDefault(kv => kv.Value?.Contains(option) == true).Key;
            return false;
        }

        /// <summary>
        /// 自定义开启选项
        /// </summary>
        private void EnterProcedure(OptionBase option)
        {
            bool isMain = JudgeMainProcedure(option, out OptionBase _Main);

            if (isMain && option.procedureInfo.hasExtension)
            {
                ChangeState(option);
                ChangeState(options[option].First());
            }
            else if (isMain && !option.procedureInfo.hasExtension)
            {
                ChangeState(option);
            }
            else if (!isMain)
            {
                ChangeState(_Main);
                ChangeState(option);
            }
        }

        private string GetTitleOfOption(OptionBase option)
        {
            string[] parts = option.gameObject.name.Split('_');
            return parts[1];
        }

        private OptionBase GetOptionByConfig(ProcedureConfig config)
        {
            foreach (var key in options.Keys)
            {
                if (GetTitleOfOption(key) == config.procedureTitle)
                    return key;
            }

            foreach (var value in options.Values)
            {
                foreach (var item in value)
                {
                    if (GetTitleOfOption(item) == config.procedureTitle)
                        return item;
                }
            }

            return null;
        }

        public bool NextProcedure()
        {
            // 奇葩 bug，（若在Update中使用Input.GetKeyDown(KeyCode.Space)调用此函数出现）。当使用该函数跳转流程时，突然用点击Menu按钮来跳转，可能会出现currentConfigIndex索引值混乱的情况，
            // 然后接着使用该函数调整会出现在某些流程中循环的情况（无法按正常走完所有流程），但是用点击按钮的方式选中以后，在点击一次Menu外的区域，就不会出现这种情况。
            int configCount = validProcedureConfigs.Count;
            int currentConfigIndex = validProcedureConfigs.IndexOf(currentProcedure);

            if (currentConfigIndex < configCount - 1)
            {
                OptionBase option = GetOptionByConfig(validProcedureConfigs[++currentConfigIndex]);
                EnterProcedure(option);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 框架进入流程
        /// </summary>
        public void ChangeProcedure(ProcedureConfig config)
        {
            Main.Global.ChangeProcedure(config);
            currentProcedure = config;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
