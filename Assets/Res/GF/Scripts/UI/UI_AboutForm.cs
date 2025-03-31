using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class UI_AboutForm : UGUIForm
    {
        [SerializeField]
        private RectTransform rectTrans_Content = null;

        [SerializeField]
        private float float_ScrollSpeed = 1f;
        private float float_InitPosition = 0f;

        private Button but_BackExit;

        private Procedure_Menu procedure_Menu = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            but_BackExit = transform.Find("BackButton").GetComponent<Button>();

            but_BackExit.onClick.AddListener(() =>
            {
                Close();
                int menuId = Main.UI.OpenUIForm(AssetUtility.GetUIFormAsset(nameof(UI_MenuForm)), "Default", procedure_Menu);
                Main.DataNode.SetData<VarInt32>("UIForm.UI_MenuForm.Id", menuId);
            });

            CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Log.Warning("Can not find CanvasScaler component.");
                return;
            }

            float_InitPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedure_Menu = (Procedure_Menu)userData;
            if (procedure_Menu == null)
            {
                Log.Warning("ProcedureMenuTest is invalid when open AboutForm.");
                return;
            }

            rectTrans_Content.SetLocalPositionY(float_InitPosition);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            procedure_Menu = null;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            rectTrans_Content.AddLocalPositionY(float_ScrollSpeed * elapseSeconds);
            if (rectTrans_Content.localPosition.y > rectTrans_Content.sizeDelta.y - float_InitPosition)
            {
                rectTrans_Content.SetLocalPositionY(float_InitPosition);
            }
        }
    }
}