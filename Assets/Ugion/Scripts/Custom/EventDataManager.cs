using GameFramework;
using GameFramework.Event;
using Launch;
using UnityEngine;

public class EventDataManager : MonoBehaviour
{
    public void Initialize()
    {
        Main.Event.Subscribe(Event_ChangeState.EventId, OnEvent_ChangeStateFire);
    }

    private void OnEvent_ChangeStateFire(object sender, GameEventArgs e)
    {
        Event_ChangeState ne = (Event_ChangeState)e;

        Debug.Log($"<size=13><color=orange>ChangeProcedure：</color></size>{ne.ProcedureTitle}");
    }

    public void UnInitialize()
    {
        Main.Event.Unsubscribe(Event_ChangeState.EventId, OnEvent_ChangeStateFire);
    }
}

public class Event_ChangeState : GameEventArgs
{
    public static readonly int EventId = typeof(TestEvent).GetHashCode();

    /// <summary>
    /// 初始化加载全局配置成功事件编号的新实例。
    /// </summary>
    public Event_ChangeState()
    {
    }

    public Event_ChangeState(ProcedureConfig config)
    {
        //ProcedureName = config.procedureName;
        ProcedureScript = config.procedureScript;
        ProcedureTitle = config.procedureTitle;
    }

    public override int Id
    {
        get { return EventId; }
    }

    public string ProcedureTitle
    {
        get;
        private set;
    }

    public UnityEngine.Object ProcedureScript
    {
        get;
        private set;
    }

    /// <summary>
    /// 创建加载全局配置成功事件。
    /// </summary>
    /// <param name="e">内部事件。</param>
    /// <returns>创建的加载全局配置成功事件。</returns>
    public static Event_ChangeState Create(Event_ChangeState e)
    {
        Event_ChangeState changeStateEvent = ReferencePool.Acquire<Event_ChangeState>();
        changeStateEvent.ProcedureTitle = e.ProcedureTitle;
        return changeStateEvent;
    }

    public override void Clear()
    {

    }
}