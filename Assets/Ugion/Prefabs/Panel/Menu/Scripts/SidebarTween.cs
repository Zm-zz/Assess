using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SidebarTween : MonoBehaviour
{
    private RectTransform rectTrans_Sidebar;
    public Button but_Target;
    public float float_Duration = 0.3f;
    public float float_HideOffset = 35;
    public Ease enum_EaseType = Ease.OutQuad;

    private Vector2 v2_HiddenPosition;
    private Vector2 v2_ShownPosition;

    private RectTransform rectTrans_RotationArrow;
    private Vector3 v3_ArrowShowRotation;
    private Vector3 v3_ArrowHideRotation;

    private bool bool_IsHidden = false;

    private void Start()
    {
        rectTrans_Sidebar = GetComponent<RectTransform>();
        rectTrans_RotationArrow = but_Target.GetComponent<RectTransform>();

        v2_ShownPosition = rectTrans_Sidebar.anchoredPosition;
        v2_HiddenPosition = new Vector2(-rectTrans_Sidebar.rect.width + float_HideOffset, 0);
        v3_ArrowShowRotation = rectTrans_RotationArrow.localEulerAngles;
        v3_ArrowHideRotation = new Vector3(0, 0, v3_ArrowShowRotation.y + 180);

        but_Target.onClick.AddListener(ToggleSidebar);
    }

    private void ToggleSidebar()
    {
        bool_IsHidden = !bool_IsHidden;
        rectTrans_Sidebar.DOAnchorPos(bool_IsHidden ? v2_HiddenPosition : v2_ShownPosition, float_Duration).SetEase(enum_EaseType);
        rectTrans_RotationArrow.DOLocalRotate(bool_IsHidden ? v3_ArrowHideRotation : v3_ArrowShowRotation, float_Duration).SetEase(enum_EaseType);
    }
}
