using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;
using CreativeSpore;

public class DemoPathFindingBehavior : MapPathFindingBehaviour 
{
    GUIContent[] comboBoxList;
    private ComboBox comboBoxControl;// = new ComboBox();
    private GUIStyle listStyle = new GUIStyle();

    private PlayerController m_player;
    private Camera2DController m_camera2D;

    new void Start()
    {
        base.Start();

        m_lastComputeTime = Time.realtimeSinceStartup;
        m_player = FindObjectOfType<PlayerController>();
        m_camera2D = FindObjectOfType<Camera2DController>();
        OnComputedPath += PathComputed;

        AutoTileMap.Instance.AutoTileMapGui.enabled = false;

        string[] toolBarNames = System.Enum.GetNames(typeof(MapPathFinding.eHeuristicType));
        comboBoxList = new GUIContent[toolBarNames.Length];
        for (int i = 0; i < toolBarNames.Length; ++i)
        {
            comboBoxList[i] = new GUIContent("Heuristic: "+toolBarNames[i]);
        }

        listStyle.normal.textColor = Color.white;
        listStyle.onHover.background =
        listStyle.hover.background = new Texture2D(2, 2);
        listStyle.padding.left =
        listStyle.padding.right =
        listStyle.padding.top =
        listStyle.padding.bottom = 4;

        comboBoxControl = new ComboBox(new Rect(550, 0, 150, 20), comboBoxList[0], comboBoxList, "button", "box", listStyle);
        comboBoxControl.SelectedItemIndex = (int)m_mapPathFinding.HeuristicType;
    }

    private float m_lastComputeTime;
    private int m_computeTime = 0;
    private void PathComputed(MapPathFindingBehaviour source)
    {
        m_computeTime = Mathf.RoundToInt((Time.realtimeSinceStartup - m_lastComputeTime) * 1000);
        m_lastComputeTime = Time.realtimeSinceStartup;
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        TargetPos = m_player.transform.position;

        if( !AutoTileMap.Instance.AutoTileMapGui.enabled )
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
            {
                if (m_camera2D.Zoom > 1f)
                    m_camera2D.Zoom = Mathf.Max(m_camera2D.Zoom - 1, 1);
                else
                    m_camera2D.Zoom = Mathf.Max(m_camera2D.Zoom / 2f, 0.05f);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
            {
                if (m_camera2D.Zoom >= 1f)
                    m_camera2D.Zoom = Mathf.Min(m_camera2D.Zoom + 1, 10);
                else
                    m_camera2D.Zoom *= 2f;
            }
        }
    }

    private void OnGUI()
    {
        if (AutoTileMap.Instance.AutoTileMapGui.enabled)
        {
            if( GUI.Button(new Rect(300, 0, 200, 20), "Enable Debug PathFinding"))
            {
                AutoTileMap.Instance.AutoTileMapGui.enabled = false;
            }
        }
        else
        {
            if (GUI.Button(new Rect(300, 0, 200, 20), "Enable Edit Map"))
            {
                AutoTileMap.Instance.AutoTileMapGui.enabled = true;
            }

            GUI.TextArea(new Rect(300, 25, 400, 50), "Enable Gizmos to see the Path found to reach the player\nCompute Time(ms): "+m_computeTime);

            m_mapPathFinding.HeuristicType = (MapPathFinding.eHeuristicType)comboBoxControl.Show();
        }               
    }
}
