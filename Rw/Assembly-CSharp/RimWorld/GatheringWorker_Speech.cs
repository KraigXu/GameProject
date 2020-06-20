using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C8 RID: 2248
	public class GatheringWorker_Speech : GatheringWorker
	{
		// Token: 0x06003620 RID: 13856 RVA: 0x00125AD6 File Offset: 0x00123CD6
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Speech(spot, organizer, this.def);
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x00125AE8 File Offset: 0x00123CE8
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec);
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x00125B08 File Offset: 0x00123D08
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(organizer);
			if (building_Throne != null)
			{
				spot = building_Throne.InteractionCell;
				return true;
			}
			spot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x00125B3C File Offset: 0x00123D3C
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")) + "\n\n" + GatheringWorker_Speech.OutcomeBreakdownForPawn(organizer), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x00125BB0 File Offset: 0x00123DB0
		public static string OutcomeBreakdownForPawn(Pawn organizer)
		{
			return "AbilitySpeechStatInfo".Translate(organizer.Named("ORGANIZER"), StatDefOf.SocialImpact.label) + ": " + organizer.GetStatValue(StatDefOf.SocialImpact, true).ToStringPercent() + "\n\n" + "AbilitySpeechPossibleOutcomes".Translate() + ":\n" + (from o in LordJob_Joinable_Speech.OutcomeChancesForPawn(organizer).Reverse<Tuple<ThoughtDef, float>>()
			select o.Item1.stages[0].LabelCap + " " + o.Item2.ToStringPercent()).ToLineList("  - ", false);
		}
	}
}
