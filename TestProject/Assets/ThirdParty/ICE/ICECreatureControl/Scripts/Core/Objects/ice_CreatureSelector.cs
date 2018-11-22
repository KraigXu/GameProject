// ##############################################################################
//
// ice_CreatureSelector.cs
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
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{

	[System.Serializable]
	public class SelectorObject : ICEObject
	{
		protected GameObject Owner = null;
		protected ICEWorldBehaviour OwnerComponent = null;
		protected ICECreatureEntity m_OwnerEntityComponent = null;
		protected CreatureObject m_Creature = null;
		protected MoveObject m_Move = null;
		protected BehaviourObject m_Behaviour = null;
		protected StatusObject m_Status = null;
		protected TargetObject m_ActiveTarget = null;
		protected TargetObject m_PreviousTarget = null;


		public SelectorObject( CreatureObject _creature )
		{
			Init( _creature );
		}

		public SelectorObject( ICEWorldBehaviour _behaviour )
		{
			ICECreatureCharacter _character = _behaviour as ICECreatureCharacter;
			if( _character == null )
				return;
			
			Init( _character.Creature );
		}

		private void Init( CreatureObject _creature )
		{
			if( _creature == null )
				return;
			
			Owner = _creature.Owner;
			OwnerComponent = _creature.OwnerComponent;
			m_OwnerEntityComponent = OwnerComponent as ICECreatureEntity;
			m_Creature = _creature;
			m_Move = _creature.Move;
			m_Behaviour = _creature.Behaviour;
			m_Status = _creature.Status;
			m_ActiveTarget = _creature.ActiveTarget;
			m_PreviousTarget = _creature.PreviousTarget;
		}
			
		public GameObject SelectBestGameObject( TargetObject _master_target, GameObject[] _objects )
		{
			int _counterparts_limit = _master_target.Selectors.Preselection.ActiveCounterpartsLimit;

			float _best_distance = Mathf.Infinity;		
			int _best_counterparts = _counterparts_limit;
			GameObject _best_object = null;

			TargetObject _clone = new TargetObject( _master_target, _master_target.OwnerComponent );
			for( int _i = 0 ; _i < _objects.Length ; _i++ )
			{				
				_clone.TestOverrideTargetGameObject( _objects[_i] );

				if( _clone.Selectors.Preselection.UseChildObjects || _objects[_i].transform.IsChildOf( Owner.transform ) == false )
				{
					ICECreatureEntity _entity = _clone.EntityComponent;
					int _entity_counterparts = ( _entity != null ? _entity.ActiveCounterparts.Count : _best_counterparts );

					if( _entity != null && _entity.ActiveCounterpartExists( OwnerComponent as ICECreatureEntity ) )
						_entity_counterparts--;

					if( _counterparts_limit == -1 || _entity_counterparts <= _best_counterparts )
					{
						if( CheckTarget( _clone ) )
						{
							float _distance = PositionTools.Distance( Owner.transform.position, _objects[_i].transform.position );
							if( _distance < _best_distance )
							{
								_best_distance = _distance;
								_best_object = _objects[_i];
								_best_counterparts = ( _entity != null ? _entity_counterparts : _best_counterparts );
							}
						}
					}
				}
			}

			return _best_object;
		}

		public TargetObject SelectBestTarget( List<TargetObject> _targets )
		{
			TargetObject _favourite_target = null;
			foreach( TargetObject _target in _targets )
			{
				if( CheckTarget( _target ) )
				{
					if( ( _favourite_target == null ) ||
						( _target.Selectors.Priority > _favourite_target.Selectors.Priority ) ||
						(
							_target.Selectors.Priority == _favourite_target.Selectors.Priority &&
							_target.TargetMovePositionDistanceTo( Owner.transform.position ) < _favourite_target.TargetMovePositionDistanceTo( Owner.transform.position )
						) )
					{
						_favourite_target = _target;							
					}
				}
			}

			return _favourite_target;
		}

		public bool CheckTarget( TargetObject _target )
		{
			if( _target == null || ! _target.IsValidAndReady )
				return false;
			
			_target.Selectors.ResetStatus();

			_target.Selectors.IsValid = CheckBasicConditions( _target ); 
			_target.Selectors.TotalCheckIsValid = CheckAdvancedConditions( _target, _target.Selectors.IsValid );
			_target.Selectors.TotalCheckIsValid = CheckTimeConditions( _target, _target.Selectors.TotalCheckIsValid );

			return _target.Selectors.TotalCheckIsValid;
		}

		public bool CheckBasicConditions( TargetObject _target ){
			return _target.TargetBasicCheckComplied( m_Creature );
		}

		public bool CheckTimeConditions( TargetObject _target, bool _final_result ){
			return _target.TargetTimeCheckComplied( m_Creature, _final_result );
		}

		public bool CheckAdvancedConditions( TargetObject _target, bool _final_result )
		{
			if( _target == null || _target.IsValidAndReady == false || _target.Selectors.UseAdvanced == false )
				return _final_result;

			_target.Selectors.AdvancedCheckIsValid = _final_result;
			_target.Selectors.AdvancedOperatorType = ConditionalOperatorType.AND;

			foreach( SelectionConditionGroupObject _condition_group in _target.Selectors.ConditionGroups )
			{
				if( _final_result == false && _condition_group.InitialOperatorType == ConditionalOperatorType.AND )
					continue;

				_condition_group.ResetStatus();

				if( _condition_group.Enabled == false )
					continue;

				foreach( SelectionConditionObject _condition in _condition_group.Conditions )
				{
					_condition.ResetStatus();
					if( _condition.Enabled == false )
						continue;

					if( SelectionTools.IsEnumValue( _condition.ExpressionType ) )
							_condition.IsValid = CompareEnumValue( _target, _condition );

					else if( SelectionTools.IsNumericValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareNumericValue( _target, _condition );

					else if( SelectionTools.IsObjectValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareObjects( _target, _condition );

					else if( SelectionTools.IsStringValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareStringValue( _target, _condition );

					else if( SelectionTools.IsKeyCodeValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareKeyCodeValue( _target, _condition );

					else if( SelectionTools.IsAxisValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareAxisValue( _target, _condition );

					else if( SelectionTools.IsUIToggleValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareToggleValue( _target, _condition );

					else if( SelectionTools.IsUIButtonValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareButtonValue( _target, _condition );

					else if( SelectionTools.IsBooleanValue( _condition.ExpressionType ) )
						_condition.IsValid = CompareBooleanValue( _target, _condition );
					/*
					// TIME
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentTimeHour )
						_condition.IsValid = CompareNumber( CreatureRegister.EnvironmentInfos.TimeHour, _condition.FloatValue, _condition.Operator );
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentTimeMinute )
						_condition.IsValid = CompareNumber( CreatureRegister.EnvironmentInfos.TimeMinutes, _condition.FloatValue, _condition.Operator );
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentTimeSecond )
						_condition.IsValid = CompareNumber( CreatureRegister.EnvironmentInfos.TimeSeconds, _condition.FloatValue, _condition.Operator );

					// DATE
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentDateYear )
						_condition.IsValid = CompareNumber( CreatureRegister.EnvironmentInfos.DateYear, _condition.FloatValue, _condition.Operator );
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentDateMonth )
						_condition.IsValid = CompareNumber( CreatureRegister.EnvironmentInfos.DateMonth, _condition.FloatValue, _condition.Operator );
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentDateDay )
						_condition.IsValid = CompareNumber( CreatureRegister.EnvironmentInfos.DateDay, _condition.FloatValue, _condition.Operator );


					// TEMPREATURE
					else if( _condition.ExpressionType == TargetSelectorExpressionType.EnvironmentTemperature )
						_condition.IsValid = CompareNumber( ICECreatureRegister.Instance.EnvironmentInfos.Temperature, _condition.FloatValue, _condition.Operator );
					*/

					// CREATURE BEHAVIOUR
					else if( _condition.ExpressionType == SelectionExpressionType.OwnBehaviour )
					{
						if( m_Behaviour.ActiveBehaviourModeKey == _condition.StringValue )
							_condition.IsValid = true;

						if( _condition.Operator == LogicalOperatorType.NOT )
							_condition.IsValid = ! _condition.IsValid;

					}

					else if( 
						_condition.ExpressionType == SelectionExpressionType.TargetZoneName ||
						_condition.ExpressionType == SelectionExpressionType.OwnZoneName )
					{
						if( _condition.ExpressionType == SelectionExpressionType.OwnZoneName )
							_condition.IsValid = (m_OwnerEntityComponent != null ? m_OwnerEntityComponent.IsInZone( _condition.StringValue ) :false );
						else if( _condition.ExpressionType == SelectionExpressionType.TargetZoneName )
							_condition.IsValid = (_target != null && _target.EntityComponent != null ? _target.EntityComponent.IsInZone( _condition.StringValue ) :false );
						
						if( _condition.Operator == LogicalOperatorType.NOT )
							_condition.IsValid = ! _condition.IsValid;
					}

					else if( _condition.ExpressionType == SelectionExpressionType.OwnerPosition )
					{
						switch( _condition.PositionType )
						{
							case TargetSelectorPositionType.TargetMovePosition:
								_condition.IsValid = _target.TargetMovePositionReached( Owner.transform.position );
								break;
							case TargetSelectorPositionType.TargetMaxRange:
								_condition.IsValid =_target.TargetInMaxRange( Owner.transform.position );
								break;
							case TargetSelectorPositionType.ActiveTargetMovePosition:
								_condition.IsValid = (m_ActiveTarget != null?m_ActiveTarget.TargetMovePositionReached( Owner.transform.position):false );
								break;
							case TargetSelectorPositionType.ActiveTargetMaxRange:
								_condition.IsValid = (m_ActiveTarget != null?m_ActiveTarget.TargetInMaxRange( Owner.transform.position ):false );
								break;
							case TargetSelectorPositionType.HomeTargetMovePosition:
								_condition.IsValid = (m_Creature.Essentials.Target != null?m_Creature.Essentials.Target.TargetMovePositionReached( Owner.transform.position):false );
								break;
							case TargetSelectorPositionType.HomeTargetMaxRange:
								_condition.IsValid = (m_Creature.Essentials.Target != null?m_Creature.Essentials.Target.TargetInMaxRange( Owner.transform.position ):false );
								break;
							case TargetSelectorPositionType.OutpostTargetMovePosition:
								_condition.IsValid = (m_Creature.Missions.Outpost.Target != null?m_Creature.Missions.Outpost.Target.TargetMovePositionReached( Owner.transform.position):false );
								break;
							case TargetSelectorPositionType.OutpostTargetMaxRange:
								_condition.IsValid = (m_Creature.Missions.Outpost.Target != null?m_Creature.Missions.Outpost.Target.TargetInMaxRange( Owner.transform.position ):false );
								break;
							case TargetSelectorPositionType.EscortTargetMovePosition:
								_condition.IsValid = (m_Creature.Missions.Escort.Target != null?m_Creature.Missions.Escort.Target.TargetMovePositionReached( Owner.transform.position):false );
								break;
							case TargetSelectorPositionType.EscortTargetMaxRange:
								_condition.IsValid = (m_Creature.Missions.Escort.Target != null?m_Creature.Missions.Escort.Target.TargetInMaxRange( Owner.transform.position ):false );
								break;
							case TargetSelectorPositionType.PatrolTargetMovePosition:
								_condition.IsValid = (m_Creature.Missions.Patrol.Target != null?m_Creature.Missions.Patrol.Target.TargetMovePositionReached( Owner.transform.position):false );
								break;
							case TargetSelectorPositionType.PatrolTargetMaxRange:
								_condition.IsValid = (m_Creature.Missions.Patrol.Target != null?m_Creature.Missions.Patrol.Target.TargetInMaxRange( Owner.transform.position ):false );
								break;
						}

						if( _condition.Operator == LogicalOperatorType.NOT )
							_condition.IsValid = ! _condition.IsValid;

						if( _condition.IsValid && _condition.UseUpdateLastPosition )
							_target.Move.LastKnownPosition = _target.TargetTransformPosition;

					}

					if( _condition_group.Status == SelectionStatus.UNCHECKED )
						_condition_group.IsValid = _condition.IsValid;
					else if( _condition.ConditionType == ConditionalOperatorType.AND && _condition_group.IsValid == true )
						_condition_group.IsValid = _condition.IsValid;
					else if( _condition.ConditionType == ConditionalOperatorType.OR && _condition_group.IsValid == false )
						_condition_group.IsValid = _condition.IsValid;

					if( _condition_group.IsValid && _condition_group.UseUpdateLastPosition )
						_target.Move.LastKnownPosition = _target.TargetTransformPosition;
				}

				if( _condition_group.Status == SelectionStatus.UNCHECKED )
					continue;

				if( _target.Selectors.ConditionGroups.IndexOf( _condition_group ) == 0 )
				{
					if( _condition_group.InitialOperatorType == ConditionalOperatorType.AND && _final_result == true )
						_final_result = _condition_group.IsValid;
					else if( _condition_group.InitialOperatorType == ConditionalOperatorType.OR && _final_result == false )
						_final_result = _condition_group.IsValid;

					_target.Selectors.AdvancedCheckIsValid = _condition_group.IsValid; 
					_target.Selectors.AdvancedOperatorType = _condition_group.InitialOperatorType;
				}
				else if( _condition_group.InitialOperatorType == ConditionalOperatorType.AND && _target.Selectors.AdvancedCheckIsValid == true )
				{
					_target.Selectors.AdvancedCheckIsValid = _condition_group.IsValid; 
					_target.Selectors.AdvancedOperatorType = _condition_group.InitialOperatorType;
				}
				else if( _condition_group.InitialOperatorType == ConditionalOperatorType.OR && _target.Selectors.AdvancedCheckIsValid == false )
				{
					_target.Selectors.AdvancedCheckIsValid = _condition_group.IsValid; 
					_target.Selectors.AdvancedOperatorType = _condition_group.InitialOperatorType;
				}
			}

			if( _target.Selectors.AdvancedOperatorType == ConditionalOperatorType.AND && _final_result == true )
				_final_result = _target.Selectors.AdvancedCheckIsValid;
			else if( _target.Selectors.AdvancedOperatorType == ConditionalOperatorType.OR && _final_result == false )
				_final_result = _target.Selectors.AdvancedCheckIsValid;

			return _final_result;
		}

		private GameObject GetGameObject( TargetObject _target, SelectionExpressionType _type )
		{
			if( _target == null )
				return null;

			switch( _type )
			{
			// CREATURE 
			case SelectionExpressionType.OwnGameObject:
				return ( Owner != null)?Owner.gameObject:null;

				// TARGET  
			case SelectionExpressionType.TargetGameObject:
				return _target.TargetGameObject;

			case SelectionExpressionType.ActiveTargetGameObject:
				return ( m_ActiveTarget != null ) ? m_ActiveTarget.TargetGameObject : null;
			
				// TARGET CREATURE ACTIVE TARGET
			case SelectionExpressionType.CreatureActiveTargetGameObject:
				return (_target.EntityCreature != null && _target.EntityCreature.Creature.ActiveTarget != null )?_target.EntityCreature.Creature.ActiveTarget.TargetGameObject:null;

			}

			return null;
		}

		private string GetStringValue( TargetObject _target, SelectionExpressionType _type )
		{
			switch( _type )
			{
			// CREATURE BEHAVIOUR
			case SelectionExpressionType.OwnBehaviour:
				return m_Behaviour.ActiveBehaviourModeKey;

				// CREATURE COMMAND
			case SelectionExpressionType.OwnReceivedCommand:
				return ( m_OwnerEntityComponent != null ? m_OwnerEntityComponent.Message.LastReceivedCommand:"" );

				// ACTIVE TARGET NAME
			case SelectionExpressionType.ActiveTargetName:
				return ( m_ActiveTarget != null?m_ActiveTarget.TargetName:"" );

			// LAST TARGET NAME
			case SelectionExpressionType.LastTargetName:
				return ( m_PreviousTarget != null?m_PreviousTarget.TargetName:"" );

			case SelectionExpressionType.ActiveTargetParentName:
				return ( m_ActiveTarget != null?m_ActiveTarget.TargetParentName:"" );

			case SelectionExpressionType.TargetName:
				return ( _target != null?_target.TargetName:"" );

			case SelectionExpressionType.TargetParentName:
				return ( _target != null ?_target.TargetParentName:"" );



				// CREATURE BEHAVIOUR
			case SelectionExpressionType.CreatureBehaviour:
				return (_target.EntityCreature != null )?_target.EntityCreature.Creature.Behaviour.ActiveBehaviourModeKey:"";

				// CREATURE BEHAVIOUR
			case SelectionExpressionType.CreatureCommand:
				return (_target.EntityComponent != null )?_target.EntityComponent.Message.LastReceivedCommand:"";

			}

			return "";
		}

		private bool GetBooleanValue( TargetObject _target, SelectionExpressionType _type )
		{
			switch( _type )
			{
			// CREATURE BEHAVIOUR
			case SelectionExpressionType.OwnerIsDead:
				return m_Status.IsDead;

			case SelectionExpressionType.OwnerIsInjured:
				return m_Status.ReposeRequired;

			case SelectionExpressionType.OwnerIsGrounded:
				return m_Move.IsGrounded;

			case SelectionExpressionType.OwnerIsSheltered:
				return m_Status.IsSheltered;

			case SelectionExpressionType.OwnerIsIndoor:
				return m_Status.IsIndoor;

			case SelectionExpressionType.OwnerIsWithinHomeArea:
				return ( _target.EntityCreature != null ? _target.EntityCreature.Creature.Essentials.Target.TargetInMaxRange( Owner.transform.position ): false );


			case SelectionExpressionType.OwnerIsSelectedByTarget:				
				return ( m_OwnerEntityComponent != null ? m_OwnerEntityComponent.ActiveCounterpartExists( _target.EntityComponent ) : false );

				// CREATURE BEHAVIOUR
			case SelectionExpressionType.CreatureIsDead:
				return (_target.EntityCreature != null )?_target.EntityCreature.Creature.Status.IsDead:false;

			case SelectionExpressionType.CreatureIsInjured:
				return (_target.EntityCreature != null && _target.EntityCreature.Creature.Status.IsDead == false ? _target.EntityCreature.Creature.Status.ReposeRequired : false );

			case SelectionExpressionType.CreatureIsInjuredOrDead:
				return (_target.EntityCreature != null && _target.EntityCreature.Creature.Status.IsDead || _target.EntityCreature.Creature.Status.ReposeRequired ? true :false );

			case SelectionExpressionType.CreatureIsGrounded:
				return (_target.EntityCreature != null )?_target.EntityCreature.Creature.Move.IsGrounded:false;

			case SelectionExpressionType.CreatureIsSheltered:
				return (_target.EntityCreature != null )?_target.EntityCreature.Creature.Status.IsSheltered:false;

			case SelectionExpressionType.CreatureIsIndoor:
				return (_target.EntityCreature != null )?_target.EntityCreature.Creature.Status.IsIndoor:false;

			case SelectionExpressionType.ActiveTargetHasParent:
				return  (m_ActiveTarget != null && m_ActiveTarget.TargetGameObject != null && m_ActiveTarget.TargetGameObject.transform.parent != null )?true:false;

			case SelectionExpressionType.TargetHasParent:
				return (_target != null && _target.TargetGameObject != null && _target.TargetGameObject.transform.parent != null )?true:false;

			case SelectionExpressionType.TargetIsInFieldOfView:
				return (_target != null )?_target.TargetInFieldOfView( m_Status ):false;
			case SelectionExpressionType.ActiveTargetIsInFieldOfView:
				return ( m_ActiveTarget != null )? m_ActiveTarget.TargetInFieldOfView( m_Status ):false;
			case SelectionExpressionType.LastTargetIsInFieldOfView:
				return ( m_PreviousTarget != null )? m_PreviousTarget.TargetInFieldOfView( m_Status ):false;

			case SelectionExpressionType.TargetIsVisible:
				return (_target != null )?_target.TargetIsVisible( m_Status ):false;
			case SelectionExpressionType.ActiveTargetIsVisible:
				return ( m_ActiveTarget != null )? m_ActiveTarget.TargetIsVisible( m_Status ):false;
			case SelectionExpressionType.LastTargetIsVisible:
				return ( m_PreviousTarget != null )? m_PreviousTarget.TargetIsVisible( m_Status ):false;

			case SelectionExpressionType.TargetIsAudible:
				return (_target != null )?_target.TargetIsAudible( m_Status ):false;
			case SelectionExpressionType.ActiveTargetIsAudible:
				return ( m_ActiveTarget != null )? m_ActiveTarget.TargetIsAudible( m_Status ):false;
			case SelectionExpressionType.LastTargetIsAudible:
				return ( m_PreviousTarget != null )? m_PreviousTarget.TargetIsAudible( m_Status ):false;

			case SelectionExpressionType.TargetIsSmellable:
				return (_target != null )?_target.TargetIsSmellable( m_Status ):false;
			case SelectionExpressionType.ActiveTargetIsSmellable:
				return ( m_ActiveTarget != null )? m_ActiveTarget.TargetIsSmellable( m_Status ):false;
			case SelectionExpressionType.LastTargetIsSmellable:
				return ( m_PreviousTarget != null )? m_PreviousTarget.TargetIsSmellable( m_Status ):false;

			// IS DESTROYED
			case SelectionExpressionType.TargetIsDestroyed:
				return (_target != null && _target.EntityComponent != null ? _target.EntityComponent.IsDestroyed :false );
			case SelectionExpressionType.ActiveTargetIsDestroyed:
				return ( m_ActiveTarget != null && m_ActiveTarget.EntityComponent != null ? m_ActiveTarget.EntityComponent.IsDestroyed :false );
			case SelectionExpressionType.LastTargetIsDestroyed:
				return ( m_PreviousTarget != null && m_PreviousTarget.EntityComponent != null ? m_PreviousTarget.EntityComponent.IsDestroyed :false );

			// IS IDENTIFIED ENTITY
			case SelectionExpressionType.TargetHasOwnerActiveSelected:				
				return ( m_OwnerEntityComponent != null ? m_OwnerEntityComponent.ActiveCounterpartExists( _target.EntityComponent ) : false );
			case SelectionExpressionType.ActiveTargetHasOwnerActiveSelected:
				return ( m_OwnerEntityComponent != null && m_ActiveTarget != null ? m_OwnerEntityComponent.ActiveCounterpartExists( m_ActiveTarget.EntityComponent ) : false );
			case SelectionExpressionType.LastTargetHasOwnerActiveSelected:
				return ( m_OwnerEntityComponent != null && m_PreviousTarget != null ? m_OwnerEntityComponent.ActiveCounterpartExists( m_PreviousTarget.EntityComponent ) : false );

			case SelectionExpressionType.TargetIsActive:
				return ( _target != null ? _target.Active : false );

			case SelectionExpressionType.TargetIsLastTarget:
				return ( _target != null && m_PreviousTarget != null ? _target.CompareTarget( m_PreviousTarget ) : false );
			}

			return false;
		}

		private int GetEnumValue( TargetObject _target, SelectionExpressionType _type )
		{
			switch( _type )
			{

			case SelectionExpressionType.TargetEntityType:
				return (int)((_target != null )?_target.EntityType:EntityClassType.Undefined);
			case SelectionExpressionType.ActiveTargetEntityType:
				return (int)(( m_ActiveTarget != null )? m_ActiveTarget.EntityType : EntityClassType.Undefined );
			case SelectionExpressionType.LastTargetEntityType:
				return (int)(( m_PreviousTarget != null )? m_PreviousTarget.EntityType : EntityClassType.Undefined );
			case SelectionExpressionType.EnvironmentWeather:
				return (ICE.World.ICEWorldEnvironment.Instance != null?(int)ICE.World.ICEWorldEnvironment.Instance.WeatherForecast:0);
			case SelectionExpressionType.OwnOdour:
				return (int)m_Status.Odour.Type;
			case SelectionExpressionType.OwnGenderType:
				return (int)m_Status.GenderType;
			case SelectionExpressionType.OwnTrophicLevel:
				return (int)m_Status.TrophicLevel;
			case SelectionExpressionType.CreatureGenderType:
				return (int)((_target.EntityCreature != null )?_target.EntityCreature.Creature.Status.GenderType:CreatureGenderType.UNDEFINED);
			case SelectionExpressionType.CreatureTrophicLevel:
				return (int)((_target.EntityCreature != null )?_target.EntityCreature.Creature.Status.TrophicLevel:TrophicLevelType.UNDEFINED);
			
			case SelectionExpressionType.TargetOdour:
				return (int)((_target.Odour() != null )?_target.Odour().Type:OdourType.UNDEFINED);
			default:
				return 0;
			}
		}

		private float GetFloatValue( TargetObject _target, SelectionExpressionType _type )
		{
			switch( _type )
			{
			// CREATURE AGE
			case SelectionExpressionType.OwnAge:
				return m_Status.Age;

			case SelectionExpressionType.OwnerAltitude:
				return m_Move.Altitude;

				// CREATURE ODOUR INTENSITY
			case SelectionExpressionType.OwnOdourIntensity:
				return m_Status.Odour.Intensity;
				// CREATURE ODOUR RANGE
			case SelectionExpressionType.OwnOdourRange:
				return m_Status.Odour.Range;

				// CREATURE TEMPERATURE DEVIATION
			case SelectionExpressionType.OwnEnvTemperatureDeviation:
				return m_Status.TemperatureDeviationInPercent;

				// CREATURE FITNESS
			case SelectionExpressionType.OwnFitness:
				return m_Status.FitnessInPercent;
				// CREATURE HEALTH
			case SelectionExpressionType.OwnHealth:
				return m_Status.HealthInPercent;
				// CREATURE POWER
			case SelectionExpressionType.OwnPower:
				return m_Status.PowerInPercent;
				// CREATURE STAMINA
			case SelectionExpressionType.OwnStamina:
				return m_Status.StaminaInPercent;

				// CREATURE DAMAGE
			case SelectionExpressionType.OwnDamage:
				return m_Status.DamageInPercent;			
				// CREATURE DEBILITY
			case SelectionExpressionType.OwnDebility:
				return m_Status.DebilityInPercent;
				// CREATURE STRESS
			case SelectionExpressionType.OwnStress:
				return m_Status.StressInPercent;
				// CREATURE HUNGER
			case SelectionExpressionType.OwnHunger:
				return m_Status.HungerInPercent;
				// CREATURE THIRST
			case SelectionExpressionType.OwnThirst:
				return m_Status.ThirstInPercent;

				// CREATURE AGGRESSIVITY
			case SelectionExpressionType.OwnAggressivity:
				return m_Status.AggressivityInPercent;
				// CREATURE EXPERIENCE
			case SelectionExpressionType.OwnExperience:
				return m_Status.ExperienceInPercent;
				// CREATURE ANXIETY
			case SelectionExpressionType.OwnAnxiety:
				return m_Status.AnxietyInPercent;
				// CREATURE NOSINESS
			case SelectionExpressionType.OwnNosiness:
				return m_Status.NosinessInPercent;

				// CREATURE VISUAL
			case SelectionExpressionType.OwnVisualSense:
				return m_Status.SenseVisualInPercent;
				// CREATURE AUDITORY
			case SelectionExpressionType.OwnAuditorySense:
				return m_Status.SenseAuditoryInPercent;
				// CREATURE OLFACTORY
			case SelectionExpressionType.OwnOlfactorySense:
				return m_Status.SenseOlfactoryInPercent;
				// CREATURE GUSTATORY
			case SelectionExpressionType.OwnGustatorySense:
				return m_Status.SenseGustatoryInPercent;
				// CREATURE TOUCH
			case SelectionExpressionType.OwnTactileSense:
				return m_Status.SenseTactileInPercent;

			case SelectionExpressionType.OwnSlot0Amount:
				return m_Status.Inventory.SlotItemAmount( 0 );
			case SelectionExpressionType.OwnSlot1Amount:
				return m_Status.Inventory.SlotItemAmount( 1 );
			case SelectionExpressionType.OwnSlot2Amount:
				return m_Status.Inventory.SlotItemAmount( 2 );
			case SelectionExpressionType.OwnSlot3Amount:
				return m_Status.Inventory.SlotItemAmount( 3 );
			case SelectionExpressionType.OwnSlot4Amount:
				return m_Status.Inventory.SlotItemAmount( 4 );
			case SelectionExpressionType.OwnSlot5Amount:
				return m_Status.Inventory.SlotItemAmount( 5 );
			case SelectionExpressionType.OwnSlot6Amount:
				return m_Status.Inventory.SlotItemAmount( 6 );
			case SelectionExpressionType.OwnSlot7Amount:
				return m_Status.Inventory.SlotItemAmount( 7 );
			case SelectionExpressionType.OwnSlot8Amount:
				return m_Status.Inventory.SlotItemAmount( 8 );
			case SelectionExpressionType.OwnSlot9Amount:
				return m_Status.Inventory.SlotItemAmount( 9 );

			case SelectionExpressionType.OwnSlot0MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 0 );
			case SelectionExpressionType.OwnSlot1MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 1 );
			case SelectionExpressionType.OwnSlot2MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 2 );
			case SelectionExpressionType.OwnSlot3MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 3 );
			case SelectionExpressionType.OwnSlot4MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 4 );
			case SelectionExpressionType.OwnSlot5MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 5 );
			case SelectionExpressionType.OwnSlot6MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 6 );
			case SelectionExpressionType.OwnSlot7MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 7 );
			case SelectionExpressionType.OwnSlot8MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 8 );
			case SelectionExpressionType.OwnSlot9MaxAmount:
				return m_Status.Inventory.SlotItemMaxAmount( 9 );

				// CREATURE ACTIVE TARGET TIME
			case SelectionExpressionType.ActiveTargetTime:
				return ( m_ActiveTarget != null)?m_ActiveTarget.ActiveTime:0;
				// CREATURE ACTIVE TARGET TIME TOTAL
			case SelectionExpressionType.ActiveTargetTimeTotal:
				return ( m_ActiveTarget != null)?m_ActiveTarget.ActiveTimeTotal:0;

				// TARGET AGE
			case SelectionExpressionType.ActiveTargetAge:
				return ( m_ActiveTarget != null) ? m_ActiveTarget.Age : 0;

				// CREATURE ACTIVE TARGET INVENTORY
			case SelectionExpressionType.ActiveTargetSlot0Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 0 ):0;
			case SelectionExpressionType.ActiveTargetSlot1Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 1 ):0;
			case SelectionExpressionType.ActiveTargetSlot2Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 2 ):0;
			case SelectionExpressionType.ActiveTargetSlot3Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 3 ):0;
			case SelectionExpressionType.ActiveTargetSlot4Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 4 ):0;
			case SelectionExpressionType.ActiveTargetSlot5Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 5 ):0;
			case SelectionExpressionType.ActiveTargetSlot6Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 6 ):0;
			case SelectionExpressionType.ActiveTargetSlot7Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 7 ):0;
			case SelectionExpressionType.ActiveTargetSlot8Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 8 ):0;
			case SelectionExpressionType.ActiveTargetSlot9Amount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemAmount( 9 ):0;

			case SelectionExpressionType.ActiveTargetSlot0MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 0 ):0;
			case SelectionExpressionType.ActiveTargetSlot1MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 1 ):0;
			case SelectionExpressionType.ActiveTargetSlot2MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 2 ):0;
			case SelectionExpressionType.ActiveTargetSlot3MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 3 ):0;
			case SelectionExpressionType.ActiveTargetSlot4MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 4 ):0;
			case SelectionExpressionType.ActiveTargetSlot5MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 5 ):0;
			case SelectionExpressionType.ActiveTargetSlot6MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 6 ):0;
			case SelectionExpressionType.ActiveTargetSlot7MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 7 ):0;
			case SelectionExpressionType.ActiveTargetSlot8MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 8 ):0;
			case SelectionExpressionType.ActiveTargetSlot9MaxAmount:
				return ( m_ActiveTarget != null && m_ActiveTarget.Inventory() != null)?m_ActiveTarget.Inventory().SlotItemMaxAmount( 9 ):0;

				// ACTIVE TARGET ODOUR INTENSITY
			case SelectionExpressionType.ActiveTargetOdourIntensity:
				return ( m_ActiveTarget != null && m_ActiveTarget.Odour() != null ? m_ActiveTarget.Odour().Intensity : 0 );
				// ACTIVE TARGET ODOUR INTENSITY BY DISTANCE
			case SelectionExpressionType.ActiveTargetOdourIntensityByDistance:
				return ( m_ActiveTarget != null && m_ActiveTarget.Odour() != null ? ( m_ActiveTarget.Odour().Intensity / (m_ActiveTarget.TargetDistanceTo( Owner.transform.position )+1) ): 0 );

				// ACTIVE TARGET ODOUR INTENSITY NET
			case SelectionExpressionType.ActiveTargetOdourIntensityNet:
				return ( m_ActiveTarget != null && m_ActiveTarget.Odour() != null ? ( m_ActiveTarget.Odour().Intensity / (m_ActiveTarget.TargetDistanceTo( Owner.transform.position )+1) * m_Status.SenseOlfactoryInPercent * 0.01f ): 0 );
				// ACTIVE TARGET ODOUR RANGE
			case SelectionExpressionType.ActiveTargetOdourRange:
				return ( m_ActiveTarget != null && m_ActiveTarget.Odour() != null ? m_ActiveTarget.Odour().Range : 0 );


				// LAST TARGET TIME
			case SelectionExpressionType.LastTargetTime:
				return ( m_PreviousTarget != null)?m_PreviousTarget.ActiveTime:0;
				// LAST TARGET TIME TOTAL
			case SelectionExpressionType.LastTargetTimeTotal:
				return ( m_PreviousTarget != null)?m_PreviousTarget.ActiveTimeTotal:0;

				// LAST TARGET AGE
			case SelectionExpressionType.LastTargetAge:
				return ( m_PreviousTarget != null) ? m_PreviousTarget.Age : 0;

				// LAST TARGET INVENTORY
			case SelectionExpressionType.LastTargetSlot0Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 0 ):0;
			case SelectionExpressionType.LastTargetSlot1Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 1 ):0;
			case SelectionExpressionType.LastTargetSlot2Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 2 ):0;
			case SelectionExpressionType.LastTargetSlot3Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 3 ):0;
			case SelectionExpressionType.LastTargetSlot4Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 4 ):0;
			case SelectionExpressionType.LastTargetSlot5Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 5 ):0;
			case SelectionExpressionType.LastTargetSlot6Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 6 ):0;
			case SelectionExpressionType.LastTargetSlot7Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 7 ):0;
			case SelectionExpressionType.LastTargetSlot8Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 8 ):0;
			case SelectionExpressionType.LastTargetSlot9Amount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemAmount( 9 ):0;

			case SelectionExpressionType.LastTargetSlot0MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 0 ):0;
			case SelectionExpressionType.LastTargetSlot1MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 1 ):0;
			case SelectionExpressionType.LastTargetSlot2MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 2 ):0;
			case SelectionExpressionType.LastTargetSlot3MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 3 ):0;
			case SelectionExpressionType.LastTargetSlot4MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 4 ):0;
			case SelectionExpressionType.LastTargetSlot5MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 5 ):0;
			case SelectionExpressionType.LastTargetSlot6MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 6 ):0;
			case SelectionExpressionType.LastTargetSlot7MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 7 ):0;
			case SelectionExpressionType.LastTargetSlot8MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 8 ):0;
			case SelectionExpressionType.LastTargetSlot9MaxAmount:
				return ( m_PreviousTarget != null && m_PreviousTarget.Inventory() != null)?m_PreviousTarget.Inventory().SlotItemMaxAmount( 9 ):0;

				// LAST TARGET ODOUR INTENSITY
			case SelectionExpressionType.LastTargetOdourIntensity:
				return ( m_PreviousTarget != null && m_PreviousTarget.Odour() != null ? m_PreviousTarget.Odour().Intensity : 0 );
				// LAST TARGET ODOUR INTENSITY BY DISTANCE
			case SelectionExpressionType.LastTargetOdourIntensityByDistance:
				return ( m_PreviousTarget != null && m_PreviousTarget.Odour() != null ? ( m_PreviousTarget.Odour().Intensity / (m_PreviousTarget.TargetDistanceTo( Owner.transform.position )+1) ): 0 );

				// LAST TARGET ODOUR INTENSITY NET
			case SelectionExpressionType.LastTargetOdourIntensityNet:
				return ( m_PreviousTarget != null && m_PreviousTarget.Odour() != null ? ( m_PreviousTarget.Odour().Intensity / (m_PreviousTarget.TargetDistanceTo( Owner.transform.position )+1) * m_Status.SenseOlfactoryInPercent * 0.01f ): 0 );
				// LAST TARGET ODOUR RANGE
			case SelectionExpressionType.LastTargetOdourRange:
				return ( m_PreviousTarget != null && m_PreviousTarget.Odour() != null ? m_PreviousTarget.Odour().Range : 0 );

				// TARGET TIME
			case SelectionExpressionType.TargetTime:
				return ( _target != null)?_target.ActiveTime:0;
				// TARGET TIME TOTAL
			case SelectionExpressionType.TargetTimeTotal:
				return ( _target != null)?_target.ActiveTimeTotal:0;

			// TARGET AGE
			case SelectionExpressionType.TargetAge:
				return _target.Age;

				// TARGET 
			case SelectionExpressionType.TargetSlot0Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 0 ):0;
			case SelectionExpressionType.TargetSlot1Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 1 ):0;
			case SelectionExpressionType.TargetSlot2Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 2 ):0;
			case SelectionExpressionType.TargetSlot3Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 3 ):0;
			case SelectionExpressionType.TargetSlot4Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 4 ):0;
			case SelectionExpressionType.TargetSlot5Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 5 ):0;
			case SelectionExpressionType.TargetSlot6Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 6 ):0;
			case SelectionExpressionType.TargetSlot7Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 7 ):0;
			case SelectionExpressionType.TargetSlot8Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 8 ):0;
			case SelectionExpressionType.TargetSlot9Amount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemAmount( 9 ):0;

			case SelectionExpressionType.TargetSlot0MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 0 ):0;
			case SelectionExpressionType.TargetSlot1MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 1 ):0;
			case SelectionExpressionType.TargetSlot2MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 2 ):0;
			case SelectionExpressionType.TargetSlot3MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 3 ):0;
			case SelectionExpressionType.TargetSlot4MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 4 ):0;
			case SelectionExpressionType.TargetSlot5MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 5 ):0;
			case SelectionExpressionType.TargetSlot6MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 6 ):0;
			case SelectionExpressionType.TargetSlot7MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 7 ):0;
			case SelectionExpressionType.TargetSlot8MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 8 ):0;
			case SelectionExpressionType.TargetSlot9MaxAmount:
				return (_target.Inventory() != null)?_target.Inventory().SlotItemMaxAmount( 9 ):0;

			// VISIBILITY BY DISTANCE
			case SelectionExpressionType.TargetVisibilityByDistance:
				return (_target != null ?_target.Visibility( m_Status ):0 );
			case SelectionExpressionType.ActiveTargetVisibilityByDistance:
				return ( m_ActiveTarget != null)? m_ActiveTarget.Visibility( m_Status ):0;
			case SelectionExpressionType.LastTargetVisibilityByDistance:
				return ( m_PreviousTarget != null)? m_PreviousTarget.Visibility( m_Status ):0;

			// AUDIBILITY BY DISTANCE
			case SelectionExpressionType.TargetAudibilityByDistance:
				return (_target != null ?_target.Audibility( m_Status ):0 );
			case SelectionExpressionType.ActiveTargetAudibilityByDistance:
				return ( m_ActiveTarget != null)? m_ActiveTarget.Audibility( m_Status ):0;
			case SelectionExpressionType.LastTargetAudibilityByDistance:
				return ( m_PreviousTarget != null)? m_PreviousTarget.Audibility( m_Status ):0;

			// SMELLABILITY BY DISTANCE
			case SelectionExpressionType.TargetSmellabilityByDistance:
				return (_target != null ?_target.Smellability( m_Status ):0 );
			case SelectionExpressionType.ActiveTargetSmellabilityByDistance:
				return ( m_ActiveTarget != null)? m_ActiveTarget.Smellability( m_Status ):0;
			case SelectionExpressionType.LastTargetSmellabilityByDistance:
				return ( m_PreviousTarget != null)? m_PreviousTarget.Smellability( m_Status ):0;

			// DURABILITY
			case SelectionExpressionType.TargetDurability:
				return (_target != null && _target.EntityComponent != null ? _target.EntityComponent.Durability:0 );
			case SelectionExpressionType.ActiveTargetDurability:
				return ( m_ActiveTarget != null && m_ActiveTarget.EntityComponent != null ? m_ActiveTarget.EntityComponent.Durability:0 );
			case SelectionExpressionType.LastTargetDurability:
				return ( m_PreviousTarget != null && m_PreviousTarget.EntityComponent != null ? m_PreviousTarget.EntityComponent.Durability:0 );

			// DURABILITY IN PERCENT
			case SelectionExpressionType.TargetDurabilityInPercent:
				return (_target != null && _target.EntityComponent != null ? _target.EntityComponent.DurabilityInPercent:0 );
			case SelectionExpressionType.ActiveTargetDurabilityInPercent:
				return ( m_ActiveTarget != null && m_ActiveTarget.EntityComponent != null ? m_ActiveTarget.EntityComponent.DurabilityInPercent:0 );
			case SelectionExpressionType.LastTargetDurabilityInPercent:
				return ( m_PreviousTarget != null && m_PreviousTarget.EntityComponent != null ? m_PreviousTarget.EntityComponent.DurabilityInPercent:0 );

			case SelectionExpressionType.TargetActiveCounterpartsLimit:
				return (_target != null && _target.EntityComponent != null ? _target.EntityComponent.ActiveCounterparts.Count :0 );
			case SelectionExpressionType.ActiveTargetActiveCounterpartsLimit:
				return ( m_ActiveTarget != null && m_ActiveTarget.EntityComponent != null ? m_ActiveTarget.EntityComponent.ActiveCounterparts.Count :0 );
			case SelectionExpressionType.LastTargetActiveCounterpartsLimit:
				return ( m_PreviousTarget != null && m_PreviousTarget.EntityComponent != null ? m_PreviousTarget.EntityComponent.ActiveCounterparts.Count :0 );

			case SelectionExpressionType.CreatureAltitude:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Move.Altitude:0;


				// TARGET CREATURE TEMPERATURE DEVIATION
			case SelectionExpressionType.CreatureEnvTemperatureDeviation:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.TemperatureDeviationInPercent:0;

				// TARGET CREATURE FITNESS
			case SelectionExpressionType.CreatureFitness:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.FitnessInPercent:0;
				// TARGET CREATURE HEALTH
			case SelectionExpressionType.CreatureHealth:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.HealthInPercent:0;
				// CREATURE POWER
			case SelectionExpressionType.CreaturePower:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.PowerInPercent:0;
				// TARGET CREATURE STAMINA
			case SelectionExpressionType.CreatureStamina:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.StaminaInPercent:0;

				// TARGET CREATURE DAMAGE
			case SelectionExpressionType.CreatureDamage:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.DamageInPercent:0;			
				// TARGET CREATURE DEBILITY
			case SelectionExpressionType.CreatureDebility:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.DebilityInPercent:0;
				// CREATURE STRESS
			case SelectionExpressionType.CreatureStress:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.StressInPercent:0;
				// TARGET CREATURE HUNGER
			case SelectionExpressionType.CreatureHunger:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.HungerInPercent:0;
				// TARGET CREATURE THIRST
			case SelectionExpressionType.CreatureThirst:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.ThirstInPercent:0;

				// TARGET CREATURE AGGRESSIVITY
			case SelectionExpressionType.CreatureAggressivity:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.AggressivityInPercent:0;
				// TARGET CREATURE EXPERIENCE
			case SelectionExpressionType.CreatureExperience:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.ExperienceInPercent:0;
				// TARGET CREATURE ANXIETY
			case SelectionExpressionType.CreatureAnxiety:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.AnxietyInPercent:0;
				// TARGET CREATURE NOSINESS
			case SelectionExpressionType.CreatureNosiness:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.NosinessInPercent:0;

				// TARGET CREATURE VISUAL
			case SelectionExpressionType.CreatureVisualSense:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.SenseVisualInPercent:0;
				// CREATURE AUDITORY
			case SelectionExpressionType.CreatureAuditorySense:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.SenseAuditoryInPercent:0;
				// CREATURE OLFACTORY
			case SelectionExpressionType.CreatureOlfactorySense:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.SenseOlfactoryInPercent:0;
				// TARGET CREATURE GUSTATORY
			case SelectionExpressionType.CreatureGustatorySense:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.SenseGustatoryInPercent:0;
				// TARGET CREATURE TOUCH
			case SelectionExpressionType.CreatureTactileSense:
				return (_target.EntityCreature != null)?_target.EntityCreature.Creature.Status.SenseTactileInPercent:0;



				// TARGET CREATURE ACTIVE TARGET TIME
			case SelectionExpressionType.CreatureActiveTargetTime:
				return (_target.EntityCreature != null && _target.EntityCreature.Creature.ActiveTarget != null)?_target.EntityCreature.Creature.ActiveTarget.ActiveTime:0;
				// TARGET CREATURE ACTIVE TARGET TIME TOTAL
			case SelectionExpressionType.CreatureActiveTargetTimeTotal:
				return (_target.EntityCreature != null && _target.EntityCreature.Creature.ActiveTarget != null)?_target.EntityCreature.Creature.ActiveTarget.ActiveTimeTotal:0;



				// TARGET ODOUR INTENSITY
			case SelectionExpressionType.TargetOdourIntensity:
				return (int)((_target.Odour() != null )?_target.Odour().Intensity : 0 );
				// TARGET ODOUR INTENSITY BY DISTANCE
			case SelectionExpressionType.TargetOdourIntensityByDistance:
				return ( _target.Odour() != null ? (_target.Odour().Intensity / (_target.TargetDistanceTo( Owner.transform.position )+1) ): 0 );

				// TARGET ODOUR INTENSITY NET
			case SelectionExpressionType.TargetOdourIntensityNet:
				return ( _target.Odour() != null ? (_target.Odour().Intensity / (_target.TargetDistanceTo( Owner.transform.position )+1) * m_Status.SenseOlfactoryInPercent * 0.01f ): 0 );
				// TARGET ODOUR RANGE
			case SelectionExpressionType.TargetOdourRange:
				return ( _target.Odour() != null ? _target.Odour().Range : 0 );

				// TARGET DISTANCE
			case SelectionExpressionType.TargetDistance:
				return _target.TargetDistanceTo( Owner.transform.position );
				// TARGET MOVE POSITION DISTANCE
			case SelectionExpressionType.TargetOffsetPositionDistance:
				return _target.TargetOffsetPositionDistanceTo( Owner.transform.position );
				// TARGET MOVE POSITION DISTANCE
			case SelectionExpressionType.TargetMovePositionDistance:
				return _target.TargetMovePositionDistanceTo( Owner.transform.position );

			case SelectionExpressionType.TargetLastKnownPositionDistance:
				return _target.TargetLastKnownPositionDistanceTo( Owner.transform.position );

			case SelectionExpressionType.OwnHomeDistance:
				return ( m_Creature != null ? m_Creature.Essentials.Target.TargetMovePositionDistanceTo( Owner.transform.position ) : 0 );




				// ENVIRONMENT
			case SelectionExpressionType.EnvironmentDateDay:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.DateDay:0);
			case SelectionExpressionType.EnvironmentDateMonth:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.DateMonth:0);
			case SelectionExpressionType.EnvironmentDateYear:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.DateYear:0);

			case SelectionExpressionType.EnvironmentTimeHour:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.TimeHour:0);
			case SelectionExpressionType.EnvironmentTimeMinute:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.TimeMinutes:0);
			case SelectionExpressionType.EnvironmentTimeSecond:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.TimeSeconds:0);

			case SelectionExpressionType.EnvironmentTemperature:
				return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.Temperature:0);


			default:
				return 0;
			}
		}

		private bool CompareBooleanValue( TargetObject _target, SelectionConditionObject _condition )
		{
			return CompareBoolean( GetBooleanValue( _target, _condition.ExpressionType ), _condition.BooleanValue, _condition.Operator );
		}

		private bool CompareStringValue( TargetObject _target, SelectionConditionObject _condition )
		{
			if( _condition.UseDynamicValue )
				return CompareString( GetStringValue( _target, _condition.ExpressionType ), GetStringValue( _target, _condition.ExpressionValue ), _condition.Operator );
			else
				return CompareString( GetStringValue( _target, _condition.ExpressionType ), _condition.StringValue, _condition.Operator );
		}

		private bool CompareKeyCodeValue( TargetObject _target, SelectionConditionObject _condition )
		{
			return CompareKeyCode( _condition.KeyCodeValue, _condition.Operator );
		}

		private bool CompareAxisValue( TargetObject _target, SelectionConditionObject _condition )
		{
			return CompareAxis( _condition.AxisValue, _condition.Operator );
		}

		private bool CompareToggleValue( TargetObject _target, SelectionConditionObject _condition )
		{
			return CompareToggle( _condition.GetUIToggle(), _condition.Operator );
		}

		private bool CompareButtonValue( TargetObject _target, SelectionConditionObject _condition )
		{
			_condition.GetUIButton();

			return CompareButton( _condition.ButtonIsPressed, _condition.Operator );
		}

		private bool CompareEnumValue( TargetObject _target, SelectionConditionObject _condition )
		{
			return CompareNumber( GetEnumValue( _target, _condition.ExpressionType ), _condition.IntegerValue, _condition.Operator );
		}

		private bool CompareNumericValue( TargetObject _target, SelectionConditionObject _condition )
		{
			if( _condition.UseDynamicValue )
				return CompareNumber( GetFloatValue( _target, _condition.ExpressionType ), GetFloatValue( _target, _condition.ExpressionValue ), _condition.Operator );
			else
				return CompareNumber( GetFloatValue( _target, _condition.ExpressionType ), _condition.FloatValue, _condition.Operator );
		}

		private bool CompareObjects( TargetObject _target, SelectionConditionObject _condition )
		{
			return CompareObjects( GetGameObject( _target, _condition.ExpressionType ), GetGameObject( _target, _condition.ExpressionValue ), _condition.Operator );
		}

		public static bool CompareString( string _value_1, string _value_2, LogicalOperatorType _operator )
		{
			bool _result = false;

			if( _value_1 == _value_2 )
				_result = true;

			if( _operator == LogicalOperatorType.NOT )
				_result = ! _result;

			return _result;
		}

		public static bool CompareKeyCode( KeyCode _key, LogicalOperatorType _operator )
		{
			bool _result = Input.GetKey( _key );

			if( _operator == LogicalOperatorType.NOT )
				_result = ! _result;

			return _result;
		}

		public static bool CompareAxis( AxisInputData _axis, LogicalOperatorType _operator )
		{
			bool _result = ( Input.GetAxis( _axis.Name ) != 0 ? true : false );

			if( _operator == LogicalOperatorType.NOT )
				_result = ! _result;

			return _result;
		}

		public static bool CompareToggle( UnityEngine.UI.Toggle _toggle, LogicalOperatorType _operator )
		{
			bool _result = ( _toggle != null && _toggle.isOn ? true : false );

			if( _operator == LogicalOperatorType.NOT )
				_result = ! _result;

			return _result;
		}

		public static bool CompareButton( bool _pressed, LogicalOperatorType _operator )
		{
			bool _result = _pressed;

			if( _operator == LogicalOperatorType.NOT )
				_result = ! _result;

			return _result;
		}

		public static bool CompareBoolean( bool _value_1, bool _value_2, LogicalOperatorType _operator )
		{
			bool _result = false;

			if( _value_1 == _value_2 )
				_result = true;

			if( _operator == LogicalOperatorType.NOT )
				_result = ! _result;

			return _result;
		}

		public static bool CompareNumber( float _value_1, float _value_2, LogicalOperatorType _operator )
		{
			bool _result = false;
			switch( _operator )
			{
			case LogicalOperatorType.EQUAL:
				_result = ( _value_1 == _value_2 )?true:false;
				break;
			case LogicalOperatorType.GREATER:
				_result = ( _value_1 > _value_2 )?true:false;
				break;
			case LogicalOperatorType.GREATER_OR_EQUAL:
				_result = ( _value_1 >= _value_2 )?true:false;
				break;
			case LogicalOperatorType.LESS:
				_result = ( _value_1 < _value_2 )?true:false;
				break;
			case LogicalOperatorType.LESS_OR_EQUAL:
				_result = ( _value_1 <= _value_2 )?true:false;
				break;
			case LogicalOperatorType.NOT:
				_result = ( _value_1 != _value_2 )?true:false;
				break;
			}

			return _result;
		}

		public static bool CompareObjects( GameObject _obj_1, GameObject _obj_2, LogicalOperatorType _operator )
		{
			if( _obj_1 == null || _obj_2 == null )
				return false;

			bool _result = false;
			switch( _operator )
			{
			case LogicalOperatorType.EQUAL:
				_result = ( _obj_1.GetInstanceID() == _obj_2.GetInstanceID() )?true:false;
				break;
			case LogicalOperatorType.NOT:
				_result = ( _obj_1.GetInstanceID() != _obj_2.GetInstanceID() )?true:false;
				break;
			}

			return _result;
		}
	}
}