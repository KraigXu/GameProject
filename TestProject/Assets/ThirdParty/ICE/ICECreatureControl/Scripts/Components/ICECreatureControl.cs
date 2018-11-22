// ##############################################################################
//
// ICECreatureControl.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures
{

	/// <summary>
	/// ICE creature control.
	/// </summary>
	/// <description>You can use this class for your own code and|or settings, but please consider,
	/// that this class and also your code will be overwritten by each update/upgrade of the creature package, 
	/// so please save your work whenever you reimport this package and copied it back if the update is done.</description>
	//[AddComponentMenu("ICECreatureControl")]
	public class ICECreatureControl : ICECreatureCharacter 
	{
		#region Creature Status Values

		public string BehaviourModeKey{ 
			get{ return Creature.Behaviour.ActiveBehaviourModeKey; } 
			set{ Creature.SetActiveBehaviourModeByKey( value ); }
		}

		public float StatusDamageInPercent{ 
			get{ return Creature.Status.DamageInPercent; } 
			set{ Creature.Status.DamageInPercent = value; } }
		public float StatusStressInPercent{ 
			get{ return Creature.Status.StressInPercent; } 
			set{ Creature.Status.StressInPercent = value; } }
		public float StatusDebilityInPercent{ 
			get{ return Creature.Status.DebilityInPercent; } 
			set{ Creature.Status.DebilityInPercent = value; } }
		public float StatusThirstInPercent{ 
			get{ return Creature.Status.ThirstInPercent; } 
			set{ Creature.Status.ThirstInPercent = value; } }
		public float StatusHungerInPercent{ 
			get{ return Creature.Status.HungerInPercent; } 
			set{ Creature.Status.HungerInPercent = value; } }

		public float StatusAddDamage{ set{ Creature.Status.AddDamage( value ); } }
		public float StatusAddStress{ set{ Creature.Status.AddStress( value ); } }
		public float StatusAddDebility{ set{ Creature.Status.AddDebility( value ); } }
		public float StatusAddHunger{ set{ Creature.Status.AddHunger( value ); } }
		public float StatusAddThirst{ set{ Creature.Status.AddThirst( value ); } }

		// character
		public float StatusAggressivityInPercent{ get{ return Creature.Status.AggressivityInPercent; } }
		public float StatusAnxietyInPercent{ get{ return Creature.Status.AnxietyInPercent; } }
		public float StatusExperienceInPercent{ get{ return Creature.Status.ExperienceInPercent; } }
		public float StatusNosinessInPercent{ get{ return Creature.Status.NosinessInPercent; } }

		public float StatusAddAggressivity{ set{ Creature.Status.AddAggressivity( value ); } }
		public float StatusAddAnxiety{ set{ Creature.Status.AddAnxiety( value ); } }
		public float StatusAddExperience{ set{ Creature.Status.AddExperience( value ); } }
		public float StatusAddNosiness{	set{ Creature.Status.AddNosiness( value ); } }

		/// <summary>
		/// Gets the age or 0 if aging is not active
		/// </summary>
		/// <value>The age.</value>
		public override float Age{	
			get{ return Creature.Status.Age; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is destroyed.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public override bool IsDestroyed{	
			get{ return Creature.Status.IsDestroyed; }
		}

		[System.Obsolete ("Use Age instead")]
		public float StatusAge{	
			set{ Creature.Status.SetAge( value ); }
			get{ return Creature.Status.Age; }
		}

		public float StatusLifespanInPercent{ get{ return Creature.Status.LifespanInPercent; } }

		// Vital
		public float StatusFitnessInPercent{ get{ return Creature.Status.FitnessInPercent; } }
		public float StatusArmorInPercent{ get{ return Creature.Status.ArmorInPercent; } }
		public float StatusHealthInPercent{	get{ return Creature.Status.HealthInPercent; } }
		public float StatusStaminaInPercent{ get{ return Creature.Status.StaminaInPercent; } }
		public float StatusPowerInPercent{ get{ return Creature.Status.PowerInPercent; } }

		public GameObject ActiveTargetGameObject{
			get{ return ( Creature.ActiveTarget != null ? Creature.ActiveTarget.TargetGameObject:null ); } 
		}

		public GameObject LastTargetGameObject{
			get{ return ( Creature.PreviousTarget != null ? Creature.PreviousTarget.TargetGameObject:null ); } 
		}

		/*
		public float MovePositionAngle{ get{ return Creature.Move.MovePositionAngle; } }
		public float MoveForwardVelocity{ get{ return Creature.Move.MoveVelocity.z; } }
		public float MoveAngularVelocity{
			get{ return Creature.Move.CurrentAngularVelocity.y; }// BehaviourAngularVelocity * PositionTools.AngleDirectionExt( transform.forward, Vector3.up, Creature.Move.MovePositionDelta ); }
		}

		public float MovePositionDeltaForward{
			get{ return Creature.Move.MovePositionDelta.z; }
		}

		public float MoveRotationDeltaVertical{ get{ return Creature.Move.MoveRotationDelta.y; } }
		public float MoveAltitude{ get{ return Creature.Move.Altitude; } }
		public float MoveAbsoluteAltitude{ get{ return Creature.Move.AbsoluteAltitude; } }
		public float MoveFallSpeed{ get{ return Creature.Move.FallVelocityMax; } }

		public float BehaviourForwardVelocity{
			get{ 
				if( Creature.Behaviour.ActiveBehaviourMode != null && Creature.Behaviour.ActiveBehaviourMode.Rule != null )
					return Creature.Behaviour.ActiveBehaviourMode.Rule.Move.Velocity.Velocity.z;
				else
					return 0;
			}
		}

		public float BehaviourAngularVelocity{
			get{ 
				if( Creature.Behaviour.ActiveBehaviourMode != null && Creature.Behaviour.ActiveBehaviourMode.Rule != null )
					return Creature.Behaviour.ActiveBehaviourMode.Rule.Move.Velocity.Angular.y;
				else
					return 0;
			}
		}*/

		public override bool GetDynamicBooleanValue( DynamicBooleanValueType _type )
		{
			switch( _type )
			{
			case DynamicBooleanValueType.IsGrounded:
				return Creature.Move.IsGrounded;
			case DynamicBooleanValueType.Deadlocked:
				return Creature.Move.Deadlock.Deadlocked;
			case DynamicBooleanValueType.MovePositionReached:
				return Creature.Move.MovePositionReached;
			case DynamicBooleanValueType.TargetMovePositionReached:
				return Creature.Move.TargetMovePositionReached;
			case DynamicBooleanValueType.MovePositionUpdateRequired:
				return Creature.Move.MovePositionUpdateRequired;
			case DynamicBooleanValueType.IsJumping:
				return Creature.Move.IsJumping;
			case DynamicBooleanValueType.IsSliding:
				return Creature.Move.IsCrossBelowRequired;
			case DynamicBooleanValueType.IsVaulting:
				return Creature.Move.IsCrossOverRequired;

			}

			return false;
		}

		public override int GetDynamicIntegerValue( DynamicIntegerValueType _type )
		{/*
			switch( _type )
			{

			case DynamicIntegerValueType.CreatureForwardSpeed:
				return MoveForwardVelocity;
			case DynamicIntegerValueType.CreatureAngularSpeed:
				return MoveAngularVelocity;
			case DynamicIntegerValueType.CreatureDirection:
				return MoveDirection;
			}*/

			return 0;
		}

		public override float GetDynamicFloatValue( DynamicFloatValueType _type )
		{
			switch( _type )
			{
			case DynamicFloatValueType.MoveForwardSpeed:
				return Creature.Move.MoveSpeed;
			case DynamicFloatValueType.MoveAngularSpeed:
				return Creature.Move.MoveAngularSpeed;
			case DynamicFloatValueType.MoveAngularSpeedLimited:
				return Creature.Move.MoveAngularSpeedLimited;
			case DynamicFloatValueType.MoveDirection:
				return Creature.Move.MoveDirectionAngle;


			case DynamicFloatValueType.FallSpeed:
				return Creature.Move.FinalMoveVelocity.y;

			case DynamicFloatValueType.Altitude:
				return Creature.Move.Altitude;
			case DynamicFloatValueType.AbsoluteAltitude:
				return Creature.Move.AbsoluteAltitude;

			case DynamicFloatValueType.MovePositionDistance:
				return Creature.Move.MovePositionDistance;
			case DynamicFloatValueType.TargetMovePositionDistance:
				return Creature.Move.TargetMovePositionDistance;
			}

			return 0;
		}

		#endregion


		#region Abstract Methods

		/// <summary>
		/// Update begins.
		/// </summary>
		/// <description>This is the first call of a new update cycle. You could use this abstract method to modify the status of your creature.</description>
		/// <example>
		/// Action.Status.AddDamage( 10 ); // increased the damage of your creature (value in percent) ...
		/// Action.Status.AddDamage( -10 ); // reduced the damage of your creature (value in percent) ...
		/// Action.Status.Temperatur = 25 // adapt the current environmental temperature if required (value in celsius or fahrenheit)...
		/// Action.Status.SetTimeInSeconds( 100 ); coming soon 
		/// Action.Status.SetDateSeconds( 100 ); coming soon
		/// Action.Status.Weather = WeatherType.RAIN; coming soon
		/// Action.Status. ... check the Status member to see all parameter
		/// </example>
		public override void OnUpdateBegin()
		{
			// Action.Status.AddDamage( 10 ); // increased the damage of your creature (value in percent) ...
			// Action.Status.AddDamage( -10 ); // reduced the damage of your creature (value in percent) ...
			// Action.Status.Temperatur = 25 // adapt the current environmental temperature if required (value in celsius or fahrenheit)...
			// Action.Status.SetTimeInSeconds( 100 ); coming soon 
			// Action.Status.SetDateSeconds( 100 ); coming soon
			// Action.Status.Weather = WeatherType.RAIN; coming soon
			// Action.Status. ... check the Status member to see all parameter
			//Debug.Log ("UpdateBegin");
		}

		/// <summary>
		/// Handles all sensory perception of your creature.
		/// </summary>
		/// <description>CAUTION: USE ONLY IF YOU WANT TO HANDLE ALL SENSE ACTIVITIES BY YOURSELF</description>
		/*
		public override void Sense() // CAUTION: USE ONLY IF YOU WANT TO HANDLE ALL SENSE ACTIVITIES BY YOURSELF
		{
			// that's a short delay to slowing the sensory perception and it's also advantageous for the performance ... :) 
			if( ! Action.Status.IsSenseTime() )
				return;
					
			Debug.Log ("Sense");
		}
		 */

		/// <summary>
		/// Handles all reactions and behaviours of your creature.
		/// </summary>
		/// <description>CAUTION: USE ONLY IF YOU WANT TO HANDLE ALL REACT ACTIVITIES BY YOURSELF</description>
		/*
		public override void React() // CAUTION: USE ONLY IF YOU WANT TO HANDLE ALL REACTIONS BY YOURSELF
		{
			Debug.Log ("React");
		}
		*/

		/// <summary>
		/// Perception complete.
		/// </summary>
		/// <description>NOTE: SenseComplete() is called at the end of the React method, so please consider the delay due to the PerceptionTime.</description>
		public override void OnSenseComplete()
		{
			//Action.Behaviour.SetBehaviourModeByKey( "YOUR_BEHAVIOUR_KEY" );
			//Action.UpdateBehaviour( your_own_target_object );
		}

		public override void OnReactComplete()
		{
			//Action.Behaviour.SetBehaviourModeByKey( "YOUR_BEHAVIOUR_KEY" );
			//Action.UpdateBehaviour( your_own_target_object );
			//Debug.Log ("ReactComplete");
		}

		/// <summary>
		/// Handles all movements of your creature.
		/// </summary>
		/// <description>This virtual method should be overridden only if you want to handle all movements by yourself, otherwise 
		/// use the abstract method MoveComplete to correct the movements.</description>
		/*
		public override void Move() // CAUTION: USE ONLY IF YOU WANT TO HANDLE ALL MOVEMENTS BY YOURSELF
		{
			Debug.Log ("Move");
		}
		*/


		/// <summary>
		/// Update complete.
		/// </summary>
		/// <description>This is the last call of the update cycle. You could use this virtual method to correct all values before drawing the scene </description>
		public override void OnUpdateComplete()
		{
			
			/*
			if( Creature.TargetChanged )
				Debug.Log( "TARGET INFO : '" + gameObject.name.ToUpper() + "' CHANGED TARGET '" + Creature.PreviousTargetName + "' TO '" + Creature.ActiveTargetName + "'!");
		
			
			if( Creature.Behaviour.BehaviourModeChanged )
				Debug.Log( "BEHAVIOUR INFO : '" + gameObject.name.ToUpper() + "' CHANGED BEHAVIOURMODE '" + Creature.Behaviour.LastBehaviourModeKey + "' TO '" + Creature.Behaviour.BehaviourModeKey + "'!");
			
			if( Creature.Behaviour.BehaviourModeRulesChanged )
				Debug.Log( "BEHAVIOUR INFO : '" + gameObject.name.ToUpper() + "' PREPARES " + Creature.Behaviour.BehaviourMode.Rules.Count +  " RULES FOR '" + Creature.Behaviour.BehaviourModeKey + "'!");
			
			
			if( Creature.Behaviour.BehaviourModeRuleChanged )
				Debug.Log( "BEHAVIOUR INFO : '" + gameObject.name.ToUpper() + "' SELECT 'RULE " + (int)(Creature.Behaviour.BehaviourMode.RuleIndex + 1 )+ "' OF '" + Creature.Behaviour.BehaviourModeKey + "'!");
		
	*/
		}

		#endregion
	}
}


