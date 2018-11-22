// ##############################################################################
//
// ICECreatureRegister.cs
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects{

	[System.Serializable]
	public class ReferenceGroupCategory : ICEInfoDataObject 
	{
		public EntityClassType Type = EntityClassType.Undefined;

		public List<ReferenceGroupObject> ReferenceGroupObjects = null;
	}

	[System.Serializable]
	public class CreatureRegisterDebugObject : ICEDataObject 
	{
		public bool UseDrawSelected = false;
		public Color ColorReferences = Color.red;
		public Color ColorClones = Color.blue;
		public Color ColorSpawnPoints = Color.cyan;
		public bool ShowReferenceGizmos = true;
		public bool ShowCloneGizmos = true;
		public bool ShowSpawnPointGizmos = true;
		public bool ShowReferenceGizmosText = true;
		public bool ShowCloneGizmosText = true;
		public bool ShowSpawnPointGizmosText = true;
	}
}

namespace ICE.Creatures{
	
	public class ICECreatureRegister : ICEWorldRegister 
	{

		[SerializeField]
		private HierarchyManagementObject m_HierarchyManagement = null;
		public HierarchyManagementObject HierarchyManagement{
			get{ return m_HierarchyManagement = ( m_HierarchyManagement == null ? new HierarchyManagementObject(this):m_HierarchyManagement ); }
			set{ HierarchyManagement.Copy( value ); }
		}

		[SerializeField]
		private CreatureRegisterDebugObject m_RegisterDebug = null;
		public CreatureRegisterDebugObject RegisterDebug{
			get{ return m_RegisterDebug = ( m_RegisterDebug == null ? new CreatureRegisterDebugObject():m_RegisterDebug ); }
			set{ RegisterDebug.Copy( value ); }
		}
		/*
		[SerializeField]
		private ReferenceGroupsObject m_References = null;
		public ReferenceGroupsObject References{
			get{ return m_References = ( m_References == null ? new ReferenceGroupsObject(this):m_References ); }
			set{ References.Copy( value ); }
		}*/

		public bool ShowReferencesInfo = false;
		public string ReferencesInfo = "";

		// ################################################################################
		// EVENTS
		// ################################################################################

		/// <summary>
		/// Defines the delegate usable in OnInitialSpawnComplete.
		/// </summary>
		public delegate void OnInitialSpawnCompleteEvent();

		/// <summary>
		/// Occurs when on initial spawn complete.
		/// </summary>
		public event OnInitialSpawnCompleteEvent OnInitialSpawnComplete;


		// ################################################################################
		// STATIC REGISTER MEMBERS
		// ################################################################################

		//Here is a private reference only this class can access
		private static new ICECreatureRegister m_Instance = null;
		public static new ICECreatureRegister Instance
		{
			get
			{
				//If m_Register hasn't been set yet, we grab it from the scene!
				//This will only happen the first time this reference is used.
				if( m_Instance == null )
					m_Instance = GameObject.FindObjectOfType<ICECreatureRegister>();

				if( m_Instance == null )
				{
					GameObject _register = GameObject.Find( "CreatureRegister" );
					if( _register != null )
					{
						_register.SetActive( true );
						m_Instance = _register.GetComponent<ICECreatureRegister>();
					}
				}

				return m_Instance;

		
			}
		}

		/// <summary>
		/// Creates a new creature register instance.
		/// </summary>
		public static ICECreatureRegister Create()
		{
			if( m_Instance == null )
			{
				GameObject _object = new GameObject( "CreatureRegister" );
				//_object.name = "CreatureRegister";
				m_Instance = _object.AddComponent<ICECreatureRegister>();

				if( m_Instance != null )
					m_Instance.UpdateReferences();
			}

			return m_Instance;
		}

		public static bool Exists()
		{
			if( m_Instance != null )
				return true;
			else
				return false;
		}


		// ################################################################################
		// STATIC REGISTER MEMBERS
		// ################################################################################

		public override void Awake () 
		{
			if( m_Instance == null )
				m_Instance = this;
			else if( m_Instance != this )
			{
				ICEDebug.LogWarning( "Multiple ICECreatureRegisters was detected in scene, this is not supported and the redundant one (" + gameObject.name + ") will be removed!" );
				Destroy( gameObject ); 
			}

			transform.parent = null;

			HierarchyManagement.Init( this );

			if( UseDontDestroyOnLoad ) 
				DontDestroyOnLoad( transform.root.gameObject );
			
#if UNITY_5_4 || UNITY_5_4_OR_NEWER
				if( RandomSeed == RandomSeedType.CUSTOM )
					UnityEngine.Random.InitState( CustomRandomSeed );
				else if( RandomSeed == RandomSeedType.TIME )
					UnityEngine.Random.InitState( (int)System.DateTime.Now.Second );
#else
				if( RandomSeed == RandomSeedType.CUSTOM )
					UnityEngine.Random.seed = CustomRandomSeed;
				else if( RandomSeed == RandomSeedType.TIME )
					UnityEngine.Random.seed = (int)System.DateTime.Now.Second;
#endif
			}

		public override void OnDestroy() {
			m_Instance = null;
		}

		public override void Start () 
		{
			if( IsRemoteClient )
				return;
			
			HierarchyManagement.Init( this );

			// initial start of the update coroutine (if required)
			if( UsePoolManagementCoroutine )
			{
				if( m_PoolManagementCoroutineIsRunning == false )
					StartCoroutine( PoolManagementSpawnCoroutine() );
			}
		}

		public override void OnEnable() {

			base.OnEnable();

			if( IsRemoteClient )
				return;

			// initial start of the update coroutine (if required)
			if( UsePoolManagementCoroutine )
			{
				if( m_PoolManagementCoroutineIsRunning == false )
					StartCoroutine( PoolManagementSpawnCoroutine() );
			}
		}

