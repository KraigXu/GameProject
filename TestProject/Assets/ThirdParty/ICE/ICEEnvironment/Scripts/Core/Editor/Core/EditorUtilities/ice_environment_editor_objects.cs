// ##############################################################################
//
// ice_environment_editor_objects.cs
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

using UnityEditor;
//using UnityEditor.AnimatedValues;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;
using ICE.World.EnumTypes;

using ICE.Environment;
//using ICE.Environment.EnumTypes;
using ICE.Environment.Objects;
using ICE.Environment.EditorUtilities;
using ICE.Environment.EditorInfos;

namespace ICE.Environment.EditorUtilities
{
	public class EnvironmentEditorLayout : ICEEditorLayout
	{	
	}

	public class EnvironmentObjectEditor : WorldObjectEditor
	{	
		public static void DrawDisplayObjectSettings( ICEEnvironment _environment, DisplayObject _display, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _environment == null || _display == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Display";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.DISPLAY;

			DrawObjectHeader( _display, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _display ) )
				return;

			_display.UITextDateTime = (Text)EditorGUILayout.ObjectField( "DateTime", _display.UITextDateTime, typeof(Text), true );
			EditorGUILayout.BeginHorizontal();
				_display.UITextDateTimeFormat = ICEEditorLayout.Text( "Format", "", _display.UITextDateTimeFormat, "" );
				_display.UITextDateTimeFormat = ICEEditorLayout.ButtonDefault( _display.UITextDateTimeFormat, "dd.MM.yyyy HH:mm:ss" );
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
				_display.UITextDay = (Text)EditorGUILayout.ObjectField( "Days", _display.UITextDay, typeof(Text), true );
				_display.UseFirstDay = ICEEditorLayout.CheckButtonSmall( "+1", "", _display.UseFirstDay );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
				_display.UITextDayFormat = ICEEditorLayout.Text( "Format", "", _display.UITextDayFormat, "" );
				_display.UITextDayFormat = ICEEditorLayout.ButtonDefault( _display.UITextDayFormat, "Day #{0}" );
			EditorGUILayout.EndHorizontal();

