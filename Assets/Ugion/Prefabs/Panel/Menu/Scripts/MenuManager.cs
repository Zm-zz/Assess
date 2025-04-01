using Launch;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
//          ����������       �����ӹ���


public class MenuManager : MonoBehaviour
{
    private Transform trans_Parent;

    [Header("---------- Create")]
    public GameObject pre_ParMenu;
    public GameObject pre_SubPar;
    public GameObject pre_SubMenu;

    public ProcedureData procedureData;

    [Header("---------- Config")]
    [SerializeField] private bool _IsDefaultEnter;
    [ReadOnly][ShowInInspector] private ProcedureConfig currentProcedure;

    [ReadOnly][ShowInInspector] private Dictionary<OptionBase, List<OptionBase>> options = new Dictionary<OptionBase, List<OptionBase>>();

    public void Initialize()
    {
        trans_Parent = transform.Find("Scroll Vertical/Viewport/Content");

        UnLoadAllOptions();
        LoadAllOptions();

        if (_IsDefaultEnter)
        {
            EnterProcedure(options.Keys.First());
        }
    }

    private void LoadAllOptions()
    {
        procedureData.Procedures.ForEach(i =>
        {
            CreateOption(i, trans_Parent);
        });
    }

    private void CreateOption(ProcedureInfo procedureInfo, Transform parent, UnityAction clickAction = null)
    {
        OptionBase option = Instantiate(pre_ParMenu, parent).GetComponent<OptionBase>();
        option.gameObject.name = $"Main_{procedureInfo.ProcedureConfig.procedureTitle}";
        option.Initialize(this, procedureInfo);

        options.Add(option, new List<OptionBase>());

        if (procedureInfo.hasExtension)
        {
            GameObject subPar = Instantiate(pre_SubPar, parent);
            subPar.name = $"SubParent_{procedureInfo.ProcedureConfig.procedureTitle}";
            option.extensionMenu = subPar.transform;

            foreach (var config in option.procedureInfo.extendedProcedures)
            {
                OptionBase subOption = Instantiate(pre_SubMenu, subPar.transform).GetComponent<OptionBase>();
                subOption.gameObject.name = $"Sub_{config.procedureTitle}";
                ProcedureInfo info = new ProcedureInfo(config, false, null);
                subOption.Initialize(this, info);

                if (options.ContainsKey(option))
                {
                    options[option].Add(subOption);
                }
                else
                {
                    options.Add(option, new List<OptionBase>() { subOption });
                }
            }

            subPar.SetActive(false);
        }
    }

    public void UnLoadAllOptions()
    {
        // ʹ��HashSet�����ظ�����
        var allOptions = new HashSet<OptionBase>();

        // �ռ�����������
        foreach (var subList in options.Values)
        {
            if (subList == null) continue;

            foreach (var option in subList)
            {
                if (option != null) allOptions.Add(option);
            }
        }

        // �ռ�����������
        foreach (var mainOption in options.Keys)
        {
            if (mainOption != null) allOptions.Add(mainOption);
        }

        // ͳһ����
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
    }

    private bool JudgeMainProcedure(OptionBase option, out OptionBase mainProcedure)
    {
        // ����Ƿ���������(�ֵ��)
        if (options.TryGetValue(option, out _))
        {
            mainProcedure = option;
            return true;
        }

        // ������Ϊ�����̵�������
        mainProcedure = options.FirstOrDefault(kv => kv.Value?.Contains(option) == true).Key;
        return false;
    }

    public void ChangeState(OptionBase currOption)
    {
        bool isMain = JudgeMainProcedure(currOption, out OptionBase mainProcedure);

        // �ر�������ѡ�е�����
        if (isMain)
        {
            // �����ظ�ѡ
            foreach (var option in options.Keys.Where(o => o == currOption && o.Bool_IsOn))
            {
                return;
            }

            // �ر����������̼���������
            foreach (var option in options.Keys.Where(o => o != currOption && o.Bool_IsOn))
            {
                CloseOptionWithSubs(option);
            }
        }
        else
        {
            // �����ظ�ѡ
            foreach (var option in options[mainProcedure].Where(o => o == currOption && o.Bool_IsOn))
            {
                return;
            }

            // �رյ�ǰ�������µ�����������
            foreach (var option in options[mainProcedure].Where(o => o != currOption && o.Bool_IsOn))
            {
                CloseOption(option);
            }
        }

        // ���õ�ǰѡ��
        currOption.ChangeState(true);
        // ������չ��һ��
        if (currOption.procedureInfo.hasExtension)
        {
            options[mainProcedure].First().ChangeState(true);
        }
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
    /// �Զ����������
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
    /// ��ܽ�������
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
