using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(UIChange))]
public class OptionBase : MonoBehaviour
{
    [Header("---------- IsOn")]
    [ReadOnly][SerializeField] private bool bool_IsOn;
    [ReadOnly] public ProcedureInfo procedureInfo;
    [ReadOnly] public MenuManager menuManager;

    private Button but_Self;
    private UIChange _UIChange;

    [HideInInspector]
    public Transform extensionMenu;

    public bool Bool_IsOn
    {
        get => bool_IsOn;
        set
        {
            bool_IsOn = value;
        }
    }

    public virtual void ChangeState(bool isOn)
    {
        Bool_IsOn = isOn;
        _UIChange.ChangeState(isOn);

        if (procedureInfo.hasExtension)
        {
            extensionMenu.gameObject.SetActive(isOn);
        }
        else
        {
            if (isOn)
            {
                // 执行流程
                menuManager.ChangeProcedure(procedureInfo.ProcedureConfig);
            }
        }
    }

    /// <summary>
    /// 初始化 
    /// clickAction 为 null，不会赋值
    /// </summary>
    public virtual void Initialize(MenuManager menuManager, ProcedureInfo procedureInfo)
    {
        but_Self = GetComponent<Button>();
        _UIChange = GetComponent<UIChange>();

        bool_IsOn = false;

        this.procedureInfo = procedureInfo;
        this.menuManager = menuManager;

        transform.GetComponentInChildren<Text>().text = procedureInfo.ProcedureConfig.procedureTitle;

        but_Self.onClick.RemoveAllListeners();
        but_Self.onClick.AddListener(() => menuManager.ChangeState(this));
    }
}
