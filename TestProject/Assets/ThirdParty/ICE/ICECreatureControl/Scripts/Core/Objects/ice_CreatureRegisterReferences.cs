// ##############################################################################
//
// ice_CreatureRegisterReferences.cs
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
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class ReferenceStatusObject : ICEObject
	{
		public bool HasCreatureController;
		public bool HasCreatureAdapter;
		public bool HasHome;
		public bool HasMissionOutpost;
		public bool HasMissionEscort;
		public bool HasMissionPatrol;
		public bool isPrefab;
		public bool isActiveAndEnabled;
		public bool isActiveInHierarchy;
	}

	[System.Serializable]
	public class SpawnerObject : ICETimerObject
	{
		public SpawnerObject(){}
		public SpawnerObject( SpawnerObject _object ) : base( _object ){ Copy( _object ); }

		public void Copy( SpawnerObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}
	}

	[System.Serializable]
	public abstract class ReferenceDataObject : EntityDataObject
	{
		public ReferenceDataObject(){ Enabled = true; Foldout = true; }
		public ReferenceDataObject( GameObject _object ){
			ReferenceGameObject = _object;
			Enabled = true; 
			Foldout = true; 
		}
		/*
		[SerializeField]
		private SpawnerObject m_Spawner = null;
		public SpawnerObject Spawner{
			get{ return m_Spawner = ( m_Spawner == null ? new SpawnerObject() : m_Spawner ); }
			set{ Spawner.Copy( value ); }
		}*/

		[SerializeField]
		private List<SpawnPointObject> m_SpawnPoints = null;
		public List<SpawnPointObject> SpawnPoints{
			get{ return m_SpawnPoints = ( m_SpawnPoints == null ? new List<SpawnPointObject>() : m_SpawnPoints ); }
			set{ 
				SpawnPoints.Clear();
				if( value == null ) return;	
				foreach( SpawnPointObject _point in value )
					SpawnPoints.Add( new SpawnPointObject( _point ) ); 
			}
		}

		[SerializeField]
		private CullingOptionsObject m_CullingOptions = null;
		public CullingOptionsObject CullingOptions{
			get{ return m_CullingOptions = ( m_CullingOptions == null ? new CullingOptionsObject() : m_CullingOptions ); }
			set{ CullingOptions.Copy( value ); }
		}


		//TODO: [System.Obsolete ("Use ReferenceGameObject instead")]
		public GameObject Reference = null; // TODO: obsolete can be removed in v1.3

		[SerializeField]
		private ReferenceStatusObject m_Status = null;
		public ReferenceStatusObject Status{
			get{ return m_Status = ( m_Status == null ? new ReferenceStatusObject() : m_Status ); }
		}

		public bool GroupByTag = false;

		[SerializeField]
		public GameObject ReferenceGameObject{
			get{ 
				//TODO: UPDATE OBSOLETE VALUES
				if( Reference != null )
				{
					SetEntityGameObject( Reference );
					Reference = null;

					ICEDebug.LogInfo( "Copy 'Reference' to 'ReferenceGameObject'!" );
				}

				return EntityGameObject; 			
			}
			set{ SetEntityGameObject( value ); }
		}

		public bool PoolManagementEnabled = false;
		public bool UseSoftRespawn = true;	

		public int InitialSpawnPriority = 0;
		public int MaxCoexistingObjects = 25;
		public int MaxCoexistingObjectsMaximum = 100;

		public bool UseInitialSpawn = false;		
		public float MinSpawnInterval = 10;
		public float MaxSpawnInterval = 60;
		public float RespawnIntervalMax = 360;

		public bool UseSpawnWave = false;
		public int SpawnWaveMin = 1;
		public int SpawnWaveMax = 5;
		public int SpawnWaveMaximum = 25;

		public bool UseMaxSpawnCycles = false;
		protected int m_SpawnCycles = 0;
		public int MaxSpawnCycles = 25;
		public int MaxSpawnCyclesMaximum = 100;

		public bool UseRandomization = false;
		public float RandomSizeMin = 0;
		public float RandomSizeMax = 0;

		public bool UseGroupParent = false;
		public GameObject CustomGroupParent = null;

		public string Key{
			get{ 
				if( ReferenceGameObject != null )
				{
					if( GroupByTag )
						return ReferenceGameObject.tag;
					else
						return ReferenceGameObject.name;
				}
				else
					return "";
			}
		}

		/// <summary>
		/// Compares the specified _object with the reference object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public bool Compare( GameObject _object )
		{
			if( _object == null || ReferenceGameObject == null )
				return false;
			
			//( GroupByTag && ! string.IsNullOrEmpty( _object.tag ) && ReferenceGameObject.CompareTag( _object.tag ) ) || 
			if( SystemTools.CleanName( ReferenceGameObject.name ) == SystemTools.CleanName( _object.name ) )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Compares the specified name with the name of reference object.
		/// </summary>
		/// <returns><c>true</c>, if the compared names are identic, <c>false</c> otherwise.</returns>
		/// <param name="_name">Name.</param>
		public bool CompareByName( string _name )
		{
			if( string.IsNullOrEmpty( _name ) || ReferenceGameObject == null )
				return false;

			if( ReferenceGameObject.name == SystemTools.CleanName( _name ) )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Compares the specified tag with the tag of reference object.
		/// </summary>
		/// <returns><c>true</c>, if the compared tags are identic, <c>false</c> otherwise.</returns>
		/// <param name="_tag">Tag.</param>
		public bool CompareByTag( string _tag )
		{
			if( string.IsNullOrEmpty( _tag ) || ReferenceGameObject == null )
				return false;
			
			if( ReferenceGameObject.CompareTag( _tag ) )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Compares the specified type with the type of reference object.
		/// </summary>
		/// <returns><c>true</c>, if by type was compared, <c>false</c> otherwise.</returns>
		/// <param name="_type">Type.</param>
		public bool CompareByType( EntityClassType _type )
		{
			if( ReferenceGameObject == null )
				return false;

			return ( EntityType == _type ? true : false );
		}

		/// <summary>
		/// Compares the specified id with the id of reference object.
		/// </summary>
		/// <returns><c>true</c>, if by ID was compared, <c>false</c> otherwise.</returns>
		/// <param name="_id">Identifier.</param>
		public bool CompareByID( int _id )
		{
			if( ReferenceGameObject == null )
				return false;

			return ( ReferenceGameObject.GetInstanceID() == _id ? true : false );
		}
	}

	[System.Serializable]
	public class ReferenceGroupObject : ReferenceDataObject
	{
		public ReferenceGroupObject(){}
		public ReferenceGroupObject( GameObject _object ) : base( _object ){}

		private List<GameObject> m_LocalObjects = null;
		public List<GameObject> LocalObjects{
			get{ return m_LocalObjects = ( m_LocalObjects == null ? new List<GameObject>() : m_LocalObjects ); }
		}

		private List<GameObject> m_ActiveObjects = null;
		public List<GameObject> ActiveObjects{
			get{ return m_ActiveObjects = ( m_ActiveObjects == null ? new List<GameObject>() : m_ActiveObjects ); }
		}

		private List<GameObject> m_SuspendedObjects = null;
		public List<GameObject> SuspendedObjects{
			get{ return m_SuspendedObjects = ( m_SuspendedObjects == null ? new List<GameObject>() : m_SuspendedObjects ); }
		}
			
		public float BaseOffset{
			get{
				if( EntityComponent != null )
					return EntityComponent.BaseOffset;
				else if( ReferenceGameObject != null )
					return ReferenceGameObject.transform.localScale.y * 0.5f;
				else
					return 0;
			}
		}

		private List<SpawnPointObject> m_ValidSpawnPoints = null;
		public List<SpawnPointObject> ValidSpawnPoints{
			get{

				if( m_ValidSpawnPoints == null || ! Application.isPlaying )
				{
					m_ValidSpawnPoints = new List<SpawnPointObject>();
					foreach( SpawnPointObject _point in SpawnPoints ){
						if( _point != null && _point.IsValid )
							m_ValidSpawnPoints.Add( _point );
					}
				}
					
				return m_ValidSpawnPoints;
			}
		}

		/// <summary>
		/// Reset destroys all items and enables the initial spawning if required 
		/// </summary>
		public override void Reset()
		{
			DestroyItems();

			m_InitialSpawnComplete = false;
		}

		public bool Register( GameObject _object )
		{
			if( _object == null || ! Enabled )
				return false;
			
			bool _added = false;
			if( ! IsRegistered( _object ) )
			{
				ActiveObjects.Add( _object );
				_added = true;

				ICECreatureEntity _entity = _object.GetComponent<ICECreatureEntity>();

				// if the object is an ICE entity we have to adapt the parent according to it's hierarchy settings  
				if( _entity != null && _entity.UseHierarchyManagement )
					_object.transform.SetParent( UpdateGroupParent(), true );
				
				if( _entity != null )
					OnGroupMessage += _entity.Message.ReceiveGroupMessage;
			}
	
			return _added;
		}
		
		public bool Deregister( GameObject _object )
		{
			if( _object == null || ! Enabled )
				return false;

			ICECreatureEntity _entity = _object.GetComponent<ICECreatureEntity>();
			if( _entity != null )
				OnGroupMessage -= _entity.Message.ReceiveGroupMessage;
			
			if( SuspendedObjects.Count > 0 )
				SuspendedObjects.Remove( _object );

			if( ActiveObjects.Count > 0 )
				return ActiveObjects.Remove( _object );
			else
				return false;
		}

		public bool AttachToTransform( GameObject _object, Transform _transform ){
			return AttachToTransform( _object, _transform, true );
		}

		/// <summary>
		/// Attachs to transform.
		/// </summary>
		/// <returns><c>true</c>, if to transform was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_transform">Transform.</param>
		public bool AttachToTransform( GameObject _object, Transform _transform, bool _use_transform_position )
		{
			if( _object == null )
				return false;

			if( _use_transform_position )
			{
				_object.transform.position = _transform.position;
				_object.transform.rotation = _transform.rotation;
			}

			_object.transform.SetParent( _transform, true );

			Rigidbody _rigidbody = _object.GetComponent<Rigidbody>();
			if( _rigidbody != null )
			{
				_rigidbody.useGravity = false;
				_rigidbody.isKinematic = true;
				_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}

			SystemTools.EnableColliders( _object.transform, false );

			_object.SetActive( true );

			return true;
		}

		/// <summary>
		/// Detachs from transform.
		/// </summary>
		/// <returns><c>true</c>, if from transform was detached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public bool DetachFromTransform( GameObject _object )
		{
			if( _object == null )
				return false;

			if( ! Register( _object ) )
			{
				ICECreatureEntity _entity = _object.GetComponent<ICECreatureEntity>();

				// if the object is an ICE entity we have to adapt the parent according to it's hierarchy settings  
				if( _entity != null && _entity.UseHierarchyManagement )
					_object.transform.SetParent( UpdateGroupParent(), true );
				else
					_object.transform.SetParent( null, true );
			}

			Rigidbody _rigidbody = _object.GetComponent<Rigidbody>();
			if( _rigidbody != null )
			{
				_rigidbody.useGravity = true;
				_rigidbody.isKinematic = false;
				_rigidbody.constraints = RigidbodyConstraints.None;
			}

			SystemTools.EnableColliders( _object.transform, true );

			_object.SetActive( true );

			return true;
		}

		/// <summary>
		/// Remove the specified _object by using the UseSoftRespawn option of the ReferenceGroup. If UseSoftRespawn
		/// is active the specified _object will be moved from the ActiveObjects list to the SuspendedObjects list and
		/// its values will be adjusted to the default values, otherwise if UseSoftRespawn is disabled the specified 
		/// _object will be finally destroyed.
		/// </summary>
		/// <param name="_object">Object.</param>
		public bool Remove( GameObject _object )
		{
			if( _object == null )
				return false;
			
			if( UseSoftRespawn )
			{
				//Debug.Log( "ReferenceGroupObject.Remove: " + _object.name );

				_object.transform.position = Vector3.zero;
				_object.transform.rotation = Quaternion.identity;
				_object.SetActive( false );

				if( ActiveObjects.Count > 0 )
					return ActiveObjects.Remove( _object );

				SuspendedObjects.Add( _object );

				ICECreatureEntity _entity = _object.GetComponent<ICECreatureEntity>();

				// if the object is an ICE entity we have to adapt the parent according to it's hierarchy settings  
				if( _entity != null && _entity.UseHierarchyManagement )
					_object.transform.SetParent( UpdateGroupParent(), true );

			
				//TODO: CreatureRegister.DetachFromTransform( _object );

			}
			else
			{
				DestroyItem( _object );
			}

			return true;
		}

		// Event Handler
		public delegate void OnGroupMessageEvent( ReferenceGroupObject _group, GameObject _sender, BroadcastMessageDataObject _msg  );
		public event OnGroupMessageEvent OnGroupMessage;

		public void Message( GameObject _sender, BroadcastMessageDataObject _msg )
		{
			if( _sender == null )
				return;

			if( OnGroupMessage != null )
				OnGroupMessage( this, _sender, _msg );
		}

		public void Message( ReferenceGroupObject _group, GameObject _sender, BroadcastMessageDataObject _msg )
		{
			if( _sender == null )
				return;

			if( OnGroupMessage != null )
				OnGroupMessage( _group, _sender, _msg );
		}

		public void DestroyItem( GameObject _object )
		{
			if( _object == null && EntityGameObject != _object )
				return;
			
			SuspendedObjects.Remove( _object );
			ActiveObjects.Remove( _object );

			if( ! CompareByID( _object.GetInstanceID() ) )
				CreatureRegister.Destroy( _object );
		}
		
		public void DestroyItems()
		{
			while( ActiveObjects.Count > 0 )
				DestroyItem( ActiveObjects[0] );
		}

		public bool IsRegistered( GameObject _object )
		{
			if( _object == null )
				return false;
			
			int _id = _object.GetInstanceID();
			foreach( GameObject _item in ActiveObjects )
			{
				if( _item.GetInstanceID() == _id )
					return true;
			}
			
			return false;
		}

		public bool IsSuspended( GameObject _object )
		{
			int _id = _object.GetInstanceID();
			foreach( GameObject _item in SuspendedObjects )
			{
				if( _item.GetInstanceID() == _id )
					return true;
			}
			
			return false;
		}



		private bool m_InitialSpawnComplete = false;
		public bool InitialSpawnComplete{
			get{ return m_InitialSpawnComplete; }
		}

		[SerializeField]
		private bool m_Break = false;
		public bool Break{
			set{
				m_Break = value;
				for( int i = 0; i < ActiveObjects.Count; i++ )
				{
					GameObject _object = ActiveObjects[i];

					if( _object != null )
					{
						ICEWorldEntity _entity = _object.GetComponent<ICEWorldEntity>();
						if( _entity != null )
							_entity.enabled = ! m_Break;
					}	
				}
			}
			get{ return m_Break; }
		}





		private float m_SpawnTimer = 0;
		private float m_SpawnInterval = 0;
		public float RespawnInterval{
			get{return m_SpawnInterval; }
		}
		
		public int ActiveObjectsCount{
			get{ return ActiveObjects.Count; }
		}

		public int SuspendedObjectsCount{
			get{ return SuspendedObjects.Count; }
		}

		public int TotalSpawnCycles{
			get{ return m_SpawnCycles; }
		}

		public GameObject GetRandomObject()
		{
			if( ActiveObjects.Count > 0 )
				return ActiveObjects[ Random.Range( 0, ActiveObjects.Count ) ];
			else
				return null;
		}


		public Vector3 GetSpawnPosition() 
		{
			Vector3 _position = Vector3.zero;

			if( ValidSpawnPoints.Count > 0 )
			{
				SpawnPointObject _point = ValidSpawnPoints[ Random.Range( 0, ValidSpawnPoints.Count ) ];

				if( _point != null )
					_position = _point.GetSpawnPosition( BaseOffset );
			}
			else if( ReferenceGameObject != null )
			{
				_position = PositionTools.GetRandomPosition( ReferenceGameObject.transform.position, 25 );
				_position.y += BaseOffset;
			}
	
			return _position;
		}
			

		private bool InitialSpawningIsAvailable{
			get{ return ( SpawningIsAvailable && UseInitialSpawn && ! m_InitialSpawnComplete ? true : false ); }
		}

		private bool SpawningIsAvailable{
			get{
				if( Enabled && // group must be enabled
					PoolManagementEnabled && // pool management must be enabled 	
					( ReferenceGameObject != null && EntityPlayer == null ) && // reference object have to be exists and may not be a player
					SpawnCyclesValid &&  // spawn cycles have to be unlimited or within the defined range
					CoexistingObjectsValid &&  // the numbers of active objects have to be within the defined range
					CustomGroupParentIsValidAndActive &&  // a custom group parent must be available and active
					( ICEWorldInfo.NetworkSpawnerReady && ICEWorldInfo.IsServer ) ) // Network must be ready or inactive and the client must be the master
					return true;
				else
					return false;
			}
		}

		private bool SpawningLocalPlayerIsAvailable{
			get{
				if( Enabled && // group must be enabled
					PoolManagementEnabled && // pool management must be enabled 	
					( ReferenceGameObject != null && EntityPlayer != null ) && // reference object have to be exists and must be a player
					SpawnCyclesValid &&  // spawn cycles have to be unlimited or within the defined range
					( WorldManager.LocalPlayer == null || ! WorldManager.LocalPlayer.activeInHierarchy ) && // the local player is not empty and also not inactive
					CustomGroupParentIsValidAndActive &&  // a custom group parent must be available and active
					( ICEWorldInfo.NetworkSpawnerReady ) ) // Network must be ready or inactive 
					return true;
				else
					return false;
			}
		}

		private GameObject SpawnLocalPlayer()
		{
			if( ! SpawningLocalPlayerIsAvailable )
				return null;

			Vector3 _position = GetSpawnPosition();	

			Quaternion _rotation = Random.rotation;
			_rotation.z = 0;
			_rotation.x = 0;

			if( WorldManager.LocalPlayer != null && WorldManager.LocalPlayer.activeInHierarchy == false )
			{
				SuspendedObjects.Remove( WorldManager.LocalPlayer );

				WorldManager.LocalPlayer.transform.position = _position;
				WorldManager.LocalPlayer.transform.rotation = _rotation;
			}

			if( WorldManager.LocalPlayer == null )
				WorldManager.LocalPlayer = WorldManager.Instantiate( ReferenceGameObject, _position, _rotation );	

			if( WorldManager.LocalPlayer != null )
			{
				//WorldManager.LocalPlayer.name = ReferenceGameObject.name;

				ICEWorldEntity _entity = WorldManager.LocalPlayer.GetComponent<ICEWorldEntity>();

				// if the player is an ICE entity we have to adapt the parent according to it's hierarchy settings
				if( _entity != null && _entity.UseHierarchyManagement )
					WorldManager.LocalPlayer.transform.SetParent( UpdateGroupParent(), true );

				if( _entity != null )
				{
					_entity.Reset();
					_entity.IsLocal = true;
				}

				WorldManager.LocalPlayer.SetActive( true );
				Register( WorldManager.LocalPlayer );

				m_SpawnCycles++;
			}

			return WorldManager.LocalPlayer;	
		}

		private GameObject PoolManagementSpawn()
		{
			if( ! SpawningIsAvailable )
				return null;

			Vector3 _position = GetSpawnPosition();	

			// check if the generated spawn position fullfil the given conditions
			if( CullingOptions.CheckCullingConditions( _position ) ) 
				return null;

			Quaternion _rotation = Quaternion.Euler( new Vector3( 0, UnityEngine.Random.Range( 0, 360 ), 0 ) );

			GameObject _object = ActivateSuspendedObject( _position, _rotation );

			if( _object == null )
				_object = InstantiateNewObject( _position, _rotation );	

			if( _object != null )
				m_SpawnCycles++;

			return _object;	
		}

		public GameObject ActivateSuspendedObject( Vector3 _position, Quaternion _rotation ) 
		{
			GameObject _object = null;
			if( SuspendedObjects.Count > 0 )
			{
				_object = SuspendedObjects[0];

				if( _object != null )
				{
					SuspendedObjects.Remove( _object );

					_object.transform.position = _position;
					_object.transform.rotation = _rotation;

					ICEWorldEntity _entity = _object.GetComponent<ICEWorldEntity>();

					if( _entity != null )
						_entity.Reset();

					_object.SetActive( true );
					Register( _object );
				}
			}

			return _object;
		}

		public GameObject InstantiateNewObject( Vector3 _position, Quaternion _rotation ) 
		{
			if( ReferenceGameObject == null )
				return null;
			
			GameObject _object = WorldManager.Instantiate( ReferenceGameObject, _position, _rotation );

			if( _object == null )
				return null;

			if( UseRandomization )
				_object.transform.localScale = _object.transform.localScale + ( _object.transform.localScale * (float)Random.Range( RandomSizeMin, RandomSizeMax ) );

			//_object.name = ReferenceGameObject.name;

			ICEWorldEntity _entity = _object.GetComponent<ICEWorldEntity>();

			// if the object is an ICE entity we have to adapt the parent according to it's hierarchy settings 
			if( _entity != null && _entity.UseHierarchyManagement )
				_object.transform.SetParent( UpdateGroupParent(), true );

			if( _entity != null )
			{
				_entity.Reset();
				_entity.IsLocal = true;
			}

			_object.SetActive( true );

			Register( _object );

			return _object;
		}




			
		private Transform m_GroupParentTransform = null;

		/// <summary>
		/// Gets the custom hierarchy group transform.
		/// </summary>
		/// <returns>The custom hierarchy group transform.</returns>
		private Transform GetCustomGroupParentTransform(){

			if( CustomGroupParent == null ) 
				return null;
			else if( CustomGroupParent.activeInHierarchy )
				return CustomGroupParent.transform;
			else
			{
				GameObject _object = GameObject.Find( CustomGroupParent.name );
				return ( _object != null && _object.activeInHierarchy ? _object.transform : null );
			}
		}

		/// <summary>
		/// Gets the default hierarchy group transform.
		/// </summary>
		/// <returns>The default hierarchy group transform.</returns>
		private Transform GetHierarchyGroupTransform(){

			if( ICECreatureRegister.Instance != null )
				return ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroupTransform( EntityType );
			else
				return null;
		}

		private bool CustomGroupParentIsValidAndActive{
			get { return ( UseGroupParent == false || CustomGroupParent == null || GetCustomGroupParentTransform() != null ? true : false ); }
		}


		private bool SpawnCyclesValid{
			get{ return ( UseMaxSpawnCycles && MaxSpawnCycles > 0 && m_SpawnCycles >= MaxSpawnCycles ? false : true ); }
		}

		private bool CoexistingObjectsValid{
			get{ return ( ActiveObjectsCount < MaxCoexistingObjects ? true : false ); }
		}


		private Transform UpdateGroupParent()
		{
			if( UseGroupParent )
			{
				if( m_GroupParentTransform == null )
					m_GroupParentTransform = GetCustomGroupParentTransform(); 

				// if there is no group object we create it 
				if( m_GroupParentTransform == null && CustomGroupParent == null )
				{
					m_GroupParentTransform = new GameObject().transform;
					m_GroupParentTransform.name = Name + "Group";
					m_GroupParentTransform.position = Vector3.zero;
				}
			}

			Transform _root = GetHierarchyGroupTransform();
							
			if( m_GroupParentTransform != null )
				m_GroupParentTransform.SetParent( _root, true );
			else
				m_GroupParentTransform = _root;

			return m_GroupParentTransform;
		}

		/*
		public bool CheckSpawnConditions()
		{
			//if(	Camera.main != null && Vector3.Distance( Camera.main.transform.position, _position ) > 20 )
			//	return false;
		}*/



		public void TryPoolManagementInitialSpawn()
		{
			if( ! InitialSpawningIsAvailable )
				return;

			bool _allow_spawning = ( EntityPlayer == null ? true : false );
			while( _allow_spawning )
				_allow_spawning = ( PoolManagementSpawn() == null ? false : true );
		}
		
		public void TryPoolManagementSpawn()
		{
			if( SpawningLocalPlayerIsAvailable )
			{
				MaxCoexistingObjects = 1;
				UseInitialSpawn = true;
				InitialSpawnPriority = 0;
				UseRandomization = false;
				UseSoftRespawn = true;

				SpawnLocalPlayer();
			}
			else if( InitialSpawningIsAvailable )
			{
				m_InitialSpawnComplete = true;
				TryPoolManagementInitialSpawn();
			}
			else if( SpawningIsAvailable )
			{
				if( m_SpawnInterval == 0 )
					m_SpawnInterval = UnityEngine.Random.Range( MinSpawnInterval, MaxSpawnInterval );

				m_SpawnTimer += Time.deltaTime;				
				if( m_SpawnTimer >= m_SpawnInterval )
				{
					m_SpawnTimer = 0;
					m_SpawnInterval = UnityEngine.Random.Range( MinSpawnInterval, MaxSpawnInterval );

					if( UseSpawnWave )
					{
						int _wave = UnityEngine.Random.Range( SpawnWaveMin, SpawnWaveMax );
						for( int _i = 0 ; _i <= _wave ; _i++ )
							PoolManagementSpawn();
					}
					else
						PoolManagementSpawn();
				}
			}
		}
	}
	/*
	[System.Serializable]
	public class ReferenceGroupsObject : ICEOwnerObject
	{
		public ReferenceGroupsObject(){}
		public ReferenceGroupsObject( ICEWorldBehaviour _component ) : base( _component ){}
	}*/
}
