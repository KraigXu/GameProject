// ##############################################################################
//
// ICEEnvironmentMenu.cs
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

using ICE.Environment;

namespace ICE.Environment.Menus
{
	public class ICEEnvironmentMenu : MonoBehaviour {

		[MenuItem ( "ICE/ICE Environment/Components/Create Environment Controller", false, 1001 )]
		static void AddEnvironmentController() 
		{
			ICEEnvironment _environment = ICEEnvironment.Instance as ICEEnvironment;

			if( _environment == null )
			{
				GameObject _object = new GameObject();
				_environment = _object.AddComponent<ICEEnvironment>();
				_object.name = "Environment";
			}
		}
			
		[MenuItem ( "ICE/ICE Environment/Components/Create Environment Controller", true)]
		static bool ValidateAddEnvironmentController() {
			if( ICEEnvironment.Instance == null )
				return true;
			else
				return false;
		}
	}
}