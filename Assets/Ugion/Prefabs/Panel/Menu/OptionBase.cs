using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(UIChange))]
public class OptionBase : MonoBehaviour
{
    [Header("---------- IsOn")]
    [ReadOnly][SerializeField] private bool bool_IsOn;

    private string str_ProcedureName;
    private string str_ProcedureTitle;

    public MenuManager menuManager;
    private Button but_Self;
    private UIChange _UIChange;
    private bool bool_isSpread;
    private UnityAction onClick;

    public bool Bool_IsOn
    {
        get => bool_IsOn;
        set
        {
            bool_IsOn = value;
            OnStateChange(bool_IsOn);
        }
    }

    public virtual void ChangeState(bool isOn)
    {
        Bool_IsOn = isOn;
    }

    /// <summary>
    /// 初始化 
    /// clickAction 为 null，不会赋值
    /// </summary>
    /// <param name="isOn"></param>
    /// <param name="clickAction"></param>
    public virtual void Initialize(bool isOn, bool isSpread, MenuManager menuManager, string procedureName, string procedureTitle, UnityAction clickAction = null)
    {
        but_Self = GetComponent<Button>();
        _UIChange = GetComponent<UIChange>();

        Bool_IsOn = isOn;
        bool_isSpread = isSpread;
        str_ProcedureName = procedureName;
        str_ProcedureTitle = procedureTitle;
        this.menuManager = menuManager;

        if (clickAction != null)
        {
            onClick = clickAction;
        }

        transform.GetComponentInChildren<Text>().text = procedureTitle;

        but_Self.onClick.RemoveAllListeners();
        but_Self.onClick.AddListener(() => Bool_IsOn = !Bool_IsOn);
    }

    public virtual void OnStateChange(bool isOn)
    {
        _UIChange.ChangeState(isOn);

        if (isOn)
        {
            onClick?.Invoke();
            menuManager.ChangeState(this);

            Debug.Log($"进入流程：<color=green>{str_ProcedureTitle}</color>");
        }
        else
        {
            Debug.Log($"离开流程：<color=green>{str_ProcedureTitle}</color>");
        }
    }


}
