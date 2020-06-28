using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009EE RID: 2542
	public class IncidentWorker_RansomDemand : IncidentWorker
	{
		// Token: 0x06003C7C RID: 15484 RVA: 0x0013F924 File Offset: 0x0013DB24
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return CommsConsoleUtility.PlayerHasPoweredCommsConsole((Map)parms.target) && this.RandomKidnappedColonist() != null && base.CanFireNowSub(parms);
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0013F94C File Offset: 0x0013DB4C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = this.RandomKidnappedColonist();
			if (pawn == null)
			{
				return false;
			}
			Faction faction = this.FactionWhichKidnapped(pawn);
			int num = this.RandomFee(pawn);
			ChoiceLetter_RansomDemand choiceLetter_RansomDemand = (ChoiceLetter_RansomDemand)LetterMaker.MakeLetter(this.def.letterLabel, "RansomDemand".Translate(pawn.LabelShort, faction.NameColored, num, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), this.def.letterDef, null, null);
			choiceLetter_RansomDemand.title = "RansomDemandTitle".Translate(map.Parent.Label);
			choiceLetter_RansomDemand.radioMode = true;
			choiceLetter_RansomDemand.kidnapped = pawn;
			choiceLetter_RansomDemand.faction = faction;
			choiceLetter_RansomDemand.map = map;
			choiceLetter_RansomDemand.fee = num;
			choiceLetter_RansomDemand.relatedFaction = faction;
			choiceLetter_RansomDemand.quest = parms.quest;
			choiceLetter_RansomDemand.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_RansomDemand, null);
			return true;
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x0013FA68 File Offset: 0x0013DC68
		private Pawn RandomKidnappedColonist()
		{
			IncidentWorker_RansomDemand.candidates.Clear();
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				List<Pawn> kidnappedPawnsListForReading = allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading;
				for (int j = 0; j < kidnappedPawnsListForReading.Count; j++)
				{
					if (kidnappedPawnsListForReading[j].Faction == Faction.OfPlayer && kidnappedPawnsListForReading[j].RaceProps.Humanlike)
					{
						IncidentWorker_RansomDemand.candidates.Add(kidnappedPawnsListForReading[j]);
					}
				}
			}
			List<Letter> lettersListForReading = Find.LetterStack.LettersListForReading;
			for (int k = 0; k < lettersListForReading.Count; k++)
			{
				ChoiceLetter_RansomDemand choiceLetter_RansomDemand = lettersListForReading[k] as ChoiceLetter_RansomDemand;
				if (choiceLetter_RansomDemand != null)
				{
					IncidentWorker_RansomDemand.candidates.Remove(choiceLetter_RansomDemand.kidnapped);
				}
			}
			Pawn result;
			if (!IncidentWorker_RansomDemand.candidates.TryRandomElement(out result))
			{
				return null;
			}
			IncidentWorker_RansomDemand.candidates.Clear();
			return result;
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x0013FB64 File Offset: 0x0013DD64
		private Faction FactionWhichKidnapped(Pawn pawn)
		{
			return Find.FactionManager.AllFactionsListForReading.Find((Faction x) => x.kidnapped.KidnappedPawnsListForReading.Contains(pawn));
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0013FB9C File Offset: 0x0013DD9C
		private int RandomFee(Pawn pawn)
		{
			return (int)(pawn.MarketValue * DiplomacyTuning.RansomFeeMarketValueFactorRange.RandomInRange);
		}

		// Token: 0x04002390 RID: 9104
		private const int TimeoutTicks = 60000;

		// Token: 0x04002391 RID: 9105
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