		public override void OnDisable() {

			base.OnDisable();

			if( IsRemoteClient )
				return;
			
			// deactivating the gameobject will stopping the coroutine, so we capture the ondisable 
			// and stops the coroutine controlled ... btw. if only the script was disabled, the coroutine would be 
			// still running, but we don't need the coroutine if the rest of the script isn't running and so we 
			// capture all cases
			if( UsePoolManagementCoroutine )
			{
				StopCoroutine( PoolManagementSpawnCoroutine() );
			}

			m_PoolManagementCoroutineIsRunning = false;
		}


		//################################################################################
		// SPAWN HANDLER
		//################################################################################


		private bool m_InitialSpawnComplete = false;
		public bool InitialSpawnComplete{
			get{ return m_InitialSpawnComplete; }
		}

		public override void Update () 
		{
			base.Update();

			if( ! UsePoolManagementCoroutine )
				DoPoolManagementSpawn();
			else if( gameObject.activeInHierarchy && m_PoolManagementCoroutineIsRunning == false )
				StartCoroutine( PoolManagementSpawnCoroutine() );
		}

	
		IEnumerator PoolManagementSpawnCoroutine()
		{
			while( UsePoolManagementCoroutine )
			{
				// coroutine is alive ... 
				m_PoolManagementCoroutineIsRunning = true;

				DoPoolManagementSpawn();

				yield return null;
			}		

			m_PoolManagementCoroutineIsRunning = false;
			yield break;
		}

		protected void DoPoolManagementSpawn()
		{
			if( ! UsePoolManagement || ! NetworkConnectedAndReady )
				return;

			if( ! m_InitialSpawnComplete )
			{
				List<ReferenceGroupObject> _groups = ReferenceGroupObjects.OrderByDescending( _item => _item.InitialSpawnPriority ).ToList();

				for( int i = 0 ; i < _groups.Count ; i++ )
				{
					ReferenceGroupObject _group = _groups[i];
					if( _group != null ) 
						_group.TryPoolManagementInitialSpawn();
				}

				m_InitialSpawnComplete = true;

				if( OnInitialSpawnComplete != null )
					OnInitialSpawnComplete();
			}
			else
			{
				for( int i = 0 ; i < ReferenceGroupObjects.Count ; i++ )
				{
					ReferenceGroupObject _group = ReferenceGroupObjects[i];
					if( _group != null )
						_group.TryPoolManagementSpawn();
				}
			}
		}
			
		[SerializeField]
		private bool m_BreakAll = false;
		public bool BreakAll{
			set{
				if( m_BreakAll != value )
				{
					m_BreakAll = value;
					foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
					{
						if( _group.EntityType != EntityClassType.Player )
							_group.Break = m_BreakAll;
					}
				}
			}
			get{return m_BreakAll;}
		}

		/// <summary>
		/// Reset destroys all items and enables the initial spawning if required
		/// </summary>
		public void Reset()
		{
			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				_group.Reset();
		}

		public List<ReferenceGroupObject> ReferenceGroupObjects = new List<ReferenceGroupObject>();
		public bool UseReferenceCategories = false;

		[SerializeField]
		private List<ReferenceGroupCategory> m_ReferenceGroupCategories = null;
		public List<ReferenceGroupCategory> ReferenceGroupCategories{
			get{
				if( m_ReferenceGroupCategories == null || m_ReferenceGroupCategories.Count != System.Enum.GetValues( typeof(EntityClassType) ).Length )
				{
					m_ReferenceGroupCategories = new List<ReferenceGroupCategory>();
					foreach( EntityClassType _type in System.Enum.GetValues( typeof(EntityClassType) ) )
					{
						ReferenceGroupCategory _cat = new ReferenceGroupCategory();

						_cat.Type = _type;
						_cat.Foldout = true;
						_cat.ReferenceGroupObjects = GetReferenceGroupObjectsByType( _type );

						m_ReferenceGroupCategories.Add( _cat );
					}
				}
				else
				{
					foreach( ReferenceGroupCategory _cat in m_ReferenceGroupCategories )
						_cat.ReferenceGroupObjects = GetReferenceGroupObjectsByType( _cat.Type );
				}

				return m_ReferenceGroupCategories;
			}
		}

		public List<ReferenceGroupObject> GetReferenceGroupObjectsByType( EntityClassType _type ){

			List<ReferenceGroupObject> _list = new List<ReferenceGroupObject>();

			for( int _index = 0 ; _index < ReferenceGroupObjects.Count ; _index++ )
			{
				ReferenceGroupObject _group = ReferenceGroupObjects[_index];

				if( _group != null && _group.ReferenceGameObject != null )
				{
					if( _group.EntityType != _type )
						continue;

					_list.Add( _group );
				}
				else
				{
					ReferenceGroupObjects.RemoveAt(_index);
					--_index;
				}
			}

			return _list;
		}

		// ################################################################################
		// PUBLIC REGISTER MEMBERS (STATIC CALLS AVAILABLE)
		// ################################################################################

		/// <summary>
		/// Register the specified _object.
		/// </summary>
		/// <param name="_object">_object.</param>
		public new ReferenceGroupObject Register( GameObject _object )
		{
			if( _object == null )
				return null;

			ReferenceGroupObject _group = ForceGroup( _object );
			if( _group != null )
				_group.Register( _object );

			return _group;
		}


		
		/// <summary>
		/// Deregister the specified _object.
		/// </summary>
		/// <param name="_object">_object.</param>
		public override bool Deregister( GameObject _object )
		{
			if( _object == null )
				return false;

			ReferenceGroupObject _group = GetGroup( _object );
			if( _group != null )
				return _group.Deregister( _object );
			else 
				return false;
		}

