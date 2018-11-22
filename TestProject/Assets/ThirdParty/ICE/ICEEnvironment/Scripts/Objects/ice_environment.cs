// ##############################################################################
//
// ice_CreatureInventory.cs
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
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Environment;
using ICE.Environment.Objects;

namespace ICE.Environment.Objects
{

	[System.Serializable]
	public class DisplayDataObject : ICEOwnerObject{
		public DisplayDataObject(){}
		public DisplayDataObject( DisplayDataObject _object ) : base( _object ){ Copy( _object );}
		public DisplayDataObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public Text UITextDateTime = null;
		public string UITextDateTimeFormat = "dd.MM.yyyy HH:mm:ss";

		public Text UITextDay = null;
		public string UITextDayFormat = "Day #{0}";
		public bool UseFirstDay = true;

		public Text UITextDate = null;
		public Text UITextTime = null;

		public Text UITemperatur = null;
		public string UITemperaturFormat = "{0}°C";


		public bool UseShortTime = true;
	}

	[System.Serializable]
	public class DisplayObject : DisplayDataObject{
		public DisplayObject(){}
		public DisplayObject( DisplayDataObject _object ) : base( _object ){ Copy( _object );}
		public DisplayObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

	}

	[System.Serializable]
	public class WeatherDataObject : ICEOwnerObject{
		public WeatherDataObject(){}
		public WeatherDataObject( WeatherDataObject _object ) : base( _object ){ Copy( _object );}
		public WeatherDataObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public bool UseTemperature = false;
		public TemperatureScaleType TemperatureScale = TemperatureScaleType.CELSIUS;
		public float MinTemperature = -25;
		public float MaxTemperature = 50;
		public float TemperatureMaximum = 50;
		public AnimationCurve DaytimeTemperature = new AnimationCurve( new Keyframe[5]{
			new Keyframe( 0, 10.0f ),
			new Keyframe(5, 15.05f ),
			new Keyframe(12, 35.0f ),
			new Keyframe( 20, 25.01f ),
			new Keyframe( 24, 10.0f) } );

		public AnimationCurve AnnualAverageTemperature = new AnimationCurve( new Keyframe[7]{
			new Keyframe( 1, -15 ),
			new Keyframe( 3, -10 ),
			new Keyframe( 5, 15 ),
			new Keyframe( 7, 30 ),
			new Keyframe( 9, 35 ),
			new Keyframe( 11, 20 ),
			new Keyframe( 13, -15 ) } );



		public bool UseFog = false;
		public float FogInitialDensity = 0.1f;
		public AnimationCurve FogDensity = new AnimationCurve( new Keyframe[5]{
			new Keyframe( 0, 0.0f ),
			new Keyframe(5, 0.05f ),
			new Keyframe(12, 0.0f ),
			new Keyframe( 20, 0.01f ),
			new Keyframe( 24, 0.0f) } );

		public AnimationCurve FogProbability = new AnimationCurve( new Keyframe[7]{
			new Keyframe( 1, 0 ),
			new Keyframe( 3, 1 ),
			new Keyframe( 5, 0.5f ),
			new Keyframe( 7, 0 ),
			new Keyframe( 9, 0.25f ),
			new Keyframe( 11, 1 ),
			new Keyframe( 13, 0 ) } );
		
		public float FogDensityMin = 0.01f;
		public float FogDensityMax = 0.5f;

		public Color FogDayColor = new HSBColor( 0.0f ,0.0f, 1f ).ToColor();
		public Color FogNightColor = new HSBColor( 0.91f ,0.04f, 0.30f ).ToColor();
		public Color FogSunriseColor = new HSBColor( 0.14f ,0.22f, 0.64f ).ToColor();

		protected AstronomyObject m_Astronomy = null; 
		protected AstronomyObject Astronomy{
			get{ return m_Astronomy = ( m_Astronomy == null && ( OwnerComponent as ICEEnvironment ) != null ? ( OwnerComponent as ICEEnvironment ).Astronomy : m_Astronomy ); }
		}

		protected float m_DaytimePeakNormalized{
			get{ return ( Astronomy != null ? Astronomy.DaytimePeakNormalized : 1 ); }
		}

