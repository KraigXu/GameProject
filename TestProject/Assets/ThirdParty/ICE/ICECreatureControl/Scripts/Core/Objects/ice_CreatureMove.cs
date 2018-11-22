// ##############################################################################
//
// ice_CreatureMove.cs
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
using System.Xml;
using System.Xml.Serialization;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{

	[System.Serializable]
	public class LocomotionDataObject : ICEOwnerObject
	{
		public LocomotionDataObject(){}
		public LocomotionDataObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public LocomotionDataObject( LocomotionDataObject _object ) : base( _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );	

			OverlapPrevention.Init( _component );
			ObstacleAvoidance.Init( _component );
		}

		public void Copy( LocomotionDataObject _object )
		{
			if( _object == null )
				return;
			
			base.Copy( _object );	

			DefaultBody.Copy( _object.DefaultBody );
			CurrentBody.Copy( _object.CurrentBody );
			DefaultMove.Copy( _object.DefaultMove );
			CurrentMove.Copy( _object.CurrentMove );

			Deadlock.Copy( _object.Deadlock );
			OverlapPrevention.Copy( _object.OverlapPrevention );
			GroundAvoidance.Copy( _object.GroundAvoidance );
			ObstacleAvoidance.Copy( _object.ObstacleAvoidance );

			MotionControl = _object.MotionControl;

			UseInternalGravity = _object.UseInternalGravity;
			UseWorldGravity = _object.UseWorldGravity;

			m_Gravity = _object.Gravity;

			FallVelocityMax = _object.FallVelocityMax;
			FallVelocityMaximum = _object.FallVelocityMaximum;

			GroundCheck = _object.GroundCheck;
			WaterCheck = _object.WaterCheck;
			ObstacleCheck = _object.ObstacleCheck;

			GroundLayer.Copy( _object.GroundLayer );
			WaterLayer.Copy( _object.WaterLayer );
			ObstacleLayer.Copy( _object.ObstacleLayer );

			CustomGroundLevel = _object.CustomGroundLevel;
			GroundLevelVariance = _object.GroundLevelVariance;

			VerticalRaycastOffset = _object.VerticalRaycastOffset;
			VerticalRaycastOffsetMaximum = _object.VerticalRaycastOffsetMaximum;

			UseSamplePosition = _object.UseSamplePosition;
			SamplePositionRange = _object.SamplePositionRange;

		}

		// SETTINGS

		public BodyDataObject DefaultBody = new BodyDataObject();
		public MoveDataObject DefaultMove = new MoveDataObject();

		private BodyDataObject m_CurrentBody = null;
		public BodyDataObject CurrentBody{
			get{ return m_CurrentBody = ( m_CurrentBody == null ? new BodyDataObject() : m_CurrentBody ); }
		}

		private CurrentMoveDataObject m_CurrentMove = null;
		public CurrentMoveDataObject CurrentMove{
			get{ return m_CurrentMove = ( m_CurrentMove == null ? new CurrentMoveDataObject() : m_CurrentMove ); }
		}

		[SerializeField]
		private MoveDeadlockObject m_Deadlock = null;
		public MoveDeadlockObject Deadlock{
			get{ return m_Deadlock = ( m_Deadlock == null ? new MoveDeadlockObject() : m_Deadlock ); }
			set{ Deadlock.Copy( value ); }
		}

		[SerializeField]
		private OverlapPreventionObject m_OverlapPrevention = null;
		public OverlapPreventionObject OverlapPrevention{
			get{ return m_OverlapPrevention = ( m_OverlapPrevention == null ? new OverlapPreventionObject() : m_OverlapPrevention ); }
			set{ OverlapPrevention.Copy( value ); }
		}

		[SerializeField]
		private GroundAvoidanceObject m_GroundAvoidance = null;
		public GroundAvoidanceObject GroundAvoidance{
			get{ return m_GroundAvoidance = ( m_GroundAvoidance == null ? new GroundAvoidanceObject() : m_GroundAvoidance ); }
			set{ GroundAvoidance.Copy( value ); }
		}

		[SerializeField]
		private ObstacleAvoidanceObject m_ObstacleAvoidance = null;
		public ObstacleAvoidanceObject ObstacleAvoidance{
			get{ return m_ObstacleAvoidance = ( m_ObstacleAvoidance == null ? new ObstacleAvoidanceObject() : m_ObstacleAvoidance ); }
			set{ ObstacleAvoidance.Copy( value ); }
		}


			
		public MotionControlType MotionControl = MotionControlType.INTERNAL;

		public bool UseSamplePosition = false;
		public float SamplePositionRange = 5.0f;
		public bool UseInternalGravity = true;
		public bool UseWorldGravity = true;

		public bool IgnoreRootMotion = true;


		// SETTINGS GRAVITY

		[SerializeField]
		private float m_Gravity = 9.8f;
		public float Gravity{
			set{ m_Gravity = value; }
			get{ return ( UseWorldGravity ? ( Physics.gravity.y * -1 ) : m_Gravity ); }
		}
			
		public float FallVelocityMax = 250;
		public float FallVelocityMaximum = 300;
		public float GravityInterpolator = 0.5f;

		// SETTINGS GROUND CHECK

		public float CustomGroundLevel = 0;
		public float GroundLevelVariance = 0.5f;



		public float VerticalRaycastOffset = 0.5f;
		public float VerticalRaycastOffsetMaximum = 50;

		protected float m_AutoHorizontalRaycastOffset = 0;
		protected float m_AutoVerticalRaycastOffset = 0;
		public float AutoVerticalRaycastOffset{
			get{ return VerticalRaycastOffset + m_AutoVerticalRaycastOffset; }
		}


		public GroundCheckType GroundCheck = GroundCheckType.NONE;
		[SerializeField]
		private LayerObject m_GroundLayer = null;
		public LayerObject GroundLayer{
			get{ return m_GroundLayer = ( m_GroundLayer == null ? new LayerObject() : m_GroundLayer ); }
			set{ GroundLayer.Copy( value ); }
		}

		public WaterCheckType WaterCheck = WaterCheckType.DEFAULT;
		[SerializeField]
		private LayerObject m_WaterLayer = null;
		public LayerObject WaterLayer{
			get{ return m_WaterLayer = ( m_WaterLayer == null ? new LayerObject( "Water" ) : m_WaterLayer ); }
			set{ WaterLayer.Copy( value ); }
		}

		public ObstacleCheckType ObstacleCheck = ObstacleCheckType.NONE;
		[SerializeField]
		private LayerObject m_ObstacleLayer = null;
		public LayerObject ObstacleLayer{
			get{ return m_ObstacleLayer = ( m_ObstacleLayer == null ? new LayerObject() : m_ObstacleLayer ); }
			set{ ObstacleLayer.Copy( value ); }
		}

		public LayerMask GroundLayerMask{
			get{ return GroundLayer.Mask; }
		}

		public LayerMask WaterLayerMask{
			get{ return WaterLayer.Mask; }
		}

		public LayerMask ObstacleLayerMask{
			get{ return ObstacleLayer.Mask; }
		}

		private LayerMask m_OverlapPreventionLayerMask = 0;
		public LayerMask OverlapPreventionLayerMask{
			get{ 
				if( m_OverlapPreventionLayerMask == 0 )
				{
					int _mask = Physics.AllLayers;

					_mask ^= Physics.IgnoreRaycastLayer;

					if( GroundCheck == GroundCheckType.RAYCAST && GroundLayer.Layers.Count > 0 )
						_mask ^= GroundLayerMask;

					if( WaterLayerMask.value != 0 )
						_mask ^= WaterLayerMask;

					if( ObstacleCheck != ObstacleCheckType.NONE && ObstacleLayer.Layers.Count > 0 )
						_mask |= ObstacleLayerMask;

					m_OverlapPreventionLayerMask = _mask;
				}
			
				return m_OverlapPreventionLayerMask;
			}
		}
			
	}

	[System.Serializable]
	public class LocomotionInfoObject : LocomotionDataObject
	{
		public LocomotionInfoObject(){}
		public LocomotionInfoObject( LocomotionInfoObject _object ) : base( _object ){}
		public LocomotionInfoObject( ICEWorldBehaviour _component ) : base( _component ){}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );	

			if( Owner == null )
				return;

			m_Animator = Owner.GetComponentInChildren<Animator>();
			m_Animation = Owner.GetComponentInChildren<Animation>();

			m_Rigidbody = Owner.GetComponent<Rigidbody>();
			m_CharacterController = Owner.GetComponent<CharacterController>();
			m_NavMeshAgent = Owner.GetComponent<NavMeshAgent>();
		}


		public Vector3 ClimbingOffset = Vector3.zero;
		public float ClimbingSpeed = 1;
		public float ClimbingSpeedMaximum = 5;
		public float ClimbingDownSpeed = 1;
		public float ClimbingDownSpeedMaximum = 5;

		// AVAILABLE OBJECTS AND COMPONENTS

		private ICECreatureControl m_Controller = null;
		private ICECreatureEntity m_Entity = null;
		private Animator m_Animator = null;
		private Animation m_Animation = null;
		private Rigidbody m_Rigidbody = null;
		private CharacterController m_CharacterController = null;
		private NavMeshAgent m_NavMeshAgent = null;

		[XmlIgnore]
		public ICECreatureControl Controller{
			get{ return m_Controller = ( m_Controller == null ?Owner.GetComponent<ICECreatureControl>():m_Controller ); }
		}
			
		[XmlIgnore]
		public Animator AnimatorComponent{
			get{ return m_Animator = ( m_Animator == null ?Owner.GetComponentInChildren<Animator>():m_Animator ); }
		}
			
		[XmlIgnore]
		public Animation AnimationComponent{
			get{ return m_Animation = ( m_Animation == null ?Owner.GetComponentInChildren<Animation>():m_Animation ); }
		}
						
		[XmlIgnore]
		public ICECreatureEntity EntityComponent{
			get{ return m_Entity = ( m_Entity == null && Owner != null ? Owner.GetComponent<ICECreatureEntity>() : m_Entity ); }
		}
			
		[XmlIgnore]
		public Rigidbody RigidbodyComponent{
			get{ return m_Rigidbody = ( m_Rigidbody == null ? Owner.GetComponent<Rigidbody>() : m_Rigidbody ); }
		}
			
		[XmlIgnore]
		public CharacterController CharacterControllerComponent{
			get{ return m_CharacterController = ( m_CharacterController == null ? Owner.GetComponent<CharacterController>() : m_CharacterController ); }
		}
			
		[XmlIgnore]
		public NavMeshAgent NavMeshAgentComponent{
			get{ return m_NavMeshAgent = ( m_NavMeshAgent == null ? Owner.GetComponent<NavMeshAgent>() : m_NavMeshAgent ); }
		}

		protected CreatureObject Creature{
			get{ return ( Controller != null ? Controller.Creature : null ); }
		}

		//private TargetObject m_PreviousTarget = null;
		protected TargetObject m_CurrentTarget = null;
		[XmlIgnore]
		public TargetObject CurrentTarget{
			get{ return m_CurrentTarget; }
		}

		protected TargetObject m_HomeTarget = null;
		[XmlIgnore]
		public TargetObject HomeTarget{
			get{ return m_HomeTarget; }
		}

		protected void SetCurrentTarget( TargetObject _target )
		{
			if( m_CurrentTarget != _target )
				m_CurrentTarget = _target;
		}

		protected void SetHomeTarget( TargetObject _target )
		{
			if( m_HomeTarget != _target )
				m_HomeTarget = _target;
		}

		//private BehaviourModeRuleObject m_PreviousBehaviourModeRule = null;
		private BehaviourModeRuleObject m_CurrentBehaviourModeRule = null;
		public BehaviourModeRuleObject CurrentBehaviourModeRule{
			get{ return m_CurrentBehaviourModeRule; }
		}

		protected void SetCurrentBehaviourModeRule( BehaviourModeRuleObject _rule )
		{
			if( m_CurrentBehaviourModeRule != _rule )
			{
				m_CurrentBehaviourModeRule = _rule;

				if( m_CurrentBehaviourModeRule != null )
				{
					m_CurrentBehaviourModeRule.Move.Motion.UpdateVelocityMultiplier();

					CurrentBody.Copy( CurrentBehaviourModeRule.Body );
					if( CurrentBehaviourModeRule.Body.Type == BodyOrientationType.DEFAULT )
						CurrentBody.CopyDefault( DefaultBody );

					CurrentMove.Copy( CurrentBehaviourModeRule.Move );
					if( CurrentBehaviourModeRule.Move.Type == MoveType.DEFAULT )
						CurrentMove.CopyDefault( DefaultMove );


					m_RootMotionRequestedByRule = m_CurrentBehaviourModeRule.UseRootMotion;
					m_RuleAnimationEnabled = m_CurrentBehaviourModeRule.Animation.IsValid;
				}
	
				m_MovePosition = Vector3.zero;
			}
		}
			
		protected Terrain m_SurfaceTerrain = null;
		public Terrain SurfaceTerrain{
			get{ return m_SurfaceTerrain; }
		}

		protected TerrainData m_SurfaceTerrainData = null;
		public TerrainData SurfaceTerrainData{
			get{ return m_SurfaceTerrainData; }
		}

		// BASICS

		protected ActionStatusType m_ActionStatus = ActionStatusType.IsUndefined;
		public ActionStatusType ActionStatus{
			get{ return m_ActionStatus;}
		}
			
		public bool IsBlocked{
			get{ return ( OverlapPrevention.IsBlocked ? true : false ); }
		}

		public bool IsJumping{
			get{ return ( m_ActionStatus == ActionStatusType.IsJumping ? true : false );}
		}

		public bool IsStopRequired{
			get{ return ( ObstacleAvoidance.ActionType == ObstacleAvoidanceActionType.Stop ? true : false );}
		}

		public bool IsCrossBelowRequired{
			get{ return ( ObstacleAvoidance.ActionType == ObstacleAvoidanceActionType.CrossBelow ? true : false );}
		}

		public bool IsCrossOverRequired{
			get{ return ( ObstacleAvoidance.ActionType == ObstacleAvoidanceActionType.CrossOver ? true : false );}
		}

		public bool IsClimbing{
			get{ return ( m_ActionStatus == ActionStatusType.IsClimbing ? true : false );}
		}
			
		protected bool m_IsGrounded = true;
		public bool IsGrounded{
			get{ return m_IsGrounded;}
		}

		public bool IsGliding{
			get{ return ( m_ActionStatus == ActionStatusType.IsGliding ? true : false );}
		}

		public bool IsFalling{
			get{ return ( m_ActionStatus == ActionStatusType.IsFalling ? true : false );}
		}

		public bool IsUndefined{
			get{ return ( m_ActionStatus == ActionStatusType.IsUndefined ? true : false );}
		}

		/// <summary>
		/// Gets the base offset.
		/// </summary>
		/// <value>The base offset.</value>
		public float BaseOffset{
			get{ return ( EntityComponent != null ? EntityComponent.BaseOffset : 0 ); }
		}

		protected float m_FallTime = 0; 

		protected bool m_RootMotionRequestedByRule = false;
		public bool RootMotionRequestedByRule{
			get{ return m_RootMotionRequestedByRule; }
		}

		protected bool m_RuleAnimationEnabled = false;
		public bool RuleAnimationEnabled{
			get{ return m_RuleAnimationEnabled; }
		}

		public bool RootMotionAvailable{
			get{ return ( RuleAnimationEnabled && RootMotionRequestedByRule && AnimatorComponent != null && AnimatorComponent.applyRootMotion ? true : false ); }
		}

		public bool MoveHandledByRootMotion{
			get{ return ( ! IgnoreRootMotion && RootMotionAvailable ? true : false ); }
		}


		public bool AllowOutOfArea = true;

		protected bool m_OutOfArea = false;
		public bool OutOfArea{
			get{ return m_OutOfArea; }
		}

		/*public float FallSpeed{
			get{ return m_VerticalSpeed;}
		}*/



		// ALTITUDE 

		protected float m_Altitude = 0;
		public float Altitude{
			get{ return m_Altitude - BaseOffset;}
		}

		protected float m_AbsoluteAltitude = 0;
		public float AbsoluteAltitude{
			get{ return m_AbsoluteAltitude - BaseOffset;}
		}

		public float CurrentOperatingLevel{
			get{ return ( CurrentMove.Altitude.UseAltitudeAboveGround && ! CurrentMove.Altitude.UseTargetLevel ? m_Altitude : m_AbsoluteAltitude ); }
		}

		public float CurrentOperatingLevelDifference{
			get{ 
				if( CurrentMove.Altitude.UseTargetLevel && CurrentTarget != null && CurrentTarget.IsValidAndReady )
					return CurrentTarget.TargetTransformPosition.y + CurrentMove.Altitude.DesiredAltitude - CurrentOperatingLevel;
				else
					return CurrentMove.Altitude.DesiredAltitude - CurrentOperatingLevel;
			}
		}

		protected float m_GroundLevel = 0;
		public float GroundLevel{
			get{ return m_GroundLevel; }
		}
			
		// POSITIONS

		protected Vector3 m_DesiredMovePosition = Vector3.zero;
		public Vector3 DesiredMovePosition{
			get{ return m_DesiredMovePosition; }
		}

		protected Vector3 m_MovePosition = Vector3.zero;
		public Vector3 MovePosition{
			get{ return m_MovePosition; }
		}

		protected Vector3 m_LastMovePosition = Vector3.zero;
		public Vector3 LastMovePosition{
			get{ return m_LastMovePosition; }
		}

		protected Vector3 m_NavMeshCurrentMovePosition = Vector3.zero;
		public Vector3 NavMeshCurrentMovePosition{
			get{ return m_NavMeshCurrentMovePosition; }
		}

		protected Vector3 m_MoveStepPosition = Vector3.zero;
		public Vector3 MoveStepPosition{
			get{ return m_MoveStepPosition; }
		}

		protected Vector3 m_LastMoveStepPosition= Vector3.zero;
		public Vector3 LastMoveStepPosition{
			get{ return m_LastMoveStepPosition; }
		}

		protected Vector3 m_LastTransformPosition = Vector3.zero;
		public Vector3 LastTransformPosition{
			get{ return m_LastTransformPosition; }
		}

		protected Vector3 m_ObstacleAvoidancePosition = Vector3.zero;
		public Vector3 ObstacleAvoidancePosition{
			get{ return ObstacleAvoidance.ObstacleAvoidancePosition; }
		}

		protected Vector3 m_EscapeMovePosition = Vector3.zero;
		public Vector3 EscapeMovePosition {
			get{ return m_EscapeMovePosition;}
		}

		protected Vector3 m_AvoidMovePosition = Vector3.zero;
		public Vector3 AvoidMovePosition {
			get{ return m_AvoidMovePosition;}
		}

		protected Vector3 m_CoverMovePosition = Vector3.zero;
		public Vector3 CoverMovePosition {
			get{ return m_CoverMovePosition;}
		}
			
		/*
		protected Vector3 m_AvoidMovePosition = Vector3.zero;
		public Vector3 AvoidMovePosition{
			get{ return m_AvoidMovePosition; }
		}

		protected Quaternion m_AvoidMoveRotation = Quaternion.identity;
		public Quaternion AvoidMoveRotation{
			get{ return m_AvoidMoveRotation; }
		}*/

		// ROTATIONS

		protected Quaternion m_DesiredMoveRotation = Quaternion.identity;
		public Quaternion DesiredMoveRotation{
			get{ return m_DesiredMoveRotation; }
		}

		protected Quaternion m_MoveRotation = Quaternion.identity;
		public Quaternion MoveRotation{
			get{ return m_MoveRotation; }
		}

		protected Quaternion m_LastMoveRotation = Quaternion.identity;
		public Quaternion LastMoveRotation{
			get{ return m_LastMoveRotation; }
		}

		protected Quaternion m_MoveStepRotation = Quaternion.identity;
		public Quaternion MoveStepRotation{
			get{ return m_MoveStepRotation; }
		}

		protected Quaternion m_LastMoveStepRotation = Quaternion.identity;
		public Quaternion LastMoveStepRotation{
			get{ return m_LastMoveStepRotation; }
		}

		protected Quaternion m_LastTransformRotation = Quaternion.identity;
		public Quaternion LastTransformRotation{
			get{ return m_LastTransformRotation; }
		}

		// ANGLES

		protected float m_TargetRelatedDirectionAngle = 0;
		public float TargetRelatedDirectionAngle {
			get{ return m_TargetRelatedDirectionAngle;}
		}
			
		protected float m_CreatureRelatedDirectionAngle = 0;
		public float CreatureRelatedDirectionAngle {
			get{ return m_CreatureRelatedDirectionAngle;}
		}

		protected float m_EscapeAngle = 0;
		public float EscapeAngle {
			get{ return m_EscapeAngle;}
		}


		protected float m_MoveDirection = 0;
		public float MoveDirectionAngle{
			get{ return m_MoveDirection; }
		}
			
		protected float m_MoveCourseDeviation = 0;
		public float MoveCourseDeviation{
			get{ return m_MoveCourseDeviation; }
		}


		protected float m_MoveSpeed = 0;
		public float MoveSpeed{
			get{ return m_MoveSpeed; }
		}

		protected float m_MoveAngularSpeedRaw = 0;
		public float MoveInterpolatedAngularSpeed{
			get{ return m_MoveAngularSpeedRaw; }
		}

		protected float m_MoveAngularSpeed = 0;
		public float MoveAngularSpeed{
			get{ return m_MoveAngularSpeed; }
		}

		protected float m_MoveAngularSpeedLimited = 0;
		public float MoveAngularSpeedLimited{
			get{ return m_MoveAngularSpeedLimited; }
		}


		protected float m_VerticalSpeed = 0;
		public float VerticalSpeed{
			get{ return m_VerticalSpeed; }
		}


		// DISTANCES

		protected float m_DesiredStoppingDistance = 0;
		public float DesiredStoppingDistance{
			get{ return m_DesiredStoppingDistance; }
		}

		protected bool m_DesiredIgnoreLevelDifference = true;
		public bool DesiredIgnoreLevelDifference{
			get{ return m_DesiredIgnoreLevelDifference; }
		}

		// VELOCITIES

		protected Vector3 m_DesiredVelocity = Vector3.zero;
		public Vector3 DesiredVelocity{
			get{ return m_DesiredVelocity; }
		}

		protected Vector3 m_DesiredAngularVelocity = Vector3.zero;
		public Vector3 DesiredAngularVelocity{
			get{ return m_DesiredAngularVelocity; }
		}

		protected Vector3 m_FinalMoveVelocity = Vector3.zero;
		public Vector3 FinalMoveVelocity{
			get{ return m_FinalMoveVelocity; }
		}

		protected Vector3 m_FinalAngularVelocity = Vector3.zero;
		public Vector3 FinalAngularVelocity{
			get{ return m_FinalAngularVelocity; }
		}

		protected Vector3 m_ClimbingDirection = Vector3.zero;
		public Vector3 ClimbingDirection{
			get{ return m_ClimbingDirection;}
		}

		// RIGIDBODY 

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.MoveObject"/> use rigidbody.
		/// </summary>
		/// <value><c>true</c> if use rigidbody; otherwise, <c>false</c>.</value>
		public bool UseRigidbody{
			get{ return ( MotionControl == MotionControlType.RIGIDBODY && RigidbodyComponent != null ? true:false ); }
		}

		// CHARACTER CONTROLLER

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.MoveObject"/> use character controller.
		/// </summary>
		/// <value><c>true</c> if use character controller; otherwise, <c>false</c>.</value>
		public bool UseCharacterController{
			get{ return ( MotionControl == MotionControlType.CHARACTERCONTROLLER && CharacterControllerComponent != null && CharacterControllerComponent.enabled ? true:false ); }
		}

		// NAVIGATION MESH AGENT

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.MoveObject"/> use nav mesh agent.
		/// </summary>
		/// <value><c>true</c> if use nav mesh agent; otherwise, <c>false</c>.</value>
		public bool UseNavMeshAgent{
			get{ return ( MotionControl == MotionControlType.NAVMESHAGENT && NavMeshAgentComponent != null && m_NavMeshAgent.isActiveAndEnabled ? true:false ); }
		}
			
		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.MoveObject"/> NavMeshAgent has path.
		/// </summary>
		/// <value><c>true</c> if nav mesh agent has path; otherwise, <c>false</c>.</value>
		public bool NavMeshAgentHasPath{
			get{ return ( UseNavMeshAgent == true && m_NavMeshAgent.hasPath == true ? true:false ); }			
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.MoveObject"/> NavMeshAgent is on navigation mesh.
		/// </summary>
		/// <value><c>true</c> if nav mesh agent is on nav mesh; otherwise, <c>false</c>.</value>
		public bool NavMeshAgentIsOnNavMesh{
			get{ return ( UseNavMeshAgent == true && m_NavMeshAgent.isOnNavMesh == true ? true:false ); }
		}

		// MOVEMENT INFOS



		protected bool m_HasDetour = false;
		public bool HasDetour{
			get{return m_HasDetour;}
		}

		protected bool m_DetourComplete = false;
		public bool DetourComplete{
			get{return m_DetourComplete;}
		}

		protected bool m_HasEscapePosition = false;
		public bool HasEscape{
			get{return m_HasEscapePosition;}
		}

		protected bool m_HasAvoidPosition = false;
		public bool HasAvoid{
			get{return m_HasAvoidPosition;}
		}

		// BODY ORIENTATION

		protected Vector3 m_FrontLeftPosition = Vector3.zero;
		public Vector3 FrontLeftPosition{
			get{return m_FrontLeftPosition;}
		}

		protected Vector3 m_FrontRightPosition = Vector3.zero;
		public Vector3 FrontRightPosition{
			get{return m_FrontRightPosition;}
		}

		protected Vector3 m_BackLeftPosition = Vector3.zero;
		public Vector3 BackLeftPosition{
			get{return m_BackLeftPosition;}
		}

		protected Vector3 m_BackRightPosition = Vector3.zero;
		public Vector3 BackRightPosition{
			get{return m_BackRightPosition;}
		}

		protected Vector3 m_FrontLeftPositionGround = Vector3.zero;
		public Vector3 FrontLeftPositionGround{
			get{return m_FrontLeftPositionGround;}
		}

		protected Vector3 m_FrontRightPositionGround = Vector3.zero;
		public Vector3 FrontRightPositionGround{
			get{return m_FrontRightPositionGround;}
		}

		protected Vector3 m_BackLeftPositionGround = Vector3.zero;
		public Vector3 BackLeftPositionGround{
			get{return m_BackLeftPositionGround;}
		}

		protected Vector3 m_BackRightPositionGround = Vector3.zero;
		public Vector3 BackRightPositionGround{
			get{return m_BackRightPositionGround;}
		}
			
		// REACHED OR REQUIRED

		public bool MovePositionUpdateRequired{
			get{ return ( MovePositionReached || TargetMovePositionReached || m_MovePosition == Vector3.zero || Deadlock.Deadlocked ? true : false ); }
		}

		public bool MovePositionReached{
			get{ return ( CurrentMove.SegmentLength == 0 || DesiredStoppingDistance == 0 || MovePositionDistance <= DesiredStoppingDistance ? true : false ); }
		}

		public bool TargetMovePositionReached{
			get{ return ( CurrentTarget != null && CurrentTarget.TargetMovePositionReached( Owner.transform.position ) ? true : false ); }
		}

		public bool DetourPositionReached{
			get{ return ( DesiredStoppingDistance == 0 || DetourPositionDistance <= CurrentMove.StoppingDistance ? true : false ); }
		}

		public bool CoverPositionReached{
			get{ return ( DesiredStoppingDistance == 0 || DetourPositionDistance <= CurrentMove.StoppingDistance ? true : false ); }
		}

		// DISTANCES

		public float MovePositionDistance{
			get{ return ( Owner == null || CurrentMove == null ? 0 : PositionTools.GetDistance( MovePosition, Owner.transform.position, DesiredIgnoreLevelDifference ) ); }
		}

		public float TargetMovePositionDistance{
			get{ return ( Owner == null || CurrentTarget == null ? 0 : CurrentTarget.TargetMovePositionDistanceTo( Owner.transform.position ) ); }
		}

		protected float DetourPositionDistance{
			get{ return ( Owner == null || CurrentMove == null || CurrentMove.Detour.Position == Vector3.zero ? 0 :
				PositionTools.GetDistance( CurrentMove.Detour.Position, Owner.transform.position, CurrentMove.IgnoreLevelDifference ) ); }
		}



		/// <summary>
		/// Evaluates the best stopping distance and ignore level value.
		/// </summary>
		/// <value>The best stopping distance.</value>
		protected void UpdateDesiredStoppingDistance()
		{
			if( CurrentMove.SegmentLength == 0 || CurrentMove.SegmentLength > TargetMovePositionDistance )
			{				
				m_DesiredStoppingDistance = ( CurrentTarget.Move.StoppingDistance > 0 ? CurrentTarget.Move.StoppingDistance : 1 );
				m_DesiredIgnoreLevelDifference = ( CurrentTarget.Move.StoppingDistance > 0 ? CurrentTarget.Move.IgnoreLevelDifference : true );
			}
			else if( CurrentMove.StoppingDistance > 0 )
			{
				m_DesiredStoppingDistance = CurrentMove.StoppingDistance;
				m_DesiredIgnoreLevelDifference = CurrentMove.IgnoreLevelDifference;
			}
			else if( DefaultMove.StoppingDistance > 0 )
			{
				m_DesiredStoppingDistance = DefaultMove.StoppingDistance;
				m_DesiredIgnoreLevelDifference = DefaultMove.IgnoreLevelDifference;
			}
			else
			{
				m_DesiredStoppingDistance = ( CurrentTarget.Move.StoppingDistance > 0 ? CurrentTarget.Move.StoppingDistance : 1 );
				m_DesiredIgnoreLevelDifference = ( CurrentTarget.Move.StoppingDistance > 0 ? CurrentTarget.Move.IgnoreLevelDifference : true );
			}
		}


	}

	[System.Serializable]
	public class LocomotionMovesObject : LocomotionInfoObject
	{
		public LocomotionMovesObject(){}
		public LocomotionMovesObject( LocomotionMovesObject _object ) : base( _object ){}
		public LocomotionMovesObject( ICEWorldBehaviour _component ) : base( _component ){}

		public float HorizontalJumpDistance{
			get{ return m_HorizontalJumpDistance; }
		}

		public float VerticalJumpDistance{
			get{ return m_VerticalJumpDistance; }
		}

		private float m_JumpTime = 0;
		private Vector3 m_JumpStartPos = Vector3.zero;
		private Vector3 m_JumpEndPos = Vector3.zero;
		private float m_HorizontalJumpDistance = 0;
		private float m_VerticalJumpDistance = 0;

		protected bool HandleClimb( ICEWorldEntity _entity, Vector3 _end_pos )
		{
			INavigationLink _link = _entity as INavigationLink;

			if( _link == null )
				return false;


			Vector3 _up = _entity.transform.up;

			_end_pos = _end_pos + Vector3.up* BaseOffset; 

			if( PositionTools.GetDistance( Owner.transform.position, _end_pos, false ) < DesiredStoppingDistance )
			{
				m_ActionStatus = ActionStatusType.IsUndefined;
				m_ClimbingDirection = Vector3.zero;
			}
			else
			{
				Owner.transform.rotation = Quaternion.LookRotation( - _entity.transform.forward );

				if( ! IsClimbing )
					Owner.transform.position = _link.GetStartPosition( Owner, ClimbingOffset );

				bool _climbing_up = ( PositionTools.GetVerticalDistance( Owner.transform.position, _end_pos ) < 0 ? true : false );

				m_ClimbingDirection = _up * ( _climbing_up ? 1 : -1 ); 
					
				Owner.transform.position = Owner.transform.position + ( m_ClimbingDirection * ( _climbing_up ? ClimbingSpeed : ClimbingDownSpeed ) * Time.deltaTime );

				m_ActionStatus = ActionStatusType.IsClimbing;
			}
			
			return IsClimbing;
		}

		protected bool HandleJump( Vector3 _end_pos, float _height )
		{
			if( ! IsJumping )
			{
				m_JumpStartPos = Owner.transform.position;
				m_JumpEndPos = _end_pos + Vector3.up* BaseOffset;
				m_HorizontalJumpDistance = PositionTools.Distance( new Vector3( m_JumpStartPos.x, 0, m_JumpStartPos.z ), new Vector3( m_JumpEndPos.x, 0, m_JumpEndPos.z ) );
				m_VerticalJumpDistance = PositionTools.Distance( new Vector3( 0, m_JumpStartPos.y, 0 ), new Vector3( 0, m_JumpEndPos.y, 0 ) );

				m_JumpTime = 0;

				m_ActionStatus = ActionStatusType.IsJumping;
			}

			if( m_JumpTime < 1.0f ) 
			{
				float yOffset = _height * 4.0f*(m_JumpTime - m_JumpTime*m_JumpTime);
				Owner.transform.position = Vector3.Lerp (m_JumpStartPos, m_JumpEndPos, m_JumpTime) + yOffset * Vector3.up;
				m_JumpTime += Time.deltaTime / 2;
			}

			if( PositionTools.GetDistance( Owner.transform.position, m_JumpEndPos, false ) < DesiredStoppingDistance )
				m_ActionStatus = ActionStatusType.IsUndefined;

			return IsJumping;
		}
	
		protected bool HandleCrossBelowPosition( ref Vector3 _position )
		{
			if( IsCrossOverRequired )
			{
				m_ActionStatus = ActionStatusType.IsSliding;
			}
			else if( m_ActionStatus == ActionStatusType.IsSliding )
				m_ActionStatus = ActionStatusType.IsUndefined;

			return ( m_ActionStatus == ActionStatusType.IsSliding ? true : false );
		}

		protected bool HandleCrossOverPosition( ref Vector3 _position )
		{
			if( IsCrossOverRequired )
			{
				m_ActionStatus = ActionStatusType.IsVaulting;
			}
			else if( m_ActionStatus == ActionStatusType.IsVaulting )
				m_ActionStatus = ActionStatusType.IsUndefined;

			return ( m_ActionStatus == ActionStatusType.IsVaulting ? true : false );
		}

		protected bool HandleGlidePosition( ref Vector3 _position )
		{
			if( CurrentMove.Altitude.Enabled )
			{
				float _altitude = CurrentOperatingLevel;
				//float _altitude_differnce = CurrentOperatingLevelDifference;
				float _speed = DesiredVelocity.y;

				//Debug.Log( _speed + " : " + _altitude + " - " + _altitude_differnce );

				if( CurrentMove.Altitude.UseTargetLevel && CurrentTarget != null && CurrentTarget.IsValidAndReady )
				{
					if( _altitude < CurrentTarget.TargetTransformPosition.y + CurrentMove.Altitude.DesiredAltitude )
						m_VerticalSpeed = Mathf.Lerp( m_VerticalSpeed, _speed * Time.deltaTime, 0.1f );
					else if( _altitude > CurrentTarget.TargetTransformPosition.y + CurrentMove.Altitude.DesiredAltitude )
						m_VerticalSpeed = Mathf.Lerp( m_VerticalSpeed, - _speed * Time.deltaTime, 0.1f );
				}
				else
				{
					if( _altitude < CurrentMove.Altitude.DesiredAltitude )
						m_VerticalSpeed = Mathf.Lerp( m_VerticalSpeed, _speed * Time.deltaTime, 0.1f );
					else if( _altitude > CurrentMove.Altitude.DesiredAltitude )
						m_VerticalSpeed = Mathf.Lerp( m_VerticalSpeed, - _speed * Time.deltaTime, 0.1f );
				}

				//Debug.Log( _speed + " : " + _altitude + " - " + _altitude_differnce + " SPEED = " + m_VerticalSpeed );

				_position += Vector3.up * m_VerticalSpeed;

				m_ActionStatus = ActionStatusType.IsGliding;

				if( m_GroundLevel > _position.y )
					_position.y = m_GroundLevel;
					
				m_IsGrounded = ( m_Altitude <= BaseOffset + GroundLevelVariance ? true : false );
			}
			else if( m_ActionStatus == ActionStatusType.IsGliding )
				m_ActionStatus = ActionStatusType.IsUndefined;

			return ( m_ActionStatus == ActionStatusType.IsGliding ? true : false );
		}
	}
		
	[System.Serializable]
	public class LocomotionObject : LocomotionMovesObject
	{
		public LocomotionObject(){}
		public LocomotionObject( LocomotionObject _object ) : base( _object ){}
		public LocomotionObject( ICEWorldBehaviour _component ) : base( _component ){}


		/// <summary>
		/// Handles the ground orientation.
		/// </summary>
		public Quaternion HandleGroundOrientation( Quaternion _rotation )
		{
			if( CurrentBody.Type == BodyOrientationType.DEFAULT )
				return _rotation;

			float _roll_angle = 0;
			float _pitch_angle = 0;

			if( CurrentBody.UseRollAngle )
			{
				float _course_deviation = PositionTools.CourseDeviation( Owner.transform, m_MovePosition );

				_roll_angle = MathTools.Denormalize( _course_deviation * CurrentBody.RollAngleMultiplier, 0, CurrentBody.MaxRollAngle );

				// limits
				_roll_angle = Mathf.Clamp( _roll_angle, - CurrentBody.MaxRollAngle, CurrentBody.MaxRollAngle );
				/*
				if( _roll_angle > CurrentBody.MaxRollAngle )
					_roll_angle = CurrentBody.MaxRollAngle;

				if( _roll_angle < - CurrentBody.MaxRollAngle )
					_roll_angle = - CurrentBody.MaxRollAngle;*/

				// prepare euler angle 
				if( _roll_angle < 0 )
					_roll_angle = 360 + _roll_angle;
			}



			if( CurrentBody.Type == BodyOrientationType.BIPED )
			{
				_rotation = Quaternion.Euler( 0, _rotation.eulerAngles.y, _roll_angle );
			}
			else if( CurrentBody.Type == BodyOrientationType.GLIDER )
			{
				if( CurrentBody.UsePitch )
				{
					
					float _desired_altitude = ( CurrentMove.Altitude.UseTargetLevel && CurrentTarget != null ? CurrentTarget.TargetMovePosition.y : 0 ) + CurrentMove.Altitude.DesiredAltitude;
					float _level_deviation = PositionTools.LevelDeviation( Owner.transform, new Vector3( m_MovePosition.x, _desired_altitude , m_MovePosition.z ) );

					_pitch_angle = MathTools.Denormalize( _level_deviation * CurrentBody.PitchAngleMultiplier, 0, CurrentBody.MaxPitchAngle );

					// limits
					_pitch_angle = Mathf.Clamp( _pitch_angle, - CurrentBody.MaxPitchAngle, CurrentBody.MaxPitchAngle );

					/*if( _pitch_angle > CurrentBody.MaxPitchAngle )
						_pitch_angle = CurrentBody.MaxPitchAngle;

					if( _pitch_angle < - CurrentBody.MaxPitchAngle )
						_pitch_angle = - CurrentBody.MaxPitchAngle;*/

					// prepare euler angle 
					if( _pitch_angle < 0 )
						_pitch_angle = 360 + _pitch_angle;
				}

				_rotation = Quaternion.Lerp( _rotation, Quaternion.Euler( _pitch_angle, _rotation.eulerAngles.y, _roll_angle ), DesiredVelocity.y * Time.deltaTime );

				//_rotation = Quaternion.Euler( _pitch_angle, _rotation.eulerAngles.y, _roll_angle );
			}
			else
			{

				Vector3 ray = Vector3.down;//Owner.transform.TransformDirection(Vector3.down); //Vector3.down * 100;//
				Vector3 pos = Owner.transform.position;
				Vector3 _normal = Vector3.zero;

				CurrentBody.GetSize( Owner );

				m_FrontLeftPosition = Owner.transform.TransformPoint(new Vector3( - (CurrentBody.Width/2)+CurrentBody.WidthOffset, 0, (CurrentBody.Length/2)+CurrentBody.LengthOffset ));
				m_FrontRightPosition = Owner.transform.TransformPoint(new Vector3( (CurrentBody.Width/2)+CurrentBody.WidthOffset, 0, (CurrentBody.Length/2)+CurrentBody.LengthOffset ));
				m_BackLeftPosition = Owner.transform.TransformPoint(new Vector3( - (CurrentBody.Width/2)+CurrentBody.WidthOffset, 0, - (CurrentBody.Length/2)+CurrentBody.LengthOffset ));
				m_BackRightPosition = Owner.transform.TransformPoint(new Vector3( (CurrentBody.Width/2)+CurrentBody.WidthOffset, 0, - (CurrentBody.Length/2)+CurrentBody.LengthOffset ));

				if( GroundCheck == GroundCheckType.RAYCAST )
				{
					if( CurrentBody.UseAdvanced == false )
					{
						RaycastHit hit;
						if( Physics.Raycast( pos, ray ,out hit, Mathf.Infinity, GroundLayerMask, WorldManager.TriggerInteraction ) )
						{
							if( hit.collider.gameObject.GetComponent<Terrain>() ||  hit.collider.gameObject.GetComponent<MeshFilter>() )
								_normal = hit.normal;
						}
					}
					else
					{
						RaycastHit _hit_back_left;
						RaycastHit _hit_back_right;
						RaycastHit _hit_front_left;
						RaycastHit _hit_front_right;

						LayerMask _mask = GroundLayerMask;						
						if( Physics.Raycast( m_FrontLeftPosition + Vector3.up, ray, out _hit_front_left, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
							m_FrontLeftPositionGround = _hit_front_left.point;
						if( Physics.Raycast( m_FrontRightPosition + Vector3.up, ray, out _hit_front_right, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
							m_FrontRightPositionGround = _hit_front_right.point;
						if( Physics.Raycast( m_BackLeftPosition + Vector3.up, ray, out _hit_back_left, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
							m_BackLeftPositionGround = _hit_back_left.point;
						if( Physics.Raycast( m_BackRightPosition + Vector3.up, ray, out _hit_back_right, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
							m_BackRightPositionGround = _hit_back_right.point;

						_normal = (Vector3.Cross( m_BackRightPositionGround - Vector3.up, m_BackLeftPositionGround - Vector3.up) +
							Vector3.Cross( m_BackLeftPositionGround - Vector3.up, m_FrontLeftPositionGround - Vector3.up) +
							Vector3.Cross( m_FrontLeftPositionGround - Vector3.up, m_FrontRightPositionGround - Vector3.up) +
							Vector3.Cross( m_FrontRightPositionGround - Vector3.up, m_BackRightPositionGround - Vector3.up)
						).normalized;
					}
				}
				else if( Terrain.activeTerrain != null )
				{
					if( CurrentBody.UseAdvanced == false )
					{
						pos.x =  pos.x - Terrain.activeTerrain.transform.position.x;
						pos.z =  pos.z - Terrain.activeTerrain.transform.position.z;

						TerrainData _terrain_data = Terrain.activeTerrain.terrainData;
						_normal = _terrain_data.GetInterpolatedNormal( pos.x/_terrain_data.size.x, pos.z/_terrain_data.size.z );
					}
					else
					{
						m_BackRightPositionGround = m_BackRightPosition;
						m_BackRightPositionGround.y = Terrain.activeTerrain.SampleHeight( m_BackRightPositionGround );

						m_BackLeftPositionGround = m_BackLeftPosition;
						m_BackLeftPositionGround.y = Terrain.activeTerrain.SampleHeight( m_BackLeftPositionGround );

						m_FrontLeftPositionGround = m_FrontLeftPosition;
						m_FrontLeftPositionGround.y = Terrain.activeTerrain.SampleHeight( m_FrontLeftPositionGround );

						m_FrontRightPositionGround = m_FrontRightPosition;
						m_FrontRightPositionGround.y = Terrain.activeTerrain.SampleHeight( m_FrontRightPositionGround );


						_normal = (Vector3.Cross( m_BackRightPositionGround - Vector3.up, m_BackLeftPositionGround - Vector3.up) +
							Vector3.Cross( m_BackLeftPositionGround - Vector3.up, m_FrontLeftPositionGround - Vector3.up) +
							Vector3.Cross( m_FrontLeftPositionGround - Vector3.up, m_FrontRightPositionGround - Vector3.up) +
							Vector3.Cross( m_FrontRightPositionGround - Vector3.up, m_BackRightPositionGround - Vector3.up)
						).normalized;
					}
				}	

				Quaternion _new_rotation = Quaternion.FromToRotation( Vector3.up , _normal ) * _rotation;

				if( CurrentBody.Type == BodyOrientationType.QUADRUPED )
					_rotation = Quaternion.Euler( _new_rotation.eulerAngles.x, _rotation.eulerAngles.y, _roll_angle );
				else
					_rotation = Quaternion.Euler( _new_rotation.eulerAngles.x, _rotation.eulerAngles.y, _new_rotation.eulerAngles.z );

			}

			return _rotation;
		}

		/// <summary>
		/// Gets the avoid position.
		/// </summary>
		/// <returns>The avoid position.</returns>
		protected Vector3 GetAvoidPosition()
		{
			Transform _creature = Owner.transform;
			Transform _target = CurrentTarget.TargetGameObject.transform;

			m_TargetRelatedDirectionAngle = PositionTools.GetSignedDirectionAngle( _target, _creature.position );
			m_CreatureRelatedDirectionAngle = PositionTools.GetSignedDirectionAngle( _creature , _target.position );

			m_HasAvoidPosition = true;

			float _avoid_range = CurrentMove.Avoid.AvoidDistance;
			if( ( m_CreatureRelatedDirectionAngle >= 0 && m_TargetRelatedDirectionAngle >= 0 ) || 
				( m_CreatureRelatedDirectionAngle >= 0 && m_TargetRelatedDirectionAngle <= 0 ) )// AVOID LEFT 
				_avoid_range *= 1;
			else if( ( m_CreatureRelatedDirectionAngle <= 0 && m_TargetRelatedDirectionAngle <= 0 ) || 
				( m_CreatureRelatedDirectionAngle <= 0 && m_TargetRelatedDirectionAngle >= 0 ) ) // AVOID RIGHT
				_avoid_range *= -1;

			Vector3 _right = Vector3.Cross( _target.up, _target.forward );
			m_AvoidMovePosition = _target.position + ( _right * _avoid_range );

			Debug.DrawLine(_target.position, m_AvoidMovePosition, Color.green);

			return m_AvoidMovePosition;

		}

		/// <summary>
		/// Gets the escape position.
		/// </summary>
		/// <returns>The escape position.</returns>
		protected Vector3 GetEscapePosition()
		{
			Transform _creature = Owner.transform;
			Transform _target = CurrentTarget.TargetTransform;

			if( _target == null )
				Debug.Log( "mist" );
			
			m_HasEscapePosition = true;

			m_TargetRelatedDirectionAngle = PositionTools.GetSignedDirectionAngle( _target, _creature.position );

			Vector3 _heading = _creature.position - _target.position;
			m_EscapeAngle = PositionTools.GetNormalizedVectorAngle( _heading );			
			m_EscapeAngle += Random.Range( - CurrentMove.Escape.RandomEscapeAngle, CurrentMove.Escape.RandomEscapeAngle );
			m_EscapeMovePosition  = PositionTools.GetPositionByAngleAndRadius( _target.position, m_EscapeAngle, CurrentTarget.Selectors.SelectionRange + CurrentMove.Escape.EscapeDistance );

			//Debug.DrawLine(_creature.position, m_EscapeMovePosition, Color.green);

			return m_EscapeMovePosition;
		}

		/// <summary>
		/// Gets the orbit position.
		/// </summary>
		/// <returns>The orbit position.</returns>
		protected Vector3 GetOrbitPosition() 
		{ 
			Vector3 _center = CurrentTarget.TargetMovePosition;
			float _radius = CurrentMove.Orbit.Radius;
			float _rate = CurrentMove.Orbit.RadiusShift;
			float _min = CurrentMove.Orbit.MinDistance;
			float _max = CurrentMove.Orbit.MaxDistance;

			if( CurrentMove.OrbitRadius == 0 )
			{
				CurrentMove.OrbitRadius = _radius;
				CurrentMove.OrbitAngle = PositionTools.GetNormalizedVectorAngle( Owner.transform.position - _center );
				CurrentMove.OrbitDegrees = CurrentMove.OrbitDegrees * (Random.Range(0,1) == 1?1:-1);
			}

			if( ( _rate > 0 && CurrentMove.OrbitRadius < _max ) || ( _rate < 0 && CurrentMove.OrbitRadius > _min ) )
			{
				CurrentMove.OrbitRadius += _rate * Time.deltaTime;
				CurrentMove.SetOrbitComplete( false );

				if( CurrentMove.OrbitRadius < _min )
					CurrentMove.OrbitRadius = _min;
				else if( _max > 0 && CurrentMove.OrbitRadius > _max )
					CurrentMove.OrbitRadius = _max;

			}
			else
				CurrentMove.SetOrbitComplete( true );

			CurrentMove.OrbitAngle += CurrentMove.OrbitDegrees;

			if( CurrentMove.OrbitAngle > 360 )
				CurrentMove.OrbitAngle = CurrentMove.OrbitAngle - 360;

			float _a = CurrentMove.OrbitAngle * Mathf.PI / 180f;

			Vector3 new_position = _center + new Vector3(Mathf.Sin(_a) * CurrentMove.OrbitRadius, 0, Mathf.Cos(_a) * CurrentMove.OrbitRadius );
			//new_position.y = GetGroundLevel( new_position );

			return new_position;

		}
			
		/// <summary>
		/// Gets the detour position.
		/// </summary>
		/// <returns>The detour position.</returns>
		protected Vector3 GetDetourPosition()
		{
			if( DetourPositionReached )
				m_DetourComplete = true;

			if( m_DetourComplete )
				return CurrentTarget.TargetMovePosition;
			else
				return CurrentMove.Detour.Position;
		}

		protected Vector3 GetCoverPosition()
		{
			//Vector3 _owner_position = Owner.transform.position;
			float _distance = CurrentMove.Cover.MaxDistance;
			float _step_angle = Mathf.Clamp( CurrentMove.Cover.StepAngle, 1, 36 );
			float _offset = CurrentMove.Cover.HorizontalOffset;
			//float _vertical_offset = CurrentMove.Cover.VerticalOffset;
			RaycastHit _hit;

			Vector3 _origin_pos = Vector3.Lerp( Owner.transform.position, CurrentTarget.TargetMovePosition, 1f );

			Vector3 _direction = Owner.transform.forward;
			bool _found_cover = false;

			//if( m_CoverMovePosition == Vector3.zero || m_CoverMovePosition == CurrentTarget.TargetMovePosition )
			{
				float _raycast_angle = 0;
				while( _raycast_angle < 360 ) 
				{
					Ray _ray = new Ray( _origin_pos, _direction );
					if( Physics.Raycast( _ray, out _hit, _distance, ObstacleLayerMask ) ) 
					{
						DebugLine( _origin_pos, _hit.point, Color.yellow );

						Ray _target_ray = new Ray( _hit.point - _hit.normal * _distance, _hit.normal );
						if( _hit.collider.Raycast( _target_ray, out _hit, Mathf.Infinity ) ) 
						{
							m_CoverMovePosition = _hit.point + ( _hit.normal * _offset );

							DebugLine( Owner.transform.position, m_CoverMovePosition, Color.green );

							_found_cover = true;
							break;
						}
					}
					else
						DebugRay( _origin_pos, _direction * _distance, Color.red );

					_raycast_angle += _step_angle;
					_direction = Quaternion.Euler( 0, Owner.transform.eulerAngles.y + _raycast_angle, 0 ) * Vector3.forward;
				}

				if( ! _found_cover )
					m_CoverMovePosition = CurrentTarget.TargetMovePosition;
			}

			return m_CoverMovePosition;
		}

		/*
		protected Vector3 GetCoverPosition2()
		{
			Vector3 _position = CurrentTarget.TargetMovePosition;
			float _distance = CurrentMove.Cover.MaxDistance;
			float _step_angle = Mathf.Clamp( CurrentMove.Cover.StepAngle, 1, 36 );
			float _offset = CurrentMove.Cover.HorizontalOffset;
			RaycastHit _hit;

			float _raycast_angle = 0;
			Vector3 _direction = Owner.transform.forward;
			bool _found_cover = false;

			while( _raycast_angle < 360 ) 
			{
				Ray _ray = new Ray( Owner.transform.position, _direction );
				if( Physics.Raycast( _ray, out _hit, _distance, ObstacleLayerMask ) ) 
				{
					DebugLine( Owner.transform.position, _hit.point, Color.yellow );
					if( _hit.collider.Raycast( new Ray( _hit.point - _hit.normal * _distance, _hit.normal ), out _hit, Mathf.Infinity ) ) 
					{
						_position = _hit.point + ( _hit.normal * _offset );

						DebugLine( Owner.transform.position, _position, Color.green );

						_found_cover = true;
						break;
					}
				}
	
				_raycast_angle += _step_angle;
				_direction = Quaternion.Euler( 0, Owner.transform.eulerAngles.y + _raycast_angle, 0 ) * Vector3.forward;
			}

			if( ! _found_cover )
				_position = CurrentTarget.TargetMovePosition;

			return _position;
		}

*/
		/// <summary>
		/// Gets a randomized position.
		/// </summary>
		/// <returns>The random position.</returns>
		protected Vector3 GetRandomPosition()
		{
			return PositionTools.GetRandomPosition( Owner.transform.position, CurrentMove.GetMoveSegmentLength() ); 
		}


		/// <summary>
		/// Gets the modulated move position.
		/// </summary>
		/// <returns>The modulated move position.</returns>
		/// <param name="_owner_pos">_owner_pos.</param>
		/// <param name="_target_pos">_target_pos.</param>
		protected Vector3 ModulateMovePosition ( Vector3 _owner_position, Vector3 _desired_move_position ) {

			Vector3 _position = _desired_move_position;

			if( CurrentMove.SegmentLength > 0 )
			{
				float _target_distance = PositionTools.Distance(_owner_position, _desired_move_position);
				float _segment_legth = CurrentMove.GetMoveSegmentLength();

				if( _target_distance > 0 && _target_distance > _segment_legth * 0.5f )
				{
					float _f = Mathf.Max( _segment_legth/_target_distance, 0.1f );
					_position = Vector3.Lerp( _owner_position, _desired_move_position, _f );

					if( CurrentMove.DeviationLength > 0 )
					{
						Vector3 _forward = _position - _owner_position;
						Vector3 _right = Vector3.Cross( Vector3.up, _forward ).normalized;
						_position = _position + ( _right * CurrentMove.GetMoveDeviationVariance() );
					}
				}
			}

			if( CurrentBody.Type != BodyOrientationType.GLIDER )
				_position.y = GetGroundLevelByPosition( _position );

			return _position;

		}

		protected float GetCreatureGroundLevel(){
			return GetCreatureGroundLevel( Owner.transform.position );
		}

		/// <summary>
		/// Gets the creature ground level.
		/// </summary>
		/// <returns>The creature ground level.</returns>
		protected float GetCreatureGroundLevel( Vector3 _position )
		{
			if( GroundCheck == GroundCheckType.RAYCAST )
			{
				RaycastHit _hit;
				Vector3 _origin =  _position + ( Vector3.up * AutoVerticalRaycastOffset );
				if( Physics.Raycast( _origin, Vector3.down, out _hit, Mathf.Infinity, GroundLayerMask.value, WorldManager.TriggerInteraction ) )
				{
			
					if( _hit.collider.transform.IsChildOf( Owner.transform ) )
					{
						// backup in cases that the layer of the creature is also used as ground layer 
						SystemTools.EnableColliders( Owner.transform, false );
						if( Physics.Raycast( _origin, Vector3.down, out _hit, Mathf.Infinity, GroundLayerMask.value, WorldManager.TriggerInteraction ) )
							m_GroundLevel = _hit.point.y;
						SystemTools.EnableColliders( Owner.transform, true );
					}
					else
						m_GroundLevel = _hit.point.y;
				}
				else
					m_AutoVerticalRaycastOffset += 0.25f; //increases the offset automatically if there was no hit
			}
			else if( GroundCheck == GroundCheckType.SAMPLEHEIGHT && Terrain.activeTerrain != null )
				m_GroundLevel = Terrain.activeTerrain.SampleHeight( _position ) + Terrain.activeTerrain.transform.position.y;
			else if( GroundCheck == GroundCheckType.CUSTOM )
				m_GroundLevel = CustomGroundLevel;
			else if( GroundCheck == GroundCheckType.ZERO )
				m_GroundLevel = 0;
			else 
				m_GroundLevel =  _position.y - BaseOffset;

			return m_GroundLevel;
		}


		/// <summary>
		/// Gets the ground level.
		/// </summary>
		/// <returns>The ground level.</returns>
		/// <param name="position">Position.</param>
		protected float GetGroundLevelByPosition( Vector3 _position )
		{
			float _level = _position.y;

			if( GroundCheck == GroundCheckType.RAYCAST )
			{
				RaycastHit hit;
				if( Physics.Raycast( _position + ( Vector3.up * AutoVerticalRaycastOffset ), -Vector3.up, out hit, Mathf.Infinity, GroundLayerMask.value, WorldManager.TriggerInteraction ) )
				{
					_level = hit.point.y;
					m_OutOfArea = false;
				}
				else if( Physics.Raycast( _position + ( Vector3.up * VerticalRaycastOffsetMaximum ) , -Vector3.up, out hit, Mathf.Infinity, GroundLayerMask.value, WorldManager.TriggerInteraction ) )
				{
					_level = hit.point.y;
					m_OutOfArea = false;
				}
				else
				{
					m_OutOfArea = true;					
					m_AutoVerticalRaycastOffset += 0.25f;
				}
			}
			else if( GroundCheck == GroundCheckType.CUSTOM )
				_level = CustomGroundLevel;
			else if( GroundCheck == GroundCheckType.ZERO )
				_level = 0;
			else if( GroundCheck == GroundCheckType.SAMPLEHEIGHT && Terrain.activeTerrain != null )
				_level = Terrain.activeTerrain.SampleHeight( Owner.transform.position ) + Terrain.activeTerrain.transform.position.y;
			else 
				_level = _position.y;

			return _level;
		}

		public string UpdateGroundTextureName(){
			return SystemTools.GetGroundTextureName( Owner.transform.position, GroundLayerMask, ref m_SurfaceTerrain, ref m_SurfaceTerrainData, 2 );
		}
	}

	/// <summary>
	/// Move object. Handles all creature motions.
	/// </summary>
	[System.Serializable]
	public class MoveObject : LocomotionObject
	{
		public MoveObject(){}
		public MoveObject( MoveObject _object ) : base( _object ){}
		public MoveObject( ICEWorldBehaviour _component ) : base( _component ){}

		// EVENT HANDLER

		public delegate void OnTargetMovePositionReachedEvent( GameObject _sender, TargetObject _target );
		public event OnTargetMovePositionReachedEvent OnTargetMovePositionReached;

		public delegate void OnMoveCompleteEvent( GameObject _sender, TargetObject _target );
		public event OnMoveCompleteEvent OnMoveComplete;

		public delegate void OnUpdateDesiredMovePositionEvent( GameObject _sender, ref Vector3 _position );
		public event OnUpdateDesiredMovePositionEvent OnUpdateDesiredMovePosition;

		public delegate void OnMoveUpdatePositionEvent( GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position );
		public event OnMoveUpdatePositionEvent OnUpdateMovePosition;

		public delegate void OnCustomMoveEvent( GameObject _sender, ref Vector3 _new_position, ref Quaternion _new_rotation );
		public event OnCustomMoveEvent OnCustomMove;

		public delegate void OnMoveUpdateStepPositionEvent( GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position );
		public event OnMoveUpdateStepPositionEvent OnUpdateStepPosition;

		public delegate void OnMoveUpdateStepRotationEvent( GameObject _sender, Quaternion _origin_rotation, ref Quaternion _new_rotation );
		public event OnMoveUpdateStepRotationEvent OnUpdateStepRotation;



		//public bool NavMeshForced = false;
		//public int m_NavMeshPathPendingCounter = 0;

		/// <summary>
		/// Stops the move.
		/// </summary>
		public void Stop()
		{			
			if( NavMeshAgentHasPath )
				NavMeshAgentComponent.ResetPath();
		}

		public void Move()
		{
			if( Owner == null || CurrentTarget == null )
				return;
			
			if( TryMoveNavMeshAgent() ){}
			else if( TryMoveCharacterController() ){}
			else if( TryMoveRigidbody() ){}
			else if( TryCustomMove() ){}
			else
			{
				m_MoveStepRotation = HandleStepRotation();
				m_MoveStepPosition = HandleStepPosition();

				m_MoveStepPosition = GetGroundPosition( m_MoveStepPosition );

				if( AnimatorComponent != null && AnimatorComponent.applyRootMotion )
				{
					if( RigidbodyComponent != null )
					{
						if( RigidbodyComponent.useGravity == false )
							RigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionY;
						else
							RigidbodyComponent.constraints = RigidbodyConstraints.FreezeRotation;
					}
				}

				Owner.transform.rotation = m_MoveStepRotation;

				if( ! MoveHandledByRootMotion )
					Owner.transform.position = m_MoveStepPosition;
				else
				{
					Owner.transform.position = new Vector3( Owner.transform.position.x, m_MoveStepPosition.y, Owner.transform.position.z );
				}
			}

			// correct potential overlaps with other game objects
			OverlapPrevention.Update( Owner.transform, OverlapPreventionLayerMask, m_DesiredVelocity.z, ! ObstacleAvoidance.OvercomeObstaclesPossible );

			Deadlock.Check( Owner.transform, DesiredVelocity );

			Owner.transform.rotation = HandleGroundOrientation( Owner.transform.rotation );

			m_LastTransformPosition = Owner.transform.position;
			m_LastTransformRotation = Owner.transform.rotation;

			if( OnMoveComplete != null )
				OnMoveComplete( Owner, CurrentTarget );
		}

		/// <summary>
		/// Tries to move a nav mesh agent.
		/// </summary>
		/// <returns><c>true</c>, if move nav mesh agent was tryed, <c>false</c> otherwise.</returns>
		private bool TryMoveNavMeshAgent()
		{
			if( ! UseNavMeshAgent )
				return false;

			NavMeshAgent _agent = NavMeshAgentComponent;

			if( _agent.isOnOffMeshLink )
			{
				OffMeshLinkData _data = _agent.currentOffMeshLinkData;
				if( _data.valid )
				{					
					if( DebugLogIsEnabled )
						PrintDebugLog( this, _data.linkType.ToString() );

					// autoTraverseOffMeshLink must be false otherwise the agent will handle the link automatically
					if( ! _agent.autoTraverseOffMeshLink )
					{	
						bool _handled = false;

						// The OffMeshLink based on a manually placed object, so we can try to read further link information
						if( _data.offMeshLink != null )
						{
							if( _data.offMeshLink.startTransform != null && _data.offMeshLink.endTransform != null )
							{
								//ICECreatureLink _start_link = _data.offMeshLink.startTransform.GetComponent<ICECreatureLink>();
								//ICECreatureLink _end_link = _data.offMeshLink.endTransform.GetComponent<ICECreatureLink>();
							}
					
							ICEWorldEntity _entity = _data.offMeshLink.startTransform.GetComponentInParent<ICEWorldEntity>();
							if( _entity != null )
							{
								if( _entity.EntityType == EntityClassType.Ladder )
								{
									//if( PositionTools.GetDistance( Owner.transform.position, _entity., false ) < DesiredStoppingDistance )


									_handled = HandleClimb( _entity, _data.endPos );
								}
								else
								{
								}
							}
						}
													
						if( ! _handled && HandleJump( _data.endPos, 4 ) == false )
							_agent.CompleteOffMeshLink();
					}			
					else
					{
						PrintDebugLog( this,  "Using autoTraverseOffMeshLink" );
					}
				}
				else
					PrintDebugLog( this, "The currentOffMeshLinkData is not valid" );
			}
			else if( _agent.pathPending )
			{
				NavMeshPath _path = _agent.path;

				if( _path != null )
				{
					if( _path.status == NavMeshPathStatus.PathComplete )
					{
						//PrintDebugLog( this, "The agent can reach the destionation" );
					}
					else if( _path.status == NavMeshPathStatus.PathPartial )
					{
						//PrintDebugLog( this, "The agent can only get close to the destination" );

						//m_DesiredMovePosition = CurrentTarget.SetTargetMovePosition( _path.corners[_path.corners.Length-1] );
					}
					else if( _path.status == NavMeshPathStatus.PathInvalid )
					{
						//PrintDebugLog( this, "The agent cannot reach the destination" );
					}
				}
			}
			else
			{
				_agent.speed = m_DesiredVelocity.z;

				//_agent.baseOffset = GroundCheckBaseOffset;

				//_agent.acceleration = m_DesiredAngularVelocity.y * 10;
				//_agent.angularSpeed = m_DesiredAngularVelocity.y * 100; //Todo : could be to slow ...
				_agent.stoppingDistance = DesiredStoppingDistance;// * 0.5f;

				if( m_MovePosition != m_NavMeshCurrentMovePosition || ! _agent.hasPath )
				{
					_agent.SetDestination( m_MovePosition );
					m_NavMeshCurrentMovePosition = m_MovePosition;

					NavMeshPath _path = _agent.path;

					if( _path != null )
					{
						if( _path.status == NavMeshPathStatus.PathComplete )
						{
							//Debug.Log( "The agent can reach the destionation" );
						}
						else if( _path.status == NavMeshPathStatus.PathPartial )
						{
							//PrintDebugLog( this, "The agent can only get close to the destination" );
						}
						else if( _path.status == NavMeshPathStatus.PathInvalid )
						{
							//PrintDebugLog( this, "The agent cannot reach the destination" );
							//PrintDebugLog( this, "hasFoundPath will be false" );
						}
					}
				}

				if( m_DesiredVelocity == Vector3.zero )
				{
					#if UNITY_5_6_OR_NEWER
						_agent.isStopped = true;
					#else
						_agent.Stop(); 
					#endif

					_agent.velocity = Vector3.zero;
				}
				else if( _agent.hasPath ) 
				{
					#if UNITY_5_6_OR_NEWER
						_agent.isStopped = false;
					#else
						_agent.Resume();
					#endif
				}

				// CREATURE ROTATION BEGIN	
				if( CurrentMove.ViewingDirection != ViewingDirectionType.DEFAULT )
					Owner.transform.rotation = HandleStepRotation();
				// CREATURE ROTATION END
			}
	
			return true;

		}



		/// <summary>
		/// Tries to move the character controller.
		/// </summary>
		/// <returns><c>true</c>, if move character controller was tryed, <c>false</c> otherwise.</returns>
		private bool TryMoveCharacterController()
		{
			if( MotionControl == MotionControlType.CHARACTERCONTROLLER && CharacterControllerComponent != null )
				CharacterControllerComponent.enabled = ! IsCrossBelowRequired;

			if( ! UseCharacterController )
				return false;

			m_MoveStepRotation = HandleStepRotation();
			m_MoveStepPosition = HandleStepPosition();

			m_MoveStepPosition = GetGroundPosition( m_MoveStepPosition );

			// CREATURE ROTATION BEGIN
			Owner.transform.rotation = m_MoveStepRotation;
			// CREATURE ROTATION END

			// CREATURE MOVE BEGIN
			if( ! MoveHandledByRootMotion )
			{
				// CREATURE MOVE BEGIN
				Vector3 _forward = Owner.transform.TransformDirection( Vector3.forward ) ;				
				CharacterControllerComponent.SimpleMove( _forward * m_DesiredVelocity.z );
				// CREATURE MOVE END
			}
	
			// TODO: MOVE
			//Vector3 _direction = Owner.transform.InverseTransformPoint( Vector3.forward) ;				
			//CharacterControllerComponent.SimpleMove( _forward * m_MoveVelocity.z );

			//Vector3 _direction = m_MoveStepPosition - Owner.transform.position;
			//CharacterControllerComponent.Move( _direction );
			/*
				 
				 */

			return true;
		}

		/// <summary>
		/// Tries a custom move.
		/// </summary>
		/// <returns><c>true</c>, if custom move was tryed, <c>false</c> otherwise.</returns>
		private bool TryCustomMove()
		{
			if( MotionControl != MotionControlType.CUSTOM || OnCustomMove == null )
				return false;

			if( OnCustomMove != null )
				OnCustomMove( Owner, ref m_MovePosition, ref m_MoveRotation );

			return true;
		}

		/// <summary>
		/// Tries to move the rigidbody.
		/// </summary>
		/// <returns><c>true</c>, if move rigidbody was tryed, <c>false</c> otherwise.</returns>
		private bool TryMoveRigidbody()
		{
			if( ! UseRigidbody )
				return false;

			m_MoveStepRotation = HandleStepRotation();
			m_MoveStepPosition = HandleStepPosition();

			Owner.transform.position = new Vector3( Owner.transform.position.x, GetGroundPosition( m_MoveStepPosition ).y, Owner.transform.position.z );

			/*
			if( RigidbodyComponent.useGravity == false )
				RigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionY;
			else
				RigidbodyComponent.constraints = RigidbodyConstraints.FreezeRotation;*/

			// CREATURE ROTATION BEGIN
			//RigidbodyComponent.MoveRotation( m_MoveStepRotation );
			Owner.transform.rotation = m_MoveStepRotation;
			// CREATURE ROTATION END

			// CREATURE MOVE BEGIN
			if( ! MoveHandledByRootMotion )
			{
				if( RigidbodyComponent.isKinematic )
				{
					RigidbodyComponent.MovePosition( m_MoveStepPosition );			
				}
				else
				{
					Vector3 _velocity = ( m_MoveStepPosition - Owner.transform.position );

					if( _velocity.magnitude > 0 )
						_velocity = _velocity / Time.deltaTime;
					else
						_velocity = Vector3.zero;
					
						
					RigidbodyComponent.velocity = new Vector3( _velocity.x, RigidbodyComponent.velocity.y, _velocity.z );
				}
			}
			// CREATURE MOVE END

			return true;
		}


		public void UpdateTargets( TargetObject _target, TargetObject _home )
		{
			SetHomeTarget( _home );
			SetCurrentTarget( _target );
		}
			
		public void Prepare( TargetObject _target, TargetObject _home, BehaviourModeRuleObject _rule )
		{
			if( Owner == null || _rule == null || _target == null || _target.IsValidAndReady == false )
			{
				SetCurrentTarget( null );
				SetCurrentBehaviourModeRule( null );

				return;
			}




		}

		public void Update( BehaviourModeRuleObject _rule )
		{

			if( Owner == null || CurrentTarget == null || ! CurrentTarget.IsValidAndReady || _rule == null )
				return;

			SetCurrentBehaviourModeRule( _rule );

			UpdateDesiredStoppingDistance();

			// VELOCITY BEGIN
			if( CurrentBehaviourModeRule.MoveRequired )
			{
				m_DesiredVelocity = CurrentMove.GetVelocity( Controller, m_DesiredVelocity );
				m_DesiredAngularVelocity.y = CurrentMove.GetYawSpeed( Controller );
			}
			else
			{
				m_DesiredVelocity = Vector3.zero;
				m_DesiredAngularVelocity = Vector3.zero;
			}


			// VELOCITY END
			if( CurrentMove.Type == MoveType.DETOUR )
				m_HasDetour = true;
			else
			{
				m_HasDetour = false;
				m_DetourComplete = false;
			}

			if( CurrentMove.Type != MoveType.ESCAPE )
				m_HasEscapePosition = false;

			//m_CurrentTarget.TargetMovePositionUpdateLevel( GetGroundLevelByPosition( m_CurrentTarget.TargetPosition ) );

			// HANDLE TARGET MOVE POSITION
			if( CurrentTarget.TargetMovePositionReached( Owner.transform.position ) )
			{
				if( OnTargetMovePositionReached != null )
					OnTargetMovePositionReached( Owner, CurrentTarget );
			}

			//TODO:m_ActionStatus = ActionStatusType.IsUndefined;

			m_LastMovePosition = m_MovePosition;
			m_LastMoveRotation = m_MoveRotation;

			m_MovePosition = GetMovePosition();
			m_MoveRotation = GetMoveRotation();
		}


		/// <summary>
		/// Handles the step rotation.
		/// </summary>
		/// <returns>The step rotation.</returns>
		private Quaternion HandleStepRotation()
		{
			Quaternion _step_rotation = Owner.transform.rotation;

			float _distance = PositionTools.GetOverGroundDistance( m_MovePosition, Owner.transform.position );

			m_MoveCourseDeviation = PositionTools.CourseDeviation( Owner.transform.rotation,  m_MoveRotation );

			//Debug.Log( m_FinalAngularVelocity.y + " = " + _deviation + " * " + _dist_norm + " --- " + _rot_norm + " (" + _time + ")" );

			//Debug.Log( MoveDirectionAngle + " vs. " + m_MoveCourseDeviation );

			if( CurrentMove.Motion.UseAutomaticAngularVelocity || ( _distance < m_DesiredVelocity.z && m_DesiredAngularVelocity.y < m_DesiredVelocity.z ) )
			{
				float _deviation = m_MoveCourseDeviation;

				float _forward_speed = Mathf.Max( m_DesiredVelocity.z, 0.1f );
				float _dist_norm = MathTools.Normalize( _forward_speed , 0, _distance );
				float _rot_norm = MathTools.Normalize( _deviation , 0, 180 );
				float _time = Mathf.Pow( 1 - _rot_norm, 2 ) * Time.deltaTime;

				m_FinalAngularVelocity.y = Mathf.Lerp( m_FinalAngularVelocity.y, _deviation * _dist_norm, _time );
			}
			else
				m_FinalAngularVelocity.y = m_DesiredAngularVelocity.y;
			
			_step_rotation = Quaternion.Slerp( Owner.transform.rotation, m_MoveRotation, m_FinalAngularVelocity.y * Time.deltaTime );

			if( OnUpdateStepRotation != null )
				OnUpdateStepRotation( Owner, Owner.transform.rotation, ref _step_rotation );
			
			m_LastMoveStepRotation = m_MoveStepRotation;
			m_MoveStepRotation = _step_rotation;

			return _step_rotation; 
		}



		/// <summary>
		/// Handles the step position.
		/// </summary>
		/// <returns>The step position.</returns>
		/// <description>Step position defines the move during a frame update</description>
		private Vector3 HandleStepPosition()
		{
			Vector3 _step_position = Owner.transform.position;

			if(  CurrentTarget.Move.StoppingDistanceZoneRestricted && TargetMovePositionReached  )
			{
				//if( m_DesiredVelocity.z > 0 ) // || ! MoveHandledByRootMotion 
				{
					Vector3 _heading = Owner.transform.position - CurrentTarget.TargetMovePosition;
					_step_position = PositionTools.GetPositionByAngleAndRadius( CurrentTarget.TargetMovePosition, PositionTools.GetNormalizedVectorAngle( _heading ), CurrentTarget.Move.StoppingDistance );
					_step_position = Vector3.Lerp( Owner.transform.position, _step_position, 0.1f );
					_step_position.y = Owner.transform.position.y;	
				}
				//else
				//	_step_position = Owner.transform.position;
			}
			else
			{

				Vector3 _heading = m_MovePosition - Owner.transform.position;
				_heading.y = 0;

				// the clamped velocity multiplier will reduce the velocity according to the distance 
				// to avoid that the creature runs over its target
				float _velocity_multiplier = ( _heading.magnitude <= DesiredStoppingDistance ? Mathf.Clamp01( Mathf.Pow( _heading.sqrMagnitude, 2 ) ) : 1 );

				m_FinalMoveVelocity = m_DesiredVelocity * _velocity_multiplier;

				if( CurrentBody.Type == BodyOrientationType.GLIDER )
				{
					_step_position += Owner.transform.TransformDirection( Vector3.forward )* m_FinalMoveVelocity.z * Time.deltaTime;
					_step_position += Owner.transform.TransformDirection( Vector3.right )* m_FinalMoveVelocity.x * Time.deltaTime;
					_step_position.y = Owner.transform.position.y;	
				}
				else
				{
					_step_position += Owner.transform.TransformDirection( m_FinalMoveVelocity ) * Time.deltaTime;

					//_step_position += Owner.transform.TransformDirection( Vector3.forward )* m_FinalVelocity.z * Time.deltaTime;
					//_step_position += Owner.transform.TransformDirection( Vector3.right )* m_FinalVelocity.x * Time.deltaTime;
					//_step_position += Owner.transform.TransformDirection( Vector3.up )* m_FinalVelocity.y * Time.deltaTime;	
				}
			}

			if( OnUpdateStepPosition != null )
				OnUpdateStepPosition( Owner, Owner.transform.position, ref _step_position );


			m_LastMoveStepPosition = m_MoveStepPosition;
			m_MoveStepPosition = _step_position;

			return _step_position;
		}

		//********************************************************************************
		// GROUND HANDLING
		//********************************************************************************


		private bool CheckIsGrounded()
		{
			if( UseInternalGravity )
				return ( m_Altitude <= BaseOffset + GroundLevelVariance ? true : false );
			else if( UseRigidbody )
				return ( m_Altitude <= BaseOffset + GroundLevelVariance ? true : false );
			else if( UseCharacterController )
				return CharacterControllerComponent.isGrounded;	
			else if( UseNavMeshAgent )
				return NavMeshAgentComponent.isOnNavMesh;
			else
				return false;
		}

		private Vector3 GetGravityPosition( Vector3 _position )
		{
			if( ! UseInternalGravity )
				return _position;
			
			if( m_Altitude <= BaseOffset + GroundLevelVariance )
			{
				_position.y = m_GroundLevel + BaseOffset;

				m_IsGrounded = true;				
				m_VerticalSpeed = 0;	
				m_FallTime = Time.time;
			}
			else
			{
				float _speed = Gravity*Gravity * (Time.time - m_FallTime) * -1; // not correct but looks good  -9.81 m/s^2 * time

				m_VerticalSpeed = Mathf.Lerp( m_VerticalSpeed, _speed * Time.deltaTime, GravityInterpolator );

				if( m_VerticalSpeed < - FallVelocityMax )
					m_VerticalSpeed = - FallVelocityMax;

				_position += ( Vector3.up * m_VerticalSpeed );
				if( m_GroundLevel > _position.y + BaseOffset )
					_position.y = m_GroundLevel + BaseOffset;

				m_ActionStatus = ActionStatusType.IsFalling;
				m_IsGrounded = false;
			}

			return _position;
		}

		private Vector3 GetGroundPosition( Vector3 _position )
		{
			m_GroundLevel = GetCreatureGroundLevel( _position );			
			m_Altitude = _position.y - m_GroundLevel;
			m_AbsoluteAltitude = _position.y;
			m_IsGrounded = CheckIsGrounded();

			if( ! HandleGlidePosition( ref _position ) && ! HandleCrossOverPosition( ref _position ) )
				_position = GetGravityPosition( _position );

			return _position;
		}


		//********************************************************************************
		// MOVE POSITION HANDLING
		//********************************************************************************
		
		private Quaternion GetMoveRotation()
		{
			Vector3 _position = Owner.transform.position;
			Vector3 _heading = ( m_MovePosition - _position );
			_heading.y = 0;

			if( CurrentMove.ViewingDirection == ViewingDirectionType.CENTER )
				_heading = ( CurrentTarget.TargetTransformPosition - _position );
			else if( CurrentMove.ViewingDirection == ViewingDirectionType.OFFSET )
				_heading = ( CurrentTarget.TargetOffsetPosition - _position );
			else if( CurrentMove.ViewingDirection == ViewingDirectionType.MOVE )
				_heading = ( CurrentTarget.TargetMovePosition - _position );
			else if( CurrentMove.ViewingDirection == ViewingDirectionType.POSITION )
				_heading = ( CurrentMove.ViewingDirectionPosition - _position );

			Quaternion _rotation = Owner.transform.rotation;

			if( CurrentMove.ViewingDirection != ViewingDirectionType.NONE )
			{
				if( _heading != Vector3.zero )
					_rotation = Quaternion.LookRotation( _heading, Vector3.up );

				_rotation = Quaternion.Euler( 0, _rotation.eulerAngles.y, 0 );
			}

			return _rotation;
		}
	


		private Vector3 GetMovePosition()
		{
			// REGULAR COURSE EVALUATIONS BEGIN
			if( MovePositionUpdateRequired )
			{
				switch( CurrentMove.Type )
				{
					case MoveType.AVOID: // AVOID MOVE
						m_DesiredMovePosition = GetAvoidPosition();	
						break;
					case MoveType.ESCAPE: // ESCAPE MOVE
						m_DesiredMovePosition = GetEscapePosition();	
						break;
					case MoveType.ORBIT: // ORBIT MOVE
						m_DesiredMovePosition = GetOrbitPosition();
						break;
					case MoveType.DETOUR: // DETOUR MOVE
						m_DesiredMovePosition = GetDetourPosition();
						break;
					//case MoveType.COVER: // COVER MOVE
					//	m_DesiredMovePosition = GetCoverPosition();
					//	break;
					case MoveType.RANDOM: // RANDOM MOVE
						m_DesiredMovePosition = GetRandomPosition();
						break;
					default: // DEFAULT AND CUSTOM MOVE
						m_DesiredMovePosition = CurrentTarget.TargetMovePosition;
						break;
				}

				m_DesiredMovePosition = ModulateMovePosition( Owner.transform.position, m_DesiredMovePosition );

				if( OutOfArea && ! AllowOutOfArea )
				{
					Vector3 _invalid_pos = m_DesiredMovePosition;
						
					if( HomeTarget != null )
						m_DesiredMovePosition = ModulateMovePosition( Owner.transform.position, HomeTarget.TargetMovePosition );
					else
						m_DesiredMovePosition = ModulateMovePosition( Owner.transform.position, Vector3.zero );	

					if( DebugLogIsEnabled ) PrintDebugLog( this, "GetMovePosition - The desired move position " + _invalid_pos + " would be out of the area and was adapted to " + m_DesiredMovePosition + "." );
				}
					

				if( UseNavMeshAgent && UseSamplePosition )
				{
					NavMeshHit hit;
					if( ! NavMesh.SamplePosition( m_DesiredMovePosition, out hit, 1.0f, NavMesh.AllAreas ) ) 
					{
						if( NavMesh.SamplePosition( m_DesiredMovePosition, out hit, SamplePositionRange , NavMesh.AllAreas ) )// (CurrentTarget.Move.RandomRange > 0?CurrentTarget.Move.RandomRange:5), NavMesh.AllAreas ) )
						{
							PrintDebugLog( this, "Update DesiredMovePosition from " + m_DesiredMovePosition + " to SamplePosition " + hit.position + "!" );
							m_DesiredMovePosition = CurrentTarget.SetTargetMovePosition( hit.position );
						}
					}
				}

				if( OnUpdateDesiredMovePosition != null )
				{
					Vector3 _updated_position = m_DesiredMovePosition;
					OnUpdateDesiredMovePosition( Owner, ref _updated_position );

					if( _updated_position != m_DesiredMovePosition )
					{
						PrintDebugLog( this, "Update DesiredMovePosition from " + m_DesiredMovePosition + " to custom position " + _updated_position + "!" );
						m_DesiredMovePosition = CurrentTarget.SetTargetMovePosition( _updated_position );
					}
				}
			}
			// REGULAR COURSE EVALUATIONS END

			// POTENTIAL COURSE CORRECTIONS BEGIN
			Vector3 _position = m_DesiredMovePosition;

			//Debug.Log( "m_DesiredMovePosition : " + m_DesiredMovePosition + " m_MovePosition : " + m_MovePosition );
				
			if( GroundCheck == GroundCheckType.RAYCAST )
				_position = GroundAvoidance.Scan( Owner.transform, _position, GroundLayerMask, WaterLayerMask, AutoVerticalRaycastOffset );

			if( ObstacleCheck == ObstacleCheckType.BASIC )
				_position = ObstacleAvoidance.Scan( Owner.transform, _position, ObstacleLayerMask, DesiredStoppingDistance, m_DesiredVelocity.z );
			// POTENTIAL COURSE CORRECTIONS END


			// POTENTIAL MANUAL CORRECTIONS BEGIN
			if( OnUpdateMovePosition != null )
				OnUpdateMovePosition( Owner, Owner.transform.position, ref _position );
			// POTENTIAL MANUAL CORRECTIONS END

			m_MovePosition = _position;
			m_MoveSpeed = m_DesiredVelocity.z;
			m_MoveDirection = PositionTools.GetDirectionAngleByPosition( Owner.transform, _position ) * 180;
			m_MoveAngularSpeedRaw = ( m_MoveDirection > -1 && m_MoveDirection < 1 ? 0 : ( m_MoveDirection * m_DesiredAngularVelocity.y ) );
			m_MoveAngularSpeed = Mathf.Clamp( Mathf.Lerp( m_MoveAngularSpeed, m_MoveAngularSpeedRaw, 0.1f ) , -180, 180 );
			m_MoveAngularSpeedLimited = m_MoveAngularSpeed / 100;// Mathf.Clamp( m_MoveAngularSpeed, - m_DesiredAngularVelocity.y, m_DesiredAngularVelocity.y );


			return m_MovePosition;
		}
	}
}

