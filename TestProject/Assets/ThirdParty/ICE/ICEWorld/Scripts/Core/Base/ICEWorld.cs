// ##############################################################################
//
// ICEWorldRegister.cs | ICEWorldRegister
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

#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World
{
	[System.Serializable]
	public class RegisterTerrainObject : ICEDataObject
	{
		public RegisterTerrainObject(){}
		/*
		public void GetTreeInstances()
		{
			if( Terrain.activeTerrain == null || Terrain.activeTerrain.terrainData == null )
				return;

			Terrain _terrain = Terrain.activeTerrain;
			TerrainData _terrain_data = Terrain.activeTerrain.terrainData;

			TreePrototype[] _tree_types = _terrain_data.treePrototypes;

			TreeInstance[] _trees = _terrain_data.treeInstances;

			foreach( TreeInstance _tree in _trees )
			{
			//	_tree.prototypeIndex
			}
		}*/
	}

	[System.Serializable]
	public class RegisterOptionsObject : ICEDataObject
	{
		public RegisterOptionsObject(){}
		public RegisterOptionsObject( RegisterOptionsObject _options ) : base( _options ) { Copy( _options ); }

		public void Copy( RegisterOptionsObject _options )
		{
			if( _options == null )
				return;

			base.Copy( _options );

			Enabled = true;

			GroundLayer = _options.GroundLayer;
			WaterLayer = _options.WaterLayer;
			ObstacleLayer = _options.ObstacleLayer;

			GroundCheck = _options.GroundCheck;
			WaterCheck = _options.WaterCheck;
			ObstacleCheck = _options.ObstacleCheck;
		}

		public QueryTriggerInteraction TriggerInteraction = QueryTriggerInteraction.Ignore;

		public GroundCheckType GroundCheck = GroundCheckType.NONE;
		[SerializeField]
		private LayerObject m_GroundLayer = null;
		public LayerObject GroundLayer{
			get{ return m_GroundLayer = ( m_GroundLayer == null ? new LayerObject() : m_GroundLayer ); }
			set{ GroundLayer.Copy( value ); }
		}

		public WaterCheckType WaterCheck = WaterCheckType.DEFAULT;
		[SerializeField]
		private LayerObject m_WaterLayer = null;
		public LayerObject WaterLayer{
			get{ return m_WaterLayer = ( m_WaterLayer == null ? new LayerObject( "Water" ) : m_WaterLayer ); }
			set{ WaterLayer.Copy( value ); }
		}

		public ObstacleCheckType ObstacleCheck = ObstacleCheckType.NONE;
		[SerializeField]
		private LayerObject m_ObstacleLayer = null;
		public LayerObject ObstacleLayer{
			get{ return m_ObstacleLayer = ( m_ObstacleLayer == null ? new LayerObject( (LayerMask)Physics.DefaultRaycastLayers ) : m_ObstacleLayer ); }
			set{ ObstacleLayer.Copy( value ); }
		}

		public LayerMask GroundLayerMask{
			get{ return GroundLayer.Mask; }
		}

		public LayerMask WaterLayerMask{
			get{ return WaterLayer.Mask; }
		}

		public LayerMask ObstacleLayerMask{
			get{ return ObstacleLayer.Mask; }
		}

		private LayerMask m_OverlapPreventionLayerMask = 0;
		public LayerMask OverlapPreventionLayerMask{
			get{ 
				if( m_OverlapPreventionLayerMask == 0 || ! Application.isPlaying )
				{
					int _mask = Physics.AllLayers;

					_mask ^= Physics.IgnoreRaycastLayer;
			
					if( GroundCheck == GroundCheckType.RAYCAST && GroundLayer.Layers.Count > 0 )
						_mask ^= GroundLayerMask;

					if( WaterLayerMask.value != 0 )
						_mask ^= WaterLayerMask;

					if( ObstacleCheck != ObstacleCheckType.NONE && ObstacleLayer.Layers.Count > 0 )
						_mask |= ObstacleLayerMask;

					m_OverlapPreventionLayerMask = _mask;
				}

				/*
				Debug.Log( "Ground : " + SystemTools.IsInLayerMask( "Ground", m_OverlapPreventionLayerMask ) );
				Debug.Log( "Obstacle : " + SystemTools.IsInLayerMask( "Obstacle", m_OverlapPreventionLayerMask ) );
				Debug.Log( "Water : " + SystemTools.IsInLayerMask( "Water", m_OverlapPreventionLayerMask ) );
				Debug.Log( "Default : " + SystemTools.IsInLayerMask( "Default", m_OverlapPreventionLayerMask ) );
				*/

				return m_OverlapPreventionLayerMask;
			}
		}
	}

	public class ICEWorldRegister : ICEWorldSingleton {

		[SerializeField]
		private RegisterOptionsObject m_Options = null;
		public RegisterOptionsObject Options{
			get{ return m_Options = ( m_Options == null ? new RegisterOptionsObject() : m_Options ); }
			set{ Options.Copy( value ); }
		}

		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayer = null;

		public bool UsePoolManagement = false;
		public bool UsePoolManagementCoroutine = false;
		protected bool m_PoolManagementCoroutineIsRunning = false;

		public Vector3 GridSize = Vector3.one;

		public RandomSeedType RandomSeed = RandomSeedType.DEFAULT;
		public int CustomRandomSeed = 23;

		public bool UseGarbageCollection = false;
		public float GarbageCollectionInterval = 1.5f;
		private float m_GarbageCollectionIntervalTimer = 0;



		protected static new ICEWorldRegister m_Instance = null;
		public static new ICEWorldRegister Instance{
			get{ return m_Instance = ( m_Instance == null?GameObject.FindObjectOfType<ICEWorldRegister>():m_Instance ); }
		}

		public delegate void OnSpawnObjectEvent( out GameObject _object, GameObject _reference, Vector3 _position, Quaternion _rotation );
		public event OnSpawnObjectEvent OnSpawnObject;
		public delegate void OnInstantiateObjectEvent( out GameObject _object, GameObject _reference, Vector3 _position, Quaternion _rotation );
		public event OnInstantiateObjectEvent OnInstantiateObject;
		public delegate void OnRemoveObjectEvent( GameObject _object, out bool _removed );
		public event OnRemoveObjectEvent OnRemoveObject;
		public delegate void OnDestroyObjectEvent( GameObject _object, out bool _destroyed );
		public event OnDestroyObjectEvent OnDestroyObject;

		public override void OnEnable()
		{
#if UNITY_5_4_OR_NEWER
			SceneManager.sceneLoaded += OnLevelLoad;
#endif
		}

		public override void OnDisable()
		{
#if UNITY_5_4_OR_NEWER
			SceneManager.sceneLoaded -= OnLevelLoad;
#endif
		}

#if UNITY_5_4_OR_NEWER
		protected virtual void OnLevelLoad( Scene scene, LoadSceneMode mode )
#else
		protected virtual void OnLevelWasLoaded()
#endif
		{

		}

		public override void Update() 
		{
			base.Update();

			if( UseGarbageCollection )
			{
				m_GarbageCollectionIntervalTimer += Time.deltaTime;
				if( m_GarbageCollectionIntervalTimer >= GarbageCollectionInterval ) //if( Time.frameCount % 30 == 0 )
				{
					System.GC.Collect();
					m_GarbageCollectionIntervalTimer = 0;
				}
			}
		}

		public virtual void Register( GameObject _object )
		{
		}

		public virtual bool Deregister( GameObject _object )
		{
			return true;
		}


		public virtual GameObject Spawn( GameObject _reference, Vector3 _position, Quaternion _rotation )
		{
			if( _reference == null )
				return null;

			GameObject _object = null;
			if( ! TryCustomSpawn( out _object, _reference, _position, _rotation ) )
				_object = (GameObject)Instantiate( _reference, _position, _rotation );

			return _object;
		}

		public bool TryCustomSpawn( out GameObject _object, GameObject _reference, Vector3 _position, Quaternion _rotation )
		{
			_object = null;

			if( OnSpawnObject != null )
				OnSpawnObject( out _object, _reference, _position, _rotation );

			return ( _object != null ? true : false );
		}

		/// <summary>
		/// Instantiates the specified reference by using its defined position and rotation. Use this method to instantiate
		/// new ICE objects.
		/// </summary>
		/// <param name="_reference">Reference.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public virtual GameObject Instantiate( GameObject _reference, Vector3 _position, Quaternion _rotation )
		{
			if( _reference == null )
				return null;

			GameObject _object = null;
			if( OnInstantiateObject != null )
				OnInstantiateObject( out _object, _reference, _position, _rotation );
			else
				_object = (GameObject)Object.Instantiate( _reference, _position, _rotation );

			return _object;
		}

		/// <summary>
		/// Tries to remove the specified _object from a potential list. Override this method 
		/// to remove the object from a list.
		/// </summary>
		/// <param name="_object">Object.</param>
		public virtual bool Remove( GameObject _object )
		{
			if( _object == null )
				return false;

			bool _removed = false;
			if( OnRemoveObject != null )
				OnRemoveObject( _object, out _removed );
			else
			{
				_removed = true;
				WorldManager.Destroy( _object );
			}

			return _removed;
		}

		public virtual bool Destroy( GameObject _object )
		{
			if( _object == null )
				return false;

			bool _destroyed = false;
			if( OnDestroyObject != null )
				OnDestroyObject( _object, out _destroyed );
			else
			{
				_destroyed = true;
				WorldManager.Destroy( _object );
			}

			return _destroyed;
		}

		public virtual bool AttachToTransform( GameObject _object, Transform _transform ){
			return AttachToTransform( _object, _transform, true );
		}

		public virtual bool AttachToTransform( GameObject _object, Transform _transform, bool _use_transform_position )
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

			return false;
		}

		public virtual bool DetachFromTransform( GameObject _object )
		{
			if( _object == null )
				return false;

			_object.transform.SetParent( null, true );

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
			
		public virtual Color GetDebugDefaultColor( GameObject _object )
		{
			return Color.red;
		}
	}

	public class WorldManager{

		public static GameObject LocalPlayer{
			get{ return ICEWorldRegister.LocalPlayer; }
			set{ ICEWorldRegister.LocalPlayer = value; }
		}

		public static GroundCheckType GroundCheck{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.GroundCheck : GroundCheckType.NONE ); }
		}

		public static WaterCheckType WaterCheck{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.WaterCheck : WaterCheckType.DEFAULT ); }
		}

		public static ObstacleCheckType ObstacleCheck{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.ObstacleCheck : ObstacleCheckType.NONE ); }
		}

		/// <summary>
		/// Gets the ground layer or null.
		/// </summary>
		/// <value>The ground layer.</value>
		public static LayerObject GroundLayer{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.GroundLayer : null ); }
		}

		/// <summary>
		/// Gets the water layer or null.
		/// </summary>
		/// <value>The water layer.</value>
		public static LayerObject WaterLayer{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.WaterLayer : null ); }
		}

		/// <summary>
		/// Gets the obstacle layer or null.
		/// </summary>
		/// <value>The obstacle layer.</value>
		public static LayerObject ObstacleLayer{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.ObstacleLayer : null ); }
		}

		public static LayerMask GroundLayerMask{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.GroundLayerMask: (LayerMask)Physics.DefaultRaycastLayers ); }
		}

		public static LayerMask WaterLayerMask{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.WaterLayerMask: (LayerMask)Physics.DefaultRaycastLayers ); }
		}

		public static LayerMask ObstacleLayerMask{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.ObstacleLayerMask: (LayerMask)Physics.DefaultRaycastLayers ); }
		}

		public static LayerMask OverlapPreventionLayerMask{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.OverlapPreventionLayerMask: (LayerMask)Physics.DefaultRaycastLayers ); }
		}
			
		public static void Register( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Register( _object );
		}

		public static void Deregister( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Deregister( _object );
		}

		/// <summary>
		/// Recyles or instantiate a clone of the specified reference object by using the defined _position and _rotation.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public static GameObject Spawn( GameObject _object, Vector3 _position, Quaternion _rotation  )
		{
			if( _object == null )
				return null;
			
			ICEWorldEntity _entity = _object.GetComponent<ICEWorldEntity>();
			if( _entity != null )
				_position.y += _entity.BaseOffset;
				
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.Spawn( _object, _position, _rotation );
			else
				return WorldManager.Instantiate( _object, _position, _rotation );
		}

		/// <summary>
		/// Instantiate a new clone of the specified reference object by using the defined _position and _rotation.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public static GameObject Instantiate( GameObject _reference, Vector3 _position, Quaternion _rotation  )
		{
			if( _reference == null )
				return null;
			
			ICEWorldEntity _entity = _reference.GetComponent<ICEWorldEntity>();
			if( _entity != null )
				_position.y += _entity.BaseOffset;
			
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.Instantiate( _reference, _position, _rotation );
			else
				return (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
		}

		/// <summary>
		/// Remove the specified _object according to the given removing features and their current options, 
		/// so it could be that the specified _object will be deactivate only. If you want to destroy an object 
		/// finally use Destroy( GameObject _object ). 
		/// </summary>
		/// <param name="_object">Object.</param>
		public static void Remove( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Remove( _object );
			else
				Destroy( _object );
		}

		/// <summary>
		/// Destroy the specified _object finally.
		/// </summary>
		/// <param name="_object">Object.</param>
		public static void Destroy( GameObject _object )
		{
			if( Application.isPlaying )
				GameObject.Destroy( _object );
			else
				GameObject.DestroyImmediate( _object );
		}

		/// <summary>
		/// Destroy the specified _object at the given _time.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_time">Time.</param>
		public static void Destroy( GameObject _object, float _time )
		{
			if( Application.isPlaying )
				GameObject.Destroy( _object, _time );
		}


		/// <summary>
		/// Attachs the specified object to the defined transform.
		/// </summary>
		/// <returns><c>true</c>, if to transform was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_transform">Transform.</param>
		public static bool AttachToTransform( GameObject _object, Transform _transform ){
			return AttachToTransform( _object, _transform, true );
		}

		/// <summary>
		/// Attachs the specified object to the defined transform.
		/// </summary>
		/// <returns><c>true</c>, if to transform was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_transform">Transform.</param>
		public static bool AttachToTransform( GameObject _object, Transform _transform, bool _use_transform_position )
		{
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.AttachToTransform( _object, _transform, _use_transform_position );
			else if( _object != null )
			{
				if( _use_transform_position && _transform != null )
				{
					_object.transform.position = _transform.position;
					_object.transform.rotation = _transform.rotation;
				}

				_object.transform.SetParent( _transform, true );
			}

			return true;
		}

		/// <summary>
		/// Detachs the specified object from its parent.
		/// </summary>
		/// <returns><c>true</c>, if from transform was detached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public static bool DetachFromTransform( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.DetachFromTransform( _object );
			else if( _object != null )
				_object.transform.SetParent( null, true );

			return true;
		}

		/// <summary>
		/// Gets the default color of the gizmo.
		/// </summary>
		/// <returns>The debug default color.</returns>
		/// <param name="_object">Object.</param>
		public static Color GetDebugDefaultColor( GameObject _object ) 
		{
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.GetDebugDefaultColor( _object );
			else
				return Color.red;
		}

		/// <summary>
		/// Sets the ground level.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_offset">Offset.</param>
		public static void SetGroundLevel( Transform _transform, float _offset ) 
		{
			SystemTools.EnableColliders( _transform, false );
			float _ground_level = GetGroundLevel( _transform.position, _offset );

			_transform.position = new Vector3( 
				_transform.position.x,
				_ground_level + _offset, 
				_transform.position.z );
			SystemTools.EnableColliders( _transform, true );
		}

		/// <summary>
		/// Gets the ground level.
		/// </summary>
		/// <returns>The ground level.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_offset">Offset.</param>
		public static float GetGroundLevel( Vector3 _position, float _offset = 0 ) 
		{
			if( ICEWorldRegister.Instance != null && ICEWorldRegister.Instance.Options.GroundCheck == GroundCheckType.RAYCAST )
				return PositionTools.GetGroundLevel( _position, ICEWorldRegister.Instance.Options.GroundCheck , ICEWorldRegister.Instance.Options.GroundLayerMask, 0.5f, 1000, _offset );
			else
				return PositionTools.GetGroundLevel( _position, 0.5f, 1000, _offset );
		}

		/// <summary>
		/// Gets the trigger interaction.
		/// </summary>
		/// <value>The trigger interaction.</value>
		public static QueryTriggerInteraction TriggerInteraction{
			get{ return ( ICEWorldRegister.Instance != null ? ICEWorldRegister.Instance.Options.TriggerInteraction : QueryTriggerInteraction.UseGlobal ); }
		}
	}
}
