using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("---------- Component")]
    [SerializeField] private Image img_State;
    [SerializeField] private Text txt_Content;
    [SerializeField] private Image img_Icon;

    [Header("---------- State Image")]
    [ShowIf("img_State")][SerializeField] private Sprite spr_Normal;
    [ShowIf("img_State")][SerializeField] private Sprite spr_Select;
    [ShowIf("img_State")][SerializeField] private Sprite spr_Hover;

    [Header("---------- State Text")]
    [ShowIf("txt_Content")][SerializeField] private Color color_Normal;
    [ShowIf("txt_Content")][SerializeField] private Color color_Select;
    [ShowIf("txt_Content")][SerializeField] private Color color_Hover;

    [Header("---------- IsOn")]
    private bool bool_IsOn;

    public void ChangeState(bool isOn)
    {
        bool_IsOn = isOn;
        if (img_State != null)
        {
            img_State.sprite = bool_IsOn ? spr_Select : spr_Normal;
        }

        if (txt_Content != null)
        {
            txt_Content.color = bool_IsOn ? color_Select : color_Normal;
        }

        if (img_Icon != null)
        {
            img_Icon.gameObject.SetActive(bool_IsOn);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (img_State != null)
        {
            img_State.sprite = bool_IsOn ? spr_Select : spr_Normal;
        }

        if (txt_Content != null)
        {
            txt_Content.color = bool_IsOn ? color_Select : color_Hover;
        }

        if (img_Icon != null)
        {
            img_Icon.gameObject.SetActive(bool_IsOn);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (img_State != null)
        {
            img_State.sprite = bool_IsOn ? spr_Select : spr_Normal;
        }

        if (txt_Content != null)
        {
            txt_Content.color = bool_IsOn ? color_Select : color_Normal;
        }

        if (img_Icon != null)
        {
            img_Icon.gameObject.SetActive(bool_IsOn);
        }
    }
}
