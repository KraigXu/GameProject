// ##############################################################################
//
// ICE.World.ICEWorldInfo.cs
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

namespace ICE.World
{
	public class ICEWorldInfo {

		public static string Version = "0.0.0";
		public static string PlayerName = "Player";

		private static bool m_IsMultiplayer = false;
		public static bool IsMultiplayer
		{
			get { return m_IsMultiplayer; }
			set { m_IsMultiplayer = value; }
		}

		private static bool m_IsServer = true;
		public static bool IsServer{
			get{ return ( m_IsMultiplayer == false ? true : m_IsServer ); }
			set{
				if( m_IsMultiplayer == false )
					return;

				m_IsServer = value;
			}
		}

		public static bool IsClient{
			get{ return ! IsServer; }
		}
			
		public static bool IsMultiplayerServer{
			get { return ( m_IsMultiplayer && m_IsServer ? true : false ); }
		}

		public static bool IsMultiplayerServerConnectedAndReady{
			get { return ( m_IsMultiplayer && m_IsServer && m_NetworkConnectedAndReady ? true : false ); }
		}

		private static bool m_IsMultiplayerMine = false;
		public static bool IsMultiplayerMine
		{
			get { return m_IsMultiplayerMine; }
			set { m_IsMultiplayerMine = value; }
		}

		private static bool m_NetworkConnectedAndReady = true;
		public static bool NetworkConnectedAndReady{
			get{ return ( m_IsMultiplayer == false ? true: m_NetworkConnectedAndReady ); }
			set{ m_NetworkConnectedAndReady = value; }
		}

		private static bool m_NetworkSpawnerReady = false;
		public static bool NetworkSpawnerReady{
			get{ return ( m_IsMultiplayer == false ? true: m_NetworkSpawnerReady ); }
			set{ m_NetworkSpawnerReady = value; }
		}

		/// <summary>
		/// Quit the application
		/// </summary>
		/// <param name="_url">URL.</param>
		public static void Quit( string _url = "http://www.icecreaturecontrol.com" )
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
			Application.Quit();
#elif UNITY_WEBPLAYER
			Application.OpenURL( _url );
#endif
		}
	}
}