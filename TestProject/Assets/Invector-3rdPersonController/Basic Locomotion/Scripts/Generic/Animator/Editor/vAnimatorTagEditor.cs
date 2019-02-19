using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Invector.vEventSystems
{
    [CustomEditor(typeof(vAnimatorTag))]
    public class vAnimatorTagEditor : Editor
    {
        public vAnimatorTag animatorTag;
        public GUISkin skin;

        private void OnEnable()
        {
            animatorTag = target as vAnimatorTag;
        }

        public override void OnInspectorGUI()
        {
            if (!skin) skin = Resources.Load("skin") as GUISkin;
            serializedObject.Update();
            GUILayout.BeginVertical(skin.box);
            EditorStyles.helpBox.richText = true;
            EditorGUILayout.HelpBox("Useful Tags:\n " +
                "<b>CustomAction </b> - <i> Lock's position and rotation to use RootMotion instead</i> \n " +
                "<b>LockMovement </b> - <i> Use to lock all character movement </i> \n " +
                "<b>Attack </b> - <i> Use for Melee Attacks </i>"
                , MessageType.Info);
            var tags = serializedObject.FindProperty("tags");
            if (GUILayout.Button("Add Tag", skin.button, GUILayout.ExpandWidth(true)))
            {
                tags.arraySize++;
                tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = "New Tag";
            }
            for (int i = 0; i < tags.arraySize; i++)
            {
                if (!DrawTag(tags, i)) break;
            }
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        public bool DrawTag(SerializedProperty list, int index)
        {
            GUILayout.BeginHorizontal(skin.box);
            GUILayout.BeginVertical();
            var tagName = list.GetArrayElementAtIndex(index);

            EditorGUILayout.PropertyField(tagName, GUIContent.none, GUILayout.Height(15));
            GUILayout.Space(-10);
            GUILayout.EndVertical();
            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                list.DeleteArrayElementAtIndex(index);
                return false;
            }

            GUILayout.EndHorizontal();
            return true;
        }
    }
}