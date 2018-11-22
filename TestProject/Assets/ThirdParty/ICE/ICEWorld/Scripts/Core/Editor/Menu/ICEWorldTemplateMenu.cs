// ##############################################################################
//
// ICEWorldTemplateMenu.cs | ICEWorldTemplateMenu
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

namespace ICE.World.Menus
{
	public class ICEWorldTemplateMenu : MonoBehaviour {

		[MenuItem ( "Assets/Create/ICE/C# ICEWorldEntity Template", false, 1000 )]
		static void AddICEWorldEntityTemplate() {

			ICEWorldTemplateData _data = new ICEWorldTemplateData();

			_data.ClassName = "NewWorldEntity";
			_data.Namespace = "";
			_data.ProjectName = "";

			var selected = Selection.activeObject;

			string _path = AssetDatabase.GetAssetPath(selected);

			ICEWorldTemplateDesigner.CreateWorldEntity( _data, _path );

			AssetDatabase.Refresh();
		}
			
		[MenuItem ( "Assets/Create/ICE/C# ICEWorldBehaviour Template", false, 1001 )]
		static void AddICEWorldBehaviourTemplate() {

			ICEWorldTemplateData _data = new ICEWorldTemplateData();

			_data.ClassName = "NewWorldBehaviour";
			_data.Namespace = "";
			_data.ProjectName = "";

			var selected = Selection.activeObject;

			string _path = AssetDatabase.GetAssetPath(selected);

			ICEWorldTemplateDesigner.CreateWorldBehaviour( _data, _path );

			AssetDatabase.Refresh();
		}
	}
}
