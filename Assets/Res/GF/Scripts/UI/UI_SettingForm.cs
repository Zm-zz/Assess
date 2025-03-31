using UnityEngine.UI;

namespace Launch
{
    public class UI_SettingForm : UGUIForm
    {
        private Dropdown dropDown_FrameRate;
        private Dropdown dropDown_GameSpeed;
        private Button but_ReturnMenu;

        private Procedure_Setting procedure_Setting = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dropDown_FrameRate = transform.Find("SettingPanel/Content/FrameRate/DropDown_FrameRate").GetComponent<Dropdown>();
            dropDown_GameSpeed = transform.Find("SettingPanel/Content/GameSpeed/DropDown_GameSpeed").GetComponent<Dropdown>();
            but_ReturnMenu = transform.Find("SettingPanel/Content/ReturnMenu/But_Return").GetComponent<Button>();

            dropDown_FrameRate.onValueChanged.AddListener((value) =>
            {
                procedure_Setting.ControlFrameRate(int.Parse(dropDown_FrameRate.options[value].text));
            });

            dropDown_GameSpeed.onValueChanged.AddListener((value) =>
            {
                procedure_Setting.FakeSetGameSpeed(float.Parse(dropDown_GameSpeed.options[value].text));
            });

            but_ReturnMenu.onClick.AddListener(() =>
            {
                procedure_Setting.ChangeToMenu();
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedure_Setting = (Procedure_Setting)userData;

            InitValue();
        }

        // 面板打开，初始化选项值
        private void InitValue()
        {
            for (int i = 0; i < dropDown_FrameRate.options.Count; i++)
            {
                if (int.Parse(dropDown_FrameRate.options[i].text) == Main.Base.FrameRate)
                {
                    dropDown_FrameRate.value = i;
                    break;
                }
            }

            for (int i = 0; i < dropDown_GameSpeed.options.Count; i++)
            {
                if (float.Parse(dropDown_GameSpeed.options[i].text) == procedure_Setting.Float_GameSpeed)
                {
                    dropDown_GameSpeed.value = i;
                    break;
                }
            }
        }
    }
}
