// ##############################################################################
//
// ice_creature_editor_status.cs | StatusEditor
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
using ICE.World;
using ICE.World.EditorUtilities;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;




namespace ICE.Creatures.EditorUtilities
{
	
	public static class StatusEditor
	{	
		public static void Print( ICECreatureControl _control ){
			
			if( ! _control.Display.ShowStatus )
				return;
			
			ICEEditorStyle.SplitterByIndent( 0 );
			ICEEditorLayout.BeginHorizontal();
				_control.Display.FoldoutStatus = ICEEditorLayout.Foldout( _control.Display.FoldoutStatus, "Status" );
				if( ICEEditorLayout.SaveButton() )
					CreatureEditorIO.SaveStatusToFile( _control.Creature.Status, _control.gameObject.name );
				if( ICEEditorLayout.LoadButton() )
					_control.Creature.Status = CreatureEditorIO.LoadStatusFromFile( _control.Creature.Status );				
				if( ICEEditorLayout.ResetButton() )
					_control.Creature.Status.ResetDefaultValues();
			ICEEditorLayout.EndHorizontal( Info.STATUS );
			
			if( _control.Display.FoldoutStatus ) 
			{
				EditorGUILayout.Separator();
				
				DrawStatus( _control );
				DrawStatusBasics( _control );
				DrawStatusAdvanced( _control );
				DrawStatusCorpse( _control );
				DrawStatusSensoria( _control );
				DrawStatusMemory( _control );
				DrawStatusInventory( _control );
	
				EditorGUILayout.Separator();
			}
		}

		private static void DrawStatus( ICECreatureControl _control )
		{
			if( ! _control.Display.ShowStatus )
				return;

			EditorGUI.indentLevel++;

			_control.Display.FoldoutStatusInfos = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusInfos, "Infos", Info.STATUS_INFLUENCE_INDICATORS );				
			if( _control.Display.FoldoutStatusInfos )
			{
				ICEEditorLayout.DrawProgressBar( "Durability", _control.Status.DurabilityInPercent );

				if( _control.Creature.Status.UseAging )
				{
					EditorGUILayout.LabelField( "Age", "Current age : " + (int)(_control.Creature.Status.Age/60) + "min. / Max. age : " + (int)(_control.Creature.Status.MaxAge/60) + "min." );
					EditorGUI.indentLevel++;					
						ICEEditorLayout.DrawProgressBar("Lifespan", _control.Creature.Status.LifespanInPercent);					
					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();
				}	

				if( _control.Creature.Status.UseAdvanced )
				{

					ICEEditorLayout.DrawProgressBar("Fitness (major vital sign)", _control.Creature.Status.FitnessInPercent);				
					EditorGUILayout.Separator();				
					EditorGUI.indentLevel++;				
						ICEEditorLayout.DrawProgressBar("Health", _control.Creature.Status.HealthInPercent );
						ICEEditorLayout.DrawProgressBar("Stamina", _control.Creature.Status.StaminaInPercent );
						ICEEditorLayout.DrawProgressBar("Power", _control.Creature.Status.PowerInPercent );				
					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();

					ICEEditorLayout.DrawProgressBar("Senses (major sensory signs)", _control.Creature.Status.SensesInPercent);				
					EditorGUILayout.Separator();				
					EditorGUI.indentLevel++;				
						ICEEditorLayout.DrawProgressBar("Visual", _control.Creature.Status.SenseVisualInPercent );
						ICEEditorLayout.DrawProgressBar("Auditory", _control.Creature.Status.SenseAuditoryInPercent );
						ICEEditorLayout.DrawProgressBar("Olfactory", _control.Creature.Status.SenseOlfactoryInPercent );	
						ICEEditorLayout.DrawProgressBar("Gustatory", _control.Creature.Status.SenseGustatoryInPercent );	
						ICEEditorLayout.DrawProgressBar("Tactile", _control.Creature.Status.SenseTactileInPercent );	
					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();

					ICEEditorLayout.Label( "Character (major character traits)", false );
					EditorGUI.indentLevel++;
						ICEEditorLayout.DrawProgressBar("Aggressivity", _control.Creature.Status.AggressivityInPercent );
						ICEEditorLayout.DrawProgressBar("Anxiety", _control.Creature.Status.AnxietyInPercent );
						ICEEditorLayout.DrawProgressBar("Experience", _control.Creature.Status.ExperienceInPercent );
						ICEEditorLayout.DrawProgressBar("Nosiness", _control.Creature.Status.NosinessInPercent );
					EditorGUI.indentLevel--;
				}
				else
				{
					ICEEditorLayout.DrawProgressBar("Health (Durability)", _control.Creature.Status.HealthInPercent );
				}

				EditorGUILayout.Separator();
			}
			
