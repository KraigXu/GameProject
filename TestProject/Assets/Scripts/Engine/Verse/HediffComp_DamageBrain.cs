using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_DamageBrain : HediffComp
	{
		
		
		public HediffCompProperties_DamageBrain Props
		{
			get
			{
				return (HediffCompProperties_DamageBrain)this.props;
			}
		}

		
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
