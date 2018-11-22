// ##############################################################################
//
// ICECreatureComponentMenu.cs
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
	public class ICECreatureAttributesMenu : MonoBehaviour {

		// ATTRIBUTES
		[MenuItem ( "ICE/ICE Creature Control/Components/Attributes/Add Target Attribute", false, 1088 )]
		static void AddTargetAttribute() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureTargetAttribute>() == null )
				_object.AddComponent<ICECreatureTargetAttribute>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Attributes/Add Target Attribute", true)]
		static bool ValidateTargetAttribute(){
			return ValidateAttributeObject();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Attributes/Add Odour Attribute", false, 1088 )]
		static void AddOdourAttribute() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponent<ICECreatureOdourAttribute>() == null )
				_object.AddComponent<ICECreatureOdourAttribute>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/Attributes/Add Odour Attribute", true)]
		static bool ValidateOdourAttribute(){
			return ValidateAttributeObject();
		}

		static bool ValidateAttributeObject() 
		{
			GameObject _obj = Selection.activeObject as GameObject;

			if( _obj != null && 
				_obj.GetComponent<ICECreatureControl>() == null && 
				_obj.GetComponent<ICECreatureRegister>() == null )
				return true;
			else
				return false;
		}
	}
}