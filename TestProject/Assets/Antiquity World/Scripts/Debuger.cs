using System;
using UnityEngine;

public sealed class Debuger
{
    public static bool EnableLog = false;
    public static void Log(object message)
    {
        Debuger.Log(message, null);
    }

    public static void Log(object message, UnityEngine.Object context)
    {
        if (Debuger.EnableLog)
        {
            Debug.Log(message, context);
        }
    }

    public static void LogError(object message)
    {
        Debuger.LogError(message, null);
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        if (Debuger.EnableLog)
        {
            Debug.LogError(message, context);
        }
    }

    public static void LogError(string message, params object[] values)
    {
        if (Debuger.EnableLog)
        {
            Debug.LogErrorFormat(message,values);
        }
    }



    public static void LogWarning(object message)
    {
        Debuger.LogWarning(message, null);
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        if (Debuger.EnableLog)
        {
            Debug.LogWarning(message, context);
        }
    }

}
