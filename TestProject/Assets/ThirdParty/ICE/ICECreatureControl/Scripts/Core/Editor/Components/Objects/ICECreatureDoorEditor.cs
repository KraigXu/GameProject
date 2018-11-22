// ##############################################################################
//
// ICECreatureDoorEditor.cs
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures
{
	[CustomEditor(typeof(ICECreatureDoor)), CanEditMultipleObjects]
	public class ICECreatureDoorEditor : ICECreatureObjectEditor  
	{
		public override void OnInspectorGUI()
		{
			ICECreatureDoor _target = DrawEntityHeader<ICECreatureDoor>();
			DrawObjectFunctionsContent( _target );
			DrawFooter( _target );
		}

		public virtual void DrawObjectFunctionsContent( ICECreatureDoor _target )
		{
			_target.FunctionalityFoldout = ICEEditorLayout.Foldout( _target.FunctionalityFoldout, "Functionality", true );
				
			if( ! _target.FunctionalityFoldout )
				return;

			EditorGUI.indentLevel++;

				// DOOR SETTINGS
				_target.OpenAnimatorParameter = ICEEditorLayout.AnimatorParametersPopup( _target.GetComponent<Animator>(), "Open Parameter (boolean)", "Animator Open Parameter", _target.OpenAnimatorParameter );

				ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _target.UseFunctionKey == false );
				_target.DoorFunctionKey = (KeyCode)ICEEditorLayout.EnumPopup( "Function Key", "Desired key code to open and close the door", _target.DoorFunctionKey , "" );
				_target.IsOpen = ICEEditorLayout.CheckButtonMiddle( "OPEN", "", _target.IsOpen );
				EditorGUI.EndDisabledGroup();
				_target.UseFunctionKey = ICEEditorLayout.EnableButton( _target.UseFunctionKey );
				ICEEditorLayout.EndHorizontal("");

				ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _target.UseLockToggleKey == false );
				_target.DoorLockToggleKey = (KeyCode)ICEEditorLayout.EnumPopup( "Lock Toggle Key", "Desired key code to lock and unlock the door", _target.DoorLockToggleKey , "" );
				_target.IsLocked = ICEEditorLayout.CheckButtonMiddle( "LOCKED", "", _target.IsLocked );
				EditorGUI.EndDisabledGroup();
				_target.UseLockToggleKey = ICEEditorLayout.EnableButton( _target.UseLockToggleKey );
				ICEEditorLayout.EndHorizontal("");

				_target.ClosingDelayTime = ICEEditorLayout.MaxDefaultSlider( "Closing Delay", "", _target.ClosingDelayTime, 0.01f, 0, ref _target.ClosingDelayMaximum, 0, "" );

			EditorGUI.indentLevel--;
		}
	}
}

