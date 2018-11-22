// ##############################################################################
//
// ice_objects_lifespan.cs
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

using ICE.World;
using ICE.World.Objects;

namespace ICE.World.Objects
{
	public enum EntityLifespanType
	{
		NONE,
		LIFESPAN,
		AGING
	}

	[System.Serializable]
	public class LifespanObject : ICEOwnerObject
	{
		public LifespanObject(){}
		public LifespanObject( LifespanObject _object ) : base( _object ){}
		public LifespanObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public void Copy( LifespanObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			LifespanType = _object.LifespanType;

			LifespanMin = _object.LifespanMin;
			LifespanMax = _object.LifespanMax;
			LifespanMaximum = _object.LifespanMaximum;

			MaxAge = _object.MaxAge;	
			MaxAgeMaximum = _object.MaxAgeMaximum;


		}

		public EntityLifespanType LifespanType = EntityLifespanType.NONE;

		public bool UseAging{
			get{ return ( LifespanType == EntityLifespanType.AGING ? true : false ); }
			set{ LifespanType = ( value ? EntityLifespanType.AGING : EntityLifespanType.NONE ); }
		}
			
		/// <summary>
		/// The use lifespan.
		/// </summary>
		public bool UseLifespan{
			get{ return ( LifespanType == EntityLifespanType.LIFESPAN ? true : false ); }
			set{ LifespanType = ( value ? EntityLifespanType.LIFESPAN : EntityLifespanType.NONE ); }
		}

		/// <summary>
		/// Gets the total lifetime since the start
		/// </summary>
		/// <value>The total lifetime.</value>
		public float TotalLifetime{ 
			get{ return Time.time - m_InitTime; } 
		}

		/// <summary>
		/// Gets the lifetime since the last reset.
		/// </summary>
		/// <value>The lifetime.</value>
		public float Lifetime{ 			
			get{ return Time.time - m_ResetTime; } 
		}
		/// <summary>
		/// The init time.
		/// </summary>
		private float m_InitTime = 0;

		/// <summary>
		/// The reset time.
		/// </summary>
		private float m_ResetTime = 0;
	
		/// <summary>
		/// The age in seconds.
		/// </summary>
		protected float m_AgeInSeconds = 0;



		/// <summary>
		/// The lifespan minimum.
		/// </summary>
		public float LifespanMin = 0;
		/// <summary>
		/// The lifespan max.
		/// </summary>
		public float LifespanMax = 0;
		/// <summary>
		/// The lifespan default max.
		/// </summary>
		public float LifespanMaximum = 300;

		public float LifespanInPercent{		
			get{ return FixedPercent( 100 - ( 100/MaxAge*m_Age) ); }
		}

		/// <summary>
		/// The age in seconds.
		/// </summary>
		public float AgeInSeconds{
			get{ return m_AgeInSeconds; }
		}

		/// <summary>
		/// Gets the age in minutes.
		/// </summary>
		/// <value>The age in minutes.</value>
		public float AgeInMinutes{
			get{ return m_AgeInSeconds/60; }
		}

		/// <summary>
		/// Gets the age in hours.
		/// </summary>
		/// <value>The age in hours.</value>
		public float AgeInHours{
			get{ return m_AgeInSeconds/3600; }
		}


		protected float m_ExpectedLifespan = 0;
		public float ExpectedLifespan{		
			get{ return m_ExpectedLifespan; }
		}

		public float MaxAge = 60f;	
		public float MaxAgeMaximum = 3600f;

		protected float m_Age = 0.0f;
		public float Age{ 
			get{ return m_Age; }
		}

		public void SetAge( float _age ){ 
			m_Age = Mathf.Clamp( _age, 0, MaxAge );
		}

		/// <summary>
		/// Overrides the parent Init method to initiate the lifespan procedure
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public override void Init( ICEWorldBehaviour _owner )
		{
			base.Init( _owner );

			if( OwnerComponent == null )
				return;

			m_InitTime = Time.time;
			m_ResetTime = Time.time;

			m_ExpectedLifespan = UpdateRandomLifespan();
				
			OwnerComponent.OnUpdate += DoUpdate;

			if( UseLifespan )
				OwnerComponent.Invoke( "Remove", m_ExpectedLifespan );
		}

		public override void Reset()
		{
			m_ResetTime = Time.time;

			m_InitTime = Time.time;
			m_ExpectedLifespan = UpdateRandomLifespan();
			m_AgeInSeconds = 0;
			m_Age = 0;
		}


		/// <summary>
		/// Gets a random lifespan.
		/// </summary>
		/// <value>The random lifespan.</value>
		public float UpdateRandomLifespan(){
			return Random.Range( LifespanMin, LifespanMax ); 
		}


		/// <summary>
		/// Dos the update begin.
		/// </summary>
		private void DoUpdate()
		{

		}

		public virtual void Update()
		{
			if( UseAging )
				m_Age +=  Time.deltaTime;

			m_AgeInSeconds = Time.time - m_InitTime;

			if( UseLifespan || UseAging )
				PrintDebugLog( this, "Lifespan : " + ( ( UseLifespan && m_ExpectedLifespan > 0 ) ? m_ExpectedLifespan.ToString() : "disabled" ) + " - Age :" + ( UseAging ? AgeInMinutes.ToString() + " min." : "disabled" ) );
		}

		/// <summary>
		/// Returns valid rounded percent value
		/// </summary>
		/// <returns>The percent.</returns>
		/// <param name="_value">Value.</param>
		protected float FixedPercent( float _value ){
			return (float)System.Math.Round( Mathf.Clamp( _value, 0, 100 ), 2 );
		}

		/// <summary>
		/// Returns valid multiplier between 0 and 1
		/// </summary>
		/// <returns>The multiplier.</returns>
		/// <param name="_value">Value.</param>
		protected float FixedMultiplier( float _value ){
			return Mathf.Clamp01( _value );
		}
	}


}
