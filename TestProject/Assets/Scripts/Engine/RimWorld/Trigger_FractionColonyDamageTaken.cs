using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x0011BE0B File Offset: 0x0011A00B
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.40282347E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_FractionColonyDamageTaken))
				{
					BackCompatibility.TriggerDataFractionColonyDamageTakenNull(this, lord.Map);
				}
				float num = Mathf.Max((float)lord.initialColonyHealthTotal * this.desiredColonyDamageFraction, this.minDamage);
				return lord.Map.damageWatcher.DamageTakenEver > this.Data.startColonyDamage + num;
			}
			return false;
		}

		
		private float desiredColonyDamageFraction;

		
		private float minDamage;
	}
}
