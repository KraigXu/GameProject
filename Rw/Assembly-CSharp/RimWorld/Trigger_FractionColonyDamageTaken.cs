using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B6 RID: 1974
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x0011BE0B File Offset: 0x0011A00B
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x0011BE18 File Offset: 0x0011A018
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.40282347E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x0011BE39 File Offset: 0x0011A039
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x0011BE64 File Offset: 0x0011A064
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

		// Token: 0x04001B8C RID: 7052
		private float desiredColonyDamageFraction;

		// Token: 0x04001B8D RID: 7053
		private float minDamage;
	}
}
