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

        [ReadOnly][ShowInInspector][BoxGroup("实时追踪数据")] private ProcedureConfig currentProcedure;
        [ReadOnly][ShowInInspector][BoxGroup("实时追踪数据")] private int currentMainIndex = -1;
        [ReadOnly][ShowInInspector][BoxGroup("实时追踪数据")] private int currentSubIndex = -1;

        [ReadOnly][ShowInInspector][BoxGroup("动态对象")] private List<GameObject> spreadOptions = new List<GameObject>();
        [ReadOnly][ShowInInspector][BoxGroup("动态对象")] private Dictionary<OptionBase, List<OptionBase>> options = new Dictionary<OptionBase, List<OptionBase>>();

        // 步骤索引标识
        private int mainIndex = 0;
        private int subIndex = 0;

        public void Initialize()
        {
            trans_Parent = GameObject.Find("MainCanvas/Panel_Menu/Scroll Vertical/Viewport/Content").transform;

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
                        Vector3 lastPos = subSpread.GetChild(subSpread.childCount - 2).GetComponent<RectTransform>().position;
                        Vector3 newPos = new Vector3(lastPos.x, lastPos.y - subRect.rect.height, 0);
                        subRect.position = newPos;
                    }

                    ProcedureInfo info = new ProcedureInfo(config, false, null);
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

            // 启用当前选项
            currOption.ChangeState(true);
            // 启用拓展第一项
            if (currOption.procedureInfo.hasExtension)
            {
                options[mainProcedure].First().ChangeState(true);
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

        /// <summary>
        /// 框架进入流程
        /// </summary>
        public void ChangeProcedure(ProcedureConfig config)
        {
            Main.Event.Fire(this, Event_ChangeState.Create(new Event_ChangeState(config)));
            currentProcedure = config;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
