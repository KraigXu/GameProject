using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StrategyEditorWindow : EditorWindow {


    [MenuItem("Antiquity World/StrategyEditorWindow")]
    private static void Open()
    {
        StrategyEditor test = GetWindow<StrategyEditor>();
        test.Show();

    }


}
