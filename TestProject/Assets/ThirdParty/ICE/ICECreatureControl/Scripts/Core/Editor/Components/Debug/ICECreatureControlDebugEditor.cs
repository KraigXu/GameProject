// ##############################################################################
//
// ICECreatureControlDebugEditor.cs
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
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.EditorUtilities;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures
{
	[CustomEditor(typeof(ICECreatureControlDebug))]
	public class ICECreatureControlDebugEditor : ICECreatureEntityDebugEditor
	{
		public override void OnInspectorGUI()
		{
			ICECreatureControlDebug _target = DrawDefaultHeader<ICECreatureControlDebug>();

			DrawEntityDebugContent( _target );
			DrawCreatureControlDebugContent( _target );
			DrawPointer( _target );
			DrawDefaultFooter( _target );
		}

		void DrawCreatureControlDebugContent( ICECreatureControlDebug _target )
		{
	
			_target.CreatureDebug.Gizmos.Enabled = ICEEditorLayout.Toggle("Use Advanced Gizmos", "", _target.CreatureDebug.Gizmos.Enabled );
			
			if( _target.CreatureDebug.Gizmos.Enabled )
			{
								
				EditorGUI.indentLevel++;
				
					ICEEditorLayout.BeginHorizontal();

					/*
					LabelType label = (LabelType)ICEEditorLayout.EnumPopup ("Labels", m_creature_debug.Debug.Gizmos.Label );
					
					if( label != m_creature_debug.Debug.Gizmos.Label )
					{
						m_creature_debug.Debug.Gizmos.Label = label;
						ICEEditorLayout.AssignLabel( _creature_control.gameObject, (int)m_creature_debug.Debug.Gizmos.Label );
					}*/
					
					ICEEditorLayout.EndHorizontal();
					
					//EditorGUILayout.Separator();
					
					//m_creature_debug.CreatureDebug.Gizmos.ShowText = EditorGUILayout.Toggle( "Show Text", m_creature_debug.CreatureDebug.Gizmos.ShowText );
					_target.CreatureDebug.Gizmos.Level = ICEEditorLayout.Slider( "Gizmos Offset", "", _target.CreatureDebug.Gizmos.Level, 0.5f, 0,50 );
					_target.CreatureDebug.Gizmos.UseObjectLevel = ICEEditorLayout.Toggle( "Use Object Level", "", _target.CreatureDebug.Gizmos.UseObjectLevel );

					EditorGUILayout.Separator();
					ICEEditorLayout.Label( "Move Gizmos", true );
					EditorGUI.indentLevel++;					
		
						_target.CreatureDebug.Gizmos.ShowPath = ICEEditorLayout.Toggle( "Path", "", _target.CreatureDebug.Gizmos.ShowPath );
								
						EditorGUI.BeginDisabledGroup( _target.CreatureDebug.Gizmos.ShowPath == false );
						EditorGUI.indentLevel++;
							_target.CreatureDebug.Gizmos.PathPositionsLimit = (int)ICEEditorLayout.DefaultSlider( "Max. Path Length", "", _target.CreatureDebug.Gizmos.PathPositionsLimit, 1, 10,10000, 100 );
							_target.CreatureDebug.Gizmos.PathPrecision = ICEEditorLayout.DefaultSlider( "Path Precision", "", _target.CreatureDebug.Gizmos.PathPrecision, 0.25f, 0,5, 0.5f );
							EditorGUILayout.Separator();
							_target.CreatureDebug.Gizmos.MoveProjectedPathColor = ICEEditorLayout.DefaultColor ("Projected Path", "", _target.CreatureDebug.Gizmos.MoveProjectedPathColor, Init.GIZMOS_COLOR_PATH_PROJECTED );
							_target.CreatureDebug.Gizmos.MovePreviousPathColor = ICEEditorLayout.DefaultColor ("Previous Path", "", _target.CreatureDebug.Gizmos.MovePreviousPathColor, Init.GIZMOS_COLOR_PATH_PREVIOUS );
							_target.CreatureDebug.Gizmos.MoveCurrentPathColor = ICEEditorLayout.DefaultColor ("Current Path", "", _target.CreatureDebug.Gizmos.MoveCurrentPathColor, Init.GIZMOS_COLOR_PATH_CURRENT );
						EditorGUI.indentLevel--;
						EditorGUI.EndDisabledGroup();
						

						EditorGUILayout.Separator();
						_target.CreatureDebug.Gizmos.MoveColor = ICEEditorLayout.DefaultColor ("Move", "", _target.CreatureDebug.Gizmos.MoveColor, Init.GIZMOS_COLOR_MOVE );
						EditorGUI.indentLevel++;
							_target.CreatureDebug.Gizmos.MoveDetourColor = ICEEditorLayout.DefaultColor ("Detour", "", _target.CreatureDebug.Gizmos.MoveDetourColor, Init.GIZMOS_COLOR_MOVE_DETOUR );
							_target.CreatureDebug.Gizmos.MoveOrbitColor = ICEEditorLayout.DefaultColor ("Orbit", "", _target.CreatureDebug.Gizmos.MoveOrbitColor, Init.GIZMOS_COLOR_MOVE_ORBIT );
							_target.CreatureDebug.Gizmos.MoveEscapeColor = ICEEditorLayout.DefaultColor ("Escape", "", _target.CreatureDebug.Gizmos.MoveEscapeColor, Init.GIZMOS_COLOR_MOVE_ESCAPE );
							_target.CreatureDebug.Gizmos.MoveAvoidColor = ICEEditorLayout.DefaultColor ("Avoid", "", _target.CreatureDebug.Gizmos.MoveAvoidColor, Init.GIZMOS_COLOR_MOVE_AVOID );
						EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;

					EditorGUILayout.Separator();
					ICEEditorLayout.Label( "Target Gizmos", true );
					EditorGUI.indentLevel++;
						_target.CreatureDebug.Gizmos.TargetColor = ICEEditorLayout.DefaultColor ("inactive", "", _target.CreatureDebug.Gizmos.TargetColor, Init.GIZMOS_COLOR_TARGET );
						_target.CreatureDebug.Gizmos.ActiveTargetColor = ICEEditorLayout.DefaultColor("active", "", _target.CreatureDebug.Gizmos.ActiveTargetColor, Init.GIZMOS_COLOR_TARGET_ACTIVE );
					
						EditorGUILayout.Separator();
						_target.CreatureDebug.Gizmos.TargetStoppingDistanceColor = ICEEditorLayout.DefaultColor("Stopping Distance", "", _target.CreatureDebug.Gizmos.TargetStoppingDistanceColor, Init.GIZMOS_COLOR_TARGET_STOPPING_DISTANCE );
						_target.CreatureDebug.Gizmos.TargetSelectionRangeColor = ICEEditorLayout.DefaultColor("Selection Range", "", _target.CreatureDebug.Gizmos.TargetSelectionRangeColor, Init.GIZMOS_COLOR_TARGET_SELECTION_RANGE );
						_target.CreatureDebug.Gizmos.TargetRandomRangeColor = ICEEditorLayout.DefaultColor("Random Positioning Range", "", _target.CreatureDebug.Gizmos.TargetRandomRangeColor, Init.GIZMOS_COLOR_TARGET_RANDOM_RANGE );

					EditorGUI.indentLevel--;	

					EditorGUILayout.Separator();

					_target.CreatureDebug.Gizmos.ShowHome = ICEEditorLayout.ToggleLeft( "Home Gizmos", "", _target.CreatureDebug.Gizmos.ShowHome, true );
					_target.CreatureDebug.Gizmos.ShowOutpost = ICEEditorLayout.ToggleLeft( "Mission Outpost Gizmos", "", _target.CreatureDebug.Gizmos.ShowOutpost, true );
					_target.CreatureDebug.Gizmos.ShowEscort = ICEEditorLayout.ToggleLeft( "Mission Escort Gizmos", "", _target.CreatureDebug.Gizmos.ShowEscort, true );
					_target.CreatureDebug.Gizmos.ShowPatrol = ICEEditorLayout.ToggleLeft( "Mission Patrol Gizmos", "", _target.CreatureDebug.Gizmos.ShowPatrol, true );


					_target.CreatureDebug.Gizmos.ShowInteractor = ICEEditorLayout.ToggleLeft( "Interaction Gizmos", "", _target.CreatureDebug.Gizmos.ShowInteractor, true );

					EditorGUI.BeginDisabledGroup( _target.CreatureDebug.Gizmos.ShowInteractor == false );
					EditorGUI.indentLevel++;	
						_target.CreatureDebug.Gizmos.InteractionColor = ICEEditorLayout.DefaultColor("Interaction", "", _target.CreatureDebug.Gizmos.InteractionColor, Init.GIZMOS_COLOR_INTERACTION );
						/*EditorGUI.indentLevel++;
						
							m_creature_debug.CreatureDebug.Gizmos.ShowSolidInteractionRange = ICEEditorLayout.Toggle( "Solid Interaction Range", "", m_creature_debug.CreatureDebug.Gizmos.ShowSolidInteractionRange );

							EditorGUI.BeginDisabledGroup( m_creature_debug.CreatureDebug.Gizmos.ShowSolidInteractionRange == false );
							m_creature_debug.CreatureDebug.Gizmos.SolidInteractionAlpha = ICEEditorLayout.DefaultSlider( "Solid Interaction Range Alpha", "", m_creature_debug.CreatureDebug.Gizmos.SolidInteractionAlpha, 0.005f, 0, 1, Init.GIZMOS_COLOR_INTERACTION_ALPHA);
							EditorGUI.EndDisabledGroup();
				
						EditorGUI.indentLevel--;	*/
					EditorGUI.indentLevel--;	
					EditorGUILayout.Separator();
					EditorGUI.EndDisabledGroup();
				/*
								ICEEditorLayout.BeginHorizontal();
								_creature_control.Action.Move.Gizmos.TargetMovePosition = EditorGUILayout.ColorField ("Waypoint", _creature_control.Action.Move.Gizmos.WaypointColor );
								*/

				
				EditorGUI.indentLevel--;
				
				EditorGUILayout.Separator();
			}

		}

		void DrawPointer( ICECreatureControlDebug _target )
		{
			ICEEditorLayout.Label( "Runtime Pointer", false );
			EditorGUI.indentLevel++;
				_target.CreatureDebug.MovePointer.Enabled = ICEEditorLayout.Toggle("Use Path Pointer", "", _target.CreatureDebug.MovePointer.Enabled );
				if( _target.CreatureDebug.MovePointer.Enabled )
				{
					EditorGUI.indentLevel++;					
						_target.CreatureDebug.MovePointer.PointerType = (PrimitiveType)ICEEditorLayout.EnumPopup ("Type","", _target.CreatureDebug.MovePointer.PointerType );
						_target.CreatureDebug.MovePointer.PointerSize = ICEEditorLayout.DefaultVector3Field ("Size","", _target.CreatureDebug.MovePointer.PointerSize, new Vector3( 0.5f, 0.25f, 0.5f ) );
						_target.CreatureDebug.MovePointer.PointerColor = ICEEditorLayout.DefaultColor ("Color", "", _target.CreatureDebug.MovePointer.PointerColor, Init.GIZMOS_COLOR_POINTER_MOVE );
						_target.CreatureDebug.MovePointer.PointerName = "Pointer : " + _target.name + "(" + _target.GetInstanceID() + ") MovePosition" ;
					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();
				}

				_target.CreatureDebug.TargetMovePositionPointer.Enabled = ICEEditorLayout.Toggle("Use Destination Pointer", "", _target.CreatureDebug.TargetMovePositionPointer.Enabled );
				if( _target.CreatureDebug.TargetMovePositionPointer.Enabled )
				{
					EditorGUI.indentLevel++;					
						_target.CreatureDebug.TargetMovePositionPointer.PointerType = (PrimitiveType)ICEEditorLayout.EnumPopup ("Type","", _target.CreatureDebug.TargetMovePositionPointer.PointerType );
						_target.CreatureDebug.TargetMovePositionPointer.PointerSize = ICEEditorLayout.DefaultVector3Field ("Size","", _target.CreatureDebug.TargetMovePositionPointer.PointerSize, new Vector3( 0.25f, 1f, 0.25f ) );
						_target.CreatureDebug.TargetMovePositionPointer.PointerColor = ICEEditorLayout.DefaultColor ("Color", "", _target.CreatureDebug.TargetMovePositionPointer.PointerColor, Init.GIZMOS_COLOR_POINTER_MOVE_TARGET );
						_target.CreatureDebug.TargetMovePositionPointer.PointerName = "Pointer : " + _target.name + "(" + _target.GetInstanceID() + ") TargetMovePosition" ;
					EditorGUI.indentLevel--;					
					EditorGUILayout.Separator();
				}

				_target.CreatureDebug.DesiredTargetMovePositionPointer.Enabled = ICEEditorLayout.Toggle("Use Desired Destination Pointer", "", _target.CreatureDebug.DesiredTargetMovePositionPointer.Enabled );
				if( _target.CreatureDebug.DesiredTargetMovePositionPointer.Enabled )
				{
					EditorGUI.indentLevel++;					
						_target.CreatureDebug.DesiredTargetMovePositionPointer.PointerType = (PrimitiveType)ICEEditorLayout.EnumPopup ("Type","", _target.CreatureDebug.DesiredTargetMovePositionPointer.PointerType );
						_target.CreatureDebug.DesiredTargetMovePositionPointer.PointerSize = ICEEditorLayout.DefaultVector3Field ("Size","", _target.CreatureDebug.DesiredTargetMovePositionPointer.PointerSize, new Vector3( 0.15f, 2f, 0.15f ) );
						_target.CreatureDebug.DesiredTargetMovePositionPointer.PointerColor = ICEEditorLayout.DefaultColor ("Color", "", _target.CreatureDebug.DesiredTargetMovePositionPointer.PointerColor, Init.GIZMOS_COLOR_POINTER_MOVE_DESIRED );
						_target.CreatureDebug.DesiredTargetMovePositionPointer.PointerName = "Pointer : " + _target.name + "(" + _target.GetInstanceID() + ") DesiredTargetMovePosition" ;
					EditorGUI.indentLevel--;					
					EditorGUILayout.Separator();
				}
			EditorGUI.indentLevel--;			
			EditorGUILayout.Separator();
		}
	}
}
