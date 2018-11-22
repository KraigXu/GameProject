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
using ICE.Creatures.UI;

namespace ICE.Creatures.Menus
{
	public class ICECreatureComponentsUIMenu : MonoBehaviour {

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Creature Status Display", false, 1099 )]
		static void AddStatusDisplayCreature() 
		{
			GameObject _object = Selection.activeObject as GameObject;

			if( _object != null && _object.GetComponentInParent<ICECreatureControlUI>() == null )
				_object.AddComponent<ICECreatureControlUI>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Creature Status Display", true)]
		static bool ValidateStatusDisplayCreature()
		{
			GameObject _obj = Selection.activeObject as GameObject;
			return ( _obj != null && _obj.GetComponentInParent<ICECreatureControl>() != null && _obj.GetComponentInChildren<ICECreatureControlUI>() == null  );
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Player Status Display", false, 1099 )]
		static void AddStatusDisplayPlayer() 
		{
			GameObject _obj = Selection.activeObject as GameObject;

			if( _obj != null && _obj.GetComponentInParent<ICECreaturePlayerUI>() == null && _obj.GetComponentInChildren<ICECreaturePlayerUI>() == null )
				_obj.AddComponent<ICECreaturePlayerUI>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Player Status Display", true)]
		static bool ValidateStatusDisplayPlayer()
		{
			GameObject _obj = Selection.activeObject as GameObject;
			return ( _obj != null && _obj.GetComponentInParent<ICECreaturePlayer>() != null );
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Item Status Display", false, 1099 )]
		static void AddStatusDisplayItem() 
		{
			GameObject _obj = Selection.activeObject as GameObject;

			if( _obj != null && _obj.GetComponentInParent<ICECreatureItemUI>() == null && _obj.GetComponentInChildren<ICECreatureItemUI>() == null )
				_obj.AddComponent<ICECreatureItemUI>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Item Status Display", true)]
		static bool ValidateStatusDisplayItem()
		{
			GameObject _obj = Selection.activeObject as GameObject;
			return ( _obj != null && _obj.GetComponentInParent<ICECreatureItem>() != null );
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Object Status Display", false, 1099 )]
		static void AddStatusDisplayObject() 
		{
			GameObject _obj = Selection.activeObject as GameObject;

			if( _obj != null && _obj.GetComponentInParent<ICECreatureObjectUI>() == null && _obj.GetComponentInChildren<ICECreatureObjectUI>() == null )
				_obj.AddComponent<ICECreatureObjectUI>();
		}

		[MenuItem ( "ICE/ICE Creature Control/Components/UI/Add Object Status Display", true)]
		static bool ValidateStatusDisplayObject()
		{
			GameObject _obj = Selection.activeObject as GameObject;
			return ( _obj != null && _obj.GetComponentInParent<ICECreatureObject>() != null );
		}
	}
}