// ##############################################################################
//
// ice_CreatureTypes.cs
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

namespace ICE.Creatures.EnumTypes
{
	// *** A ***

	/// <summary>
	/// Attribute type.
	/// </summary>
	public enum AttributeType
	{
		TARGET,
		ODOUR
	}

	// *** B ***

	/// <summary>
	/// Body orientation type.
	/// </summary>
	public enum BodyOrientationType
	{
		DEFAULT=0,
		BIPED,
		QUADRUPED,
		CRAWLER,
		GLIDER
	}

	/// <summary>
	/// Broadcast message type.
	/// </summary>
	public enum BroadcastMessageType
	{
		NONE,
		COMMAND
	}

	// *** C ***
	// *** D ***

	/// <summary>
	/// Deadlock action type.
	/// </summary>
	public enum DeadlockActionType
	{
		DIE=0,
		BEHAVIOUR,
		UPDATE
	}

	// *** E ***



	// *** F ***
	// *** G ***

	public enum CreatureGenderType
	{
		UNDEFINED=0,
		MALE,
		FEMALE,
		NEUTERED,
		HERMAPHRODITE
	}

	// *** H ***
	// *** I ***

	/// <summary>
	/// Inventory action type.
	/// </summary>
	public enum InventoryActionType{
		CollectActiveItem,
		DropItem,
		ChangeParent
	}

	// *** J ***
	// *** K ***
	// *** L ***

	/// <summary>
	/// Look invisible type.
	/// </summary>
	public enum LookInvisibleType 
	{
		None,
		AllRenderer,
		SkinnedMeshRendererOnly,
		ShaderManipulation
	}

	/// <summary>
	/// Label type.
	/// </summary>
	public enum LabelType {
		Gray = 0,
		Blue,
		Teal,
		Green,
		Yellow,
		Orange,
		Red,
		Purple,
		None
	}

	/// <summary>
	/// Link type.
	/// </summary>
	public enum LinkType
	{
		MODE,
		RULE
	}

	// *** M ***

	/// <summary>
	/// Mounting pivot type.
	/// </summary>
	public enum MountingPivotType
	{
		PivotalPoint,
		SeperateAxes
	}

	/// <summary>
	/// Move type.
	/// </summary>
	public enum MoveType
	{
		DEFAULT = 0,
		CUSTOM,
		ESCAPE,
		AVOID,
		ORBIT,
		DETOUR,
		//COVER,
		RANDOM
	}

	/// <summary>
	/// Motion control type.
	/// </summary>
	public enum MotionControlType
	{
		INTERNAL,
		NAVMESHAGENT,
		RIGIDBODY,
		CHARACTERCONTROLLER,
		CUSTOM
	}

	// *** O ***

	/*
	public enum OdourType
	{
		NONE=0,
		SWEATY,
		SPERMOUS,
		FISHY,
		MALTY,
		MUSKY,
		URINOUS,
		MINTY,
		CAMPHORACEOUS,
		UNDEFINED
	}*/

	// *** P ***
	// *** Q ***
	// *** R ***

	/// <summary>
	/// Ranged weapon ammunition type.
	/// </summary>
	public enum RangedWeaponAmmunitionType
	{
		Simulated,
		Projectile,
		BallisticProjectile
	}

	// *** S ***

	/// <summary>
	/// Selection status.
	/// </summary>
	public enum SelectionStatus
	{
		UNCHECKED,
		VALID,
		INVALID
	}

	/// <summary>
	/// Selection data type.
	/// </summary>
	public enum SelectionExpressionDataType
	{
		UNDEFINED,
		NUMBER,
		DYNAMICNUMBER,
		STRING,
		BOOLEAN,
		ENUM,
		OBJECT,
		KEYCODE,
		AXIS,
		TOGGLE,
		BUTTON
	}

