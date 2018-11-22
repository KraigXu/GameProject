// ##############################################################################
//
// ice_objects_animation.cs | ICE.World.Objects.AnimationObject | AnimationDataObject | AnimatorParameterObject
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

using ICE;
using ICE.World;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class AnimatorParameterObject : ICEDataObject
	{
		public AnimatorParameterObject(){}
		public AnimatorParameterObject( AnimatorParameterObject _parameter ) : base( _parameter ) {
			Copy( _parameter );
		}

		public void Copy( AnimatorParameterObject _parameter )
		{
			base.Copy( _parameter );

			UseDynamicValue = _parameter.UseDynamicValue;
			Name = _parameter.Name;
			Type = _parameter.Type;
			IntegerValueType = _parameter.IntegerValueType;
			IntegerValue = _parameter.IntegerValue;
			FloatValueType = _parameter.FloatValueType;
			FloatValue = _parameter.FloatValue;
			BooleanValueType = _parameter.BooleanValueType;
			BooleanValue = _parameter.BooleanValue;

			TriggerIntervalMin = _parameter.TriggerIntervalMin;
			TriggerIntervalMax = _parameter.TriggerIntervalMax;
			TriggerIntervalMaximum = _parameter.TriggerIntervalMaximum;

			UseEnd = _parameter.UseEnd;
		}

		public bool UseDynamicValue = false;
		public string Name = "";
		private int m_HashId = 0;
		public int HashId{
			get{ return m_HashId = ( m_HashId == 0 ? Animator.StringToHash( Name ) : m_HashId ); }
		}
		public AnimatorControllerParameterType Type = AnimatorControllerParameterType.Float;

		public DynamicIntegerValueType IntegerValueType = DynamicIntegerValueType.undefined;
		public int IntegerValue = 0;

		public DynamicFloatValueType FloatValueType = DynamicFloatValueType.MoveAngularSpeed;
		public float FloatValue = 0;

		public DynamicBooleanValueType BooleanValueType = DynamicBooleanValueType.IsGrounded;
		public bool BooleanValue = false;

		public float TriggerIntervalMin = 0;
		public float TriggerIntervalMax = 0;
		public float TriggerIntervalMaximum = 5;

		public bool UseEnd = false;

		protected float m_TriggerTimer = 0;
		protected float m_TriggerInterval = 0;

		protected bool m_TriggerIsRunning = false;

		public bool Start()
		{
			if( Type == AnimatorControllerParameterType.Trigger && m_TriggerIsRunning == false )
			{
				m_TriggerIsRunning = true;
				m_TriggerTimer = 0;
				m_TriggerInterval = UnityEngine.Random.Range( TriggerIntervalMin, TriggerIntervalMax );

				if( m_TriggerInterval == 0 )
					return true;
			}
				
			return false;
		}

		public bool Update()
		{
			if( Type == AnimatorControllerParameterType.Trigger && m_TriggerIsRunning == true && m_TriggerInterval > 0 )
			{
				m_TriggerTimer += Time.deltaTime;
				if( m_TriggerTimer >= m_TriggerInterval )
				{
					m_TriggerTimer = 0;
					m_TriggerInterval = UnityEngine.Random.Range( TriggerIntervalMin, TriggerIntervalMax );
					return true;
				}
			}

			return false;
		}

		public bool End()
		{
			if( Type == AnimatorControllerParameterType.Trigger )
			{
				m_TriggerIsRunning = false;
				m_TriggerTimer = 0;
				m_TriggerInterval = 0;

				if( UseEnd )
					return true;
			}

			return false;
		}
	}


	[System.Serializable]
	public class AnimationEventObject : ICEObject
	{
		public AnimationEventObject(){}
		public AnimationEventObject( AnimationEvent _event ){ Copy( _event ); }
		public AnimationEventObject( AnimationEventObject _event ){ Copy( _event ); }

		public void Copy( AnimationEvent _event )
		{
			if( _event == null )
				return;
			
			this.m_IsUpdateRequired = false;

			this.IsActive = false;
			this.UseCustomFunction = false;
			this.MethodName = _event.functionName;
			this.Time = _event.time;

			this.ParameterType = AnimationEventParameterType.None;
			this.ParameterString = _event.stringParameter;
			this.ParameterFloat = _event.floatParameter;
			this.ParameterInteger = _event.intParameter;
			//TODO: this.ParameterBoolean = false;	
		}


		public void SetInfo( BehaviourEventInfo _event )
		{
			this.MethodName = _event.FunctionName;

			if( _event.ParameterType == BehaviourEventParameterType.Float )
				this.ParameterType = AnimationEventParameterType.Float;
			else if( _event.ParameterType == BehaviourEventParameterType.Integer )
				this.ParameterType = AnimationEventParameterType.Integer;
			else if( _event.ParameterType == BehaviourEventParameterType.String )
				this.ParameterType = AnimationEventParameterType.String;
			else
				this.ParameterType = AnimationEventParameterType.None;
		}

		public void Copy( AnimationEventObject _event )
		{
			this.m_IsUpdateRequired = _event.IsUpdateRequired;

			this.IsActive = _event.IsActive;
			this.UseCustomFunction = _event.UseCustomFunction;
			this.MethodName = _event.MethodName;
			this.Time = _event.Time;

			this.ParameterType = AnimationEventParameterType.None;
			this.ParameterString = _event.ParameterString;
			this.ParameterFloat = _event.ParameterFloat;
			this.ParameterInteger = _event.ParameterInteger;
			//TODO: this.ParameterBoolean = false;	
		}

		public override void Reset()
		{
			this.m_IsUpdateRequired = false;

			this.IsActive = false;
			this.UseCustomFunction = false;
			this.MethodName = "";
			this.Time = 0;

			this.ParameterType = AnimationEventParameterType.None;
			this.ParameterString = "";
			this.ParameterFloat = 0f;
			this.ParameterInteger = 0;
			//TODO: this.ParameterBoolean = false;	
		}

		public bool IsActive;
		private bool m_IsUpdateRequired;
		public bool IsUpdateRequired{
			get{ return m_IsUpdateRequired; }
		}
		public bool UseCustomFunction;
		public string MethodName;

		public AnimationEventParameterType ParameterType;
		public string ParameterString; 
		public int ParameterInteger;
		public float ParameterFloat; 
		//TODO: public bool ParameterBoolean;

		public float Time;

		public void SetAnimationEvent( AnimationEvent _event )
		{
			if( _event == null )
				return;

			IsActive = true;
			MethodName = _event.functionName;
			Time = _event.time;

			ParameterString = _event.stringParameter;
			ParameterFloat = _event.floatParameter;
			ParameterInteger = _event.intParameter;
			
		}

		public AnimationEvent GetAnimationEvent()
		{
			return GetAnimationEvent( null );
		}

		public AnimationEvent GetAnimationEvent( AnimationEvent _event )
		{
			if( _event == null )
				_event = new AnimationEvent();

			_event.functionName = MethodName;
			_event.time = Time;
			_event.stringParameter = ParameterString;
			_event.floatParameter = ParameterFloat;
			_event.intParameter = ParameterInteger;
			_event.messageOptions = SendMessageOptions.DontRequireReceiver;

			return _event;
		}

		public bool UpdateRequired( AnimationEvent _event )
		{
			if( _event == null )
				m_IsUpdateRequired = true;
			else if( _event.functionName != MethodName )
				m_IsUpdateRequired = true;
			else if( _event.time != Time )
				m_IsUpdateRequired = true;
			else if( _event.stringParameter != ParameterString )
				m_IsUpdateRequired = true;
			else if( _event.floatParameter != ParameterFloat )
				m_IsUpdateRequired = true;
			else if( _event.intParameter != ParameterInteger )
				m_IsUpdateRequired = true;
			else
				m_IsUpdateRequired = false;

			return m_IsUpdateRequired;
		}
	}

	[System.Serializable]
	public class AnimationEventsObject : ICEDataObject
	{
		public AnimationEventsObject(){}
		public AnimationEventsObject( AnimationEventsObject _events ) : base( _events ) {
			Copy( _events );
		}

		public void Copy( AnimationEventsObject _events )
		{
			Events.Clear();
			foreach( AnimationEventObject _event in _events.Events )
				Events.Add( _event );
		}

		[SerializeField]
		private List<AnimationEventObject> m_Events = null;
		public List<AnimationEventObject> Events{
			get{ return m_Events = ( m_Events == null ? new List<AnimationEventObject>() : m_Events ); }
			set{ 
				Events.Clear();
				if( value == null ) return;	
				foreach( AnimationEventObject _event in value )
					Events.Add( new AnimationEventObject( _event ) );			
			} 
		}

		public bool UpdateRequired( AnimationEvent[] _events )
		{
			foreach( AnimationEventObject _data in Events )
				if( _data.UpdateRequired( GetAnimationEventByName( _events, _data.MethodName ) ) )
					return true;

			return false;
		}

		public void UpdateAnimationEvents( AnimationEvent[] _events )
		{
			//foreach( AnimationEventData _event in Events ) 
			for( int i = 0 ; i < Events.Count ; i++)
				Events[i].IsActive = false;
				
			for( int i = 0 ; i < _events.Length ; i++ )
			{
				AnimationEventObject _event = GetAnimationEventData( _events[i].functionName );
				if( _event == null )
					AddAnimationDataEvent( _events[i], true );
				else
					_event.IsActive = true;				
			}
		}

		public AnimationEvent[] GetAnimationEvents()
		{
			List<AnimationEvent> _events = new List<AnimationEvent>();
			for( int i = 0 ; i < Events.Count ; i++ )
			{
				if( Events[i].IsActive )
					_events.Add( Events[i].GetAnimationEvent() );
			}
			return _events.ToArray();
		}

		public void AddAnimationDataEvent( AnimationEvent _event, bool _active )
		{
			if( _event == null || string.IsNullOrEmpty( _event.functionName ) || GetAnimationEventData( _event.functionName ) != null )
				return;

			Events.Add( new AnimationEventObject( _event ) );
		}

		public AnimationEventObject GetAnimationEventData( string _name )
		{
			foreach( AnimationEventObject _event in Events ) 
				if( _event.MethodName == _name )
					return _event;
	
			return null;
		}

		public AnimationEvent GetAnimationEventByName( AnimationEvent[] _events, string _name )
		{
			foreach( AnimationEvent _event in _events ) 
				if( _event.functionName == _name )
					return _event;

			return null;
		}

	}

	[System.Serializable]
	public struct AnimatorInterface
	{
		public void Copy( AnimatorInterface _data )
		{
			StateName = _data.StateName;
			Name = _data.Name;
			Index = _data.Index;
			Length = _data.Length;
			AutoSpeed = _data.AutoSpeed;
			Speed = _data.Speed;
			TransitionDuration = _data.TransitionDuration;
			AutoTransitionDuration = _data.AutoTransitionDuration;
			Type = _data.Type;
			ApplyRootMotion = _data.ApplyRootMotion;

			CopyParameters( _data.Parameters );
		}

		public void CopyParameters( List<AnimatorParameterObject> _parameters )
		{
			Parameters.Clear();
			foreach( AnimatorParameterObject _parameter in _parameters ){
				Parameters.Add( new AnimatorParameterObject( _parameter ) );
			}	
		}

		[SerializeField]
		private List<AnimatorParameterObject> m_Parameters;
		public List<AnimatorParameterObject> Parameters{
			get{ return m_Parameters = ( m_Parameters == null ? new List<AnimatorParameterObject>() : m_Parameters ); }
			set{ CopyParameters( value ); }
		}

		public string StateName;
		public string Name;
		public int Index;
		public float Length;
		public bool AutoSpeed;
		public float Speed;
		public float TransitionDuration;
		public bool AutoTransitionDuration;

		public bool ShowDetails;

		public AnimatorControlType Type;
		public bool ApplyRootMotion;

		public void Init()
		{
			this.AutoSpeed = false;
			this.Speed = 1;
			this.TransitionDuration = 0.05f;
		}
	}

	[System.Serializable]
	public struct AnimationInterface
	{
		public void Copy( AnimationInterface _data )
		{
			Name = _data.Name;
			Index = _data.Index;
			Length = _data.Length;
			wrapMode = _data.wrapMode;
			DefaultSpeed = _data.DefaultSpeed;
			DefaultWrapMode = _data.DefaultWrapMode;
			Speed = _data.Speed;
			AutoSpeed = _data.AutoSpeed;
			AutoTransitionDuration = _data.AutoTransitionDuration;
			TransitionDuration = _data.TransitionDuration;
		}

		public string Name;
		public int Index;
		public float Length;
		public WrapMode wrapMode;
		public float DefaultSpeed;
		public WrapMode DefaultWrapMode;
		public float Speed;
		public bool AutoSpeed;
		public bool AutoTransitionDuration;
		public float TransitionDuration;


		public void Init()
		{
			this.AutoSpeed = false;
			this.AutoTransitionDuration = false;
			this.Speed = 1;
			this.TransitionDuration = 0.25f;
		}
	}

	[System.Serializable]
	public struct AnimationClipInterface
	{
		public void Copy( AnimationClipInterface _data )
		{
			Clip = _data.Clip;
			TransitionDuration = _data.TransitionDuration;
			AutoTransitionDuration = _data.AutoTransitionDuration;
		}

		[XmlIgnore]
		public AnimationClip Clip;

		public  float TransitionDuration;
		public bool AutoTransitionDuration;

		public void Init()
		{
			//this.AutoSpeed = false;
			//this.Speed = 1;
			this.TransitionDuration = 0.25f;
		}
	}
		
	[System.Serializable]
	public class AnimationDataObject : ICEDataObject
	{
		public AnimationDataObject(){}
		public AnimationDataObject( AnimationDataObject _data ) : base( _data ){ Copy( _data ); }

		public void Copy( AnimationDataObject _data )
		{
			base.Copy( _data );

			AllowInterfaceSelector = _data.AllowInterfaceSelector;
			InterfaceType = _data.InterfaceType;

			Animator.Copy( _data.Animator );
			Animation.Copy( _data.Animation );
			Clip.Copy( _data.Clip );
			Events.Copy( _data.Events );
		}

		public bool AllowInterfaceSelector;
		public AnimationInterfaceType InterfaceType;
		public AnimatorInterface Animator;
		public AnimationInterface Animation;
		public AnimationClipInterface Clip;

		[SerializeField]
		private AnimationEventsObject m_Events = null;
		public AnimationEventsObject Events{
			get{ return m_Events = ( m_Events == null ? new AnimationEventsObject() : m_Events ); }
			set{ Events.Copy( value ); }
		}


		public float GetAnimationLength()
		{
			if( InterfaceType == AnimationInterfaceType.LEGACY )
				return Animation.Length;
			else if( InterfaceType == AnimationInterfaceType.MECANIM )
				return Animator.Length;
			else if( InterfaceType == AnimationInterfaceType.CLIP && Clip.Clip != null )
				return Clip.Clip.length;
			else 
				return 0;
		}

		public string GetAnimationName()
		{
			if( InterfaceType == AnimationInterfaceType.LEGACY )
				return Animation.Name;
			else if( InterfaceType == AnimationInterfaceType.MECANIM )
				return Animator.Name;
			else if( InterfaceType == AnimationInterfaceType.CLIP && Clip.Clip != null )
				return Clip.Clip.name;
			else 
				return "";
		}

		public bool UseRootMotion{
			get{ 
				if( InterfaceType == AnimationInterfaceType.MECANIM )
					return Animator.ApplyRootMotion;
				else
					return false;
			}
		}

		public void Init()
		{
			Animator.Init();
			Animation.Init();
			Clip.Init();
		}

		public bool IsValid{
			get{
				if( Enabled == true &&
					InterfaceType != AnimationInterfaceType.NONE )
					return true;
				else
					return false;
			}
		}
	}

	[System.Serializable]
	public class AnimationPlayerObject : ICEOwnerObject
	{
		public AnimationPlayerObject(){}
		public AnimationPlayerObject( ICEWorldBehaviour _component ) : base( _component ) { Init( _component ); }

		protected Animation m_Animation = null;
		public Animation AnimationComponent{
			get{ return m_Animation = ( m_Animation == null && Owner != null ? Owner.GetComponentInChildren<Animation>() : m_Animation ); }
		}

		protected Animator m_Animator = null;
		public Animator AnimatorComponent{
			get{ return m_Animator = ( m_Animator == null && Owner != null ? Owner.GetComponentInChildren<Animator>() : m_Animator ); }
		}


		public delegate void OnCustomAnimationEvent();
		public event OnCustomAnimationEvent OnCustomAnimation;

		public delegate void OnCustomAnimationUpdateEvent();
		public event OnCustomAnimationUpdateEvent OnCustomAnimationUpdate;

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			if( Owner != null )
			{
				m_Animation = Owner.GetComponentInChildren<Animation>();
				m_Animator = Owner.GetComponentInChildren<Animator>();
			}
		}

		public void Play()
		{
			if( AnimatorComponent != null  )
				AnimatorComponent.StartPlayback();
			else if( AnimationComponent != null && ! AnimationComponent.isPlaying )
				AnimationComponent.Play();				
		}

		public void Play( AnimationDataObject _data )
		{
			if( _data == null || _data.Enabled == false || _data.InterfaceType == AnimationInterfaceType.NONE )
				return;

			if( _data.InterfaceType == AnimationInterfaceType.LEGACY )
			{
				if ( AnimationComponent == null ) 
				{	
					PrintErrorLog( this, "Missing Animation Component!" );
					return;
				}

				AnimationComponent[ _data.Animation.Name ].wrapMode = _data.Animation.wrapMode;
				AnimationComponent[ _data.Animation.Name ].speed = _data.Animation.Speed;//Mathf.Clamp( m_BehaviourData.MoveVelocity. controller.velocity.magnitude, 0.0, runMaxAnimationSpeed);
				AnimationComponent.CrossFade( _data.Animation.Name, _data.Animation.TransitionDuration );	

			}
			else if( _data.InterfaceType == AnimationInterfaceType.CLIP )
			{
				if( AnimationComponent != null && _data.Clip.Clip != null ) 
				{
					AnimationComponent.AddClip( _data.Clip.Clip, _data.Clip.Clip.name );
					AnimationComponent.CrossFade( _data.Clip.Clip.name, _data.Clip.TransitionDuration );	
				}
			}
			else if( _data.InterfaceType == AnimationInterfaceType.MECANIM )
			{
				if( AnimatorComponent == null || AnimatorComponent.runtimeAnimatorController == null || ! AnimatorComponent.isInitialized ) 
					return;

				if( AnimatorComponent.applyRootMotion != _data.Animator.ApplyRootMotion )
					AnimatorComponent.applyRootMotion = _data.Animator.ApplyRootMotion;

				if( _data.Animator.Type == AnimatorControlType.DIRECT )
					UpdateAnimatorState( _data.Animator );
				else if( _data.Animator.Type == AnimatorControlType.ADVANCED )
					UpdateAnimatorParameter( _data.Animator, false );
			}
			else if( _data.InterfaceType == AnimationInterfaceType.CUSTOM )
			{
				if( OnCustomAnimation != null )
					OnCustomAnimation();
			}
		}

		public void Stop( AnimationDataObject _data )
		{
			if( _data == null || _data.Enabled == false || _data.InterfaceType == AnimationInterfaceType.NONE )
				return;

			if( _data.InterfaceType == AnimationInterfaceType.MECANIM )
			{
				if( _data.Animator.Type == AnimatorControlType.ADVANCED )
					UpdateAnimatorParameter( _data.Animator, true );
			}
			else if( _data.InterfaceType == AnimationInterfaceType.CUSTOM )
			{
				if( OnCustomAnimationUpdate != null )
					OnCustomAnimationUpdate();
			}
		}

		public void Update( AnimationDataObject _data )
		{
			if( _data == null || _data.Enabled == false || _data.InterfaceType == AnimationInterfaceType.NONE )
				return;

			if( _data.InterfaceType == AnimationInterfaceType.MECANIM )
			{
				if( _data.Animator.Type == AnimatorControlType.ADVANCED )
					UpdateAnimatorParameter( _data.Animator, false );
				else if( _data.Animator.Type == AnimatorControlType.DIRECT )
					UpdateAnimatorState( _data.Animator );
			}
			else if( _data.InterfaceType == AnimationInterfaceType.CUSTOM )
			{
				if( OnCustomAnimationUpdate != null )
					OnCustomAnimationUpdate();
			}
		}

		public virtual void UpdateAnimatorState( AnimatorInterface _animator_data )
		{
			if( AnimatorComponent == null || AnimatorComponent.runtimeAnimatorController == null || ! AnimatorComponent.isInitialized || AnimatorComponent.IsInTransition(0) ) 
				return;

			AnimatorStateInfo _state = AnimatorComponent.GetCurrentAnimatorStateInfo(0);

			if( _state.IsName( _animator_data.StateName ) )
				return;

			AnimatorComponent.speed = _animator_data.Speed;

			//Debug.Log( _animator_data.StateName + " - " + Time.frameCount );

			if( _animator_data.TransitionDuration == 0 )
				AnimatorComponent.Play( _animator_data.StateName );
			else
				AnimatorComponent.CrossFade( _animator_data.StateName, _animator_data.TransitionDuration ); 
			
			//m_AnimatorAutoSpeed = _rule.Animation.Animator.AutoSpeed;
			/*if( _rule.Animation.Animator.AutoSpeed )
						m_Animator.speed = Mathf.Clamp( m_BehaviourData.MoveVelocity. controller.velocity.magnitude, 0.0, runMaxAnimationSpeed);
					else*/

		}

		public virtual void UpdateAnimatorParameter( AnimatorInterface _animator_data, bool _stop )
		{
			if( AnimatorComponent == null || AnimatorComponent.runtimeAnimatorController == null || ! AnimatorComponent.isInitialized ) 
				return;
			
			foreach( AnimatorParameterObject _parameter in _animator_data.Parameters )
			{
				if( _parameter.Enabled )
				{
					if( _parameter.Type == AnimatorControllerParameterType.Bool && ( ( ! _stop && ! _parameter.UseEnd ) || ( _stop && _parameter.UseEnd ) ) )
					{
						if( _parameter.UseDynamicValue )
							AnimatorComponent.SetBool( _parameter.HashId, ( OwnerComponent != null ? OwnerComponent.GetDynamicBooleanValue( _parameter.BooleanValueType ) :_parameter.BooleanValue ) );
						else
							AnimatorComponent.SetBool( _parameter.HashId, _parameter.BooleanValue );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Float && ( ( ! _stop && ! _parameter.UseEnd ) || ( _stop && _parameter.UseEnd ) ) )
					{
						if( _parameter.UseDynamicValue )
							AnimatorComponent.SetFloat( _parameter.HashId, ( OwnerComponent != null ? OwnerComponent.GetDynamicFloatValue( _parameter.FloatValueType ):_parameter.FloatValue ) );
						else
							AnimatorComponent.SetFloat( _parameter.HashId, _parameter.FloatValue );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Int && ( ( ! _stop && ! _parameter.UseEnd ) || ( _stop && _parameter.UseEnd ) ) )
					{
						if( _parameter.UseDynamicValue )
							AnimatorComponent.SetInteger( _parameter.HashId, ( OwnerComponent != null ? OwnerComponent.GetDynamicIntegerValue( _parameter.IntegerValueType ):_parameter.IntegerValue ) );
						else
							AnimatorComponent.SetInteger( _parameter.HashId, _parameter.IntegerValue );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Trigger )
					{
						if( ( _stop && _parameter.End() ) || ( ! _stop && ( _parameter.Start() || _parameter.Update() ) ) )							
							AnimatorComponent.SetTrigger( _parameter.HashId );
					}
				}
			}
		}

		public void Break()
		{
			if( AnimatorComponent != null  )
			{
				AnimatorComponent.StopPlayback();
				/*
				AnimationInfo[] _animator_info = m_Animator.GetCurrentAnimationClipState(0);
				
				
				if( _animator_info.Length >> ){

				}else{
					for(int idx=0;idx<_animator_info.Length;idx++){

						_animator_info[idx].
					}
				}*/
			}
			else if( AnimationComponent != null && AnimationComponent.isPlaying )
				AnimationComponent.Stop();				
		}
	}
}
