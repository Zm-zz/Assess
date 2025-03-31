using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : GameEventArgs
{
    public static readonly int EventId = typeof(TestEvent).GetHashCode();

    /// <summary>
    /// ��ʼ������ȫ�����óɹ��¼���ŵ���ʵ����
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
    /// ��ȡȫ��������Դ���ơ�
    /// </summary>
    public string ConfigAssetName
    {
        get;
        private set;
    }

    /// <summary>
    /// ��ȡ���س���ʱ�䡣
    /// </summary>
    public float Duration
    {
        get;
        private set;
    }

    /// <summary>
    /// ��ȡ�û��Զ������ݡ�
    /// </summary>
    public object UserData
    {
        get;
        private set;
    }

    /// <summary>
    /// ��������ȫ�����óɹ��¼���
    /// </summary>
    /// <param name="e">�ڲ��¼���</param>
    /// <returns>�����ļ���ȫ�����óɹ��¼���</returns>
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
