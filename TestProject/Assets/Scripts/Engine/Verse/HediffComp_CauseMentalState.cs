using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200023F RID: 575
	public class HediffComp_CauseMentalState : HediffComp
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0005CD6C File Offset: 0x0005AF6C
		public HediffCompProperties_CauseMentalState Props
		{
			get
			{
				return (HediffCompProperties_CauseMentalState)this.props;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0005CD7C File Offset: 0x0005AF7C
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(60))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					if (base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.humanMentalState && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.humanMentalState, this.parent.def.LabelCap, false, false, null, true) && base.Pawn.Spawned)
					{
						this.SendLetter(this.Props.humanMentalState);
						return;
					}
				}
				else if (base.Pawn.RaceProps.Animal && base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalState && (this.Props.animalMentalStateAlias == null || base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalStateAlias) && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.animalMentalState, this.parent.def.LabelCap, false, false, null, true) && base.Pawn.Spawned)
				{
					this.SendLetter(this.Props.animalMentalState);
				}
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005CF54 File Offset: 0x0005B154
		private void SendLetter(MentalStateDef mentalStateDef)
		{
			Find.LetterStack.ReceiveLetter((mentalStateDef.beginLetterLabel ?? mentalStateDef.LabelCap).CapitalizeFirst() + ": " + base.Pawn.LabelShortCap, base.Pawn.mindState.mentalStateHandler.CurState.GetBeginLetterText() + "\n\n" + "CausedByHediff".Translate(this.parent.LabelCap), this.Props.letterDef, base.Pawn, null, null, null, null);
		}
	}
}
