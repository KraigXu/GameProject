using UnityEngine;
using System.Text;
using UnityEditor;

public class ShowFps : MonoBehaviour
{

    public bool m_isShow = true;

    private int m_FrameCounter;
    private float m_ClientTimeAccumulator;
    private float m_RenderTimeAccumulator;
    private float m_MaxTimeAccumulator;
    private float m_ClientFrameTime;
    private float m_RenderFrameTime;
    private float m_MaxFrameTime;
    private GUIStyle s_SectionHeaderStyle;
    private GUIStyle s_LabelStyle;
    [Range(0, 1)]
    public float position = 0.5f;
    void Start()
    {
    }



    void OnGUI()
    {
        if (m_isShow)
        {
            GameViewStatsGUI();
        }

    }

    private GUIStyle sectionHeaderStyle
    {
        get
        {
            if (s_SectionHeaderStyle == null)
            {
                s_SectionHeaderStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("BoldLabel");
            }
            return s_SectionHeaderStyle;
        }
    }
    private GUIStyle labelStyle
    {
        get
        {
            if (s_LabelStyle == null)
            {
                s_LabelStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label);
                s_LabelStyle.richText = true;
            }
            return s_LabelStyle;
        }
    }
    private string FormatNumber(int num)
    {
        if (num < 1000)
        {
            return num.ToString();
        }
        if (num < 1000000)
        {
            return ((double)num * 0.001).ToString("f1") + "k";
        }
        return ((double)num * 1E-06).ToString("f1") + "M";
    }
    public void UpdateFrameTime()
    {
        float frameTime = UnityStats.frameTime;
        float renderTime = UnityStats.renderTime;
        m_ClientTimeAccumulator += frameTime;
        m_RenderTimeAccumulator += renderTime;
        m_MaxTimeAccumulator += Mathf.Max(frameTime, renderTime);
        m_FrameCounter++;
        bool flag = m_ClientFrameTime == 0f && m_RenderFrameTime == 0f;
        bool flag2 = m_FrameCounter > 30 || m_ClientTimeAccumulator > 0.3f || m_RenderTimeAccumulator > 0.3f;
        if (flag || flag2)
        {
            m_ClientFrameTime = m_ClientTimeAccumulator / (float)m_FrameCounter;
            m_RenderFrameTime = m_RenderTimeAccumulator / (float)m_FrameCounter;
            m_MaxFrameTime = m_MaxTimeAccumulator / (float)m_FrameCounter;
        }
        if (flag2)
        {
            m_ClientTimeAccumulator = 0f;
            m_RenderTimeAccumulator = 0f;
            m_MaxTimeAccumulator = 0f;
            m_FrameCounter = 0;
        }
    }
    private static string FormatDb(float vol)
    {
        if (vol == 0f)
        {
            return "-∞ dB";
        }
        return string.Format("{0:F1} dB", 20f * Mathf.Log10(vol));
    }
    public void GameViewStatsGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        GUI.color = new Color(1f, 1f, 1f, 0.75f);
        float num = 300f;
        float num2 = 204f;
        int num3 =0;
        if (num3 != 0)
        {
            num2 += 220f;
        }
        GUILayout.BeginArea(new Rect(Screen.width * position - num - 10f, 27f, num, num2), "Statistics", GUI.skin.window);
        GUILayout.Label("Audio:", sectionHeaderStyle, new GUILayoutOption[0]);
        StringBuilder stringBuilder = new StringBuilder(400);
        float audioLevel = UnityStats.audioLevel;
        stringBuilder.Append("  Level: " + FormatDb(audioLevel) + ((!EditorUtility.audioMasterMute) ? "\n" : " (MUTED)\n"));
        stringBuilder.Append(string.Format("  Clipping: {0:F1}%", 100f * UnityStats.audioClippingAmount));
        GUILayout.Label(stringBuilder.ToString(), new GUILayoutOption[0]);
        GUI.Label(new Rect(170f, 40f, 120f, 20f), string.Format("DSP load: {0:F1}%", 100f * UnityStats.audioDSPLoad));
        GUI.Label(new Rect(170f, 53f, 120f, 20f), string.Format("Stream load: {0:F1}%", 100f * UnityStats.audioStreamLoad));
        GUILayout.Label("Graphics:", sectionHeaderStyle, new GUILayoutOption[0]);
        UpdateFrameTime();
        string text = string.Format("{0:F1} FPS ({1:F1}ms)", 1f / Mathf.Max(m_MaxFrameTime, 1E-05f), m_MaxFrameTime * 1000f);
        GUI.Label(new Rect(170f, 75f, 120f, 20f), text);
        int screenBytes = UnityStats.screenBytes;
        int num4 = UnityStats.dynamicBatchedDrawCalls - UnityStats.dynamicBatches;
        int num5 = UnityStats.staticBatchedDrawCalls - UnityStats.staticBatches;
        StringBuilder stringBuilder2 = new StringBuilder(400);
        if (m_ClientFrameTime > m_RenderFrameTime)
        {
            stringBuilder2.Append(string.Format("  CPU: main <b>{0:F1}</b>ms  render thread {1:F1}ms\n", m_ClientFrameTime * 1000f, m_RenderFrameTime * 1000f));
        }
        else
        {
            stringBuilder2.Append(string.Format("  CPU: main {0:F1}ms  render thread <b>{1:F1}</b>ms\n", m_ClientFrameTime * 1000f, m_RenderFrameTime * 1000f));
        }
        stringBuilder2.Append(string.Format("  Batches: <b>{0}</b> \tSaved by batching: {1}\n", UnityStats.batches, num4 + num5));
        stringBuilder2.Append(string.Format("  Tris: {0} \tVerts: {1} \n", FormatNumber(UnityStats.triangles), FormatNumber(UnityStats.vertices)));
        stringBuilder2.Append(string.Format("  Screen: {0} - {1}\n", UnityStats.screenRes, EditorUtility.FormatBytes(screenBytes)));
        stringBuilder2.Append(string.Format("  SetPass calls: {0} \tShadow casters: {1} \n", UnityStats.setPassCalls, UnityStats.shadowCasters));
        stringBuilder2.Append(string.Format("  Visible skinned meshes: {0}  s: {1}", UnityStats.visibleSkinnedMeshes, UnityStats.visibleAnimations));
        GUILayout.Label(stringBuilder2.ToString(), labelStyle, new GUILayoutOption[0]);
        if (num3 != 0)
        {
            GUILayout.Label("Network:", sectionHeaderStyle, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            //for (int i = 0; i < num3; i++)
            //{
            //    GUILayout.Label(UnityStats.GetNetworkStats(i), new GUILayoutOption[0]);
            //}
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("Network: (no players connected)", sectionHeaderStyle, new GUILayoutOption[0]);
        }
        GUILayout.EndArea();
    }
}
