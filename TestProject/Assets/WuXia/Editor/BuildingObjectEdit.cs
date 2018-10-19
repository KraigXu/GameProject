using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;
using LivingArea;
public class BuildingObjectEdit : EditorWindowBase
{
    private static Vector2 minResolution = new Vector2(1200, 1000);
    private static Rect leftUpRect = new Rect(new Vector2(0, 0), minResolution);

    private List<BuildingObject>_buildings=new List<BuildingObject>();
    private string json;

    private static  GUIStyle titlestyle = new GUIStyle();
    private static  GUIStyle inputstyle=new GUIStyle();

    public static void Popup(Vector3 position)
    {
        titlestyle.fixedHeight = 20;
        titlestyle.fixedWidth = 110f;
        inputstyle.fixedHeight = 20;
        inputstyle.fixedWidth = 110f;


        // RepeateWindow window = new RepeateWindow();
        BuildingObjectEdit window = GetWindowWithRectPrivate(typeof(BuildingObjectEdit), leftUpRect, true, "BuildingObject") as BuildingObjectEdit;
        window.minSize = minResolution;
        //要在设置位置之前，先把窗体注册到管理器中，以便更新窗体的优先级
        EditorWindowMgr.AddRepeateWindow(window);
        //刷新界面偏移量
        int offset = (window.Priority - 10) * 30;
        window.position = new Rect(new Vector2(position.x + offset, position.y + offset), new Vector2(800, 400));
        window.Show();
        //手动聚焦
        window.Focus();


    }

    /// <summary>
    /// 重写EditorWindow父类的创建窗口函数
    /// </summary>
    /// <param name="t"></param>
    /// <param name="rect"></param>
    /// <param name="utility"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    private static EditorWindow GetWindowWithRectPrivate(Type t, Rect rect, bool utility, string title)
    {
        //UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(t);
        EditorWindow editorWindow = null;/*= (array.Length <= 0) ? null : ((EditorWindow)array[0]);*/
        if (!(bool)editorWindow)
        {
            editorWindow = (ScriptableObject.CreateInstance(t) as EditorWindow);
            editorWindow.minSize = new Vector2(rect.width, rect.height);
            editorWindow.maxSize = new Vector2(rect.width, rect.height);
            editorWindow.position = rect;
            if (title != null)
            {
                editorWindow.titleContent = new GUIContent(title);
            }
            if (utility)
            {
                editorWindow.ShowUtility();
            }
            else
            {
                editorWindow.Show();
            }
        }
        else
        {
            editorWindow.Focus();
        }
        return editorWindow;
    }

    //    [{
    //    "Name": "市集",
    //    "Description": "无说明",
    //    "TypeId": 3,
    //    "BuildingLevel": 3,
    //    "DurableValue": 1000,
    //    "DurableMax": 5000,
    //    "BuildingStatus": 1,
    //    "HaveId": 1,
    //    "BuildingFeaturesIds": "1"
    //}, {
    //    "Name": "酒馆",
    //    "Description": "无说明",
    //    "TypeId": 1,
    //    "BuildingLevel": 1,
    //    "DurableValue": 110,
    //    "DurableMax": 5000,
    //    "BuildingStatus": 2,
    //    "HaveId": 1,
    //    "BuildingFeaturesIds": "3"
    //}]

    private void OnGUI()
    {
        GUILayout.Space(12);
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("数据",GUILayout.Width(200));
        GUILayout.EndVertical();

        
        for (int i = 0; i < _buildings.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("名称", titlestyle);
            GUILayout.Label("说明", titlestyle);
           // GUILayout.Label("类型", titlestyle);
            GUILayout.Label("等级", titlestyle);
            GUILayout.Label("耐久", titlestyle);
            GUILayout.Label("最大耐久", titlestyle);
            GUILayout.Label("建筑状态", titlestyle);
            GUILayout.Label("所属Id", titlestyle);
            GUILayout.Label("功能Id", titlestyle);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _buildings[i].Name = GUILayout.TextField(_buildings[i].Name, 50, inputstyle);
            _buildings[i].Description = GUILayout.TextField(_buildings[i].Description, 50, inputstyle);
            //_buildings[i].TypeId = Int16.Parse(GUILayout.TextField(_buildings[i].TypeId.ToString(), 50, inputstyle));
            _buildings[i].BuildingLevel = Int16.Parse(GUILayout.TextField(_buildings[i].BuildingLevel.ToString(), 50, inputstyle));
            _buildings[i].DurableValue = Int16.Parse(GUILayout.TextField(_buildings[i].DurableValue.ToString(), 50, inputstyle));
            _buildings[i].DurableMax = Int16.Parse(GUILayout.TextField(_buildings[i].DurableMax.ToString(), 50, inputstyle));
            _buildings[i].BuildingStatus = Int16.Parse(GUILayout.TextField(_buildings[i].BuildingStatus.ToString(), 50, inputstyle));
            _buildings[i].HaveId = Int16.Parse(GUILayout.TextField(_buildings[i].HaveId.ToString(), 50, inputstyle));
            _buildings[i].BuildingFeaturesIds = GUILayout.TextField(_buildings[i].BuildingFeaturesIds, 50, inputstyle);
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
 
         if(GUILayout.Button("新增一条",GUILayout.Width(100)))
        {
             BuildingObject b=new BuildingObject();
            b.Name = "";
            b.Description = "";
          //  b.TypeId = 0;
            b.BuildingLevel = 0;
            b.DurableValue = 0;
            b.DurableMax = 0;
            b.BuildingStatus = 0;
            b.HaveId = 0;
            b.BuildingFeaturesIds = "";
            _buildings.Add(b);
        }
        

        if (GUILayout.Button("确认(转换JSON)",GUILayout.Width(300)))
        {
            json = JsonMapper.ToJson(_buildings);
            Debug.Log(json); 
        }

         GUILayout.EndHorizontal();
    }

    private void OnDestroy()
    {
        //销毁窗体的时候，从管理器中移除该窗体的缓存，并且重新刷新焦点
        EditorWindowMgr.RemoveRepeateWindow(this);
        EditorWindowMgr.FoucusWindow();
    }

    private void OnFocus()
    {
        EditorWindowMgr.FoucusWindow();
    }
}
