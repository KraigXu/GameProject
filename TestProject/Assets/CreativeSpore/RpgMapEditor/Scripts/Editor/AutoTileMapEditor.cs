using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

namespace CreativeSpore.RpgMapEditor
{
	[CustomEditor(typeof(AutoTileMap))]
	public class AutoTileMapEditor : Editor
	{
		AutoTileMap MyAutoTileMap { get { return (AutoTileMap)target; } }
		static bool s_isEditModeOn = false;
		TilesetComponent m_tilesetComponent;

		int m_mapWidth;
		int m_mapHeight;
		bool m_showCollisions = false;

        // Thanks to http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
        ReorderableList m_layerList;

        string[] m_sortingLayerNames;

        private enum eBrushMode
        {
            Paint,
            Erase
        }
        static eBrushMode s_brushMode = eBrushMode.Paint;       

		void OnEnable()
		{
            m_sortingLayerNames = GetSortingLayerNames();
			m_tilesetComponent = new TilesetComponent( MyAutoTileMap );
            m_tilesetComponent.OnTileSelectionChanged += OnTileSelectionChanged;
			if( MyAutoTileMap.MapData != null )
			{
				m_mapWidth = MyAutoTileMap.MapData.Data.TileMapWidth;
				m_mapHeight = MyAutoTileMap.MapData.Data.TileMapHeight;
			}
			if( MyAutoTileMap.BrushGizmo != null )
			{
				MyAutoTileMap.BrushGizmo.gameObject.SetActive(false);
			}

            m_layerList = new ReorderableList(serializedObject, serializedObject.FindProperty("MapLayers"), true, true, true, true);
            m_layerList.drawElementCallback += _LayerList_DrawElementCallback;
            m_layerList.drawHeaderCallback = (Rect rect) =>
            {  
                EditorGUI.LabelField(rect, "Map Layers");
            };
            m_layerList.onChangedCallback = (ReorderableList list) =>
            {
                //NOTE: When reordering elements in ReorderableList, elements are not moved, but data is swapped between them.
                // So if you keep addres of element 0 ex: data = list[0], after reordering element 0 with 1, data will contain the elemnt1 data.
                // Keeping a reference to MapLayer in TileChunks is useless
                serializedObject.ApplyModifiedProperties(); // apply adding and removing changes
                MyAutoTileMap.SaveMap();
                MyAutoTileMap.LoadMap();
            };
            m_layerList.onAddCallback = (ReorderableList list) =>
            {
                list.index = MyAutoTileMap.MapLayers.Count; // select added layer
                MyAutoTileMap.AddMapLayer();
            };
		}

        private void OnTileSelectionChanged(TilesetComponent source, int newTileId, int tilesetSelStart, int tilesetSelEnd)
        {
            s_brushMode = eBrushMode.Paint;
        }

