// ##############################################################################
//
// ice_utilities_types.cs
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

namespace ICE.World.EnumTypes
{
	// *** A ***

	/// <summary>
	/// Anchor presets.
	/// </summary>
	public enum AnchorPresets
	{
		TopLeft,
		TopCenter,
		TopRight,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		BottomLeft,
		BottonCenter,
		BottomRight,
		BottomStretch,

		VertStretchLeft,
		VertStretchRight,
		VertStretchCenter,

		HorStretchTop,
		HorStretchMiddle,
		HorStretchBottom,

		StretchAll
	}

	/// <summary>
	/// Animator control type.
	/// </summary>
	public enum AnimatorControlType
	{
		DIRECT,
		ADVANCED
	}
		
	/// <summary>
	/// Animation interface type.
	/// </summary>
	public enum AnimationInterfaceType
	{
		NONE=0,
		MECANIM,
		LEGACY,
		CLIP,
		CUSTOM
	}

	/// <summary>
	/// Axis input type.
	/// </summary>
	public enum AxisInputType
	{
		KeyOrMouseButton,
		MouseMovement,
		JoystickAxis
	}

	public enum AnimationEventParameterType
	{
		None=0,
		Float,
		Integer,
		String
	}

	// *** B *** 

	/// <summary>
	/// Boolean value type.
	/// </summary>
	public enum BooleanValueType
	{
		TRUE=0,
		FALSE
	}

	// *** C *** 

	/// <summary>
	/// Conditional operator type.
	/// </summary>
	public enum ConditionalOperatorType
	{
		AND = 0,
		OR = 1
	}

	/// <summary>
	/// Supported Collider types.
	/// </summary>
	public enum ColliderType
	{
		Sphere,
		Box,
		Capsule,
		Mesh
	}

	/// <summary>
	/// Collider event type.
	/// </summary>
	public enum ColliderEventType
	{
		ENTER,
		STAY,
		EXIT,
		HIT
	}

	// *** D *** 

	public enum DamageTransferType{
		Direct,
		Message,
		DirectOrMessage,
		DirectAndMessage
	}

	/// <summary>
	/// Damage impact type.
	/// </summary>
	public enum DamageForceType{
		None,
		Direction,
		Explosion
	}

	/// <summary>
	/// Dynamic boolean value type.
	/// </summary>
	public enum DynamicBooleanValueType
	{
		IsGrounded,
		IsJumping,
		IsSliding,
		IsVaulting,
		Deadlocked,
		MovePositionReached,
		MovePositionUpdateRequired,
		TargetMovePositionReached
	}

	/// <summary>
	/// Dynamic integer value type.
	/// </summary>
	public enum DynamicIntegerValueType
	{
		undefined
	}

	/// <summary>
	/// Dynamic float value type.
	/// </summary>
	public enum DynamicFloatValueType
	{
		MoveForwardSpeed,
		MoveAngularSpeed,
		MoveAngularSpeedLimited,
		MoveDirection,
		MovePositionDistance,

		FallSpeed,

		Altitude,
		AbsoluteAltitude,

		TargetMovePositionDistance

	}

	// *** E *** 

	public enum EntityClassType
	{
		Player,
		Creature,
		Plant,

		Object,
		Ladder,
		Door,
		Item,
		Tool,
		Flashlight,

		Weapon,
		RangedWeapon,
		MeleeWeapon,
		Torch,
		Projectile,
		Explosive,
		Mine,
		Turret,

		Organism,
		BodyPart,

		Location,
		Shelter,
		Fireplace,
		Waypoint,
		Marker,
		Breadcrumb,
		Mouse,
		Zone,
		Link,
		Entity,
		Undefined
	}


	// *** F *** 
	// *** G *** 

	/// <summary>
	/// Ground check type.
	/// </summary>
	public enum GroundCheckType
	{
		NONE,
		RAYCAST,
		SAMPLEHEIGHT,
		CUSTOM,
		ZERO
	}

	// *** H *** 
	// *** I *** 

	/// <summary>
	/// Impact check type.
	/// </summary>
	public enum ImpactCheckType
	{
		DEFAULT,
		CUSTOM
	}

	/// <summary>
	/// Influence type. TODO: BETA dynamic influences coming soon
	/// </summary>
	public enum InfluenceType
	{
		Unknown

	}

	// *** J *** 
	// *** K *** 
	// *** L *** 

