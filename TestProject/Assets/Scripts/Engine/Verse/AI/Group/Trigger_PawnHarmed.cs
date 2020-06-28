using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005FE RID: 1534
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x06002A1D RID: 10781 RVA: 0x000F622B File Offset: 0x000F442B
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000F6254 File Offset: 0x000F4454
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000F62D4 File Offset: 0x000F44D4
		public static bool SignalIsHarm(TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnDamaged)
			{
				return signal.dinfo.Def.ExternalViolenceFor(signal.Pawn);
			}
			if (signal.type == TriggerSignalType.PawnLost)
			{
				return signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled;
			}
			return signal.type == TriggerSignalType.PawnArrestAttempted;
		}

		// Token: 0x04001929 RID: 6441
		public float chance = 1f;

		// Token: 0x0400192A RID: 6442
		public bool requireInstigatorWithFaction;

		// Token: 0x0400192B RID: 6443
		public Faction requireInstigatorWithSpecificFaction;
	}
}
