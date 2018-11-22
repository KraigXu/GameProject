// ##############################################################################
//
// ICE.World.ICEWorldEnvironment.cs
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

using ICE.World.EnumTypes;

namespace ICE.World
{

	/// <summary>
	/// ICE world environment.
	/// </summary>
	/// <description>
	/// ICEWorldEnvironment contains several environment parameter, such as Date and Time, Temperature and Weather Conditions etc. 
	/// You can use ICEWorldEnvironment as base class for your own Day&Night Cycle and/or Weather System, so it will be automatically 
	/// compatible with the rest of the ICE world. 
	/// <description>
	public class ICEWorldEnvironment : ICEWorldSingleton {

		//protected ICEWorldEnvironment() {} // guarantee this will be always a singleton only - can't use the constructor!
	
		protected static new ICEWorldEnvironment m_Instance = null;
		public static new ICEWorldEnvironment Instance{
			get{ return m_Instance = ( m_Instance == null?GameObject.FindObjectOfType<ICEWorldEnvironment>():m_Instance ) as ICEWorldEnvironment; }
		}
			
		public float LightIntensity;

		public TemperatureScaleType TemperatureScale;
		public float Temperature;
		public float MinTemperature;
		public float MaxTemperature;

		public int DateDay;
		public int DateMonth;
		public int DateYear;

		public int TimeHour;
		public int TimeMinutes;
		public int TimeSeconds;

		public WeatherType WeatherForecast;

		public float WindSpeed;
		public float WindDirection;

		public void UpdateTemperatureScale( TemperatureScaleType _scale )
		{
			if( _scale == TemperatureScaleType.CELSIUS && TemperatureScale == TemperatureScaleType.FAHRENHEIT )
			{
				TemperatureScale = _scale;
				Temperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( Temperature );
				MinTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( MinTemperature );
				MaxTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( MaxTemperature );

			}
			else if( _scale == TemperatureScaleType.FAHRENHEIT && TemperatureScale == TemperatureScaleType.CELSIUS )
			{
				TemperatureScale = _scale;
				Temperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( Temperature );
				MinTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( MinTemperature );
				MaxTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( MaxTemperature );
			}
		}
	}
}
