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

    // Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
    public static void LogError(object message)
    {
        Debuger.LogError(message, null);
    }

    // Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
    public static void LogError(object message, UnityEngine.Object context)
    {
        if (Debuger.EnableLog)
        {
            Debug.LogError(message, context);
        }
    }

    // Token: 0x06000005 RID: 5 RVA: 0x000020B0 File Offset: 0x000002B0
    public static void LogWarning(object message)
    {
        Debuger.LogWarning(message, null);
    }

    // Token: 0x06000006 RID: 6 RVA: 0x000020BC File Offset: 0x000002BC
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        if (Debuger.EnableLog)
        {
            Debug.LogWarning(message, context);
        }
    }

}