			EditorGUI.indentLevel--;

		}

		private static void DrawStatusBasics( ICECreatureControl _control )
		{
			_control.Creature.Status.Enabled = true;

			if( ! _control.Display.ShowStatus )
				return;

			EditorGUI.indentLevel++;
			ICEEditorLayout.BeginHorizontal();
				_control.Display.FoldoutStatusBasics = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusBasics, "Basics" );	
			ICEEditorLayout.EndHorizontal( Info.STATUS_BASICS );

			if( _control.Display.FoldoutStatusBasics )
			{
				EditorGUI.indentLevel++;

					EditorGUILayout.Separator();
					CreatureObjectEditor.DrawInitialDurability( _control.Creature.Status );

					//EditorGUI.BeginDisabledGroup(  _control.Creature.Status.UseAdvanced == true );
				//	CreatureObjectEditor.DrawDurabilitySlider( _control.Creature.Status );
				//EditorGUI.EndDisabledGroup();

					EditorGUI.BeginDisabledGroup( _control.Creature.Status.IsDestructible == false );
					EditorGUI.indentLevel++;
						if( _control.Creature.Status.UseAdvanced )
						{
							_control.Creature.Status.DamageInPercent = ICEEditorLayout.DefaultSlider( "Damage","", _control.Creature.Status.DamageInPercent,0.025f, 0,100,0, Info.STATUS_DAMAGE_IN_PERCENT);
							_control.Creature.Status.StressInPercent = ICEEditorLayout.DefaultSlider("Stress","", _control.Creature.Status.StressInPercent,0.025f, 0,100,0, Info.STATUS_STRESS_IN_PERCENT);
							_control.Creature.Status.DebilityInPercent = ICEEditorLayout.DefaultSlider("Debility","", _control.Creature.Status.DebilityInPercent,0.025f, 0,100,0, Info.STATUS_DEBILITY_IN_PERCENT);
							_control.Creature.Status.HungerInPercent = ICEEditorLayout.DefaultSlider( "Hunger","", _control.Creature.Status.HungerInPercent,0.025f, 0,100,0, Info.STATUS_HUNGER_IN_PERCENT);
							_control.Creature.Status.ThirstInPercent = ICEEditorLayout.DefaultSlider("Thirst","", _control.Creature.Status.ThirstInPercent,0.025f, 0,100,0, Info.STATUS_THIRST_IN_PERCENT);

							EditorGUILayout.Separator();

							_control.Creature.Status.Aggressivity = ICEEditorLayout.DefaultSlider("Aggressivity","", _control.Creature.Status.Aggressivity, 0.025f, 0, 100, _control.Creature.Status.DefaultAggressivity, Info.STATUS_INFLUENCES_AGGRESSIVITY );
							_control.Creature.Status.Experience = ICEEditorLayout.DefaultSlider("Experience","", _control.Creature.Status.Experience, 0.025f, 0, 100, _control.Creature.Status.DefaultExperience, Info.STATUS_INFLUENCES_EXPERIENCE );
							_control.Creature.Status.Nosiness = ICEEditorLayout.DefaultSlider("Nosiness","", _control.Creature.Status.Nosiness, 0.025f, 0, 100, _control.Creature.Status.DefaultNosiness, Info.STATUS_INFLUENCES_NOSINESS );
							_control.Creature.Status.Anxiety = ICEEditorLayout.DefaultSlider("Anxiety","", _control.Creature.Status.Anxiety, 0.025f, 0, 100, _control.Creature.Status.DefaultAnxiety, Info.STATUS_INFLUENCES_ANXIETY );
						}
						else
							_control.Creature.Status.DamageInPercent = ICEEditorLayout.Slider( "Damage","", _control.Creature.Status.DamageInPercent, Init.DECIMAL_PRECISION, 0,100, Info.STATUS_DAMAGE_IN_PERCENT );
					
						EditorGUILayout.Separator();
						CreatureObjectEditor.DrawDamageTransfer( _control.Creature.Status, Info.STATUS_DAMAGE_TRANSFER_MULTIPLIER );
					EditorGUI.indentLevel--;
					EditorGUI.EndDisabledGroup();
					

					EditorGUILayout.Separator();
					CreatureObjectEditor.DrawStatusMass( _control.Creature.Status );

					ICEEditorLayout.MinMaxDefaultSlider( "Perception Time (secs.)", "", 					
						ref _control.Creature.Status.PerceptionTimeMin, 
						ref _control.Creature.Status.PerceptionTimeMax,
						0,
						10,
						0.4f,
						0.6f,
						Init.DECIMAL_PRECISION_TIMER,
						40,
						Info.STATUS_PERCEPTION_TIME );

					if( _control.Creature.Status.UseAdvanced )
					{
						EditorGUI.indentLevel++;
							_control.Creature.Status.PerceptionTimeFitnessMultiplier = ICEEditorLayout.DefaultSlider("Fitness Multiplier (inv. +)","", _control.Creature.Status.PerceptionTimeFitnessMultiplier,0.025f, 0,1, 0.3f, Info.STATUS_REACTION_TIME_MULTIPLIER );
						EditorGUI.indentLevel--;
					}

					ICEEditorLayout.MinMaxDefaultSlider( "Reaction Time (secs.)", "", 					
						ref _control.Creature.Status.ReactionTimeMin, 
						ref _control.Creature.Status.ReactionTimeMax,
						0,
						2,
						0.1f,
						0.2f,
						Init.DECIMAL_PRECISION_TIMER,
						40,
						Info.STATUS_REACTION_TIME );

					EditorGUILayout.Separator();

					ICEEditorLayout.MinMaxRandomDefaultSlider("Recovery Phase (secs.)","Defines how long the creature will be defenceless after spawning.", ref _control.Creature.Status.RecoveryPhaseMin, ref _control.Creature.Status.RecoveryPhaseMax, 0, ref _control.Creature.Status.RecoveryPhaseMaximum, 0.01f, 0.1f, Init.DECIMAL_PRECISION_TIMER, 30, Info.STATUS_RECOVERY_PHASE );
					ICEEditorLayout.MinMaxRandomDefaultSlider("Removing Delay (secs.)","Defines how long the creature will be visible after dying and before respawning.", ref _control.Creature.Status.RemovingDelayMin, ref _control.Creature.Status.RemovingDelayMax, 0, ref _control.Creature.Status.RemovingDelayMaximum, 0, 0, Init.DECIMAL_PRECISION_TIMER, 30, Info.STATUS_REMOVING_DELAY );

					EditorGUILayout.Separator();
					_control.Creature.Status.FitnessRecreationLimit = ICEEditorLayout.DefaultSlider("Recreation Limit (%)","If the fitness value reached this limit your creature will go home to recreate.", _control.Creature.Status.FitnessRecreationLimit, 0.5f, 0, 100,0, Info.STATUS_FITNESS_RECREATION_LIMIT );
					_control.Creature.Status.FitnessVitalityLimit = ICEEditorLayout.DefaultSlider("Vitality Limit (%)","If the fitness value reached this limit your creature will be to weak for further activities.", _control.Creature.Status.FitnessVitalityLimit, 0.5f, 0, 100,0, Info.STATUS_FITNESS_VITALITY_LIMIT );

					EditorGUILayout.Separator();

					// ODOUR BEGIN
					CreatureObjectEditor.DrawOdourObject( _control.Creature.Status.Odour );
					// ODOUR END

					// GENDER BEGIN
					_control.Creature.Status.GenderType = (CreatureGenderType)ICEEditorLayout.EnumPopup( "Gender","", _control.Creature.Status.GenderType, Info.STATUS_GENDER  ); 
					// GENDER END

					// TROPHIC LEVEL BEGIN
					ICEEditorLayout.BeginHorizontal();
						_control.Creature.Status.TrophicLevel = (TrophicLevelType)ICEEditorLayout.EnumPopup( "Trophic Level","", _control.Creature.Status.TrophicLevel  ); 
						if( ICEEditorLayout.RandomButton( "" ) )
							_control.Creature.Status.CalculateRandomStatusValues( _control.Creature.Status.TrophicLevel );
						_control.Creature.Status.UseDynamicInitialisation = ICEEditorLayout.CheckButtonSmall( "DYN", "", _control.Creature.Status.UseDynamicInitialisation );
					ICEEditorLayout.EndHorizontal( Info.STATUS_FEEDTYPE );
					EditorGUI.indentLevel++;
						if( _control.Creature.Status.TrophicLevel == TrophicLevelType.OMNIVORES || _control.Creature.Status.TrophicLevel == TrophicLevelType.CARNIVORE )
							_control.Creature.Status.IsCannibal = ICEEditorLayout.Toggle( "Is Cannibal","", _control.Creature.Status.IsCannibal, Info.STATUS_FEEDTYPE_CANNIBAL  ); 
						else
							_control.Creature.Status.IsCannibal = false;
					EditorGUI.indentLevel--;
					// TROPHIC LEVEL END

					EditorGUILayout.Separator();

					WorldObjectEditor.DrawStatusAging( _control.Creature.Status );
	

					_control.Creature.Status.UseEnvironmentTemperature = ICEEditorLayout.Toggle( "Use Environment Temperature","", _control.Creature.Status.UseEnvironmentTemperature, Info.STATUS_TEMPERATURE );			
					if( _control.Creature.Status.UseEnvironmentTemperature )
					{
						EditorGUI.indentLevel++;
							_control.Creature.Status.ComfortEnvironmentTemperature = ICEEditorLayout.Slider( "Comfort Environment Temperature","", _control.Creature.Status.ComfortEnvironmentTemperature, 1,_control.Creature.Status.MinEnvironmentTemperature,_control.Creature.Status.MaxEnvironmentTemperature, Info.STATUS_TEMPERATURE_BEST );
							EditorGUI.indentLevel++;
								ICEEditorLayout.MinMaxSlider( "Temperature Scope", "Minimal and maximal Temperatures", 
								ref _control.Creature.Status.MinEnvironmentTemperature, 
								ref _control.Creature.Status.MaxEnvironmentTemperature,
								_control.Creature.Status.EnvironmentMinTemperature,
								_control.Creature.Status.EnvironmentMaxTemperature,
								1,
								40,
								Info.STATUS_TEMPERATURE_SCOPE );
							EditorGUI.indentLevel--;
						EditorGUI.indentLevel--;

						EditorGUILayout.Separator();
					}

					_control.Creature.Status.UseArmor = ICEEditorLayout.Toggle( "Use Armor","", _control.Creature.Status.UseArmor, Info.STATUS_ARMOR );
					if( _control.Creature.Status.UseArmor )
					{
						EditorGUI.indentLevel++;
						_control.Creature.Status.ArmorInPercent = ICEEditorLayout.DefaultSlider( "Armor","", _control.Creature.Status.ArmorInPercent, 1,0,100, 100, Info.STATUS_ARMOR_IN_PERCENT );
						EditorGUI.indentLevel--;
					}
						
					_control.Creature.Status.UseShelter = ICEEditorLayout.Toggle("Use Shelter","", _control.Creature.Status.UseShelter, Info.STATUS_SHELTER );
					if( _control.Creature.Status.UseShelter )
					{
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.IsSheltered )
								GUI.backgroundColor = Color.green;
							_control.Creature.Status.ShelterTag = ICEEditorLayout.Tag( "Shelter Tag", "", _control.Creature.Status.ShelterTag, Info.STATUS_SHELTER_TAG );
							GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();
					}

