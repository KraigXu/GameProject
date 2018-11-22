// ##############################################################################
//
// ice_creature_editor_info.cs | InfoEditor
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
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;



using ICE.Creatures.EditorInfos;



namespace ICE.Creatures.EditorUtilities
{
	
	public static class InfoEditor
	{	
		public static void Print( ICECreatureControl _control ){

			if( ! _control.ShowInfo )
				return;

			string _info = "\n"; 

			_info += "Status: \n";

			_info += "\tAge\t\t\t : " + ( _control.Creature.Status.UseAging ? DateTools.FormatTimeDetailed( _control.Creature.Status.Age ) : "INACTIVE" ) + "\n";
			_info += "\tGender\t\t\t : " + _control.Creature.Status.GenderType.ToString() + "\n";
			_info += "\tTrophic Level\t\t : " + _control.Creature.Status.TrophicLevel.ToString() + "\n";
			_info += "\tEnvironment Temperature\t : " + ( _control.Creature.Status.UseEnvironmentTemperature ? _control.Creature.Status.TemperatureDeviationInPercent + "%" : "INACTIVE" ) + "\n";
			_info += "\n";

			if( _control.Creature.Status.UseAdvanced )
			{
				_info += "\tDurability\t\t : " + _control.Creature.Status.Durability + "/" + _control.Creature.Status.InitialDurabilityMaximum + " (" + _control.Creature.Status.DurabilityInPercent + "%) \n";
				_info += "\n";
				_info += "\tFitness\t\t\t : " + _control.Creature.Status.FitnessInPercent + "% \n";
				_info += "\t\tHealth\t\t : " + _control.Creature.Status.HealthInPercent + "% \n";
				_info += "\t\tPower\t\t : " + _control.Creature.Status.PowerInPercent + "% \n";
				_info += "\t\tStamina\t\t : " + _control.Creature.Status.StaminaInPercent + "% \n";
				_info += "\n";
				_info += "\tArmor\t\t\t : " + ( _control.Creature.Status.UseArmor ? _control.Creature.Status.ArmorInPercent + "%" : "INACTIVE" ) + "\n";
				_info += "\tDamage\t\t\t : " + _control.Creature.Status.DamageInPercent + "% \n";
				_info += "\tStress\t\t\t : " + _control.Creature.Status.StressInPercent + "% \n";
				_info += "\tDebility\t\t\t : " + _control.Creature.Status.DebilityInPercent + "% \n";
				_info += "\tHunger\t\t\t : " + _control.Creature.Status.HungerInPercent + "% \n";
				_info += "\tThirst\t\t\t : " + _control.Creature.Status.ThirstInPercent + "% \n";
				_info += "\n";
				_info += "\tAggressivity\t\t : " + _control.Creature.Status.AggressivityInPercent + "% \n";
				_info += "\tExperience\t\t : " + _control.Creature.Status.ExperienceInPercent + "% \n";
				_info += "\tNosiness\t\t\t : " + _control.Creature.Status.NosinessInPercent + "% \n";
				_info += "\tAnxiety\t\t\t : " + _control.Creature.Status.AnxietyInPercent + "% \n";
			}
			else
			{
				_info += "\tDurability\t\t : " + _control.Creature.Status.Durability + "/" + _control.Creature.Status.InitialDurabilityMaximum + " (" + _control.Creature.Status.DurabilityInPercent + "%) \n";
				_info += "\n";
				_info += "\tFitness\t\t\t : " + _control.Creature.Status.FitnessInPercent + "% \n";
				_info += "\tHealth\t\t\t : " + _control.Creature.Status.HealthInPercent + "% \n";
				_info += "\tDamage\t\t\t : " + _control.Creature.Status.DamageInPercent + "% \n";
			}

			_info += "\n";

			_info += "Motion Control: " + _control.Creature.Move.MotionControl.ToString();

			if( _control.Creature.Move.MotionControl == MotionControlType.RIGIDBODY )
			{
				Rigidbody _rb = _control.GetComponent<Rigidbody>();
				if( _rb == null )
					_info += " - MISSING RIGIDBODY";
				else
				{
					_info += "\n";
					_info += "\tRigidbody Kinematic\t : " + _rb.isKinematic.ToString() + "\n";
					_info += "\tRigidbody Gravity   \t : " + _rb.useGravity.ToString() + "\n";
					_info += "\tRigidbody Constraints\t : " + _rb.constraints.ToString() + "\n";
				}
			}

			_info += "\n";
			_info += "\tVelocity\t\t\t : " + _control.Velocity + "\n";
			_info += "\tForward Speed\t\t : " + _control.Creature.Move.MoveSpeed + "\n";
			_info += "\tAngular Speed\t\t : " + System.Math.Round( _control.Creature.Move.MoveAngularSpeed, 2 ) + " (" + System.Math.Round( _control.Creature.Move.MoveAngularSpeedLimited, 2 ) + ")\n";
			_info += "\tVertical Speed\t\t : " + System.Math.Round( _control.Creature.Move.VerticalSpeed, 2 ) + "\n";
			_info += "\tMove Direction\t\t : " + System.Math.Round( _control.Creature.Move.MoveDirectionAngle, 2 ) + "\n";
			_info += "\tMove Distance\t\t : " + System.Math.Round( _control.Creature.Move.TargetMovePositionDistance, 2 ) + " (" + System.Math.Round( _control.Creature.Move.MovePositionDistance, 2 ) + ")\n";
			_info += "\tAltitude\t\t\t : " + System.Math.Round( _control.Creature.Move.Altitude, 2 ) + " (" + System.Math.Round( _control.Creature.Move.AbsoluteAltitude, 2 ) + ")\n";

			//+ " Behaviour Velocity : " + _control.Creature.Move.CurrentMove.Motion.Velocity.ToString() + "/" + _control.Creature.Move.CurrentMove.Motion.AngularVelocity.y + "\n"; 


			Animator _animator = _control.GetComponent<Animator>();
			if( _animator != null )
			{
				_info += "\n";
				_info += "Animator\n";
				_info += "\tController\t : " + ( _animator.runtimeAnimatorController == null ? "missing" : _animator.runtimeAnimatorController.name ) + "\n";
				_info += "\tRootMotion\t : " + _animator.applyRootMotion.ToString() + "\n";
			}

			_info += "\n";

			_info += "Targets : " + GetTargetsCount( _control ) + " (currently available: " + _control.Creature.AvailableTargets.Count + ")\n";
			_info += "\tActive Target\t : '" + _control.Creature.ActiveTargetName + "' (" + _control.Creature.ActiveTargetID + ")\n";
			_info += "\t\tRuntime\t : " + System.Math.Round( _control.Creature.ActiveTargetActiveTime, 3 ) + " secs. (total : " + System.Math.Round( _control.Creature.ActiveTargetActiveTimeTotal, 3 ) + " secs.)\n";
			_info += "\t\tSpeed\t : " +  System.Math.Round( _control.Creature.ActiveTargetSpeed, 3 ) + "\n";
			_info += "\t\tVelocity\t : " + _control.Creature.ActiveTargetVelocity + "\n";
			_info += "\t\tDistance\t : " +  System.Math.Round( _control.Creature.ActiveTargetDistance, 3 ) + "\n";
			_info += "\t\tDirection\t : " + _control.Creature.ActiveTargetDirection + "\n";
			_info += "\t\tPosition\t : " + _control.Creature.ActiveTargetTransformPosition + "\n";
			_info += "\n";
			_info += "\tPrevious Target : '" + _control.Creature.PreviousTargetID + "' (" + _control.Creature.PreviousTargetName + ")\n\n";

			_info += "Behaviours : " + _control.Creature.Behaviour.BehaviourModes.Count + " Modes with " + GetBehaviorModeRulesCount( _control ) + " Rules \n";
			_info += "\tActive Mode\t : '" + _control.Creature.Behaviour.ActiveBehaviourModeKey + "'\n";
			_info += "\t\tRuntime\t : " + System.Math.Round( _control.Creature.Behaviour.BehaviourTimer, 3 ) + " secs.\n";
			_info += "\tPrevious Mode\t : '" + _control.Creature.Behaviour.LastBehaviourModeKey + "'\n\n";

			BehaviourModeRuleObject _rule = _control.Creature.Behaviour.ActiveBehaviourModeRule;

			if( _rule != null )
			{
				_info += "Animation Name : " + _rule.Animation.GetAnimationName() + " (" +_rule.Animation.GetAnimationLength()+ " secs.)\n\n";
			}
			else
			{
			}
				
			_info += "Current Move: " + _control.Creature.Move.CurrentMove.Enabled.ToString().ToUpper() + " type: " + _control.Creature.Move.CurrentMove.Type.ToString() + "\n";
			_info += "\tStopping Distance: " + _control.Creature.Move.CurrentMove.StoppingDistance + " (default: " + _control.Creature.Move.DefaultMove.StoppingDistance + ")\n";
			_info += "\t\tIgnore Level Difference: " + _control.Creature.Move.CurrentMove.IgnoreLevelDifference.ToString().ToUpper() + " (default: " + _control.Creature.Move.DefaultMove.IgnoreLevelDifference.ToString().ToUpper() + ")\n";

			_info += "\tSegment Length: " + _control.Creature.Move.CurrentMove.SegmentLength + " (default: " + _control.Creature.Move.DefaultMove.SegmentLength + ")\n";
			_info += "\t\tSegment Variance: " + _control.Creature.Move.CurrentMove.SegmentVariance + " (default: " + _control.Creature.Move.DefaultMove.SegmentVariance + ")\n";
			_info += "\tDeviation Length: " + _control.Creature.Move.CurrentMove.DeviationLength + " (default: " + _control.Creature.Move.DefaultMove.DeviationLength + ")\n";
			_info += "\t\tDeviation Variance: " + _control.Creature.Move.CurrentMove.DeviationVariance + " (default: " + _control.Creature.Move.DefaultMove.DeviationVariance + ")\n\n";

		

			if( _control.Creature.Move.Deadlock.Enabled )
			{
				_info += "Deadlocked: " + (_control.Creature.Move.Deadlock.Deadlocked?"TRUE":"FALSE") + " (distance: " + _control.Creature.Move.Deadlock.DeadlocksDistance + " time: " + _control.Creature.Move.Deadlock.DeadlockMoveTimer + "/" + _control.Creature.Move.Deadlock.DeadlockLoopTimer + " secs.)\n";
				_info += "  deadlocks: " + _control.Creature.Move.Deadlock.DeadlocksCount + " - critical positions: " + _control.Creature.Move.Deadlock.DeadlocksCriticalPositions;
				_info += "  loops: " + _control.Creature.Move.Deadlock.DeadlockLoopsCount + " - critical loops: " + _control.Creature.Move.Deadlock.DeadlocksCriticalLoops;
			}
			else
				_info += "Deadlock Handling: deactivated";

			_info += "\n";
			
			//_info += "Active Behaviour Rule : '" + _control.Creature.Behaviour.c + "'";

			_info += "Active Counterparts: " + _control.ActiveCounterparts.Count + "\n";

			foreach( ICECreatureEntity _entity in _control.ActiveCounterparts )
				_info += "\t - " + _entity.name + " (" + _entity.ObjectInstanceID + ")\n";
				

		
			Info.Note( _info );
		}

		private static int GetBehaviorModeRulesCount( ICECreatureControl _control )
		{
			int _i = 0;
			foreach( BehaviourModeObject _mode in _control.Creature.Behaviour.BehaviourModes )
				_i += _mode.Rules.Count;
			return _i;
		}

		private static int GetTargetsCount( ICECreatureControl _control )
		{
			int _i = 0;

			if( _control.Creature.Essentials.TargetReady() )
				_i++;
			if( _control.Creature.Missions.Outpost.TargetReady() )
				_i++;
			if( _control.Creature.Missions.Escort.TargetReady() )
				_i++;
			if( _control.Creature.Missions.Patrol.TargetReady() )
				_i += _control.Creature.Missions.Patrol.Waypoints.GetEnabledWaypoints().Count;

				_i += _control.Creature.Interaction.GetValidInteractors().Count;

			return _i;
		}
	}
}