		public override bool AttachToTransform( GameObject _object, Transform _transform, bool _use_transform_position )
		{
			if( _object == null )
				return false;

			ReferenceGroupObject _group = GetGroup( _object );
			if( _group != null )
				return _group.AttachToTransform( _object, _transform, _use_transform_position );

			return false;
		}

		public override bool DetachFromTransform( GameObject _object )
		{
			if( _object == null )
				return false;

			ReferenceGroupObject _group = GetGroup( _object );
			if( _group != null )
				return _group.DetachFromTransform( _object );

			return false;
		}

		/// <summary>
		/// Spawns the specified reference by using the defined position and rotation. This method overrides 
		/// the ICEWorldRegister.Spawn method to handle the reuse of suspended objects.
		/// </summary>
		/// <param name="_reference">Reference.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public override GameObject Spawn( GameObject _reference, Vector3 _position, Quaternion _rotation )
		{
			if( _reference == null )
				return null;

			GameObject _object = null;

			// try to spawn the object by using an external script
			if( ! TryCustomSpawn( out _object, _reference, _position, _rotation ) )
			{
				// try to recyle a suspended object
				ReferenceGroupObject _group = GetGroup( _reference );
				if( _group != null )
				{
					_object = _group.ActivateSuspendedObject( _position, _rotation );

					// instantiate a new one if there was no suspended one available
					if( _object == null )
						_object = _group.InstantiateNewObject( _position, _rotation );
				}

				// instantiate a new one if the group 
				if( _object == null )
					_object = WorldManager.Instantiate( _reference, _position, _rotation );
			}
				

			return _object;
		}

		public override bool Remove( GameObject _object )
		{
			if( _object == null )
				return false;

			bool _removed = false;
			ReferenceGroupObject _group = GetGroup( _object );
			if( _group != null )
				_removed = _group.Remove( _object );
			else
				_removed = base.Remove( _object );

			return _removed;
		}

		public void SendGroupMessage( ReferenceGroupObject _sender_group, GameObject _sender, BroadcastMessageDataObject _msg )
		{
			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				_group.Message( _sender_group, _sender, _msg );
		}

		// ################################################################################
		// PUBLIC REGISTER MEMBERS (STATIC CALLS AVAILABLE)
		// ################################################################################

		private ReferenceGroupObject ForceGroup( GameObject _object )
		{
			ReferenceGroupObject _group = GetGroup( _object );

			if( _group == null && ! IsListedAsReference( _object ) )
			{
				_group = new ReferenceGroupObject( _object );
				ReferenceGroupObjects.Add( _group );
			}

			return _group;
		}

		private ReferenceGroupObject GetGroup( GameObject _object )
		{
			if( _object == null )
				return null;
			
			return GetBufferedGroupByName( _object.name );
		}

		#region Buffered Reference Groups

		private Dictionary<string, ReferenceGroupObject> m_ReferenceGroupKeyDictonary = new Dictionary<string, ReferenceGroupObject>();
		private Dictionary<string, ReferenceGroupObject> m_ReferenceGroupNameDictonary = new Dictionary<string, ReferenceGroupObject>();
		private Dictionary<string, ReferenceGroupBuffer> m_ReferenceGroupTagDictonary = new Dictionary<string, ReferenceGroupBuffer>();
		private Dictionary<EntityClassType, ReferenceGroupBuffer> m_ReferenceGroupTypeDictonary = new Dictionary<EntityClassType, ReferenceGroupBuffer>();

		private class ReferenceGroupBuffer
		{
			private List<ReferenceGroupObject> m_Groups = null;
			public List<ReferenceGroupObject> Groups{
				get{ return m_Groups = ( m_Groups == null ? new List<ReferenceGroupObject>() : m_Groups ); }
			}
		}

		public ReferenceGroupObject GetBufferedGroupByObject( GameObject _object )
		{
			if( _object == null )
				return null;

			string _name = SystemTools.CleanName( _object.name );
			string _id = _object.GetInstanceID().ToString();
			string _key = _name + _id;

			ReferenceGroupObject _found_group = null;

			if( m_ReferenceGroupKeyDictonary.TryGetValue( _key, out _found_group ) )
				return _found_group;
			else
			{
				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group.CompareByName( _name ) )
					{
						m_ReferenceGroupNameDictonary.Add( _key , _group );
						return _group;
					}
				}
			}