        // Get the sorting layer names
        public string[] GetSortingLayerNames()
        {
            System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        private void _LayerList_DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            float savedLabelWidth = EditorGUIUtility.labelWidth;
            var element = m_layerList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float elemWidth = 20; float elemX = rect.x;
            EditorGUI.PropertyField(
                new Rect(elemX, rect.y, elemWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Visible"), GUIContent.none);
            elemX += elemWidth; elemWidth = Mathf.Clamp(Screen.width - 500, 50, 240); //NOTE: Screen.width - n, avoid left element to overlap right padding elements when resizing inspector width
            EditorGUI.PropertyField(
                new Rect(elemX, rect.y, elemWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Name"), GUIContent.none);
            elemX += elemWidth; elemWidth = 80;
            EditorGUI.PropertyField(
                new Rect(elemX, rect.y, elemWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("LayerType"), GUIContent.none);
            EditorGUIUtility.labelWidth = 75f;
            elemX += elemWidth; elemWidth = 155;
            int sortingLayerIdx = System.Array.IndexOf(m_sortingLayerNames, element.FindPropertyRelative("SortingLayer").stringValue);
            if (sortingLayerIdx < 0)
            {
                Debug.LogError(" Sorting Layer " + element.FindPropertyRelative("SortingLayer").stringValue + " not found! Default layer will be taken instead.");
                sortingLayerIdx = 0;
            }
            sortingLayerIdx = EditorGUI.Popup(
                new Rect(elemX, rect.y, elemWidth, EditorGUIUtility.singleLineHeight),
                "SortingLayer:", sortingLayerIdx, m_sortingLayerNames);
            element.FindPropertyRelative("SortingLayer").stringValue = m_sortingLayerNames[sortingLayerIdx];
            elemX += elemWidth; elemWidth = 90;
            EditorGUIUtility.labelWidth = 45f;
            EditorGUI.PropertyField(
                new Rect(elemX, rect.y, elemWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("SortingOrder"), new GUIContent() { text = "Order:" });
            elemX += elemWidth; elemWidth = 90;
            EditorGUIUtility.labelWidth = 45f;
            EditorGUI.PropertyField(
                new Rect(elemX, rect.y, elemWidth, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("Depth"), new GUIContent() { text = "Depth:" });

            EditorGUIUtility.labelWidth = savedLabelWidth;
        }

		void OnDisable()
		{
			if( MyAutoTileMap != null )
			{
				if( MyAutoTileMap.BrushGizmo != null )
				{
					MyAutoTileMap.BrushGizmo.gameObject.SetActive(false);
				}

				if( s_isEditModeOn )
				{
					//Debug.LogWarning(" You forgot to commit map changes! Map will be saved automatically for you! ");
					MyAutoTileMap.SaveMap();
				}
			}
		}
        
		public void OnSceneGUI()
		{
			if( !MyAutoTileMap.IsInitialized )
			{
				return;
			}

            DoToolBar();

            Rect rAutoTileMap = new Rect(MyAutoTileMap.transform.position.x, MyAutoTileMap.transform.position.y, MyAutoTileMap.MapTileWidth * MyAutoTileMap.CellSize.x, -MyAutoTileMap.MapTileHeight * MyAutoTileMap.CellSize.y);
			UtilsGuiDrawing.DrawRectWithOutline( rAutoTileMap, new Color(0f, 0f, 0f, 0f), new Color(1f, 1f, 1f, 1f));
			if( m_showCollisions )
			{
				DrawCollisions();
			}

			if (s_isEditModeOn)
			{
				int controlID = GUIUtility.GetControlID(FocusType.Passive);
				HandleUtility.AddDefaultControl(controlID);
				EventType currentEventType = Event.current.GetTypeForControl(controlID);
				bool skip = false;
				int saveControl = GUIUtility.hotControl;

                if (currentEventType == EventType.Layout) { skip = true; }
                else if (currentEventType == EventType.ScrollWheel) { skip = true; }

                if (!skip)
                {
                    EditorGUIUtility.AddCursorRect(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), MouseCursor.Arrow);
                    GUIUtility.hotControl = controlID;

                    if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
                    {
                        s_brushMode = eBrushMode.Paint;
                    }
                    m_tilesetComponent.OnSceneGUI();

                    if (currentEventType == EventType.MouseDrag && Event.current.button < 2) // 2 is for central mouse button
                    {
                        // avoid dragging the map
                        Event.current.Use();
                    }
                }

				GUIUtility.hotControl = saveControl;

				if (GUI.changed) 
				{
					EditorUtility.SetDirty(target);
				}
			}
		}

        enum eTabType
        {
            Settings,
            Paint,
            Data
        }
        private static eTabType s_tabType = eTabType.Settings;
        private bool m_isMapInitialized = false;
		public override void OnInspectorGUI()
		{
			// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
			serializedObject.Update();

            if (Event.current.type == EventType.Layout) // Fix gui repaint exception
            {
                m_isMapInitialized = MyAutoTileMap.IsInitialized;
            }

			EditorGUILayout.BeginHorizontal();
			if( MyAutoTileMap.Tileset != null && MyAutoTileMap.Tileset.AtlasTexture == null )
			{
				MyAutoTileMap.Tileset.AtlasTexture = EditorGUILayout.ObjectField ("Tileset Atlas", MyAutoTileMap.Tileset.AtlasTexture, typeof(Texture2D), false) as Texture2D;
				if (GUILayout.Button("Edit Tileset..."))
				{
					AutoTilesetEditorWindow.ShowDialog( MyAutoTileMap.Tileset );
				}
			}
			else
			{
				MyAutoTileMap.Tileset = (AutoTileset)EditorGUILayout.ObjectField("Tileset", MyAutoTileMap.Tileset, typeof(AutoTileset), false);
			}

			if( MyAutoTileMap.Tileset == null )
			{
				if( GUILayout.Button("Create...") )
				{
					MyAutoTileMap.Tileset = RpgMapMakerEditor.CreateTileset();
				}
			}
			EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
			MyAutoTileMap.MapData = (AutoTileMapData)EditorGUILayout.ObjectField("Map Data", MyAutoTileMap.MapData, typeof(AutoTileMapData), false);
			if( MyAutoTileMap.MapData == null && GUILayout.Button("Create...") )
			{
				MyAutoTileMap.MapData = RpgMapMakerEditor.CreateAutoTileMapData();                
			}

            if ( EditorGUI.EndChangeCheck() && MyAutoTileMap.MapData != null)
            {
                m_mapWidth = MyAutoTileMap.MapData.Data.TileMapWidth;
                m_mapHeight = MyAutoTileMap.MapData.Data.TileMapHeight;
            }
			EditorGUILayout.EndHorizontal();

            if (MyAutoTileMap.Tileset != null && MyAutoTileMap.MapData != null && m_isMapInitialized)
			{
                string[] toolBarButtonNames = System.Enum.GetNames(typeof(eTabType));

                s_tabType = (eTabType)GUILayout.Toolbar((int)s_tabType, toolBarButtonNames);
                switch (s_tabType)
                {
                    case eTabType.Settings: DrawSettingsTab(); break;
                    case eTabType.Paint: DrawPaintTab(); break;
                    case eTabType.Data: DrawDataTab(); break;
                }								
			}
            else if (!MyAutoTileMap.IsInitialized)
            {
                MyAutoTileMap.LoadMap();
            }
            else
            {
                EditorGUILayout.HelpBox("You need to select or create a Tileset and a Map Data asset", MessageType.Info);
            }

			if( GUI.changed )
			{
				EditorUtility.SetDirty( MyAutoTileMap );
				if( MyAutoTileMap.Tileset != null )
					EditorUtility.SetDirty( MyAutoTileMap.Tileset );
				if( MyAutoTileMap.MapData != null )
					EditorUtility.SetDirty( MyAutoTileMap.MapData );
			}

			// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
			serializedObject.ApplyModifiedProperties();

            SceneView.RepaintAll();
		}

        private bool showReloadMapBtn = false;
        void DrawSettingsTab()
        {
            MyAutoTileMap.ViewCamera = (Camera)EditorGUILayout.ObjectField("View Camera", MyAutoTileMap.ViewCamera, typeof(Camera), true);
            m_mapWidth = EditorGUILayout.IntField("Map Width", m_mapWidth);
            m_mapHeight = EditorGUILayout.IntField("Map Height", m_mapHeight);
            if (m_mapWidth != MyAutoTileMap.MapData.Data.TileMapWidth || m_mapHeight != MyAutoTileMap.MapData.Data.TileMapHeight)
            {
                //TODO: refactor resize dimensions
                if (GUILayout.Button("Apply New Dimensions"))
                {
                    MyAutoTileMap.SaveMap(m_mapWidth, m_mapHeight);
                    MyAutoTileMap.LoadMap();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_cellSize"));
            if( EditorGUI.EndChangeCheck() )
            {
                showReloadMapBtn = true;
            }
            if( showReloadMapBtn && GUILayout.Button("Reload Map"))
            {
                showReloadMapBtn = false;
                MyAutoTileMap.LoadMap();
            }

            MyAutoTileMap.AnimatedTileSpeed = EditorGUILayout.FloatField("Animated Tile Speed", MyAutoTileMap.AnimatedTileSpeed);

            MyAutoTileMap.AutoTileMapGui.enabled = EditorGUILayout.Toggle("Show Map Editor On Play", MyAutoTileMap.AutoTileMapGui.enabled);
            if (Application.isPlaying)
            {
                MyAutoTileMap.IsCollisionEnabled = EditorGUILayout.Toggle("Collision Enabled", MyAutoTileMap.IsCollisionEnabled);
            }
            else
            {
                Renderer minimapRenderer = MyAutoTileMap.EditorMinimapRender.GetComponent<Renderer>();
                bool prevEnabled = minimapRenderer.enabled;
                minimapRenderer.enabled = EditorGUILayout.Toggle("Show Minimap", minimapRenderer.enabled);                
                if (!prevEnabled && minimapRenderer.enabled) MyAutoTileMap.RefreshMinimapTexture();
                MyAutoTileMap.BrushGizmo.IsRefreshMinimapEnabled = minimapRenderer.enabled;
            }
            m_showCollisions = EditorGUILayout.Toggle("Show Collisions", m_showCollisions);
            MyAutoTileMap.ShowGrid = EditorGUILayout.Toggle("Show Grid", MyAutoTileMap.ShowGrid);
            MyAutoTileMap.SaveChangesAfterPlaying = EditorGUILayout.Toggle("Save Changes After Playing", MyAutoTileMap.SaveChangesAfterPlaying);

            //DrawDefaultInspector();
        }

        void DrawDataTab()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (GUILayout.Button("Reload Map"))
            {
                MyAutoTileMap.LoadMap();
            }

            if (GUILayout.Button("Import Map..."))
            {
                if (MyAutoTileMap.ShowLoadDialog())
                {
                    EditorUtility.SetDirty(MyAutoTileMap);
                    SceneView.RepaintAll();
                }
            }

            if (GUILayout.Button("Export Map..."))
            {
                MyAutoTileMap.ShowSaveDialog();
            }

            if (GUILayout.Button("Clear Map..."))
            {
                bool isOk = EditorUtility.DisplayDialog("Clear Map...", "Are you sure?", "Yes", "No");
                if (isOk)
                {
                    MyAutoTileMap.ClearMap();
                    MyAutoTileMap.SaveMap();
                }
            }

            if (GUILayout.Button("Generate Map..."))
            {
                bool isWarning = MyAutoTileMap.MapTileWidth * MyAutoTileMap.MapTileHeight >= 400 * 400;
                string sWarning = "\n\nMap is too big. This can take up to several minutes.";
                bool isOk = EditorUtility.DisplayDialog("Generate Map...", "Are you sure?" + (isWarning? sWarning : ""), "Yes", "No");
                if (isOk)
                {
                    MyAutoTileMap.ClearMap();

                    // set the right layer index if default layers are changed
                    int gndLayer = 0;
                    int gndOverlay = 1;                    

                    float fDiv = 25f;
                    float xf = Random.value * 100;
                    float yf = Random.value * 100;
                    for (int i = 0; i < MyAutoTileMap.MapTileWidth; i++)
                    {
                        for (int j = 0; j < MyAutoTileMap.MapTileHeight; j++)
                        {
                            float fRand = Random.value;
                            float noise = Mathf.PerlinNoise((i + xf) / fDiv, (j + yf) / fDiv);
                            //Debug.Log( "noise: "+noise+"; i: "+i+"; j: "+j );
                            if (noise < 0.3) //water
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 0, gndLayer, false);
                            }
                            else if (noise < 0.4) // water plants
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 0, gndLayer, false);
                                if (fRand < noise / 3)
                                    MyAutoTileMap.SetAutoTile(i, j, 5, gndOverlay, false);
                            }
                            else if (noise < 0.5 && fRand < (1 - noise / 2)) // dark grass
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 32, gndLayer, false);
                            }
                            else if (noise < 0.6 && fRand < (1 - 1.2 * noise)) // flowers
                            {
                                //MyAutoTileMap.AddAutoTile( i, j, 24, (int)AutoTileMap.eTileLayer.GROUND);
                                MyAutoTileMap.SetAutoTile(i, j, 144, gndLayer, false);
                                MyAutoTileMap.SetAutoTile(i, j, 288 + Random.Range(0, 5), gndOverlay, false);
                            }
                            else if (noise < 0.7) // grass
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 144, gndLayer, false);
                            }
                            else // mountains
                            {
                                MyAutoTileMap.SetAutoTile(i, j, 33, gndLayer, false);
                            }
                        }
                    }
                    //float now, now2;
                    //now = Time.realtimeSinceStartup;

                    //now2 = Time.realtimeSinceStartup;
                    MyAutoTileMap.RefreshAllTiles();
                    //Debug.Log("RefreshAllTiles execution time(ms): " + (Time.realtimeSinceStartup - now2) * 1000);

                    //now2 = Time.realtimeSinceStartup;
                    MyAutoTileMap.SaveMap();
                    //Debug.Log("SaveMap execution time(ms): " + (Time.realtimeSinceStartup - now2) * 1000);

                    MyAutoTileMap.RefreshMinimapTexture();

                    //now2 = Time.realtimeSinceStartup;
                    MyAutoTileMap.UpdateChunks();
                    //Debug.Log("UpdateChunks execution time(ms): " + (Time.realtimeSinceStartup - now2) * 1000);

                    //Debug.Log("Total execution time(ms): " + (Time.realtimeSinceStartup - now) * 1000);

                }
            }
        }

