using GameFramework.Fsm;
using Launch;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 自定义状态机
/// </summary>
public class FsmActor
{
    private IFsm<FsmActor> m_Fsm = null;

    public FsmActor()
    {
        m_Fsm = Main.Fsm.CreateFsm("ActorFsm", this, new ActorIdleState(), new ActorMoveState());
        m_Fsm.Start<ActorIdleState>();
    }
}

public class ActorIdleState : FsmState<FsmActor>
{
    protected override void OnInit(IFsm<FsmActor> fsm)
    {
        base.OnInit(fsm);

        Log.Info("ActorIdle Init!");
    }

    protected override void OnEnter(IFsm<FsmActor> fsm)
    {
        base.OnEnter(fsm);

        Log.Info("ActorIdle Enter!");
    }

    protected override void OnUpdate(IFsm<FsmActor> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (Input.GetKeyUp(KeyCode.C))
        {
            ChangeState<ActorMoveState>(fsm);
        }
    }

    protected override void OnLeave(IFsm<FsmActor> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        Log.Info("ActorIdle Leave!");
    }

    protected override void OnDestroy(IFsm<FsmActor> fsm)
    {
        base.OnDestroy(fsm);

        Log.Info("ActorIdle Destroy!");
    }
}

public class ActorMoveState : FsmState<FsmActor>
{
    protected override void OnInit(IFsm<FsmActor> fsm)
    {
        base.OnInit(fsm);

        Log.Info("ActorMove Init!");
    }

    protected override void OnEnter(IFsm<FsmActor> fsm)
    {
        base.OnEnter(fsm);

        Log.Info("ActorMove Enter!");
    }

    protected override void OnUpdate(IFsm<FsmActor> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (Input.GetKeyUp(KeyCode.C))
        {
            ChangeState<ActorIdleState>(fsm);
        }
    }

    protected override void OnLeave(IFsm<FsmActor> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        Log.Info("ActorMove Leave!");
    }

    protected override void OnDestroy(IFsm<FsmActor> fsm)
    {
        base.OnDestroy(fsm);

        Log.Info("ActorMove Destroy!");
    }
}