			return null;
		}

		public ReferenceGroupObject GetBufferedGroupByName( string _name )
		{
			if( string.IsNullOrEmpty( _name ) )
				return null;

			_name = SystemTools.CleanName( _name );
			
			if( m_ReferenceGroupNameDictonary.Count != ReferenceGroupObjects.Count )
			{
				m_ReferenceGroupNameDictonary.Clear();

				for( int _index = 0 ; _index < ReferenceGroupObjects.Count ; _index++ )
				{
					ReferenceGroupObject _group = ReferenceGroupObjects[_index];

					if( _group != null && _group.ReferenceGameObject != null && ! string.IsNullOrEmpty( _group.Name ) )
					{
						string _clean_name = SystemTools.CleanName( _group.Name );

						if( ! m_ReferenceGroupNameDictonary.ContainsKey( _clean_name ) )
							m_ReferenceGroupNameDictonary.Add( _clean_name, _group );
					}
					else
					{
						ReferenceGroupObjects.RemoveAt(_index);
						--_index;
					}
				}
			}

			ReferenceGroupObject _found_group = null;

			if( m_ReferenceGroupNameDictonary.TryGetValue( _name, out _found_group ) )
				return _found_group;
			else
			{
				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group.CompareByName( _name ) )
					{
						m_ReferenceGroupNameDictonary.Add( _name , _group );
						return _group;
					}
				}
			}
			
			return null;
		}
			
		private List<ReferenceGroupObject> GetBufferedGroupsByTag( string _tag )
		{
			if( string.IsNullOrEmpty( _tag ) )
				return null;
			
			ReferenceGroupBuffer _buffer = null;

			if( m_ReferenceGroupTagDictonary.TryGetValue( _tag, out _buffer ) )
				return ( _buffer != null ? _buffer.Groups : null );
			else
			{
				_buffer = new ReferenceGroupBuffer();
				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group.CompareByTag( _tag ) )
						_buffer.Groups.Add( _group );
				}

				m_ReferenceGroupTagDictonary.Add( _tag , _buffer );

				return ( _buffer != null ? _buffer.Groups : null );
			}
		}

		private List<ReferenceGroupObject> GetBufferedGroupsByType( EntityClassType _type )
		{
			ReferenceGroupBuffer _buffer = null;

			if( m_ReferenceGroupTypeDictonary.TryGetValue( _type, out _buffer ) )
				return ( _buffer != null ? _buffer.Groups : null );
			else
			{
				_buffer = new ReferenceGroupBuffer();
				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group.CompareByType( _type ) )
						_buffer.Groups.Add( _group );
				}

				m_ReferenceGroupTypeDictonary.Add( _type , _buffer );

				return ( _buffer != null ? _buffer.Groups : null );
			}
		}

		#endregion

		//################################################################################
		// RANDOM RUNTIME TARGETS
		//################################################################################



		/// <summary>
		/// Gets the random name of the target by.
		/// </summary>
		/// <description>GetRandomTargetByTag will be called by a ReferenceGroupObject inside GetSpawnPosition()</description>
		/// <returns>The random target by name.</returns>
		/// <param name="_name">Name.</param>
		public GameObject GetRandomTargetByName( string _name )
		{
			if( string.IsNullOrEmpty( _name ) )
				return null;
			
			GameObject _target_game_object = null;

			ReferenceGroupObject _group = GetBufferedGroupByName( _name );
			if( _group != null )
				_target_game_object = _group.GetRandomObject();

			// BACKUP SEARCH
			if( _target_game_object == null && _name != "" )
			{
				_target_game_object = ObjectTools.GetRandomObjectByName( _name );
				if( _target_game_object == null )
					_target_game_object = ObjectTools.GetRandomObjectByName( _name + "(Clone)" );
			}

			return _target_game_object;
		}
			
		/// <summary>
		/// Gets the random target by tag.
		/// </summary>
		/// <description>GetRandomTargetByTag will be called by a ReferenceGroupObject inside GetSpawnPosition()</description>
		/// <returns>The random target by tag.</returns>
		/// <param name="_tag">Tag.</param>
		public GameObject GetRandomTargetByTag( string _tag )
		{
			if( string.IsNullOrEmpty( _tag ) )
				return null;
			
			GameObject _target_game_object = null;

			List<ReferenceGroupObject> _groups = GetBufferedGroupsByTag( _tag );
			if( _groups != null )
			{
				if( _groups.Count == 1 )
					_target_game_object = _groups[0].GetRandomObject();
				else if( _groups.Count > 1 )
					_target_game_object = _groups[ Random.Range(0, _groups.Count) ].GetRandomObject();					
			}

			// BACKUP SEARCH
			if( _target_game_object == null && _tag != "" )
				_target_game_object = ObjectTools.GetRandomObjectByTag( _tag );

			return _target_game_object;
		}

		//################################################################################
		// NEAREST RUNTIME TARGETS
		//################################################################################

		#region PRIMARY TARGET SEARCH

		/// <summary>
		/// Finds the nearest target by name.
		/// </summary>
		/// <description>FindBestObjectByName will be called directly from a target to update the TargetGameObject</description>
		/// <returns>The nearest target by name.</returns>
		/// <param name="_name">Name.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_distance">Distance.</param>
		/// <param name="_sender">Sender.</param>
		public GameObject FindBestObjectByName( GameObject _sender, string _name, float _range, int _max_counterparts, bool _prefer_counterpart, bool _allow_child )
		{
			if( _sender == null )
				return null;
			
			GameObject _object = null;
			ReferenceGroupObject _group = GetBufferedGroupByName( _name );
			if( _group != null )
			{
				_object = CreatureRegister.GetBestObject( _sender, _group.ActiveObjects.ToArray(), _range, _max_counterparts, _prefer_counterpart, _allow_child );

				if( _object == null && _group.EntityType == EntityClassType.Undefined && _group.EntityGameObject != null && _group.EntityGameObject.activeInHierarchy )
					_object = _group.EntityGameObject;
			}
			
			return _object;
		}

		/// <summary>
		/// Finds the nearest target by tag.
		/// </summary>
		/// <description>FindBestObjectByTag will be called directly from a target to update the TargetGameObject</description>
		/// <returns>The nearest target by tag.</returns>
		/// <param name="_tag">Tag.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_distance">Distance.</param>
		/// <param name="_sender">Sender.</param>
		public GameObject FindBestObjectByTag( GameObject _sender, string _tag, float _range, int _max_counterparts, bool _prefer_counterpart, bool _allow_child )
		{
			if( _sender == null )
				return null;

			GameObject _best_object = null;

			List<ReferenceGroupObject> _groups = GetBufferedGroupsByTag( _tag );
			if( _groups != null )
			{
				List<GameObject> _objects = new List<GameObject>( _groups.Count );
				for( int _i = 0 ; _i < _groups.Count() ; _i++ )
				{
					ReferenceGroupObject _group = _groups[_i];
					if( _group == null )
						continue;

					GameObject _object = CreatureRegister.GetBestObject( _sender, _group.ActiveObjects.ToArray(), _range, _max_counterparts, _prefer_counterpart, _allow_child );
					if( _object != null )
						_objects.Add( _object );
				}

				_best_object = CreatureRegister.GetBestObject( _sender, _objects.ToArray(), _range, _max_counterparts, _prefer_counterpart, _allow_child );
			}
			/*
			if( _best_object == null )
			{
				GameObject[] _objects = GameObject.FindGameObjectsWithTag( _tag );
				if( _objects != null )
				{
					_best_object = CreatureRegister.GetBestObject( _sender, _objects, _range, _max_counterparts, _prefer_counterpart, _allow_child );
				}
			}*/

			return _best_object;
		}
			
		/// <summary>
		/// Finds nearest target within the specified range by type
		/// </summary>
		/// <returns>The nearest target by type.</returns>
		/// <param name="_sender">Sender.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_range">Range.</param>
		/// <param name="_allow_child">If set to <c>true</c> allow child.</param>
		public GameObject FindBestObjectByType( GameObject _sender, EntityClassType _type, float _range, int _max_counterparts, bool _prefer_counterpart, bool _allow_child )
		{
			if( _sender == null )
				return null;

			GameObject _best_object = null;

			List<ReferenceGroupObject> _groups = GetBufferedGroupsByType( _type );
			if( _groups != null )
			{
				List<GameObject> _objects = new List<GameObject>( _groups.Count );
				for( int _i = 0 ; _i < _groups.Count() ; _i++ )
				{
					ReferenceGroupObject _group = _groups[_i];
					if( _group == null )
						continue;

					GameObject _object = CreatureRegister.GetBestObject( _sender, _group.ActiveObjects.ToArray(), _range, _max_counterparts, _prefer_counterpart, _allow_child );
					if( _object != null )
						_objects.Add( _object );
				}

				_best_object = CreatureRegister.GetBestObject( _sender, _objects.ToArray(), _range, _max_counterparts, _prefer_counterpart, _allow_child );
			}

			return _best_object;
		}

		#endregion

		#region SECONDARY TARGET SEARCH (TARGET PRESELECTION)

		public GameObject[] FindObjectsByName( string _name )
		{
			GameObject[] _objects = null;
			ReferenceGroupObject _group = GetBufferedGroupByName( _name );
			if( _group != null )
				_objects = _group.ActiveObjects.ToArray();

			return _objects;
		}

		public GameObject[] FindObjectsByTag( string _tag )
		{
			List<GameObject> _best_objects = new List<GameObject>();
			List<ReferenceGroupObject> _groups = GetBufferedGroupsByTag( _tag );
			if( _groups != null )
			{
				for( int _i = 0 ; _i < _groups.Count ; _i++ )
				{
					ReferenceGroupObject _group = _groups[_i];
					if( _group != null )
					{
						GameObject[] _objects = _group.ActiveObjects.ToArray();
						if( _objects != null )
						{
							for( int _o = 0 ; _o < _objects.Length ; _o++ )
							{
								if( _objects[_o] != null )
									_best_objects.Add( _objects[_o] );	
							}							
						}
					}
				}
			}

			return _best_objects.ToArray();
		}


		public GameObject[] FindObjectsByType( EntityClassType _type )
		{
			List<GameObject> _best_objects = new List<GameObject>();
			List<ReferenceGroupObject> _groups = GetBufferedGroupsByType( _type );
			if( _groups != null )
			{
				for( int _i = 0 ; _i < _groups.Count ; _i++ )
				{
					ReferenceGroupObject _group = _groups[_i];
					if( _group != null )
					{
						GameObject[] _objects = _group.ActiveObjects.ToArray();
						if( _objects != null )
						{
							for( int _o = 0 ; _o < _objects.Length ; _o++ )
							{
								if( _objects[_o] != null )
									_best_objects.Add( _objects[_o] );	
							}
						}
					}
				}
			}

			return _best_objects.ToArray();
		}

		#endregion


		//################################################################################
		// REFERENCE GAME OBJECTS
		//################################################################################

		/// <summary>
		/// Gets all the reference item names.
		/// </summary>
		/// <description>ReferenceItemNames will be used only to fill the item selection popup</description>
		/// <value>The reference item names.</value>
		public List<string> ReferenceItemNames
		{
			get{
				List<string> _keys = new List<string>();

				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group != null && _group.ReferenceGameObject != null && _group.ReferenceGameObject.GetComponent<ICECreatureItem>() != null )
						_keys.Add( _group.Name );
				}

				return _keys;
			}
		}

		public List<string> ReferencePlayerNames
		{
			get{
				List<string> _keys = new List<string>();

				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group != null && _group.ReferenceGameObject != null && _group.ReferenceGameObject.GetComponent<ICECreaturePlayer>() != null )
						_keys.Add( _group.Name );
				}

				return _keys;
			}
		}



		public List<string> ReferenceZoneNames
		{
			get{
				List<string> _keys = new List<string>();

				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group != null && _group.ReferenceGameObject != null && _group.ReferenceGameObject.GetComponent<ICECreatureZone>() != null )
						_keys.Add( _group.Name );
				}

				return _keys;
			}
		}

		public List<string> GetReferenceNamesByEntityType( EntityClassType _type )
		{
			List<string> _keys = new List<string>();
			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
			{
				if( _group != null && _group.ReferenceGameObject != null && _group.EntityType == _type )
					_keys.Add( _group.Name );
			}

			return _keys;
		}

		public List<string> GetReferenceNamesByType<T>()
		{
			List<string> _keys = new List<string>();
			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
			{
				if( _group != null && _group.ReferenceGameObject != null && _group.ReferenceGameObject.GetComponent<T>() != null )
					_keys.Add( _group.Name );
			}

			return _keys;
		}

		/// <summary>
		/// Gets all reference GameObject names.
		/// </summary>
		/// <description>ReferenceGameObjectNames will be used only to fill the target selection popup</description>
		/// <value>The reference GameObject names.</value>
		public List<string> ReferenceGameObjectNames
		{
			get{
				
				List<string> _types = System.Enum.GetNames(typeof(EntityClassType)).ToList();
				Dictionary<string, List<string>> _table = new Dictionary<string, List<string>>();
			
				foreach( string _key in _types )
				{
					if( ! _table.ContainsKey( _key ) )
						_table.Add( _key, new List<string>() );
				}

				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				{
					if( _group != null && _group.ReferenceGameObject != null && ! string.IsNullOrEmpty( _group.Name ) )
					{
						List<string> _list = new List<string>();
						if( _table.TryGetValue( _group.EntityType.ToString(), out _list ) && _list != null )
							_list.Add( _group.Name );
					}
					else
					{
						//Debug.Log( "Invalid ReferenceGroupObject" );
					}

				}

				List<string> _names = new List<string>();
				foreach( string _key in _types )
				{
					List<string> _list = new List<string>();
					if( _table.TryGetValue( _key, out _list ) && _list != null && _list.Count > 0  )
					{
						if( _names.Count > 0 ) 
							_names.Add( " " );

						foreach( string _name in _list )
							_names.Add( _name );
					}
				}

				return _names;
			}
		}


		/// <summary>
		/// Gets all listed reference GameObjects.
		/// </summary>
		/// <value>All reference targets.</value>
		public List<GameObject> ReferenceGameObjects{
			get{
				List<GameObject> _targets = new List<GameObject>();

				foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
					_targets.Add( _group.ReferenceGameObject );
				
				return _targets;
			}

		}


		/// <summary>
		/// Gets the reference game object by tag.
		/// </summary>
		/// <returns>The reference game object by tag.</returns>
		/// <param name="_tag">Tag.</param>
		public GameObject GetReferenceGameObjectByTag( string _tag )
		{
			if( string.IsNullOrEmpty( _tag ) || _tag.Trim() == "" )
				return null;

			GameObject _object = null;

			foreach( GameObject _reference_object in ReferenceGameObjects )
			{
				if( _reference_object != null && _reference_object.tag == _tag )
				{
					_object = _reference_object;
					break;
				}
			}

			// BACKUP SEARCH
			if( _object == null )
			{
				_object = GameObject.FindWithTag( _tag );

				// add as reference while its not listed
				AddReference( _object );
			}

			return _object;
		}

		/// <summary>
		/// Gets the reference game object by name.
		/// </summary>
		/// <returns>The reference game object by name.</returns>
		/// <param name="_name">Name.</param>
		public GameObject GetReferenceGameObjectByName( string _name )
		{
			if( string.IsNullOrEmpty( _name ) || _name.Trim() == "" )
				return null;
			
			GameObject _object = null;

			foreach( GameObject _reference_object in ReferenceGameObjects )
			{
				if( _reference_object != null && _reference_object.name.Trim() == _name.Trim() )
				{
					_object = _reference_object;
					break;
				}
			}

			if( _object == null )
			{
				_object = GameObject.Find( _name );

				// add as reference while its not listed
				AddReference( _object );
			}

			return _object;
		}

		public GameObject GetReferenceGameObjectByType( EntityClassType _type )
		{
			GameObject _object = null;

			List<ReferenceGroupObject> _groups = GetReferenceGroupObjectsByType( _type );

			foreach( ReferenceGroupObject _group in _groups )
			{
				if( _group != null && _group.ReferenceGameObject )
				{
					_object = _group.ReferenceGameObject;
					break;
				}
			}

			if( _object == null )
			{
				ICEWorldEntity[] _entities = GameObject.FindObjectsOfType(typeof(ICEWorldEntity)) as ICEWorldEntity[];

				if( _entities != null && _entities.Length > 0 )
				{
					_object = _entities[0].gameObject;

					foreach( ICEWorldEntity _entity in _entities )
					{
						if( _entity != null && _entity.CompareType( _type ) )
						{
							_object = _entity.gameObject;
							break;
						}
					}
				}
					

				// add as reference while its not listed
				AddReference( _object );
			}

			return _object;
		}

		public int GetReferenceIndexByName( string _name )
		{
			if( string.IsNullOrEmpty( _name ) || _name.Trim() == "" )
				return 0;

			for( int _i = 0; _i < ReferenceGameObjects.Count ; _i++ )
			{
				if( ReferenceGameObjects[_i] != null && ReferenceGameObjects[_i].name == _name )
					return _i;
			}

			return 0;
		}


		public override Color GetDebugDefaultColor( GameObject _object )
		{
			return Color.red;
		}


		/// <summary>
		/// Gets a list with all active GameObjects of the named group.
		/// </summary>
		/// <returns>All active GameObjects of the named group</returns>
		/// <param name="_name">Name.</param>
		public List<GameObject> GetActiveObjectsByName( string _name )
		{
			List<GameObject> _objects = new List<GameObject>();
			
			ReferenceGroupObject _group = GetBufferedGroupByName( _name );
			if( _group != null && _group.Enabled )
			{
				foreach( GameObject _object in _group.ActiveObjects )
					_objects.Add( _object );

				if( _objects.Count == 0 && _group.ReferenceGameObject != null && _group.ReferenceGameObject.activeInHierarchy )
					_objects.Add( _group.ReferenceGameObject ); 
			}

			return _objects;
		}

		/// <summary>
		/// Gets a list with all active GameObjects of the tagged group.
		/// </summary>
		/// <returns>All active GameObjects of the tagged group</returns>
		/// <param name="_name">Name.</param>
		public List<GameObject> GetActiveObjectsByTag( string _tag )
		{
			List<GameObject> _objects = new List<GameObject>();

			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
			{
				if( _group != null && _group.Enabled && _group.CompareByTag( _tag ) )
				{
					foreach( GameObject _object in _group.ActiveObjects )
						_objects.Add( _object );

					if( _objects.Count == 0 && _group.ReferenceGameObject != null && _group.ReferenceGameObject.activeInHierarchy )
						_objects.Add( _group.ReferenceGameObject ); 
				}
			}

			return _objects;
		}

		/// <summary>
		/// Gets a list with all active GameObjects with the specified
		/// </summary>
		/// <returns>The active objects by type.</returns>
		/// <param name="_type">Type.</param>
		public List<GameObject> GetActiveObjectsByType( EntityClassType _type )
		{
			List<GameObject> _objects = new List<GameObject>();

			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
			{
				if( _group != null && _group.Enabled && _group.CompareByType( _type ) )
				{
					foreach( GameObject _object in _group.ActiveObjects )
						_objects.Add( _object );

					if( _objects.Count == 0 && _group.ReferenceGameObject != null && _group.ReferenceGameObject.activeInHierarchy )
						_objects.Add( _group.ReferenceGameObject ); 
				}
			}

			return _objects;
		}

		//################################################################################
		// REFERENCE MANAGEMENT
		//################################################################################

		public void UpdateReferences()
		{
			ICECreatureEntity[] _entities = FindObjectsOfType<ICECreatureEntity>();			
			foreach( ICECreatureEntity _entity in _entities )
			{
				if( _entity != null )
					AddReference( _entity.gameObject );
			}
		}
			
		public bool AddReference( GameObject _object )
		{
			if( _object == null )
				return false;

			if( ! IsListedAsReference( _object ) )
			{
				ReferenceGroupObject _reference = new ReferenceGroupObject( _object );

				if( _reference.EntityType ==  EntityClassType.BodyPart ||
					( _reference.EntityComponent != null && _reference.EntityComponent.IsChildEntity ) )
				{
					_reference.Enabled = false;
				}

				ReferenceGroupObjects.Add( _reference );
				return true;
			}
			else
			{
				return false;
			}
		}

		public ReferenceGroupObject AddReferenceAndReturnGroup( GameObject _object )
		{
			if( _object == null )
				return null;

			if( ! IsListedAsReference( _object ) )
			{
				ReferenceGroupObject _group = new ReferenceGroupObject( _object );
				ReferenceGroupObjects.Add( _group );	
				return _group;
			}

			return null;
		}

		/// <summary>
		/// Determines whether the specified object is already registered.
		/// </summary>
		/// <returns><c>RegisterReferenceType</c> if this instance is registered the specified _object; otherwise, <c>RegisterReferenceType.NONE</c>.</returns>
		/// <param name="_object">_object.</param>
		private bool IsListedAsReference( GameObject _object )
		{
			if( _object == null )
				return false;

			return IsListedAsReferenceByName( _object.name );
		}

		/// <summary>
		/// Determines whether this instance is registered the specified _object_name.
		/// </summary>
		/// <returns><c>RegisterReferenceType</c> if this instance is registered the specified _object_name; otherwise, <c>RegisterReferenceType.NONE</c>.</returns>
		/// <param name="_object_name">_object_name.</param>
		private bool IsListedAsReferenceByName( string _name )
		{
			_name = SystemTools.CleanName( _name );

			if( string.IsNullOrEmpty( _name ) )
				return false;

			foreach( ReferenceGroupObject _group in ReferenceGroupObjects )
				if( _group.ReferenceGameObject != null && _group.ReferenceGameObject.name == _name )
					return true;

			return false;
		}
	}
}