        void DrawPaintTab()
        {
            if (MyAutoTileMap.BrushGizmo != null)
            {
                MyAutoTileMap.BrushGizmo.gameObject.SetActive(s_isEditModeOn);
            }
            if (s_isEditModeOn)
            {
                if (GUILayout.Button("Commit"))
                {
                    s_isEditModeOn = false;                 
                    MyAutoTileMap.SaveMap();
                    EditorUtility.SetDirty(MyAutoTileMap);
                    EditorUtility.SetDirty(MyAutoTileMap.Tileset);
                    EditorUtility.SetDirty(MyAutoTileMap.MapData);
                    AssetDatabase.SaveAssets();
                    Repaint();
                }

                // avoid value -1 (  no row selected ) if there is at least an element
                if( m_layerList.count > 0 )
                {
                    m_layerList.index = Mathf.Max(m_layerList.index, 0);
                    EditorGUILayout.HelpBox("Selected Layer: " + MyAutoTileMap.MapLayers[m_layerList.index].Name, MessageType.None);
                    m_layerList.DoLayoutList();
                    MyAutoTileMap.BrushGizmo.SelectedLayer = m_layerList.index;
                    MyAutoTileMap.UpdateChunkLayersData(); //update layer data changed by DoLayoutList
                }
                else
                {
                    m_layerList.DoLayoutList();
                }

                if (GUILayout.Button("Clear Selected Layer: " + MyAutoTileMap.MapLayers[m_layerList.index].Name))
                {
                    if (EditorUtility.DisplayDialog("Clear Layer " + MyAutoTileMap.MapLayers[m_layerList.index].Name, "Are your sure? This action cannot be undone!", "Clear Layer", "Cancel"))
                    {
                        MyAutoTileMap.ClearLayer(MyAutoTileMap.MapLayers[m_layerList.index]);
                        MyAutoTileMap.MarkLayerChunksForUpdate(MyAutoTileMap.MapLayers[m_layerList.index]);
                        MyAutoTileMap.UpdateChunks();
                    }
                }

                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });                
                MyAutoTileMap.BrushGizmo.SmartBrushEnabled = EditorGUILayout.ToggleLeft("Smart Brush", MyAutoTileMap.BrushGizmo.SmartBrushEnabled, EditorStyles.boldLabel);
                if (MyAutoTileMap.BrushGizmo.SmartBrushEnabled)
                {
                    GUILayout.Box("Tiles with overlay collision ★ will be placed in the first overlay layer found over selected layer");
                    GUILayout.Box("Tiles with alpha will be placed in the layer over the selected layer");
                }
                else
                {
                    GUILayout.Box("Tiles will be placed in the selected layer");
                }

