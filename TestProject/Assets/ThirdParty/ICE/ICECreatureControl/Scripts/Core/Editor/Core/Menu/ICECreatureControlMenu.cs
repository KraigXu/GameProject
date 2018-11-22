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
	public class ICECreatureControlMenu : MonoBehaviour {


		// WIZARD
		[MenuItem ("ICE/ICE Creature Control/Wizard", false, 1955 )]
		static void Wizard ()
		{
			ICECreatureWizard.Create();
		}

		// CREDITS
		[MenuItem ("ICE/ICE Creature Control/Credits", false, 1971 )]
		static void Credits ()
		{
			ICECreatureCredits.Create();
		}

		// ABOUT
		[MenuItem ("ICE/ICE Creature Control/About", false, 1999 )]
		static void AboutICE ()
		{
			ICECreatureAbout.Create();
		}
	}
}