	/// <summary>
	/// Selection expression type.
	/// </summary>
	public enum SelectionExpressionType
	{
		// OWNER
		OwnGameObject,
		OwnBehaviour,
		OwnReceivedCommand,
		OwnAge,
		OwnGenderType,
		OwnTrophicLevel,
		OwnOdour,
		OwnOdourIntensity,
		OwnOdourRange,
		OwnEnvTemperatureDeviation,
		OwnFitness,
		OwnHealth,
		OwnStamina,
		OwnPower,
		OwnDamage,
		OwnStress,
		OwnDebility,
		OwnHunger,
		OwnThirst,
		OwnAggressivity,
		OwnExperience,
		OwnAnxiety,
		OwnNosiness,
		OwnZoneName,

		OwnVisualSense,
		OwnAuditorySense,
		OwnOlfactorySense,
		OwnGustatorySense,
		OwnTactileSense,

		OwnSlot0Amount,
		OwnSlot1Amount,
		OwnSlot2Amount,
		OwnSlot3Amount,
		OwnSlot4Amount,
		OwnSlot5Amount,
		OwnSlot6Amount,
		OwnSlot7Amount,
		OwnSlot8Amount,
		OwnSlot9Amount,

		OwnSlot0MaxAmount,
		OwnSlot1MaxAmount,
		OwnSlot2MaxAmount,
		OwnSlot3MaxAmount,
		OwnSlot4MaxAmount,
		OwnSlot5MaxAmount,
		OwnSlot6MaxAmount,
		OwnSlot7MaxAmount,
		OwnSlot8MaxAmount,
		OwnSlot9MaxAmount,

		OwnerIsWithinHomeArea,
		OwnerPosition,
		OwnerIsDead,
		OwnerIsInjured,
		OwnerIsSheltered,
		OwnerIsIndoor,
		OwnerIsGrounded,
		OwnerAltitude,
		OwnerIsSelectedByTarget,

		OwnHomeDistance,

		// ACTIVE
		ActiveTargetGameObject,
		ActiveTargetName,
		ActiveTargetEntityType,
		ActiveTargetTime,
		ActiveTargetTimeTotal,
		ActiveTargetAge,
		ActiveTargetHasParent,
		ActiveTargetParentName,

		ActiveTargetIsDestroyed,
		ActiveTargetHasOwnerActiveSelected,
		ActiveTargetActiveCounterpartsLimit,
		ActiveTargetDurability,
		ActiveTargetDurabilityInPercent,
		ActiveTargetIsInFieldOfView,
		ActiveTargetIsVisible,
		ActiveTargetIsAudible,
		ActiveTargetIsSmellable,
		ActiveTargetVisibilityByDistance,
		ActiveTargetAudibilityByDistance,
		ActiveTargetSmellabilityByDistance,

		ActiveTargetSlot0Amount,
		ActiveTargetSlot1Amount,
		ActiveTargetSlot2Amount,
		ActiveTargetSlot3Amount,
		ActiveTargetSlot4Amount,
		ActiveTargetSlot5Amount,
		ActiveTargetSlot6Amount,
		ActiveTargetSlot7Amount,
		ActiveTargetSlot8Amount,
		ActiveTargetSlot9Amount,

		ActiveTargetSlot0MaxAmount,
		ActiveTargetSlot1MaxAmount,
		ActiveTargetSlot2MaxAmount,
		ActiveTargetSlot3MaxAmount,
		ActiveTargetSlot4MaxAmount,
		ActiveTargetSlot5MaxAmount,
		ActiveTargetSlot6MaxAmount,
		ActiveTargetSlot7MaxAmount,
		ActiveTargetSlot8MaxAmount,
		ActiveTargetSlot9MaxAmount,

		ActiveTargetOdour,
		ActiveTargetOdourIntensity,
		ActiveTargetOdourIntensityNet,
		ActiveTargetOdourIntensityByDistance,
		ActiveTargetOdourRange,

		// LAST
		LastTargetGameObject,
		LastTargetName,
		LastTargetEntityType,
		LastTargetTime,
		LastTargetTimeTotal,
		LastTargetAge,
		LastTargetHasParent,
		LastTargetParentName,

