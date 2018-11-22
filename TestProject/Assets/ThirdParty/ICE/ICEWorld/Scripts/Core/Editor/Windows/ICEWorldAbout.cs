// ##############################################################################
//
// ICEWorldAbout.cs
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
using UnityEditor;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.Windows
{

	public class ICEWorldAbout : EditorWindow {

		private static Texture2D m_ICEWorldLogo = null;
		private static Texture2D m_ICELogo = null;

		private static Vector2 m_DialogSize = new Vector2(520, 260);
		private static string m_Version = "Version " + Info.Version;
		private static string m_Copyright = "© " + System.DateTime.Now.Year + " Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.";

		/// <summary>
		/// 
		/// </summary>
		public static void Create()
		{
			if( EditorGUIUtility.isProSkin )
				m_ICEWorldLogo = (Texture2D)Resources.Load("ICEWORLDLOGO_W", typeof(Texture2D));
			else
				m_ICEWorldLogo = (Texture2D)Resources.Load("ICEWORLDLOGO", typeof(Texture2D));

			if( EditorGUIUtility.isProSkin )
				m_ICELogo = (Texture2D)Resources.Load("ICE_LOGO_W", typeof(Texture2D));
			else
				m_ICELogo = (Texture2D)Resources.Load("ICE_LOGO", typeof(Texture2D));

			ICEWorldAbout msgBox = (ICEWorldAbout)EditorWindow.GetWindow(typeof(ICEWorldAbout), true);

			msgBox.titleContent = new GUIContent( "About ICE World", "");
		
			msgBox.minSize = new Vector2(m_DialogSize.x, m_DialogSize.y);
			msgBox.maxSize = new Vector2(m_DialogSize.x + 1, m_DialogSize.y + 1);
			msgBox.position = new Rect(
				(Screen.currentResolution.width / 2) - (m_DialogSize.x / 2),
				(Screen.currentResolution.height / 2) - (m_DialogSize.y / 2),
				m_DialogSize.x,
				m_DialogSize.y);
			msgBox.Show();
			
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		void OnGUI()
		{
			if( m_ICEWorldLogo != null )
				GUI.DrawTexture(new Rect(10, 10, m_ICEWorldLogo.width, m_ICEWorldLogo.height), m_ICEWorldLogo);
			
			GUILayout.BeginArea(new Rect(20, 140, Screen.width - 40, Screen.height - 40));
				GUI.backgroundColor = Color.clear;
				GUILayout.Label(m_Version);
				GUILayout.Label(m_Copyright  + "\n\n", ICEEditorStyle.SmallTextStyle );

				if( ICEEditorLayout.Button( "Contact Pit Vetterick (Skype:pit.vetterick)", "", ICEEditorStyle.LinkStyle ) ) { Application.OpenURL( "skype:pit.vetterick?add" ); }
				if( ICEEditorLayout.Button( "ICEWorld Repository", "", ICEEditorStyle.LinkStyle ) ) { Application.OpenURL( "https://github.com/icetec/ICEWorld" ); }
				if( ICEEditorLayout.Button( "ICEWorld Wiki", "", ICEEditorStyle.LinkStyle ) ) { Application.OpenURL( "https://github.com/icetec/ICEWorld/wiki" ); }
				GUI.color = ICEEditorLayout.DefaultGUIColor;
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			GUILayout.EndArea();
			

			if( m_ICELogo != null )
				GUI.DrawTexture(new Rect(270, 190, m_ICELogo.width, m_ICELogo.height), m_ICELogo);
			
			
		}
	}
}
