// ##############################################################################
//
// ice_creature_editor_essentials.cs | EssentialsEditor
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

#if UNITY_5_5 || UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.EnumTypes;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures.EditorUtilities
{
	
	public static class EssentialsEditor
	{	
		public static void Print( ICECreatureControl _creature_control )
		{
			if( ! _creature_control.Display.ShowEssentials )
				return;

			ICEEditorStyle.SplitterByIndent( 0 );

			ICEEditorLayout.BeginHorizontal();
			_creature_control.Display.FoldoutEssentials = ICEEditorLayout.Foldout( _creature_control.Display.FoldoutEssentials , "Essentials" );

			if( ICEEditorLayout.SaveButton( "Saves essentials to file" ) )
				CreatureEditorIO.SaveEssentialsToFile( new EssentialsDataObject( _creature_control.Creature.Essentials, _creature_control.Creature.Move ), _creature_control.gameObject.name );	
			
			if( ICEEditorLayout.LoadButton( "Loads essentials form file" ) )
			{
				EssentialsDataObject _essentials = CreatureEditorIO.LoadEssentialsFromFile( new EssentialsDataObject() );

				if( _essentials != null )
				{
					_creature_control.Creature.Essentials = _essentials.Essentials;
					_creature_control.Creature.Move = _essentials.Move;
				}
			}
			if( ICEEditorLayout.ResetButton( "Removes all essentials" ) )
			{
				_creature_control.Creature.Essentials = new EssentialsObject();
				_creature_control.Creature.Move = new MoveObject();
			}
				

			ICEEditorLayout.EndHorizontal( Info.ESSENTIALS );
			
			if( ! _creature_control.Display.FoldoutEssentials ) 
				return;
			EditorGUI.indentLevel++;
				DrawEssentialsHome( _creature_control );
				DrawEssentialsBehaviours( _creature_control );
				DrawEssentialsMotion( _creature_control );
				DrawEssentialsBodyParts( _creature_control );
				DrawEssentialsSystem( _creature_control );
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
		}

		/// <summary>
		/// Handles the essential settings.
		/// </summary>
		/// <param name="_creature_control">_creature_control.</param>
		private static void DrawEssentialsHome( ICECreatureControl _control )
		{
			_control.Creature.Essentials.Target.SetType( TargetType.HOME );

			ICEEditorLayout.BeginHorizontal();	
				_control.Creature.Essentials.Target.Foldout = ICEEditorLayout.Foldout( _control.Creature.Essentials.Target.Foldout, "Home Location" );
				ICEEditorLayout.PriorityButton( _control.Creature.Essentials.Target.SelectionPriority );
			ICEEditorLayout.EndHorizontal(  ref _control.Creature.Essentials.Target.ShowInfoText, ref _control.Creature.Essentials.Target.InfoText, Info.ESSENTIALS_HOME );
			if( _control.Creature.Essentials.Target.Foldout )
			{
				TargetEditor.DrawTargetObject( _control, _control.Creature.Essentials.Target, "", "" );
				TargetEditor.DrawTargetContent( _control, _control.Creature.Essentials.Target, false );

				EditorGUI.indentLevel++;
					EditorGUI.BeginDisabledGroup( _control.Creature.Essentials.Target.TargetGameObject == null );
					ICEEditorLayout.BeginHorizontal();
						_control.Creature.Essentials.BehaviourFoldout = ICEEditorLayout.Foldout( _control.Creature.Essentials.BehaviourFoldout , "Creature Behaviour", false );
						
						if( ICEEditorLayout.AutoButton( "Assigns the default behaviours" ) )
						{
							_control.Creature.Essentials.BehaviourModeTravel = _control.Creature.Essentials.BehaviourModeRun;
							_control.Creature.Essentials.BehaviourModeRendezvous = _control.Creature.Essentials.BehaviourModeIdle;
							_control.Creature.Essentials.BehaviourModeLeisure = _control.Creature.Essentials.BehaviourModeWalk;
						}
					ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_HOME_BEHAVIOUR );
					if( _control.Creature.Essentials.BehaviourFoldout )
					{
						_control.Creature.Essentials.BehaviourModeTravel = BehaviourEditor.BehaviourSelect( _control, "Travel", "Move behaviour to reach the home area", _control.Creature.Essentials.BehaviourModeTravel , "HOME_TRAVEL", Info.ESSENTIALS_HOME_BEHAVIOUR_TRAVEL ); 
						_control.Creature.Essentials.BehaviourModeRendezvous = BehaviourEditor.BehaviourSelect( _control, "Rendezvous", "Idle behaviour after reaching the current target move position", _control.Creature.Essentials.BehaviourModeRendezvous, "HOME_RENDEZVOUS", Info.ESSENTIALS_HOME_BEHAVIOUR_RENDEZVOUS ); 
						if( _control.Creature.Essentials.Target.Move.HasRandomRange )
							_control.Creature.Essentials.BehaviourModeLeisure = BehaviourEditor.BehaviourSelect( _control, "Leisure", "Randomized leisure activities around the home location.", _control.Creature.Essentials.BehaviourModeLeisure, "HOME_LEISURE", Info.ESSENTIALS_HOME_BEHAVIOUR_LEISURE ); 
					}
					EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}
		}

		/// <summary>
		/// Draws the essentials behaviours.
		/// </summary>
		/// <param name="_control">Control.</param>
		private static void DrawEssentialsBehaviours( ICECreatureControl _control )
		{
			_control.Display.FoldoutEssentialsBehaviours = ICEEditorLayout.Foldout( _control.Display.FoldoutEssentialsBehaviours, "Default Behaviours", Info.ESSENTIALS_BEHAVIOURS );				
			if( _control.Display.FoldoutEssentialsBehaviours )
			{
				_control.Creature.Essentials.BehaviourModeIdle = BehaviourEditor.BehaviourSelect( _control, "Idle", "Default rendezvous behaviour after reaching a given target move position.", _control.Creature.Essentials.BehaviourModeIdle, "IDLE", Info.ESSENTIALS_BEHAVIOURS_IDLE );
				_control.Creature.Essentials.BehaviourModeWalk = BehaviourEditor.BehaviourSelect( _control, "Walk", "Default walking behaviour if your creature is close to a target.", _control.Creature.Essentials.BehaviourModeWalk, "WALK", Info.ESSENTIALS_BEHAVIOURS_WALK ); 
				_control.Creature.Essentials.BehaviourModeRun = BehaviourEditor.BehaviourSelect( _control, "Run", "Default behaviour if your creature is on a journey.", _control.Creature.Essentials.BehaviourModeRun, "RUN", Info.ESSENTIALS_BEHAVIOURS_RUN );

				EditorGUILayout.Separator();

				_control.Creature.Essentials.BehaviourModeSpawn = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeSpawnEnabled, "Spawn" + ( ! _control.Creature.Essentials.BehaviourModeSpawnEnabled ? " (Idle)":"") , "Idle behaviour during the respawn process", _control.Creature.Essentials.BehaviourModeSpawn, "SPAWN", Info.ESSENTIALS_BEHAVIOURS_SPAWN );
				if( _control.Creature.Essentials.BehaviourModeSpawnEnabled )
				{
					EditorGUI.indentLevel++;
					ICEEditorLayout.MinMaxRandomDefaultSlider("Recovery Phase (secs.)","Defines how long the creature will be defenceless after spawning.", ref _control.Creature.Status.RecoveryPhaseMin, ref _control.Creature.Status.RecoveryPhaseMax, 0, ref _control.Creature.Status.RecoveryPhaseMaximum, 0.01f, 0.1f, Init.DECIMAL_PRECISION_TIMER, 30, Info.STATUS_RECOVERY_PHASE );
					EditorGUI.indentLevel--;
				}

				_control.Creature.Essentials.BehaviourModeRepose = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeReposeEnabled, "Repose" + ( ! _control.Creature.Essentials.BehaviourModeReposeEnabled ? " (Idle)":""), "Static behaviour if your creature is wounded", _control.Creature.Essentials.BehaviourModeRepose, "REPOSE", Info.ESSENTIALS_BEHAVIOURS_REPOSE );
				if( _control.Creature.Essentials.BehaviourModeReposeEnabled )
				{
					EditorGUI.indentLevel++;
					_control.Creature.Status.FitnessVitalityLimit = ICEEditorLayout.DefaultSlider("Vitality Limit (%)","If the fitness value reached this limit your creature will be to weak for further activities.", _control.Creature.Status.FitnessVitalityLimit, 0.5f, 0, 100,0, Info.STATUS_FITNESS_VITALITY_LIMIT );
					EditorGUI.indentLevel--;
				}
					
				_control.Creature.Essentials.BehaviourModeDead = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeDeadEnabled, "Dead" + ( ! _control.Creature.Essentials.BehaviourModeDeadEnabled ? " (Idle)":""), "Static behaviour if your creature is dead", _control.Creature.Essentials.BehaviourModeDead, "DEAD", Info.ESSENTIALS_BEHAVIOURS_DEAD );
				if( _control.Creature.Essentials.BehaviourModeDeadEnabled )
				{
					EditorGUI.indentLevel++;
					ICEEditorLayout.MinMaxRandomDefaultSlider("Removing Delay (secs.)","Defines how long the creature will be visible after dying and before respawning.", ref _control.Creature.Status.RemovingDelayMin, ref _control.Creature.Status.RemovingDelayMax, 0, ref _control.Creature.Status.RemovingDelayMaximum, 0, 0, Init.DECIMAL_PRECISION_TIMER, 30, Info.STATUS_REMOVING_DELAY );
					EditorGUI.indentLevel--;
				}

				EditorGUILayout.Separator();

				//_control.Creature.Essentials.BehaviourModeWounded = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeWoundedEnabled, "Wounded" + ( ! _control.Creature.Essentials.BehaviourModeWoundedEnabled ? " (Idle)":""), "Static behaviour if your creature is wounded", _control.Creature.Essentials.BehaviourModeWounded, "WOUNDED", Info.ESSENTIALS_BEHAVIOURS_WOUNDED );
				_control.Creature.Essentials.BehaviourModeImpact = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeImpactEnabled, "Impact" + ( ! _control.Creature.Essentials.BehaviourModeImpactEnabled ? " (Idle)":""), "Static behaviour if your creature is affected by the forces of an impact.", _control.Creature.Essentials.BehaviourModeImpact, "IMPACT", Info.ESSENTIALS_BEHAVIOURS_IMPACT );
				_control.Creature.Essentials.BehaviourModeMounted = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeMountedEnabled, "Mounted" + ( ! _control.Creature.Essentials.BehaviourModeMountedEnabled ? " (Idle)":""), "Idle behaviour while the creature is mounted on another entity object", _control.Creature.Essentials.BehaviourModeMounted, "MOUNTED", Info.ESSENTIALS_BEHAVIOURS_MOUNTED );

				EditorGUILayout.Separator();

				_control.Creature.Essentials.BehaviourModeWait = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeWaitEnabled, "Wait", "Static behaviour if the creature is currently blocked", _control.Creature.Essentials.BehaviourModeWait, "WAIT", Info.ESSENTIALS_BEHAVIOURS_WAIT );
				_control.Creature.Essentials.BehaviourModeJump = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeJumpEnabled, "Jump", "Move behaviour if your creature is currently jumping", _control.Creature.Essentials.BehaviourModeJump, "JUMP", Info.ESSENTIALS_BEHAVIOURS_JUMP );
				_control.Creature.Essentials.BehaviourModeFall = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeFallEnabled, "Fall", "Move behaviour if your creature is currently falling", _control.Creature.Essentials.BehaviourModeFall, "FALL", Info.ESSENTIALS_BEHAVIOURS_FALL );
				_control.Creature.Essentials.BehaviourModeSlide = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeSlideEnabled, "Slide", "Move behaviour if your creature is currently sliding", _control.Creature.Essentials.BehaviourModeSlide, "SLIDE", Info.ESSENTIALS_BEHAVIOURS_SLIDE );
				_control.Creature.Essentials.BehaviourModeVault = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeVaultEnabled, "Vault", "Move behaviour if your creature is currently vaulting", _control.Creature.Essentials.BehaviourModeVault, "VAULT", Info.ESSENTIALS_BEHAVIOURS_VAULT );

				EditorGUILayout.Separator();

				_control.Creature.Essentials.BehaviourModeClimb = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeClimbEnabled, "Climb", "Move behaviour if your creature is climbing", _control.Creature.Essentials.BehaviourModeClimb, "CLIMB", Info.ESSENTIALS_BEHAVIOURS_CLIMB );
				if( _control.Creature.Essentials.BehaviourModeClimbEnabled )
				{
					EditorGUI.indentLevel++;
						_control.Creature.Essentials.BehaviourModeClimbDown = BehaviourEditor.BehaviourSelectEnabled( _control, ref _control.Creature.Essentials.BehaviourModeClimbDownEnabled, "Climb Down", "Move behaviour if your creature is climbing", _control.Creature.Essentials.BehaviourModeClimbDown, "CLIMB_DOWN", Info.ESSENTIALS_BEHAVIOURS_CLIMB );

						ICEEditorLayout.Label( "Speed" );
						EditorGUI.indentLevel++;
							_control.Creature.Move.ClimbingSpeed = ICEEditorLayout.MaxDefaultSlider( "Up", "", _control.Creature.Move.ClimbingSpeed, Init.DECIMAL_PRECISION, 0f, ref _control.Creature.Move.ClimbingSpeedMaximum, 1f, "" );
							EditorGUI.BeginDisabledGroup( _control.Creature.Essentials.BehaviourModeClimbDownEnabled == false );
								_control.Creature.Move.ClimbingDownSpeed = ICEEditorLayout.MaxDefaultSlider( "Down", "", _control.Creature.Move.ClimbingDownSpeed, Init.DECIMAL_PRECISION, 0f, ref _control.Creature.Move.ClimbingDownSpeedMaximum, 1f, "" );
							EditorGUI.EndDisabledGroup();
						EditorGUI.indentLevel--;

						_control.Creature.Move.ClimbingOffset = ICEEditorLayout.Vector3Field("Body Offset","", _control.Creature.Move.ClimbingOffset, Info.ESSENTIALS_BEHAVIOURS_CLIMB_OFFSET );

					EditorGUI.indentLevel--;
				}


				EditorGUILayout.Separator();
			}
		}

		private static string m_BodyPartTransformName = "";
		private static void DrawEssentialsBodyParts( ICECreatureControl _control )
		{
			ICECreatureBodyPart[] _parts = _control.transform.GetComponentsInChildren<ICECreatureBodyPart>();

			_control.Display.FoldoutEssentialsBodyParts = ICEEditorLayout.Foldout( _control.Display.FoldoutEssentialsBodyParts, "Body Parts (" + _parts.Length + ")", Info.ESSENTIALS_BODYPARTS );				
			if( _control.Display.FoldoutEssentialsBodyParts )
			{
				

				int _index = 1;
				foreach( ICECreatureBodyPart _part in _parts )
				{
					ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.Label( "BodyPart #" + _index + " - " + _part.name );
					_index++;

					ICEEditorLayout.ButtonSelectObject( _part.gameObject, ICEEditorStyle.CMDButtonDouble  );

					if( ICEEditorLayout.DeleteButton() )
					{
						GameObject.DestroyImmediate( _part );
						return;
					}

					ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_BODYPART );
				}


				ICEEditorLayout.BeginHorizontal();

					m_BodyPartTransformName = ICEEditorLayout.TransformPopup( "Add Body Part", "", m_BodyPartTransformName, _control.transform, true );
					if( ICEEditorLayout.AddButton( "Adds a new BodyPart" ) )
					{
						Transform _child = SystemTools.FindChildByName( m_BodyPartTransformName, _control.transform );
						if( _child != null && _child.GetComponent<ICECreatureBodyPart>() == null )
							_child.gameObject.AddComponent<ICECreatureBodyPart>();
					}
				ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_BODYPART_ADD );
			}
		}

		private static void DrawEssentialsMotion( ICECreatureControl _control )
		{
			_control.Display.FoldoutEssentialsMotion = ICEEditorLayout.Foldout( _control.Display.FoldoutEssentialsMotion, "Motion and Pathfinding", Info.ESSENTIALS_SYSTEM );				
			if( _control.Display.FoldoutEssentialsMotion )
			{

				DrawEssentialsMotionControl( _control );
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				CreatureObjectEditor.DrawBodyDataObject( _control, _control.Creature.Move.DefaultBody, true );
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				DrawEssentialsMotionGroundCheck( _control );
				//ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				DrawEssentialsMotionGravity( _control );
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				CreatureEditorLayout.DrawWaterCheck( ref _control.Creature.Move.WaterCheck, _control.Creature.Move.WaterLayer.Layers );
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				EditorGUI.BeginDisabledGroup( _control.GetComponent<NavMeshAgent>() != null );

					DrawEssentialsObstacleAvoidance( _control );
					ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

					DrawEssentialsOverlapPrevention( _control );
					ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				EditorGUI.EndDisabledGroup();

				DrawEssentialsMotionDeadlock( _control );
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				DrawEssentialsMotionDefaultMove( _control );
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );

				EditorGUILayout.Separator();
			}
		}

		private static void DrawEssentialsSystem( ICECreatureControl _control )
		{
			WorldObjectEditor.DrawEntityRuntimeBehaviourObject( _control, _control.RuntimeBehaviour, EditorHeaderType.FOLDOUT_BOLD );
		}

		private static void DrawEssentialsMotionControl( ICECreatureControl _control )
		{
			if( _control == null )
				return;
			
			NavMeshAgent _nav_mesh_agent = _control.GetComponent<NavMeshAgent>();
			Rigidbody _rigidbody = _control.GetComponent<Rigidbody>();
			CharacterController _character_controller = _control.GetComponent<CharacterController>();

			if( _nav_mesh_agent != null && _nav_mesh_agent.enabled )
				_control.Creature.Move.MotionControl = MotionControlType.NAVMESHAGENT;

			string _motion_control_help = Info.MOTION_CONTROL;
			if( _control.Creature.Move.MotionControl == MotionControlType.NAVMESHAGENT )
				_motion_control_help += "\n\n" + Info.MOTION_CONTROL_NAVMESHAGENT;
			else if( _control.Creature.Move.MotionControl == MotionControlType.RIGIDBODY )
				_motion_control_help += "\n\n" + Info.MOTION_CONTROL_RIGIDBODY;
			else if( _control.Creature.Move.MotionControl == MotionControlType.CHARACTERCONTROLLER )
				_motion_control_help += "\n\n" + Info.MOTION_CONTROL_CHARACTER_CONTROLLER;
			else if( _control.Creature.Move.MotionControl == MotionControlType.CUSTOM )
				_motion_control_help += "\n\n" + Info.MOTION_CONTROL_CUSTOM;

			_control.Creature.Move.MotionControl = (MotionControlType)ICEEditorLayout.EnumPopup("Motion Control","", _control.Creature.Move.MotionControl, _motion_control_help );

			if( _nav_mesh_agent != null )
				_nav_mesh_agent.enabled = ( _control.Creature.Move.MotionControl == MotionControlType.NAVMESHAGENT );

	

			EditorGUI.indentLevel++;
			if( _control.Creature.Move.MotionControl == MotionControlType.NAVMESHAGENT )
			{
				if( _nav_mesh_agent == null )
				{
					ICEEditorLayout.BeginHorizontal();
						ICEEditorLayout.Label( "NavMeshAgent required", false );
						if( ICEEditorLayout.ButtonLarge("ADD AGENT" ))
						{
							_nav_mesh_agent = _control.transform.gameObject.AddComponent<NavMeshAgent>();
							if( _nav_mesh_agent != null )
							{
								_nav_mesh_agent.autoTraverseOffMeshLink = false;
								_nav_mesh_agent.autoBraking = true;
								_nav_mesh_agent.autoRepath = true;
							}
						}
					ICEEditorLayout.EndHorizontal( Info.MOTION_CONTROL_NAVMESHAGENT_MISSING );
				}
				else
				{
					_control.Creature.Move.ObstacleCheck = ObstacleCheckType.NONE;
					_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.NONE;  

					ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup( _control.Creature.Move.UseSamplePosition == false );
							_control.Creature.Move.SamplePositionRange = ICEEditorLayout.DefaultSlider( "Use Sample Position", "", _control.Creature.Move.SamplePositionRange, Init.DECIMAL_PRECISION_DISTANCES, 0.5f, 100f, 5f );
						EditorGUI.EndDisabledGroup();
						_control.Creature.Move.UseSamplePosition = ICEEditorLayout.EnableButton("Use NavMeshAgent SamplePosition", _control.Creature.Move.UseSamplePosition );
					ICEEditorLayout.EndHorizontal( Info.MOTION_CONTROL_NAVMESHAGENT_SAMPLEPOSITION );

					EditorGUILayout.Separator();

					_nav_mesh_agent.autoTraverseOffMeshLink = ICEEditorLayout.Toggle( "Auto Traverse OffMeshLink", "", "(FALSE)", _nav_mesh_agent.autoTraverseOffMeshLink );
					_nav_mesh_agent.autoBraking = ICEEditorLayout.Toggle( "Auto Braking", "", "(TRUE)", _nav_mesh_agent.autoBraking );
					_nav_mesh_agent.autoRepath = ICEEditorLayout.Toggle( "Auto Repath", "", "(TRUE)", _nav_mesh_agent.autoRepath );

					_nav_mesh_agent.angularSpeed = ICEEditorLayout.DefaultSlider( "Angular Speed", "",  _nav_mesh_agent.angularSpeed , Init.DECIMAL_PRECISION, 0, 1000, 500, Info.MOTION_CONTROL_NAVMESHAGENT_ANGULARSPEED );

					// handle non-kinematic rigidbody to avoid physical affects while using the the navmeshagent
					//Rigidbody _rigidbody = _control.GetComponent<Rigidbody>();
					if( _rigidbody != null && ! _rigidbody.isKinematic )
					{
						bool _has_active_collider = false;

						Collider[] _colliders = _control.GetComponentsInChildren<Collider>();
						foreach( Collider _collider in _colliders )
						{
							if( ! _collider.isTrigger )
							{
								_has_active_collider = true;
								break;
							}
						}

						if( _has_active_collider )
						{
							_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
							EditorGUILayout.HelpBox( Info.MOTION_CONTROL_NAVMESHAGENT_RIGIDBODY_INFO, MessageType.None );
						}
					}
				}
			}
			else if( _control.Creature.Move.MotionControl == MotionControlType.RIGIDBODY )
			{
				//Rigidbody _rigidbody = _control.GetComponent<Rigidbody>();

				if( _rigidbody == null )
				{
					ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.Label( "Rigidbody required", false );
					if( ICEEditorLayout.ButtonLarge( "ADD BODY" ) )
						_control.transform.gameObject.AddComponent<Rigidbody>();
					ICEEditorLayout.EndHorizontal( Info.MOTION_CONTROL_RIGIDBODY_MISSING );
				}
				else
				{
					_control.Creature.Move.IgnoreRootMotion = ICEEditorLayout.Toggle( "Ignore Root Motion", "", _control.Creature.Move.IgnoreRootMotion, Info.MOTION_CONTROL_IGNORE_ROOT_MOTION );

					_rigidbody.useGravity = ICEEditorLayout.Toggle( "Gravity", "", _rigidbody.useGravity );
					_rigidbody.isKinematic = ICEEditorLayout.Toggle( "Kinematic", "", _rigidbody.isKinematic );
					_rigidbody.constraints = RigidbodyConstraints.None;

					if( _rigidbody.useGravity == false )
						_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

					_rigidbody.freezeRotation = true;

					Collider _collider = _control.GetComponentInChildren<Collider>();
					if( _collider == null )
					{
						ICEEditorLayout.BeginHorizontal();
							ICEEditorLayout.Label( "Add Collider", false );
							if( ICEEditorLayout.ButtonMiddle( "CAPSULE", "" ) )
								_control.gameObject.AddComponent<CapsuleCollider>();
							if( ICEEditorLayout.ButtonMiddle( "BOX", "" ) )
								_control.gameObject.AddComponent<BoxCollider>();
						ICEEditorLayout.EndHorizontal();
					}
				}
				EditorGUILayout.Separator();
			}
			else if( _control.Creature.Move.MotionControl == MotionControlType.CHARACTERCONTROLLER )
			{
				//CharacterController _character_controller = _control.GetComponent<CharacterController>();

				if( _character_controller == null )
				{
					ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.Label( "CharacterController required", false );
					if( ICEEditorLayout.ButtonLarge( "ADD CONTROLLER" ) )
						_control.transform.gameObject.AddComponent<CharacterController>();
					ICEEditorLayout.EndHorizontal( Info.MOTION_CONTROL_CHARACTER_CONTROLLER_MISSING );
					EditorGUILayout.Separator();
				}
				else
				{
					_control.Creature.Move.IgnoreRootMotion = ICEEditorLayout.Toggle( "Ignore Root Motion", "", _control.Creature.Move.IgnoreRootMotion, Info.MOTION_CONTROL_IGNORE_ROOT_MOTION  );

					//_character_controller.detectCollisions = ICEEditorLayout.Toggle( "Detect Collisions", "", _character_controller.detectCollisions );
				}

			}
			else if( _control.Creature.Move.MotionControl == MotionControlType.CUSTOM )
			{
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _control.Creature.Move.UseSamplePosition == false );
						_control.Creature.Move.SamplePositionRange = ICEEditorLayout.DefaultSlider( "Use Sample Position", "", _control.Creature.Move.SamplePositionRange, Init.DECIMAL_PRECISION_DISTANCES, 0.5f, 100f, 5f );
					EditorGUI.EndDisabledGroup();
					_control.Creature.Move.UseSamplePosition = ICEEditorLayout.EnableButton("Use Sample Position", _control.Creature.Move.UseSamplePosition );
				ICEEditorLayout.EndHorizontal( Info.MOTION_CONTROL_NAVMESHAGENT_SAMPLEPOSITION );

				/*
						ICEEditorLayout.BeginHorizontal();
							GUILayout.FlexibleSpace();						
							EditorGUILayout.LabelField( new GUIContent( "external pathfinding active", "" ), EditorStyles.wordWrappedMiniLabel );
							GUILayout.FlexibleSpace();
						ICEEditorLayout.EndHorizontal();
						*/
			}
			else 
			{
				_control.Creature.Move.IgnoreRootMotion = ICEEditorLayout.Toggle( "Ignore Root Motion", "", _control.Creature.Move.IgnoreRootMotion, Info.MOTION_CONTROL_IGNORE_ROOT_MOTION  );

				if( _control.GetComponent<Rigidbody>() != null )
				{
					_control.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				}

				_control.Creature.Move.MotionControl = MotionControlType.INTERNAL;
			}
			EditorGUI.indentLevel--;
		}

		private static void DrawEssentialsMotionGroundCheck( ICECreatureControl _control )
		{
			// GROUND CHECK BEGIN
			CreatureEditorLayout.DrawGroundCheck( ref _control.Creature.Move.GroundCheck, _control.Creature.Move.GroundLayer.Layers );
			if( _control.Creature.Move.GroundCheck != GroundCheckType.NONE )
			{
				EditorGUI.indentLevel++;

					_control.BaseOffset = CreatureEditorLayout.DrawBaseOffset( _control.transform, "Base Offset", "", _control.BaseOffset, ref _control.BaseOffsetMaximum, "" );

					if( _control.Creature.Move.GroundCheck == GroundCheckType.RAYCAST )
					{
						_control.Creature.Move.VerticalRaycastOffset = ICEEditorLayout.MaxDefaultSlider( "Vertical Raycast Offset", "", _control.Creature.Move.VerticalRaycastOffset, 0.25f, 0, ref _control.Creature.Move.VerticalRaycastOffsetMaximum,0.5f, Info.ESSENTIALS_SYSTEM_RAYCAST_VERTICAL_OFFSET ); 
						
						EditorGUI.BeginDisabledGroup( ! _control.Creature.Move.GroundAvoidance.AvoidWater && ! _control.Creature.Move.GroundAvoidance.UseSlopeLimits );
						{
							EditorGUILayout.Separator();
							ICEEditorLayout.Label( "Ground Scan", false );
							EditorGUI.indentLevel++;
							_control.Creature.Move.GroundAvoidance.ScanningRange = ICEEditorLayout.MaxDefaultSlider("Scanning Range (units)", "Defines the distance to scan the surface", _control.Creature.Move.GroundAvoidance.ScanningRange, 1, 0.5f, ref _control.Creature.Move.GroundAvoidance.ScanningRangeMaximum, 5, Info.ESSENTIALS_SLOPE_LIMITS_SCANNING_RANGE );
							_control.Creature.Move.GroundAvoidance.ScanningAngle = (int)ICEEditorLayout.DefaultSlider("Scanning Angle (degrees)", "Defines the distance to scan the surface", _control.Creature.Move.GroundAvoidance.ScanningAngle, 1, 1, 45, 10, Info.ESSENTIALS_SLOPE_LIMITS_SCANNING_ANGLE );

							if( _control.Creature.Move.GroundAvoidance.ScanningRangeMaximum < 0.5f )
								_control.Creature.Move.GroundAvoidance.ScanningRangeMaximum = 0.5f;
							EditorGUI.indentLevel--;

						}
						EditorGUI.EndDisabledGroup();

						EditorGUI.indentLevel++;
							_control.Creature.Move.GroundAvoidance.AvoidWater = ICEEditorLayout.Toggle("Avoid Water", "", _control.Creature.Move.GroundAvoidance.AvoidWater,  Info.ESSENTIALS_AVOID_WATER );
							_control.Creature.Move.GroundAvoidance.UseSlopeLimits = ICEEditorLayout.Toggle("Slope Limits", "", _control.Creature.Move.GroundAvoidance.UseSlopeLimits,  Info.ESSENTIALS_SLOPE_LIMITS );
							if( _control.Creature.Move.GroundAvoidance.UseSlopeLimits )
							{
								EditorGUI.indentLevel++;				
									ICEEditorLayout.BeginHorizontal();
										_control.Creature.Move.GroundAvoidance.MaxSurfaceSlopeAngle = ICEEditorLayout.DefaultSlider("Max. Surface Slope Angle", "Maximum allowed slope angle of walkable surfaces", _control.Creature.Move.GroundAvoidance.MaxSurfaceSlopeAngle, 1, 0, 90, 45 );
										EditorGUI.BeginDisabledGroup( _control.Creature.Move.GroundAvoidance.MaxSurfaceSlopeAngle == 0 );
											_control.Creature.Move.GroundAvoidance.UseSlipping = ICEEditorLayout.CheckButtonSmall( "SLIP", "Force slipping down hill whenever the surface slope angle is larger as specified.", _control.Creature.Move.GroundAvoidance.UseSlipping );
										EditorGUI.EndDisabledGroup();
									ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SLOPE_LIMITS_MAX_SLOPE_ANGLE  );
									_control.Creature.Move.GroundAvoidance.MaxPathSlopeAngle = ICEEditorLayout.DefaultSlider("Max. Path Slope Angle", "Maximum allowed slope angle of a walkable path", _control.Creature.Move.GroundAvoidance.MaxPathSlopeAngle, 1, 0, 90, 35, Info.ESSENTIALS_SLOPE_LIMITS_BEST_SLOPE_ANGLE );
								EditorGUI.indentLevel--;		
							}	
							_control.Creature.Move.AllowOutOfArea = ICEEditorLayout.Toggle( "Allow Out-Of-Area Moves", "", _control.Creature.Move.AllowOutOfArea, Info.ESSENTIALS_OUT_OF_AREA );

						EditorGUI.indentLevel--;


					}
					else if( _control.Creature.Move.GroundCheck == GroundCheckType.CUSTOM )
					{
						_control.Creature.Move.CustomGroundLevel = ICEEditorLayout.Float( "Custom Ground Level", "", _control.Creature.Move.CustomGroundLevel, Info.GROUND_CHECK_CUSTOM ); 
					}

				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}
			// GROUND CHECK END
		}

		private static void DrawEssentialsOverlapPrevention( ICECreatureControl _control )
		{
			// OVERLAP PREVENTION BEGIN
			ICEEditorLayout.BeginHorizontal();

				_control.Creature.Move.OverlapPrevention.OverlapPreventionType = (OverlapType)ICEEditorLayout.EnumPopup("Overlap Prevention", "", _control.Creature.Move.OverlapPrevention.OverlapPreventionType );				

				EditorGUI.BeginDisabledGroup( _control.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.NONE );
					_control.Creature.Move.OverlapPrevention.UseAvoiding = ICEEditorLayout.CheckButtonMiddle( "AVOID", "", _control.Creature.Move.OverlapPrevention.UseAvoiding );
				EditorGUI.EndDisabledGroup();

				if( ICEEditorLayout.AutoButton( "" ) )
				{
					if( _control.GetComponent<CapsuleCollider>() != null )
					{
						#if UNITY_5_4_OR_NEWER
							_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.CAPSULE;	
						#else
							_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.SPHERE;	
						#endif
						_control.Creature.Move.OverlapPrevention.Radius = _control.GetComponentInChildren<CapsuleCollider>().radius;
						if( _control.GetComponentInChildren<CapsuleCollider>().direction == 0 ){
							_control.Creature.Move.OverlapPrevention.Center = _control.GetComponent<CapsuleCollider>().center - ( Vector3.right * _control.GetComponent<CapsuleCollider>().height / 2 );
							_control.Creature.Move.OverlapPrevention.End = _control.GetComponent<CapsuleCollider>().center + ( Vector3.right * _control.GetComponent<CapsuleCollider>().height / 2 );
						}else if( _control.GetComponentInChildren<CapsuleCollider>().direction == 2 ){
							_control.Creature.Move.OverlapPrevention.Center = _control.GetComponent<CapsuleCollider>().center - ( Vector3.forward * _control.GetComponent<CapsuleCollider>().height / 2 );
							_control.Creature.Move.OverlapPrevention.End = _control.GetComponent<CapsuleCollider>().center + ( Vector3.forward * _control.GetComponent<CapsuleCollider>().height / 2 );
						}else{
							_control.Creature.Move.OverlapPrevention.Center = _control.GetComponent<CapsuleCollider>().center - ( Vector3.up * _control.GetComponent<CapsuleCollider>().height / 2 );
							_control.Creature.Move.OverlapPrevention.End = _control.GetComponent<CapsuleCollider>().center + ( Vector3.up * _control.GetComponent<CapsuleCollider>().height / 2 );
						}
					}
					else if( _control.GetComponent<CharacterController>() != null )
					{
						#if UNITY_5_4_OR_NEWER
						_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.CAPSULE;	
						#else
						_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.SPHERE;	
						#endif
						_control.Creature.Move.OverlapPrevention.Center = _control.GetComponent<CharacterController>().center - ( Vector3.up * _control.GetComponent<CharacterController>().height / 2 );
						_control.Creature.Move.OverlapPrevention.End = _control.GetComponent<CharacterController>().center + ( Vector3.up * _control.GetComponent<CharacterController>().height / 2 );
					}
					else if( _control.GetComponent<BoxCollider>() != null )
					{
						#if UNITY_5_3 || UNITY_5_3_OR_NEWER
							_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.BOX;
						#else
							_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.SPHERE;	
						#endif
						
						_control.Creature.Move.OverlapPrevention.Center = _control.GetComponent<BoxCollider>().center;
						
						_control.Creature.Move.OverlapPrevention.Size = new Vector3( 
							_control.GetComponent<BoxCollider>().size.x * _control.transform.localScale.x,
							_control.GetComponent<BoxCollider>().size.y * _control.transform.localScale.y,
							_control.GetComponent<BoxCollider>().size.z * _control.transform.localScale.z );
					}
					else if( _control.GetComponent<SphereCollider>() != null )
					{
						_control.Creature.Move.OverlapPrevention.OverlapPreventionType = OverlapType.SPHERE;
						_control.Creature.Move.OverlapPrevention.Center = _control.GetComponent<SphereCollider>().center;
						_control.Creature.Move.OverlapPrevention.Radius = _control.GetComponent<SphereCollider>().radius;
					}
				}
				
			ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION );

			EditorGUI.indentLevel++;

			if( _control.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.SPHERE )
			{
				float _default_radius = ICEEditorLayout.Round( _control.Creature.Move.DefaultBody.GetDefaultSize( _control.gameObject ).magnitude, Init.DECIMAL_PRECISION_DISTANCES ) / 2;
				_control.Creature.Move.OverlapPrevention.Radius = ICEEditorLayout.MaxDefaultSlider( "Radius", "", _control.Creature.Move.OverlapPrevention.Radius, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _control.Creature.Move.OverlapPrevention.OverlapRadiusMaximum, _default_radius, Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_RADIUS );
			}
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
				else if( _control.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.BOX )
				{
					ICEEditorLayout.BeginHorizontal();
					_control.Creature.Move.OverlapPrevention.Size = EditorGUILayout.Vector3Field( new GUIContent( "Size", "" ), _control.Creature.Move.OverlapPrevention.Size );

					_control.Creature.Move.OverlapPrevention.Size = ICEEditorLayout.ButtonDefault( _control.Creature.Move.OverlapPrevention.Size, _control.Creature.Move.DefaultBody.GetDefaultSize( _control.gameObject ) );

					ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_SIZE );		
				}	
