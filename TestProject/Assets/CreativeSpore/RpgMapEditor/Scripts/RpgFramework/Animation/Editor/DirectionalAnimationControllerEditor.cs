using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace CreativeSpore.RpgMapEditor
{
    [CustomEditor(typeof(DirectionalAnimationController))]
    public class DirectionalAnimationControllerEditor : Editor
    {
        [MenuItem("Assets/Create/RpgMapEditor/Directional Animation Controller")]
        public static DirectionalAnimationController CreateAsset()
        {
            return RpgMapMakerEditor.CreateAssetInSelectedDirectory<DirectionalAnimationController>();
        }

        class Styles
        {
            static Styles s_instance;
            public static Styles Instance { get { return s_instance != null ? s_instance : (s_instance = new Styles()); } }

            public GUIStyle richHelpBox = new GUIStyle("HelpBox")
            {
                richText = true,
            };
        }

        private ReorderableList m_animReordList;
        private ReorderableList m_dirMappingReordList;
        private DirectionalAnimationController m_target;
        void OnEnable()
        {
            m_target = target as DirectionalAnimationController;
            m_target.CleanInvalidAnims();
            m_animReordList = CreateAnimReorderableList();
            m_dirMappingReordList = CreateDirMappingReordList();
        }

        public override void OnInspectorGUI()
        {
            Event e = Event.current;
            serializedObject.Update();

            //DrawDefaultInspector();
            m_dirMappingReordList.DoLayoutList();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_dirType"), new GUIContent("Directions"));
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_spriteAlignment"));
            GUI.enabled = m_target.SpriteAlignment == SpriteAlignment.Custom;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_pivot"));
            GUI.enabled = true;
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                UpdateAligmentAndPivotAll();
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fpa"), new GUIContent("Animation Frames", "How many frames each direction have."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fps"), new GUIContent("Animation Speed", "Frames per second."));

            if (GUILayout.Button("Clear All Animations") && EditorUtility.DisplayDialog("Clear All Animations", "Are you sure to clear all animations?", "Yes", "No"))
            {
                Undo.RegisterCompleteObjectUndo(target, "Clear All Animations");
                m_target.GetAnimList().Clear();
                serializedObject.Update();
            }

            if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                Undo.RegisterCompleteObjectUndo(target, "Remove Character Animation");
                ReorderableList.defaultBehaviours.DoRemoveButton(m_animReordList);
            }
            m_animReordList.headerHeight = 64f;
            m_animReordList.elementHeight = m_target.DirectionsPerAnim <= 4? 64f : 128f;
            m_animReordList.DoLayoutList();

            Repaint();
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private ReorderableList CreateDirMappingReordList()
        {
            ReorderableList reordList = new ReorderableList(m_target.DirectionMapping, typeof(DirectionalAnimData), true, true, true, true);
            reordList.drawHeaderCallback += (Rect rect) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Character Sheet Direction Order", EditorStyles.boldLabel);
                EditorGUI.TextArea(new Rect(rect.x - 6f, rect.y + EditorGUIUtility.singleLineHeight, rect.width + 12f, rect.height - EditorGUIUtility.singleLineHeight),
                        "<size=11><i>Reorder the directions in the same way they appear in the character sheet</i></size>", Styles.Instance.richHelpBox);
                Texture2D btnTexture = reordList.elementHeight == 0f ? EditorGUIUtility.FindTexture("winbtn_win_max_h") : EditorGUIUtility.FindTexture("winbtn_win_min_h");
                if (GUI.Button(new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), btnTexture, EditorStyles.label))
                {
                    reordList.elementHeight = reordList.elementHeight == 0f ? 21f : 0f;
                    reordList.draggable = reordList.elementHeight > 0f;
                }
                reordList.displayAdd = reordList.displayRemove = reordList.elementHeight > 0;
                reordList.headerHeight = reordList.elementHeight == 0f ? 21f : 50f;
            };
            reordList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (reordList.elementHeight > 0f)
                {
                    GUI.Box(rect, "" + ((eAnimDir)reordList.list[index]).ToString());
                }
            };
            return reordList;            
        }

        private ReorderableList CreateAnimReorderableList()
        {
            ReorderableList reordList = new ReorderableList(m_target.GetAnimList(), typeof(DirectionalAnimData), true, true, true, true);
            reordList.drawHeaderCallback += (Rect rect) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Animations", EditorStyles.boldLabel);
                EditorGUI.TextArea(new Rect(rect.x - 6f, rect.y + EditorGUIUtility.singleLineHeight, rect.width + 12f, rect.height - EditorGUIUtility.singleLineHeight),
                    "<size=11><i>Drag a sprite sheet into this box to create a new animation or into any animation to modify it</i></size>", Styles.Instance.richHelpBox);
                DoAnimationDragAndDrop(rect, -1);
            };
            reordList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                DirectionalAnimData anim = (DirectionalAnimData)reordList.list[index];
                anim.name = GUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), anim.name);

                int dirNb = m_target.DirectionsPerAnim;
                float frameWidth = rect.width / Mathf.Min(4, dirNb);
                int rowNb = 1 + (dirNb - 1) / 4;
                float frameHeight = (rect.height - EditorGUIUtility.singleLineHeight) / rowNb;
                for (int i = 0; i < dirNb; ++i)
                {
                    Vector2 dir = Quaternion.Euler(Vector3.forward * (((float)i / dirNb) * 360f)) * Vector3.down;
                    Sprite sprite = m_target.GetPreviewAnimSprite(dir, index);
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

                DoAnimationDragAndDrop(rect, index);
            };
            reordList.onAddCallback += (ReorderableList list) =>
            {
                Undo.RegisterCompleteObjectUndo(target, "Add Character Animation");
                DirectionalAnimData animToClone = m_target.GetAnim(list.index);
                m_target.AddAnim(animToClone);
                serializedObject.Update();
                GUI.changed = true;
            };
            return reordList;
        }

        private void DoAnimationDragAndDrop(Rect rect, int animIdx)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!rect.Contains(e.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (e.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        GUI.changed = true;

                        foreach (Object dragged_object in DragAndDrop.objectReferences)
                        {
                            Undo.RegisterCompleteObjectUndo(target, "Import Character Sprite Sheet");
                            if (dragged_object is Texture2D)
                            {
                                Debug.Log("Import texture " + dragged_object.name);
                                ImportSpriteSheetIntoAnimation((Texture2D)dragged_object, animIdx);
                            }
                        }
                    }
                    break;
            }
        }

        private void ImportSpriteSheetIntoAnimation( Texture2D spriteSheet, int animIdx = -1)
        {
            string assetPath = AssetDatabase.GetAssetPath(spriteSheet);
            if (string.IsNullOrEmpty(assetPath)) return;
            TextureImporter spriteSheetImporter = (TextureImporter)TextureImporter.GetAtPath(assetPath);

            int characterNb;
            int charRowLength;
            int charColumnLength;
            int charFramesCount = m_target.DirectionsPerAnim * m_target.FramesPerAnim;
            int columns = 0, rows = 0;
            if (spriteSheetImporter.textureType != TextureImporterType.Sprite
                || spriteSheetImporter.spriteImportMode != SpriteImportMode.Multiple
                || spriteSheetImporter.spritesheet.Length == 0
                || spriteSheetImporter.spritesheet.Length % charFramesCount != 0)
            {
                
                Rect[] rects = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(spriteSheet, 4, 0);
                if ( rects.Length > 0 && rects.Length % charFramesCount == 0)
                {
                    for (; columns < rects.Length; ++columns)
                    {
                        //NOTE: the order of slicing in GenerateAutomaticSpriteRectangles is from bottom to top, not from top to bottom like Sprite Editor Slicing
                        if (rects[columns].yMin >= rects[0].yMax)
                        {
                            rows = rects.Length / columns;
                            break;
                        }
                    }
                }
                else
                {
                    columns = m_target.FramesPerAnim;
                    rows = m_target.DirectionsPerAnim;
                }

                charRowLength = Mathf.Max(1, columns / m_target.FramesPerAnim);
                charColumnLength = Mathf.Max(1, rows / m_target.FramesPerAnim);
                characterNb = charRowLength * charColumnLength;

                int spriteCount = charFramesCount * characterNb;
                SpriteMetaData[] aSpriteMetaData = spriteSheetImporter.spritesheet;
                if (spriteSheetImporter.spritesheet.Length != spriteCount || spriteSheetImporter.spriteImportMode != SpriteImportMode.Multiple)
                {
                    aSpriteMetaData = new SpriteMetaData[spriteCount];
                    spriteSheetImporter.textureType = TextureImporterType.Sprite;
                    spriteSheetImporter.spriteImportMode = SpriteImportMode.Multiple;
                    spriteSheetImporter.filterMode = FilterMode.Point;
                    spriteSheetImporter.mipmapEnabled = false;
#if UNITY_5_5_OR_NEWER
                    spriteSheetImporter.textureCompression = TextureImporterCompression.Uncompressed;
#else
                    spriteSheetImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
#endif
                    Rect spriteRect = new Rect(0, 0, spriteSheet.width / (m_target.FramesPerAnim * charRowLength), spriteSheet.height / (m_target.DirectionsPerAnim * charColumnLength));
                    for (int gy = 0, spriteIdx = 0; gy < rows; ++gy)
                    {
                        for (int gx = 0; gx < columns; ++gx, ++spriteIdx)
                        {

                            spriteRect.position = new Vector2(gx * spriteRect.width, spriteSheet.height - (1 + gy) * spriteRect.height);
                            SpriteMetaData spriteMetaData = new SpriteMetaData();
                            //int characterIdx = (gy / m_target.DirectionNb) * charRowLength + (gx / m_target.FramesPerAnim);
                            
                            //NOTE: the sprites are sorted alphabetically, so spriteIdx should be in the first place after the name and nothing else
                            spriteMetaData.name = spriteSheet.name + "_" + spriteIdx; // + (characterNb > 1 ? ("_" + characterIdx) : "") + "_" + gy + "_" + gx;
                            spriteMetaData.rect = spriteRect;
                            aSpriteMetaData[spriteIdx] = spriteMetaData;
                        }
                    }
                    spriteSheetImporter.spritesheet = aSpriteMetaData;
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }

            UpdateAligmentAndPivot(spriteSheet);

            List<Sprite> sprites = new List<Sprite>(AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>().ToArray());
            //sort them properly using the last number
            sprites = sprites.OrderBy(s => int.Parse(s.name.Substring(s.name.LastIndexOf("_") + 1))).ToList();
            for (; columns < sprites.Count; ++columns)
            {
                if (sprites[columns].rect.yMax <= sprites[0].rect.yMin)
                {
                    rows = sprites.Count / columns;
                    break;
                }
            }

            if (columns * rows != sprites.Count || columns % m_target.FramesPerAnim != 0 || rows % m_target.DirectionsPerAnim != 0)
            {
                Debug.LogError("Something was wrong with the sprite sheet. Try slicing it again using the Sprite Editor using grid settings or set the Sprite Mode to single and try again.");
                return;
            }

            List<Sprite> sortedSprites = new List<Sprite>();
            charRowLength = Mathf.Max(1, columns / m_target.FramesPerAnim);
            charColumnLength = Mathf.Max(1, rows / m_target.DirectionsPerAnim);
            for(int charY = 0; charY < charColumnLength; ++charY)
            {
                for(int charX = 0; charX < charRowLength; ++charX)
                {
                    for(int c = 0; c < m_target.DirectionsPerAnim; ++c)
                    {
                        for(int r = 0; r < m_target.FramesPerAnim; ++r)
                        {
                            int gx = charX * m_target.FramesPerAnim + r;
                            int gy = charY * m_target.DirectionsPerAnim + c;
                            sortedSprites.Add(sprites[gy * columns + gx]);
                        }
                    }
                }
            }
            characterNb = sortedSprites.Count / charFramesCount;

            if (animIdx >= 0)
            {
                ImportSpriteSheetIntoAnimation(spriteSheet, sortedSprites.Take(charFramesCount).ToArray(), animIdx, spriteSheet.name);
            }
            else
            {
                for (int characterIdx = 0; characterIdx < characterNb; ++characterIdx)
                {
                    Sprite[] characterSprites = sortedSprites.Skip(characterIdx * charFramesCount).Take(charFramesCount).ToArray();
                    string charName = spriteSheet.name + (characterNb > 1? ("_" + characterIdx) : "");
                    ImportSpriteSheetIntoAnimation(spriteSheet, characterSprites, m_target.GetAnimList().FindIndex(x => x.name == charName), charName);
                }
            }
        }

        private void ImportSpriteSheetIntoAnimation(Texture2D spriteSheet, Sprite[] sprites, int animIdx = -1, string name = "")
        {

            DirectionalAnimData anim = animIdx >= 0 ? m_target.GetAnimList()[animIdx] : null;
            if (anim == null)
            {
                DirectionalAnimData animToClone = m_target.GetAnim(animIdx);
                anim = m_target.AddAnim(animToClone);
                anim.name = name;
            }

            int spriteCount = m_target.DirectionsPerAnim * m_target.FramesPerAnim;
            if (sprites.Length == spriteCount)
            {
                for (int gy = 0, spriteIdx = 0; gy < m_target.DirectionsPerAnim; ++gy)
                {
                    int mappedDir = (int)m_target.DirectionMapping[gy];
                    mappedDir >>= (int)(eDirectionalAnimType.Eight - m_target.DirectionType);
                    for (int gx = 0; gx < m_target.FramesPerAnim; ++gx, ++spriteIdx)
                    {
                        anim.SetAnimFrame(mappedDir, gx, sprites[spriteIdx]);
                    }
                }
            }

            serializedObject.Update();            
        }

        private void UpdateAligmentAndPivotAll()
        {
            List<Texture2D> textures = new List<Texture2D>();
            foreach (DirectionalAnimData anim in m_target.GetAnimList())
            {
                for(int dir = 0; dir < anim.DirCount; ++dir)
                {
                    for(int frame = 0; frame < anim.FramesPerDir; ++frame)
                    {
                        Sprite sprite = anim.GetAnimFrame(dir, frame);
                        if( sprite && !textures.Contains(sprite.texture))
                        {
                            textures.Add(sprite.texture);
                        }
                    }
                }
            }

            foreach(Texture2D texture in textures)
            {
                UpdateAligmentAndPivot(texture);
            }
        }

        private void UpdateAligmentAndPivot(Texture2D spriteSheet)
        {
            string assetPath = AssetDatabase.GetAssetPath(spriteSheet);
            if (string.IsNullOrEmpty(assetPath)) return;
            TextureImporter spriteSheetImporter = (TextureImporter)TextureImporter.GetAtPath(assetPath);
            SpriteMetaData[] aSpriteMetaData = spriteSheetImporter.spritesheet;
            for (int i = 0; i < aSpriteMetaData.Length; ++i)
            {
                aSpriteMetaData[i].alignment = (int)m_target.SpriteAlignment;
                aSpriteMetaData[i].pivot = GetPivotValue(m_target.SpriteAlignment, m_target.CustomPivot);
            }
            SpriteImportMode savedMode = spriteSheetImporter.spriteImportMode;
            spriteSheetImporter.textureType = TextureImporterType.Sprite; // NOTE: without this, "spriteSheetImporter.spritesheet = aSpriteMetaData" won't be effective. I don't know why.
            spriteSheetImporter.spriteImportMode = savedMode; // fix: Unity 5.5 will set this to Single after setting textureType to Sprite.
            spriteSheetImporter.spritesheet = aSpriteMetaData;
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset)
        {
			switch (alignment)
			{
			case SpriteAlignment.Center:
				return new Vector2(0.5f, 0.5f);
			case SpriteAlignment.TopLeft:
				return new Vector2(0f, 1f);
			case SpriteAlignment.TopCenter:
				return new Vector2(0.5f, 1f);
			case SpriteAlignment.TopRight:
				return new Vector2(1f, 1f);
			case SpriteAlignment.LeftCenter:
				return new Vector2(0f, 0.5f);
			case SpriteAlignment.RightCenter:
				return new Vector2(1f, 0.5f);
			case SpriteAlignment.BottomLeft:
				return new Vector2(0f, 0f);
			case SpriteAlignment.BottomCenter:
				return new Vector2(0.5f, 0f);
			case SpriteAlignment.BottomRight:
				return new Vector2(1f, 0f);
			case SpriteAlignment.Custom:
				return customOffset;
			default:
				return Vector2.zero;
			}
        }
    }
}
