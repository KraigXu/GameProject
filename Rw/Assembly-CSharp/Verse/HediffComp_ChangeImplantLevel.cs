using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000244 RID: 580
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0005D13C File Offset: 0x0005B33C
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0005D14C File Offset: 0x0005B34C
		public override void CompPostTick(ref float severityAdjustment)
		{
			float mtbDays = this.Props.probabilityPerStage[this.parent.CurStageIndex].mtbDays;
			if (mtbDays > 0f && base.Pawn.IsHashIntervalTick(60))
			{
				ChangeImplantLevel_Probability changeImplantLevel_Probability = this.Props.probabilityPerStage[this.parent.CurStageIndex];
				if ((this.lastChangeLevelTick < 0 || (float)(Find.TickManager.TicksGame - this.lastChangeLevelTick) >= changeImplantLevel_Probability.minIntervalDays * 60000f) && Rand.MTBEventOccurs(mtbDays, 60000f, 60f))
				{
					Hediff_ImplantWithLevel hediff_ImplantWithLevel = this.parent.pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.implant, false) as Hediff_ImplantWithLevel;
					if (hediff_ImplantWithLevel != null)
					{
						hediff_ImplantWithLevel.ChangeLevel(this.Props.levelOffset);
						this.lastChangeLevelTick = Find.TickManager.TicksGame;
						Messages.Message("MessageLostImplantLevelFromHediff".Translate(this.parent.pawn.Named("PAWN"), hediff_ImplantWithLevel.LabelBase, this.parent.Label), this.parent.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0005D29E File Offset: 0x0005B49E
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		// Token: 0x04000BE1 RID: 3041
		public int lastChangeLevelTick = -1;
	}
}
