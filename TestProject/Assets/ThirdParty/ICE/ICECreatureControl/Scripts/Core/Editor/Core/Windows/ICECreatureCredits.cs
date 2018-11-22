// ##############################################################################
//
// ice_CreatureAbout.cs
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

using ICE.Creatures.EditorInfos;

namespace ICE.Creatures.Windows
{

	public class ICECreatureCredits : EditorWindow {

		private static Texture2D m_ICECCLogo = null;
		private static Texture2D m_ICELogo = null;

		private static Vector2 m_DialogSize = new Vector2(520, 600);
		private static string m_Version = "Version " + Info.Version;
		private static string m_Copyright = "© " + System.DateTime.Now.Year + " Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.";


		/// <summary>
		/// 
		/// </summary>
		public static void Create()
		{
			if( EditorGUIUtility.isProSkin )
				m_ICECCLogo = (Texture2D)Resources.Load("ICECC_LOGO_W", typeof(Texture2D));
			else
				m_ICECCLogo = (Texture2D)Resources.Load("ICECC_LOGO", typeof(Texture2D));

			if( EditorGUIUtility.isProSkin )
				m_ICELogo = (Texture2D)Resources.Load("ICE_LOGO_W", typeof(Texture2D));
			else
				m_ICELogo = (Texture2D)Resources.Load("ICE_LOGO", typeof(Texture2D));

			ICECreatureCredits _window = (ICECreatureCredits)EditorWindow.GetWindow(typeof(ICECreatureCredits), true);

			_window.titleContent = new GUIContent( "ICE Creature Control Credits", "");
		
			_window.minSize = new Vector2(m_DialogSize.x, m_DialogSize.y);
			_window.maxSize = new Vector2(m_DialogSize.x + 1, m_DialogSize.y + 1);
			_window.position = new Rect(
				(Screen.currentResolution.width / 2) - (m_DialogSize.x / 2),
				(Screen.currentResolution.height / 2) - (m_DialogSize.y / 2),
				m_DialogSize.x,
				m_DialogSize.y);
			_window.Show();
			
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		void OnGUI()
		{
			if( m_ICECCLogo == null ) m_ICECCLogo = (Texture2D)Resources.Load("ICECC_LOGO", typeof(Texture2D));
			if( m_ICELogo == null ) m_ICELogo = (Texture2D)Resources.Load("ICE_LOGO", typeof(Texture2D));

			if( m_ICECCLogo != null )
				GUI.DrawTexture(new Rect(10, 10, m_ICECCLogo.width, m_ICECCLogo.height), m_ICECCLogo);

			GUILayout.BeginArea( new Rect(20, 150 , Screen.width - 40, Screen.height - 40));

				GUILayout.Label( "3RD PARTY ASSETS", EditorStyles.boldLabel );

				GUILayout.Label( "To provide nice looking and helpful demo scenes your ICE package contains additional " +
					"content of external parties, according to the given licence regulations and/or the explicit permission " +
					"of the author. Here I would like to thanks all these creative people for their contribution and want " +
					"point you to their original sources.\n", EditorStyles.wordWrappedLabel );

				if( ICEEditorLayout.Button( "3D Models/Characters/Animals - Cute Kitten by leshiy3d", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/33121"); }
				//if( ICEEditorLayout.Button( "3D Models/Characters/Humanoids - Overlord and more by 3DMaesen", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/48768"); }

				if( ICEEditorLayout.Button( "3D Models/Vegetation/Trees - Mobile Tree Package by Laxer", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/18866"); }
				if( ICEEditorLayout.Button( "3D Models/Environments - Campfire by David Stenfors", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/45038"); }
				if( ICEEditorLayout.Button( "Unity Essentials/Sample Projects - Teddy and more by UNITY", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/5328"); }
				if( ICEEditorLayout.Button( "Opengameart.org - MuzzleFlash by Julius Krischan", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("http://opengameart.org/content/muzzle-flash-with-model"); }
				if( ICEEditorLayout.Button( "Textures.com - Diverse ground and nature textures", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("www.textures.com"); }
				if( ICEEditorLayout.Button( "Freesound.org - Footsteps by OwlStorm", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("https://www.freesound.org/people/OwlStorm/packs/9344/"); }


				GUILayout.Label( "\nAlso I would like to thanks all the countless authors who supports all of us with their " +
					"experience, helpful code snippets and practical approaches, without such unselfish inputs the internet " +
					"would be significantly poorer.", EditorStyles.wordWrappedLabel );

				GUILayout.Label( "\nLast but not least, I would like to mention also the ICE community which supports and inspires " +
					"me with great suggestions and ideas. Thanks so much!\n", EditorStyles.wordWrappedLabel );

				if( ICEEditorLayout.Button( "ICE Creature Control - Forum", "", ICEEditorStyle.LinkStyle)) { Application.OpenURL("http://forum.unity3d.com/threads/347147/"); }


			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(20, m_DialogSize.y - 85 , Screen.width - 40, Screen.height - 40));
				GUI.backgroundColor = Color.clear;

				if( ICEEditorLayout.Button( "Contact Pit Vetterick (Skype:pit.vetterick)", "", ICEEditorStyle.GetLinkStyle(Color.grey))) { Application.OpenURL("skype:pit.vetterick?add"); }
				if( ICEEditorLayout.Button( "https://twitter.com/CreatureAI", "", ICEEditorStyle.GetLinkStyle(Color.grey))) { Application.OpenURL("https://twitter.com/CreatureAI"); }
				if( ICEEditorLayout.Button( "http://www.icecreaturecontrol.com", "", ICEEditorStyle.GetLinkStyle(Color.grey))) { Application.OpenURL("http://www.icecreaturecontrol.com"); }

				GUI.color = ICEEditorLayout.DefaultGUIColor;
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				EditorGUILayout.Separator();
				GUILayout.Label( m_Copyright + " - " +  m_Version , EditorStyles.centeredGreyMiniLabel );
			GUILayout.EndArea();

			if( m_ICELogo != null )
				GUI.DrawTexture(new Rect(270, m_DialogSize.y - 90 , m_ICELogo.width, m_ICELogo.height), m_ICELogo);
			
			
		}
	}
}
