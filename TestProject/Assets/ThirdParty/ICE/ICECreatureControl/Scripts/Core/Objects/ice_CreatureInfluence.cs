// ##############################################################################
//
// ice_CreatureInfluence.cs
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

using ICE.Creatures.Utilities;
using ICE.World.Utilities;

using ICE.World;
using ICE.World.Objects;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class InfluenceDataObject : ICEDataObject
	{
		public InfluenceDataObject(){}
		public InfluenceDataObject( InfluenceDataObject _object ) : base( _object )
		{ Copy( _object ); }

		public void Copy( InfluenceDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			UseDamageInPercent = _object.UseDamageInPercent;
			UseStressInPercent = _object.UseStressInPercent;
			UseDebilityInPercent = _object.UseDebilityInPercent;
			UseHungerInPercent = _object.UseHungerInPercent;
			UseThirstInPercent = _object.UseThirstInPercent;

			Damage = _object.Damage;
			DamageMin = _object.DamageMin;
			DamageMax = _object.DamageMax;
			UseDamageRange = _object.UseDamageRange;

			Stress = _object.Stress;
			StressMin = _object.StressMin;
			StressMax = _object.StressMax;
			UseStressRange = _object.UseStressRange;

			Debility = _object.Debility;
			Hunger = _object.Hunger;
			Thirst = _object.Thirst;

			Aggressivity = _object.Aggressivity;
			Anxiety = _object.Anxiety;
			Experience = _object.Experience;
			Nosiness = _object.Nosiness;

			OverwritePerceptionTime = _object.OverwritePerceptionTime;
			PerceptionTimeMin = _object.PerceptionTimeMin;
			PerceptionTimeMax = _object.PerceptionTimeMax;

			OverwriteReactionTime = _object.OverwriteReactionTime;
			ReactionTimeMin = _object.ReactionTimeMin;
			ReactionTimeMax = _object.ReactionTimeMax;
		}

		public float Damage;
		public float DamageMin;
		public float DamageMax;
		public bool UseDamageRange;
		public bool UseDamageInPercent;

		public float Stress;
		public float StressMin;
		public float StressMax;
		public bool UseStressRange;
		public bool UseStressInPercent;

		public float Debility;
		public float DebilityMin;
		public float DebilityMax;
		public bool UseDebilityRange;
		public bool UseDebilityInPercent;

		public float Hunger;
		public float HungerMin;
		public float HungerMax;
		public bool UseHungerRange;
		public bool UseHungerInPercent;

		public float Thirst;
		public float ThirstMin;
		public float ThirstMax;
		public bool UseThirstRange;
		public bool UseThirstInPercent;

		public float Aggressivity;
		public float Anxiety;
		public float Experience;
		public float Nosiness;

		public bool OverwriteReactionTime = false;
		public float ReactionTimeMin = 0.1f;
		public float ReactionTimeMax = 0.2f;

		public bool OverwritePerceptionTime = false;
		public float PerceptionTimeMin = 0.4f;
		public float PerceptionTimeMax = 0.6f;
	}

	[System.Serializable]
	public class BehaviourInfluenceObject : InfluenceObject
	{
		public BehaviourInfluenceObject(){}
		public BehaviourInfluenceObject( BehaviourInfluenceObject _object ) : base( _object ) { Copy( _object ); }
		public BehaviourInfluenceObject( InfluenceDataObject _object ) { Copy( _object ); }

		public void Copy( BehaviourInfluenceObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			OverwritePerceptionTime = _object.OverwritePerceptionTime;
			PerceptionTimeMin = _object.PerceptionTimeMin;
			PerceptionTimeMax = _object.PerceptionTimeMax;

			OverwriteReactionTime = _object.OverwriteReactionTime;
			ReactionTimeMin = _object.ReactionTimeMin;
			ReactionTimeMax = _object.ReactionTimeMax;
		}

		public bool OverwriteReactionTime = false;
		public float ReactionTimeMin = 0.1f;
		public float ReactionTimeMax = 0.2f;

		public bool OverwritePerceptionTime = false;
		public float PerceptionTimeMin = 0.4f;
		public float PerceptionTimeMax = 0.6f;

		public float GetPerceiptionTime(){
			return UnityEngine.Random.Range( PerceptionTimeMin, PerceptionTimeMax ); 
		}

		public float GetReactionTime(){
			return UnityEngine.Random.Range( ReactionTimeMin, ReactionTimeMax ); 
		}
	}

	[System.Serializable]
	public class InfluenceObject : ICETimerObject
	{
		public InfluenceObject(){}
		public InfluenceObject( InfluenceObject _object ) : base( _object ) { Copy( _object ); }
		public InfluenceObject( InfluenceDataObject _object ) { Copy( _object ); }

		public void Copy( InfluenceObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			SetImpulseData( _object );

			UseDamageInPercent = _object.UseDamageInPercent;
			UseStressInPercent = _object.UseStressInPercent;
			UseDebilityInPercent = _object.UseDebilityInPercent;
			UseHungerInPercent = _object.UseHungerInPercent;
			UseThirstInPercent = _object.UseThirstInPercent;

			Damage = _object.Damage;
			DamageMin = _object.DamageMin;
			DamageMax = _object.DamageMax;
			UseDamageRange = _object.UseDamageRange;

			Stress = _object.Stress;
			StressMin = _object.StressMin;
			StressMax = _object.StressMax;
			UseStressRange = _object.UseStressRange;

			Debility = _object.Debility;
			DebilityMin = _object.DebilityMin;
			DebilityMax = _object.DebilityMax;
			UseDebilityRange = _object.UseDebilityRange;

			Hunger = _object.Hunger;
			HungerMin = _object.HungerMin;
			HungerMax = _object.HungerMax;
			UseHungerRange = _object.UseHungerRange;

			Thirst = _object.Thirst;
			ThirstMin = _object.ThirstMin;
			ThirstMax = _object.ThirstMax;
			UseThirstRange = _object.UseThirstRange;

			Aggressivity = _object.Aggressivity;
			Anxiety = _object.Anxiety;
			Experience = _object.Experience;
			Nosiness = _object.Nosiness;

		}

		public void Copy( InfluenceDataObject _object )
		{
			if( _object == null )
				return;

			Enabled = _object.Enabled;
			Foldout = _object.Foldout;

			UseDamageInPercent = _object.UseDamageInPercent;
			UseStressInPercent = _object.UseStressInPercent;
			UseDebilityInPercent = _object.UseDebilityInPercent;
			UseHungerInPercent = _object.UseHungerInPercent;
			UseThirstInPercent = _object.UseThirstInPercent;

			Damage = _object.Damage;
			DamageMin = _object.DamageMin;
			DamageMax = _object.DamageMax;
			UseDamageRange = _object.UseDamageRange;

			Stress = _object.Stress;
			StressMin = _object.StressMin;
			StressMax = _object.StressMax;
			UseStressRange = _object.UseStressRange;

			Debility = _object.Debility;
			DebilityMin = _object.DebilityMin;
			DebilityMax = _object.DebilityMax;
			UseDebilityRange = _object.UseDebilityRange;

			Hunger = _object.Hunger;
			HungerMin = _object.HungerMin;
			HungerMax = _object.HungerMax;
			UseHungerRange = _object.UseHungerRange;

			Thirst = _object.Thirst;
			ThirstMin = _object.ThirstMin;
			ThirstMax = _object.ThirstMax;
			UseThirstRange = _object.UseThirstRange;

			Aggressivity = _object.Aggressivity;
			Anxiety = _object.Anxiety;
			Experience = _object.Experience;
			Nosiness = _object.Nosiness;
		}

		//public float Damage;

		public float Damage;
		public float DamageMin;
		public float DamageMax;
		public bool UseDamageRange;
		public bool UseDamageInPercent;

		public float Stress;
		public float StressMin;
		public float StressMax;
		public bool UseStressRange;
		public bool UseStressInPercent;

		public float Debility;
		public float DebilityMin;
		public float DebilityMax;
		public bool UseDebilityRange;
		public bool UseDebilityInPercent;

		public float Hunger;
		public float HungerMin;
		public float HungerMax;
		public bool UseHungerRange;
		public bool UseHungerInPercent;

		public float Thirst;
		public float ThirstMin;
		public float ThirstMax;
		public bool UseThirstRange;
		public bool UseThirstInPercent;

		public float Aggressivity;
		public float Anxiety;
		public float Experience;
		public float Nosiness;

		public void Stop( GameObject _owner ){

			if( _owner != null )
				Stop( _owner.GetComponent<ICECreatureControl>() );
			else
				base.Stop();
		}

		public void Stop( ICEWorldBehaviour _component ){

			ICECreatureControl _control = _component as ICECreatureControl;

			if( base.Stop() && _control != null )
				_control.Creature.UpdateStatusInfluences( this );
		}

		public float GetDamage(){
			return UseDamageRange ? UnityEngine.Random.Range( DamageMin, DamageMax ) : Damage; 
		}

		public float GetStress(){
			return UseStressRange ? UnityEngine.Random.Range( StressMin, StressMax ) : Stress; 
		}

		public float GetDebility(){
			return UseDebilityRange ? UnityEngine.Random.Range( DebilityMin, DebilityMax ) : Debility; 
		}

		public float GetHunger(){
			return UseHungerRange ? UnityEngine.Random.Range( HungerMin, HungerMax ) : Hunger; 
		}

		public float GetThirst(){
			return UseThirstRange ? UnityEngine.Random.Range( ThirstMin, ThirstMax ) : Thirst; 
		}
	}

}
