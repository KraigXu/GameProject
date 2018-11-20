using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DemoGUI : MonoBehaviour 
{
    public string[] Scenes;

    private ComboBox m_comboBox;

#if UNITY_EDITOR
    private string[] ReadNames()
    {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();
    }

    private void Reset()
    {
        Scenes = ReadNames();
    }

#endif

    float m_timer = 1f;
    float m_savedFrames = 0f;
    float m_savedStartFrames = 0f;
    float m_frameCount = 0f;
    float m_fps = 0f;

    void Start()
    {
        m_savedStartFrames = Time.frameCount;
    }

    void Update()
    {
        m_frameCount = Time.frameCount - m_savedStartFrames;

        m_timer -= Time.deltaTime;
        if (m_timer <= 0f)
        {
            m_timer += 1;
            m_fps = Time.frameCount - m_savedFrames;
            m_savedFrames = Time.frameCount;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadLevel(GetLoadedLevelId());
        }
    }

    string GetLoadedLevelName()
    {
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
#else
        return Application.loadedLevelName;
#endif
    }

    int GetLoadedLevelId()
    {
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
#else
        return Application.loadedLevel;
#endif
    }

    void LoadLevel( int idx )
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player)
        {
            player.UndoDontDestroyOnLoad();
        }
        Destroy(gameObject); // avoids loading level more than once
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.LoadScene(idx);        
#else
        Application.LoadLevel(idx);
#endif
    }

    void LoadLevel(string name)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player)
        {
            player.UndoDontDestroyOnLoad();
        }
        Destroy(gameObject); // avoids loading level more than once
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
#else
        Application.LoadLevel(name);
#endif
    }

    private GUIStyle m_textStyle;
    void OnGUI()
    {

        if (m_comboBox == null)
        {
            int fontSize = 30;
            GUIStyle listStyle = new GUIStyle();
            listStyle.normal.textColor = Color.grey;
            listStyle.onHover.background =
            listStyle.hover.background = new Texture2D(2, 2);
            listStyle.padding.left =
            listStyle.padding.right =
            listStyle.padding.top =
            listStyle.padding.bottom = 4;
            listStyle.fontSize = fontSize;

            int selectedIdx = 0;
            for (int i = 0; i < Scenes.Length; ++i)
            {
                if (GetLoadedLevelName() == Scenes[i])
                {
                    selectedIdx = i;
                }
            }   

            m_comboBox = new ComboBox(new Rect(300f, 0f, 550f, 50f), selectedIdx, Scenes, listStyle);
            m_comboBox.buttonStyle.fontSize = fontSize;
            m_comboBox.boxStyle.fontSize = fontSize;

            m_textStyle = new GUIStyle("label");
            m_textStyle.fontSize = fontSize;
        }

        if( GUILayout.Button("Reset", m_comboBox.buttonStyle, GUILayout.Width(200)) )
        {            
            LoadLevel( GetLoadedLevelId() );
        }

        GUILayout.Label("FPS: " + Mathf.RoundToInt(m_fps), m_textStyle, GUILayout.Width(400));
        GUILayout.Label("FPS (Avg): " + Mathf.RoundToInt(m_frameCount / Time.timeSinceLevelLoad), m_textStyle, GUILayout.Width(400));

        string sLevel = Scenes[m_comboBox.Show()];
        if( sLevel != GetLoadedLevelName() )
        {
            LoadLevel( sLevel );
        }
    }
}
