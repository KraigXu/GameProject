// ##############################################################################
//
// ICEEnvironment.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.ice-technologies.com
// mailto:support@ice-technologies.com
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

using ICE.Environment;
using ICE.Environment.Objects;

namespace ICE.Environment
{
	public class ICEEnvironment : ICEWorldEnvironment {
		
		private static new ICEEnvironment m_Instance;
		public static new ICEEnvironment Instance{
			get{ return m_Instance = ( m_Instance == null?GameObject.FindObjectOfType<ICEEnvironment>():m_Instance ) as ICEEnvironment; }
		}

		[SerializeField]
		private DisplayObject m_Display = null;
		public virtual DisplayObject Display{
			get{ return m_Display = ( m_Display == null ? new DisplayObject(this):m_Display ); }
			set{ Display.Copy( value ); }
		}


		[SerializeField]
		private AstronomyObject m_Astronomy = null;
		public virtual AstronomyObject Astronomy{
			get{ return m_Astronomy = ( m_Astronomy == null ? new AstronomyObject(this):m_Astronomy ); }
			set{ Astronomy.Copy( value ); }
		}

		[SerializeField]
		private WeatherObject m_Weather = null;
		public virtual WeatherObject Weather{
			get{ return m_Weather = ( m_Weather == null ? new WeatherObject(this):m_Weather ); }
			set{ Weather.Copy( value ); }
		}


		public string CurrentDateTimeString{
			get{ return DateTools.SecondsToString( Astronomy.WorldTimeInSeconds, Display.UITextDateTimeFormat ); }
		}



		public override void Start () {

			Display.Init( this );
			Astronomy.Init( this );
			Weather.Init( this );

		}

		private float m_UpdateInterval = 0.5f;
		private float m_UpdateTimer = 0.5f;

		public override void Update () {

			Astronomy.Update();
			Weather.Update();

			m_PathPositions.Add( Astronomy.Orbit );

			m_UpdateInterval = 0;
			if( m_UpdateInterval > 0 )
			{
				m_UpdateTimer += Time.deltaTime;
				if( m_UpdateTimer < m_UpdateInterval )
					return;

				m_UpdateTimer = 0;
			}

			//UPDATE GLOABL ENVIRONMENT INFOS

			// TEMPERATURE
			TemperatureScale = Weather.TemperatureScale;
			Temperature = Weather.Temperature;
			MinTemperature = Weather.MinTemperature;
			MaxTemperature = Weather.MaxTemperature;

			// DATE & TIME
			TimeHour = (int)Astronomy.Hour;
			TimeMinutes = (int)Astronomy.Minutes;
			TimeSeconds = (int)Astronomy.Seconds;

			DateDay = (int)Astronomy.Day;
			DateMonth = (int)Astronomy.Month;
			DateYear = (int)Astronomy.Year;

			// ASTRO
			if( Display.UITextDateTime != null )
				Display.UITextDateTime.text = CurrentDateTimeString;
			
			if( Display.UITextDay != null )
				Display.UITextDay.text = string.Format( Display.UITextDayFormat, Astronomy.DayTotal + ( Display.UseFirstDay ? 1 : 0 ) ); 

			// WEATHER
			if( Display.UITemperatur != null )
				Display.UITemperatur.text = string.Format( Display.UITemperaturFormat, Mathf.RoundToInt( Weather.Temperature ) ); 

			// LIGHT
			LightIntensity = Astronomy.SunLightIntensity;
		}

		public override void FixedUpdate(){
			Astronomy.FixedUpdate();
		}



		List<Vector3> m_PathPositions = new List<Vector3>();
		#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if( Astronomy.SunLight == null )
				return;
			
			if( ! Application.isPlaying )
				Astronomy.SunLight.transform.localRotation = Astronomy.Rotation();

			Gizmos.color = Color.blue;
			Gizmos.DrawLine( Astronomy.SunLight.transform.position + ( Astronomy.SunLight.transform.forward * Astronomy.Radius * 0.75f ), Astronomy.SunLight.transform.position + ( Astronomy.SunLight.transform.forward * Astronomy.Radius ) );
			//Gizmos.DrawLine( Sun.transform.position - ( Sun.transform.forward * Radius * 0.75f ), Sun.transform.position - ( Sun.transform.forward * Radius ) );
			CustomGizmos.Circle( Astronomy.SunLight.transform.position, Astronomy.SunLight.transform.up, Astronomy.Radius );
			CustomGizmos.Arrow( Astronomy.SunLight.transform.position + ( Astronomy.SunLight.transform.forward * Astronomy.Radius * 0.75f ), Astronomy.SunLight.transform.forward * -2, 10 );
			CustomGizmos.HandlesColor( Gizmos.color );
			CustomGizmos.Arrow( 0, Astronomy.SunLight.transform.position - ( Astronomy.SunLight.transform.forward * Astronomy.Radius ), Astronomy.SunLight.transform.rotation, 50 );


			if( m_PathPositions.Count > 1000 )
				m_PathPositions.RemoveAt(0);
	
			Vector3 _prior_pos = Vector3.zero;
			foreach( Vector3 _pos in m_PathPositions)
			{
				if( _prior_pos != Vector3.zero  )
					Gizmos.DrawLine( _prior_pos, _pos );
				
				_prior_pos = _pos;
			}
			
		}
		#endif
	}
}
