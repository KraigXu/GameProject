using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

namespace CreativeSpore.RpgMapEditor
{
    [CustomEditor(typeof(DirectionalAnimation))]
    public class DirectionalAnimationEditor : Editor
    {
        float m_repaintTimeStamp;
        DirectionalAnimation m_target;
        ReorderableList m_animReordList;
        void OnEnable()
        {
            m_repaintTimeStamp = Time.realtimeSinceStartup;
            m_target = (DirectionalAnimation)target;
            if (m_target.DirectionalAnimController)
            {
                m_animReordList = CreateAnimReorderableList();
            }
        }

        public override void OnInspectorGUI()
        {
            Event e = Event.current;
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_dirAnimCtrl"));
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (m_target.DirectionalAnimController)
                {
                    m_animReordList = CreateAnimReorderableList();
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetSpriteRenderer"));
            if (m_target.DirectionalAnimController)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_playMode"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fps"), new GUIContent("Animation Speed", "Frames per second."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_dir"), new GUIContent("Direction"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_isPlaying"));
                
                m_animReordList.DoLayoutList();                
            }

            if (e.type == EventType.Repaint)
            {
                float timeDt = Time.realtimeSinceStartup - m_repaintTimeStamp;
                m_repaintTimeStamp = Time.realtimeSinceStartup;
                if (!Application.isPlaying)
                {
                    m_target.UpdateAnim(timeDt);
                }
            }

            Repaint();
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private ReorderableList CreateAnimReorderableList()
        {
            ReorderableList reordList = new ReorderableList(m_target.DirectionalAnimController.GetAnimList(), typeof(DirectionalAnimData), true, true, true, true);
            float elementHeight = m_target.DirectionalAnimController.DirectionsPerAnim <= 4 ? 64f : 128f;
            reordList.displayAdd = reordList.displayRemove = false;
            reordList.elementHeight = 0;
            reordList.draggable = reordList.elementHeight > 0f;
            reordList.drawHeaderCallback += (Rect rect) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Animations", EditorStyles.boldLabel);
                Texture2D btnTexture = reordList.elementHeight == 0f ? EditorGUIUtility.FindTexture("winbtn_win_max_h") : EditorGUIUtility.FindTexture("winbtn_win_min_h");                
                if (GUI.Button(new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), btnTexture, EditorStyles.label))
                {
                    reordList.elementHeight = reordList.elementHeight == 0f ? elementHeight : 0f;
                    reordList.draggable = reordList.elementHeight > 0f;
                }
            };
            reordList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (reordList.elementHeight == 0f) return;
                DirectionalAnimData anim = (DirectionalAnimData)reordList.list[index];
                anim.name = GUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), anim.name);

                int dirNb = m_target.DirectionalAnimController.DirectionsPerAnim;
                float frameWidth = rect.width / Mathf.Min(4, dirNb);
                int rowNb = 1 + (dirNb - 1) / 4;
                float frameHeight = (rect.height - EditorGUIUtility.singleLineHeight) / rowNb;
                for (int i = 0; i < dirNb; ++i)
                {
                    Vector2 dir = Quaternion.Euler(Vector3.forward * (((float)i / dirNb) * 360f)) * Vector3.down;
                    Sprite sprite = m_target.DirectionalAnimController.GetPreviewAnimSprite(dir, index);
                    if (sprite)
                    {
                        float aspectRatio = sprite.rect.width / sprite.rect.height;
                        Rect uv = new Rect() { position = Vector2.Scale(sprite.rect.position, sprite.texture.texelSize), size = Vector2.Scale(sprite.rect.size, sprite.texture.texelSize) };
                        Rect rAnim = new Rect(rect.x + frameWidth * (i % 4), rect.y + EditorGUIUtility.singleLineHeight + (i / 4) * frameHeight, frameHeight * aspectRatio, frameHeight);
                        GUI.DrawTextureWithTexCoords(rAnim, sprite.texture, uv);
                    }
                    else
                    {
                        Rect rBox = new Rect(rect.x + frameWidth * i, rect.y + EditorGUIUtility.singleLineHeight, frameWidth, rect.height - EditorGUIUtility.singleLineHeight);
                        GUI.Box(rBox, "");
                    }
                }
            };
            reordList.onSelectCallback += (ReorderableList list) =>
            {
                m_target.AnimIndex = m_animReordList.index;
                GUI.changed = true;
            };
            return reordList;
        }
    }
}