// ##############################################################################
//
// ICECreatureControlMenu.cs
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

using UnityEditor;
using UnityEngine;

using ICE;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.Windows;

namespace ICE.Creatures.Menus
{
	public class ICECreatureSupportMenu : MonoBehaviour {


		[MenuItem ("ICE/ICE Creature Control/Support/Manual (online)", false, 1971 )]
		static void ManualOnline ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/files/manual/ICECC_MANUAL.pdf");
		}

		[MenuItem ("ICE/ICE Creature Control/Support/Homepage", false, 1971 )]
		static void Homepage ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com");
		}

		[MenuItem ("ICE/ICE Creature Control/FAQ", false, 1971 )]
		static void FAQ ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/FAQ/" );
		}

		[MenuItem ("ICE/ICE Creature Control/Support/Wiki", false, 1971 )]
		static void Wiki ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/wiki/" );
		}

		[MenuItem ("ICE/ICE Creature Control/Support/Tutorials", false, 1971 )]
		static void Tutorials ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/TUTORIALS/" );
		}

		[MenuItem ("ICE/ICE Creature Control/Support/Support Forum", false, 1971 )]
		static void SupportForum ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/forum-news/");
		}

		[MenuItem ("ICE/ICE Creature Control/Support/Unity Forum", false, 1971 )]
		static void UnityForum ()
		{
			Application.OpenURL("http://forum.unity3d.com/threads/347147/");
		}

		[MenuItem ("ICE/ICE Creature Control/Support/Bug Report", false, 1971 )]
		static void BugReport ()
		{
			Application.OpenURL("http://www.ice-technologies.de/mantis/");
		}
	}
}