#endif

#if UNITY_5_4_OR_NEWER
				else if( _control.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.SPHERE || _control.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.CAPSULE )
				{
					float _default_radius = ICEEditorLayout.Round( _control.Creature.Move.DefaultBody.GetDefaultSize( _control.gameObject ).magnitude, Init.DECIMAL_PRECISION_DISTANCES ) / 2;
					_control.Creature.Move.OverlapPrevention.Radius = ICEEditorLayout.MaxDefaultSlider( "Radius", "", _control.Creature.Move.OverlapPrevention.Radius, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _control.Creature.Move.OverlapPrevention.OverlapRadiusMaximum, _default_radius, Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_RADIUS );
				}			
#endif
	

				if( _control.Creature.Move.OverlapPrevention.OverlapPreventionType != OverlapType.NONE )
				{
					ICEEditorLayout.BeginHorizontal();
						_control.Creature.Move.OverlapPrevention.Center = EditorGUILayout.Vector3Field( new GUIContent( "Center", "" ), _control.Creature.Move.OverlapPrevention.Center );
						_control.Creature.Move.OverlapPrevention.Center = ICEEditorLayout.ButtonDefault( _control.Creature.Move.OverlapPrevention.Center, Vector3.zero );
					ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_CENTER );

#if UNITY_5_4_OR_NEWER
					if( _control.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.CAPSULE )
					{
						ICEEditorLayout.BeginHorizontal();
							_control.Creature.Move.OverlapPrevention.End = EditorGUILayout.Vector3Field( new GUIContent( "End", "" ), _control.Creature.Move.OverlapPrevention.End );
							_control.Creature.Move.OverlapPrevention.End = ICEEditorLayout.ButtonDefault( _control.Creature.Move.OverlapPrevention.End, Vector3.zero );
						ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_END );
					}
