using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000248 RID: 584
	public class HediffComp_DamageBrain : HediffComp
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0005D384 File Offset: 0x0005B584
		public HediffCompProperties_DamageBrain Props
		{
			get
			{
				return (HediffCompProperties_DamageBrain)this.props;
			}
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0005D394 File Offset: 0x0005B594
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.mtbDaysPerStage[this.parent.CurStageIndex] > 0f && base.Pawn.IsHashIntervalTick(60) && Rand.MTBEventOccurs(this.Props.mtbDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
			{
				BodyPartRecord brain = base.Pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int randomInRange = this.Props.damageAmount.RandomInRange;
				base.Pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, (float)randomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				Messages.Message("MessageReceivedBrainDamageFromHediff".Translate(base.Pawn.Named("PAWN"), randomInRange, this.parent.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}
