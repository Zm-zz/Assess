using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : GameEventArgs
{
    public static readonly int EventId = typeof(TestEvent).GetHashCode();

    /// <summary>
    /// 初始化加载全局配置成功事件编号的新实例。
    /// </summary>
    public TestEvent()
    {
        ConfigAssetName = null;
        Duration = 0;
        UserData = null;
    }

    public TestEvent(string configAssetName, float duration, object userData)
    {
        ConfigAssetName = configAssetName;
        Duration = duration;
        UserData = userData;
    }

    public override int Id
    {
        get { return EventId; }
    }

    /// <summary>
    /// 获取全局配置资源名称。
    /// </summary>
    public string ConfigAssetName
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取加载持续时间。
    /// </summary>
    public float Duration
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData
    {
        get;
        private set;
    }

    /// <summary>
    /// 创建加载全局配置成功事件。
    /// </summary>
    /// <param name="e">内部事件。</param>
    /// <returns>创建的加载全局配置成功事件。</returns>
    public static TestEvent Create(TestEvent e)
    {
        TestEvent testEvent = ReferencePool.Acquire<TestEvent>();
        testEvent.ConfigAssetName = e.ConfigAssetName;
        testEvent.Duration = e.Duration;
        testEvent.UserData = e.UserData;
        return testEvent;
    }

    public override void Clear()
    {
        ConfigAssetName = null;
        Duration = 0f;
        UserData = null;
    }
}
