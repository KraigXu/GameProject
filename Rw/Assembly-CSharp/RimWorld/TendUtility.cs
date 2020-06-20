﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000643 RID: 1603
	public static class TendUtility
	{
		// Token: 0x06002BDB RID: 11227 RVA: 0x000FBF34 File Offset: 0x000FA134
		public static void DoTend(Pawn doctor, Pawn patient, Medicine medicine)
		{
			if (!patient.health.HasHediffsNeedingTend(false))
			{
				return;
			}
			if (medicine != null && medicine.Destroyed)
			{
				Log.Warning("Tried to use destroyed medicine.", false);
				medicine = null;
			}
			float quality = TendUtility.CalculateBaseTendQuality(doctor, patient, (medicine != null) ? medicine.def : null);
			TendUtility.GetOptimalHediffsToTendWithSingleTreatment(patient, medicine != null, TendUtility.tmpHediffsToTend, null);
			for (int i = 0; i < TendUtility.tmpHediffsToTend.Count; i++)
			{
				TendUtility.tmpHediffsToTend[i].Tended(quality, i);
			}
			if (doctor != null && doctor.Faction == Faction.OfPlayer && patient.Faction != doctor.Faction && !patient.IsPrisoner && patient.Faction != null)
			{
				patient.mindState.timesGuestTendedToByPlayer++;
			}
			if (doctor != null && doctor.IsColonistPlayerControlled)
			{
				patient.records.AccumulateStoryEvent(StoryEventDefOf.TendedByPlayer);
			}
			if (doctor != null && doctor.RaceProps.Humanlike && patient.RaceProps.Animal && RelationsUtility.TryDevelopBondRelation(doctor, patient, 0.004f) && doctor.Faction != null && doctor.Faction != patient.Faction)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(doctor, patient, 1f, false);
			}
			patient.records.Increment(RecordDefOf.TimesTendedTo);
			if (doctor != null)
			{
				doctor.records.Increment(RecordDefOf.TimesTendedOther);
			}
			if (doctor == patient && !doctor.Dead)
			{
				doctor.mindState.Notify_SelfTended();
			}
			if (medicine != null)
			{
				if ((patient.Spawned || (doctor != null && doctor.Spawned)) && medicine != null && medicine.GetStatValue(StatDefOf.MedicalPotency, true) > ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null))
				{
					SoundDefOf.TechMedicineUsed.PlayOneShot(new TargetInfo(patient.Position, patient.Map, false));
				}
				if (medicine.stackCount > 1)
				{
					medicine.stackCount--;
					return;
				}
				if (!medicine.Destroyed)
				{
					medicine.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000FC118 File Offset: 0x000FA318
		public static float CalculateBaseTendQuality(Pawn doctor, Pawn patient, ThingDef medicine)
		{
			float medicinePotency = (medicine != null) ? medicine.GetStatValueAbstract(StatDefOf.MedicalPotency, null) : 0.3f;
			float medicineQualityMax = (medicine != null) ? medicine.GetStatValueAbstract(StatDefOf.MedicalQualityMax, null) : 0.7f;
			return TendUtility.CalculateBaseTendQuality(doctor, patient, medicinePotency, medicineQualityMax);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000FC15C File Offset: 0x000FA35C
		public static float CalculateBaseTendQuality(Pawn doctor, Pawn patient, float medicinePotency, float medicineQualityMax)
		{
			float num;
			if (doctor != null)
			{
				num = doctor.GetStatValue(StatDefOf.MedicalTendQuality, true);
			}
			else
			{
				num = 0.75f;
			}
			num *= medicinePotency;
			Building_Bed building_Bed = (patient != null) ? patient.CurrentBed() : null;
			if (building_Bed != null)
			{
				num += building_Bed.GetStatValue(StatDefOf.MedicalTendQualityOffset, true);
			}
			if (doctor == patient && doctor != null)
			{
				num *= 0.7f;
			}
			return Mathf.Clamp(num, 0f, medicineQualityMax);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000FC1C4 File Offset: 0x000FA3C4
		public static void GetOptimalHediffsToTendWithSingleTreatment(Pawn patient, bool usingMedicine, List<Hediff> outHediffsToTend, List<Hediff> tendableHediffsInTendPriorityOrder = null)
		{
			outHediffsToTend.Clear();
			TendUtility.tmpHediffs.Clear();
			if (tendableHediffsInTendPriorityOrder != null)
			{
				TendUtility.tmpHediffs.AddRange(tendableHediffsInTendPriorityOrder);
			}
			else
			{
				List<Hediff> hediffs = patient.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].TendableNow(false))
					{
						TendUtility.tmpHediffs.Add(hediffs[i]);
					}
				}
				TendUtility.SortByTendPriority(TendUtility.tmpHediffs);
			}
			if (!TendUtility.tmpHediffs.Any<Hediff>())
			{
				return;
			}
			Hediff hediff = TendUtility.tmpHediffs[0];
			outHediffsToTend.Add(hediff);
			HediffCompProperties_TendDuration hediffCompProperties_TendDuration = hediff.def.CompProps<HediffCompProperties_TendDuration>();
			if (hediffCompProperties_TendDuration != null && hediffCompProperties_TendDuration.tendAllAtOnce)
			{
				for (int j = 0; j < TendUtility.tmpHediffs.Count; j++)
				{
					if (TendUtility.tmpHediffs[j] != hediff && TendUtility.tmpHediffs[j].def == hediff.def)
					{
						outHediffsToTend.Add(TendUtility.tmpHediffs[j]);
					}
				}
			}
			else if (hediff is Hediff_Injury && usingMedicine)
			{
				float num = hediff.Severity;
				for (int k = 0; k < TendUtility.tmpHediffs.Count; k++)
				{
					if (TendUtility.tmpHediffs[k] != hediff)
					{
						Hediff_Injury hediff_Injury = TendUtility.tmpHediffs[k] as Hediff_Injury;
						if (hediff_Injury != null)
						{
							float severity = hediff_Injury.Severity;
							if (num + severity <= 20f)
							{
								num += severity;
								outHediffsToTend.Add(hediff_Injury);
							}
						}
					}
				}
			}
			TendUtility.tmpHediffs.Clear();
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000FC34C File Offset: 0x000FA54C
		public static void SortByTendPriority(List<Hediff> hediffs)
		{
			if (hediffs.Count <= 1)
			{
				return;
			}
			TendUtility.tmpHediffsWithTendPriority.Clear();
			for (int i = 0; i < hediffs.Count; i++)
			{
				TendUtility.tmpHediffsWithTendPriority.Add(new Pair<Hediff, float>(hediffs[i], hediffs[i].TendPriority));
			}
			TendUtility.tmpHediffsWithTendPriority.SortByDescending((Pair<Hediff, float> x) => x.Second, (Pair<Hediff, float> x) => x.First.Severity);
			hediffs.Clear();
			for (int j = 0; j < TendUtility.tmpHediffsWithTendPriority.Count; j++)
			{
				hediffs.Add(TendUtility.tmpHediffsWithTendPriority[j].First);
			}
			TendUtility.tmpHediffsWithTendPriority.Clear();
		}

		// Token: 0x040019B7 RID: 6583
		public const float NoMedicinePotency = 0.3f;

		// Token: 0x040019B8 RID: 6584
		public const float NoMedicineQualityMax = 0.7f;

		// Token: 0x040019B9 RID: 6585
		public const float NoDoctorTendQuality = 0.75f;

		// Token: 0x040019BA RID: 6586
		public const float SelfTendQualityFactor = 0.7f;

		// Token: 0x040019BB RID: 6587
		private const float ChanceToDevelopBondRelationOnTended = 0.004f;

		// Token: 0x040019BC RID: 6588
		private static List<Hediff> tmpHediffsToTend = new List<Hediff>();

		// Token: 0x040019BD RID: 6589
		private static List<Hediff> tmpHediffs = new List<Hediff>();

		// Token: 0x040019BE RID: 6590
		private static List<Pair<Hediff, float>> tmpHediffsWithTendPriority = new List<Pair<Hediff, float>>();
	}
}