#endif
					if( _control.Creature.Move.OverlapPrevention.UseAvoiding )
					{
						_control.Creature.Move.OverlapPrevention.AvoidSpeedMultiplier = ICEEditorLayout.MaxDefaultSlider( "Avoid Speed Multiplier", "", _control.Creature.Move.OverlapPrevention.AvoidSpeedMultiplier, Init.DECIMAL_PRECISION_DISTANCES, 0.01f, ref _control.Creature.Move.OverlapPrevention.AvoidSpeedMultiplierMaximum, 0.5f, Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_AVOID_SPEED_MULTIPLIER );
						_control.Creature.Move.OverlapPrevention.EscapeSpeedMultiplier = ICEEditorLayout.MaxDefaultSlider( "Escape Speed Multiplier", "", _control.Creature.Move.OverlapPrevention.EscapeSpeedMultiplier, Init.DECIMAL_PRECISION_DISTANCES, 0.01f, ref _control.Creature.Move.OverlapPrevention.EscapeSpeedMultiplierMaximum, 1.5f, Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_ESCAPE_SPEED_MULTIPLIER );
						_control.Creature.Move.OverlapPrevention.AngularSpeed = ICEEditorLayout.MaxDefaultSlider( "Angular Speed", "", _control.Creature.Move.OverlapPrevention.AngularSpeed, Init.DECIMAL_PRECISION_DISTANCES, 0.5f, ref _control.Creature.Move.OverlapPrevention.AngularSpeedMaximum, 10, Info.ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_ANGULAR_SPEED );
					}

					EditorGUILayout.Separator();
				}
			EditorGUI.indentLevel--;					
			// OVERLAP PREVENTION END

		}

		private static void DrawEssentialsObstacleAvoidance( ICECreatureControl _control )
		{
			// OBSTACLE AVOIDANCE BEGIN
			EditorGUI.BeginDisabledGroup( _control.Creature.Move.MotionControl == MotionControlType.NAVMESHAGENT );

				CreatureEditorLayout.DrawObstacleCheck( ref _control.Creature.Move.ObstacleCheck, _control.Creature.Move.ObstacleLayer.Layers, true, Info.ESSENTIALS_SYSTEM_AVOIDANCE );
							
				if( _control.Creature.Move.ObstacleCheck == ObstacleCheckType.BASIC )
				{
					EditorGUI.BeginDisabledGroup( _control.Creature.Move.ObstacleLayer.Layers.Count == 0 );

						EditorGUI.indentLevel++;
							// SCANNING RANGE BEGIN
							ICEEditorLayout.BeginHorizontal();
								if( _control.Creature.Move.ObstacleAvoidance.UseDynamicScanningRange )
									_control.Creature.Move.ObstacleAvoidance.DynamicScanningRangeSpeedMultiplier = ICEEditorLayout.MaxDefaultSlider( "Scanning Range (speed multiplier)", "", _control.Creature.Move.ObstacleAvoidance.DynamicScanningRangeSpeedMultiplier, Init.DECIMAL_PRECISION_DISTANCES, 0.5f, ref _control.Creature.Move.ObstacleAvoidance.CheckDistanceMax, 1.5f );
								else
									_control.Creature.Move.ObstacleAvoidance.ScanningRange = ICEEditorLayout.MaxDefaultSlider( "Scanning Range (units)", "", _control.Creature.Move.ObstacleAvoidance.ScanningRange, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _control.Creature.Move.ObstacleAvoidance.CheckDistanceMax,15 );
									_control.Creature.Move.ObstacleAvoidance.UseDynamicScanningRange = ICEEditorLayout.CheckButtonSmall( "DYN", "Enable FIX to prevent needless changes of the given direction!", _control.Creature.Move.ObstacleAvoidance.UseDynamicScanningRange );
							ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_AVOIDANCE_RANGE );
							// SCANNING RANGE END

							// SCANNING ANGLE BEGIN
							ICEEditorLayout.BeginHorizontal();
								_control.Creature.Move.ObstacleAvoidance.ScanningAngle = (int)ICEEditorLayout.DefaultSlider( "Scanning Angle (degrees)", "", _control.Creature.Move.ObstacleAvoidance.ScanningAngle, 1, 0, 45,10 );
								_control.Creature.Move.ObstacleAvoidance.UseFixDirection = ICEEditorLayout.CheckButtonSmall( "FIX", "Enable FIX to prevent needless changes of the given direction!", _control.Creature.Move.ObstacleAvoidance.UseFixDirection );
							ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_AVOIDANCE_ANGLE );
							// SCANNING ANGLE END

							// SCANNING OFFSET BEGIN
							ICEEditorLayout.BeginHorizontal();
								_control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffset = ICEEditorLayout.MaxDefaultSlider( "Raycast Offset", "Level offset of the ray in ralation to the the transform position.", _control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffset, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffsetMaximum, 15 );
							ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_AVOIDANCE_OFFSET );	
							// SCANNING OFFSET END


							_control.Creature.Move.ObstacleAvoidance.UseOvercomeObstacles = ICEEditorLayout.Toggle( "Allow Overcome Obstacles", "", _control.Creature.Move.ObstacleAvoidance.UseOvercomeObstacles, Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME );
							if( _control.Creature.Move.ObstacleAvoidance.UseOvercomeObstacles )
							{
								EditorGUI.indentLevel++;
									_control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffsetDifference = ICEEditorLayout.DefaultSlider( "Overcome Offset Difference", "", _control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffsetDifference, Init.DECIMAL_PRECISION_DISTANCES, 0, _control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffset,_control.Creature.Move.ObstacleAvoidance.VerticalRaycastOffset/2, Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_OFFSET_DIFFERENCE );

									ICEEditorLayout.BeginHorizontal();
										ICEEditorLayout.Label( "Cross Below", false );
										_control.Creature.Move.ObstacleAvoidance.UseCrossBelowSpeed = ICEEditorLayout.CheckButtonMiddle( "SPEED", "", _control.Creature.Move.ObstacleAvoidance.UseCrossBelowSpeed );
									ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_BELOW );
									EditorGUI.indentLevel++;											
										if( _control.Creature.Move.ObstacleAvoidance.UseCrossBelowSpeed )
											_control.Creature.Move.ObstacleAvoidance.CrossBelowStartDistanceSpeedMultiplier = ICEEditorLayout.DefaultSlider( " Start Distance (speed multiplier)", "Cross Below Start Distance", _control.Creature.Move.ObstacleAvoidance.CrossBelowStartDistanceSpeedMultiplier, Init.DECIMAL_PRECISION_DISTANCES, 0, 2,1, Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_START_MULTIPLIER  );
										else
											_control.Creature.Move.ObstacleAvoidance.CrossBelowStartDistance = ICEEditorLayout.DefaultSlider( "Start Distance", "Cross Below Start Distance", _control.Creature.Move.ObstacleAvoidance.CrossBelowStartDistance, Init.DECIMAL_PRECISION_DISTANCES, 0, _control.Creature.Move.ObstacleAvoidance.ScanningRange,_control.Creature.Move.ObstacleAvoidance.ScanningRange/2, Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_START_DISTANCE  );
									EditorGUI.indentLevel--;

									ICEEditorLayout.BeginHorizontal();
										ICEEditorLayout.Label( "Cross Over", false );
										_control.Creature.Move.ObstacleAvoidance.UseCrossOverSpeed = ICEEditorLayout.CheckButtonMiddle( "SPEED", "", _control.Creature.Move.ObstacleAvoidance.UseCrossOverSpeed );
									ICEEditorLayout.EndHorizontal( Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_OVER );
									EditorGUI.indentLevel++;											
										if( _control.Creature.Move.ObstacleAvoidance.UseCrossOverSpeed )
											_control.Creature.Move.ObstacleAvoidance.CrossOverStartDistanceSpeedMultiplier = ICEEditorLayout.DefaultSlider( "Start Distance (speed multiplier)", "", _control.Creature.Move.ObstacleAvoidance.CrossOverStartDistanceSpeedMultiplier, Init.DECIMAL_PRECISION_DISTANCES, 0, 2,1, Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_START_MULTIPLIER );
										else
											_control.Creature.Move.ObstacleAvoidance.CrossOverStartDistance = ICEEditorLayout.DefaultSlider( "Start Distance", "", _control.Creature.Move.ObstacleAvoidance.CrossOverStartDistance, Init.DECIMAL_PRECISION_DISTANCES, 0, _control.Creature.Move.ObstacleAvoidance.ScanningRange,_control.Creature.Move.ObstacleAvoidance.ScanningRange/2, Info.ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_START_DISTANCE  );
									EditorGUI.indentLevel--;
								EditorGUI.indentLevel--;
							}
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();
					EditorGUI.EndDisabledGroup();
				}
								
			EditorGUI.EndDisabledGroup();
			// OBSTACLE AVOIDANCE END
		}
			
		private static void DrawEssentialsMotionGravity( ICECreatureControl _control )
		{
			// GRAVITY BEGIN
			bool _allow_internal_gravity = true;
			if( _control.Creature.Move.MotionControl == MotionControlType.RIGIDBODY && _control.GetComponent<Rigidbody>() != null && _control.GetComponent<Rigidbody>().useGravity )
			{
				_allow_internal_gravity = false;
				_control.Creature.Move.UseInternalGravity = false;
			}
			else if( _control.GetComponent<Rigidbody>() != null && ! _control.GetComponent<Rigidbody>().useGravity )
			{
				_allow_internal_gravity = true;
				_control.Creature.Move.UseInternalGravity = true;
			}

			if( _control.Creature.Move.GroundCheck == GroundCheckType.NONE )
				return;

			EditorGUI.indentLevel++;
			string _title = "Gravity" + ( ! _allow_internal_gravity ? " (Rigidbody)" : "" );

			EditorGUI.BeginDisabledGroup( _allow_internal_gravity == false );
				_control.Creature.Move.UseInternalGravity = ICEEditorLayout.Toggle( _title , "Use internal gravity", _control.Creature.Move.UseInternalGravity,  Info.ESSENTIALS_SYSTEM_GRAVITY );
				if( _control.Creature.Move.UseInternalGravity )
				{
					EditorGUI.indentLevel++;
						_control.Creature.Move.Gravity = ICEEditorLayout.AutoSlider( "Gravity", "Gravity value (default 9.8)", "Will use the default world gravity.", _control.Creature.Move.Gravity, 0.01f, 0, 100, ref _control.Creature.Move.UseWorldGravity, 9.81f, Info.ESSENTIALS_SYSTEM_GRAVITY_VALUE  );
						_control.Creature.Move.FallVelocityMax = ICEEditorLayout.MaxDefaultSlider( "Max. Fall Velocity", "Maximum fall velocity of the creature", _control.Creature.Move.FallVelocityMax, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _control.Creature.Move.FallVelocityMaximum, 250, Info.ESSENTIALS_SYSTEM_GRAVITY_FALL_VELOCITY );
						_control.Creature.Move.GravityInterpolator = ICEEditorLayout.DefaultSlider( "Interpolator", "Interpolates the current fall velocity and the gravity forces.", _control.Creature.Move.GravityInterpolator, Init.DECIMAL_PRECISION_VELOCITY, 0, 1, 0.5f , Info.ESSENTIALS_SYSTEM_GRAVITY_INTERPOLATOR );

						EditorGUILayout.Separator();
					EditorGUI.indentLevel--;
				}
			EditorGUI.EndDisabledGroup();
			// GRAVITY END
			EditorGUI.indentLevel--;

			if( _control.GetComponent<Rigidbody>() != null )
				_control.GetComponent<Rigidbody>().useGravity = ! _control.Creature.Move.UseInternalGravity;
		}

		private static void DrawEssentialsMotionDeadlock( ICECreatureControl _control )
		{
			// DEADLOCK BEGIN
			_control.Creature.Move.Deadlock.Enabled = ICEEditorLayout.Toggle("Deadlock Prevention", "Use deadlock handling", _control.Creature.Move.Deadlock.Enabled, Info.DEADLOCK );
			if( _control.Creature.Move.Deadlock.Enabled )
			{
				EditorGUI.indentLevel++;
				_control.Creature.Move.Deadlock.MinMoveDistance = ICEEditorLayout.DefaultSlider( "Move Distance Check", "Expected distance the creature should have covered until the defined interval", _control.Creature.Move.Deadlock.MinMoveDistance, 0.01f, 0, 5, 0.2f, Info.DEADLOCK_MOVE_DISTANCE );
				EditorGUI.indentLevel++;
				_control.Creature.Move.Deadlock.MoveInterval = ICEEditorLayout.DefaultSlider( "Test Interval (sec.)", "Interval until the next test", _control.Creature.Move.Deadlock.MoveInterval, 0.25f, 0, 30, 2, Info.DEADLOCK_MOVE_INTERVAL );
				_control.Creature.Move.Deadlock.MoveMaxCriticalPositions = (int)ICEEditorLayout.DefaultSlider( "Max. Critical Positions", "Tolerates the defined number of critical positions before deadlocked will flagged as true.", _control.Creature.Move.Deadlock.MoveMaxCriticalPositions, 1, 0, 100, 10, Info.DEADLOCK_MOVE_CRITICAL_POSITION );
				EditorGUI.indentLevel--;

				_control.Creature.Move.Deadlock.LoopRange = ICEEditorLayout.DefaultSlider( "Loop Range Check", "Expected distance the creature should have covered until the defined interval", _control.Creature.Move.Deadlock.LoopRange, 0.01f, 0, 25, 2, Info.DEADLOCK_LOOP_RANGE );
				EditorGUI.indentLevel++;
				_control.Creature.Move.Deadlock.LoopInterval = ICEEditorLayout.DefaultSlider( "Test Interval (sec.)", "Interval until the next test", _control.Creature.Move.Deadlock.LoopInterval, 0.25f, 0, 30, 5, Info.DEADLOCK_LOOP_INTERVAL );
				_control.Creature.Move.Deadlock.LoopMaxCriticalPositions = (int)ICEEditorLayout.DefaultSlider( "Max. Critical Positions", "Tolerates the defined number of critical positions before deadlocked will flagged as true.", _control.Creature.Move.Deadlock.LoopMaxCriticalPositions, 1, 0, 100, 10, Info.DEADLOCK_LOOP_CRITICAL_POSITION );
				EditorGUI.indentLevel--;


				_control.Creature.Move.Deadlock.Action = (DeadlockActionType)ICEEditorLayout.EnumPopup( "Deadlock Action", "", _control.Creature.Move.Deadlock.Action, Info.DEADLOCK_ACTION );
				if( _control.Creature.Move.Deadlock.Action == DeadlockActionType.BEHAVIOUR )
				{
					EditorGUI.indentLevel++;
					_control.Creature.Move.Deadlock.Behaviour = BehaviourEditor.BehaviourSelect( _control, "Deadlock Behaviour","", _control.Creature.Move.Deadlock.Behaviour, "DEADLOCK", Info.DEADLOCK_ACTION_BEHAVIOUR );
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.Separator();
				EditorGUI.indentLevel--;
			}
			// DEADLOCK END
		}

		private static void DrawEssentialsMotionDefaultMove( ICECreatureControl _control )
		{
			EditorGUILayout.LabelField( "Default Move" );
			EditorGUI.indentLevel++;
			_control.Creature.Move.DefaultMove = CreatureEditorLayout.DrawMove( _control.Creature.Move.DefaultMove, Info.MOVE_DEFAULT  );
			EditorGUI.indentLevel--;
		}


	}
}
