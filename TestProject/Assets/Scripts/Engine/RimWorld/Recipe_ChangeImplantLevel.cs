using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF4 RID: 2804
	public class Recipe_ChangeImplantLevel : Recipe_Surgery
	{
		// Token: 0x06004240 RID: 16960 RVA: 0x00161D64 File Offset: 0x0015FF64
		private bool Operable(Hediff target, RecipeDef recipe)
		{
			int hediffLevelOffset = recipe.hediffLevelOffset;
			if (hediffLevelOffset == 0)
			{
				return false;
			}
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = target as Hediff_ImplantWithLevel;
			if (hediff_ImplantWithLevel == null)
			{
				return false;
			}
			int level = hediff_ImplantWithLevel.level;
			if (hediff_ImplantWithLevel.def != recipe.changesHediffLevel)
			{
				return false;
			}
			if (hediffLevelOffset <= 0)
			{
				return level > 0;
			}
			return (float)level < hediff_ImplantWithLevel.def.maxSeverity;
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x00161DB8 File Offset: 0x0015FFB8
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && this.Operable(x, recipe)));
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x00161E00 File Offset: 0x00160000
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = (Hediff_ImplantWithLevel)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => this.Operable(h, this.recipe) && h.Part == part);
			if (hediff_ImplantWithLevel != null)
			{
				if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
				{
					base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraHomeFaction, -70, "GoodwillChangedReason_DowngradedImplant".Translate(hediff_ImplantWithLevel.Label));
				}
				hediff_ImplantWithLevel.ChangeLevel(this.recipe.hediffLevelOffset);
			}
		}
	}
}