namespace ICE.Creatures.Objects{
	//################################################################################
	//################################################################################
	// STATIC FEATURES
	//################################################################################
	//################################################################################

	public class CreatureRegister : WorldManager
	{
		/// <summary>
		/// Gets the creature register instance.
		/// </summary>
		/// <value>The creature register instance.</value>
		public static ICECreatureRegister RegisterInstance{
			get{ return ICECreatureRegister.Instance; }
		}

		public static bool AddReference( GameObject _object ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.AddReference( _object ): false );
		}

		/// <summary>
		/// Register the specified _object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public static new ReferenceGroupObject Register( GameObject _object ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.Register( _object ): null );
		}

		/// <summary>
		/// Deregister the specified _object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public static new bool Deregister( GameObject _object ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.Deregister( _object ):true );
		}
		/*
		/// <summary>
		/// Attachs to transform.
		/// </summary>
		/// <returns><c>true</c>, if to transform was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_transform">Transform.</param>
		public static bool AttachToTransform( GameObject _object, Transform _transform ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.AttachToTransform( _object, _transform ):true );
		}

		/// <summary>
		/// Detachs from transform.
		/// </summary>
		/// <returns><c>true</c>, if from transform was detached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public static bool DetachFromTransform( GameObject _object ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.DetachFromTransform( _object ):true );
		}*/
			

		/// <summary>
		/// Sends the group message.
		/// </summary>
		/// <param name="_sender_group">Sender group.</param>
		/// <param name="_sender">Sender.</param>
		/// <param name="_msg">Message.</param>
		public static void SendGroupMessage( ReferenceGroupObject _sender_group, GameObject _sender, BroadcastMessageDataObject _msg )
		{
			if( ICECreatureRegister.Instance != null )
				ICECreatureRegister.Instance.SendGroupMessage( _sender_group, _sender, _msg );
		}

		#region Main Search Routine

		/// <summary>
		/// Gets the best object.
		/// </summary>
		/// <returns>The best object.</returns>
		/// <param name="_owner">Owner.</param>
		/// <param name="_objects">Objects.</param>
		/// <param name="_range">Range.</param>
		/// <param name="_max_counterparts">Max counterparts.</param>
		/// <param name="_prefer_counterpart">If set to <c>true</c> prefer counterpart.</param>
		/// <param name="_allow_child">If set to <c>true</c> allow child.</param>
		public static GameObject GetBestObject( GameObject _owner, GameObject[] _objects, float _default_range, int _max_counterparts, bool _prefer_counterpart, bool _allow_child )
		{
			if( _owner == null || _objects == null || _objects.Length == 0 )
				return null;

			_default_range = ( _default_range > 0 ? _default_range : Mathf.Infinity );

			GameObject _best_object = null;
			float _best_distance = ( _default_range > 0 ? _default_range : Mathf.Infinity );
			int _best_counterparts = _max_counterparts;

			GameObject _best_counterpart_object = null;
			float _best_counterpart_distance = _best_distance;
			int _best_counterpart_counterparts = _best_counterparts;

			// transform buffer
			Transform _owner_transform = _owner.transform;
			ICECreatureEntity _owner_entity = _owner.GetComponent<ICECreatureEntity>();

			for( int i = 0; i < _objects.Length; i++ )
			{
				GameObject _object = _objects[i];

				if( _object == null || ! _object.activeInHierarchy || _object == _owner )
					continue;

				// transform buffer
				Transform _object_transform = _object.transform;

				if( ! _allow_child && _object_transform.IsChildOf( _owner_transform ) == true )
					continue;

				float _distance = PositionTools.Distance( _owner_transform.position, _object_transform.position );

				if( _distance > _default_range )
					continue;

				if( _max_counterparts > -1 || _prefer_counterpart )
				{
					ICECreatureEntity _entity = _object.GetComponent<ICECreatureEntity>();
					if( _entity != null && _owner_entity != null )
					{
						int _entity_counterparts = _entity.ActiveCounterparts.Count;

						if( _distance < _best_counterpart_distance && ( ! _prefer_counterpart || _owner_entity.ActiveCounterpartExists( _entity ) ) )
						{
							_best_counterpart_object = _object;
							_best_counterpart_counterparts = _entity_counterparts;
							_best_counterpart_distance = _distance;
						}
						else if( _distance < _best_distance && ( _max_counterparts == -1 || _entity_counterparts <= _best_counterparts ) )
						{
							_best_counterparts = _entity_counterparts;
							_best_distance = _distance;	
							_best_object = _object;
						}
					}
				}
				else if( _distance < _best_distance )
				{
					_best_distance = _distance;	
					_best_object = _object;
				}
			}

			if( _best_counterpart_object != null )
			{
				_best_counterparts = _best_counterpart_counterparts;
				_best_distance = _best_counterpart_distance;	
				_best_object = _best_counterpart_object;
			}

			return _best_object;
		}

		#endregion

		#region Primary Static Search Methods (Target Main-Selection)

		public static GameObject FindBestObjectByName( GameObject _sender, string _name, float _distance, int _max_counterparts, bool _prefer_counterpart, bool _child ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.FindBestObjectByName( _sender, _name, _distance, _max_counterparts, _prefer_counterpart, _child ): null );
		}

		public static GameObject FindBestObjectByTag( GameObject _sender, string _tag, float _distance, int _max_counterparts, bool _prefer_counterpart, bool _child  ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.FindBestObjectByTag( _sender, _tag, _distance, _max_counterparts, _prefer_counterpart, _child ): null );
		}

		public static GameObject FindBestObjectByType( GameObject _sender, EntityClassType _type, float _distance, int _max_counterparts, bool _prefer_counterpart, bool _child  ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.FindBestObjectByType( _sender, _type, _distance, _max_counterparts, _prefer_counterpart, _child ): null );
		}

		#endregion

		#region Secondary Static Search Methods (Target Pre-Selection)

		public static GameObject[] FindObjectsByName( string _name ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.FindObjectsByName( _name ): null );
		}

		public static GameObject[] FindObjectsByTag( string _tag ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.FindObjectsByTag( _tag ): null );
		}

		public static GameObject[] FindObjectsByType( EntityClassType _type ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.FindObjectsByType( _type ): null );
		}

		#endregion

		#region Spawn Points Search

		public static GameObject GetRandomTargetByName( string _name ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.GetRandomTargetByName( _name ): null );
		}
			
		public static GameObject GetRandomTargetByTag( string _tag ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.GetRandomTargetByTag( _tag ): null );
		}

		#endregion

		#region Reference Objects

		public static GameObject GetReferenceGameObjectByTag( string _tag ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.GetReferenceGameObjectByTag( _tag ): null );
		}

		public static GameObject GetReferenceGameObjectByName( string _name ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.GetReferenceGameObjectByName( _name ): null );
		}

		public static GameObject GetReferenceGameObjectByType( EntityClassType _type ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.GetReferenceGameObjectByType( _type ): null );
		}

		public static int GetReferenceIndexByName( string _name ){
			return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.GetReferenceIndexByName( _name ): 0 );
		}

		public static List<ReferenceGroupObject> ReferenceGroupObjects{
			get{ return ( ICECreatureRegister.Instance != null ? ICECreatureRegister.Instance.ReferenceGroupObjects: new List<ReferenceGroupObject>() ); }
		}

		#endregion

	
		public static bool UseDebug{
			get{
				if( ICECreatureRegister.Instance != null )
					return ICECreatureRegister.Instance.UseDebug;
				else
					return true;
			}
		}



		/*
		public static EnvironmentInfoContainer EnvironmentInfos{
			get{ return GlobalEnvironment.Infos; }
		}*/
	}

}
