// ##############################################################################
//
// ice_objects_timer.cs
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public class ICEDirectTimerObject : ICETimerObject
	{
		public ICEDirectTimerObject(){}
		public ICEDirectTimerObject( ICEDirectTimerObject _object ) : base( _object ) { SetImpulseData( _object ); }

		public override bool TimerEnabled{
			get{ return Enabled; }
			set{ Enabled = value; }
		}
	}

	[System.Serializable]
	public abstract class ICETimerObject : ICEDataObject
	{
		public ICETimerObject(){}
		public ICETimerObject( ICETimerObject _object ) : base( _object ) { SetImpulseData( _object ); }

		public void SetImpulseData( ICETimerObject _object )
		{
			if( _object == null )
				return;

			TimerEnabled = _object.TimerEnabled;

			Enabled = _object.Enabled;

			UseInterval = _object.UseInterval;
			UseEnd = _object.UseEnd;
			UseTrigger = _object.UseTrigger;

			InitialImpulsTime = _object.InitialImpulsTime;
			InitialImpulsTimeMaximum = _object.InitialImpulsTimeMaximum;

			ImpulseLimitMin = _object.ImpulseLimitMin;
			ImpulseLimitMax = _object.ImpulseLimitMax;
			ImpulseLimitMaximum = _object.ImpulseLimitMaximum;

			ImpulseIntervalMin = _object.ImpulseIntervalMin;
			ImpulseIntervalMax = _object.ImpulseIntervalMax;
			ImpulseIntervalMaximum = _object.ImpulseIntervalMaximum;

			ImpulseSequenceLimitMin = _object.ImpulseSequenceLimitMin;
			ImpulseSequenceLimitMax = _object.ImpulseSequenceLimitMax;
			ImpulseSequenceLimitMaximum = _object.ImpulseSequenceLimitMaximum;

			ImpulseSequenceBreakLengthMin = _object.ImpulseSequenceBreakLengthMin;
			ImpulseSequenceBreakLengthMax = _object.ImpulseSequenceBreakLengthMax;
			ImpulseSequenceBreakLengthMaximum = _object.ImpulseSequenceBreakLengthMaximum;

			SetActive( false );
		}

		private bool m_Active = false;
		public bool Active{
			get{ return m_Active; }
		}
			
		[SerializeField]
		private bool m_TimerEnabled = true;
		public virtual bool TimerEnabled{
			get{ return ( Enabled == false ? false : m_TimerEnabled ); }
			set{ m_TimerEnabled = value; }
		}

		public bool TimerFoldout = false;

		public bool UseInterval = false;
		public bool UseEnd = false;
		public bool UseTrigger = false;

		public float InitialImpulsTime = 0;
		public float InitialImpulsTimeMaximum = 60;

		private int m_ImpulseLimitCounter = 0;
		private int m_ImpulseLimit = 0;
		public int ImpulseLimitMin = 0;
		public int ImpulseLimitMax = 0;
		public int ImpulseLimitMaximum = 5;

		private float m_ImpulseIntervalTimer = 0;
		private float m_ImpulseInterval = 0;
		public float ImpulseIntervalMin = 0;
		public float ImpulseIntervalMax = 0;
		public float ImpulseIntervalMaximum = 5;

		private int m_ImpulseSequenceLimitCounter = 0;
		private int m_ImpulseSequenceLimit = 0;
		public int ImpulseSequenceLimitMin = 0;
		public int ImpulseSequenceLimitMax = 0;
		public int ImpulseSequenceLimitMaximum = 10;

		private float m_ImpulseSequenceBreakLengthTimer = 0;
		private float m_ImpulseSequenceBreakLength = 0;
		public float ImpulseSequenceBreakLengthMin = 0;
		public float ImpulseSequenceBreakLengthMax = 0;
		public float ImpulseSequenceBreakLengthMaximum = 10;

		public void ResetTimer()
		{
			m_Active = false;
			m_ImpulseLimit = 0;
			m_ImpulseSequenceLimit = 0;
			m_ImpulseInterval = 0;
			m_ImpulseSequenceBreakLength = 0;

			m_ImpulseLimitCounter = 0;
			m_ImpulseSequenceLimitCounter = 0;	
			m_ImpulseIntervalTimer = 0;
			m_ImpulseSequenceBreakLengthTimer = 0;
		}

		public void SetActive( bool _start ){

			if( _start && ! m_Active && TimerEnabled )
			{
				m_Active = true;
				if( UseInterval )
				{
					m_ImpulseInterval = InitialImpulsTime;
					m_ImpulseLimit = Random.Range( ImpulseLimitMin, ImpulseLimitMax );
					m_ImpulseSequenceLimit = Random.Range( ImpulseSequenceLimitMin, ImpulseSequenceLimitMax );
					m_ImpulseSequenceBreakLength = Random.Range( ImpulseSequenceBreakLengthMin, ImpulseSequenceBreakLengthMax );
				}
				else if( UseEnd )
				{
					m_ImpulseInterval = 0;
					m_ImpulseLimit = 0;
					m_ImpulseSequenceLimit = 0;
					m_ImpulseSequenceBreakLength = 0;
				}
				else
				{
					m_ImpulseInterval = InitialImpulsTime;
					m_ImpulseLimit = 1;
					m_ImpulseSequenceLimit = 0;
					m_ImpulseSequenceBreakLength = 0;
				}

				m_ImpulseLimitCounter = 0;
				m_ImpulseSequenceLimitCounter = 0;	
				m_ImpulseIntervalTimer = 0;
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
			else if( ! _start )
				ResetTimer();
		}
			
		public virtual void StartWithImpulsLimit( int _limit ){

			ResetTimer();
			SetActive( Enabled );
			m_ImpulseLimit = _limit;
		}

		public virtual void Start( bool _reset ){

			if( _reset )
				ResetTimer();
			
			SetActive( Enabled );
		}

		public virtual void Start(){
			SetActive( Enabled );
		}

		public virtual bool Stop(){
			SetActive( false );

			return ( TimerEnabled && UseEnd ? true : false ); 
		}

		public virtual bool Update()
		{
			if( ! TimerEnabled || ! m_Active || UseEnd ) 
				return false;

			// False whenever the specified impulse limit was reached
			if( m_ImpulseLimit > 0 && m_ImpulseLimitCounter >= m_ImpulseLimit ) 
				return false;

			bool _impulse = false;

			// if there is sequence limit or the counter is within the defined limit 
			if( m_ImpulseSequenceLimit == 0 || m_ImpulseSequenceLimitCounter < m_ImpulseSequenceLimit )
			{
				if( m_ImpulseInterval == 0 || m_ImpulseIntervalTimer >= m_ImpulseInterval )
				{
					_impulse = true;

					// prepare next interval and reset timer
					m_ImpulseInterval = Random.Range( ImpulseIntervalMin, ImpulseIntervalMax );
					m_ImpulseIntervalTimer = 0;

					// increase sequence limit counter after sending a message
					if( m_ImpulseSequenceLimit > 0 )
						m_ImpulseSequenceLimitCounter++;

					// increase limit counter after sending a message
					if( m_ImpulseLimit > 0 )
						m_ImpulseLimitCounter++;
				}
				else
					m_ImpulseIntervalTimer += Time.deltaTime;
			}

			// if the sequence limit is reached we have to do a break by using the m_ImpulseSequenceBreakLength
			else if( m_ImpulseSequenceBreakLength == 0 || m_ImpulseSequenceBreakLengthTimer >= m_ImpulseSequenceBreakLength )
			{
				// prepare next sequence and reset counter
				m_ImpulseSequenceLimit = Random.Range( ImpulseSequenceLimitMin, ImpulseSequenceLimitMax );
				m_ImpulseSequenceLimitCounter = 0;

				// prepare next break and reset timer
				m_ImpulseSequenceBreakLength = Random.Range( ImpulseSequenceBreakLengthMin, ImpulseSequenceBreakLengthMax );
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
			else
				m_ImpulseSequenceBreakLengthTimer +=  Time.deltaTime;

			return _impulse;
		}
	}


	[System.Serializable]
	public abstract class ICEImpulsTimerObject : ICEOwnerObject
	{
		public ICEImpulsTimerObject(){}
		public ICEImpulsTimerObject( ICEWorldBehaviour _component ) : base( _component ){} 
		public ICEImpulsTimerObject( ICEImpulsTimerObject _object ) : base( _object as ICEOwnerObject ){ Copy( _object ); }

		public void Copy( ICEImpulsTimerObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			TimerEnabled = _object.TimerEnabled;

			Enabled = _object.Enabled;

			UseInterval = _object.UseInterval;
			UseEnd = _object.UseEnd;
			UseTrigger = _object.UseTrigger;

			InitialImpulsTime = _object.InitialImpulsTime;
			InitialImpulsTimeMaximum = _object.InitialImpulsTimeMaximum;

			ImpulseLimitMin = _object.ImpulseLimitMin;
			ImpulseLimitMax = _object.ImpulseLimitMax;
			ImpulseLimitMaximum = _object.ImpulseLimitMaximum;

			ImpulseIntervalMin = _object.ImpulseIntervalMin;
			ImpulseIntervalMax = _object.ImpulseIntervalMax;
			ImpulseIntervalMaximum = _object.ImpulseIntervalMaximum;

			ImpulseSequenceLimitMin = _object.ImpulseSequenceLimitMin;
			ImpulseSequenceLimitMax = _object.ImpulseSequenceLimitMax;
			ImpulseSequenceLimitMaximum = _object.ImpulseSequenceLimitMaximum;

			ImpulseSequenceBreakLengthMin = _object.ImpulseSequenceBreakLengthMin;
			ImpulseSequenceBreakLengthMax = _object.ImpulseSequenceBreakLengthMax;
			ImpulseSequenceBreakLengthMaximum = _object.ImpulseSequenceBreakLengthMaximum;

			SetActive( false );
		}

		public void SetImpulseData( ICEImpulsTimerObject _object )
		{
			if( _object == null )
				return;

			TimerEnabled = _object.TimerEnabled;

			Enabled = _object.Enabled;

			UseInterval = _object.UseInterval;
			UseEnd = _object.UseEnd;
			UseTrigger = _object.UseTrigger;

			InitialImpulsTime = _object.InitialImpulsTime;
			InitialImpulsTimeMaximum = _object.InitialImpulsTimeMaximum;

			ImpulseLimitMin = _object.ImpulseLimitMin;
			ImpulseLimitMax = _object.ImpulseLimitMax;
			ImpulseLimitMaximum = _object.ImpulseLimitMaximum;
			
			ImpulseIntervalMin = _object.ImpulseIntervalMin;
			ImpulseIntervalMax = _object.ImpulseIntervalMax;
			ImpulseIntervalMaximum = _object.ImpulseIntervalMaximum;

			ImpulseSequenceLimitMin = _object.ImpulseSequenceLimitMin;
			ImpulseSequenceLimitMax = _object.ImpulseSequenceLimitMax;
			ImpulseSequenceLimitMaximum = _object.ImpulseSequenceLimitMaximum;

			ImpulseSequenceBreakLengthMin = _object.ImpulseSequenceBreakLengthMin;
			ImpulseSequenceBreakLengthMax = _object.ImpulseSequenceBreakLengthMax;
			ImpulseSequenceBreakLengthMaximum = _object.ImpulseSequenceBreakLengthMaximum;

			SetActive( false );
		}

		private bool m_Active = false;
		public bool Active{
			get{ return m_Active; }
		}

		public void SetActive( bool _active ){

			if( _active && ! m_Active && TimerEnabled )
			{
				m_Active = true;
				if( UseInterval )
				{
					m_ImpulseInterval = InitialImpulsTime;
					m_ImpulseLimit = Random.Range( ImpulseLimitMin, ImpulseLimitMax );
					m_ImpulseSequenceLimit = Random.Range( ImpulseSequenceLimitMin, ImpulseSequenceLimitMax );
					m_ImpulseSequenceBreakLength = Random.Range( ImpulseSequenceBreakLengthMin, ImpulseSequenceBreakLengthMax );
				}
				else if( UseEnd )
				{
					m_ImpulseInterval = 0;
					m_ImpulseLimit = 0;
					m_ImpulseSequenceLimit = 0;
					m_ImpulseSequenceBreakLength = 0;
				}
				else
				{
					m_ImpulseInterval = InitialImpulsTime;
					m_ImpulseLimit = 1;
					m_ImpulseSequenceLimit = 0;
					m_ImpulseSequenceBreakLength = 0;
				}

				m_ImpulseLimitCounter = 0;
				m_ImpulseSequenceLimitCounter = 0;	
				m_ImpulseIntervalTimer = 0;
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
			else if( ! _active )
			{				
				if( UseEnd && m_Active )
					Action();
					
				m_Active = false;
				m_ImpulseLimit = 0;
				m_ImpulseSequenceLimit = 0;
				m_ImpulseInterval = 0;
				m_ImpulseSequenceBreakLength = 0;

				m_ImpulseLimitCounter = 0;
				m_ImpulseSequenceLimitCounter = 0;	
				m_ImpulseIntervalTimer = 0;
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
		}

		[SerializeField]
		private bool m_TimerEnabled = true;
		public bool TimerEnabled{
			get{ return ( Enabled == false ? false : m_TimerEnabled ); }
			set{ m_TimerEnabled = value; }
		}

		public bool TimerFoldout = false;

		public bool UseInterval = false;
		public bool UseEnd = false;
		public bool UseTrigger = false;

		public float InitialImpulsTime = 0;
		public float InitialImpulsTimeMaximum = 60;

		private int m_ImpulseLimitCounter = 0;
		private int m_ImpulseLimit = 0;
		public int ImpulseLimitMin = 0;
		public int ImpulseLimitMax = 0;
		public int ImpulseLimitMaximum = 5;

		private float m_ImpulseIntervalTimer = 0;
		private float m_ImpulseInterval = 0;
		public float ImpulseIntervalMin = 0;
		public float ImpulseIntervalMax = 0;
		public float ImpulseIntervalMaximum = 5;

		private int m_ImpulseSequenceLimitCounter = 0;
		private int m_ImpulseSequenceLimit = 0;
		public int ImpulseSequenceLimitMin = 0;
		public int ImpulseSequenceLimitMax = 0;
		public int ImpulseSequenceLimitMaximum = 10;

		private float m_ImpulseSequenceBreakLengthTimer = 0;
		private float m_ImpulseSequenceBreakLength = 0;
		public float ImpulseSequenceBreakLengthMin = 0;
		public float ImpulseSequenceBreakLengthMax = 0;
		public float ImpulseSequenceBreakLengthMaximum = 10;

		public virtual void Start(){
			SetActive( Enabled );
		}

		public virtual void Stop(){
			SetActive( false );
		}

		public virtual void Update()
		{
			if( ! m_Active || UseEnd || UseTrigger || ! TimerEnabled )
				return;

			if( m_ImpulseLimit > 0 && m_ImpulseLimitCounter >= m_ImpulseLimit )
				return;

			// if there is sequence limit or the counter is within the defined limit 
			if( m_ImpulseSequenceLimit == 0 || m_ImpulseSequenceLimitCounter < m_ImpulseSequenceLimit )
			{
				if( m_ImpulseInterval == 0 || m_ImpulseIntervalTimer >= m_ImpulseInterval )
				{
					Action();

					// prepare next interval and reset timer
					m_ImpulseInterval = Random.Range( ImpulseIntervalMin, ImpulseIntervalMax );
					m_ImpulseIntervalTimer = 0;

					// increase sequence limit counter after sending a message
					if( m_ImpulseSequenceLimit > 0 )
						m_ImpulseSequenceLimitCounter++;

					// increase limit counter after sending a message
					if( m_ImpulseLimit > 0 )
						m_ImpulseLimitCounter++;
				}
				else
					m_ImpulseIntervalTimer += Time.deltaTime;
			}

			// if the sequence limit is reached we have to do a break by using the m_ImpulseSequenceBreakLength
			else if( m_ImpulseSequenceBreakLength == 0 || m_ImpulseSequenceBreakLengthTimer >= m_ImpulseSequenceBreakLength )
			{
				// prepare next sequence and reset counter
				m_ImpulseSequenceLimit = Random.Range( ImpulseSequenceLimitMin, ImpulseSequenceLimitMax );
				m_ImpulseSequenceLimitCounter = 0;

				// prepare next break and reset timer
				m_ImpulseSequenceBreakLength = Random.Range( ImpulseSequenceBreakLengthMin, ImpulseSequenceBreakLengthMax );
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
			else
				m_ImpulseSequenceBreakLengthTimer +=  Time.deltaTime;	
		}

		protected virtual void Action(){}
	}

}