		LastTargetIsDestroyed,
		LastTargetHasOwnerActiveSelected,
		LastTargetActiveCounterpartsLimit,
		LastTargetDurability,
		LastTargetDurabilityInPercent,
		LastTargetIsInFieldOfView,
		LastTargetIsVisible,
		LastTargetIsAudible,
		LastTargetIsSmellable,
		LastTargetVisibilityByDistance,
		LastTargetAudibilityByDistance,
		LastTargetSmellabilityByDistance,

		LastTargetSlot0Amount,
		LastTargetSlot1Amount,
		LastTargetSlot2Amount,
		LastTargetSlot3Amount,
		LastTargetSlot4Amount,
		LastTargetSlot5Amount,
		LastTargetSlot6Amount,
		LastTargetSlot7Amount,
		LastTargetSlot8Amount,
		LastTargetSlot9Amount,

		LastTargetSlot0MaxAmount,
		LastTargetSlot1MaxAmount,
		LastTargetSlot2MaxAmount,
		LastTargetSlot3MaxAmount,
		LastTargetSlot4MaxAmount,
		LastTargetSlot5MaxAmount,
		LastTargetSlot6MaxAmount,
		LastTargetSlot7MaxAmount,
		LastTargetSlot8MaxAmount,
		LastTargetSlot9MaxAmount,

		LastTargetOdour,
		LastTargetOdourIntensity,
		LastTargetOdourIntensityNet,
		LastTargetOdourIntensityByDistance,
		LastTargetOdourRange,

		// TARGET
		TargetGameObject,
		TargetName,
		TargetEntityType,
		TargetTime,
		TargetTimeTotal,
		TargetAge,
		TargetHasParent,
		TargetParentName,
		TargetZoneName,

		TargetIsActive,
		TargetIsLastTarget,
		TargetHasOwnerActiveSelected,
		TargetIsDestroyed,
		TargetDurability,
		TargetDurabilityInPercent,
		TargetActiveCounterpartsLimit,
		TargetIsInFieldOfView,
		TargetIsVisible,
		TargetIsAudible,
		TargetIsSmellable,
		TargetVisibilityByDistance,
		TargetAudibilityByDistance,
		TargetSmellabilityByDistance,

		TargetDistance,
		TargetOffsetPositionDistance,
		TargetMovePositionDistance,
		TargetLastKnownPositionDistance,

		TargetOdour,
		TargetOdourIntensity,
		TargetOdourIntensityNet,
		TargetOdourIntensityByDistance,
		TargetOdourRange,

		TargetSlot0Amount,
		TargetSlot1Amount,
		TargetSlot2Amount,
		TargetSlot3Amount,
		TargetSlot4Amount,
		TargetSlot5Amount,
		TargetSlot6Amount,
		TargetSlot7Amount,
		TargetSlot8Amount,
		TargetSlot9Amount,

		TargetSlot0MaxAmount,
		TargetSlot1MaxAmount,
		TargetSlot2MaxAmount,
		TargetSlot3MaxAmount,
		TargetSlot4MaxAmount,
		TargetSlot5MaxAmount,
		TargetSlot6MaxAmount,
		TargetSlot7MaxAmount,
		TargetSlot8MaxAmount,
		TargetSlot9MaxAmount,

		CreatureActiveTargetGameObject,
		CreatureActiveTargetTime,
		CreatureActiveTargetTimeTotal,
		CreatureBehaviour,
		CreatureCommand,
		CreatureAltitude,

		CreatureEnvTemperatureDeviation,
		CreatureFitness,
		CreatureHealth,
		CreatureStamina,
		CreaturePower,
		CreatureDamage,
		CreatureStress,
		CreatureDebility,
		CreatureHunger,
		CreatureThirst,
		CreatureAggressivity,
		CreatureExperience,
		CreatureAnxiety,
		CreatureNosiness,