					_control.Creature.Status.UseIndoor = ICEEditorLayout.Toggle("Use Indoor","", _control.Creature.Status.UseIndoor, Info.STATUS_INDOOR );	
					if( _control.Creature.Status.UseIndoor )
					{
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.IsIndoor )
								GUI.backgroundColor = Color.green;
							_control.Creature.Status.IndoorTag = ICEEditorLayout.Tag( "Indoor Tag", "", _control.Creature.Status.IndoorTag, Info.STATUS_INDOOR_TAG );
							GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();
					}


		
				EditorGUI.indentLevel--;
				
				EditorGUILayout.Separator();
			}
			EditorGUI.indentLevel--;
		}



		private static void DrawStatusAdvanced( ICECreatureControl _control )
		{
			EditorGUI.indentLevel++;
				ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _control.Creature.Status.UseAdvanced == false );
						_control.Display.FoldoutStatusAdvanced = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusAdvanced, "Dynamic Vital Signs" );	
					EditorGUI.EndDisabledGroup();
					_control.Creature.Status.UseAdvanced = ICEEditorLayout.EnableButton( _control.Creature.Status.UseAdvanced );
				ICEEditorLayout.EndHorizontal( Info.STATUS_ADVANCED );
			EditorGUI.indentLevel--;

			if( ! _control.Display.FoldoutStatusAdvanced )
				return;

			EditorGUI.indentLevel++;
				EditorGUI.BeginDisabledGroup( _control.Creature.Status.UseAdvanced == false );
				EditorGUI.indentLevel++;

					_control.Display.FoldoutStatusVital = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusVital, "Vital Indicators", Info.STATUS_VITAL_INDICATORS );
					if( _control.Display.FoldoutStatusVital )
					{
						EditorGUI.indentLevel++;
							ICEEditorLayout.Label("Health", true );
							EditorGUI.indentLevel++;			
								_control.Creature.Status.HealthDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.HealthDamageMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,1);
								_control.Creature.Status.HealthStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.HealthStressMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.HealthDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.HealthDebilityMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.HealthHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.HealthHungerMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.HealthThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.HealthThirstMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								EditorGUILayout.Separator();
			
								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.HealthTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.HealthTemperaturMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.HealthAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.HealthAgeMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								
								EditorGUILayout.Separator();
								_control.Creature.Status.HealthRecreationMultiplier = ICEEditorLayout.DefaultSlider("Recreation Multiplier","", _control.Creature.Status.HealthRecreationMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);

							EditorGUI.indentLevel--;

							EditorGUILayout.Separator();	
							ICEEditorLayout.Label("Stamina", true );
							EditorGUI.indentLevel++;			
								_control.Creature.Status.StaminaDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.StaminaDamageMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,1);
								_control.Creature.Status.StaminaStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.StaminaStressMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.StaminaDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.StaminaDebilityMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.StaminaHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.StaminaHungerMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.StaminaThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.StaminaThirstMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								EditorGUILayout.Separator();
								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.StaminaTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.StaminaTemperaturMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.StaminaAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.StaminaAgeMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								EditorGUILayout.Separator();
								_control.Creature.Status.StaminaHealthMultiplier = ICEEditorLayout.DefaultSlider("Health Multiplier","", _control.Creature.Status.StaminaHealthMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
							
							EditorGUI.indentLevel--;

							EditorGUILayout.Separator();	
							ICEEditorLayout.Label("Power", true );
							EditorGUI.indentLevel++;
								_control.Creature.Status.PowerDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.PowerDamageMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,-1);
								_control.Creature.Status.PowerStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.PowerStressMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.PowerDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.PowerDebilityMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.PowerHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.PowerHungerMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.PowerThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.PowerThirstMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								EditorGUILayout.Separator();
								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.PowerTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.PowerTemperaturMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.PowerAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.PowerAgeMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								EditorGUILayout.Separator();
								_control.Creature.Status.PowerHealthMultiplier = ICEEditorLayout.DefaultSlider("Health Multiplier","", _control.Creature.Status.PowerHealthMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);
								_control.Creature.Status.PowerStaminaMultiplier = ICEEditorLayout.DefaultSlider("Stamina Multiplier","", _control.Creature.Status.PowerStaminaMultiplier, Init.DECIMAL_PRECISION_INDICATOR,-1,1,0);

							EditorGUI.indentLevel--;	
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();
					}
				
					_control.Display.FoldoutStatusCharacter = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusCharacter, "Character Indicators", Info.STATUS_CHARACTER_INDICATORS );				
					if( _control.Display.FoldoutStatusCharacter )
					{
						EditorGUI.indentLevel++;
							_control.Creature.Status.DefaultAggressivity = ICEEditorLayout.DefaultSlider("Aggressivity (%)","", _control.Creature.Status.DefaultAggressivity, 0.25f, 0, 100, 25, Info.STATUS_CHARACTER_DEFAULT_AGGRESSITY );
							EditorGUI.indentLevel++;			
								_control.Creature.Status.AggressivityDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.AggressivityDamageMultiplier,0.025f,-1,1,0);
								_control.Creature.Status.AggressivityStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.AggressivityStressMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AggressivityDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.AggressivityDebilityMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AggressivityHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.AggressivityHungerMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AggressivityThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.AggressivityThirstMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AggressivityHealthMultiplier = ICEEditorLayout.DefaultSlider("Health Multiplier","", _control.Creature.Status.AggressivityHealthMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AggressivityStaminaMultiplier = ICEEditorLayout.DefaultSlider("Stamina Multiplier","", _control.Creature.Status.AggressivityStaminaMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AggressivityPowerMultiplier = ICEEditorLayout.DefaultSlider("Power Multiplier","", _control.Creature.Status.AggressivityPowerMultiplier, 0.025f,-1,1,0);


								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.AggressivityTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.AggressivityTemperaturMultiplier, 0.025f,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.AggressivityAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.AggressivityAgeMultiplier, 0.025f,-1,1,0);
							EditorGUI.indentLevel--;

							EditorGUILayout.Separator();
							_control.Creature.Status.DefaultAnxiety = ICEEditorLayout.DefaultSlider("Anxiety (%)","", _control.Creature.Status.DefaultAnxiety, 0.25f, 0, 100, 0, Info.STATUS_CHARACTER_DEFAULT_ANXIETY );
							EditorGUI.indentLevel++;			
								_control.Creature.Status.AnxietyDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.AnxietyDamageMultiplier,0.025f,-1,1,0);
								_control.Creature.Status.AnxietyStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.AnxietyStressMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AnxietyDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.AnxietyDebilityMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AnxietyHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.AnxietyHungerMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AnxietyThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.AnxietyThirstMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AnxietyHealthMultiplier = ICEEditorLayout.DefaultSlider("Health Multiplier","", _control.Creature.Status.AnxietyHealthMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AnxietyStaminaMultiplier = ICEEditorLayout.DefaultSlider("Stamina Multiplier","", _control.Creature.Status.AnxietyStaminaMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.AnxietyPowerMultiplier = ICEEditorLayout.DefaultSlider("Power Multiplier","", _control.Creature.Status.AnxietyPowerMultiplier, 0.025f,-1,1,0);

								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.AnxietyTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.AnxietyTemperaturMultiplier, 0.025f,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.AnxietyAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.AnxietyAgeMultiplier, 0.025f,-1,1,0);
							EditorGUI.indentLevel--;

							EditorGUILayout.Separator();
							_control.Creature.Status.DefaultExperience = ICEEditorLayout.DefaultSlider("Experience (%)","", _control.Creature.Status.DefaultExperience, 0.25f, 0, 100, 0, Info.STATUS_CHARACTER_DEFAULT_EXPERIENCE );
							EditorGUI.indentLevel++;			
								_control.Creature.Status.ExperienceDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.ExperienceDamageMultiplier,0.025f,-1,1,0);
								_control.Creature.Status.ExperienceStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.ExperienceStressMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.ExperienceDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.ExperienceDebilityMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.ExperienceHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.ExperienceHungerMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.ExperienceThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.ExperienceThirstMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.ExperienceHealthMultiplier = ICEEditorLayout.DefaultSlider("Health Multiplier","", _control.Creature.Status.ExperienceHealthMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.ExperienceStaminaMultiplier = ICEEditorLayout.DefaultSlider("Stamina Multiplier","", _control.Creature.Status.ExperienceStaminaMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.ExperiencePowerMultiplier = ICEEditorLayout.DefaultSlider("Power Multiplier","", _control.Creature.Status.ExperiencePowerMultiplier, 0.025f,-1,1,0);

								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.ExperienceTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.ExperienceTemperaturMultiplier, 0.025f,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.ExperienceAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.ExperienceAgeMultiplier, 0.025f,-1,1,0);
							EditorGUI.indentLevel--;

							EditorGUILayout.Separator();
							_control.Creature.Status.DefaultNosiness = ICEEditorLayout.DefaultSlider("Nosiness (%)","", _control.Creature.Status.DefaultNosiness, 0.25f, 0, 100, 0, Info.STATUS_CHARACTER_DEFAULT_NOSINESS );
							EditorGUI.indentLevel++;			
								_control.Creature.Status.NosinessDamageMultiplier = ICEEditorLayout.DefaultSlider("Damage Multiplier","", _control.Creature.Status.NosinessDamageMultiplier,0.025f,-1,1,0);
								_control.Creature.Status.NosinessStressMultiplier = ICEEditorLayout.DefaultSlider("Stress Multiplier","", _control.Creature.Status.NosinessStressMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.NosinessDebilityMultiplier = ICEEditorLayout.DefaultSlider("Debility Multiplier","", _control.Creature.Status.NosinessDebilityMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.NosinessHungerMultiplier = ICEEditorLayout.DefaultSlider("Hunger Multiplier","", _control.Creature.Status.NosinessHungerMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.NosinessThirstMultiplier = ICEEditorLayout.DefaultSlider("Thirst Multiplier","", _control.Creature.Status.NosinessThirstMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.NosinessHealthMultiplier = ICEEditorLayout.DefaultSlider("Health Multiplier","", _control.Creature.Status.NosinessHealthMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.NosinessStaminaMultiplier = ICEEditorLayout.DefaultSlider("Stamina Multiplier","", _control.Creature.Status.NosinessStaminaMultiplier, 0.025f,-1,1,0);
								_control.Creature.Status.NosinessPowerMultiplier = ICEEditorLayout.DefaultSlider("Power Multiplier","", _control.Creature.Status.NosinessPowerMultiplier, 0.025f,-1,1,0);

								if( _control.Creature.Status.UseEnvironmentTemperature )
									_control.Creature.Status.NosinessTemperaturMultiplier = ICEEditorLayout.DefaultSlider("Temperatur Multiplier","", _control.Creature.Status.NosinessTemperaturMultiplier, 0.025f,-1,1,0);
								if( _control.Creature.Status.UseAging )
									_control.Creature.Status.NosinessAgeMultiplier = ICEEditorLayout.DefaultSlider("Aging Multiplier","", _control.Creature.Status.NosinessAgeMultiplier, 0.025f,-1,1,0);
							EditorGUI.indentLevel--;
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();
					}

					_control.Display.FoldoutStatusDynamicInfluences = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusDynamicInfluences, "Dynamic Influences", Info.STATUS_DYNAMIC_INFLUENCES );
					if( _control.Display.FoldoutStatusDynamicInfluences )
					{
						EditorGUI.indentLevel++;
							EditorGUILayout.LabelField( "Fitness" );
							EditorGUI.indentLevel++;
								_control.Creature.Status.FitnessSpeedMultiplier = ICEEditorLayout.Slider("Velocity Multiplier (-)","", _control.Creature.Status.FitnessSpeedMultiplier, 0.025f,0,1);
							EditorGUI.indentLevel--;
						EditorGUI.indentLevel--;
					}

				EditorGUI.indentLevel--;
				EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel--;

		}


		/// <summary>
		/// Draws the status corpse.
		/// </summary>
		/// <param name="_control">Control.</param>
		private static void DrawStatusCorpse( ICECreatureControl _control )
		{
			EditorGUI.indentLevel++;
			CreatureObjectEditor.DrawCorpseObject( _control, _control.Creature.Status.Corpse, EditorHeaderType.FOLDOUT_ENABLED_BOLD );
			EditorGUI.indentLevel--;
		}

		/// <summary>
		/// Draws the status inventory.
		/// </summary>
		/// <param name="_control">Control.</param>
		private static void DrawStatusInventory( ICECreatureControl _control )
		{
			EditorGUI.indentLevel++;
			CreatureObjectEditor.DrawInventoryObject( _control.gameObject, _control.Creature.Status.Inventory, EditorHeaderType.FOLDOUT_ENABLED_BOLD );
			EditorGUI.indentLevel--;
		}

		/// <summary>
		/// Draws the status memory.
		/// </summary>
		/// <param name="_control">Control.</param>
		private static void DrawStatusMemory( ICECreatureControl _control )
		{
			EditorGUI.indentLevel++;
			CreatureObjectEditor.DrawMemoryObject( _control, _control.Creature.Status.Memory, EditorHeaderType.FOLDOUT_ENABLED_BOLD );
			EditorGUI.indentLevel--;
		}

		/// <summary>
		/// Draws the status sensoria.
		/// </summary>
		/// <param name="_control">Control.</param>
		private static void DrawStatusSensoria( ICECreatureControl _control )
		{
			EditorGUI.indentLevel++;
			CreatureObjectEditor.DrawSensoriaObject( _control, _control.Creature.Status.Sensoria, EditorHeaderType.FOLDOUT_ENABLED_BOLD );
			EditorGUI.indentLevel--;
		}
		
	}
}