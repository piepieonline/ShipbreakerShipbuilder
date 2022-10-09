using UnityEngine;
using System.Collections;
using System.IO;
using System;

// Mute known extraneous error messages
public class LogHandler : ILogHandler
{
    private static ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;

    public static void Setup()
    {
        m_DefaultLogHandler = Debug.unityLogger.logHandler;
        Debug.unityLogger.logHandler = new LogHandler();
    }

    public static void Teardown()
    {
        Debug.unityLogger.logHandler = m_DefaultLogHandler;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if(ShouldLogMessage(String.Format(format, args)))
            m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if(ShouldLogMessage(exception.Message))
            m_DefaultLogHandler.LogException(exception, context);
    }

    bool ShouldLogMessage(string message)
    {
        if(message.Contains("Could not Move to directory Library/com.unity.addressables/aa/Windows, directory arlready exists.")) return false;
        if(message.Contains("Burst error BC1054: Unable to resolve type `Unity.Entities.TransformStash. Reason: Unknown.`")) return false;

        return true;
    }
}