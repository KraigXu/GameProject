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

using System;
using System.IO;
using System.Text;
using System.Collections;

using ICE;
using ICE.World.EditorUtilities;
using ICE.World.Windows;


namespace ICE.World.Menus
{
	public class ICEWorldMenu : MonoBehaviour {

		[MenuItem ("ICE/ICE World/Repository", false, 9000 )]
		static void Repository (){
			Application.OpenURL("https://github.com/icetec/ICEWorld");
		}

		[MenuItem ("ICE/ICE World/Wiki", false, 9000 )]
		static void Wiki (){
			Application.OpenURL("https://github.com/icetec/ICEWorld/wiki");
		}
					
		[MenuItem ("ICE/ICE World/Template Designer (BETA)", false, 9000 )]
		static void ShowTemplateDesigner(){
			TemplateDesigner.Create();
		} 

		[MenuItem ("ICE/ICE World/About", false, 9000 )]
		static void ShowAbout(){
			ICEWorldAbout.Create();
		} 
	}
}