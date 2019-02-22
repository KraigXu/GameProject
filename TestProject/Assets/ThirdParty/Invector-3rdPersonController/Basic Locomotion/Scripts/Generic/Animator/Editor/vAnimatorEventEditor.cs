using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Invector.vEventSystems
{
    [CustomEditor(typeof(vAnimatorEvent))]
    public class vAnimatorEventEditor : Editor
    {
        public vAnimatorEvent animatorEvent;
        public GUISkin skin;
        public GUIStyle timeLineStyle;

        private void OnEnable()
        {
            animatorEvent = target as vAnimatorEvent;
        }

        public override void OnInspectorGUI()
        {
            if (!skin) skin = Resources.Load("skin") as GUISkin;

            if (timeLineStyle == null) timeLineStyle = new GUIStyle(EditorStyles.inspectorFullWidthMargins);
            serializedObject.Update();
            EditorGUILayout.BeginVertical(skin.box);
            EditorGUILayout.HelpBox("<b>Make sure to use a lower value than the Exit Time of this State.</b>", MessageType.Warning);
            var events = serializedObject.FindProperty("eventTriggers");
            if (GUILayout.Button("Add Event", skin.button, GUILayout.ExpandWidth(true)))
            {
                events.arraySize++;
                events.GetArrayElementAtIndex(events.arraySize - 1).FindPropertyRelative("eventName").stringValue = "New Event";
            }
            for (int i = 0; i < events.arraySize; i++)
            {
                if (!DraEvent(events, i)) break;
            }
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        public bool DraEvent(SerializedProperty list, int index)
        {
            EditorGUILayout.BeginHorizontal(skin.box, GUILayout.ExpandWidth(true));
            {
                var eventName = list.GetArrayElementAtIndex(index).FindPropertyRelative("eventName");
                var normalizedTime = list.GetArrayElementAtIndex(index).FindPropertyRelative("normalizedTime");
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                {
                    eventName.stringValue = EditorGUILayout.TextField(GUIContent.none, eventName.stringValue, GUILayout.ExpandWidth(true));
                    Rect rect = GUILayoutUtility.GetLastRect();
                    var color = GUI.color;

                    rect.y += 20;
                    GUI.Box(rect, "", skin.GetStyle("AnimatorEventBar"));
                    int number = 0;
                    for (int i = 0; i < 101; i++)
                    {
                        timeLineStyle.alignment = TextAnchor.UpperCenter;
                        var rectAdjust = new Rect(rect.position.x + ((rect.width - 5) * (0.01f * i)), rect.position.y, 2.5f, 15);
                        var par = i % 10f;
                        timeLineStyle.fontSize = par == 0 ? 15 : par == 5 ? (rect.width * 0.01f > 3f) ? 10 : 5 : (rect.width * 0.01f > 4f) ? 5 : 1;
                        rectAdjust.y += par == 0 ? -2 : par == 5 ? (rect.width * 0.01f > 3f) ? -1 : 0 : 0;
                        timeLineStyle.normal.textColor = Color.grey;
                        GUI.Box(rectAdjust, "|", timeLineStyle);
                        rectAdjust.y = rect.position.y + 5;

                        if (par == 0)
                        {
                            timeLineStyle.normal.textColor = Color.black;
                            timeLineStyle.alignment = TextAnchor.MiddleLeft;
                            timeLineStyle.fontSize = Mathf.Clamp((int)(rect.width * 0.02f), 6, 10);
                            rectAdjust.width = 25;
                            rectAdjust.height = 15;
                            rectAdjust.x += number < 10 ? -1 : -7;
                            GUI.Label(rectAdjust, (number * 0.1f).ToString(), timeLineStyle);
                            number++;
                        }
                    }
                    GUI.color = color;
                    rect.x -= 5;
                    rect.width += 10;
                    normalizedTime.floatValue = GUI.HorizontalSlider(rect, normalizedTime.floatValue, 0f, 1f, GUIStyle.none, skin.GetStyle("AnimatorEventThumb"));
                }
                EditorGUILayout.EndVertical();
                var width = 30;
                EditorGUILayout.BeginVertical(GUILayout.Width(width));
                {
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(width)))
                    {
                        list.DeleteArrayElementAtIndex(index);

                        return false;
                    }
                    normalizedTime.floatValue = EditorGUILayout.FloatField((float)System.Math.Round(normalizedTime.floatValue, 2), GUILayout.Width(width));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            return true;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }
    }
}