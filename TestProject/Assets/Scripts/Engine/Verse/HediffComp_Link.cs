using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000266 RID: 614
	public class HediffComp_Link : HediffComp
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0005EBE9 File Offset: 0x0005CDE9
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0005EBF8 File Offset: 0x0005CDF8
		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				if (this.other == null || !this.parent.pawn.Spawned || !this.other.Spawned)
				{
					return true;
				}
				if (this.Props.maxDistance > 0f && !this.parent.pawn.Position.InHorDistOf(this.other.Position, this.Props.maxDistance))
				{
					return true;
				}
				foreach (Hediff hediff in this.other.health.hediffSet.hediffs)
				{
					HediffWithComps hediffWithComps = hediff as HediffWithComps;
					if (hediffWithComps != null && hediffWithComps.comps.FirstOrDefault(delegate(HediffComp c)
					{
						HediffComp_Link hediffComp_Link = c as HediffComp_Link;
						return hediffComp_Link != null && hediffComp_Link.other == this.parent.pawn && hediffComp_Link.parent.def == this.parent.def;
					}) != null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0005ECF4 File Offset: 0x0005CEF4
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.drawConnection)
			{
				if (this.mote == null)
				{
					this.mote = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkLine, this.parent.pawn, this.other);
				}
				this.mote.Maintain();
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0005ED4E File Offset: 0x0005CF4E
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Pawn>(ref this.other, "other", false);
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0005ED67 File Offset: 0x0005CF67
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showName || this.other == null)
				{
					return null;
				}
				return this.other.LabelShort;
			}
		}

		// Token: 0x04000C20 RID: 3104
		public Pawn other;

		// Token: 0x04000C21 RID: 3105
		private MoteDualAttached mote;

		// Token: 0x04000C22 RID: 3106
		public bool drawConnection;
	}
}
