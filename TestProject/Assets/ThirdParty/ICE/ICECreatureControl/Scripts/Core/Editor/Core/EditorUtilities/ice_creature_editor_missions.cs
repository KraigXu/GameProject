// ##############################################################################
//
// ice_creature_editor_missions.cs | MissionsEditor
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
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures.EditorUtilities
{
	
	public static class MissionsEditor
	{	
		public static void Print( ICECreatureControl _control )
		{
			if( ! _control.Display.ShowMissions )
				return;

			ICEEditorStyle.SplitterByIndent( 0 );
			_control.Display.FoldoutMissions = ICEEditorLayout.Foldout( _control.Display.FoldoutMissions, "Missions", Info.MISSIONS );
			
			if( ! _control.Display.FoldoutMissions ) 
				return;

			EditorGUI.indentLevel++;

			DrawMissionOutpost( _control );
			DrawMissionEscort( _control );
			DrawMissionPatrol( _control );

			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
		}

		private static void DrawMissionOutpost( ICECreatureControl _control ){
			
			ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );

			//HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();
				_control.Creature.Missions.Outpost.Foldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Outpost.Foldout, "Outpost Mission" );

				if( ICEEditorLayout.SaveButton() )
					CreatureEditorIO.SaveMissionOutpostToFile( _control.Creature.Missions.Outpost, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton() )
					_control.Creature.Missions.Outpost = CreatureEditorIO.LoadMissionOutpostFromFile( _control.Creature.Missions.Outpost );
				if( ICEEditorLayout.ResetButton() )
					_control.Creature.Missions.Outpost = new OutpostObject();

				_control.Creature.Missions.Outpost.Enabled = ICEEditorLayout.EnableButton( _control.Creature.Missions.Outpost.Enabled );
			ICEEditorLayout.EndHorizontal( Info.MISSION_OUTPOST);
			//HEADER END

			if ( ! _control.Creature.Missions.Outpost.Foldout ) 
				return;

			EditorGUI.BeginDisabledGroup ( _control.Creature.Missions.Outpost.Enabled == false );	
			EditorGUI.indentLevel++;
				TargetEditor.DrawMissionTarget( _control, _control.Creature.Missions.Outpost.Target, "Target", Info.MISSION_OUTPOST_TARGET );

				EditorGUI.BeginDisabledGroup( _control.Creature.Missions.Outpost.Target.TargetGameObject == null );
				_control.Creature.Missions.Outpost.BehaviourFoldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Outpost.BehaviourFoldout , "Behaviour", Info.MISSION_OUTPOST_BEHAVIOR, true );
				if( _control.Creature.Missions.Outpost.BehaviourFoldout )
				{
					_control.Creature.Missions.Outpost.BehaviourModeTravel = BehaviourEditor.BehaviourSelect( _control, "Travel", "Move behaviour to reach the Outpost", _control.Creature.Missions.Outpost.BehaviourModeTravel , "OUTPOST_TRAVEL" ); 
					_control.Creature.Missions.Outpost.BehaviourModeRendezvous = BehaviourEditor.BehaviourSelect( _control, "Rendezvous", "Idle behaviour after reaching the current target move position.", _control.Creature.Missions.Outpost.BehaviourModeRendezvous, "OUTPOST_RENDEZVOUS" ); 
					if( _control.Creature.Missions.Outpost.Target.Move.HasRandomRange )
						_control.Creature.Missions.Outpost.BehaviourModeLeisure = BehaviourEditor.BehaviourSelect( _control, "Leisure", "Randomized leisure activities around the Outpost", _control.Creature.Missions.Outpost.BehaviourModeLeisure, "OUTPOST_LEISURE" ); 	
				}
				EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();


			EditorGUILayout.Separator();

		}

		private static void DrawMissionEscort( ICECreatureControl _control  ){
			
			ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );

			//HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();
				_control.Creature.Missions.Escort.Foldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Escort.Foldout , "Escort Mission" );

				if( ICEEditorLayout.SaveButton() )
					CreatureEditorIO.SaveMissionEscortToFile( _control.Creature.Missions.Escort, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton() )
					_control.Creature.Missions.Escort = CreatureEditorIO.LoadMissionEscortFromFile( _control.Creature.Missions.Escort );
				if( ICEEditorLayout.ResetButton() )
					_control.Creature.Missions.Escort = new EscortObject();
			
				_control.Creature.Missions.Escort.Enabled = ICEEditorLayout.EnableButton( _control.Creature.Missions.Escort.Enabled );
			ICEEditorLayout.EndHorizontal( Info.MISSION_ESCORT );
			//HEADER END

			if ( ! _control.Creature.Missions.Escort.Foldout ) 
				return;

			EditorGUI.BeginDisabledGroup ( _control.Creature.Missions.Escort.Enabled == false);		
			EditorGUI.indentLevel++;
				TargetEditor.DrawMissionTarget( _control, _control.Creature.Missions.Escort.Target, "Target", Info.MISSION_ESCORT_TARGET );
				
				EditorGUI.BeginDisabledGroup( _control.Creature.Missions.Escort.Target.TargetGameObject == null );
					_control.Creature.Missions.Escort.BehaviourFoldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Escort.BehaviourFoldout , "Behaviour", Info.MISSION_ESCORT_BEHAVIOUR, true );
					if( _control.Creature.Missions.Escort.BehaviourFoldout )
					{
						_control.Creature.Missions.Escort.BehaviourModeFollow = BehaviourEditor.BehaviourSelect( _control, "Follow", "Move behaviour to follow and reach the leader", _control.Creature.Missions.Escort.BehaviourModeFollow, "ESCORT_FOLLOW" );
						_control.Creature.Missions.Escort.BehaviourModeEscort = BehaviourEditor.BehaviourSelect( _control, "Escort", "Move behaviour to escort the leader", _control.Creature.Missions.Escort.BehaviourModeEscort, "ESCORT" );		
						_control.Creature.Missions.Escort.BehaviourModeStandby = BehaviourEditor.BehaviourSelect( _control, "Standby", "Idle behaviour if the leader stops", _control.Creature.Missions.Escort.BehaviourModeStandby, "ESCORT_STANDBY" );
						EditorGUI.indentLevel++;				
							_control.Creature.Missions.Escort.DurationStandby = ICEEditorLayout.Slider( "Duration (until IDLE)", "", _control.Creature.Missions.Escort.DurationStandby, 1, 0, 60 );				
						EditorGUI.indentLevel--;				
						_control.Creature.Missions.Escort.BehaviourModeIdle = BehaviourEditor.BehaviourSelect( _control, "Idle", "Idle behaviour if the leader breaks for a longer time-span", _control.Creature.Missions.Escort.BehaviourModeIdle, "ESCORT_IDLE" );
					}
				EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.Separator();
		}
		
		private static void DrawMissionPatrol( ICECreatureControl _control )
		{
			ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );

			//HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();
				_control.Creature.Missions.Patrol.Foldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Patrol.Foldout, "Patrol Mission" );
				if( ICEEditorLayout.SaveButton() )
					CreatureEditorIO.SaveMissionPatrolToFile( _control.Creature.Missions.Patrol, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton() )
					_control.Creature.Missions.Patrol = CreatureEditorIO.LoadMissionPatrolFromFile( _control.Creature.Missions.Patrol );	
				if( ICEEditorLayout.ResetButton() )
					_control.Creature.Missions.Patrol = new PatrolObject();			
				_control.Creature.Missions.Patrol.Enabled = ICEEditorLayout.EnableButton( _control.Creature.Missions.Patrol.Enabled );
			ICEEditorLayout.EndHorizontal( Info.MISSION_PATROL );
			//HEADER END

			if( ! _control.Creature.Missions.Patrol.Foldout ) 
				return;

			EditorGUI.BeginDisabledGroup ( _control.Creature.Missions.Patrol.Enabled == false);
			


				EditorGUILayout.Separator();	

				EditorGUI.indentLevel++;	
					_control.Creature.Missions.Patrol.BehaviourFoldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Patrol.BehaviourFoldout , "Behaviour", Info.MISSION_OUTPOST_BEHAVIOR, true );
					if( _control.Creature.Missions.Patrol.BehaviourFoldout )
					{
						_control.Creature.Missions.Patrol.BehaviourModeTravel = BehaviourEditor.BehaviourSelect( _control, "Travel", "Default travel behaviour to reach the first waypoint",  _control.Creature.Missions.Patrol.BehaviourModeTravel, "WP_TRAVEL" );
						_control.Creature.Missions.Patrol.BehaviourModePatrol = BehaviourEditor.BehaviourSelect( _control, "Patrol","Default patrol behaviour to reach the next waypoint", _control.Creature.Missions.Patrol.BehaviourModePatrol, "WP_PATROL" );
						BehaviourEditor.DrawInRangeBehaviour( _control,
							ref _control.Creature.Missions.Patrol.BehaviourModeLeisure,
							ref _control.Creature.Missions.Patrol.BehaviourModeRendezvous,
							ref _control.Creature.Missions.Patrol.DurationOfStay,
							ref _control.Creature.Missions.Patrol.IsTransitPoint,
							1 );
						EditorGUILayout.Separator();
					}
				EditorGUI.indentLevel--;
					


			EditorGUI.indentLevel++;	
			ICEEditorLayout.BeginHorizontal();

				int _count_all = _control.Creature.Missions.Patrol.Waypoints.Waypoints.Count;
				int _count_valid = _control.Creature.Missions.Patrol.Waypoints.GetValidWaypoints().Count;			
				string _title = "Waypoints" + ( _count_all > 0 ? " (" + _count_valid + "/" + _count_all + ")" : "" );
				_control.Creature.Missions.Patrol.WaypointsFoldout = ICEEditorLayout.Foldout( _control.Creature.Missions.Patrol.WaypointsFoldout , _title , true );


				_control.Creature.Missions.Patrol.Waypoints.WaypointGroup = (GameObject)EditorGUILayout.ObjectField( _control.Creature.Missions.Patrol.Waypoints.WaypointGroup, typeof(GameObject), true, GUILayout.Width( 65 ) );
				_control.Creature.Missions.Patrol.Waypoints.WaypointGroup = null;

				//if( ICEEditorLayout.ButtonMiddle( "RESET", "Removes all waypoints." ) )
				//	_control.Creature.Missions.Patrol.Waypoints.Waypoints.Clear();

				if( ICEEditorLayout.ListClearButton<WaypointObject>( _control.Creature.Missions.Patrol.Waypoints.Waypoints ) )
					return;

				if( ICEEditorLayout.AddButtonSmall( "Adds a new waypoint item." ) )
					_control.Creature.Missions.Patrol.Waypoints.Waypoints.Add( new WaypointObject() );

				GUILayout.Space( 5 );
				ICEEditorLayout.ListFoldoutButtons<WaypointObject>( _control.Creature.Missions.Patrol.Waypoints.Waypoints );

			ICEEditorLayout.EndHorizontal( Info.MISSION_PATROL_WAYPOINTS );
			if( _control.Creature.Missions.Patrol.WaypointsFoldout )
			{
				ICEEditorLayout.BeginHorizontal();
					_control.Creature.Missions.Patrol.Waypoints.Order = (WaypointOrderType)ICEEditorLayout.EnumPopup("Order Type","", _control.Creature.Missions.Patrol.Waypoints.Order );
					EditorGUI.BeginDisabledGroup( _control.Creature.Missions.Patrol.Waypoints.Order != WaypointOrderType.CYCLE );
					_control.Creature.Missions.Patrol.Waypoints.Ascending = ! ICEEditorLayout.CheckButtonSmall( "DESC", "descending order", ! _control.Creature.Missions.Patrol.Waypoints.Ascending );
					EditorGUI.EndDisabledGroup();	
				ICEEditorLayout.EndHorizontal( Info.MISSION_PATROL_ORDER_TYPE);

				EditorGUILayout.Separator();					
				for (int i = 0; i < _control.Creature.Missions.Patrol.Waypoints.Waypoints.Count ; ++i)
					if( DrawMissionPatrolWaypoint( _control, i ) )
						return;
			}
			EditorGUI.indentLevel--;				

			EditorGUI.EndDisabledGroup();
			EditorGUILayout.Separator();	
		}

		private static bool DrawMissionPatrolWaypoint( ICECreatureControl _control, int _index )
		{
			ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );
			WaypointObject _waypoint = _control.Creature.Missions.Patrol.Waypoints.Waypoints[ _index ];

			if( _waypoint == null )
				return false;

			// HEADER BEGIN
			ICEEditorLayout.BeginHorizontal();		
				_waypoint.WaypointFoldout = ICEEditorLayout.Foldout( _waypoint.WaypointFoldout, "Waypoint #"+ (int)(_index + 1) + ( ! string.IsNullOrEmpty( _waypoint.TargetName ) ? " (" + _waypoint.TargetName + ")": "" ), "", true );
				
				if( ICEEditorLayout.ListDeleteButton<WaypointObject>( _control.Creature.Missions.Patrol.Waypoints.Waypoints, _waypoint ) )
					return true;
				
				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListUpDownButtons<WaypointObject>( _control.Creature.Missions.Patrol.Waypoints.Waypoints, _control.Creature.Missions.Patrol.Waypoints.Waypoints.IndexOf( _waypoint ) ) )
					return true;
				


				_waypoint.Enabled = ICEEditorLayout.EnableButton( _waypoint.Enabled );

			ICEEditorLayout.EndHorizontal( Info.MISSION_PATROL_WAYPOINT );
			// HEADER END

			// CONTENT BEGIN
			if( _waypoint.WaypointFoldout )
			{
				EditorGUI.indentLevel++;
				EditorGUI.BeginDisabledGroup ( _waypoint.Enabled == false);		
					TargetEditor.DrawMissionTarget( _control, (TargetObject)_waypoint, "Target", Info.MISSION_PATROL_TARGET );

					ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup ( _waypoint.UseCustomBehaviour == false);	
							_waypoint.BehaviourFoldout = ICEEditorLayout.Foldout( _waypoint.BehaviourFoldout , "Behaviour", true );
						EditorGUI.EndDisabledGroup();
						_waypoint.UseCustomBehaviour = ICEEditorLayout.CheckButton( "Override", "", _waypoint.UseCustomBehaviour, ICEEditorStyle.ButtonMiddle );
					ICEEditorLayout.EndHorizontal( Info.MISSION_PATROL_CUSTOM_BEHAVIOUR );

					if( ! _waypoint.UseCustomBehaviour )
						_waypoint.BehaviourFoldout = false;

					if( _waypoint.BehaviourFoldout )
					{
						_waypoint.BehaviourModeTravel = BehaviourEditor.BehaviourSelect( _control, "Travel", "Travel behaviour to reach this waypoint and to start this mission", _waypoint.BehaviourModeTravel, "WP_TRAVEL_" + (int)(_index + 1) );
						_waypoint.BehaviourModePatrol = BehaviourEditor.BehaviourSelect( _control, "Patrol", "Patrol behaviour to reach this waypoint", _waypoint.BehaviourModePatrol, "WP_PATROL_" + (int)(_index + 1) );

						BehaviourEditor.DrawInRangeBehaviour( _control,
							ref _waypoint.BehaviourModeLeisure,
							ref _waypoint.BehaviourModeRendezvous,
							ref _waypoint.DurationOfStay,
							ref _waypoint.IsTransitPoint,
							_waypoint.Move.RandomRange,
							(int)(_index + 1) );

						EditorGUILayout.Separator();
					}

				EditorGUI.EndDisabledGroup();	
				EditorGUI.indentLevel--;
			}
			// CONTENT END


			return false;
		}
	}
}