	public enum LinkMotionType
	{
		JumpUp,
		JumpDown,
		JumpOver,
		Climb
	}

	/// <summary>
	/// Logical operator type.
	/// </summary>
	public enum LogicalOperatorType
	{
		EQUAL = 0,
		NOT = 1,
		LESS = 2,
		LESS_OR_EQUAL = 3,
		GREATER = 4,
		GREATER_OR_EQUAL = 5
	}
	// *** M *** 

	public enum ActionStatusType
	{
		IsUndefined,
		IsFalling,
		IsGliding,
		IsJumping,
		IsSliding,
		IsVaulting,
		IsClimbing
	}
		
	/// <summary>
	/// Method parameter type.
	/// </summary>
	public enum BehaviourEventParameterType
	{
		None=0,
		Float,
		Integer,
		String,
		Boolean,
		Sender,
		SenderComponent,
		SenderTransform
	}
	// *** N *** 
	// *** O *** 


	/// <summary>
	/// Odour type.
	/// </summary>
	public enum OdourType
	{
		NONE=0,
		CAMPHORACEOUS, //Camphoraceous – mothballs
		MUSKY, //Musky – perfumes/aftershave
		FLORAL, //Floral – roses
		MINTY, //Pepperminty – mint gum
		ETHEREAL,//Ethereal – dry cleaning fluid
		PUNGENT, //Pungent – vinegar
		PUTRID, //Putrid – rotten eggs
		UNDEFINED
	}

	/// <summary>
	/// Obstacle check type.
	/// </summary>
	public enum ObstacleCheckType
	{
		NONE,
		BASIC
	}

	/// <summary>
	/// Overlap type.
	/// </summary>
#if UNITY_5_4_OR_NEWER
	public enum OverlapType
	{
		NONE,
		SPHERE,
		BOX,
		CAPSULE
	}
#elif UNITY_5_3 || UNITY_5_3_OR_NEWER
	public enum OverlapType
	{
		NONE,
		SPHERE,
		BOX
	}
#else
	public enum OverlapType
	{
		NONE,
		SPHERE
	}
#endif

	/// <summary>
	/// Obstacle avoidance action type.
	/// </summary>
	public enum ObstacleAvoidanceActionType 
	{
		None,
		CrossBelow,
		CrossOver,
		Stop
	}

	// *** P *** 

	/// <summary>
	/// Pivot presets.
	/// </summary>
	public enum PivotPresets
	{
		TopLeft,
		TopCenter,
		TopRight,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		BottomLeft,
		BottomCenter,
		BottomRight,
	}

	public enum PreferedDirectionType 
	{
		UNDEFINED,
		LEFT,
		RIGHT
	}

	// *** Q *** 

	public enum ObjectSelectType
	{
		OVER,
		CLICK
	}

	public enum ObjectSelectVisibilityType
	{
		NONE,
		COLOR,
		MATERIAL,
		SHADER
	}

	// *** R *** 

	/// <summary>
	/// Random offset type.
	/// </summary>
	public enum RandomOffsetType
	{
		EXACT,
		CIRCLE,
		HEMISPHERE,
		SPHERE
	}

	/// <summary>
	/// Random seed type.
	/// </summary>
	public enum RandomSeedType
	{
		DEFAULT = 0,
		TIME,
		CUSTOM
	}

	// *** S *** 

	/// <summary>
	/// String operator type.
	/// </summary>
	public enum StringOperatorType
	{
		EQUAL,
		NOT
	}

	/// <summary>
	/// Sequence order type.
	/// </summary>
	public enum SequenceOrderType
	{
		CYCLE,
		RANDOM,
		PINGPONG,
		WEIGHTEDRANDOM
	}

	// *** T *** 

	/// <summary>
	/// Temperature scale type.
	/// </summary>
	public enum TemperatureScaleType
	{
		CELSIUS,
		FAHRENHEIT
	}


	// *** U *** 
	// *** V *** 
	// *** W *** 

	/// <summary>
	/// Weather type.
	/// </summary>
	public enum WeatherType
	{
		UNDEFINED = 0,
		FOGGY,
		RAIN,
		HEAVY_RAIN,
		WINDY,
		STORMY,
		CLEAR,
		PARTLY_CLOUDY,
		MOSTLY_CLOUDY,
		CLOUDY
	}

	/// <summary>
	/// Water check type.
	/// </summary>
	public enum WaterCheckType
	{
		DEFAULT,
		CUSTOM
	}

	// *** XYZ *** 
}