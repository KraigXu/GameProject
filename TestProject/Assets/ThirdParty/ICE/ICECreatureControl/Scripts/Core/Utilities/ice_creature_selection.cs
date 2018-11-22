// ##############################################################################
//
// ice_creature_selection.cs | SelectionTools
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
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Utilities
{
	public static class SelectionTools{

		public static bool ExpressionContains( string _type, SelectionExpressionType _expression ){
			return ( _expression.ToString().IndexOf( _type ) == 0 ? true : false );
		}

		public static SelectionExpressionType Type( ref SelectionExpressionType _type, string _key )
		{
			if( _type.ToString() != _key )
				_type = StrToType( _key );

			return _type;
		}

		public static SelectionExpressionType StrToType( string _key )
		{			
			int _count = (int)SelectionExpressionType.None; // last one ...
			for( int _i = 0 ; _i <= _count; _i++ )
			{
				SelectionExpressionType _type = (SelectionExpressionType)(_i);
				if( _type.ToString() == _key )
					return _type;
			}

			return SelectionExpressionType.OwnGameObject;
		}

		public static string TypeToStr( SelectionExpressionType _type ){
			return _type.ToString();
		}



		public static string[] ToArray(){
			int _count = (int)SelectionExpressionType.EnvironmentTemperature;

			string[] _array = new string[_count];
			for( int _i = 0 ; _i <= _count; _i++ )
			{
				SelectionExpressionType _type = (SelectionExpressionType)(_i);
				_array[_i] = _type.ToString();
			}

			return _array;
		}

		public static bool IsObjectValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.OBJECT )
				return true;
			else
				return false;
		}

		public static bool IsNumericValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.NUMBER ||
				DataType( _type ) == SelectionExpressionDataType.DYNAMICNUMBER )
				return true;
			else
				return false;
		}

		public static bool IsEnumValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.ENUM )
				return true;
			else
				return false;
		}

		public static bool IsStringValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.STRING )
				return true;
			else
				return false;
		}

		public static bool IsKeyCodeValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.KEYCODE )
				return true;
			else
				return false;
		}

		public static bool IsAxisValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.AXIS )
				return true;
			else
				return false;
		}

		public static bool IsUIToggleValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.TOGGLE )
				return true;
			else
				return false;
		}

		public static bool IsUIButtonValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.BUTTON )
				return true;
			else
				return false;
		}

		public static bool IsBooleanValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.BOOLEAN )
				return true;
			else
				return false;
		}

		public static bool IsDynamicValue( SelectionExpressionType _type )
		{
			if( DataType( _type ) == SelectionExpressionDataType.DYNAMICNUMBER )
				return true;
			else
				return false;
		}

		public static bool NeedLogicalOperator( SelectionExpressionType _type )
		{
			if( IsNumericValue( _type ) )
				return true;
			else
				return false;
		}

		public static SelectionExpressionDataType DataType( SelectionExpressionType _type )
		{
			switch( _type )
			{
			// DYNAMIC NUMBERS
			case SelectionExpressionType.OwnAge:
			case SelectionExpressionType.OwnerAltitude:
			case SelectionExpressionType.OwnOdourIntensity:
			case SelectionExpressionType.OwnOdourRange:
			case SelectionExpressionType.OwnDamage:
			case SelectionExpressionType.OwnDebility:
			case SelectionExpressionType.OwnFitness:
			case SelectionExpressionType.OwnHealth:
			case SelectionExpressionType.OwnHunger:
			case SelectionExpressionType.OwnPower:
			case SelectionExpressionType.OwnStamina:
			case SelectionExpressionType.OwnStress:
			case SelectionExpressionType.OwnThirst:
			case SelectionExpressionType.OwnAggressivity:
			case SelectionExpressionType.OwnExperience:
			case SelectionExpressionType.OwnAnxiety:
			case SelectionExpressionType.OwnNosiness:
			case SelectionExpressionType.OwnVisualSense:
			case SelectionExpressionType.OwnAuditorySense:
			case SelectionExpressionType.OwnOlfactorySense:
			case SelectionExpressionType.OwnGustatorySense:
			case SelectionExpressionType.OwnTactileSense:
			case SelectionExpressionType.OwnHomeDistance:

			case SelectionExpressionType.OwnSlot0Amount:
			case SelectionExpressionType.OwnSlot1Amount:
			case SelectionExpressionType.OwnSlot2Amount:
			case SelectionExpressionType.OwnSlot3Amount:
			case SelectionExpressionType.OwnSlot4Amount:
			case SelectionExpressionType.OwnSlot5Amount:
			case SelectionExpressionType.OwnSlot6Amount:
			case SelectionExpressionType.OwnSlot7Amount:
			case SelectionExpressionType.OwnSlot8Amount:
			case SelectionExpressionType.OwnSlot9Amount:

			case SelectionExpressionType.OwnSlot0MaxAmount:
			case SelectionExpressionType.OwnSlot1MaxAmount:
			case SelectionExpressionType.OwnSlot2MaxAmount:
			case SelectionExpressionType.OwnSlot3MaxAmount:
			case SelectionExpressionType.OwnSlot4MaxAmount:
			case SelectionExpressionType.OwnSlot5MaxAmount:
			case SelectionExpressionType.OwnSlot6MaxAmount:
			case SelectionExpressionType.OwnSlot7MaxAmount:
			case SelectionExpressionType.OwnSlot8MaxAmount:
			case SelectionExpressionType.OwnSlot9MaxAmount:



			case SelectionExpressionType.OwnEnvTemperatureDeviation:

			case SelectionExpressionType.TargetAge:		
			case SelectionExpressionType.TargetActiveCounterpartsLimit:
			case SelectionExpressionType.TargetDurability:
			case SelectionExpressionType.TargetDurabilityInPercent:
			case SelectionExpressionType.TargetSlot0Amount:
			case SelectionExpressionType.TargetSlot1Amount:
			case SelectionExpressionType.TargetSlot2Amount:
			case SelectionExpressionType.TargetSlot3Amount:
			case SelectionExpressionType.TargetSlot4Amount:
			case SelectionExpressionType.TargetSlot5Amount:
			case SelectionExpressionType.TargetSlot6Amount:
			case SelectionExpressionType.TargetSlot7Amount:
			case SelectionExpressionType.TargetSlot8Amount:
			case SelectionExpressionType.TargetSlot9Amount:
			case SelectionExpressionType.TargetSlot0MaxAmount:
			case SelectionExpressionType.TargetSlot1MaxAmount:
			case SelectionExpressionType.TargetSlot2MaxAmount:
			case SelectionExpressionType.TargetSlot3MaxAmount:
			case SelectionExpressionType.TargetSlot4MaxAmount:
			case SelectionExpressionType.TargetSlot5MaxAmount:
			case SelectionExpressionType.TargetSlot6MaxAmount:
			case SelectionExpressionType.TargetSlot7MaxAmount:
			case SelectionExpressionType.TargetSlot8MaxAmount:
			case SelectionExpressionType.TargetSlot9MaxAmount:
			case SelectionExpressionType.TargetVisibilityByDistance:
			case SelectionExpressionType.TargetAudibilityByDistance:
			case SelectionExpressionType.TargetSmellabilityByDistance:

			case SelectionExpressionType.ActiveTargetAge:	
			case SelectionExpressionType.ActiveTargetDurability:
			case SelectionExpressionType.ActiveTargetDurabilityInPercent:
			case SelectionExpressionType.ActiveTargetActiveCounterpartsLimit:
			case SelectionExpressionType.ActiveTargetSlot0Amount:
			case SelectionExpressionType.ActiveTargetSlot1Amount:
			case SelectionExpressionType.ActiveTargetSlot2Amount:
			case SelectionExpressionType.ActiveTargetSlot3Amount:
			case SelectionExpressionType.ActiveTargetSlot4Amount:
			case SelectionExpressionType.ActiveTargetSlot5Amount:
			case SelectionExpressionType.ActiveTargetSlot6Amount:
			case SelectionExpressionType.ActiveTargetSlot7Amount:
			case SelectionExpressionType.ActiveTargetSlot8Amount:
			case SelectionExpressionType.ActiveTargetSlot9Amount:
			case SelectionExpressionType.ActiveTargetSlot0MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot1MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot2MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot3MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot4MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot5MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot6MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot7MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot8MaxAmount:
			case SelectionExpressionType.ActiveTargetSlot9MaxAmount:
			case SelectionExpressionType.ActiveTargetVisibilityByDistance:
			case SelectionExpressionType.ActiveTargetAudibilityByDistance:
			case SelectionExpressionType.ActiveTargetSmellabilityByDistance:

			case SelectionExpressionType.LastTargetAge:
			case SelectionExpressionType.LastTargetDurability:
			case SelectionExpressionType.LastTargetDurabilityInPercent:
			case SelectionExpressionType.LastTargetActiveCounterpartsLimit:
			case SelectionExpressionType.LastTargetSlot0Amount:
			case SelectionExpressionType.LastTargetSlot1Amount:
			case SelectionExpressionType.LastTargetSlot2Amount:
			case SelectionExpressionType.LastTargetSlot3Amount:
			case SelectionExpressionType.LastTargetSlot4Amount:
			case SelectionExpressionType.LastTargetSlot5Amount:
			case SelectionExpressionType.LastTargetSlot6Amount:
			case SelectionExpressionType.LastTargetSlot7Amount:
			case SelectionExpressionType.LastTargetSlot8Amount:
			case SelectionExpressionType.LastTargetSlot9Amount:
			case SelectionExpressionType.LastTargetSlot0MaxAmount:
			case SelectionExpressionType.LastTargetSlot1MaxAmount:
			case SelectionExpressionType.LastTargetSlot2MaxAmount:
			case SelectionExpressionType.LastTargetSlot3MaxAmount:
			case SelectionExpressionType.LastTargetSlot4MaxAmount:
			case SelectionExpressionType.LastTargetSlot5MaxAmount:
			case SelectionExpressionType.LastTargetSlot6MaxAmount:
			case SelectionExpressionType.LastTargetSlot7MaxAmount:
			case SelectionExpressionType.LastTargetSlot8MaxAmount:
			case SelectionExpressionType.LastTargetSlot9MaxAmount:
			case SelectionExpressionType.LastTargetVisibilityByDistance:
			case SelectionExpressionType.LastTargetAudibilityByDistance:
			case SelectionExpressionType.LastTargetSmellabilityByDistance:
				
			case SelectionExpressionType.CreatureAltitude:
			case SelectionExpressionType.CreatureDamage:
			case SelectionExpressionType.CreatureDebility:
			case SelectionExpressionType.CreatureFitness:
			case SelectionExpressionType.CreatureHealth:
			case SelectionExpressionType.CreatureHunger:
			case SelectionExpressionType.CreaturePower:
			case SelectionExpressionType.CreatureStamina:
			case SelectionExpressionType.CreatureStress:
			case SelectionExpressionType.CreatureThirst:
			case SelectionExpressionType.CreatureAggressivity:
			case SelectionExpressionType.CreatureExperience:
			case SelectionExpressionType.CreatureAnxiety:
			case SelectionExpressionType.CreatureNosiness:
			case SelectionExpressionType.CreatureVisualSense:
			case SelectionExpressionType.CreatureAuditorySense:
			case SelectionExpressionType.CreatureOlfactorySense:
			case SelectionExpressionType.CreatureGustatorySense:
			case SelectionExpressionType.CreatureTactileSense:
			case SelectionExpressionType.CreatureEnvTemperatureDeviation:

			case SelectionExpressionType.TargetOdourIntensity:
			case SelectionExpressionType.TargetOdourIntensityNet:
			case SelectionExpressionType.TargetOdourIntensityByDistance:
			case SelectionExpressionType.TargetOdourRange:

			case SelectionExpressionType.TargetDistance:
			case SelectionExpressionType.TargetOffsetPositionDistance:
			case SelectionExpressionType.TargetMovePositionDistance:
			case SelectionExpressionType.TargetLastKnownPositionDistance:
				return SelectionExpressionDataType.DYNAMICNUMBER;

				// STATIC NUMBERS
			case SelectionExpressionType.ActiveTargetTime:
			case SelectionExpressionType.ActiveTargetTimeTotal:

			case SelectionExpressionType.LastTargetTime:
			case SelectionExpressionType.LastTargetTimeTotal:

			case SelectionExpressionType.TargetTime:
			case SelectionExpressionType.TargetTimeTotal:

			case SelectionExpressionType.CreatureActiveTargetTime:
			case SelectionExpressionType.CreatureActiveTargetTimeTotal:

			case SelectionExpressionType.EnvironmentTimeHour:
			case SelectionExpressionType.EnvironmentTimeMinute:
			case SelectionExpressionType.EnvironmentTimeSecond:
			case SelectionExpressionType.EnvironmentDateYear:
			case SelectionExpressionType.EnvironmentDateMonth:
			case SelectionExpressionType.EnvironmentDateDay:
			case SelectionExpressionType.EnvironmentTemperature:
				return SelectionExpressionDataType.NUMBER;

				// STRINGS
			case SelectionExpressionType.OwnReceivedCommand:
			case SelectionExpressionType.OwnBehaviour:
			case SelectionExpressionType.ActiveTargetName:
			case SelectionExpressionType.ActiveTargetParentName:
			case SelectionExpressionType.LastTargetName:
			case SelectionExpressionType.LastTargetParentName:
			case SelectionExpressionType.TargetName:
			case SelectionExpressionType.TargetParentName:
			case SelectionExpressionType.CreatureBehaviour:
			case SelectionExpressionType.CreatureCommand:
				return SelectionExpressionDataType.STRING;

				// BOOLEAN
			case SelectionExpressionType.OwnerIsWithinHomeArea:
			case SelectionExpressionType.OwnerIsDead:
			case SelectionExpressionType.OwnerIsInjured:
			case SelectionExpressionType.OwnerIsGrounded:
			case SelectionExpressionType.OwnerIsSheltered:
			case SelectionExpressionType.OwnerIsIndoor:
			case SelectionExpressionType.OwnerIsSelectedByTarget:
			case SelectionExpressionType.CreatureIsDead:
			case SelectionExpressionType.CreatureIsInjured:
			case SelectionExpressionType.CreatureIsInjuredOrDead:
			case SelectionExpressionType.CreatureIsGrounded:
			case SelectionExpressionType.CreatureIsSheltered:
			case SelectionExpressionType.CreatureIsIndoor:
			case SelectionExpressionType.TargetHasParent:
			case SelectionExpressionType.ActiveTargetHasParent:
			case SelectionExpressionType.LastTargetHasParent:
				
			case SelectionExpressionType.TargetIsDestroyed:
			case SelectionExpressionType.ActiveTargetIsDestroyed:
			case SelectionExpressionType.LastTargetIsDestroyed:

			case SelectionExpressionType.TargetHasOwnerActiveSelected:
			case SelectionExpressionType.ActiveTargetHasOwnerActiveSelected:
			case SelectionExpressionType.LastTargetHasOwnerActiveSelected:

			case SelectionExpressionType.TargetIsActive:
			case SelectionExpressionType.TargetIsLastTarget:
				
			case SelectionExpressionType.TargetIsInFieldOfView:
			case SelectionExpressionType.TargetIsVisible:
			case SelectionExpressionType.TargetIsAudible:
			case SelectionExpressionType.TargetIsSmellable:

			case SelectionExpressionType.LastTargetIsInFieldOfView:
			case SelectionExpressionType.LastTargetIsVisible:
			case SelectionExpressionType.LastTargetIsAudible:
			case SelectionExpressionType.LastTargetIsSmellable:

			case SelectionExpressionType.ActiveTargetIsInFieldOfView:
			case SelectionExpressionType.ActiveTargetIsVisible:
			case SelectionExpressionType.ActiveTargetIsAudible:
			case SelectionExpressionType.ActiveTargetIsSmellable:
				return SelectionExpressionDataType.BOOLEAN;

				// ENUM
			case SelectionExpressionType.TargetEntityType:
			case SelectionExpressionType.ActiveTargetEntityType:
			case SelectionExpressionType.LastTargetEntityType:
			case SelectionExpressionType.OwnGenderType:
			case SelectionExpressionType.OwnTrophicLevel:
			case SelectionExpressionType.CreatureGenderType:
			case SelectionExpressionType.CreatureTrophicLevel:
				
			case SelectionExpressionType.OwnOdour:
			case SelectionExpressionType.TargetOdour:
			case SelectionExpressionType.ActiveTargetOdour:
			case SelectionExpressionType.LastTargetOdour:
				
			case SelectionExpressionType.EnvironmentWeather:
				return SelectionExpressionDataType.ENUM;

				// OBJECTS
			case SelectionExpressionType.OwnGameObject:
			case SelectionExpressionType.TargetGameObject:
			case SelectionExpressionType.ActiveTargetGameObject:
			case SelectionExpressionType.LastTargetGameObject:
			case SelectionExpressionType.CreatureActiveTargetGameObject:
				return SelectionExpressionDataType.OBJECT;

				// KEYCODE
			case SelectionExpressionType.SystemInputKey:
				return SelectionExpressionDataType.KEYCODE;

				// AXIS
			case SelectionExpressionType.SystemInputAxis:
				return SelectionExpressionDataType.AXIS;

			// TOGGLE
			case SelectionExpressionType.SystemUIToggle:
				return SelectionExpressionDataType.TOGGLE;

			// BUTTON
			case SelectionExpressionType.SystemUIButton:
				return SelectionExpressionDataType.BUTTON;

			default:
				return SelectionExpressionDataType.UNDEFINED;
			}
		}
	}

}