		protected float m_DaytimeInSeconds{
			get{ return ( Astronomy != null ? Astronomy.DaytimeInSeconds : 0 ); }
		}

		protected float m_DayOfYear{
			get{ return ( Astronomy != null ? Astronomy.DayOfYear : 0 ); }
		}

		protected float m_Temperature = 0;
		public float Temperature{
			get{ return m_Temperature; }
		}
	}

	[System.Serializable]
	public class WeatherObject : WeatherDataObject{
		public WeatherObject(){}
		public WeatherObject( WeatherObject _object ) : base( _object ){ Copy( _object );}
		public WeatherObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			// TODO: it could be that there is no main cam ...
			if( Application.isPlaying && Camera.main != null )
			{
				m_DefaultCameraBackgroundColor = Camera.main.backgroundColor;
				m_DefaultCameraCameraClearFlags = Camera.main.clearFlags;
			}
		}



		public Color FogColor{
			get{ 
				if( m_DaytimePeakNormalized < 0.25f )
					return Color.Lerp( FogNightColor, FogSunriseColor, MathTools.Normalize( m_DaytimePeakNormalized, 0f, 0.25f ) ); 
				else if( m_DaytimePeakNormalized >= 0.25f )
					return Color.Lerp( FogSunriseColor, FogDayColor, MathTools.Normalize( m_DaytimePeakNormalized, 0.25f, 1f ) ); 
				else
					return Color.Lerp( FogNightColor, FogDayColor, m_DaytimePeakNormalized ); 
			}
		}