			_display.UITemperatur = (Text)EditorGUILayout.ObjectField( "Temperatur", _display.UITemperatur, typeof(Text), true );
			EditorGUILayout.BeginHorizontal();
			_display.UITemperaturFormat = ICEEditorLayout.Text( "Format", "", _display.UITemperaturFormat, "" );
			_display.UITemperaturFormat = ICEEditorLayout.ButtonDefault( _display.UITemperaturFormat, "{0}°C" );
			EditorGUILayout.EndHorizontal();

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawWeatherObjectSettings( ICEEnvironment _environment, WeatherObject _weather, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _environment == null || _weather == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Meteorological Settings";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.WEATHER;

			DrawObjectHeader( _weather, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _weather ) )
				return;

				ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.Label( "Temperature" );
					_weather.UseTemperature = ICEEditorLayout.EnableButton( _weather.UseTemperature );
				ICEEditorLayout.EndHorizontal();
				if( _weather.UseTemperature )
				{
					EditorGUI.indentLevel++;

						_weather.TemperatureScale = (TemperatureScaleType)ICEEditorLayout.EnumPopup( "Temperature Scale", "", _weather.TemperatureScale , "" );
						ICEEditorLayout.MinMaxDefaultSlider( "Temperature (min/max)", "", ref _weather.MinTemperature, ref _weather.MaxTemperature, - _weather.TemperatureMaximum, ref _weather.TemperatureMaximum, -25, 50, Init.DECIMAL_PRECISION_DISTANCES , 40, "" );
					
					Keyframe[] _keys_1 = new Keyframe[5]{
						new Keyframe( 0, 10.0f ),
						new Keyframe(5, 15.05f ),
						new Keyframe(12, 35.0f ),
						new Keyframe( 20, 25.01f ),
						new Keyframe( 24, 10.0f) };

					_weather.DaytimeTemperature = ICEEditorLayout.DefaultCurve( "Daytime Temperature", "", _weather.DaytimeTemperature, new AnimationCurve(_keys_1) );

					Keyframe[] _keys_2 = new Keyframe[7]{
						new Keyframe( 1, -15 ),
						new Keyframe( 3, -10 ),
						new Keyframe( 5, 15 ),
						new Keyframe( 7, 30 ),
						new Keyframe( 9, 35 ),
						new Keyframe( 11, 20 ),
						new Keyframe( 13, -15 ) };

					_weather.AnnualAverageTemperature = ICEEditorLayout.DefaultCurve( "Annual Temperatures (average)", "Average annual temperatures", _weather.AnnualAverageTemperature, new AnimationCurve(_keys_2) );


					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();
				}
				// FOG BEGIN
				ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.Label( "Fog" );
					_weather.UseFog = ICEEditorLayout.EnableButton( _weather.UseFog );
				ICEEditorLayout.EndHorizontal();
				if( _weather.UseFog )
				{
					EditorGUI.indentLevel++;

					Keyframe[] _keys_1 = new Keyframe[5]{
						new Keyframe( 0, 0.0f ),
						new Keyframe(5, 0.05f ),
						new Keyframe(12, 0.0f ),
						new Keyframe( 20, 0.01f ),
						new Keyframe( 24, 0.0f) };

						_weather.FogDensity = ICEEditorLayout.DefaultCurve( "Density", "", _weather.FogDensity, new AnimationCurve(_keys_1) );


					Keyframe[] _keys_2 = new Keyframe[7]{
						new Keyframe( 1, 0 ),
						new Keyframe( 3, 1 ),
						new Keyframe( 5, 0.5f ),
						new Keyframe( 7, 0 ),
						new Keyframe( 9, 0.25f ),
						new Keyframe( 11, 1 ),
						new Keyframe( 13, 0 ) };

					_weather.FogProbability = ICEEditorLayout.DefaultCurve( "Annual Probability", "", _weather.FogProbability, new AnimationCurve(_keys_2) );

				
						_weather.FogInitialDensity = ICEEditorLayout.DefaultSlider( "Fog Initial Density", "", _weather.FogInitialDensity, Init.DECIMAL_PRECISION, 0, 8, 1, "");
						ICEEditorLayout.MinMaxDefaultSlider( "Density (min/max)", "", ref _weather.FogDensityMin, ref _weather.FogDensityMax, 0, 1, 0.01f, 0.025f, Init.DECIMAL_PRECISION_DISTANCES , 40, "" );
						EditorGUI.indentLevel++;
							_weather.FogDayColor = ICEEditorLayout.DefaultColor( "Day", "", _weather.FogDayColor, new HSBColor(  0.0f ,0.0f, 1f ).ToColor(), "" );
							_weather.FogSunriseColor = ICEEditorLayout.DefaultColor( "Sunrise", "", _weather.FogSunriseColor, new HSBColor( 0.14f ,0.22f, 0.64f ).ToColor(), "" );
							_weather.FogNightColor = ICEEditorLayout.DefaultColor( "Night", "", _weather.FogNightColor, new HSBColor( 0.91f ,0.04f, 0.30f ).ToColor(), "" );
						EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();
				}
				// FOG END

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawAstronomyObjectSettings( ICEEnvironment _environment, AstronomyObject _astronomy, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _environment == null || _astronomy == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Astronomical Settings";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.ASTRONOMY;

			DrawObjectHeader( _astronomy, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _astronomy ) )
				return;

				ICEEditorLayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel( new GUIContent( "Start Date (d.m.y)" ) );

					int _indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;
					_astronomy.StartDay = ICEEditorLayout.DayPopup( _astronomy.StartDay, _astronomy.StartYear, _astronomy.StartMonth ,GUILayout.Width( 50 ) );
					_astronomy.StartMonth = EditorGUILayout.IntPopup( _astronomy.StartMonth, new string[]{"January","February","March","April","May","June","July","August","September","October","November","December"}, new int[]{1,2,3,4,5,6,7,8,9,10,11,12} );
					_astronomy.StartYear = EditorGUILayout.IntField( _astronomy.StartYear, GUILayout.Width( 50 ) );
					EditorGUI.indentLevel = _indent;		

					Vector3 _date = ICEEditorLayout.ButtonDefault( new Vector3(_astronomy.StartYear, _astronomy.StartMonth, _astronomy.StartDay ), new Vector3( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) );

					_astronomy.StartYear = (int)_date.x;
					_astronomy.StartMonth = (int)_date.y;
					_astronomy.StartDay = (int)_date.z;

