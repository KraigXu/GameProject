using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogObject
{
    public string output = "";
    public string stack = "";
    public LogType logType;
}

public class DebugOutside : MonoBehaviour
{
    public bool IsOpenConsole = false;
    public int LogFontSize = 18;
    public bool IsShowLogUI = false;
    public List<LogObject> lo = new List<LogObject>();
    public Vector2 scrollPosition = new Vector2(0, 1);
    public Rect m_logWinRect = new Rect(10, 10, 600f, 800f);
    public Rect m_inputRect = new Rect(15, 750, 560f, 30);
    public Rect m_minBtnRect = new Rect(0, 0, 150, 50);
    public bool m_showLog = true;
    public bool m_showWarning = false;
    public bool m_showError = true;
    public string m_inputValue = "";

    void Start()
    {

    }

    void Update()
    {
        if (IsOpenConsole)
        {
            if (Input.GetKeyUp(KeyCode.BackQuote))
            {
                IsShowLogUI = !IsShowLogUI;
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                Debug.Log(">>s");
                RunMain(m_inputValue);
                m_inputValue = "";
            }

        }
    }

    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }

    void HandleLog(string logString, string stackTrace, LogType ElementType)
    {
        LogObject o = new LogObject();
        o.output = logString;
        o.stack = stackTrace;
        o.logType = ElementType;
        if (lo.Count > 200)
        {
            lo.RemoveAt(0);
        }
        lo.Add(o);
    }
    void OnGUI()
    {
        if (IsShowLogUI == false)
            return;

        m_logWinRect = GUI.Window(0, m_logWinRect, LogWin, "ConsoleView");
        GUI.skin.label.fontSize = LogFontSize;
    }

    public void ClearLog()
    {
        lo.Clear();
    }
    void LogWin(int id)
    {
        GUILayout.BeginScrollView(m_logWinRect.position, false, true, GUILayout.Width(m_logWinRect.width), GUILayout.Height(m_logWinRect.height));
        foreach (LogObject item in lo)
        {
            if (item.logType == LogType.Log && !m_showLog)
            {
                continue;
            }
            if (item.logType == LogType.Warning && !m_showWarning)
            {
                continue;
            }
            if (item.logType == LogType.Error && !m_showError)
            {
                continue;
            }

            switch (item.logType)
            {
                case LogType.Log:
                    GUI.color = Color.white;
                    break;
                case LogType.Warning:
                    GUI.color = Color.yellow;
                    break;
                case LogType.Error:
                    GUI.color = Color.red;
                    break;
                default:
                    GUI.color = Color.red;
                    break;
            }

            GUILayout.Label(item.output + "\n" + item.stack);
            GUILayout.Space(-25);
            GUI.color = Color.gray;
            GUILayout.Label("---------------------------------------------------------------------------");
            GUILayout.Space(-10);
        }
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        GUI.color = Color.white;
        if (GUILayout.Button("Close"))
        {
            IsShowLogUI = !IsShowLogUI;
        }
        if (GUILayout.Button("Clear"))
        {
            lo.Clear();
        }
        m_showLog = GUILayout.Toggle(m_showLog, "Log");
        m_showWarning = GUILayout.Toggle(m_showWarning, "Warning");
        m_showError = GUILayout.Toggle(m_showError, "Error");
        GUILayout.Space(-20);

        m_inputValue = GUI.TextField(m_inputRect, m_inputValue, 50);

        GUILayout.EndHorizontal();


    }

    private void RunMain(string value)
    {
        string[] str = value.Split('.');
        if (str.Length >= 2)
        {
            if (str.Length <= 2)
            {
                GameObject.Find(str[0]).SendMessage(str[1]);
            }
            else if (str.Length > 2)
            {
                GameObject.Find(str[0]).SendMessage(str[1], str[2]);
            }

        }

    }
}
