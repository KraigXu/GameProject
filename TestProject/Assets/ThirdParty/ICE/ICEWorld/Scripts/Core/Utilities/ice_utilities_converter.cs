// ##############################################################################
//
// ice_utilities_converter.cs | ICE.World.Utilities.Converter
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

namespace ICE.World.Utilities
{
	/// <summary>
	/// Converter contains several converter tools 
	/// </summary>
	public static class Converter 
	{
		/// <summary>
		/// Fahrenheits to celsius.
		/// </summary>
		/// <returns>The to celsius.</returns>
		/// <param name="_fahrenheit">Fahrenheit.</param>
		public static float FahrenheitToCelsius( float _fahrenheit ){
			return (5f / 9f) * (_fahrenheit - 32f);
		}

		/// <summary>
		/// Celsiuses to fahrenheit.
		/// </summary>
		/// <returns>The to fahrenheit.</returns>
		/// <param name="_celsius">Celsius.</param>
		public static float CelsiusToFahrenheit( float _celsius ){
			return _celsius * (9f / 5f) + 32f;
		}

		public static Vector4 QuaternionToVector4( Quaternion _rot ) {
			return new Vector4( _rot.x, _rot.y, _rot.z, _rot.w);
		}

		public static Quaternion Vector4ToQuaternion( Vector4 _vector ) {
			return new Quaternion( _vector.x, _vector.y, _vector.z, _vector.w );
		}
	}
}