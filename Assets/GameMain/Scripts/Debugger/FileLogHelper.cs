using GameFramework;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

internal class FileLogHelper : DefaultLogHelper
{
    //设置日志文件保存路径-你可以自定义，也可以使用系统的
    private readonly string CurrentLogPath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath, "current.log"));
    private readonly string PreviousLogPath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath, "previous.log"));
    public FileLogHelper()
    {
        Application.logMessageReceived += OnLogMessageReceived;
        try
        {
            //每次运行的时候将日志替换，就像队列一样
            if (File.Exists(PreviousLogPath))
            {
                File.Delete(PreviousLogPath);
            }
            if (File.Exists(CurrentLogPath))
            {
                File.Move(CurrentLogPath, PreviousLogPath);
            }
        }
        catch
        {
        }
    }

    private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
    {
        string log = Utility.Text.Format("[{0}][{1}] {2}{4}{3}{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logType.ToString(), logMessage ?? "<Empty Message>", stackTrace ?? "<Empty StackTrace>", Environment.NewLine);
        try
        {
            File.AppendAllText(CurrentLogPath, log, Encoding.UTF8);
        }
        catch
        {
        }
    }
}
