using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class UI_InGameForm : UGUIForm
    {
        private Procedure_InGame procedure_InGame = null;

        private Button but_BackMenu;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            but_BackMenu = transform.Find("But_BackMenu").GetComponent<Button>();

            but_BackMenu.onClick.AddListener(OnBackMenuClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedure_InGame = (Procedure_InGame)userData;
            if (procedure_InGame == null)
            {
                Log.Warning("ProcedureInGame is invalid when open InGameForm.");
                return;
            }
        }

        private void OnBackMenuClick()
        {
            procedure_InGame.BackMenu();
        }
    }
}