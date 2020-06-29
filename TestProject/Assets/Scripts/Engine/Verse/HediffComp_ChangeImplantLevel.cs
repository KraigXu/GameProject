using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0005D13C File Offset: 0x0005B33C
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		
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

		
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		
		public int lastChangeLevelTick = -1;
	}
}
