using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Launch
{
    public class Procedure_TestEvent : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Main.Event.Subscribe(TestEvent.EventId, OnTestEventFire);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetMouseButtonDown(0))
            {
                Main.Event.Fire(this, TestEvent.Create(new TestEvent("�����¼�", 2, new TestEvent())));
            }
        }

        private void OnTestEventFire(object sender, GameEventArgs e)
        {
            TestEvent ne = (TestEvent)e;
            if (ne != null)
            {
                Log.Info(ne.ConfigAssetName);
                Log.Info(ne.Duration);
                Log.Info(ne.UserData);

                Log.Info($"<color=green>�¼����ã�</color>{ne.ConfigAssetName}  ����");
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            Main.Event.Unsubscribe(TestEvent.EventId, OnTestEventFire);
        }

    }
}
