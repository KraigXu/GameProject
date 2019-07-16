using System;
using UnityEngine;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

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

    public static void TestLog(object message)
    {
        Debug.Log(message);
    }

    public static bool mainsceneIsflag = false;
    public static bool correlationIsflag = false;
    public static bool assListSceneIsflag = false;
    public static bool inputIsflag = false;

    public static string inputValue;

    public static void OnDebugWindow(int id)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("MainScene"))
        {
            mainsceneIsflag = !mainsceneIsflag;
            if (mainsceneIsflag == true)
            {
                correlationIsflag = false;
                assListSceneIsflag = false;
                inputIsflag = false;
            }
        }

        if (GUILayout.Button("CorrelationComScene"))
        {
            correlationIsflag = !correlationIsflag;
            if (correlationIsflag == true)
            {
                mainsceneIsflag = false;
                assListSceneIsflag = false;
                inputIsflag = false;
            }
        }

        if (GUILayout.Button("Input"))
        {
            inputIsflag = !inputIsflag;
            if (inputIsflag)
            {
                mainsceneIsflag = false;
                correlationIsflag = false;
                assListSceneIsflag = false;
            }

        }
        if (GUILayout.Button("AssetListScene"))
        {
            assListSceneIsflag = !assListSceneIsflag;
            if (assListSceneIsflag == true)
            {
                mainsceneIsflag = false;
                correlationIsflag = false;
                inputIsflag = false;
            }
        }

        GUILayout.EndHorizontal();


        //----mainscene

        if (mainsceneIsflag == true)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Select Button");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Button("Enter");
            GUILayout.Button("Exit");
            GUILayout.EndHorizontal();
        }

        if (correlationIsflag == true)
        {
            GUILayout.BeginScrollView(Vector2.zero, GUIStyle.none);

            GUILayout.Button("Test");
            GUILayout.Button("Test12");

            GUILayout.EndScrollView();
        }

        if (inputIsflag)
        {

            GUILayout.Label("string type not '' ");

            inputValue = GUILayout.TextField(inputValue);
            if (GUILayout.Button("Confirm"))
            {

                string classname = inputValue.Substring(0, inputValue.IndexOf('.'));
                Debug.Log(classname);

                string mainname = inputValue.Substring(inputValue.IndexOf('.')+1, inputValue.IndexOf('(')- inputValue.IndexOf('.')-1);
                Debug.Log(mainname);

                string values = inputValue.Substring(inputValue.IndexOf('(')+1,inputValue.IndexOf(')')-inputValue.IndexOf('(')-1);
                Debug.Log(values);

                //获取类型信息
                Type t = Type.GetType(classname);
                //构造器的参数
                object[] constuctParms = values.Split(',');
                //根据类型创建对象
                object dObj = Activator.CreateInstance(t);
                //获取方法的信息
                MethodInfo method = t.GetMethod(mainname);
                //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
                BindingFlags flag = BindingFlags.Public | BindingFlags.Static;

                //调用方法，用一个object接收返回值
                method.Invoke(dObj, flag, Type.DefaultBinder, constuctParms, null);
            }
        }


        GUILayout.EndVertical();


    }

}
