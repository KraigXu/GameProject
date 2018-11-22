// ##############################################################################
//
// ice_CreatureEssentials.cs
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

using ICE.World;
using ICE.World.Objects;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class EssentialsDataObject : ICEObject 
	{
		public EssentialsDataObject(){}
		public EssentialsDataObject( EssentialsObject _essentials, MoveObject _move ){

			Essentials = _essentials;
			Move = _move;
		}

		[SerializeField]
		private EssentialsObject m_Essentials = null;
		public EssentialsObject Essentials{
			get{ return m_Essentials = ( m_Essentials == null ?new EssentialsObject():m_Essentials );  }
			set{ Essentials.Copy( value ); }
		}

		[SerializeField]
		private MoveObject m_Move = null;
		public MoveObject Move{
			get{ return m_Move = ( m_Move == null ?new MoveObject():m_Move ); }
			set{ Move.Copy( value ); }
		}
	}

	[System.Serializable]
	public class EssentialsObject : ICEOwnerObject 
	{
		public EssentialsObject(){}
		public EssentialsObject( EssentialsObject _object ) : base( _object ){ Copy( _object ); }
		public EssentialsObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( EssentialsObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			DefaultRunningSpeed = _object.DefaultRunningSpeed;
			DefaultWalkingSpeed = _object.DefaultWalkingSpeed;
			DefaultTurningSpeed = _object.DefaultTurningSpeed;

			IgnoreAnimationRun = _object.IgnoreAnimationRun;
			AnimationRun.Copy( _object.AnimationRun );

			IgnoreAnimationWalk = _object.IgnoreAnimationWalk;
			AnimationWalk.Copy( _object.AnimationWalk );

			IgnoreAnimationIdle = _object.IgnoreAnimationIdle;
			AnimationIdle.Copy( _object.AnimationIdle );

			IgnoreAnimationJump = _object.IgnoreAnimationJump;
			AnimationJump.Copy( _object.AnimationJump );

			IgnoreAnimationFall = _object.IgnoreAnimationFall;
			AnimationFall.Copy( _object.AnimationFall );

			IgnoreAnimationCrawlMove = _object.IgnoreAnimationCrawlMove;
			AnimationCrawlMove.Copy( _object.AnimationCrawlMove );

			IgnoreAnimationCrawlIdle = _object.IgnoreAnimationCrawlIdle;
			AnimationCrawlIdle.Copy( _object.AnimationCrawlIdle );

			IgnoreAnimationCrouchMove = _object.IgnoreAnimationCrouchMove;
			AnimationCrouchMove.Copy( _object.AnimationCrouchMove );

			IgnoreAnimationCrouchIdle = _object.IgnoreAnimationCrouchIdle;
			AnimationCrouchIdle.Copy( _object.AnimationCrouchIdle );

			IgnoreAnimationDead = _object.IgnoreAnimationDead;
			AnimationDead.Copy( AnimationDead );

			IgnoreAnimationAttack = _object.IgnoreAnimationAttack;
			AnimationAttack.Copy( _object.AnimationAttack );

			IgnoreAnimationImpact = _object.IgnoreAnimationImpact;
			AnimationImpact.Copy( _object.AnimationImpact );

			//MotionControl = _object.MotionControl;
			GroundOrientation = _object.GroundOrientation;

			TrophicLevel = _object.TrophicLevel;
			IsCannibal = _object.IsCannibal;

			UseAutoDetectInteractors = _object.UseAutoDetectInteractors;

			Target.Copy( _object.Target );

			BehaviourFoldout = _object.BehaviourFoldout;
			BehaviourModeTravel = _object.BehaviourModeTravel;
			BehaviourModeLeisure = _object.BehaviourModeLeisure;
			BehaviourModeRendezvous = _object.BehaviourModeRendezvous;

			BehaviourModeRun = _object.BehaviourModeRun;
			BehaviourModeIdle  = _object.BehaviourModeIdle;
			BehaviourModeWalk  = _object.BehaviourModeWalk;

			BehaviourModeSpawn  = _object.BehaviourModeSpawn;
			BehaviourModeSpawnEnabled = _object.BehaviourModeSpawnEnabled;
			BehaviourModeDead  = _object.BehaviourModeDead;
			BehaviourModeDeadEnabled = _object.BehaviourModeDeadEnabled;
			BehaviourModeWait  = _object.BehaviourModeWait;
			BehaviourModeWaitEnabled = _object.BehaviourModeWaitEnabled;
			BehaviourModeMounted  = _object.BehaviourModeMounted;
			BehaviourModeMountedEnabled = _object.BehaviourModeMountedEnabled;



			BehaviourModeJump  = _object.BehaviourModeJump;
			BehaviourModeJumpEnabled = _object.BehaviourModeJumpEnabled;
			BehaviourModeFall  = _object.BehaviourModeFall;
			BehaviourModeFallEnabled = _object.BehaviourModeFallEnabled;
			BehaviourModeSlide  = _object.BehaviourModeSlide;
			BehaviourModeSlideEnabled = _object.BehaviourModeSlideEnabled;
			BehaviourModeVault  = _object.BehaviourModeVault;
			BehaviourModeVaultEnabled = _object.BehaviourModeVaultEnabled;
			BehaviourModeClimb  = _object.BehaviourModeClimb;
			BehaviourModeClimbEnabled = _object.BehaviourModeClimbEnabled;

		}

		public bool BehaviourFoldout = true;
		public string BehaviourModeTravel = "";
		public string BehaviourModeLeisure = "";
		public string BehaviourModeRendezvous = "";

		public string BehaviourModeRun = "";
		public string BehaviourModeIdle = "";
		public string BehaviourModeWalk = "";

		public string BehaviourModeSpawn = "";
		public bool BehaviourModeSpawnEnabled = false;
		public string BehaviourModeRepose = "";
		public bool BehaviourModeReposeEnabled = false;
		//public string BehaviourModeWounded = "";
		//public bool BehaviourModeWoundedEnabled = false;
		public string BehaviourModeImpact = "";
		public bool BehaviourModeImpactEnabled = false;
		public string BehaviourModeDead = "";
		public bool BehaviourModeDeadEnabled = false;
		public string BehaviourModeWait = "";
		public bool BehaviourModeWaitEnabled = false;
		public string BehaviourModeMounted = "";
		public bool BehaviourModeMountedEnabled = false;

		public string BehaviourModeJump = "";
		public bool BehaviourModeJumpEnabled = false;
		public string BehaviourModeFall = "";
		public bool BehaviourModeFallEnabled = false;
		public string BehaviourModeSlide = "";
		public bool BehaviourModeSlideEnabled = false;
		public string BehaviourModeVault = "";
		public bool BehaviourModeVaultEnabled = false;
		public string BehaviourModeClimb = "";
		public bool BehaviourModeClimbEnabled = false;
		public string BehaviourModeClimbDown = "";
		public bool BehaviourModeClimbDownEnabled = false;


		public float DefaultRunningSpeed = 7;
		public float DefaultWalkingSpeed = 3;
		public float DefaultTurningSpeed = 4;

		public bool IgnoreAnimationRun = false;
		public AnimationDataObject AnimationRun = new AnimationDataObject();

		public bool IgnoreAnimationWalk = false;
		public AnimationDataObject AnimationWalk = new AnimationDataObject();

		public bool IgnoreAnimationIdle = false;
		public AnimationDataObject AnimationIdle = new AnimationDataObject();

		public bool IgnoreAnimationJump = true;
		public AnimationDataObject AnimationJump = new AnimationDataObject();

		public bool IgnoreAnimationFall = true;
		public AnimationDataObject AnimationFall = new AnimationDataObject();

		public bool IgnoreAnimationCrawlMove = true;
		public AnimationDataObject AnimationCrawlMove = new AnimationDataObject();

		public bool IgnoreAnimationCrawlIdle = true;
		public AnimationDataObject AnimationCrawlIdle = new AnimationDataObject();

		public bool IgnoreAnimationCrouchMove = true;
		public AnimationDataObject AnimationCrouchMove = new AnimationDataObject();

		public bool IgnoreAnimationCrouchIdle = true;
		public AnimationDataObject AnimationCrouchIdle = new AnimationDataObject();

		public bool IgnoreAnimationDead = true;
		public AnimationDataObject AnimationDead = new AnimationDataObject();

		public bool IgnoreAnimationAttack = true;
		public AnimationDataObject AnimationAttack = new AnimationDataObject();

		public bool IgnoreAnimationImpact = true;
		public AnimationDataObject AnimationImpact = new AnimationDataObject();

		//public MotionControlType MotionControl = MotionControlType.INTERNAL;
		public BodyOrientationType GroundOrientation = BodyOrientationType.BIPED;

		public TrophicLevelType TrophicLevel = TrophicLevelType.UNDEFINED;
		public bool IsCannibal = false;

		public bool UseAutoDetectInteractors = false;

		[SerializeField]
		private TargetObject m_Target = null;
		public virtual TargetObject Target {
			get{ return m_Target = ( m_Target == null ? new TargetObject( TargetType.HOME ):m_Target ); }
			set{ Target.Copy( value ); }
		}

		public bool TargetReady(){
			return ( Target.IsValidAndReady ? true : false );
		}

		public TargetObject PrepareTarget( ICEWorldBehaviour _component )
		{
			if( ! OwnerIsReady( _component ) || Target.PrepareTargetGameObject( _component ) == null || ! Target.IsValidAndReady )
				return null;

			Target.Behaviour.SetDefault();

			// if the creature outside the max range it have to travel to reach its target
			if( ! Target.TargetInMaxRange( Owner.transform.position ) )
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeTravel;

			// if the creature reached the TargetMovePosition it should do the rendezvous behaviour
			else if( Target.TargetMovePositionReached( Owner ) )
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeRendezvous;

			// in all other case the creature should be standby and do some leisure activities
			else 
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeLeisure;

			return Target;
		}
	}
}
