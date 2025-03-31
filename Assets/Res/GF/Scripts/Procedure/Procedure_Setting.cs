using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class Procedure_Setting : ProcedureBase
    {
        private bool bool_ChangeToProcedureMenu;

        private float float_gameSpeed;

        public float Float_GameSpeed
        {
            get => float_gameSpeed;
        }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            bool_ChangeToProcedureMenu = false;

            float_gameSpeed = Main.Base.GameSpeed;

            int settingID = Main.UI.OpenUIForm(AssetUtility.GetUIFormAsset(nameof(UI_SettingForm)), "Default", this);
            Main.DataNode.SetData<VarInt32>("UIForm.UI_SettingForm.Id", settingID);

            ControlPauseGame(true);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (bool_ChangeToProcedureMenu)
            {
                ChangeState<Procedure_Menu>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            Main.UI.CloseUIForm(Main.DataNode.GetData<VarInt32>("UIForm.UI_SettingForm.Id"));

            ControlPauseGame(false);
            SettingModify();
        }

        public void ControlPauseGame(bool isPause)
        {
            if (isPause)
                Main.Base.PauseGame();
            else
                Main.Base.ResumeGame();
        }

        public void ChangeToMenu()
        {
            bool_ChangeToProcedureMenu = true;
        }

        public void ControlFrameRate(int frameRate)
        {
            Main.Base.FrameRate = frameRate;
        }

        // 伪设置,退出设置流程才实际设置
        public void FakeSetGameSpeed(float speed)
        {
            float_gameSpeed = speed;
        }

        public void SettingModify()
        {
            Main.Base.GameSpeed = float_gameSpeed;
        }
    }
}
