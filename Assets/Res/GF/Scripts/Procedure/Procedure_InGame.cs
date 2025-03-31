using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class Procedure_InGame : ProcedureBase
    {
        private bool bool_ChangeToProcedureMenu;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            bool_ChangeToProcedureMenu = false;

            int inGameID = Main.UI.OpenUIForm(AssetUtility.GetUIFormAsset(nameof(UI_InGameForm)), "Default", this);
            Main.DataNode.SetData<VarInt32>("UIForm.UI_InGameForm.Id", inGameID);
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

            Main.UI.CloseUIForm(Main.DataNode.GetData<VarInt32>("UIForm.UI_InGameForm.Id"));
        }

        public void BackMenu()
        {
            bool_ChangeToProcedureMenu = true;
        }
    }
}
