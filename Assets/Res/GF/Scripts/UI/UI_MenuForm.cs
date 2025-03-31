using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class UI_MenuForm : UGUIForm
    {
        private Button but_Start;
        private Button but_About;
        private Button but_Setting;
        private Button but_Exit;

        private Procedure_Menu procedure_Menu = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            but_Start = transform.Find("Panel/Content/But_Content/But_Start").GetComponent<Button>();
            but_About = transform.Find("Panel/Content/But_Content/But_About").GetComponent<Button>();
            but_Setting = transform.Find("Panel/Content/But_Content/But_Setting").GetComponent<Button>();
            but_Exit = transform.Find("Panel/Content/But_Content/But_Exit").GetComponent<Button>();

            but_Start.onClick.AddListener(OnStartButtonClick);
            but_About.onClick.AddListener(OnAboutButtonClick);
            but_Setting.onClick.AddListener(OnSettingButtonClick);
            but_Exit.onClick.AddListener(OnExitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedure_Menu = (Procedure_Menu)userData;
            if (procedure_Menu == null)
            {
                Log.Warning("ProcedureMenuTest is invalid when open MenuForm.");
                return;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            procedure_Menu = null;
        }

        public void OnStartButtonClick()
        {
            procedure_Menu.StartGame();
        }

        public void OnAboutButtonClick()
        {
            procedure_Menu.OpenAboutForm();
        }

        public void OnSettingButtonClick()
        {
            procedure_Menu.OpenSettingForm();
        }

        public void OnExitButtonClick()
        {
            procedure_Menu.ExitGame();
        }
    }
}
