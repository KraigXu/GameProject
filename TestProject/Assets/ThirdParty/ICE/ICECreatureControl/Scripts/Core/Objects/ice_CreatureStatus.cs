// ##############################################################################
//
// ice_CreatureStatus.cs
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
using System.Text.RegularExpressions;

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
	public class StatusDataObject : ICE.World.Objects.EntityStatusObject
	{
		public StatusDataObject(){Enabled = true;}
		public StatusDataObject( StatusDataObject _object ) : base( _object ){ Copy( _object ); }
		public StatusDataObject( ICEWorldBehaviour _component ) : base( _component ){Enabled = true;}

		public void Copy( StatusDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Enabled = true;

			PerceptionTime = _object.PerceptionTime;
			PerceptionTimeMin = _object.PerceptionTimeMin;
			PerceptionTimeMax = _object.PerceptionTimeMax;

			ReactionTime = _object.ReactionTime;
			ReactionTimeMin = _object.ReactionTimeMin;
			ReactionTimeMax = _object.ReactionTimeMax;

			PerceptionTimeFitnessMultiplier = _object.PerceptionTimeFitnessMultiplier;

			RecoveryPhase = _object.RecoveryPhase;
			RecoveryPhaseMin = _object.RecoveryPhaseMin;
			RecoveryPhaseMax = _object.RecoveryPhaseMax;
			RecoveryPhaseMaximum = _object.RecoveryPhaseMaximum;

			RemovingDelay = _object.RemovingDelay;
			RemovingDelayMin = _object.RemovingDelayMin;
			RemovingDelayMax = _object.RemovingDelayMax;
			RemovingDelayMaximum = _object.RemovingDelayMaximum;


			FitnessRecreationLimit = _object.FitnessRecreationLimit;

			TrophicLevel = _object.TrophicLevel;
			GenderType = _object.GenderType;
			IsCannibal = _object.IsCannibal;
			UseDynamicInitialisation = _object.UseDynamicInitialisation;

			UseAdvanced = _object.UseAdvanced;
			UseArmor = _object.UseArmor;

			UseEnvironmentTemperature = _object.UseEnvironmentTemperature;
			MinEnvironmentTemperature = _object.MinEnvironmentTemperature;
			MaxEnvironmentTemperature = _object.MaxEnvironmentTemperature;
			ComfortEnvironmentTemperature = _object.ComfortEnvironmentTemperature;

			UseTime = _object.UseTime;
			UseDate = _object.UseDate;

			UseShelter = _object.UseShelter;
			ShelterTag = _object.ShelterTag;
			IsSheltered = _object.IsSheltered;

			UseIndoor = _object.UseIndoor;
			IndoorTag = _object.IndoorTag;
			IsIndoor = _object.IsIndoor;
			// ShelterTemperature = false;

			//ConsiderBreathing = false;

			Aggressivity = _object.Aggressivity;
			DefaultAggressivity = _object.DefaultAggressivity;
			AggressivityHealthMultiplier = _object.AggressivityHealthMultiplier;
			AggressivityStaminaMultiplier = _object.AggressivityStaminaMultiplier;
			AggressivityPowerMultiplier = _object.AggressivityPowerMultiplier;
			AggressivityDamageMultiplier = _object.AggressivityDamageMultiplier;
			AggressivityStressMultiplier = _object.AggressivityStressMultiplier;
			AggressivityDebilityMultiplier = _object.AggressivityDebilityMultiplier;
			AggressivityHungerMultiplier = _object.AggressivityHungerMultiplier;
			AggressivityThirstMultiplier = _object.AggressivityThirstMultiplier;
			AggressivityTemperaturMultiplier = _object.AggressivityTemperaturMultiplier;
			AggressivityAgeMultiplier = _object.AggressivityAgeMultiplier;

			Anxiety = _object.Anxiety;
			DefaultAnxiety = _object.DefaultAnxiety;
			AnxietyHealthMultiplier = _object.AnxietyHealthMultiplier;
			AnxietyStaminaMultiplier = _object.AnxietyStaminaMultiplier;
			AnxietyPowerMultiplier = _object.AnxietyPowerMultiplier;
			AnxietyDamageMultiplier = _object.AnxietyDamageMultiplier;
			AnxietyStressMultiplier = _object.AnxietyStressMultiplier;
			AnxietyDebilityMultiplier = _object.AnxietyDebilityMultiplier;
			AnxietyHungerMultiplier = _object.AnxietyHungerMultiplier;
			AnxietyThirstMultiplier = _object.AnxietyThirstMultiplier;
			AnxietyTemperaturMultiplier = _object.AnxietyTemperaturMultiplier;
			AnxietyAgeMultiplier = _object.AnxietyAgeMultiplier;

			Experience = _object.Experience;
			DefaultExperience = _object.DefaultExperience;
			ExperienceHealthMultiplier = _object.ExperienceHealthMultiplier;
			ExperienceStaminaMultiplier = _object.ExperienceStaminaMultiplier;
			ExperiencePowerMultiplier = _object.ExperiencePowerMultiplier;
			ExperienceDamageMultiplier = _object.ExperienceDamageMultiplier;
			ExperienceStressMultiplier = _object.ExperienceStressMultiplier;
			ExperienceDebilityMultiplier = _object.ExperienceDebilityMultiplier;
			ExperienceHungerMultiplier = _object.ExperienceHungerMultiplier;
			ExperienceThirstMultiplier = _object.ExperienceThirstMultiplier;
			ExperienceTemperaturMultiplier = _object.ExperienceTemperaturMultiplier;
			ExperienceAgeMultiplier = _object.ExperienceAgeMultiplier;

			Nosiness = _object.Nosiness;
			DefaultNosiness = _object.DefaultNosiness;
			NosinessHealthMultiplier = _object.NosinessHealthMultiplier;
			NosinessStaminaMultiplier = _object.NosinessStaminaMultiplier;
			NosinessPowerMultiplier = _object.NosinessPowerMultiplier;
			NosinessDamageMultiplier = _object.NosinessDamageMultiplier;
			NosinessStressMultiplier = _object.NosinessStressMultiplier;
			NosinessDebilityMultiplier = _object.NosinessDebilityMultiplier;
			NosinessHungerMultiplier = _object.NosinessHungerMultiplier;
			NosinessThirstMultiplier = _object.NosinessThirstMultiplier;
			NosinessTemperaturMultiplier = _object.NosinessTemperaturMultiplier;
			NosinessAgeMultiplier = _object.NosinessAgeMultiplier;

			HealthDamageMultiplier = _object.HealthDamageMultiplier;
			HealthStressMultiplier = _object.HealthStressMultiplier;
			HealthDebilityMultiplier = _object.HealthDebilityMultiplier;
			HealthHungerMultiplier = _object.HealthHungerMultiplier;
			HealthThirstMultiplier = _object.HealthThirstMultiplier;
			HealthRecreationMultiplier = _object.HealthRecreationMultiplier;
			HealthTemperaturMultiplier = _object.HealthTemperaturMultiplier;
			HealthAgeMultiplier = _object.HealthAgeMultiplier;

			PowerHealthMultiplier = _object.PowerHealthMultiplier;
			PowerDamageMultiplier = _object.PowerDamageMultiplier;
			PowerStaminaMultiplier = _object.PowerStaminaMultiplier;
			PowerStressMultiplier = _object.PowerStressMultiplier;
			PowerDebilityMultiplier = _object.PowerDebilityMultiplier;
			PowerHungerMultiplier = _object.PowerHungerMultiplier;
			PowerThirstMultiplier = _object.PowerThirstMultiplier;
			PowerTemperaturMultiplier = _object.PowerTemperaturMultiplier;
			PowerAgeMultiplier = _object.PowerAgeMultiplier;

			StaminaHealthMultiplier = _object.StaminaHealthMultiplier;
			StaminaDamageMultiplier = _object.StaminaDamageMultiplier;
			StaminaStressMultiplier = _object.StaminaStressMultiplier;
			StaminaDebilityMultiplier = _object.StaminaDebilityMultiplier;
			StaminaHungerMultiplier = _object.StaminaHungerMultiplier;
			StaminaThirstMultiplier = _object.StaminaThirstMultiplier;
			StaminaTemperaturMultiplier = _object.StaminaTemperaturMultiplier;
			StaminaAgeMultiplier = _object.StaminaAgeMultiplier;

			// RecreationMultiplier = _object.;
			FitnessSpeedMultiplier = _object.FitnessSpeedMultiplier;

			Sensoria.Copy( _object.Sensoria );
			Temperatur.Copy( _object.Temperatur );
			Inventory.Copy( _object.Inventory );
			Memory.Copy( _object.Memory );
		}

		public float ReactionTime = 0.5f;
		public float ReactionTimeMin = 0.1f;
		public float ReactionTimeMax = 0.2f;
			
		public float PerceptionTime = 0.5f;
		public float PerceptionTimeMin = 0.4f;
		public float PerceptionTimeMax = 0.6f;

		public float PerceptionTimeFitnessMultiplier = 0.0f;

		public float RecoveryPhase = 0;
		public float RecoveryPhaseMin = 0;
		public float RecoveryPhaseMax = 0;
		public float RecoveryPhaseMaximum = 30;

		public float RemovingDelay = 0;
		public float RemovingDelayMin = 0;
		public float RemovingDelayMax = 0;
		public float RemovingDelayMaximum = 300.0f;


		public float FitnessRecreationLimit = 0;
		public float FitnessVitalityLimit = 0;

		public TrophicLevelType TrophicLevel = TrophicLevelType.UNDEFINED;
		public CreatureGenderType GenderType = CreatureGenderType.UNDEFINED;
		public bool IsCannibal = false;
		public bool UseDynamicInitialisation = false;
		
		public bool UseAdvanced = false;
		public bool UseArmor = false;

		public bool UseEnvironmentTemperature = false;
		public float MinEnvironmentTemperature = -25;
		public float MaxEnvironmentTemperature = 50;
		public float ComfortEnvironmentTemperature = 25;

		public bool UseTime = false;
		public bool UseDate = false;

		public bool UseShelter = false;
		public string ShelterTag = "Untagged";
		public bool IsSheltered = false;

		public bool UseIndoor = false;
		public string IndoorTag = "Untagged";
		public bool IsIndoor = false;
		//public float ShelterTemperature = false;

		//public bool ConsiderBreathing = false;
			
		public float Aggressivity = 25;
		public float DefaultAggressivity = 25;
		public float AggressivityHealthMultiplier = 1.0f;
		public float AggressivityStaminaMultiplier = 0.01f;
		public float AggressivityPowerMultiplier = 0.01f;
		public float AggressivityDamageMultiplier = 0.01f;
		public float AggressivityStressMultiplier = 0.01f;
		public float AggressivityDebilityMultiplier = 0.25f;
		public float AggressivityHungerMultiplier = 0.0f;
		public float AggressivityThirstMultiplier = 0.0f;
		public float AggressivityTemperaturMultiplier = 0.0f;
		public float AggressivityAgeMultiplier = 0.0f;
		
		public float Anxiety = 0;
		public float DefaultAnxiety = 0;
		public float AnxietyHealthMultiplier = 1.0f;
		public float AnxietyStaminaMultiplier = 0.01f;
		public float AnxietyPowerMultiplier = 0.01f;
		public float AnxietyDamageMultiplier = 0.01f;
		public float AnxietyStressMultiplier = 0.01f;
		public float AnxietyDebilityMultiplier = 0.25f;
		public float AnxietyHungerMultiplier = 0.0f;
		public float AnxietyThirstMultiplier = 0.0f;
		public float AnxietyTemperaturMultiplier = 0.0f;
		public float AnxietyAgeMultiplier = 0.0f;
		
		public float Experience = 0;
		public float DefaultExperience = 0;
		public float ExperienceHealthMultiplier = 0.0f;
		public float ExperienceStaminaMultiplier = 0.0f;
		public float ExperiencePowerMultiplier = 0.01f;
		public float ExperienceDamageMultiplier = 0.0f;
		public float ExperienceStressMultiplier = 0.0f;
		public float ExperienceDebilityMultiplier = 0.0f;
		public float ExperienceHungerMultiplier = 0.0f;
		public float ExperienceThirstMultiplier = 0.0f;
		public float ExperienceTemperaturMultiplier = 0.0f;
		public float ExperienceAgeMultiplier = 0.0f;
		
		public float Nosiness = 0;
		public float DefaultNosiness = 0;
		public float NosinessHealthMultiplier = 0.0f;
		public float NosinessStaminaMultiplier = 0.0f;
		public float NosinessPowerMultiplier = 0.01f;
		public float NosinessDamageMultiplier = 0.0f;
		public float NosinessStressMultiplier = 0.0f;
		public float NosinessDebilityMultiplier = 0.0f;
		public float NosinessHungerMultiplier = 0.0f;
		public float NosinessThirstMultiplier = 0.0f;
		public float NosinessTemperaturMultiplier = 0.0f;
		public float NosinessAgeMultiplier = 0.0f;

		// HEALTH
		public float HealthDamageMultiplier = 1f;
		public float HealthStressMultiplier = 0;
		public float HealthDebilityMultiplier = 0f;
		public float HealthHungerMultiplier = 0;
		public float HealthThirstMultiplier = 0;
		public float HealthTemperaturMultiplier = 0f;
		public float HealthAgeMultiplier = 0f;
		public float HealthRecreationMultiplier = 0f;

		// POWER
		public float PowerDamageMultiplier = 1f;
		public float PowerStressMultiplier = 0;
		public float PowerDebilityMultiplier = 0;
		public float PowerHungerMultiplier = 0;
		public float PowerThirstMultiplier = 0;
		public float PowerTemperaturMultiplier = 0f;
		public float PowerAgeMultiplier = 0f;
		public float PowerStaminaMultiplier = 0f;
		public float PowerHealthMultiplier = 0f;

		// STAMINA
		public float StaminaDamageMultiplier = 1f;
		public float StaminaStressMultiplier = 0;
		public float StaminaDebilityMultiplier = 0;
		public float StaminaHungerMultiplier = 0;
		public float StaminaThirstMultiplier = 0;
		public float StaminaTemperaturMultiplier = 0f;
		public float StaminaAgeMultiplier = 0f;
		public float StaminaHealthMultiplier = 0.01f;

		//public float RecreationMultiplier = 0.01f;
		public float FitnessSpeedMultiplier = 0.01f;


		[SerializeField]
		private SensoriaObject m_Sensoria = null;
		public SensoriaObject Sensoria{
			get{ return m_Sensoria = ( m_Sensoria == null ? new SensoriaObject( OwnerComponent ) : m_Sensoria ); }
			set{ Sensoria.Copy( value ); }
		}

		[SerializeField]
		private TemperaturObject m_Temperatur = null;
		public TemperaturObject Temperatur{
			get{ return m_Temperatur = ( m_Temperatur == null ? new TemperaturObject( OwnerComponent ) : m_Temperatur ); }
			set{ Temperatur.Copy( value ); }
		}

		[SerializeField]
		private InventoryObject m_Inventory = null;
		public InventoryObject Inventory{
			get{ return m_Inventory = ( m_Inventory == null ? new InventoryObject( OwnerComponent ) : m_Inventory ); }
			set{ Inventory.Copy( value ); }
		}

		[SerializeField]
		private MemoryObject m_Memory = null;
		public MemoryObject Memory{
			get{ return m_Memory = ( m_Memory == null ? new MemoryObject() : m_Memory ); }
			set{ Memory.Copy( value ); }
		}
	}

	[System.Serializable]
	public class StatusObject : StatusDataObject
	{
		public StatusObject(){}
		public StatusObject( StatusObject _object ) : base( _object ){}
		public StatusObject( ICEWorldBehaviour _component ) : base( _component ){}

		public delegate void OnRemoveRequestEvent( GameObject _sender );
		public event OnRemoveRequestEvent OnRemoveRequest;

		public override void Init( ICEWorldBehaviour _component )
		{
			Enabled = true;

			base.Init( _component );

			Sensoria.Init( OwnerComponent );
			Inventory.Init( OwnerComponent );
			Temperatur.Init( OwnerComponent );
			//Memory.Init( OwnerComponent );

			Reset();
		}


		/// <summary>
		/// Reset the status values.
		/// </summary>
		public override void Reset()
		{
			base.Reset();

			Enabled = true;

			RecoveryPhase = Random.Range( RecoveryPhaseMin, RecoveryPhaseMax );
			RemovingDelay = Random.Range( RemovingDelayMin, RemovingDelayMax );
			PerceptionTime = Random.Range( PerceptionTimeMin, PerceptionTimeMax ); 
			ReactionTime = Random.Range( ReactionTimeMin, ReactionTimeMax ); 

			m_ReactionTimer = ReactionTime;
			m_PerceptionTimer = PerceptionTime;
			m_RemoveTimer = 0.0f;
			m_RemoveRequired = false;

			Inventory.Reset();
			Corpse.Reset();

			//Sensoria.Reset();
			//Odour.Reset();
			//Temperatur.Reset();

			m_Age = 0;
			m_DamageInPercent = 0;
			m_StressInPercent = 0;
			m_DebilityInPercent = 0;
			m_HungerInPercent = 0;
			m_ThirstInPercent = 0;

			Aggressivity = DefaultAggressivity;
			Nosiness = DefaultNosiness;
			Experience = DefaultExperience;
			Anxiety = DefaultAnxiety;

			if( ! UseArmor )
				m_ArmorInPercent = 100;

			if( UseDynamicInitialisation )
				CalculateRandomStatusValues( TrophicLevel );
		}

		public void Kill()
		{
			m_Durability = 0;
			m_Age = MaxAge;
			m_DamageInPercent = 100;
			m_StressInPercent = 100;
			m_DebilityInPercent = 100;
			m_HungerInPercent = 100;
			m_ThirstInPercent = 100;

			if( ! UseArmor )
				m_ArmorInPercent = 0;
		}
		


		public float SpeedMultiplier{
			get{
				float current_value = 100;						
				current_value -= ( 100 - FitnessInPercent ) * FitnessSpeedMultiplier;						
				return FixedMultiplier( current_value / 100 );
			}
		}

		public bool RecreationRequired{
			get{ return ( FitnessRecreationLimit > 0 && FitnessInPercent <= FitnessRecreationLimit ? true : false ); }
		}

		public bool ReposeRequired{
			get{ return ( FitnessVitalityLimit > 0 && FitnessInPercent <= FitnessVitalityLimit ? true : false ); }
		}


		private TemperatureScaleType m_TemperatureScale;
		public TemperatureScaleType TemperatureScale
		{
			get{
				TemperatureScaleType _scale = (ICE.World.ICEWorldEnvironment.Instance != null?ICE.World.ICEWorldEnvironment.Instance.TemperatureScale:TemperatureScaleType.CELSIUS);

				if( m_TemperatureScale != _scale )
				{
					if( _scale == TemperatureScaleType.CELSIUS && m_TemperatureScale == TemperatureScaleType.FAHRENHEIT )
					{
						MaxEnvironmentTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( MaxEnvironmentTemperature );
						MinEnvironmentTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( MinEnvironmentTemperature );
						ComfortEnvironmentTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( ComfortEnvironmentTemperature );
					}
					else if( _scale == TemperatureScaleType.FAHRENHEIT && m_TemperatureScale == TemperatureScaleType.CELSIUS )
					{
						MaxEnvironmentTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( MaxEnvironmentTemperature );
						MinEnvironmentTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( MinEnvironmentTemperature );
						ComfortEnvironmentTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( ComfortEnvironmentTemperature );
					}
				}

				m_TemperatureScale = _scale;

				return m_TemperatureScale;
			}

		}

		public float EnvironmentTemperature{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.Temperature:0); }
		}

		public float EnvironmentMinTemperature{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.MinTemperature:0); }
		}

		public float EnvironmentMaxTemperature{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.MaxTemperature:0); }
		}

		public float TimeHour{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.TimeHour:0); }
		}

		public float TimeMinutes{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.TimeMinutes:0); }
		}

		public float TimeSeconds{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.TimeSeconds:0); }
		}

		public float DateDay{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.DateDay:0); }
		}

		public float DateMonth{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.DateMonth:0); }
		}

		public float DateYear{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.DateYear:0); }
		}

		public WeatherType Weather{
			get{ return (ICEWorldEnvironment.Instance != null?ICEWorldEnvironment.Instance.WeatherForecast:WeatherType.CLEAR); }
		}

		public float TemperatureDeviationInPercent
		{
			get{ 
				float _tmp_1 = 0;
				float _tmp_2 = 0;
				if( EnvironmentTemperature < ComfortEnvironmentTemperature )
				{
					_tmp_1 = Mathf.Abs(MinEnvironmentTemperature - ComfortEnvironmentTemperature);
					_tmp_2 = Mathf.Abs(MinEnvironmentTemperature - EnvironmentTemperature);
				}
				else
				{
					_tmp_1 = Mathf.Abs(MaxEnvironmentTemperature - ComfortEnvironmentTemperature);
					_tmp_2 = Mathf.Abs(MaxEnvironmentTemperature - EnvironmentTemperature);
				}

				return FixedPercent( 100 - ( 100/_tmp_1*_tmp_2 ) ); 
			}
		}
			
		public bool IsPerceptionForced = false;
		private float m_PerceptionTimer = 0.0f;
		public bool IsPerceptionTime( BehaviourModeRuleObject _rule )
		{
			if( IsDead )
				return false;

			float _delay = 0;
			if( UseAdvanced && PerceptionTimeFitnessMultiplier > 0 )
				_delay = ( ( 100 - FitnessInPercent ) * PerceptionTimeFitnessMultiplier * 0.1f );

			if( m_PerceptionTimer >= PerceptionTime + _delay || IsPerceptionForced )
			{
				if( _rule != null && _rule.Influences.Enabled && _rule.Influences.OverwritePerceptionTime )
					PerceptionTime = _rule.Influences.GetPerceiptionTime();
				else
					PerceptionTime = Random.Range( PerceptionTimeMin, PerceptionTimeMax ); 
				m_PerceptionTimer = 0;
				IsPerceptionForced = false;
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsReactionForced = false;
		private float m_ReactionTimer = 0.0f;
		public bool IsReactionTime( BehaviourModeRuleObject _rule )
		{
			if( IsDead )
				return false;

			if( m_ReactionTimer >= ReactionTime || IsReactionForced )
			{
				if( _rule != null && _rule.Influences.Enabled && _rule.Influences.OverwriteReactionTime )
					ReactionTime = _rule.Influences.GetReactionTime();
				else
					ReactionTime = Random.Range( ReactionTimeMin, ReactionTimeMax ); 
				m_ReactionTimer = 0;
				IsReactionForced = false;
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool m_RemoveRequired = false;
		private float m_RemoveTimer = 0.0f;
		public bool IsRemoveTime{
			get{ return ( m_RemoveRequired && m_RemoveTimer >= RemovingDelay )?true:false; }
		}

		public void RemoveRequest()
		{
			if( m_RemoveRequired || ! IsDead )
				return;

			//Debug.Log( "Creature.Status.RemoveRequest" );
			if( OnRemoveRequest != null )
				OnRemoveRequest( Owner );

			m_RemoveTimer = 0.0f;
			m_RemoveRequired = true;
		}

		/// <summary>
		/// Remove this instance according its reference settings
		/// </summary>
		public override void Remove()
		{
			if( ! m_RemoveRequired || ! IsDead )
				return;

			// Detaches the inventory items if required
			Inventory.DetachOnDie();

			// handles the base method to remove the object from the world
			base.Remove();
		}

		private float m_ArmorInPercent = 0;
		public float ArmorInPercent
		{
			set{ m_ArmorInPercent = value; }
			get{ return FixedPercent( m_ArmorInPercent ); }
		}

		/// <summary>
		/// Adds the armor in percent.
		/// </summary>
		/// <param name="_armor">Armor.</param>
		public void AddArmorInPercent( float _armor )
		{
			if( ! UseArmor )
				return;

			m_ArmorInPercent += _armor;
			m_ArmorInPercent = FixedPercent( m_ArmorInPercent );

			PrintDebugLog( this, "add " + _armor + "% to Armor (" + m_ArmorInPercent + ")" );
		}

		/// <summary>
		/// Handles the armor while receives damage.
		/// </summary>
		/// <returns>The armor.</returns>
		/// <param name="_damage">Damage.</param>
		private float HandleArmor( float _damage )
		{
			if( ! UseArmor )
				return _damage;

			if( _damage < 0 )
				return _damage;

			m_ArmorInPercent -= _damage;

			if( ArmorInPercent > 0 )
				_damage -= m_ArmorInPercent / 100;

			return _damage;
		}

		/// <summary>
		/// Resets the armor.
		/// </summary>
		public void ResetArmor()
		{
			if( UseArmor )
				m_ArmorInPercent = 100;
			else
				m_ArmorInPercent = 0;
		}

		public float Damage{
			set{ DamageInPercent = value * InitialDurabilityMultiplier; }
			get{ return DamageInPercent / InitialDurabilityMultiplier; }
		}

		public float Stress{
			set{ StressInPercent = value * InitialDurabilityMultiplier; }
			get{ return StressInPercent / InitialDurabilityMultiplier; }
		}

		public float Debility{
			set{ DebilityInPercent = value * InitialDurabilityMultiplier; }
			get{ return DebilityInPercent / InitialDurabilityMultiplier; }
		}

		public float Hunger{
			set{ HungerInPercent = value * InitialDurabilityMultiplier; }
			get{ return HungerInPercent / InitialDurabilityMultiplier; }
		}

		public float Thirst{
			set{ ThirstInPercent = value * InitialDurabilityMultiplier; }
			get{ return ThirstInPercent / InitialDurabilityMultiplier; }
		}

		private float m_DamageInPercent = 0;
		public float DamageInPercent
		{
			set{ m_DamageInPercent = FixedPercent( value ); }
			get{ return FixedPercent( m_DamageInPercent ); }
		}

		private float m_StressInPercent = 0;
		public float StressInPercent
		{
			set{ m_StressInPercent = FixedPercent( value ); }
			get{ return FixedPercent( m_StressInPercent ); }
		}
	
		private float m_DebilityInPercent = 0;
		public float DebilityInPercent
		{
			set{ m_DebilityInPercent = FixedPercent( value ); }
			get{ return FixedPercent( m_DebilityInPercent ); }
		}

		private float m_HungerInPercent = 0;
		public float HungerInPercent
		{
			set{ m_HungerInPercent = FixedPercent( value ); }
			get{ return FixedPercent( m_HungerInPercent ); }
		}

		private float m_ThirstInPercent = 0;
		public float ThirstInPercent
		{
			set{ m_ThirstInPercent = FixedPercent( value ); }
			get{ return FixedPercent( m_ThirstInPercent ); }
		}
			
		public override void AddDamage( float _damage ){
			AddDamageInPercent( _damage * InitialDurabilityMultiplier );
		}

		public void AddStress( float _stress ){
			AddStressInPercent( _stress * InitialDurabilityMultiplier );
		}

		public void AddDebility( float _debility ){
			AddDebilityInPercent( _debility * InitialDurabilityMultiplier );
		}

		public void AddHunger( float _hunger ){
			AddHungerInPercent( _hunger * InitialDurabilityMultiplier );
		}

		public void AddThirst( float _thirst ){
			AddThirstInPercent( _thirst * InitialDurabilityMultiplier );
		}

		/// <summary>
		/// Adds the damage in percent.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		public void AddDamageInPercent( float _damage )
		{
			_damage = HandleArmor( _damage );

			m_DamageInPercent += _damage;
			m_DamageInPercent = FixedPercent( m_DamageInPercent );

			PrintDebugLog( this, "add " + _damage + "% to Damage (" + m_DamageInPercent + ")" );
		}

		/// <summary>
		/// Adds the stress in percent.
		/// </summary>
		/// <param name="_stress">Stress.</param>
		public void AddStressInPercent( float _stress )
		{
			m_StressInPercent += _stress;
			m_StressInPercent = FixedPercent( m_StressInPercent );

			PrintDebugLog( this, "add " + _stress + "% to Stress (" + m_StressInPercent + ")" );
		}

		/// <summary>
		/// Adds the debility in percent.
		/// </summary>
		/// <param name="_debility">Debility.</param>
		public void AddDebilityInPercent( float _debility )
		{
			m_DebilityInPercent += _debility;
			m_DebilityInPercent = FixedPercent( m_DebilityInPercent );

			PrintDebugLog( this, "add " + _debility + "% to Debility (" + m_DebilityInPercent + ")" );
		}
			
		/// <summary>
		/// Adds the hunger in percent.
		/// </summary>
		/// <param name="_hunger">Hunger.</param>
		public void AddHungerInPercent( float _hunger )
		{
			m_HungerInPercent += _hunger;
			m_HungerInPercent = FixedPercent( m_HungerInPercent );

			PrintDebugLog( this, "add " + _hunger + "% to Hunger (" + m_HungerInPercent + ")" );
		}
			
		/// <summary>
		/// Adds the thirst in percent.
		/// </summary>
		/// <param name="_thirst">Thirst.</param>
		public void AddThirstInPercent( float _thirst )
		{
			m_ThirstInPercent += _thirst;
			m_ThirstInPercent = FixedPercent( m_ThirstInPercent );

			PrintDebugLog( this, "add " + _thirst + "% to Thirst (" + m_ThirstInPercent + ")" );
		}

		/// <summary>
		/// Adds the aggressivity.
		/// </summary>
		/// <param name="_value">Value.</param>
		public void AddAggressivity( float _value )
		{
			Aggressivity += _value;
			Aggressivity = FixedPercent( Aggressivity );

			PrintDebugLog( this, "add " + _value + "% to Aggressivity (" + Aggressivity + ")"   );
		}

		/// <summary>
		/// Adds the anxiety.
		/// </summary>
		/// <param name="_value">Value.</param>
		public void AddAnxiety( float _value )
		{
			Anxiety += _value;
			Anxiety = FixedPercent( Anxiety );

			PrintDebugLog( this, "add " + _value + "% to Anxiety (" + Anxiety + ")"   );
		}

		/// <summary>
		/// Adds the experience.
		/// </summary>
		/// <param name="_value">Value.</param>
		public void AddExperience( float _value )
		{
			Experience += _value;
			Experience = FixedPercent( Experience );

			PrintDebugLog( this, "add " + _value + "% to Experience (" + Experience + ")"   );
		}

		/// <summary>
		/// Adds the nosiness.
		/// </summary>
		/// <param name="_value">Value.</param>
		public void AddNosiness( float _value )
		{
			Nosiness += _value;
			Nosiness = FixedPercent( Nosiness );

			PrintDebugLog( this, "add " + _value + "% to Nosiness (" + Nosiness + ")"   );
		}


		/// <summary>
		/// Gets the aggressivity in percent.
		/// </summary>
		/// <value>The aggressivity in percent.</value>
		public float AggressivityInPercent
		{
			get{
				float _value = FixedPercent( Aggressivity ) / 100 * DefaultAggressivity;

				if( UseAdvanced )
				{
					_value += DamageInPercent * AggressivityDamageMultiplier;
					_value += StressInPercent * AggressivityStressMultiplier;
					_value += DebilityInPercent * AggressivityDebilityMultiplier;
					_value += HungerInPercent * AggressivityHungerMultiplier;
					_value += ThirstInPercent * AggressivityThirstMultiplier;
					_value += ( ( 100 - StaminaInPercent ) * AggressivityStaminaMultiplier );
					_value += ( ( 100 - HealthInPercent ) * AggressivityHealthMultiplier );
					_value += ( ( 100 - PowerInPercent ) * AggressivityPowerMultiplier );
					_value += ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * AggressivityTemperaturMultiplier : 0 );
					_value += ( UseAging ? ( 100 - LifespanInPercent ) * AggressivityAgeMultiplier : 0 );
				}

				_value = ( DefaultAggressivity > 0 ? _value * 100 / DefaultAggressivity : 0 );
				_value = ( IsDead ? 0 : _value );

				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the anxiety in percent.
		/// </summary>
		/// <value>The anxiety in percent.</value>
		public float AnxietyInPercent
		{
			get{
				float _value = FixedPercent( Anxiety ) / 100 * DefaultAnxiety;

				if( UseAdvanced )
				{
					_value += DamageInPercent * AnxietyDamageMultiplier;
					_value += StressInPercent * AnxietyStressMultiplier;
					_value += DebilityInPercent * AnxietyDebilityMultiplier;
					_value += HungerInPercent * AnxietyHungerMultiplier;
					_value += ThirstInPercent * AnxietyThirstMultiplier;
					_value += ( ( 100 - StaminaInPercent ) * AnxietyStaminaMultiplier );
					_value += ( ( 100 - HealthInPercent ) * AnxietyHealthMultiplier );
					_value += ( ( 100 - PowerInPercent ) * AnxietyPowerMultiplier );
					_value += ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * AnxietyTemperaturMultiplier : 0 );
					_value += ( UseAging ? ( 100 - LifespanInPercent ) * AnxietyAgeMultiplier : 0 );
				}

				_value = ( DefaultAnxiety > 0 ? _value * 100 / DefaultAnxiety : 0 );
				_value = ( IsDead ? 0 : _value );

				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the experience in percent.
		/// </summary>
		/// <value>The experience in percent.</value>
		public float ExperienceInPercent
		{
			get{
				float _value = FixedPercent( Experience ) / 100 * DefaultExperience;

				if( UseAdvanced )
				{
					_value += DamageInPercent * ExperienceDamageMultiplier;
					_value += StressInPercent * ExperienceStressMultiplier;
					_value += DebilityInPercent * ExperienceDebilityMultiplier;
					_value += HungerInPercent * ExperienceHungerMultiplier;
					_value += ThirstInPercent * ExperienceThirstMultiplier;
					_value += ( ( 100 - StaminaInPercent ) * ExperienceStaminaMultiplier );
					_value += ( ( 100 - HealthInPercent ) * ExperienceHealthMultiplier );
					_value += ( ( 100 - PowerInPercent ) * ExperiencePowerMultiplier );
					_value += ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * ExperienceTemperaturMultiplier : 0 );
					_value += ( UseAging ? ( 100 - LifespanInPercent ) * ExperienceAgeMultiplier : 0 );
				}

				_value = ( DefaultExperience > 0 ? _value * 100 / DefaultExperience : 0 );
				_value = ( IsDead ? 0 : _value );

				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the nosiness in percent.
		/// </summary>
		/// <value>The nosiness in percent.</value>
		public float NosinessInPercent
		{
			get{
				float _value = FixedPercent( Nosiness ) / 100 * DefaultNosiness;

				if( UseAdvanced )
				{
					_value += DamageInPercent * NosinessDamageMultiplier;
					_value += StressInPercent * NosinessStressMultiplier;
					_value += DebilityInPercent * NosinessDebilityMultiplier;
					_value += HungerInPercent * NosinessHungerMultiplier;
					_value += ThirstInPercent * NosinessThirstMultiplier;
					_value += ( ( 100 - StaminaInPercent ) * NosinessStaminaMultiplier );
					_value += ( ( 100 - HealthInPercent ) * NosinessHealthMultiplier );
					_value += ( ( 100 - PowerInPercent ) * NosinessPowerMultiplier );
					_value += ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * NosinessTemperaturMultiplier : 0 );
					_value += ( UseAging ? ( 100 - LifespanInPercent ) * NosinessAgeMultiplier : 0 );
				}

				_value = ( DefaultNosiness > 0 ? _value * 100 / DefaultNosiness : 0 );
				_value = ( IsDead ? 0 : _value );

				return FixedPercent( _value );
			}
		}




		/// <summary>
		/// Gets the senses in percent.
		/// </summary>
		/// <value>The senses in percent.</value>
		public float SensesInPercent{
			get{ return FixedPercent( ( IsDead ? 0 : ( SenseTactileInPercent + SenseGustatoryInPercent + SenseOlfactoryInPercent + SenseAuditoryInPercent + SenseVisualInPercent ) / 5 ) ); }
		}

		/// <summary>
		/// Gets the sense tactile in percent.
		/// </summary>
		/// <value>The sense tactile in percent.</value>
		public float SenseTactileInPercent
		{
			get{
				float _value = 100;

				if( Sensoria.Enabled )
				{
					_value = Sensoria.DefaultSenseTactile;
					_value -= ( 100 - FitnessInPercent ) * Sensoria.SenseTouchFitnessMultiplier;
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * Sensoria.SenseTouchAgeMultiplier : 0 );
				}

				_value = ( IsDead ? 0 : _value );

				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the sense gustatory in percent.
		/// </summary>
		/// <value>The sense gustatory in percent.</value>
		public float SenseGustatoryInPercent
		{
			get{
				float _value = 100;
				
				if( Sensoria.Enabled )
				{
					_value = Sensoria.DefaultSenseGustatory;
					_value -= ( 100 - FitnessInPercent ) * Sensoria.SenseGustatoryFitnessMultiplier;
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * Sensoria.SenseGustatoryAgeMultiplier : 0 );	
				}

				_value = ( IsDead ? 0 : _value );
					
				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the sense olfactory in percent.
		/// </summary>
		/// <value>The sense olfactory in percent.</value>
		public float SenseOlfactoryInPercent
		{
			get{
				float _value = 100;
				
				if( Sensoria.Enabled )
				{
					_value = Sensoria.DefaultSenseOlfactory;
					_value -= ( 100 - FitnessInPercent ) * Sensoria.SenseOlfactoryFitnessMultiplier;
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * Sensoria.SenseOlfactoryAgeMultiplier : 0 );
				}
					
				_value = ( IsDead ? 0 : _value );
				
				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the sense auditory in percent.
		/// </summary>
		/// <value>The sense auditory in percent.</value>
		public float SenseAuditoryInPercent
		{
			get{
				float _value = 100;
				
				if( Sensoria.Enabled )
				{
					_value = Sensoria.DefaultSenseAuditory;
					_value -= ( 100 - FitnessInPercent ) * Sensoria.SenseAuditoryFitnessMultiplier;
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * Sensoria.SenseAuditoryAgeMultiplier : 0 );	
				}

				_value = ( IsDead ? 0 : _value );
				
				return FixedPercent( _value );
			}
		}

		/// <summary>
		/// Gets the sense visual in percent.
		/// </summary>
		/// <value>The sense visual in percent.</value>
		public float SenseVisualInPercent
		{
			get{
				float _value = 100;

				if( Sensoria.Enabled )
				{
					_value = Sensoria.DefaultSenseVisual;
					_value -= ( 100 - FitnessInPercent ) * Sensoria.SenseVisualFitnessMultiplier;
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * Sensoria.SenseVisualAgeMultiplier : 0 );

				}

				_value = ( IsDead ? 0 : _value );

				return FixedPercent( _value );
			}
		}

		public override float DurabilityInPercent{
			get{ 
				
				UpdateDurabilityByPercent( ( UseAdvanced ? ( HealthInPercent + PowerInPercent + StaminaInPercent ) / 3 : HealthInPercent ) );

				return FixedPercent( m_InitialDurability > 0 ? 100 / m_InitialDurability * Durability:100 ); 
			}
		}

		public float FitnessInPercent{
			get{ return DurabilityInPercent; }
		}

		public float HealthInPercent
		{
			get{
				float _value = 100;

				if( UseAdvanced )
				{
					_value -= DamageInPercent * HealthDamageMultiplier;
					_value -= StressInPercent * HealthStressMultiplier;
					_value -= DebilityInPercent * HealthDebilityMultiplier;
					_value -= HungerInPercent * HealthHungerMultiplier;
					_value -= ThirstInPercent * HealthThirstMultiplier;
					_value -= ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * HealthTemperaturMultiplier : 0 );
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * HealthAgeMultiplier : 0 );

					if( UseAging && m_Age >= MaxAge )
						_value = 0;
				}
				else
				{
					_value -= DamageInPercent;
				}
							
				return FixedPercent( _value );
			}
		}

		public float StaminaInPercent
		{
			get{
				float _value = 100;

				if( UseAdvanced )
				{
					_value -= DamageInPercent * StaminaDamageMultiplier;
					_value -= StressInPercent * StaminaStressMultiplier;
					_value -= HungerInPercent * StaminaHungerMultiplier;
					_value -= ThirstInPercent * StaminaThirstMultiplier;
					_value -= DebilityInPercent * StaminaDebilityMultiplier;
					_value -= ( ( 100 - HealthInPercent ) * StaminaHealthMultiplier );
					_value -= ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * StaminaTemperaturMultiplier : 0 );
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * StaminaAgeMultiplier : 0 );
				}

				return FixedPercent( _value );
			}
		}

		public float PowerInPercent
		{
			get{
				float _value = 100;

				if( UseAdvanced )
				{
					_value -= StressInPercent * PowerStressMultiplier;
					_value -= DebilityInPercent * PowerDebilityMultiplier;
					_value -= DamageInPercent * PowerDamageMultiplier;
					_value -= HungerInPercent * PowerHungerMultiplier;
					_value -= ThirstInPercent * PowerThirstMultiplier;
					_value -= ( ( 100 - StaminaInPercent ) * PowerStaminaMultiplier );
					_value -= ( ( 100 - HealthInPercent ) * PowerHealthMultiplier );
					_value -= ( UseEnvironmentTemperature ? TemperatureDeviationInPercent * PowerTemperaturMultiplier : 0 );
					_value -= ( UseAging ? ( 100 - LifespanInPercent ) * PowerAgeMultiplier : 0 );
				}

				return FixedPercent( _value );
			}
		}


		public bool IsSpawning{
			get{ return ( Lifetime <= RecoveryPhase )?true:false; }
		}

		public bool IsDead{
			get{ return IsDestroyed; }
		}

		public override bool IsDestroyed{
			get{ return ( IsDestructible && FitnessInPercent <= 0 ? true:false ); }
		}

		private float m_WoundedTimer = 0;
		public bool IsWounded{
			get{ return ( m_WoundedTimer > 0 ? true : false ); }
		}

		public void EarlyUpdate( Vector3 _velocity )
		{
			base.Update();

			// reduce wound timer
			m_WoundedTimer = Mathf.Max( 0, m_WoundedTimer - Time.deltaTime );


	
			// if respawn is required the creature is dead and doesn't need anymore information 
			if( m_RemoveRequired )
			{
				m_RemoveTimer += Time.deltaTime;
				m_PerceptionTimer = 0;
				m_ReactionTimer = 0;
			}
			else
			{
				m_ReactionTimer += Time.deltaTime;
				m_PerceptionTimer += Time.deltaTime;

				if( UseAging )
				{
					if( m_Age >= MaxAge )
					{
						AddDamage( 100 );
						AddStress( 100 );
						AddHunger( 100 );
						AddThirst( 100 );
						AddDebility( 100 );

						AddAggressivity( 0 );
						AddAnxiety( 0 );
						AddExperience( 0 );
						AddNosiness( 0 );
					}
				}
			}
		}

		public bool LateUpdate()
		{
			if( IsDead )
			{
				RemoveRequest();

				if( IsRemoveTime )
					Remove();

				return false;
			}
			else
				return true;
		}
			
		public void ResetDefaultValues()
		{
			SetAge( 0 );
			MaxAge = 3600;
			
			ComfortEnvironmentTemperature = 25;
			MinEnvironmentTemperature = -25;
			MaxEnvironmentTemperature = 50;
			UseArmor = false;
			ArmorInPercent = 100;
			
			PerceptionTime = 0.5f;
			PerceptionTimeFitnessMultiplier = 0;
			RemovingDelay = 20;

			CalculateRandomStatusValues( TrophicLevelType.UNDEFINED );
		}

		/// <summary>
		/// Determinates if the specified object is in the field of view.
		/// </summary>
		/// <returns><c>true</c>, if in field of view was objected, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_visible">Visible.</param>
		/// <param name="_detected_position">Detected position.</param>
		public bool ObjectInFieldOfView( GameObject _object, ref bool _visible, ref Vector3 _detected_position )
		{
			_visible = false;

			if( Owner == null || _object == null )
				return _visible;
			
			// FOV check isn't required or creatures fov settings OFF or adjusted to a full-circle with an infinity distance
			else if( Sensoria.Enabled == false || ( ( Sensoria.FieldOfView == 0 || Sensoria.FieldOfView == 180 ) && Sensoria.VisualRange == 0 ) )
				_visible = true;

			if( _visible == false )
			{

				float _target_distance = PositionTools.Distance( Owner.transform.position, _object.transform.position );

				//distance test - if the target is too far, we don't need further checks ... 
				if( Sensoria.VisualRange == 0 || Sensoria.VisualRange >= _target_distance )
				{
					float _angle = PositionTools.GetSignedDirectionAngle( Owner.transform , _object.transform.position );

					// FOV test - the target must be inside the given range
					_visible = ( Mathf.Abs( _angle ) <= Sensoria.FieldOfView ? true : false );
				}
			}

			if( _visible ) 
				_detected_position = _object.transform.position;
		
			return _visible;
		}

		/// <summary>
		/// Determinates if the specified object is visible.
		/// </summary>
		/// <returns><c>true</c>, if is visible was objected, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_visible">Visible.</param>
		/// <param name="_detected_position">Detected position.</param>
		public bool ObjectIsVisible( GameObject _object, ref bool _visible, ref Vector3 _detected_position )
		{
			_visible = false;

			if( Owner == null || _object == null || Sensoria.Enabled == false )
				return _visible;

			if( ObjectVisibility( _object ) > 0 )
				_visible = true;

			if( _visible )
				_detected_position = _object.transform.position;

			return _visible;
		}

		/// <summary>
		/// Determinates if the specified object is audible.
		/// </summary>
		/// <returns><c>true</c>, if is audible was objected, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_audible">Audible.</param>
		/// <param name="_detected_position">Detected position.</param>
		public bool ObjectIsAudible( GameObject _object, ref bool _audible, ref Vector3 _detected_position )
		{
			_audible = false;

			if( Owner == null || _object == null || Sensoria.Enabled == false )
				return _audible;

			if( ObjectAudibility( _object ) > 0 )
				_audible = true;

			if( _audible )
				_detected_position = _object.transform.position;

			return _audible;
		}

		/// <summary>
		/// Determinates if the specified object is smellable.
		/// </summary>
		/// <returns><c>true</c>, if is smellable was objected, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_smellable">Smellable.</param>
		/// <param name="_detected_position">Detected position.</param>
		/// <param name="_object_odour">Object odour.</param>
		public bool ObjectIsSmellable( GameObject _object, ref bool _smellable, ref Vector3 _detected_position, OdourObject _object_odour  )
		{
			_smellable = false;

			if( Owner == null || _object == null || Sensoria.Enabled == false )
				return _smellable;

			if( ObjectSmellability( _object, _object_odour ) > 0 )
				_smellable = true;

			if( _smellable )
				_detected_position = _object.transform.position;
			
			return _smellable;
		}

		/// <summary>
		/// Determinates the visibility of the specified object.
		/// </summary>
		/// <returns>The visibility.</returns>
		/// <param name="_object">Object.</param>
		public float ObjectVisibility( GameObject _object )
		{
			if( Owner == null || _object == null )
				return 0;

			float _visibility = 0;

			Vector3 _sensor_position = Sensoria.VisualSensorPosition;
			Vector3 _origin_position = _sensor_position;
			Vector3 _target_position = _object.transform.position;
			float _horizontal_offset = Sensoria.VisualSensorHorizontalOffset;
			float _sensor_visual_range = Sensoria.VisualRange;

			Collider[] _colliders = _object.GetComponentsInChildren<Collider>();

			RaycastHit _hit;
			foreach( Collider _collider in _colliders )
			{
				_target_position = _collider.transform.position;

				float _distance = PositionTools.Distance( _sensor_position, _target_position );

				if( _sensor_visual_range == 0 || _sensor_visual_range >= _distance )
				{
					if( _horizontal_offset > 0 )
						_origin_position = Vector3.Lerp( _sensor_position, _target_position, MathTools.Normalize( _horizontal_offset, 0, _distance ) );

					bool _has_hit = false;

					if( Sensoria.UseSphereCast )
					{
						Vector3 dir = (_target_position - _origin_position).normalized;
						_has_hit = Physics.SphereCast( _origin_position, Sensoria.SphereCastRadius, dir, out _hit ); 
					}
					else
					{
						_has_hit = Physics.Linecast( _origin_position, _target_position, out _hit );
					}
											
					if( _has_hit ) 
					{
						if( _hit.transform.IsChildOf( _object.transform ) || _hit.transform.name == _object.name )
						{
							float _m = ( _sensor_visual_range > 0 ? Mathf.Clamp01( 1 - MathTools.Normalize( _distance, 0, _sensor_visual_range ) ) : 1 );
							_visibility = _m * _m;
							break;
						}
						else
						{
							DebugLine( _origin_position, _hit.point, Color.yellow );
							DebugLine( _hit.point, _target_position, Color.red );
						}
					}
					else
						DebugLine( _origin_position, _target_position, Color.red );
				}
			}

			if( _visibility > 0 )
			{
				_visibility = Mathf.Clamp01(_visibility ) * SenseVisualInPercent;
				DebugLine( _origin_position, _target_position, Color.green );
				PrintDebugLog( this, "ObjectVisibility : object " + _object.name + "' is visible (" + _visibility + ")" );
			}


			return _visibility;
		}

		/// <summary>
		/// Determinates the audibility of the specified object.
		/// </summary>
		/// <returns>The audibility.</returns>
		/// <param name="_object">Object.</param>
		public float ObjectAudibility( GameObject _object )
		{
			if( Owner == null || _object == null )
				return 0;

			float _audibility = 0;
			float _distance = PositionTools.Distance( _object.transform.position, Owner.transform.position );

			AudioSource[] _sources = _object.GetComponentsInChildren<AudioSource>();

			if( _sources != null && _sources.Length > 0 )
			{
				foreach( AudioSource _source in _sources )
				{
					if( _source.isPlaying )
					{
						float _m = Mathf.Clamp01( 1 - MathTools.Normalize( _distance, 0, _source.maxDistance ) );
						_audibility += (_source.volume * _m);
					}
				}
			}

			if( _audibility > 0 )
			{
				_audibility = Mathf.Clamp01(_audibility ) * SenseAuditoryInPercent;
				DebugLine( _object.transform.position + Vector3.up, Owner.transform.position + Vector3.up, Color.blue );
				PrintDebugLog( this, "ObjectAudibility : object " + _object.name + "' is hearable (" + _audibility + ")" );
			}

			return _audibility;
		}

		/// <summary>
		/// /// Determinates the smellability of the specified object.
		/// </summary>
		/// <returns>The smellability.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_object_odour">Object odour.</param>
		public float ObjectSmellability( GameObject _object, OdourObject _object_odour )
		{
			if( Owner == null || _object == null )
				return 0;

			float _smellability = 0;
			float _distance = PositionTools.Distance( _object.transform.position, Owner.transform.position );

			ICECreatureOdourAttribute[] _odours = _object.GetComponentsInChildren<ICECreatureOdourAttribute>();

			if( _odours != null && _odours.Length > 0 )
			{
				foreach( ICECreatureOdourAttribute _odour in _odours )
				{
					if( _odour.enabled )
					{
						float _m = Mathf.Clamp01( 1 - MathTools.Normalize( _distance, 0, _odour.OdourRange ) );
						_smellability += (_odour.OdourIntensity * _m);
					}
				}
			}

			if( _object_odour != null && _object_odour.Type != OdourType.NONE )
				_smellability += _object_odour.GetIntensityByDistance( _distance );

			if( _smellability > 0 )
			{
				_smellability = Mathf.Clamp01(_smellability ) * SenseOlfactoryInPercent/100;
				DebugLine( _object.transform.position + Vector3.up, Owner.transform.position + Vector3.up, Color.blue );
				PrintDebugLog( this, "ObjectSmellability : object " + _object.name + "' is smellable (" + _smellability + ")" );
			}

			return _smellability;
		}


		public void CalculateRandomStatusValues( TrophicLevelType _type )
		{
			if( _type == TrophicLevelType.UNDEFINED ) 
			{
				DamageInPercent = 0;
				StressInPercent = 0;
				DebilityInPercent = 0; 
				HungerInPercent = 0;
				ThirstInPercent = 0; 

				Sensoria.DefaultSenseVisual = 100; 
				Sensoria.SenseVisualAgeMultiplier = 0.0f;
				Sensoria.SenseVisualFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseAuditory = 100;
				Sensoria.SenseAuditoryAgeMultiplier = 0.0f;
				Sensoria.SenseAuditoryFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseOlfactory = 100; 
				Sensoria.SenseOlfactoryAgeMultiplier = 0.0f;
				Sensoria.SenseOlfactoryFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseGustatory = 100; 
				Sensoria.SenseGustatoryAgeMultiplier = 0.0f;
				Sensoria.SenseGustatoryFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseTactile = 100; 
				Sensoria.SenseTouchAgeMultiplier = 0.0f;
				Sensoria.SenseTouchFitnessMultiplier = 0.0f;

				Aggressivity = 25;
				DefaultAggressivity = 25;
				AggressivityHealthMultiplier = 0;
				AggressivityStaminaMultiplier = 0;
				AggressivityPowerMultiplier = 0;
				AggressivityDamageMultiplier = 0;
				AggressivityStressMultiplier = 0;
				AggressivityDebilityMultiplier = 0;
				AggressivityHungerMultiplier = 0;
				AggressivityThirstMultiplier = 0;
				AggressivityAgeMultiplier = 0;
				AggressivityTemperaturMultiplier = 0;

				Anxiety = 0;
				DefaultAnxiety = 0;
				AnxietyHealthMultiplier = 0;
				AnxietyStaminaMultiplier = 0;
				AnxietyPowerMultiplier = 0;
				AnxietyDamageMultiplier  = 0;
				AnxietyStressMultiplier = 0;
				AnxietyDebilityMultiplier = 0;
				AnxietyHungerMultiplier = 0;
				AnxietyThirstMultiplier = 0;
				AnxietyAgeMultiplier = 0;
				AnxietyTemperaturMultiplier = 0;

				Experience = 0;
				DefaultExperience = 0;
				ExperienceHealthMultiplier = 0;
				ExperienceStaminaMultiplier = 0;
				ExperiencePowerMultiplier = 0;
				ExperienceDamageMultiplier  = 0;
				ExperienceStressMultiplier = 0;
				ExperienceDebilityMultiplier = 0;
				ExperienceHungerMultiplier = 0;
				ExperienceThirstMultiplier = 0;
				ExperienceAgeMultiplier = 0;
				ExperienceTemperaturMultiplier = 0;

				Nosiness = 0;
				DefaultNosiness = 0;
				NosinessHealthMultiplier = 0;
				NosinessStaminaMultiplier = 0;
				NosinessPowerMultiplier = 0;
				NosinessDamageMultiplier  = 0;
				NosinessStressMultiplier = 0;
				NosinessDebilityMultiplier = 0;
				NosinessHungerMultiplier = 0;
				NosinessThirstMultiplier = 0;
				NosinessAgeMultiplier = 0;
				NosinessTemperaturMultiplier = 0;

				HealthDamageMultiplier = 1;
				HealthStressMultiplier = 0;
				HealthDebilityMultiplier = 0;
				HealthHungerMultiplier = 0;
				HealthThirstMultiplier = 0;
				HealthAgeMultiplier = 0;
				HealthTemperaturMultiplier = 0;

				StaminaHealthMultiplier = 0;
				StaminaDamageMultiplier = 0;
				StaminaStressMultiplier = 0;							
				StaminaDebilityMultiplier = 0;
				StaminaHungerMultiplier = 0;
				StaminaThirstMultiplier = 0;
				StaminaAgeMultiplier = 0;
				StaminaTemperaturMultiplier = 0;

				PowerHealthMultiplier = 0;
				PowerStaminaMultiplier = 0;
				PowerDamageMultiplier  = 0;
				PowerStressMultiplier = 0;
				PowerDebilityMultiplier = 0;
				PowerHungerMultiplier = 0;
				PowerThirstMultiplier = 0;
				PowerAgeMultiplier = 0;
				PowerTemperaturMultiplier = 0;


			}
			else
			{
				if( UseAging )
					SetAge( Random.Range( 0, MaxAge ) );

				DamageInPercent = Random.Range( 0,25 );
				StressInPercent = Random.Range( 0,10 );
				DebilityInPercent = Random.Range( 0.05f,10 ); 
				HungerInPercent = Random.Range( 0.25f,50f );
				ThirstInPercent = Random.Range( 0.25f,50f ); 

				Sensoria.DefaultSenseVisual = 100; 
				Sensoria.SenseVisualAgeMultiplier = 0.0f;
				Sensoria.SenseVisualFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseAuditory = 100;
				Sensoria.SenseAuditoryAgeMultiplier = 0.0f;
				Sensoria.SenseAuditoryFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseOlfactory = 100; 
				Sensoria.SenseOlfactoryAgeMultiplier = 0.0f;
				Sensoria.SenseOlfactoryFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseGustatory = 100; 
				Sensoria.SenseGustatoryAgeMultiplier = 0.0f;
				Sensoria.SenseGustatoryFitnessMultiplier = 0.0f;

				Sensoria.DefaultSenseTactile = 100; 
				Sensoria.SenseTouchAgeMultiplier = 0.0f;
				Sensoria.SenseTouchFitnessMultiplier = 0.0f;

				if( _type == TrophicLevelType.CARNIVORE )
				{
					// CHARACTER
					Aggressivity = Random.Range( 50,95 );
					DefaultAggressivity = Aggressivity;
						AggressivityHealthMultiplier = Random.Range( -0.25f,-0.05f );
						AggressivityStaminaMultiplier = Random.Range( -0.45f,-0.15f );
						AggressivityPowerMultiplier = Random.Range( -0.95f,-0.15f );
						AggressivityDamageMultiplier = Random.Range( 0.55f,0.75f );
						AggressivityStressMultiplier = Random.Range( 0.50f,0.75f );
						AggressivityDebilityMultiplier = Random.Range( 0.25f,0.45f );
						AggressivityHungerMultiplier = Random.Range( 0.05f,0.25f );
						AggressivityThirstMultiplier = Random.Range( 0.05f,0.25f );
						AggressivityAgeMultiplier = Random.Range( -0.45f,-0.15f );
						AggressivityTemperaturMultiplier = Random.Range( 0.2f,0.35f );
					
					Anxiety = Random.Range( 5,15 );
					DefaultAnxiety = Anxiety;
						AnxietyHealthMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyStaminaMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyPowerMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyDamageMultiplier  = Random.Range( 0.25f,0.45f );
						AnxietyStressMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyDebilityMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyHungerMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyThirstMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyAgeMultiplier = Random.Range( 0.25f,0.45f );
						AnxietyTemperaturMultiplier = Random.Range( 0.25f,0.45f );
					
					Experience = Random.Range( 25,45 );
					DefaultExperience = Experience;
						ExperienceHealthMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceStaminaMultiplier = Random.Range( 0.25f,0.45f );
						ExperiencePowerMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceDamageMultiplier  = Random.Range( 0.25f,0.45f );
						ExperienceStressMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceDebilityMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceHungerMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceThirstMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceAgeMultiplier = Random.Range( 0.25f,0.45f );
						ExperienceTemperaturMultiplier = Random.Range( 0.25f,0.45f );
					
					Nosiness = Random.Range( 75,100 );
					DefaultNosiness = Nosiness;
						NosinessHealthMultiplier = Random.Range( 0.25f,0.45f );
						NosinessStaminaMultiplier = Random.Range( 0.25f,0.45f );
						NosinessPowerMultiplier = Random.Range( 0.25f,0.45f );
						NosinessDamageMultiplier  = Random.Range( 0.25f,0.45f );
						NosinessStressMultiplier = Random.Range( 0.25f,0.45f );
						NosinessDebilityMultiplier = Random.Range( 0.25f,0.45f );
						NosinessHungerMultiplier = Random.Range( 0.25f,0.45f );
						NosinessThirstMultiplier = Random.Range( 0.25f,0.45f );
						NosinessAgeMultiplier = Random.Range( 0.25f,0.45f );
						NosinessTemperaturMultiplier = Random.Range( 0.25f,0.45f );

					// VITAL SIGNS
						HealthDamageMultiplier = Random.Range( 0.5f,0.9f );
						HealthStressMultiplier = Random.Range( 0.35f,0.55f );
						HealthDebilityMultiplier = Random.Range( 0.15f,0.25f );
						HealthHungerMultiplier = Random.Range( 0.55f,0.75f );
						HealthThirstMultiplier = Random.Range( 0.65f,0.80f );
						HealthAgeMultiplier = Random.Range( 0.5f,0.75f );
						HealthTemperaturMultiplier = Random.Range( 0.25f,0.35f );
	
						StaminaHealthMultiplier = Random.Range( 0.15f,0.25f );
						StaminaDamageMultiplier = Random.Range( 0.25f,0.35f );
						StaminaStressMultiplier = Random.Range( 0.15f,0.25f );
						StaminaDebilityMultiplier = Random.Range( 0.15f,0.25f );
						StaminaHungerMultiplier = Random.Range( 0.15f,0.25f );
						StaminaThirstMultiplier = Random.Range( 0.15f,0.25f );
						StaminaAgeMultiplier = Random.Range( 0.5f,0.75f );
						StaminaTemperaturMultiplier = Random.Range( 0.15f,0.25f );

						PowerHealthMultiplier = Random.Range( 0.15f,0.25f );
						PowerStaminaMultiplier = Random.Range( 0.15f,0.25f );
						PowerDamageMultiplier  = Random.Range( 0.15f,0.25f );
						PowerStressMultiplier = Random.Range( 0.15f,0.25f );
						PowerDebilityMultiplier = Random.Range( 0.15f,0.25f );
						PowerHungerMultiplier = Random.Range( 0.15f,0.25f );
						PowerThirstMultiplier = Random.Range( 0.15f,0.25f );
						PowerAgeMultiplier = Random.Range( 0.5f,0.75f );
						PowerTemperaturMultiplier = Random.Range( 0.15f,0.25f );
					

				}
				else if( _type == TrophicLevelType.OMNIVORES )
				{
					// CHARACTER
					Aggressivity = Random.Range( 25,65 );
					DefaultAggressivity = Aggressivity;
					AggressivityHealthMultiplier = Random.Range( -0.25f,-0.05f );
					AggressivityStaminaMultiplier = Random.Range( -0.45f,-0.15f );
					AggressivityPowerMultiplier = Random.Range( -0.95f,-0.15f );
					AggressivityDamageMultiplier = Random.Range( 0.55f,0.75f );
					AggressivityStressMultiplier = Random.Range( 0.50f,0.75f );
					AggressivityDebilityMultiplier = Random.Range( 0.25f,0.45f );
					AggressivityHungerMultiplier = Random.Range( 0.05f,0.25f );
					AggressivityThirstMultiplier = Random.Range( 0.05f,0.25f );
					AggressivityAgeMultiplier = Random.Range( -0.45f,-0.15f );
					AggressivityTemperaturMultiplier = Random.Range( 0.2f,0.35f );

					Anxiety = Random.Range( 5,15 );
					DefaultAnxiety = Anxiety;
					AnxietyHealthMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyStaminaMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyPowerMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyDamageMultiplier  = Random.Range( 0.25f,0.45f );
					AnxietyStressMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyDebilityMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyHungerMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyThirstMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyAgeMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyTemperaturMultiplier = Random.Range( 0.25f,0.45f );

					Experience = Random.Range( 25,45 );
					DefaultExperience = Experience;
					ExperienceHealthMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceStaminaMultiplier = Random.Range( 0.25f,0.45f );
					ExperiencePowerMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceDamageMultiplier  = Random.Range( 0.25f,0.45f );
					ExperienceStressMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceDebilityMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceHungerMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceThirstMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceAgeMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceTemperaturMultiplier = Random.Range( 0.25f,0.45f );

					Nosiness = Random.Range( 75,95 );
					DefaultNosiness = Nosiness;
					NosinessHealthMultiplier = Random.Range( 0.25f,0.45f );
					NosinessStaminaMultiplier = Random.Range( 0.25f,0.45f );
					NosinessPowerMultiplier = Random.Range( 0.25f,0.45f );
					NosinessDamageMultiplier  = Random.Range( 0.25f,0.45f );
					NosinessStressMultiplier = Random.Range( 0.25f,0.45f );
					NosinessDebilityMultiplier = Random.Range( 0.25f,0.45f );
					NosinessHungerMultiplier = Random.Range( 0.25f,0.45f );
					NosinessThirstMultiplier = Random.Range( 0.25f,0.45f );
					NosinessAgeMultiplier = Random.Range( 0.25f,0.45f );
					NosinessTemperaturMultiplier = Random.Range( 0.25f,0.45f );


					HealthDamageMultiplier = Random.Range( 0.5f,0.9f );
					HealthStressMultiplier = Random.Range( 0.35f,0.55f );
					HealthDebilityMultiplier = Random.Range( 0.15f,0.25f );
					HealthHungerMultiplier = Random.Range( 0.55f,0.75f );
					HealthThirstMultiplier = Random.Range( 0.65f,0.80f );
					HealthAgeMultiplier = Random.Range( 0.5f,0.75f );
					HealthTemperaturMultiplier = Random.Range( 0.25f,0.35f );

					StaminaHealthMultiplier = Random.Range( 0.15f,0.25f );
					StaminaDamageMultiplier = Random.Range( 0.25f,0.35f );
					StaminaStressMultiplier = Random.Range( 0.15f,0.25f );
					StaminaDebilityMultiplier = Random.Range( 0.15f,0.25f );
					StaminaHungerMultiplier = Random.Range( 0.15f,0.25f );
					StaminaThirstMultiplier = Random.Range( 0.15f,0.25f );
					StaminaAgeMultiplier = Random.Range( 0.5f,0.75f );
					StaminaTemperaturMultiplier = Random.Range( 0.15f,0.25f );

					PowerHealthMultiplier = Random.Range( 0.15f,0.25f );
					PowerStaminaMultiplier = Random.Range( 0.15f,0.25f );
					PowerDamageMultiplier  = Random.Range( 0.15f,0.25f );
					PowerStressMultiplier = Random.Range( 0.15f,0.25f );
					PowerDebilityMultiplier = Random.Range( 0.15f,0.25f );
					PowerHungerMultiplier = Random.Range( 0.15f,0.25f );
					PowerThirstMultiplier = Random.Range( 0.15f,0.25f );
					PowerAgeMultiplier = Random.Range( 0.5f,0.75f );
					PowerTemperaturMultiplier = Random.Range( 0.15f,0.25f );
				}
				else if( _type == TrophicLevelType.HERBIVORE )
				{
					// CHARACTER
					Aggressivity = Random.Range( 0,25 );
					DefaultAggressivity = Aggressivity;
					AggressivityHealthMultiplier = Random.Range( -0.25f,-0.05f );
					AggressivityStaminaMultiplier = Random.Range( -0.45f,-0.15f );
					AggressivityPowerMultiplier = Random.Range( -0.95f,-0.15f );
					AggressivityDamageMultiplier = Random.Range( 0.55f,0.75f );
					AggressivityStressMultiplier = Random.Range( 0.50f,0.75f );
					AggressivityDebilityMultiplier = Random.Range( 0.25f,0.45f );
					AggressivityHungerMultiplier = Random.Range( 0.05f,0.25f );
					AggressivityThirstMultiplier = Random.Range( 0.05f,0.25f );
					AggressivityAgeMultiplier = Random.Range( -0.45f,-0.15f );
					AggressivityTemperaturMultiplier = Random.Range( 0.2f,0.35f );

					Anxiety = Random.Range( 35,75 );
					DefaultAnxiety = Anxiety;
					AnxietyHealthMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyStaminaMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyPowerMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyDamageMultiplier  = Random.Range( 0.25f,0.45f );
					AnxietyStressMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyDebilityMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyHungerMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyThirstMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyAgeMultiplier = Random.Range( 0.25f,0.45f );
					AnxietyTemperaturMultiplier = Random.Range( 0.25f,0.45f );

					Experience = Random.Range( 25,75 );
					DefaultExperience = Experience;
					ExperienceHealthMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceStaminaMultiplier = Random.Range( 0.25f,0.45f );
					ExperiencePowerMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceDamageMultiplier  = Random.Range( 0.25f,0.45f );
					ExperienceStressMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceDebilityMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceHungerMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceThirstMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceAgeMultiplier = Random.Range( 0.25f,0.45f );
					ExperienceTemperaturMultiplier = Random.Range( 0.25f,0.45f );

					Nosiness = Random.Range( 25,75 );
					DefaultNosiness = Nosiness;
					NosinessHealthMultiplier = Random.Range( 0.25f,0.45f );
					NosinessStaminaMultiplier = Random.Range( 0.25f,0.45f );
					NosinessPowerMultiplier = Random.Range( 0.25f,0.45f );
					NosinessDamageMultiplier  = Random.Range( 0.25f,0.45f );
					NosinessStressMultiplier = Random.Range( 0.25f,0.45f );
					NosinessDebilityMultiplier = Random.Range( 0.25f,0.45f );
					NosinessHungerMultiplier = Random.Range( 0.25f,0.45f );
					NosinessThirstMultiplier = Random.Range( 0.25f,0.45f );
					NosinessAgeMultiplier = Random.Range( 0.25f,0.45f );
					NosinessTemperaturMultiplier = Random.Range( 0.25f,0.45f );

	
					HealthDamageMultiplier = Random.Range( 0.5f,0.9f );
					HealthStressMultiplier = Random.Range( 0.35f,0.55f );
					HealthDebilityMultiplier = Random.Range( 0.15f,0.25f );
					HealthHungerMultiplier = Random.Range( 0.55f,0.75f );
					HealthThirstMultiplier = Random.Range( 0.65f,0.80f );
					HealthAgeMultiplier = Random.Range( 0.5f,0.75f );
					HealthTemperaturMultiplier = Random.Range( 0.25f,0.35f );

					StaminaHealthMultiplier = Random.Range( 0.15f,0.25f );
					StaminaDamageMultiplier = Random.Range( 0.25f,0.35f );
					StaminaStressMultiplier = Random.Range( 0.15f,0.25f );
					StaminaDebilityMultiplier = Random.Range( 0.15f,0.25f );
					StaminaHungerMultiplier = Random.Range( 0.15f,0.25f );
					StaminaThirstMultiplier = Random.Range( 0.15f,0.25f );
					StaminaAgeMultiplier = Random.Range( 0.5f,0.75f );
					StaminaTemperaturMultiplier = Random.Range( 0.15f,0.25f );

					PowerHealthMultiplier = Random.Range( 0.15f,0.25f );
					PowerStaminaMultiplier = Random.Range( 0.15f,0.25f );
					PowerDamageMultiplier  = Random.Range( 0.15f,0.25f );
					PowerStressMultiplier = Random.Range( 0.15f,0.25f );
					PowerDebilityMultiplier = Random.Range( 0.15f,0.25f );
					PowerHungerMultiplier = Random.Range( 0.15f,0.25f );
					PowerThirstMultiplier = Random.Range( 0.15f,0.25f );
					PowerAgeMultiplier = Random.Range( 0.5f,0.75f );
					PowerTemperaturMultiplier = Random.Range( 0.15f,0.25f );
				}
			}
		}

	}

}
