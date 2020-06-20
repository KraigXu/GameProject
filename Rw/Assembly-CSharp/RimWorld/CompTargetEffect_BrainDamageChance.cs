using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D92 RID: 3474
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06005494 RID: 21652 RVA: 0x001C359F File Offset: 0x001C179F
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x001C35AC File Offset: 0x001C17AC
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (Rand.Value <= this.PropsBrainDamageChance.brainDamageChance)
			{
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int num = Rand.RangeInclusive(1, 5);
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, user, brain, this.parent.def, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}
