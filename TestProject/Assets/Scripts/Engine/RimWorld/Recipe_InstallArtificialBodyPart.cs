using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF5 RID: 2805
	public class Recipe_InstallArtificialBodyPart : Recipe_Surgery
	{
		// Token: 0x06004244 RID: 16964 RVA: 0x00161EB8 File Offset: 0x001600B8
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, delegate(BodyPartRecord record)
			{
				IEnumerable<Hediff> source = from x in pawn.health.hediffSet.hediffs
				where x.Part == record
				select x;
				return (source.Count<Hediff>() != 1 || source.First<Hediff>().def != recipe.addsHediff) && (record.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(record.parent)) && (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record));
			});
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x00161EF8 File Offset: 0x001600F8
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = !PawnGenerator.IsBeingGenerated(pawn) && this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
				if (flag && flag2 && part.def.spawnThingOnRemoved != null)
				{
					ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn);
				}
				if (flag2)
				{
					base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraHomeFaction, -70, "GoodwillChangedReason_NeedlesslyInstalledWorseBodyPart".Translate(this.recipe.addsHediff.label));
				}
			}
			else if (pawn.Map != null)
			{
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
			}
			else
			{
				pawn.health.RestorePart(part, null, true);
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00162008 File Offset: 0x00160208
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return ((pawn.Faction != billDoerFaction && pawn.Faction != null) || pawn.IsQuestLodger()) && (this.recipe.addsHediff.addedPartProps == null || !this.recipe.addsHediff.addedPartProps.betterThanNatural) && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}
	}
}
