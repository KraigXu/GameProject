using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028C RID: 652
	public class ImmunityRecord : IExposable
	{
		// Token: 0x06001178 RID: 4472 RVA: 0x00062AC4 File Offset: 0x00060CC4
		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00062AFC File Offset: 0x00060CFC
		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00062B28 File Offset: 0x00060D28
		public float ImmunityChangePerTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.hediffDef.CompProps<HediffCompProperties_Immunizable>();
			if (sick)
			{
				float num = hediffCompProperties_Immunizable.immunityPerDaySick;
				num *= pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true);
				if (diseaseInstance != null)
				{
					Rand.PushState();
					Rand.Seed = Gen.HashCombineInt(diseaseInstance.loadID ^ Find.World.info.persistentRandomValue, 156482735);
					num *= Mathf.Lerp(0.8f, 1.2f, Rand.Value);
					Rand.PopState();
				}
				return num / 60000f;
			}
			return hediffCompProperties_Immunizable.immunityPerDayNotSick / 60000f;
		}

		// Token: 0x04000C73 RID: 3187
		public HediffDef hediffDef;

		// Token: 0x04000C74 RID: 3188
		public HediffDef source;

		// Token: 0x04000C75 RID: 3189
		public float immunity;
	}
}