		CreatureVisualSense,
		CreatureAuditorySense,
		CreatureOlfactorySense,
		CreatureGustatorySense,
		CreatureTactileSense,

		CreaturePosition,
		CreatureIsDead,
		CreatureIsInjured,
		CreatureIsInjuredOrDead,
		CreatureIsSheltered,
		CreatureIsIndoor,
		CreatureIsGrounded,

		CreatureGenderType,
		CreatureTrophicLevel,

		EnvironmentTimeHour,
		EnvironmentTimeMinute,
		EnvironmentTimeSecond,
		EnvironmentDateYear,
		EnvironmentDateMonth,
		EnvironmentDateDay,
		EnvironmentTemperature,
		EnvironmentWeather,

		SystemInputKey,
		SystemInputAxis,
		SystemUIToggle,
		SystemUIButton,

		None // last key
	}


	// *** T ***

	/// <summary>
	/// Trophic level type.
	/// </summary>
	public enum TrophicLevelType
	{
		UNDEFINED=0,
		HERBIVORE,
		OMNIVORES,
		CARNIVORE
	}

	/// <summary>
	/// Target access type.
	/// </summary>
	public enum TargetAccessType
	{
		OBJECT,
		NAME,
		TAG,
		TYPE
	}

	/// <summary>
	/// Target move position type.
	/// </summary>
	public enum TargetMovePositionType
	{
		LastKnownPosition,
		Offset,
		Range,
		Cover,
		OtherTarget
	}

	/// <summary>
	/// Target selection type.
	/// </summary>
	public enum TargetSelectionType
	{
		OBJECT,
		NAME,
		TAG
	}

	/// <summary>
	/// Target selector position type.
	/// </summary>
	public enum TargetSelectorPositionType
	{
		TargetMovePosition = 0,
		TargetMaxRange,
		ActiveTargetMovePosition,
		ActiveTargetMaxRange,
		HomeTargetMovePosition,
		HomeTargetMaxRange,
		OutpostTargetMovePosition,
		OutpostTargetMaxRange,
		EscortTargetMovePosition,
		EscortTargetMaxRange,
		PatrolTargetMovePosition,
		PatrolTargetMaxRange
	}

	/// <summary>
	/// Target type.
	/// </summary>
	public enum TargetType
	{
		UNDEFINED,
		HOME,
		OUTPOST,
		ESCORT,
		PATROL,
		WAYPOINT,
		INTERACTOR,
		ITEM,
		PLAYER,
		CREATURE
	}

	// *** U ***
	// *** V ***

	//TODO [System.Obsolete ("Use Angular instead")]

	/// <summary>
	/// Velocity type.
	/// </summary>
	public enum VelocityType
	{
		DEFAULT = 0,
		ADVANCED
	}

	/// <summary>
	/// Viewing direction type.
	/// </summary>
	public enum ViewingDirectionType
	{
		DEFAULT = 0,
		OFFSET,
		MOVE,
		CENTER,
		POSITION,
		NONE
	}

	// *** W ***

	/// <summary>
	/// Waypoint order type.
	/// </summary>
	public enum WaypointOrderType //TODO: SequenceOrderType
	{
		PINGPONG,
		CYCLE,
		RANDOM
	}

	// *** XYZ ***



	/*
	public enum AnimatorParameterType
	{
		Float = 1,
		Int = 3,
		Bool,
		Trigger = 9
	}

	public enum NavMeshType
	{
		NONE,
		PERMANENT,
		AVOIDANCE,
		COLLISION,
		DEADLOCKED
	}

	public enum CollisionType
	{
		NONE,
		TERRAIN,
		MESH,
		UNKNOWN
	}

	public enum MissionType
	{
		HOME,
		ESCORT,
		PATROL
	}

	public enum MoveCompleteType
	{
		DEFAULT,
		NEXTRULE,
		CHANGE_MODE,
		FORCE_SENSE,
		FORCE_REACT
	}
	*/

}


