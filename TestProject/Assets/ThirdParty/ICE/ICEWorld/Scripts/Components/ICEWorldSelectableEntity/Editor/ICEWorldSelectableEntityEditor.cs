// ##############################################################################
//
// ICECreatureObjectSelectEditor.cs
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

using ICE;
using ICE.World;
using ICE.World.EnumTypes;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.EditorUtilities;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.EnumTypes;

namespace ICE.World
{
	[CustomEditor(typeof(ICEWorldSelectableEntity)), CanEditMultipleObjects]
	public class ICEWorldSelectableEntityEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			Info.HelpButtonIndex = 0;
			ICEWorldSelectableEntity _adapter = (ICEWorldSelectableEntity)target;

			EditorGUILayout.Separator();


			_adapter.SelectType  = (ObjectSelectType)ICEEditorLayout.EnumPopup( "Selection Type", "", _adapter.SelectType );
			if( _adapter.SelectType == ObjectSelectType.OVER )
			{
				EditorGUI.indentLevel++;
					_adapter.SelectDelay = ICEEditorLayout.DefaultSlider( "Selection Delay", "", _adapter.SelectDelay, 0.025f, 0, 1, 0.25f );
				EditorGUI.indentLevel--;
			}

			EditorGUI.BeginDisabledGroup( _adapter.GetComponent<ICECreatureControl>() == null );
				_adapter.FreezeCreature = ICEEditorLayout.Toggle( "Freeze Creature", "Freezes the selected creature", _adapter.FreezeCreature, "" );
			EditorGUI.EndDisabledGroup();
			_adapter.TimeScale = ICEEditorLayout.DefaultSlider( "Time Scale", "", _adapter.TimeScale, 0.025f, 0, Time.timeScale, Time.timeScale );

			_adapter.VisibilityType  = (ObjectSelectVisibilityType)ICEEditorLayout.EnumPopup( "Visibility Type", "", _adapter.VisibilityType );

			EditorGUI.indentLevel++;

				if( _adapter.VisibilityType == ObjectSelectVisibilityType.COLOR )
					_adapter.SelectionColor = ICEEditorLayout.DefaultColor( "Selection Color", "", _adapter.SelectionColor, Color.red, "" );
				else if( _adapter.VisibilityType == ObjectSelectVisibilityType.MATERIAL )
					_adapter.SelectionMaterial = (Material)EditorGUILayout.ObjectField("Selection Material", _adapter.SelectionMaterial, typeof(Material), false);
				else if( _adapter.VisibilityType == ObjectSelectVisibilityType.SHADER )		
					_adapter.SelectionShader = (Shader)EditorGUILayout.ObjectField("Selection Shader", _adapter.SelectionShader, typeof(Shader), false);

				EditorGUILayout.Separator();
				_adapter.SelectionEffect = (GameObject)EditorGUILayout.ObjectField("Selection Effect", _adapter.SelectionEffect, typeof(GameObject), false);
			EditorGUI.indentLevel--;
			
			
		

		}
	}
}