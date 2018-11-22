// ##############################################################################
//
// ICE.World.ICEWorldBehaviour.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World
{
	[System.Serializable]
	public class BehaviourOptionsColor : ICEObject
	{
		public BehaviourOptionsColor(){}
		public BehaviourOptionsColor( float _h, float _s1, float _s2, float _b ){

			this.Hue = _h;
			this.Saturation = _s1;
			this.SaturationSelected = _s2;
			this.Brightness = _b;
				
			this.DefaultColor = new HSBColor( _h , _s1 , _b ).ToColor();
			this.DefaultSelectedColor = new HSBColor( _h , _s2 , _b ).ToColor();
			this.Color = this.DefaultColor;
			this.SelectedColor = this.DefaultSelectedColor;
		}

		public void Copy( BehaviourOptionsColor _color )
		{
			if( _color == null )
				return;

			this.DefaultColor = _color.DefaultColor;
			this.DefaultSelectedColor = _color.DefaultSelectedColor;
			this.Color = _color.Color;
			this.SelectedColor = _color.SelectedColor;
		}

		public override void Reset()
		{
			this.Color = this.DefaultColor;
			this.SelectedColor = this.DefaultSelectedColor;
		}

		public float Hue = 0;
		public float Saturation = 0;
		public float SaturationSelected = 0;
		public float Brightness = 0;

		public Color DefaultColor;
		public Color DefaultSelectedColor;
		public Color Color;
		public Color SelectedColor;
	}

	[System.Serializable]
	public class ICEWorldBehaviourOptionsObject : ICEDataObject
	{
		public ICEWorldBehaviourOptionsObject(){}
		public ICEWorldBehaviourOptionsObject( ICEWorldBehaviourOptionsObject _options ) : base( _options ) {
			Copy( _options );
		}

		public void Copy( ICEWorldBehaviourOptionsObject _options )
		{
			if( _options == null )
				return;

		}

		public float DefaultSaturation = 0.1f;
		public float DefaultSaturationSelected = 0.5f;

		public BehaviourOptionsColor SystemColor = new BehaviourOptionsColor( 0.6f, 0.1f, 0.5f , 1 );
		public BehaviourOptionsColor ListColor = new BehaviourOptionsColor( 0.6f, 0.1f, 0.5f , 1 );
		public BehaviourOptionsColor DebugColor = new BehaviourOptionsColor( 0.6f, 0.1f, 0.5f , 1 );

	}

	/// <summary>
	/// ICEComponent is the abstract base class of all ICEWorld based components.
	/// </summary>
	public abstract class ICEWorldBehaviour : MonoBehaviour {

		[SerializeField]
		protected ICEWorldBehaviourOptionsObject m_DisplayOptions = null;
		public virtual ICEWorldBehaviourOptionsObject DisplayOptions{
			get{ return m_DisplayOptions = ( m_DisplayOptions == null ? new ICEWorldBehaviourOptionsObject():m_DisplayOptions ); }
			set{ DisplayOptions.Copy( value ); }
		}

		public static bool IsRemoteClient{
			get{ return ( ICEWorldInfo.IsMultiplayer && ! ICEWorldInfo.IsServer ? true : false ); }
		}

		public static bool IsMultiplayer{
			get{ return ICEWorldInfo.IsMultiplayer; }
		}

		public static bool IsMasterClient{
			get{ return ICEWorldInfo.IsServer; }
		}

		public static bool NetworkConnectedAndReady{
			get{ return ICEWorldInfo.NetworkConnectedAndReady; }
		}

		public static bool NetworkSpawnerReady{
			get{ return ICEWorldInfo.NetworkSpawnerReady; }
		}

		private bool m_ApplicationQuits = false;
		public bool ApplicationQuits{
			get{ return m_ApplicationQuits; }
		}

		/// <summary>
		/// Enables/disables available debug features
		/// </summary>
		public bool UseDebug = false;

		/// <summary>
		/// Show runtime behaviour infos.
		/// </summary>
		public bool ShowInfo = false;

		/// <summary>
		/// Show all help informations.
		/// </summary>
		public bool ShowHelp = false;

		/// <summary>
		/// Show all note fields.
		/// </summary>
		public bool ShowNotes = false;
		/// <summary>
		/// Enables debug logs.
		/// </summary>
		public bool UseDebugLogs = false;
		public bool UseDebugLogsSelectedOnly = false;
		public void PrintDebugLog( string _log )
		{
#if UNITY_EDITOR
			if( DebugLogIsEnabled )
				ICEDebug.Log( name + " (" + ObjectInstanceID + ") - " + _log );
#endif
		}

		public bool UseDebugRays = false;
		public bool UseDebugRaysSelectedOnly = false;
		public void DebugRay( Vector3 _origin, Vector3 _direction, Color _color )
		{
#if UNITY_EDITOR
			if( DebugRayIsEnabled )
				ICEDebug.DrawRay( _origin, _direction, _color);
#endif
		}

		public void DebugLine( Vector3 _from, Vector3 _to, Color _color )
		{
#if UNITY_EDITOR
			if( DebugRayIsEnabled )
				ICEDebug.DrawLine( _from, _to, _color);
#endif
		}

		public bool DebugLogIsEnabled
		{
			get{ 
#if UNITY_EDITOR
				return ( UseDebugLogs && ( ! UseDebugLogsSelectedOnly || UnityEditor.Selection.activeGameObject == gameObject ) ? true : false );
#else
				return false;
#endif
			}
		}
		public bool DebugRayIsEnabled
		{
			get{ 
#if UNITY_EDITOR
				return ( UseDebugRays && ( ! UseDebugRaysSelectedOnly || UnityEditor.Selection.activeGameObject == gameObject ) ? true : false );
#else
				return false;
#endif
			}
		}

		public void PrintError( string _log )
		{
			if( UseDebugLogs )
				ICEDebug.LogError( name + " (" + ObjectInstanceID + ") - " + _log );
		}
			

		/// <summary>
		/// Activates or deactivates 'Dont Destroy On Load'.
		/// </summary>
		public bool UseDontDestroyOnLoad = false;

		/// <summary>
		/// The cached InstanceID.
		/// </summary>
		private int m_ObjectInstanceID = 0;
		/// <summary>
		/// Gets the cached InstanceID.
		/// </summary>
		/// <value>The ID.</value>
		public int ObjectInstanceID{
			get{  return m_ObjectInstanceID = ( m_ObjectInstanceID == 0 ? transform.gameObject.GetInstanceID():m_ObjectInstanceID ); }
		}


		protected Transform m_Transform = null;
		/// <summary>
		/// Gets the cached object transform.
		/// </summary>
		/// <value>The object transform.</value>
		public Transform ObjectTransform {
			get{ return m_Transform  = ( m_Transform == null? transform : m_Transform ); }
		}
			
		/// <summary>
		/// OnUpdateEvent. This delegate can be used in ICEOwnerObject classes to handle updates
		/// </summary>
		public delegate void OnUpdateEvent();

		/// <summary>
		/// OnUpdate. This event can be used in ICEOwnerObject classes to handle updates
		/// </summary>
		public event OnUpdateEvent OnUpdate;

		/// <summary>
		/// OnLateUpdateEvent. This delegate can be used in ICEOwnerObject classes to handle late updates
		/// </summary>
		public delegate void OnLateUpdateEvent();

		/// <summary>
		/// OnLateUpdate. This event can be used in ICEOwnerObject classes to handle late updates
		/// </summary>
		public event OnLateUpdateEvent OnLateUpdate;

		/// <summary>
		/// OnFixedUpdateEvent. This delegate can be used in ICEOwnerObject classes to handle fixed updates
		/// </summary>
		public delegate void OnFixedUpdateEvent();

		/// <summary>
		/// OnFixedUpdate. This event can be used in ICEOwnerObject classes to handle fixed updates
		/// </summary>
		public event OnFixedUpdateEvent OnFixedUpdate;

		// PUBLIC METHODS
		/// <summary>
		/// m_PublicMethods. PublicMethods represents a list of method names.
		/// </summary>
		protected List<BehaviourEventInfo> m_BehaviourEvents = new List<BehaviourEventInfo>();
		/// <summary>
		/// Gets the public methods.
		/// </summary>
		/// <value>The public methods.</value>
		public BehaviourEventInfo[] BehaviourEvents{
			get{ return GetBehaviourEvents(); }
		}

		protected BehaviourEventInfo[] GetBehaviourEvents()
		{
			m_BehaviourEvents.Clear();
			OnRegisterBehaviourEvents();

			List<BehaviourEventInfo> _events = new List<BehaviourEventInfo>();
			foreach( BehaviourEventInfo _event in m_BehaviourEvents )						
				_events.Add( new BehaviourEventInfo( _event ) );

			return _events.ToArray();
		}


		/// <summary>
		/// Gets all available behaviour events.
		/// </summary>
		/// <value>All available behaviour events.</value>
		public BehaviourEventInfo[] BehaviourEventsInChildren{
			get{
				List<BehaviourEventInfo> _events = new List<BehaviourEventInfo>();
				ICEWorldBehaviour[] _components = GetComponentsInChildren<ICEWorldBehaviour>();
				if( _components != null )
				{
					foreach( ICEWorldBehaviour _component in _components )
						foreach( BehaviourEventInfo _event in _component.BehaviourEvents )						
							_events.Add( new BehaviourEventInfo( _event ) );
				}

				return _events.ToArray();
			}
		}

		/// <summary>
		/// OnRegisterBehaviourEvents is called whithin the GetBehaviourEvents() method to update the 
		/// m_BehaviourEvents list. Override this event to register your own events by using the 
		/// RegisterBehaviourEvent method, while doing so you can use base.OnRegisterBehaviourEvents(); 
		/// to call the event in the base classes too.
		/// </summary>
		protected virtual void OnRegisterBehaviourEvents(){}

		/// <summary>
		/// Registers the behaviour event with zero parameter.
		/// </summary>
		/// <param name="_event">Event.</param>
		public void RegisterBehaviourEvent( string _event ){
			RegisterBehaviourEvent( _event, BehaviourEventParameterType.None );
		}

		/// <summary>
		/// Registers the behaviour event with the defind parameter type.
		/// </summary>
		/// <param name="_event">Event.</param>
		/// <param name="_type">Type.</param>
		public void RegisterBehaviourEvent( string _event, BehaviourEventParameterType _type ){
			if( string.IsNullOrEmpty( _event ) )
				return;

			m_BehaviourEvents.Add( new BehaviourEventInfo( this.name, _event, _type ) );
		}

		/// <summary>
		/// Registers the behaviour event with the defind parameter type.
		/// </summary>
		/// <param name="_event">Event.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_parameter">Parameter.</param>
		/// <param name="_description">Description.</param>
		public void RegisterBehaviourEvent( string _event, BehaviourEventParameterType _type, string _parameter, string _description ){
			if( string.IsNullOrEmpty( _event ) )
				return;

			m_BehaviourEvents.Add( new BehaviourEventInfo( this.name, _event, _type, _parameter, _description ) );
		}

		/// <summary>
		/// Clears the behaviour events.
		/// </summary>
		public void ClearBehaviourEvents(){
			m_BehaviourEvents.Clear();
		}

		public virtual bool GetDynamicBooleanValue( DynamicBooleanValueType _type )
		{
			/*
			switch( _type )
			{
			case DynamicBooleanValueType.IsGrounded:
				return Creature.Move.IsGrounded;
			case DynamicBooleanValueType.Deadlocked:
				return Creature.Move.Deadlocked;
			case DynamicBooleanValueType.MovePositionReached:
				return Creature.Move.MovePositionReached;
			case DynamicBooleanValueType.TargetMovePositionReached:
				return Creature.Move.TargetMovePositionReached;
			case DynamicBooleanValueType.MovePositionUpdateRequired:
				return Creature.Move.MovePositionUpdateRequired;
			case DynamicBooleanValueType.IsJumping:
				return Creature.Move.IsJumping;


			}*/

			return false;
		}

		public virtual int GetDynamicIntegerValue( DynamicIntegerValueType _type )
		{/*
			switch( _type )
			{

			case DynamicIntegerValueType.CreatureForwardSpeed:
				return MoveForwardVelocity;
			case DynamicIntegerValueType.CreatureAngularSpeed:
				return MoveAngularVelocity;
			case DynamicIntegerValueType.CreatureDirection:
				return MoveDirection;
			}*/

			return 0;
		}

		public virtual float GetDynamicFloatValue( DynamicFloatValueType _type )
		{
			/*
			switch( _type )
			{
			case DynamicFloatValueType.ForwardSpeed:
				return MoveForwardVelocity;
			case DynamicFloatValueType.AngularSpeed:
				return MoveAngularVelocity;
			case DynamicFloatValueType.Direction:
				return MoveDirection;

			case DynamicFloatValueType.FallSpeed:
				return Creature.Move.FallSpeed;

			case DynamicFloatValueType.Altitude:
				return MoveAltitude;
			case DynamicFloatValueType.AbsoluteAltitude:
				return MoveAbsoluteAltitude;

			case DynamicFloatValueType.MovePositionDistance:
				return Creature.Move.MovePositionDistance;
			}*/

			return 0;
		}

		public virtual void Awake () {

			if( UseDontDestroyOnLoad )
				DontDestroyOnLoad( this.gameObject );
		}

		public virtual void Start () {

		}

		public virtual void OnEnable () {

		}

		public virtual void OnDisable () {

		}

		public virtual void OnDestroy() {

		}

		public virtual void OnApplicationQuit() {
			m_ApplicationQuits = true;
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public virtual void Update () {
			DoUpdate();
		}

		public virtual void LateUpdate () {
			DoLateUpdate();
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		public virtual void FixedUpdate () {
			DoFixedUpdate();
		}

		/// <summary>
		/// Dos the update.
		/// </summary>
		protected virtual void DoUpdate () {
			if( OnUpdate != null )
				OnUpdate();
		}

		protected virtual void DoLateUpdate () {
			if( OnLateUpdate != null )
				OnLateUpdate();
		}

		/// <summary>
		/// Dos the fixed update.
		/// </summary>
		protected virtual void DoFixedUpdate () {
			if( OnFixedUpdate != null )
				OnFixedUpdate();
		}
	}
}