				ICEEditorLayout.EndHorizontal();
				_astronomy.StartTimeInHours = ICEEditorLayout.DefaultSlider( "Start Time (hour)", "", _astronomy.StartTimeInHours, 0.025f, 0, 24, 6, ""); 
				EditorGUILayout.Separator();

				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _astronomy.UseRealTime == true );
					_astronomy.DayLengthInMinutes = ICEEditorLayout.MaxDefaultSlider( "Length Of Day (minutes)" , "Length of a day (24h) in minutes", _astronomy.DayLengthInMinutes, 0.01f, 0.1f, ref _astronomy.DayLengthInMinutesMaximum, 60, ""); 
					EditorGUI.EndDisabledGroup();
					_astronomy.UseRealTime = ICEEditorLayout.CheckButtonSmall( "REAL", "Use Real Time", _astronomy.UseRealTime );  
				ICEEditorLayout.EndHorizontal();			
				EditorGUILayout.Separator();

				ICEEditorLayout.BeginHorizontal();
					_astronomy.SunLight = (Light)EditorGUILayout.ObjectField( "Sun", _astronomy.SunLight, typeof(Light), true );
					EditorGUI.BeginDisabledGroup( _astronomy.SunLight != null );
						if( ICEEditorLayout.AutoButton( "" ) )
						{
							GameObject _sun = new GameObject( "Sun" );
							if( _sun != null )
							{
								_astronomy.SunLight = _sun.AddComponent<Light>();
								if( _astronomy.SunLight != null )
								{
									_astronomy.SunLight.type = LightType.Directional;
									_astronomy.SunLight.intensity = _astronomy.SunInitialIntensity;
									_astronomy.SunLight.color = _astronomy.SunLightColor;
								}
							}
						}
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal();

				EditorGUI.indentLevel++;
					_astronomy.Azimut = (int)ICEEditorLayout.DefaultSlider( "Azimut", "", _astronomy.Azimut, 1, 0, 360, 0, ""); 
					_astronomy.Radius = (int)ICEEditorLayout.MaxDefaultSlider( "Distance", "", _astronomy.Radius, 1, 0, ref _astronomy.RadiusMax, 60, ""); 
					ICEEditorLayout.MinMaxDefaultSlider( "Zenit Angle (min/max)", "", ref _astronomy.ZenitMin, ref _astronomy.ZenitMax, 0, 90, 45, 75, Init.DECIMAL_PRECISION_DISTANCES , 30, "" );

					_astronomy.SunInitialIntensity = ICEEditorLayout.DefaultSlider( "Intensity", "", _astronomy.SunInitialIntensity, Init.DECIMAL_PRECISION, 0, 8, 1, "");
					ICEEditorLayout.MinMaxDefaultSlider( "Light Intensity (min/max)", "", ref _astronomy.SunLightIntensityMin, ref _astronomy.SunLightIntensityMax, 0, 8, 0.25f, 1.5f, Init.DECIMAL_PRECISION_DISTANCES , 40, "" );
					EditorGUI.indentLevel++;
						_astronomy.SunLightDayColor = ICEEditorLayout.DefaultColor( "Day", "", _astronomy.SunLightDayColor, new HSBColor( 0.15f ,0.25f, 1f ).ToColor(), "" );
						_astronomy.SunLightSunriseColor = ICEEditorLayout.DefaultColor( "Sunrise", "", _astronomy.SunLightSunriseColor, new HSBColor( 0.10f ,1f, 1f ).ToColor(), "" );
						_astronomy.SunLightNightColor = ICEEditorLayout.DefaultColor( "Night", "", _astronomy.SunLightNightColor, new HSBColor( 0.65f ,0.55f, 1f ).ToColor(), "" );
					EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;

			EditorGUILayout.Separator();




			//m_environment_master.SunriseHour = ICEEditorLayout.DefaultSlider( "Sunrise (hour)", "", m_environment_master.SunriseHour, 0.25f, 1, 12, 6, ""); 
			//m_environment_master.SunsetHour = ICEEditorLayout.DefaultSlider( "Sunset (hour)", "", m_environment_master.SunsetHour, 0.25f, 12, 24, 18, ""); 

	


			EndObjectContent();
			// CONTENT END

		}

	}
}