                m_tilesetComponent.OnInspectorGUI();
                m_tilesetComponent.IsEditCollision = Event.current.shift;

                Repaint();

            }
            else
            {                
                GUILayout.BeginVertical();
                if (GUILayout.Button("Edit"))
                {
                    s_isEditModeOn = true;
                    Repaint();
                }
                EditorGUILayout.HelpBox("Press Edit to start painting and remember to commit your changes to be sure they are saved into asset map data.", MessageType.Info);
                GUILayout.EndVertical();
            }
        }

		void DrawCollisions()
		{
            float fCollW = MyAutoTileMap.CellSize.x / 4f;
            float fCollH = MyAutoTileMap.CellSize.y / 4f;
			Rect rColl = new Rect(0, 0, fCollW, -fCollH);
			Color cColl = new Color( 1f, 0f, 0f, 0.1f );
			Vector3 vTopLeft = HandleUtility.GUIPointToWorldRay(Vector3.zero).origin;
			Vector3 vBottomRight = HandleUtility.GUIPointToWorldRay( new Vector3( Screen.width, Screen.height ) ).origin;
			vTopLeft.y = -vTopLeft.y;
			vBottomRight.y = -vBottomRight.y;
			vTopLeft.x -= (vTopLeft.x % fCollW) + fCollW/2;
			vTopLeft.y -= (vTopLeft.y % fCollH) + fCollH/2;
			vBottomRight.x -= (vBottomRight.x % fCollW) - fCollW/2;
			vBottomRight.y -= (vBottomRight.y % fCollH) - fCollH/2;
            for (float y = vTopLeft.y; y <= vBottomRight.y; y += MyAutoTileMap.CellSize.y / 4f)
			{
                for (float x = vTopLeft.x; x <= vBottomRight.x; x += MyAutoTileMap.CellSize.x / 4f)
				{
					eTileCollisionType collType = MyAutoTileMap.GetAutotileCollisionAtPosition( new Vector3( x, -y ) );
					if( collType != eTileCollisionType.PASSABLE )
					{
						rColl.position = new Vector2(x-fCollW/2, -(y-fCollH/2));
						UtilsGuiDrawing.DrawRectWithOutline( rColl, cColl, cColl );
					}
				}
			}
		}

        private GUIStyle m_toolbarBoxStyle;
        static Color s_toolbarBoxBgColor = new Color(0f, 0f, .4f, 0.4f);
        static Color s_toolbarBoxOutlineColor = new Color(.25f, .25f, 1f, 0.70f);
        bool DoToolBar()
        {
            bool isMouseInsideToolbar = false;
            if (m_toolbarBoxStyle == null)
            {
                m_toolbarBoxStyle = new GUIStyle();
                m_toolbarBoxStyle.normal.textColor = Color.white;
                m_toolbarBoxStyle.richText = true;
            }

            GUIContent brushCoords = new GUIContent("<b> Brush Pos: " + MyAutoTileMap.BrushGizmo.BrushTilePos + "</b>");
            GUIContent selectedTileId = new GUIContent("<b> Selected Tile Id: " + m_tilesetComponent.SelectedTileIdx + "</b>");            

            Rect rTools = new Rect(4f, 4f, Mathf.Max(m_toolbarBoxStyle.CalcSize(brushCoords).x, m_toolbarBoxStyle.CalcSize(selectedTileId).x) + 4f, 44f);

            Handles.BeginGUI();
            GUILayout.BeginArea(rTools);
            HandlesEx.DrawRectWithOutline(new Rect(0, 0, rTools.size.x, rTools.size.y), s_toolbarBoxBgColor, s_toolbarBoxOutlineColor);

            GUILayout.Space(2f);
            GUILayout.Label(brushCoords, m_toolbarBoxStyle);
            GUILayout.Label(selectedTileId, m_toolbarBoxStyle);
            GUILayout.Label("<b> F1 - Display Help</b>", m_toolbarBoxStyle);
            GUILayout.EndArea();

            if (s_tabType == eTabType.Paint && s_isEditModeOn)
            {
                // Display ToolBar
                Rect rToolBar = new Rect(rTools.xMax + 4f, rTools.y, System.Enum.GetValues(typeof(ToolIcons.eToolIcon)).Length * 32f, 32f);
                isMouseInsideToolbar = rToolBar.Contains(Event.current.mousePosition);
                GUILayout.BeginArea(rToolBar);
                HandlesEx.DrawRectWithOutline(new Rect(0, 0, rToolBar.size.x, rToolBar.size.y), s_toolbarBoxBgColor, s_toolbarBoxOutlineColor);
                GUILayout.BeginHorizontal();

                int buttonPadding = 4;
                Rect rToolBtn = new Rect(buttonPadding, buttonPadding, rToolBar.size.y - 2 * buttonPadding, rToolBar.size.y - 2 * buttonPadding);
                foreach (ToolIcons.eToolIcon toolIcon in System.Enum.GetValues(typeof(ToolIcons.eToolIcon)))
                {
                    _DoToolbarButton(rToolBtn, toolIcon);
                    rToolBtn.x = rToolBtn.xMax + 2 * buttonPadding;
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
                //---
            }

            Handles.EndGUI();

            if (m_displayHelpBox)
            {
                DisplayHelpBox();
            }
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F1)
            {
                m_displayHelpBox = !m_displayHelpBox;
            }

            return isMouseInsideToolbar;
        }

        private void _DoToolbarButton(Rect rToolBtn, ToolIcons.eToolIcon toolIcon)
        {
            int iconPadding = 6;
            Rect rToolIcon = new Rect(rToolBtn.x + iconPadding, rToolBtn.y + iconPadding, rToolBtn.size.y - 2 * iconPadding, rToolBtn.size.y - 2 * iconPadding);
            Color activeColor = new Color(1f, 1f, 1f, 0.8f);
            Color disableColor = new Color(1f, 1f, 1f, 0.4f);
            switch (toolIcon)
            {
                case ToolIcons.eToolIcon.Pencil:
                    GUI.color = s_brushMode == eBrushMode.Paint ? activeColor : disableColor;
                    if (GUI.Button(rToolBtn, new GUIContent("", "Paint")))
                    {
                        s_brushMode = eBrushMode.Paint;
                        MyAutoTileMap.BrushGizmo.Clear();
                        MyAutoTileMap.BrushGizmo.BrushAction = new AutoTileBrush.TileAction();
                        MyAutoTileMap.BrushGizmo.BrushAction.Push(MyAutoTileMap, 0, 0, m_tilesetComponent.SelectedTileIdx, MyAutoTileMap.BrushGizmo.SelectedLayer);
                    }
                    break;
                case ToolIcons.eToolIcon.Erase:
                    GUI.color = s_brushMode == eBrushMode.Erase ? activeColor : disableColor;
                    if (GUI.Button(rToolBtn, new GUIContent("", "Erase")))
                    {
                        s_brushMode = eBrushMode.Erase;
                        MyAutoTileMap.BrushGizmo.Clear();
                        MyAutoTileMap.BrushGizmo.BrushAction = new AutoTileBrush.TileAction();
                        MyAutoTileMap.BrushGizmo.BrushAction.Push(MyAutoTileMap, 0, 0, -1, MyAutoTileMap.BrushGizmo.SelectedLayer);
                    }
                    break;
                case ToolIcons.eToolIcon.Undo:
                    GUI.color = MyAutoTileMap.BrushGizmo.CanUndo()? activeColor : disableColor;
                    if (GUI.Button(rToolBtn, new GUIContent("", " Undo Last Brush Action (Shift + Z)")))
                    {
                        MyAutoTileMap.BrushGizmo.UndoAction();
                    }
                    break;
                case ToolIcons.eToolIcon.Redo:
                    GUI.color = MyAutoTileMap.BrushGizmo.CanRedo()? activeColor : disableColor;
                    if (GUI.Button(rToolBtn, new GUIContent("", " Redo Last Brush Action (Shift + Y)")))
                    {
                        MyAutoTileMap.BrushGizmo.RedoAction();
                    }
                    break;
                case ToolIcons.eToolIcon.Info:
                    GUI.color = m_displayHelpBox ? activeColor : disableColor;
                    if (GUI.Button(rToolBtn, new GUIContent("", " Display Help (F1)")))
                    {
                        m_displayHelpBox = !m_displayHelpBox;
                    }
                    break;
            }
            GUI.color = Color.white;
            GUI.DrawTexture(rToolIcon, ToolIcons.GetToolTexture(toolIcon));
        }

        private bool m_displayHelpBox = false;
        void DisplayHelpBox()
        {
            string sHelp =
                "\n" +
                " - <b>Left Mouse Button:</b>\n" +
                "   * Select tile from tileset menu\n" +
                "   * Draw tile or copied group of tiles.\n" +
                "      If the tile is selected from tileset, not copied using right mouse button,\n"+
                "      and the tile is opaque, it will be drawn in the ground layer without overwriting\n"+
                "      any other tile in the other layers.\n" + 
                "      Useful to change terrain type without erasing ground overlay tiles.\n" +
                "\n" +
                " - <b>Left Mouse Button + Shift Key:</b>\n" +
                "   * Force drawing opaque tiles in the ground overlay layer\n" +
                "\n" +
                " - <b>Right Mouse Button</b>\n" +
                "   * Copy tile under the cursor or press and drag to copy a group of tiles.\n"+
                "      It's workingboth, in the map and tileset\n" +
                "\n"+
                " - <b>Right Mouse Button + Shift:</b>\n" +
                "   * Copy only alpha tiles if any, otherwise copy opaque tile.\n"+
                "      Use this to copy, for example, a tree without copy the ground tiles under it.\n"+
                "\n" +
                " - <b>Shift + Z:</b> Undo last Brush Action\n" +
                "\n" +
                " - <b>Shift + Y:</b> Redo last Brush Action\n" +
                "\n";
            GUIContent helpContent = new GUIContent(sHelp);
            Handles.BeginGUI();
            Vector2 helpBoxSize = m_toolbarBoxStyle.CalcSize(helpContent);
            Rect rHelpBox = new Rect(2f, 50f, helpBoxSize.x + 5f, helpBoxSize.y);
            GUILayout.BeginArea(rHelpBox);
            HandlesEx.DrawRectWithOutline(new Rect(0, 0, rHelpBox.size.x, rHelpBox.size.y), s_toolbarBoxBgColor, s_toolbarBoxOutlineColor);
            GUILayout.Label(sHelp, m_toolbarBoxStyle);
            GUILayout.EndArea();
            Handles.EndGUI();
        }
	}
}