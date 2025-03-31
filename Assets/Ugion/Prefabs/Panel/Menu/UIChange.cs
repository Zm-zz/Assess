using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("---------- Component")]
    [SerializeField] private Image img_State;
    [SerializeField] private Text txt_Content;

    [Header("---------- State Image")]
    [SerializeField] private Sprite spr_Normal;
    [SerializeField] private Sprite spr_Select;
    [SerializeField] private Sprite spr_Hover;

    [Header("---------- State Text")]
    [SerializeField] private Color color_Normal;
    [SerializeField] private Color color_Select;
    [SerializeField] private Color color_Hover;

    [Header("---------- IsOn")]
    private bool bool_IsOn;

    public void ChangeState(bool isOn)
    {
        bool_IsOn = isOn;
        img_State.sprite = bool_IsOn ? spr_Select : spr_Normal;
        txt_Content.color = bool_IsOn ? color_Select : color_Normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img_State.sprite = bool_IsOn ? spr_Select : spr_Normal;
        txt_Content.color = bool_IsOn ? color_Select : color_Hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img_State.sprite = bool_IsOn ? spr_Select : spr_Normal;
        txt_Content.color = bool_IsOn ? color_Select : color_Normal;
    }
}
