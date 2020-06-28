using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFC RID: 2812
	public class Recipe_Surgery : RecipeWorker
	{
		// Token: 0x0600425D RID: 16989 RVA: 0x00162684 File Offset: 0x00160884
		protected bool CheckSurgeryFail(Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			if (bill.recipe.surgerySuccessChanceFactor >= 99999f)
			{
				return false;
			}
			float num = 1f;
			if (!patient.RaceProps.IsMechanoid)
			{
				num *= surgeon.GetStatValue(StatDefOf.MedicalSurgerySuccessChance, true);
			}
			if (patient.InBed())
			{
				num *= patient.CurrentBed().GetStatValue(StatDefOf.SurgerySuccessChanceFactor, true);
			}
			num *= Recipe_Surgery.MedicineMedicalPotencyToSurgeryChanceFactor.Evaluate(this.GetAverageMedicalPotency(ingredients, bill));
			num *= this.recipe.surgerySuccessChanceFactor;
			if (surgeon.InspirationDef == InspirationDefOf.Inspired_Surgery && !patient.RaceProps.IsMechanoid)
			{
				num *= 2f;
				surgeon.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Surgery);
			}
			num = Mathf.Min(num, 0.98f);
			if (!Rand.Chance(num))
			{
				if (Rand.Chance(this.recipe.deathOnFailedSurgeryChance))
				{
					HealthUtility.GiveInjuriesOperationFailureCatastrophic(patient, part);
					if (!patient.Dead)
					{
						patient.Kill(null, null);
					}
					Messages.Message("MessageMedicalOperationFailureFatal".Translate(surgeon.LabelShort, patient.LabelShort, this.recipe.LabelCap, surgeon.Named("SURGEON"), patient.Named("PATIENT")), patient, MessageTypeDefOf.NegativeHealthEvent, true);
				}
				else if (Rand.Chance(0.5f))
				{
					if (Rand.Chance(0.1f))
					{
						Messages.Message("MessageMedicalOperationFailureRidiculous".Translate(surgeon.LabelShort, patient.LabelShort, surgeon.Named("SURGEON"), patient.Named("PATIENT"), this.recipe.Named("RECIPE")), patient, MessageTypeDefOf.NegativeHealthEvent, true);
						HealthUtility.GiveInjuriesOperationFailureRidiculous(patient);
					}
					else
					{
						Messages.Message("MessageMedicalOperationFailureCatastrophic".Translate(surgeon.LabelShort, patient.LabelShort, surgeon.Named("SURGEON"), patient.Named("PATIENT"), this.recipe.Named("RECIPE")), patient, MessageTypeDefOf.NegativeHealthEvent, true);
						HealthUtility.GiveInjuriesOperationFailureCatastrophic(patient, part);
					}
				}
				else
				{
					Messages.Message("MessageMedicalOperationFailureMinor".Translate(surgeon.LabelShort, patient.LabelShort, surgeon.Named("SURGEON"), patient.Named("PATIENT"), this.recipe.Named("RECIPE")), patient, MessageTypeDefOf.NegativeHealthEvent, true);
					HealthUtility.GiveInjuriesOperationFailureMinor(patient, part);
				}
				if (!patient.Dead)
				{
					this.TryGainBotchedSurgeryThought(patient, surgeon);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x0016294B File Offset: 0x00160B4B
		private void TryGainBotchedSurgeryThought(Pawn patient, Pawn surgeon)
		{
			if (!patient.RaceProps.Humanlike || patient.needs.mood == null)
			{
				return;
			}
			patient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BotchedMySurgery, surgeon);
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x00162988 File Offset: 0x00160B88
		private float GetAverageMedicalPotency(List<Thing> ingredients, Bill bill)
		{
			Bill_Medical bill_Medical = bill as Bill_Medical;
			ThingDef thingDef;
			if (bill_Medical != null)
			{
				thingDef = bill_Medical.consumedInitialMedicineDef;
			}
			else
			{
				thingDef = null;
			}
			int num = 0;
			float num2 = 0f;
			if (thingDef != null)
			{
				num++;
				num2 += thingDef.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			}
			for (int i = 0; i < ingredients.Count; i++)
			{
				Medicine medicine = ingredients[i] as Medicine;
				if (medicine != null)
				{
					num += medicine.stackCount;
					num2 += medicine.GetStatValue(StatDefOf.MedicalPotency, true) * (float)medicine.stackCount;
				}
			}
			if (num == 0)
			{
				return 1f;
			}
			return num2 / (float)num;
		}

		// Token: 0x04002637 RID: 9783
		private const float MaxSuccessChance = 0.98f;

		// Token: 0x04002638 RID: 9784
		private const float CatastrophicFailChance = 0.5f;

		// Token: 0x04002639 RID: 9785
		private const float RidiculousFailChanceFromCatastrophic = 0.1f;

		// Token: 0x0400263A RID: 9786
		private const float InspiredSurgerySuccessChanceFactor = 2f;

		// Token: 0x0400263B RID: 9787
		private static readonly SimpleCurve MedicineMedicalPotencyToSurgeryChanceFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1.3f),
				true
			}
		};
	}
}
