using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CF RID: 2511
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x06003BF7 RID: 15351 RVA: 0x0013C1B4 File Offset: 0x0013A3B4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), -1, out pawnKindDef);
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0013C1E0 File Offset: 0x0013A3E0
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind;
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), parms.target.Tile, out animalKind) && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.AdjustedPoints(parms.points), -1, out animalKind))
			{
				Log.Error("Could not find any valid animal kind for " + this.def + " incident.", false);
				return new List<Pawn>();
			}
			return ManhunterPackIncidentUtility.GenerateAnimals_NewTmp(animalKind, parms.target.Tile, this.AdjustedPoints(parms.points), 0);
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0013C264 File Offset: 0x0013A464
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
			}
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0013C2C5 File Offset: 0x0013A4C5
		private float AdjustedPoints(float basePoints)
		{
			return basePoints * 0.75f;
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0013C2D0 File Offset: 0x0013A4D0
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}

		// Token: 0x04002365 RID: 9061
		private const float ManhunterAmbushPointsFactor = 0.75f;
	}
}
