// ##############################################################################
//
// ice_utilities_date.cs | ICE.World.Utilities.DateTools
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
using System;
using System.Globalization;

namespace ICE.World.Utilities
{
	public static class DateTools
	{
		
		public static int TimeToDayOfYear( long _seconds ){
			return SecondsToDateTime( _seconds ).DayOfYear;
		}
			
		public static int TimeToMonth( long _seconds ){
			return SecondsToDateTime( _seconds ).Month;
		}

		public static int TimeToDay( long _seconds ){
			return SecondsToDateTime( _seconds ).Day;
		}

		/// <summary>
		/// Converts the specified datetime to tick based seconds
		/// </summary>
		/// <returns>The time to seconds.</returns>
		/// <param name="_year">Year.</param>
		/// <param name="_month">Month.</param>
		/// <param name="_days">Days.</param>
		/// <param name="_hour">Hour.</param>
		/// <param name="_minutes">Minutes.</param>
		/// <param name="_seconds">Seconds.</param>
		public static long DateTimeToSeconds( int _year, int _month, int _days, int _hour, int _minutes, int _seconds ){
			return new System.DateTime( _year, _month, _days, _hour, _minutes, _seconds, 0, DateTimeKind.Utc ).Ticks / System.TimeSpan.TicksPerSecond; ;
		}

		/// <summary>
		/// Converts tick based seconds to datetime
		/// </summary>
		/// <returns>The to date time.</returns>
		/// <param name="_seconds">Seconds.</param>
		public static DateTime SecondsToDateTime( long _seconds ){
			return new System.DateTime( _seconds * System.TimeSpan.TicksPerSecond );
		}

		/// <summary>
		/// Converts tick based seconds to string by using the specified format
		/// </summary>
		/// <returns>The to string.</returns>
		/// <param name="_seconds">Seconds.</param>
		/// <param name="_format">Format.</param>
		public static string SecondsToString( long _seconds, string _format ){
			return SecondsToDateTime( _seconds ).ToString( _format );
		}

		public static int TimeToDays( float _seconds ){
			return Mathf.FloorToInt( _seconds/86400 );
		}

		public static int TimeToHour( float _seconds ){
			return Mathf.FloorToInt( _seconds/3600 );
		}

		public static int TimeToMinutes( float _seconds ){
			return Mathf.FloorToInt( _seconds/60%60 );
		}

		public static int TimeToSeconds( float _seconds ){
			return Mathf.FloorToInt( _seconds%60 );
		}

		/// <summary>
		/// Formats the time.
		/// </summary>
		/// <returns>The time.</returns>
		/// <param name="_seconds">Seconds.</param>
		public static string FormatTime( float _seconds )
		{
			System.TimeSpan _time_span = System.TimeSpan.FromSeconds( _seconds );
			return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", _time_span.Hours, _time_span.Minutes, _time_span.Seconds );
		}

		/// <summary>
		/// Formats the time detailed.
		/// </summary>
		/// <returns>The time detailed.</returns>
		/// <param name="_seconds">Seconds.</param>
		public static string FormatTimeDetailed( float _seconds )
		{
			System.TimeSpan _time_span = System.TimeSpan.FromSeconds( _seconds );
			return string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", _time_span.Hours, _time_span.Minutes, _time_span.Seconds, _time_span.Milliseconds );
		}

		/// <summary>
		/// Localizes the date time.
		/// </summary>
		/// <returns>The date time.</returns>
		/// <param name="_key">Key.</param>
		/// <param name="_datetime">Datetime.</param>
		public static string LocalizeDateTime( string _key, DateTime _datetime )
		{
			//       en-US: 6/19/2015 10:03:06 AM
			//       en-GB: 19/06/2015 10:03:06
			//       fr-FR: 19/06/2015 10:03:06
			//       de-DE: 19.06.2015 10:03:06

			return _datetime.ToString( new CultureInfo( _key ) );
		}
	}
}