		protected Color m_DefaultCameraBackgroundColor;
		protected CameraClearFlags m_DefaultCameraCameraClearFlags;
		public void Update()
		{
			if( ! Enabled )
				return;

			float _hours = (24f/86400f)*m_DaytimeInSeconds;
			float _month = 1 + ((12f/365f)*m_DayOfYear);

			if( UseTemperature )
			{
				float _temp_daytime = DaytimeTemperature.Evaluate( _hours );
				float _temp_annual = AnnualAverageTemperature.Evaluate( _month );
				float _temp_avarage = ( _temp_daytime + _temp_annual ) * 0.5f;
				m_Temperature = Mathf.Lerp( m_Temperature, _temp_avarage, 0.25f );
			}

			if( UseFog )
			{
				RenderSettings.fog = true;				

				float _fog_daytime = FogDensity.Evaluate( _hours );
				float _fog_annual = FogProbability.Evaluate( _month );
				float _fog_density = Mathf.Max(  _fog_daytime * _fog_annual, 0 );

				RenderSettings.fogDensity = Mathf.Lerp( RenderSettings.fogDensity, _fog_density, 0.25f );

				if( Camera.main.clearFlags != CameraClearFlags.SolidColor )
					Camera.main.clearFlags = CameraClearFlags.SolidColor;

				float _f = RenderSettings.fogDensity;
				float _m = MathTools.Normalize( _f, 0, 0.025f );
				
				Camera.main.backgroundColor = Color.Lerp( m_DefaultCameraBackgroundColor, FogColor, Mathf.Clamp01( _m ) );


				RenderSettings.fogColor = FogColor;

			}
		}
	}


	[System.Serializable]
	public class AstronomyDataObject : ICEOwnerObject{
		public AstronomyDataObject(){}
		public AstronomyDataObject( AstronomyDataObject _object ) : base( _object ){ Copy( _object );}
		public AstronomyDataObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			m_StartDateTimeInSecondes = DateTools.DateTimeToSeconds( StartYear, StartMonth, StartDay, 0, 0, 0 ) + m_DaytimeInSeconds;
			m_WorldTimeInSeconds = StartDateTimeInSecondes;
		}

		public void Copy( AstronomyDataObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );


		}

		public Light SunLight;
		public Color SunLightDayColor = new HSBColor( 0.15f ,0.25f, 1f ).ToColor();
		public Color SunLightNightColor = new HSBColor( 0.65f ,0.55f, 1f ).ToColor();
		public Color SunLightSunriseColor = new HSBColor( 0.10f ,1f, 1f ).ToColor();

		public float Azimut = 0;
		public float Radius = 100;
		public float RadiusMax = 1000;

		public float SunLightIntensityMin = 0.25f;
		public float SunLightIntensityMax = 1.5f;
	
		public float ZenitMin = 45;
		public float ZenitMax = 75;
		public float ZenitMaximum = 75;

		public bool UseRealTime = false;
		public int StartYear = DateTime.Today.Year;
		public int StartMonth = DateTime.Today.Month;
		public int StartDay = DateTime.Today.Day;

		public int StartHour{
			get{ return Mathf.FloorToInt( StartTimeInHours ); }
		}

		public float SunInitialIntensity{
			get{ return ( SunLight != null ? SunLight.intensity : 1 ); }
			set{ if( SunLight != null )SunLight.intensity = value; }
		}
			
		protected long m_StartDateTimeInSecondes = 0;
		public long StartDateTimeInSecondes{
			get{ return m_StartDateTimeInSecondes; }
		}
			
		public float StartTimeInHours{
			set{ m_DaytimeInSeconds = (long)Mathf.RoundToInt(value*3600f); }
			get{ return (float)m_DaytimeInSeconds/3600f; }
		}


		protected float m_TimeScaleMultiplier = 1;
		public float TimeScaleMultiplier{
			get{ return 1440f/m_DayLengthInMinutes; }
		}

		public float DayLengthInMinutesMaximum = 60;

		[SerializeField]
		protected float m_DayLengthInMinutes = 5;
		public float DayLengthInMinutes{
			get{ return m_DayLengthInMinutes; }
			set{ m_DayLengthInMinutes = value; }
		}

		protected long m_WorldTimeInSeconds = 0;
		public long WorldTimeInSeconds{
			get{ return m_WorldTimeInSeconds; }
		}

		[SerializeField]
		protected long m_DaytimeInSeconds = 0;
		public long DaytimeInSeconds{
			get{ return m_DaytimeInSeconds; }
		}

		[SerializeField]
		protected float m_SunriseNormalized = 0.25f;
		public float SunriseHour{
			set{ m_SunriseNormalized = MathTools.Normalize( value, 1, 24 ); }
			get{ return MathTools.Denormalize( m_SunriseNormalized, 1, 24 ); }
		}

		[SerializeField]
		protected float m_SunsetNormalized = 0.75f;
		public float SunsetHour{
			set{ m_SunsetNormalized = MathTools.Normalize( value, 1, 24 ); }
			get{ return MathTools.Denormalize( m_SunsetNormalized, 1, 24 ); }
		}
	}

	[System.Serializable]
	public class AstronomyObject : AstronomyDataObject{
		public AstronomyObject(){}
		public AstronomyObject( AstronomyObject _object ) : base( _object ){ Copy( _object );}
		public AstronomyObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

		}

		public void Copy( AstronomyObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );


		}

		public float SunLightIntensity{
			get{ return ( SunLight != null ? SunLight.intensity : 1 ); }
		}

		public Color SunLightColor{
			get{ 
				if( DaytimePeakNormalized < 0.25f )
					return Color.Lerp( SunLightNightColor, SunLightSunriseColor, MathTools.Normalize( DaytimePeakNormalized, 0f, 0.25f ) ); 
				else if( DaytimePeakNormalized >= 0.25f )
					return Color.Lerp( SunLightSunriseColor, SunLightDayColor, MathTools.Normalize( DaytimePeakNormalized, 0.25f, 1f ) ); 
				else
					return Color.Lerp( SunLightNightColor, SunLightDayColor, DaytimePeakNormalized ); 
			}
		}

		private float m_DaytimeInSecondsNormalized{
			get{ return MathTools.Normalize( m_DaytimeInSeconds, 0, 86400 ); }
		}

		public float DaytimePeakNormalized{
			get{ 
				float _t = ( m_DaytimeInSeconds < 86400 * 0.5f ? m_DaytimeInSeconds : 86400 - m_DaytimeInSeconds );
				return MathTools.Normalize( _t, 0, 86400 * 0.5f ); 
			}
		}

		public float Zenit{
			get{ 
				int _day = DayOfYear;

				if( _day > 182 )
					_day = 365 - _day;

				float _t = MathTools.Normalize( _day, 1, 182 );

				return Mathf.Lerp( ZenitMin, ZenitMax, _t );
			}
		}

		public Vector3 Orbit{
			get{ return ( SunLight != null ? SunLight.transform.position + ( SunLight.transform.forward * - Radius)  : Vector3.zero ); }
		}

		public Quaternion Rotation()
		{
			float _azimut_angle = (( m_DaytimeInSecondsNormalized * 360f ) - Azimut )* (-1);
			float _zenit_angle = Zenit - 180;

			Quaternion _qx = Quaternion.AngleAxis( _zenit_angle, Vector3.right );
			Quaternion _qy = Quaternion.AngleAxis( _azimut_angle, Vector3.up );
			return ( _qx * _qy );
		}

		public void Update()
		{
			if( ! Enabled || SunLight == null )
				return;

			//Rotates about the local axis
			SunLight.transform.localRotation = Rotation();
			/*
			float _multiplier = SunLight.intensity;			
			if( m_DaytimeInSecondsNormalized >= m_SunriseNormalized && m_DaytimeInSecondsNormalized < m_SunsetNormalized && _multiplier < SunInitialIntensity )
				_multiplier = Mathf.MoveTowards( _multiplier,SunInitialIntensity, 0.1f * Time.deltaTime );			
			else if( m_DaytimeInSecondsNormalized >= m_SunsetNormalized && _multiplier > 0 )
				_multiplier = Mathf.MoveTowards( _multiplier,0, 0.1f * Time.deltaTime );			
			else if( m_DaytimeInSecondsNormalized <= m_SunriseNormalized || m_DaytimeInSecondsNormalized >= m_SunsetNormalized ) 
				_multiplier = 0;		*/	

			//Debug.Log( " m_DaytimePeakNormalized : " + m_DaytimePeakNormalized + " - " +  ( m_DaytimePeakNormalized * m_DaytimePeakNormalized )  + " - " +  Mathf.Pow( m_DaytimePeakNormalized, 2f )  + " - " +  Mathf.Pow( m_DaytimePeakNormalized, 3f ) );

			SunLight.intensity = Mathf.Lerp( SunLightIntensityMin, SunLightIntensityMax, DaytimePeakNormalized );
			SunLight.color = SunLightColor;
		}

		float m_RuntimeInMilliseconds = 0;

		public void FixedUpdate()
		{
			float _msecs = ( Time.fixedDeltaTime * ( UseRealTime ? 1 : TimeScaleMultiplier ) ) * 1000;
			float _timer = (m_RuntimeInMilliseconds%1000) + _msecs;
			m_RuntimeInMilliseconds += _msecs;

			if( _timer < 1000 )
				return;

			long _seconds = (long)(_timer / 1000); 

			m_WorldTimeInSeconds += _seconds;				
			m_DaytimeInSeconds = (long)Mathf.Clamp( m_WorldTimeInSeconds%86400 > 0 ? m_WorldTimeInSeconds%86400 : m_DaytimeInSeconds + _seconds, 0, 86400 );
		}


		public int Year{
			get{ return DateTools.SecondsToDateTime( m_WorldTimeInSeconds ).Year; }
		}

		public int Month{
			get{ return DateTools.SecondsToDateTime( m_WorldTimeInSeconds ).Month; }
		}

		public int Day{
			get{ return DateTools.SecondsToDateTime( m_WorldTimeInSeconds ).Day; }
		}

		public int DayOfYear{
			get{ return DateTools.SecondsToDateTime( m_WorldTimeInSeconds ).DayOfYear; }
		}

		public int DayTotal{
			get{ return DateTools.TimeToDays( m_WorldTimeInSeconds - StartDateTimeInSecondes ); }
		}

		public float Hour{
			get{ return DateTools.TimeToHour( m_DaytimeInSeconds ); }
		}

		public float Minutes{
			get{ return DateTools.TimeToMinutes( m_DaytimeInSeconds ); }
		}

		public float Seconds{
			get{ return DateTools.TimeToSeconds( m_DaytimeInSeconds ); }
		}
	}
}