using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025F RID: 607
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x0005E5BF File Offset: 0x0005C7BF
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0005E5CC File Offset: 0x0005C7CC
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0005E5DA File Offset: 0x0005C7DA
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0005E5F1 File Offset: 0x0005C7F1
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0005E618 File Offset: 0x0005C818
		private void TryHealRandomPermanentWound()
		{
			Hediff hediff;
			if (!(from hd in base.Pawn.health.hediffSet.hediffs
			where hd.IsPermanent() || hd.def.chronic
			select hd).TryRandomElement(out hediff))
			{
				return;
			}
			HealthUtility.CureHediff(hediff);
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				Messages.Message("MessagePermanentWoundHealed".Translate(this.parent.LabelCap, base.Pawn.LabelShort, hediff.Label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0005E6DB File Offset: 0x0005C8DB
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0005E6EF File Offset: 0x0005C8EF
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x04000C12 RID: 3090
		private int ticksToHeal;
	}
}
