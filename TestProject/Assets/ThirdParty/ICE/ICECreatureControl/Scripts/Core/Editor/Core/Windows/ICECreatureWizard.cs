// ##############################################################################
//
// ice_CreatureWizard.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.Attributes;

using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures.Windows
{
	public class ICECreatureWizardObject : ICEObject {
	}

	public class ICECreatureWizard : EditorWindow {

		private static Texture2D m_LogoImage = null;

		private static Vector2 m_DialogSize = new Vector2(520, 520);
		private static string m_Version = "Version " + Info.Version;
		private static string m_Copyright = "© " + System.DateTime.Now.Year + " Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.";

		private GameObject m_CreatureGameObject = null;
		private GameObject m_PlayerGameObject = null;
		private GameObject m_GroundGameObject = null;

		private ICECreatureRegister m_CreatureRegisterComponent{
			get{ return ICECreatureRegister.Instance; }
		}
		private ICECreatureControl m_CreatureControlComponent = null;
		private ICECreaturePlayer m_CreaturePlayerComponent = null;

		private bool m_ShowGroundHandling = false;
		private bool m_ShowPlayerHandling = false;
		private bool m_ShowCreatureHandling = false;

		private int TurrentCount = 25;
		private int LocationCount = 25;
		private int WaypointCount = 25;
		private int ObstacleCount = 100;
		private int ShelterCount = 25;


		//private bool m_ShowEnvironmentHandling = false;
		/// <summary>
		/// 
		/// </summary>
		public static void Create()
		{
			ICECreatureWizard _window = (ICECreatureWizard)EditorWindow.GetWindow(typeof(ICECreatureWizard), false , "ICE Wizard" ); 
			/*
			_window.titleContent = new GUIContent( "ICE Creature Control Wizard v" + Info.Version + " (beta)", "");
*/
			_window.minSize = new Vector2(m_DialogSize.x, m_DialogSize.y);
			_window.maxSize = new Vector2(m_DialogSize.x + 1, m_DialogSize.y + 1);
			_window.position = new Rect(
				(Screen.currentResolution.width / 2) - (m_DialogSize.x / 2),
				(Screen.currentResolution.height / 2) - (m_DialogSize.y / 2),
				m_DialogSize.x,
				m_DialogSize.y);
			_window.Show();

		}

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		void OnEnable()
		{
			if( EditorGUIUtility.isProSkin )
				m_LogoImage = (Texture2D)Resources.Load("ICECC_WIZARD_W", typeof(Texture2D));
			else
				m_LogoImage = (Texture2D)Resources.Load("ICECC_WIZARD", typeof(Texture2D));
		}

		/// <summary>
		/// Raises the GUI event.
		/// </summary>
		void OnGUI()
		{
			ICEEditorInfo.HelpButtonIndex = 0;

			ICEEditorLayout.DefaultBackgroundColor = GUI.backgroundColor;

			if( m_LogoImage != null )
				GUI.DrawTexture( new Rect(10, 10, m_LogoImage.width, m_LogoImage.height), m_LogoImage );


			if( m_GroundGameObject == null )
			{
				if( Terrain.activeTerrain != null )
					m_GroundGameObject = Terrain.activeTerrain.gameObject;

				if( m_GroundGameObject == null )
				{
					GameObject[] _grounds = SystemTools.FindGameObjectsByLayer( LayerMask.NameToLayer( "WalkableSurface" ) );

					if( _grounds != null && _grounds.Length > 0 )
						m_GroundGameObject = _grounds[0];
				}
			}


			GUILayout.BeginArea(new Rect(20, 150, position.width - 40, position.height - 40));

				if( Application.isPlaying )
				{
					EditorGUILayout.HelpBox( "Please note, the wizard settings are not available at runtime!", MessageType.Info );
				}
				else
				{
					DrawRegister();
					DrawScene();
					DrawPlayer();
					DrawCreature();
				}

			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect( 20, position.height - 30, position.width - 40, position.height - 20 ) );
				GUI.backgroundColor = Color.clear;
				GUILayout.Label( m_Version + " - " + m_Copyright  + "\n\n", ICEEditorStyle.SmallTextStyle );
			GUILayout.EndArea();
		}


		#region Local Wizard Utilities

		/// <summary>
		/// Checks whether a valid register is available.
		/// </summary>
		/// <returns><c>true</c>, if register was checked, <c>false</c> otherwise.</returns>
		private bool CheckRegister(){
			return ( m_CreatureRegisterComponent != null ? true : false );
		}

		/// <summary>
		/// Checks whether a valid ground is available.
		/// </summary>
		/// <returns><c>true</c>, if ground was checked, <c>false</c> otherwise.</returns>
		private bool CheckGround()
		{
			if( ( m_CreatureRegisterComponent != null ) && (
				( m_CreatureRegisterComponent.Options.GroundCheck == GroundCheckType.SAMPLEHEIGHT && Terrain.activeTerrain != null ) ||
				( m_CreatureRegisterComponent.Options.GroundCheck == GroundCheckType.RAYCAST && m_CreatureRegisterComponent.Options.GroundLayer.Layers.Count > 0 ) ) )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Checks whether a valid player is available.
		/// </summary>
		/// <returns><c>true</c>, if player was checked, <c>false</c> otherwise.</returns>
		private bool CheckPlayer()
		{
			if( m_CreaturePlayerComponent == null || m_CreaturePlayerComponent.gameObject == null )
			{
				ICECreaturePlayer[] _players = GameObject.FindObjectsOfType<ICECreaturePlayer>();
				if( _players.Length >= 1 )
					m_CreaturePlayerComponent = _players[0];
			}

			return ( m_CreaturePlayerComponent != null ? true : false );
		}

		/// <summary>
		/// Checks whether a valid creature is available.
		/// </summary>
		/// <returns><c>true</c>, if creature was checked, <c>false</c> otherwise.</returns>
		private bool CheckCreature(){
			return ( m_CreatureControlComponent != null ? true : false );
		}

		private Vector3 GetRandomGroundPosition( GameObject _ground, float _base_offset = 0 )
		{
			Vector3 _size = Vector3.zero;
			Vector3 _pos = Vector3.zero;

			if( _ground != null )
			{
				if( _ground.GetComponent<Terrain>() && _ground.GetComponent<Terrain>().terrainData )
				{
					TerrainData _terrain_data = _ground.GetComponent<Terrain>().terrainData;

					_size = _terrain_data.size;
					_pos = new Vector3( UnityEngine.Random.Range( 0,_size.x ), 0, UnityEngine.Random.Range( 0,_size.z ) ) ;
				}
				else
				{
					_size = _ground.transform.localScale * 5;
					_pos = new Vector3( UnityEngine.Random.Range( -_size.x,_size.x ), 0, UnityEngine.Random.Range( -_size.z,_size.z ) ) ;
				}

				_pos += _ground.transform.position;
			}
				
			_pos.y = CreatureRegister.GetGroundLevel( _pos, _base_offset ) + _base_offset;

			return _pos;
		}

		private void UpdatePlayer()
		{
			if( m_CreaturePlayerComponent != null )
				m_CreaturePlayerComponent.transform.position = GetRandomGroundPosition( m_GroundGameObject, 2 );
		}

		#endregion

		#region Spawn Entities

		private void SpawnRandomShelters( GameObject _ground, int _count = 10 )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			GameObject _reference = (GameObject)Resources.Load("Prefabs/ICEShelter");

			if( _reference == null )
				return;

			m_CreatureRegisterComponent.HierarchyManagement.AddHierarchyGroup( EntityClassType.Location, true );

			for(int i= 0; i< _count ; i++ )
			{
				Vector3 _position = GetRandomGroundPosition( _ground, 0 );
				Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range(0, 360) , 0 );
				GameObject _shelter = (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
				_shelter.name = _reference.name;

				m_CreatureRegisterComponent.AddReference( _shelter );

				TerrainTools.CreatePlateau( _ground.GetComponent<Terrain>(), _position, 8 );
			}

			m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();

			if( _ground.GetComponent<Terrain>() != null )
				TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
		}

		private void SpawnRandomLocations( GameObject _ground, int _count = 10 )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			GameObject _reference = (GameObject)Resources.Load("Prefabs/ICELocation");

			if( _reference == null )
				return;

			m_CreatureRegisterComponent.HierarchyManagement.AddHierarchyGroup( EntityClassType.Location, true );

			for(int i= 0; i< _count ; i++ )
			{
				Vector3 _position = GetRandomGroundPosition( _ground, 0 );
				Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range(0, 360) , 0 );
				GameObject _location = (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
				_location.name = _reference.name;

				m_CreatureRegisterComponent.AddReference( _location );

				TerrainTools.CreatePlateau( _ground.GetComponent<Terrain>(), _position, 5 );
			}

			m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();

			if( _ground.GetComponent<Terrain>() != null )
				TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
		}

		private void SpawnRandomWater( GameObject _ground, int _count = 10 )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			GameObject _ponds = GameObject.Find( "Ponds" );
			if( _ponds == null )
			{
				_ponds = new GameObject();
				_ponds.name = "Ponds";
			}

			for(int i= 0; i< _count ; i++ )
			{
				GameObject _obj = GameObject.CreatePrimitive( PrimitiveType.Cube );

				if( _ponds != null )
					_obj.transform.parent = _ponds.transform;
				
				_obj.name = "Pond";		
				int _radius = UnityEngine.Random.Range( 20, 60 );
				_obj.transform.eulerAngles = new Vector3( 0, UnityEngine.Random.Range(0,360 ), 0 );
				_obj.transform.localScale = new Vector3( _radius, 0.1f, _radius );
				_obj.transform.position = GetRandomGroundPosition( _ground, _obj.transform.localScale.y * 0.5f );
				_obj.layer = LayerMask.NameToLayer( "Water" );


				//_obj.AddComponent<ICECreatureLocation>();
				Renderer _renderer = _obj.GetComponent<Renderer>();
				if( _renderer != null )
				{
					_renderer.sharedMaterial = new Material( Shader.Find("Standard") );
					_renderer.sharedMaterial.color = Color.blue;
				}

				//m_CreatureRegister.AddReference( _obj );

			}

			//m_CreatureRegister.ReorganizeHierarchy();
		}

		private void SpawnRandomWaypoints( GameObject _ground, int _count = 10 )
		{
			if( m_CreatureRegisterComponent == null || _ground == null )
				return;

			GameObject _reference = (GameObject)Resources.Load("Prefabs/ICEWaypoint");

			if( _reference == null )
				return;

			m_CreatureRegisterComponent.HierarchyManagement.AddHierarchyGroup( EntityClassType.Waypoint, true );

			for(int i= 0; i< _count ; i++ )
			{
				Vector3 _position = GetRandomGroundPosition( _ground, 0 );
				Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range(0, 180) , 0 );
				GameObject _waypoint = (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
				_waypoint.name = _reference.name;

				m_CreatureRegisterComponent.AddReference( _waypoint );

				TerrainTools.CreatePlateau( _ground.GetComponent<Terrain>(), _position, 5 );
			}

			m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();

			if( _ground.GetComponent<Terrain>() != null )
				TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
		}

		private void SpawnRandomTurrets( GameObject _ground, int _count = 250 )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			GameObject _reference = (GameObject)Resources.Load("Prefabs/ICETurret");

			if( _reference == null )
				return;

			m_CreatureRegisterComponent.HierarchyManagement.AddHierarchyGroup( EntityClassType.Turret, true );

			for(int i= 0; i< _count ; i++ )
			{
				Vector3 _position = GetRandomGroundPosition( _ground, 0 );
				Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range(0, 180) , 0 );
				GameObject _turret = (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
				_turret.name = _reference.name;

				m_CreatureRegisterComponent.AddReference( _turret );

				TerrainTools.CreatePlateau( _ground.GetComponent<Terrain>(), _position, 5 );
			}

			m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();

			if( _ground.GetComponent<Terrain>() != null )
				TerrainTools.UpdateSplatmap( _ground.GetComponent<Terrain>().terrainData );
		}

		private void SpawnRandomItems( GameObject _ground, int _count = 250 )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			m_CreatureRegisterComponent.HierarchyManagement.AddHierarchyGroup( EntityClassType.Item, true );

			for(int i= 0; i< _count ; i++ )
			{
				int _selection = UnityEngine.Random.Range(0,5 );

				PrimitiveType _type = PrimitiveType.Cube;
				if( _selection == 0 )
					_type = PrimitiveType.Sphere;
				else if( _selection == 1 )
					_type = PrimitiveType.Cylinder;
				else if( _selection == 2 )
					_type = PrimitiveType.Capsule;
				else
					_type = PrimitiveType.Cube;
				
				GameObject _obj = GameObject.CreatePrimitive( _type );

				_obj.name += "Item";
				_obj.transform.eulerAngles = new Vector3( 0, UnityEngine.Random.Range(0,360 ), 0 );
				_obj.transform.localScale = new Vector3( UnityEngine.Random.Range( 0.1f,1.5f ), UnityEngine.Random.Range( 0.1f,1.5f ), UnityEngine.Random.Range( 0.1f,1.5f) );
				_obj.transform.position = GetRandomGroundPosition( _ground, _obj.transform.localScale.y * 0.5f );

				_obj.AddComponent<Rigidbody>();
				//_obj.AddComponent<LODGroup>();
				_obj.AddComponent<ICECreatureItem>();
				Renderer _renderer = _obj.GetComponent<Renderer>();
				if( _renderer != null )
				{
					_renderer.sharedMaterial = new Material( Shader.Find("Standard") );
					_renderer.sharedMaterial.color = HSBColor.Random(0.15f,0.65f,1);// Color.cyan;//new Color( Random.Range(0,1), Random.Range(0,1), Random.Range(0,1) );// // UnityEngine.Random.ColorHSV();
				}
		
				m_CreatureRegisterComponent.AddReference( _obj );
			}

			m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();
		}

		private void SpawnRandomObstacles( GameObject _ground, int _count = 250 )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			GameObject[] _references = new GameObject[4];
			_references[0] = (GameObject)Resources.Load("Prefabs/ICERock01");
			_references[1] = (GameObject)Resources.Load("Prefabs/ICERock02");
			_references[2] = (GameObject)Resources.Load("Prefabs/ICERock03");
			_references[3] = (GameObject)Resources.Load("Prefabs/ICERock04");

			if( _references == null )
				return;

			m_CreatureRegisterComponent.HierarchyManagement.AddHierarchyGroup( EntityClassType.Object, true );

			for(int i= 0; i< _count ; i++ )
			{
				GameObject _reference = _references[ UnityEngine.Random.Range( 0, 4 ) ];

				if( _reference == null )
					continue;
				
				Vector3 _position = GetRandomGroundPosition( _ground, 0 );
				Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range(0, 360) , 0 );
				GameObject _obstacle = (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
				_obstacle.name = _reference.name;

				m_CreatureRegisterComponent.AddReference( _obstacle );
			}

			m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();

			/*
			GameObject _obstacles = GameObject.Find( "Obstacle" );
			if( _obstacles == null )
			{
				_obstacles = new GameObject();
				_obstacles.name = "Obstacle";
			}
			
			for(int i= 0; i< _count ; i++ )
			{
				GameObject _obj = GameObject.CreatePrimitive( PrimitiveType.Cube );

				if( _obstacles != null )
					_obj.transform.parent = _obstacles.transform;

				_obj.name = "Obstacle";
				_obj.layer = LayerMask.NameToLayer( "Obstacle" );
				_obj.transform.eulerAngles = new Vector3( 0, Random.Range(0,360 ), 0 );
				_obj.transform.localScale = new Vector3( Random.Range( 1,10 ), Random.Range( 1,10 ), Random.Range( 1,10 ) );
				_obj.transform.position = GetRandomGroundPosition( _ground, _obj.transform.localScale.y * 0.5f );

				//_obj.AddComponent<ICECreatureLocation>();
				Renderer _renderer = _obj.GetComponent<Renderer>();
				if( _renderer != null )
				{
					_renderer.sharedMaterial = new Material( Shader.Find("Standard") );
					_renderer.sharedMaterial.color = HSBColor.Random(0.75f,0.95f,1);//new Color( Random.Range(0,1), Random.Range(0,1), Random.Range(0,1) ); // //UnityEngine.Random.ColorHSV();
				}
		
				//m_CreatureRegister.AddReference( _obj );
			}*/

			//m_CreatureRegister.ReorganizeHierarchy();
		}

		/// <summary>
		/// Removes the objects.
		/// </summary>
		/// <param name="_type">Type.</param>
		private void RemoveObjects( EntityClassType _type )
		{
			if( m_CreatureRegisterComponent == null )
				return;

			if( m_CreatureRegisterComponent.HierarchyManagement.Enabled )
				m_CreatureRegisterComponent.HierarchyManagement.RemoveHierarchyGroup( _type );
			else
			{
				Component[] _components = null;
				if( _type == EntityClassType.Turret )
					_components = GameObject.FindObjectsOfType<ICECreatureTurret>();
				else if( _type == EntityClassType.Location )
					_components = GameObject.FindObjectsOfType<ICECreatureLocation>();
				else if( _type == EntityClassType.Waypoint )
					_components = GameObject.FindObjectsOfType<ICECreatureWaypoint>();
				else if( _type == EntityClassType.Object )
					_components = GameObject.FindObjectsOfType<ICECreatureObject>();

				if( _components != null && _components.Length > 0 )
				{
					List<GameObject> _objects = new List<GameObject>();

					foreach( Component _component in _components )
						_objects.Add( _component.gameObject );
						
					while( _objects.Count > 0 )
					{
						if( _objects[0] != null )
						{
							GameObject _obj =_objects[0];
							_objects.RemoveAt(0);
							GameObject.DestroyImmediate( _obj );

						}
					}
				}
			}
		}

		#endregion

		#region Main Drawing Utilities

		/// <summary>
		/// Draws the register settings.
		/// </summary>
		private void DrawRegister()
		{
			// CREATURE REGISTER BEGIN
			GUI.backgroundColor = (CheckRegister()?Color.green:Color.yellow);
			EditorGUI.BeginDisabledGroup ( CheckRegister() == true );
				if( ICEEditorLayout.ButtonExtraLarge( "ADD CREATURE REGISTER", "" ))
				{
					ICECreatureRegister.Create();
					Selection.activeGameObject = m_CreatureRegisterComponent.gameObject;
				}
			EditorGUI.EndDisabledGroup();
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if( m_CreatureRegisterComponent != null )
			{
				m_CreatureRegisterComponent.HierarchyManagement.Enabled = false;
				//m_CreatureRegister.HierarchyManagement.UpdateHierarchyGroups( true, true );

				m_CreatureRegisterComponent.UseDebug = true;
			}
			// CREATURE REGISTER END
		}

		/*
		private void DrawEnvironment()
		{
			// ENVIRONMENT BEGIN
			GUI.backgroundColor = (CheckEnvironment()?Color.green:Color.yellow);
			EditorGUI.BeginDisabledGroup ( CheckEnvironment() == true );

			if( ICEEditorLayout.Button( "ADD ENVIRONMENT CONTROLLER", "" , ICEEditorStyle.ButtonExtraLarge ))
			{
				GameObject _environment = new GameObject();
				_environment.name = "Environment";
				_environment.transform.position = Vector3.zero;
				_environment.AddComponent<ICEEnvironment>();
				m_Environment = _environment.GetComponent<ICEEnvironment>();

				Selection.activeGameObject = m_Environment.gameObject;
			}
			EditorGUI.EndDisabledGroup();
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if( m_Environment != null )
			{
				Light[] _lights = GameObject.FindObjectsOfType<Light>();
				foreach( Light _light in _lights )
				{
					if( _light != null && _light.type == LightType.Directional )
					{
						m_Environment.Sun = _light;
						break;
					}
				}

				if( m_Environment.Sun != null )
				{
					m_Environment.Sun.transform.parent = m_Environment.transform;
					m_Environment.Sun.transform.position = Vector3.zero;
				}
			}
			// ENVIRONMENT END
		}

*/

	
		/// <summary>
		/// Draws the scene objects settings.
		/// </summary>
		private void DrawSceneObjects()
		{
			ICEEditorStyle.Splitter();

			int _turrets = 0;
			if( ICECreatureRegister.Instance )
			{
				HierarchyGroupObject _group = ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroup( EntityClassType.Turret );
				if( _group != null && _group.GroupTransform != null )
					_turrets = _group.GroupTransform.childCount;
			}

			ICEEditorLayout.BeginHorizontal();
		//m_Ground = (GameObject)EditorGUILayout.ObjectField( "Ground Object", m_Ground, typeof(GameObject), true );
				TurrentCount = (int)ICEEditorLayout.DefaultSlider( "Turrets" + ( _turrets > 0 ? " (" + _turrets + ")" : "" ), "", TurrentCount , 1, 0, 100, 25 );
				GUILayout.Space( 2 );
				if( ICEEditorLayout.ButtonSmall( "DEL", "" ))
					RemoveObjects( EntityClassType.Turret );
				if( ICEEditorLayout.ButtonSmall( "ADD", "" ))
					SpawnRandomTurrets( m_GroundGameObject, TurrentCount );	
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_TURRETS );

			int _locations = 0;
			if( ICECreatureRegister.Instance )
			{
				HierarchyGroupObject _group = ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroup( EntityClassType.Location );
				if( _group != null && _group.GroupTransform != null )
					_locations = _group.GroupTransform.childCount;
			}

			ICEEditorLayout.BeginHorizontal();
				LocationCount = (int)ICEEditorLayout.DefaultSlider( "Locations" + ( _locations > 0 ? " (" + _locations + ")" : "" ), "", LocationCount , 1, 0, 100, 25 );
				GUILayout.Space( 2 );
				if( ICEEditorLayout.ButtonSmall( "DEL", "" ) )
					RemoveObjects( EntityClassType.Location );
				if( ICEEditorLayout.ButtonSmall( "ADD", "" ) )
					SpawnRandomLocations( m_GroundGameObject, LocationCount );	
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_LOCATIONS );

			int _waypoints = 0;
			if( ICECreatureRegister.Instance )
			{
				HierarchyGroupObject _group = ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroup( EntityClassType.Waypoint );
				if( _group != null && _group.GroupTransform != null )
					_waypoints = _group.GroupTransform.childCount;
			}

			ICEEditorLayout.BeginHorizontal();
			WaypointCount = (int)ICEEditorLayout.DefaultSlider( "Waypoints" + ( _waypoints > 0 ? " (" + _waypoints + ")" : "" ), "", WaypointCount , 1, 0, 100, 25 );
			GUILayout.Space( 2 );
			if( ICEEditorLayout.ButtonSmall( "DEL", "" ) )
				RemoveObjects( EntityClassType.Waypoint );
			if( ICEEditorLayout.ButtonSmall( "ADD", "" ) )
				SpawnRandomWaypoints( m_GroundGameObject, WaypointCount );	
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_WAYPOINTS );

			int _shelters = 0;
			if( ICECreatureRegister.Instance )
			{
				HierarchyGroupObject _group = ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroup( EntityClassType.Shelter );
				if( _group != null && _group.GroupTransform != null )
					_shelters = _group.GroupTransform.childCount;
			}

			ICEEditorLayout.BeginHorizontal();
			ShelterCount = (int)ICEEditorLayout.DefaultSlider( "Shelter" + ( _shelters > 0 ? " (" + _shelters + ")" : "" ), "", ShelterCount , 1, 0, 100, 25 );
			GUILayout.Space( 2 );
			if( ICEEditorLayout.ButtonSmall( "DEL", "" ) )
				RemoveObjects( EntityClassType.Shelter );
			if( ICEEditorLayout.ButtonSmall( "ADD", "" ) )
				SpawnRandomShelters( m_GroundGameObject, ShelterCount );	
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_SHELTERS );

			int _obstacles = 0;
			if( ICECreatureRegister.Instance )
			{
				HierarchyGroupObject _group = ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroup( EntityClassType.Object );
				if( _group != null && _group.GroupTransform != null )
					_obstacles = _group.GroupTransform.childCount;
			}

			ICEEditorLayout.BeginHorizontal();
			ObstacleCount = (int)ICEEditorLayout.DefaultSlider( "Obstacles" + ( _obstacles > 0 ? " (" + _obstacles + ")" : "" ), "", ObstacleCount , 1, 0, 1000, 100 );
			GUILayout.Space( 2 );
			if( ICEEditorLayout.ButtonSmall( "DEL", "" ) )
				RemoveObjects( EntityClassType.Object );
			if( ICEEditorLayout.ButtonSmall( "ADD", "" ) )
				SpawnRandomObstacles( m_GroundGameObject, ObstacleCount );	
			ICEEditorLayout.EndHorizontal( Info.WIZARD_TERRAIN_OBSTACLES );
		}


		/// <summary>
		/// Draws the scene settings.
		/// </summary>
		private void DrawScene()
		{
			// SCENE BEGIN
			GUI.backgroundColor = (CheckGround()?Color.green:Color.yellow);
			EditorGUI.BeginDisabledGroup ( CheckRegister() == false );
			m_ShowGroundHandling = ICEEditorLayout.CheckButtonExtraLarge( "SCENE", "" ,m_ShowGroundHandling );
			EditorGUI.EndDisabledGroup();
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if( m_ShowGroundHandling )
			{
				m_ShowCreatureHandling = false;
				m_ShowPlayerHandling = false;
			}

			if( m_ShowGroundHandling && CheckRegister() == true )
			{
				if( LayerMask.NameToLayer( "WalkableSurface" ) != -1 && LayerMask.NameToLayer( "Obstacle" ) != -1 )
				{
					//CreatureEditorLayout.DrawGroundCheck( ref m_CreatureRegister.Options.GroundCheck, m_CreatureRegister.Options.GroundLayer.Layers, false );
					//CreatureEditorLayout.DrawWaterCheck( ref m_CreatureRegister.Options.WaterCheck, m_CreatureRegister.Options.WaterLayer.Layers, false );
					//CreatureEditorLayout.DrawObstacleCheck( ref m_CreatureRegister.Options.ObstacleCheck, m_CreatureRegister.Options.ObstacleLayer.Layers, false );

					if( m_GroundGameObject != null && m_GroundGameObject.layer == LayerMask.NameToLayer( "Default" ) )
						m_GroundGameObject.layer = LayerMask.NameToLayer( "WalkableSurface" ); 

					if( ICECreatureRegister.Instance.Options.GroundCheck == GroundCheckType.NONE )
						ICECreatureRegister.Instance.Options.GroundCheck = GroundCheckType.RAYCAST;
					if( ICECreatureRegister.Instance.Options.GroundLayer.Layers.Count == 0 )
						ICECreatureRegister.Instance.Options.GroundLayer.Layers.Add( "WalkableSurface" );

					if( ICECreatureRegister.Instance.Options.ObstacleCheck == ObstacleCheckType.NONE )
						ICECreatureRegister.Instance.Options.ObstacleCheck = ObstacleCheckType.BASIC;
					if( ICECreatureRegister.Instance.Options.ObstacleLayer.Layers.Count == 0 )
						ICECreatureRegister.Instance.Options.ObstacleLayer.Layers.Add( "Obstacle" );
							
					GUI.backgroundColor = Color.green;
				}
				else
					GUI.backgroundColor = Color.yellow;
				
				EditorGUI.BeginDisabledGroup( GUI.backgroundColor == Color.green );
					if( ICEEditorLayout.ButtonExtraLarge( "Create Ground and Obstacle Layer", "" ))
					{
						if( EditorTools.AddLayer( "WalkableSurface" ) && ICECreatureRegister.Instance.Options.GroundLayer.Layers.Count == 0 )
						{
							ICECreatureRegister.Instance.Options.GroundLayer.Layers.Add( "WalkableSurface" );
							ICECreatureRegister.Instance.Options.GroundCheck = GroundCheckType.RAYCAST;
						}
						if( EditorTools.AddLayer( "Obstacle" ) && ICECreatureRegister.Instance.Options.ObstacleLayer.Layers.Count == 0 )
						{
							ICECreatureRegister.Instance.Options.ObstacleLayer.Layers.Add( "Obstacle" );
							ICECreatureRegister.Instance.Options.ObstacleCheck = ObstacleCheckType.BASIC;
						}
							
					}
				EditorGUI.EndDisabledGroup();
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				if( m_GroundGameObject == null )
				{
					if( Terrain.activeTerrain != null )
						m_GroundGameObject = Terrain.activeTerrain.gameObject;
				}

				if( m_GroundGameObject == null )
				{
					GameObject[] _grounds = SystemTools.FindGameObjectsByLayer( LayerMask.NameToLayer( "WalkableSurface" ) );

					if( _grounds != null && _grounds.Length > 0 )
						m_GroundGameObject = _grounds[0];
				}



					ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( GUI.backgroundColor == Color.green );
						m_GroundGameObject = (GameObject)EditorGUILayout.ObjectField( "Ground Object", m_GroundGameObject, typeof(GameObject), true );

						EditorGUI.BeginDisabledGroup( m_GroundGameObject != null );
							if( m_GroundGameObject != null )
								GUI.backgroundColor = Color.green;
							else
								GUI.backgroundColor = Color.yellow;
			
							if( ICEEditorLayout.ButtonMiddle( "PLANE", "" ))
							{
								//Undo.RegisterUndo( m_Ground, "Old Ground");
								m_GroundGameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
								if( m_GroundGameObject != null )
								{
									Component.DestroyImmediate( m_GroundGameObject.GetComponent<MeshCollider>() );
									m_GroundGameObject.transform.localScale = new Vector3( 100, 1, 100 );
									m_GroundGameObject.AddComponent<BoxCollider>();

									Material _material = (Material)Resources.Load("Materials/GROUND_100x100_green", typeof(Material));
									Renderer _renderer =  m_GroundGameObject.GetComponent<Renderer>();

									if( _material != null && _renderer != null )
									{
										_renderer.material = _material;
									}
								}
							}
							if( ICEEditorLayout.ButtonMiddle( "TERRAIN", "" ))
							{
								m_GroundGameObject = TerrainTools.CreateTerrain();

								if( m_GroundGameObject != null && m_GroundGameObject.GetComponent<Terrain>() != null )
								{
									Vector3 _s = m_GroundGameObject.GetComponent<Terrain>().terrainData.size;
									m_GroundGameObject.transform.position = new Vector3( _s.x / -2, 0, _s.z / -2 );
									UpdatePlayer();
								}							
							}

							EditorGUI.EndDisabledGroup();

							GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

							if( m_GroundGameObject != null )
								GUI.backgroundColor = Color.red;
		
							EditorGUI.BeginDisabledGroup( m_GroundGameObject == null );
								if( ICEEditorLayout.ButtonMiddle( "REMOVE", "" ))
								{
									GameObject.DestroyImmediate( m_GroundGameObject );
								}
							EditorGUI.EndDisabledGroup();

							GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
						
					ICEEditorLayout.EndHorizontal();

				if(  m_GroundGameObject != null )
				{
					if( m_GroundGameObject.GetComponent<Terrain>() != null )
					{
						//GameObject _player = (m_CreaturePlayer != null ? m_CreaturePlayer.gameObject : null ); 
						//scrollPos = EditorGUILayout.BeginScrollView( scrollPos, GUILayout.Height (300) );
						//ICEEditorTerrain.DrawSceneTerrain( m_Ground, _player );
						//ICEEditorTerrain.DrawSceneHills( m_Ground, _player );
						//ICEEditorTerrain.DrawSceneValleys( m_Ground, _player );
						//ICEEditorTerrain.DrawSceneTrees( m_Ground, _player );
						//ICEEditorTerrain.DrawSceneGrass( m_Ground, _player );
						//EditorGUILayout.EndScrollView();
					}

					DrawSceneObjects();
				/*
					if( m_GroundGameObject.GetComponent<Terrain>() != null )
					{
						if( ICEEditorLayout.ButtonExtraLarge( "GENERATE", "" ))
						{
							if( m_GroundGameObject != null )
								GameObject.DestroyImmediate( m_GroundGameObject );

							m_GroundGameObject = TerrainTools.CreateTerrain();

							if( m_GroundGameObject != null )
							{
								if( m_GroundGameObject.GetComponent<Terrain>() != null )
								{
									Vector3 _s = m_GroundGameObject.GetComponent<Terrain>().terrainData.size;
									m_GroundGameObject.transform.position = new Vector3( _s.x / -2, 0, _s.z / -2 );
									UpdatePlayer();
								}
							}
						}
					}*/
					/*
					int _size = 70;
					ICEEditorLayout.BeginHorizontal();
					//if(  ICEEditorLayout.ButtonFlex( "TURRETS", "" , GUILayout.MinWidth( _size ) ))
						
					//if(  ICEEditorLayout.ButtonFlex( "LOCATIONS", "" , GUILayout.MinWidth( _size ) ))
					//	SpawnRandomLocations( m_Ground, 25 );
					//if( ICEEditorLayout.ButtonFlex( "WAYPOINTS", "" , GUILayout.MinWidth( _size ) ))
									
					if( ICEEditorLayout.ButtonFlex( "ITEMS", "" , GUILayout.MinWidth( _size ) ))
						SpawnRandomItems( m_Ground, 100 );
					if( ICEEditorLayout.ButtonFlex( "OBSTACLES", "" , GUILayout.MinWidth( _size ) ))
						SpawnRandomObstacles( m_Ground, 100 );
					//if( ICEEditorLayout.ButtonFlex( "WATER", "" , GUILayout.MinWidth( _size ) ))
					//	SpawnRandomWater( m_Ground, 10 );
					ICEEditorLayout.EndHorizontal();*/
				}
			}
			// SCENE END
		}

		/// <summary>
		/// Draws the player settings.
		/// </summary>
		private void DrawPlayer()
		{
			// PLAYER BEGIN
			GUI.backgroundColor = (CheckPlayer()?Color.green:Color.yellow);
			EditorGUI.BeginDisabledGroup ( CheckGround() == false );
			m_ShowPlayerHandling = ICEEditorLayout.CheckButtonExtraLarge( "PLAYER", "" ,m_ShowPlayerHandling );
			EditorGUI.EndDisabledGroup();

			if( m_ShowPlayerHandling )
			{
				m_ShowCreatureHandling = false;
				m_ShowGroundHandling = false;
			}

			if( m_ShowPlayerHandling && CheckRegister() == true && CheckGround() == true )
			{
				if( m_CreaturePlayerComponent != null )
					m_PlayerGameObject = m_CreaturePlayerComponent.gameObject;

				ICEEditorLayout.BeginHorizontal();
					m_PlayerGameObject = (GameObject)EditorGUILayout.ObjectField( "Player Object", m_PlayerGameObject, typeof(GameObject), true );

					EditorGUI.BeginDisabledGroup( GameObject.Find( "PlayerGroup" ) != null );
						if( ICEEditorLayout.ButtonMiddle( "DEFAULT", "Loads the default example player group" ) )
						{
							GameObject _cam = GameObject.Find( "Main Camera" );
							if( _cam )
								_cam.SetActive( false );
					
							GameObject _group_object = (GameObject)Resources.Load("Prefabs/PlayerGroup", typeof(GameObject));

							if( _group_object != null )
							{
								GameObject _player_group = _group_object;

								if( EditorTools.IsPrefab( _player_group ) )
								{
									_player_group = GameObject.Instantiate( _group_object );
									_player_group.name = _group_object.name;
									_player_group.transform.position = Vector3.zero;
								}

								ICECreaturePlayer _player = _player_group.GetComponentInChildren<ICECreaturePlayer>();
								if( _player != null )
								{
									m_PlayerGameObject = _player.gameObject;
									m_PlayerGameObject.transform.position = new Vector3( m_PlayerGameObject.transform.position.x, CreatureRegister.GetGroundLevel( m_PlayerGameObject.transform.position ) , m_PlayerGameObject.transform.position.z );
								}
							}
						}
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal();

				if( m_PlayerGameObject != null )
				{
					if( EditorTools.IsPrefab( m_PlayerGameObject ) )
					{
						m_PlayerGameObject = GameObject.Instantiate( m_PlayerGameObject );
						m_PlayerGameObject.name = m_PlayerGameObject.name;
						m_PlayerGameObject.transform.position = GetRandomGroundPosition( m_GroundGameObject, 2 );
					}

					if( m_PlayerGameObject.GetComponent<ICECreaturePlayer>() == null )
						m_PlayerGameObject.AddComponent<ICECreaturePlayer>();

					ReferenceGroupObject _group = m_CreatureRegisterComponent.AddReferenceAndReturnGroup( m_PlayerGameObject );
					m_CreatureRegisterComponent.HierarchyManagement.ReorganizeSceneObjects();

					// activate pool management for player, so the player can respawn after dying
					m_CreatureRegisterComponent.UsePoolManagement = true;
					if( _group != null )
					{
						// prepares the group for spawning the player
						_group.Enabled = true;
						_group.Foldout = true;
						_group.PoolManagementEnabled = true;
						_group.MinSpawnInterval = 0;
						_group.MaxSpawnInterval = 0;

						// creates an initial spawnpoint ...
						GameObject _spawnpoint = GameObject.Find( "HOME" );
						if( _spawnpoint == null )
							_spawnpoint = new GameObject( "HOME" );
	
						if( m_GroundGameObject != null &&  m_GroundGameObject.GetComponent<Terrain>() != null && m_GroundGameObject.GetComponent<Terrain>().terrainData != null )
							_group.SpawnPoints.Add( new SpawnPointObject( _spawnpoint, m_GroundGameObject.GetComponent<Terrain>().terrainData.size ) );
						else
							_group.SpawnPoints.Add( new SpawnPointObject( _spawnpoint, 0, 100 ) );
					}

					m_CreaturePlayerComponent = m_PlayerGameObject.GetComponent<ICECreaturePlayer>();

					m_PlayerGameObject.SetActive( false );
					Selection.activeGameObject = m_PlayerGameObject;
				}

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			}
			// PLAYER END
		}

		private void DrawCreature()
		{
			// CREATURES BEGIN

			GUI.backgroundColor = (CheckCreature()?Color.green:Color.yellow);
			EditorGUI.BeginDisabledGroup ( CheckGround() == false );
				m_ShowCreatureHandling = ICEEditorLayout.CheckButtonExtraLarge( "CREATURE", "" ,m_ShowCreatureHandling );
			EditorGUI.EndDisabledGroup();
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		

			if( m_ShowCreatureHandling )
			{
				m_ShowGroundHandling = false;
				m_ShowPlayerHandling = false;
			}

			if( m_ShowCreatureHandling && CheckRegister() == true && CheckGround() == true )
			{
				if( m_CreatureRegisterComponent != null )
				{
					if( m_CreatureControlComponent != null )
						m_CreatureGameObject = m_CreatureControlComponent.gameObject;


					ICEEditorLayout.BeginHorizontal();
						m_CreatureGameObject = (GameObject)EditorGUILayout.ObjectField( "Creature Object", m_CreatureGameObject, typeof(GameObject), true );

						EditorGUI.BeginDisabledGroup ( m_CreatureControlComponent == null );
							if( ICEEditorLayout.ResetButton( "" ) )
							{
								if( m_CreatureControlComponent != null )
									m_CreatureControlComponent.Creature.Reset();
							}
						EditorGUI.EndDisabledGroup();
						
					ICEEditorLayout.EndHorizontal();


					// return while _creature is null
					if( m_CreatureGameObject == null )
						return;
	
					// instatiate a scene object of the creature if required
					if( EditorTools.IsPrefab( m_CreatureGameObject ) )
					{
						GameObject _creature = GameObject.Instantiate( m_CreatureGameObject );
						_creature.name = m_CreatureGameObject.name;
						_creature.transform.position = GetRandomGroundPosition( m_GroundGameObject, 0 );

						// set the new creature as current creature
						m_CreatureGameObject = _creature;	
					}

					// actiavte the scene creature
					if( m_CreatureGameObject != null && m_CreatureGameObject != Selection.activeGameObject )
						Selection.activeGameObject = m_CreatureGameObject;


					if( m_CreatureGameObject.GetComponent<ICECreatureControl>() == null )
						m_CreatureGameObject.AddComponent<ICECreatureControl>();

					m_CreatureRegisterComponent.AddReference( m_CreatureGameObject );

					m_CreatureControlComponent = m_CreatureGameObject.GetComponent<ICECreatureControl>();
					if( m_CreatureControlComponent != null )
					{
						m_CreatureControlComponent.Display.ShowDebug = true;

						ICEEditorLayout.BeginHorizontal();
						m_CreatureControlComponent.Creature.Essentials.TrophicLevel = (TrophicLevelType)ICEEditorLayout.EnumPopup( "Trophic Level","", m_CreatureControlComponent.Creature.Essentials.TrophicLevel ); 
						EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.TrophicLevel == TrophicLevelType.UNDEFINED );
							m_CreatureControlComponent.Creature.Essentials.UseAutoDetectInteractors = ICEEditorLayout.CheckButtonLarge( "INTERACTORS", "Detects and prepares automatically potential interactors", m_CreatureControlComponent.Creature.Essentials.UseAutoDetectInteractors );
						EditorGUI.EndDisabledGroup();
						ICEEditorLayout.EndHorizontal();
							m_CreatureControlComponent.Creature.Move.MotionControl = MotionControlType.INTERNAL;
							//m_CreatureControl.Creature.Characteristics.MotionControl = (MotionControlType)ICEEditorLayout.EnumPopup("Motion Control","", m_CreatureControl.Creature.Characteristics.MotionControl );
							m_CreatureControlComponent.Creature.Essentials.GroundOrientation = (BodyOrientationType)ICEEditorLayout.EnumPopup("Body Orientation", "Vertical direction relative to the ground", m_CreatureControlComponent.Creature.Essentials.GroundOrientation );

						EditorGUILayout.Separator();

						ICEEditorLayout.Label( "Desired Speed", false );
						EditorGUI.indentLevel++;
						m_CreatureControlComponent.Creature.Essentials.DefaultRunningSpeed = ICEEditorLayout.DefaultSlider( "Running","", m_CreatureControlComponent.Creature.Essentials.DefaultRunningSpeed, 0.25f ,0,25, 4 );
						m_CreatureControlComponent.Creature.Essentials.DefaultWalkingSpeed = ICEEditorLayout.DefaultSlider( "Walking","", m_CreatureControlComponent.Creature.Essentials.DefaultWalkingSpeed, 0.25f ,0,25, 1.5f );
						m_CreatureControlComponent.Creature.Essentials.DefaultTurningSpeed = ICEEditorLayout.DefaultSlider( "Turning","", m_CreatureControlComponent.Creature.Essentials.DefaultTurningSpeed, 0.25f ,0,25, 2 );
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();

						if( AnimationTools.HasAnimations( m_CreatureControlComponent.gameObject ) )
						{
							ICEEditorLayout.Label( "Desired Animations", false );
							EditorGUI.indentLevel++;

							ICEEditorLayout.BeginHorizontal();
								EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationIdle == true );
									WizardEditor.WizardAnimationPopup( "Idle", m_CreatureControlComponent, m_CreatureControlComponent.Creature.Essentials.AnimationIdle );
								EditorGUI.EndDisabledGroup();
								m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationIdle = ICEEditorLayout.IgnoreButton( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationIdle );
							ICEEditorLayout.EndHorizontal();

							ICEEditorLayout.BeginHorizontal();
								EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationWalk == true );
									WizardEditor.WizardAnimationPopup( "Walk", m_CreatureControlComponent, m_CreatureControlComponent.Creature.Essentials.AnimationWalk );
								EditorGUI.EndDisabledGroup();
								m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationWalk = ICEEditorLayout.IgnoreButton( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationWalk );
							ICEEditorLayout.EndHorizontal();

							ICEEditorLayout.BeginHorizontal();
								EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationRun == true );
									WizardEditor.WizardAnimationPopup( "Run", m_CreatureControlComponent, m_CreatureControlComponent.Creature.Essentials.AnimationRun );
								EditorGUI.EndDisabledGroup();
								m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationRun = ICEEditorLayout.IgnoreButton( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationRun );
							ICEEditorLayout.EndHorizontal();

							ICEEditorLayout.BeginHorizontal();
								EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationAttack == true );
									WizardEditor.WizardAnimationPopup( "Attack", m_CreatureControlComponent, m_CreatureControlComponent.Creature.Essentials.AnimationAttack );
								EditorGUI.EndDisabledGroup();
								m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationAttack = ICEEditorLayout.IgnoreButton( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationAttack );
							ICEEditorLayout.EndHorizontal();

							ICEEditorLayout.BeginHorizontal();
								EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationImpact == true );
									WizardEditor.WizardAnimationPopup( "Impact", m_CreatureControlComponent, m_CreatureControlComponent.Creature.Essentials.AnimationImpact );
								EditorGUI.EndDisabledGroup();
								m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationImpact = ICEEditorLayout.IgnoreButton( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationImpact );
							ICEEditorLayout.EndHorizontal();

							ICEEditorLayout.BeginHorizontal();
								EditorGUI.BeginDisabledGroup( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationDead == true );
									WizardEditor.WizardAnimationPopup( "Die", m_CreatureControlComponent, m_CreatureControlComponent.Creature.Essentials.AnimationDead );
								EditorGUI.EndDisabledGroup();
								m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationDead = ICEEditorLayout.IgnoreButton( m_CreatureControlComponent.Creature.Essentials.IgnoreAnimationDead );
							ICEEditorLayout.EndHorizontal();

							EditorGUI.indentLevel--;
							EditorGUILayout.Separator();
						}
						else
						{
							if( m_CreatureGameObject.GetComponentInChildren<Animator>() != null && m_CreatureGameObject.GetComponentInChildren<Animator>().runtimeAnimatorController == null ) 
							{
								m_CreatureGameObject.GetComponentInChildren<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField( "Animator Controller", null, typeof(RuntimeAnimatorController), false );
							}
							else
							{
								ICEEditorLayout.BeginHorizontal();
								//GUILayout.FlexibleSpace();
								EditorGUILayout.HelpBox( "No Mecanim or Legacy animations available. Please check your creature!", MessageType.Info );
								//EditorGUILayout.LabelField( "- No Mecanim or Legacy animations available -" );
								//GUILayout.FlexibleSpace();
								ICEEditorLayout.EndHorizontal();
								EditorGUILayout.Separator();
							}
						}

						// GENERATE BEGIN
						EditorGUI.BeginDisabledGroup( Application.isPlaying == true );
						ICEEditorLayout.BeginHorizontal();
						if( ICEEditorLayout.ButtonExtraLarge( "GENERATE" ) )
						{
							WizardEditor.WizardGenerate( m_CreatureControlComponent );
							Selection.activeGameObject = m_CreatureControlComponent.gameObject;
						}
						ICEEditorLayout.EndHorizontal();
						EditorGUI.EndDisabledGroup();
						// GENERATE END

					}
				}

			}


			// CREATURES END
		}
			
		#endregion

	
	}
}
