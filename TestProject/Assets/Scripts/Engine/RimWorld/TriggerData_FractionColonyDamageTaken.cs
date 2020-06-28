using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B7 RID: 1975
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x06003334 RID: 13108 RVA: 0x0011BED6 File Offset: 0x0011A0D6
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x04001B8E RID: 7054
		public float startColonyDamage;
	}
}
