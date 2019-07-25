using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class StrategyEditor : EditorWindow
{


    private float sliderValue = 1.0f;
    private float maxSliderValue = 10.0f;


    private void OnGUI()
    {
        //EditorGUILayout.BeginHorizontal();
        GUILayout.BeginArea(new Rect(0,0,200,60));

        GUILayout.BeginHorizontal();
        if (GUILayout.RepeatButton("Increase max\nSlider Value"))
        {
            maxSliderValue += 3.0f * Time.deltaTime;

        }

        GUILayout.BeginVertical();
        GUILayout.Box("Slider Value:"+Mathf.Round(sliderValue));
        sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.0f, maxSliderValue);

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

    }
    
}
