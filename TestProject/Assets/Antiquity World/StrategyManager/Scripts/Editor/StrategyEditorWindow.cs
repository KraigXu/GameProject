using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StrategyEditorWindow : EditorWindow {


    [MenuItem("Window/TestWindow")]
    private static void Open()
    {
        TestWindow test = GetWindow<TestWindow>();
        test.Show();

    }


    private void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("This is a test Window");
        


        EditorGUILayout.EndHorizontal();

    }

	
}
