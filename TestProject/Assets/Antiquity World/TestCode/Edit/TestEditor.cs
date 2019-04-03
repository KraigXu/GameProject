using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestE))]
public class TestEditor:Editor
{

    private TestE _test;

    private void OnEnable()
    {
        _test=target as TestE;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        //GUILayout.Label(_test.Value);
        EditorGUILayout.EndHorizontal();

    }

    public void OnSceneGUI()
    {
        Handles.Label(_test.transform.position,"这是目标");
    }
}
