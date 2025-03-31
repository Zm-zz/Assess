using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class Procedure_Menu : ProcedureBase
    {
        private bool bool_ChangeToProcedureInGame;
        private bool bool_ChangeToProcedureSetting;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            bool_ChangeToProcedureInGame = false;
            bool_ChangeToProcedureSetting = false;

            int menuId = Main.UI.OpenUIForm(AssetUtility.GetUIFormAsset(nameof(UI_MenuForm)), "Default", this);
            Main.DataNode.SetData<VarInt32>("UIForm.UI_MenuForm.Id", menuId);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (bool_ChangeToProcedureInGame)
            {
                ChangeState<Procedure_InGame>(procedureOwner);
            }
            else if (bool_ChangeToProcedureSetting)
            {
                ChangeState<Procedure_Setting>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            Main.UI.CloseUIForm(Main.DataNode.GetData<VarInt32>("UIForm.UI_MenuForm.Id"));
        }

        public void StartGame()
        {
            Log.Info("Start Game !!!");

            bool_ChangeToProcedureInGame = true;
        }

        public void OpenAboutForm()
        {
            Log.Info("OpenAboutForm !!!");

            Main.UI.CloseUIForm(Main.DataNode.GetData<VarInt32>("UIForm.UI_MenuForm.Id"));

            if (!Main.UI.IsLoadingUIForm(AssetUtility.GetUIFormAsset(nameof(UI_AboutForm))))
            {
                int aboutId = Main.UI.OpenUIForm(AssetUtility.GetUIFormAsset(nameof(UI_AboutForm)), "Default", this);
            }
        }

        public void OpenSettingForm()
        {
            Log.Info("OpenSetting !!!");

            bool_ChangeToProcedureSetting = true;
        }

        public void ExitGame()
        {
            Log.Info("Exit Game !!!");

            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }
    }
}
