// ##############################################################################
//
// ice_objects_events.cs | ICE.World.Objects.BehaviourEventsObject | BehaviourEventObject | BehaviourEvent | BehaviourEventInfo
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{

	[System.Serializable]
	public class InputEventsObject : ICEOwnerObject 
	{
		public InputEventsObject(){}
		public InputEventsObject( ICEWorldBehaviour _component ) : base( _component ){}
		public InputEventsObject( InputEventsObject _object ) : base( _object ){}

		public void Copy( InputEventsObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Events = _object.Events;
		}

		[SerializeField]
		private List<InputEventObject> m_Events = null;
		public List<InputEventObject> Events{
			get{ return m_Events = ( m_Events == null ? new List<InputEventObject>() : m_Events ); }
			set{
				Events.Clear();
				if( value == null ) return;	
				foreach( InputEventObject _event in value )
					Events.Add( new InputEventObject( _event ) );
			}
		}

		public void Update( GameObject _owner )
		{
			if( _owner == null || ! Enabled )
				return;
			
			SetOwner( _owner );

			foreach( InputEventObject _event in Events )
			{
				if( _event.Enabled && _event.IsValid )
					_event.SendMessage( Owner );
			}
		}
	}

	[System.Serializable]
	public class InputEventObject : ICEOwnerObject 
	{
		public InputEventObject(){}
		public InputEventObject( InputEventObject _object ) : base( _object ){ Copy( _object ); }

		public void Copy( InputEventObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			UseKeyInput = _object.UseKeyInput;
			KeyInput = _object.KeyInput;
			AxisInput = _object.AxisInput;
		}

		public bool UseKeyInput = false;
		public bool UseTimer = false;
		public KeyCode KeyInput;
		public AxisInputData AxisInput;

		private GameObject m_Receiver = null;
		private Transform m_ReceiverTransform = null;

		[SerializeField]
		private BehaviourEvent m_Event = null;
		public BehaviourEvent Event{
			get{ return m_Event = ( m_Event == null ? new BehaviourEvent() : m_Event ); }
			set{ Event.Copy( value ); }
		}

		[SerializeField]
		private ICEDirectTimerObject m_Timer = null;
		public ICEDirectTimerObject Timer{
			get{ return m_Timer = ( m_Timer == null ? new ICEDirectTimerObject() : m_Timer ); }
			set{ Timer.Copy( value ); }
		}

		private bool CheckTimer( bool _active )
		{
			if( ! UseTimer )
				return _active;

			bool _result = false;

			if( _active && ! Timer.Active )
				Timer.Start( false );
			
			if( _active && Timer.Active )
				_result = Timer.Update();
							
			if( ! _active && Timer.Active )
				_result = Timer.Stop();

			return _result;
		}

		private bool CheckKey()
		{
			if( ! UseKeyInput )
				return false;

			bool _value = Input.GetKeyDown( KeyInput );
			return _value;
		}

		private bool CheckAxis()
		{
			if( UseKeyInput )
				return false;

			float _value = Input.GetAxis( AxisInput.Name );
			return ( _value > 0 ? true : false );
		}

		public bool IsValid{
			get{ return Enabled && CheckTimer( CheckKey() || CheckAxis() ); }
		}


		public void SendMessage( GameObject _owner )
		{
			if( _owner == null || string.IsNullOrEmpty( Event.FunctionName ) )
				return;

			if( Owner != _owner )
				SetOwner( _owner );

			if( m_Receiver == null )
				m_Receiver = _owner;

			if( m_ReceiverTransform == null )
				m_ReceiverTransform = SystemTools.FindChildByName( Event.ComponentName, m_Receiver.transform );

			if( m_ReceiverTransform != null )
			{
				switch( Event.ParameterType ) 
				{
				case BehaviourEventParameterType.Boolean:
					m_ReceiverTransform.SendMessage( Event.FunctionName, Event.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.Integer:
					m_ReceiverTransform.SendMessage( Event.FunctionName, Event.ParameterInteger, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.Float:
					m_ReceiverTransform.SendMessage( Event.FunctionName, Event.ParameterFloat, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.String:
					m_ReceiverTransform.SendMessage( Event.FunctionName, Event.ParameterString, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.Sender:
					m_ReceiverTransform.SendMessage( Event.FunctionName, Owner, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.SenderComponent:
					m_ReceiverTransform.SendMessage( Event.FunctionName, OwnerComponent, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.SenderTransform:
					m_ReceiverTransform.SendMessage( Event.FunctionName, Owner.transform, SendMessageOptions.DontRequireReceiver );
					break;
				default:
					m_ReceiverTransform.SendMessage( Event.FunctionName, null, SendMessageOptions.DontRequireReceiver );
					break;
				}

				if( DebugLogIsEnabled ) PrintDebugLog( this, "Send Message '" + Event.FunctionName + "' with " + Event.ParameterType.ToString() + " parameter to " + m_ReceiverTransform.name + " (" + m_ReceiverTransform.GetInstanceID() + ")" );
			}
			else
			{
				switch( Event.ParameterType ) 
				{
				case BehaviourEventParameterType.Boolean:
					m_Receiver.BroadcastMessage( Event.FunctionName, Event.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.Integer:
					m_Receiver.BroadcastMessage( Event.FunctionName, Event.ParameterInteger, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.Float:
					m_Receiver.BroadcastMessage( Event.FunctionName, Event.ParameterFloat, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.String:
					m_Receiver.BroadcastMessage( Event.FunctionName, Event.ParameterString, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.Sender:
					m_Receiver.BroadcastMessage( Event.FunctionName, Owner, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.SenderComponent:
					m_Receiver.BroadcastMessage( Event.FunctionName, OwnerComponent, SendMessageOptions.DontRequireReceiver );
					break;
				case BehaviourEventParameterType.SenderTransform:
					m_Receiver.BroadcastMessage( Event.FunctionName, Owner.transform, SendMessageOptions.DontRequireReceiver );
					break;
				default:
					m_Receiver.BroadcastMessage( Event.FunctionName, null, SendMessageOptions.DontRequireReceiver );
					break;
				}

				if( DebugLogIsEnabled ) PrintDebugLog( this, "Broadcast Message '" + Event.FunctionName + "' with " + Event.ParameterType.ToString() + " parameter to " + m_Receiver.name + " (" + m_Receiver.GetInstanceID() + ")" );

			}
		}

	}


	/// <summary>
	/// The BehaviourEventsObject contains and handles the list of defined BehaviourEventObjects
	/// </summary>
	[System.Serializable]
	public class BehaviourEventsObject : ICEOwnerObject
	{
		public BehaviourEventsObject(){}
		public BehaviourEventsObject( ICEWorldBehaviour _component ) : base( _component ){}
		public BehaviourEventsObject( BehaviourEventsObject _events ) : base( _events as ICEOwnerObject ) { Copy( _events ); }

		[SerializeField]
		private List<BehaviourEventObject> m_Events = null;
		public List<BehaviourEventObject> Events{
			get{ return m_Events = ( m_Events == null ? new List<BehaviourEventObject>() : m_Events ); }
			set{ 
				Events.Clear();
				if( value == null ) return;	
				foreach( BehaviourEventObject _event in value )
					Events.Add( new BehaviourEventObject( _event ) );
			}
		}

		/// <summary>
		/// Start a specific event with the specified _owner and _index.
		/// </summary>
		/// <param name="_owner">Owner.</param>
		/// <param name="_index">Index.</param>
		public void TriggerAction( ICEWorldBehaviour _component, GameObject _receiver, int _index )
		{
			if( ! Enabled || _component == null || _index < 0 || _index >= Events.Count )
				return;

			base.Init( _component );	

			BehaviourEventObject _event = Events[ _index ];
			if( _event != null )
				_event.Action( _component, _receiver );
		}

		/// <summary>
		/// Start the event with the specified _owner only, here the owner will be also the _receiver object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public void Start( GameObject _owner ){
			Start( _owner, null );
		}

		/// <summary>
		/// Start the event with the specified _owner and _receiver.
		/// </summary>
		/// <param name="_owner">Owner.</param>
		/// <param name="_receiver">Receiver.</param>
		public void Start( GameObject _owner, GameObject _receiver )
		{
			if( ! Enabled || _owner == null )
				return;

			SetOwner( _owner );		
			foreach( BehaviourEventObject _event in Events )
				_event.Start( _owner, _receiver );
		}

		/// <summary>
		/// Start the event with the specified _component only, here the GameObject of the _component will be also the _receiver object.
		/// </summary>
		/// <param name="_component">Component.</param>
		/// <param name="_receiver">Receiver.</param>
		public void Start( ICEWorldBehaviour _component ){
			Start( _component, null );
		}

		/// <summary>
		/// Start the event with the specified _component and _receiver.
		/// </summary>
		/// <param name="_component">Component.</param>
		/// <param name="_receiver">Receiver.</param>
		public void Start( ICEWorldBehaviour _component, GameObject _receiver )
		{
			if( ! Enabled || _component == null )
				return;
			
			base.Init( _component );		
			foreach( BehaviourEventObject _event in Events )
				_event.Start( _component, _receiver );
		}

		public void Stop()
		{
			foreach( BehaviourEventObject _event in Events )
				_event.Stop();
		}

		public void Update()
		{
			if( ! Enabled )
				return;
			
			foreach( BehaviourEventObject _event in Events )
				_event.Update();
		}

		public void Copy( BehaviourEventsObject _events )
		{
			if( _events == null )
				return;
			
			base.Copy( _events );

			Events.Clear();
			foreach( BehaviourEventObject _event in _events.Events )
				Events.Add( new BehaviourEventObject( _event ) );
		}
	}

	/// <summary>
	/// The BehaviourEventObject contains and handles the defined start, stop and impluse events during the runtime
	/// </summary>
	[System.Serializable]
	public class BehaviourEventObject : ICEImpulsTimerObject
	{
		public BehaviourEventObject(){}
		public BehaviourEventObject( BehaviourEventObject _event ) : base( _event ){ Copy( _event ); }
		public BehaviourEventObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( BehaviourEventObject _event )
		{
			if( _event == null )
				return;
			
			base.Copy( _event );

			Event.Copy( _event.Event );
		}

		[SerializeField]
		private BehaviourEvent m_Event = null;
		public BehaviourEvent Event{
			get{ return m_Event = ( m_Event == null ? new BehaviourEvent() : m_Event ); }
			set{ Event.Copy( value ); }
		}

		private GameObject m_Receiver = null;
		private Transform m_ReceiverTransform = null;

		public void Start( GameObject _object, GameObject _receiver )
		{
			if( _object == null || UseTrigger )
				return;

			SetOwner( _object );	
			m_Receiver = ( _receiver == null ? Owner : _receiver );
			base.Start();
		}

		public void Start( ICEWorldBehaviour _component, GameObject _receiver )
		{
			if( _component == null || UseTrigger )
				return;
			
			base.Init( _component );	
			m_Receiver = ( _receiver == null ? Owner : _receiver );

			base.Start();
		}

		public override void Stop()
		{
			base.Stop();
		}

		protected override void Action()
		{
			SendMessage( Event );
		}

		public void Action( ICEWorldBehaviour _component, GameObject _receiver )
		{
			if( _component == null )
				return;

			base.Init( _component );	
			m_Receiver = ( _receiver == null ? Owner : _receiver );

			SendMessage( Event );
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="_method">Method.</param>
		protected virtual void SendMessage( BehaviourEvent _event )
		{
			if( Owner == null || _event == null || string.IsNullOrEmpty( _event.FunctionName ) )
				return;

			if( m_Receiver == null )
				m_Receiver = Owner;

			if( m_ReceiverTransform == null )
				m_ReceiverTransform = SystemTools.FindChildByName( _event.ComponentName, m_Receiver.transform );

			if( m_ReceiverTransform != null )
			{
				GameObject _receiver = m_ReceiverTransform.gameObject;
					
				switch( _event.ParameterType ) 
				{
					case BehaviourEventParameterType.Boolean:
						_receiver.SendMessage( _event.FunctionName, _event.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Integer:
						_receiver.SendMessage( _event.FunctionName, _event.ParameterInteger, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Float:
						_receiver.SendMessage( _event.FunctionName, _event.ParameterFloat, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.String:
						_receiver.SendMessage( _event.FunctionName, _event.ParameterString, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Sender:
						_receiver.SendMessage( _event.FunctionName, Owner, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.SenderComponent:
						_receiver.SendMessage( _event.FunctionName, OwnerComponent, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.SenderTransform:
						_receiver.SendMessage( _event.FunctionName, Owner.transform, SendMessageOptions.DontRequireReceiver );
						break;
					default:
						_receiver.SendMessage( _event.FunctionName, null, SendMessageOptions.DontRequireReceiver );
						break;
				}

				if( DebugLogIsEnabled ) PrintDebugLog( this, "Send Message '" + _event.FunctionName + "' with " + _event.ParameterType.ToString() + " parameter to " + _receiver.name + " (" + _receiver.GetInstanceID() + ")" );
			}
			else
			{
				switch( _event.ParameterType ) 
				{
					case BehaviourEventParameterType.Boolean:
						m_Receiver.BroadcastMessage( _event.FunctionName, _event.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Integer:
						m_Receiver.BroadcastMessage( _event.FunctionName, _event.ParameterInteger, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Float:
						m_Receiver.BroadcastMessage( _event.FunctionName, _event.ParameterFloat, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.String:
						m_Receiver.BroadcastMessage( _event.FunctionName, _event.ParameterString, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Sender:
						m_Receiver.BroadcastMessage( _event.FunctionName, Owner, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.SenderComponent:
						m_Receiver.BroadcastMessage( _event.FunctionName, OwnerComponent, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.SenderTransform:
						m_Receiver.BroadcastMessage( _event.FunctionName, Owner.transform, SendMessageOptions.DontRequireReceiver );
						break;
					default:
						m_Receiver.BroadcastMessage( _event.FunctionName, null, SendMessageOptions.DontRequireReceiver );
						break;
				}

				if( DebugLogIsEnabled ) PrintDebugLog( this, "Broadcast Message '" + _event.FunctionName + "' with " + _event.ParameterType.ToString() + " parameter to " + m_Receiver.name + " (" + m_Receiver.GetInstanceID() + ")" );

			}
		}
	}

	[System.Serializable]
	public class BehaviourEventInfo : ICEObject
	{
		public BehaviourEventInfo(){}
		public BehaviourEventInfo( BehaviourEventInfo _info ){ Copy( _info ); }
		public BehaviourEventInfo( string _component, string _name, BehaviourEventParameterType _type )
		{
			ComponentName = _component;
			FunctionName = _name;
			ParameterType = _type;
			ParameterName = "";
			ParameterDescription = "";
		}

		public BehaviourEventInfo( string _component, string _name, BehaviourEventParameterType _type, string _parameter, string _description )
		{
			ComponentName = _component;
			FunctionName = _name;
			ParameterType = _type;
			ParameterName = _parameter;
			ParameterDescription = _description;
		}

		/// <summary>
		/// The name of the owner component.
		/// </summary>
		public string ComponentName = "";
		public string FunctionName = "";
		public string ParameterName = "";
		public string ParameterDescription = "";
		public BehaviourEventParameterType ParameterType = BehaviourEventParameterType.None;


		public void Copy( BehaviourEventInfo _info )
		{
			if( _info == null )
				return;

			ComponentName = _info.ComponentName;
			FunctionName = _info.FunctionName;
			ParameterType = _info.ParameterType;
			ParameterName = _info.ParameterName;
			ParameterDescription = _info.ParameterDescription;
		}

		public string Key{
			get{ return SystemTools.CleanName( ComponentName ) + "." + FunctionName; }
		}

		public override void Reset()
		{
			ComponentName = "";
			FunctionName = "";
			ParameterType = BehaviourEventParameterType.None;
			ParameterName = "";
			ParameterDescription = "";
		}

		public string ParameterTitle
		{
			get{

				if( string.IsNullOrEmpty( ParameterName ) )
				{
					switch( ParameterType )
					{
						case BehaviourEventParameterType.Boolean:
							return "Parameter Boolean";
						case BehaviourEventParameterType.Integer:
							return "Parameter Integer";
						case BehaviourEventParameterType.Float:
							return "Parameter Float";
						case BehaviourEventParameterType.String:
							return "Parameter String";
						default:
							return "Parameter";
					}
				}
				else
					return ParameterName;				
			}
		}

		[XmlIgnore]
		public BehaviourEventInfo Info{
			get{ return new BehaviourEventInfo( ComponentName, FunctionName, ParameterType ); }
			set{ 
				ComponentName = value.ComponentName;
				FunctionName = value.FunctionName; 
				ParameterType = value.ParameterType; 
			}
		}
	}

	[System.Serializable]
	public class BehaviourEvent : BehaviourEventInfo
	{
		public BehaviourEvent() : base(){}
		public BehaviourEvent( BehaviourEvent _event ) : base( _event ) { Copy( _event ); }

		public bool UseCustomFunction = false;
		public bool UseCustomParameterObject = false;

		public string ParameterString = ""; 
		public int ParameterInteger = 0;
		public float ParameterFloat = 0f; 
		public bool ParameterBoolean = false;

		/*
		private GameObject m_ParameterObject = null;
		public void SetParameterObject( GameObject _object ){
			m_ParameterObject = _object;
		}

		public GameObject GetParameterObject(){
			return m_ParameterObject;
		}*/

		public void Copy( BehaviourEvent _event )
		{
			if( _event == null )
				return;

			base.Copy( _event );

			ParameterString = _event.ParameterString;
			ParameterInteger = _event.ParameterInteger;
			ParameterFloat = _event.ParameterFloat;
			ParameterBoolean = _event.ParameterBoolean;
			//SetParameterObject( _event.GetParameterObject() );
		}




	}
}
	
