using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class Procedure_ToLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GlobalManager.Instance.procedureOwner = procedureOwner;

            if (!Main.Base.EditorResourceMode)
            {
                Log.Info($"<color=orange>Non-editor mode running...</color>");
                Main.Resource.InitResources(OnInitResourcesComplete);
            }

            FsmActor actor = new FsmActor();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            ChangeState<Procedure_Menu>(procedureOwner);
        }

        private void OnInitResourcesComplete()
        {
            Log.Info("<color=green>Init resources complete.</color>");
        }
    }
}
