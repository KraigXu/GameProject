using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public static class PawnCapacityUtility
	{
		
		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		
		public static float CalculateCapacityLevel(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null, bool forTradePrice = false)
		{
			if (capacity.zeroIfCannotBeAwake && !diffSet.pawn.health.capacities.CanBeAwake)
			{
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
					{
						capacity = PawnCapacityDefOf.Consciousness
					});
				}
				return 0f;
			}
			float num = capacity.Worker.CalculateCapacityLevel(diffSet, impactors);
			if (num > 0f)
			{
				float num2 = 99999f;
				float num3 = 1f;
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff hediff = diffSet.hediffs[i];
					if (!forTradePrice || hediff.def.priceImpact)
					{
						List<PawnCapacityModifier> capMods = hediff.CapMods;
						if (capMods != null)
						{
							for (int j = 0; j < capMods.Count; j++)
							{
								PawnCapacityModifier pawnCapacityModifier = capMods[j];
								if (pawnCapacityModifier.capacity == capacity)
								{
									num += pawnCapacityModifier.offset;
									float num4 = pawnCapacityModifier.postFactor;
									if (hediff.CurStage != null && hediff.CurStage.capacityFactorEffectMultiplier != null)
									{
										num4 = StatWorker.ScaleFactor(num4, hediff.pawn.GetStatValue(hediff.CurStage.capacityFactorEffectMultiplier, true));
									}
									num3 *= num4;
									float num5 = pawnCapacityModifier.EvaluateSetMax(diffSet.pawn);
									if (num5 < num2)
									{
										num2 = num5;
									}
									if (impactors != null)
									{
										impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
										{
											hediff = hediff
										});
									}
								}
							}
						}
					}
				}
				num *= num3;
				num = Mathf.Min(num, num2);
			}
			num = Mathf.Max(num, capacity.minValue);
			return GenMath.RoundedHundredth(num);
		}

		
		public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartRecord rec;
			
			for (rec = part.parent; rec != null; rec = rec.parent)
			{
				if (diffSet.HasDirectlyAddedPartFor(rec))
				{
					IEnumerable<Hediff_AddedPart> hediffs = diffSet.GetHediffs<Hediff_AddedPart>();
					Func<Hediff_AddedPart, bool> predicate;
					if ((predicate ) == null)
					{
						predicate = (9__0 = ((Hediff_AddedPart x) => x.Part == rec));
					}
					Hediff_AddedPart hediff_AddedPart = hediffs.Where(predicate).First<Hediff_AddedPart>();
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = hediff_AddedPart
						});
					}
					return hediff_AddedPart.def.addedPartProps.partEfficiency;
				}
			}
			if (part.parent != null && diffSet.PartIsMissing(part.parent))
			{
				return 0f;
			}
			float num = 1f;
			if (!ignoreAddedParts)
			{
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff_AddedPart hediff_AddedPart2 = diffSet.hediffs[i] as Hediff_AddedPart;
					if (hediff_AddedPart2 != null && hediff_AddedPart2.Part == part)
					{
						num *= hediff_AddedPart2.def.addedPartProps.partEfficiency;
						if (hediff_AddedPart2.def.addedPartProps.partEfficiency != 1f && impactors != null)
						{
							impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
							{
								hediff = hediff_AddedPart2
							});
						}
					}
				}
			}
			float b = -1f;
			float num2 = 0f;
			bool flag = false;
			for (int j = 0; j < diffSet.hediffs.Count; j++)
			{
				if (diffSet.hediffs[j].Part == part && diffSet.hediffs[j].CurStage != null)
				{
					HediffStage curStage = diffSet.hediffs[j].CurStage;
					num2 += curStage.partEfficiencyOffset;
					flag |= curStage.partIgnoreMissingHP;
					if (curStage.partEfficiencyOffset != 0f && curStage.becomeVisible && impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = diffSet.hediffs[j]
						});
					}
				}
			}
			if (!flag)
			{
				float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
				if (num3 != 1f)
				{
					if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(part))
					{
						num3 = Mathf.InverseLerp(0.1f, 1f, num3);
					}
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorBodyPartHealth
						{
							bodyPart = part
						});
					}
					num *= num3;
				}
			}
			num += num2;
			if (num > 0.0001f)
			{
				num = Mathf.Max(num, b);
			}
			return Mathf.Max(num, 0f);
		}

		
		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				return 1f;
			}
			return PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
		}

		
		public static float CalculateNaturalPartsAverageEfficiency(HediffSet diffSet, BodyPartGroupDef bodyPartGroup)
		{
			float num = 0f;
			int num2 = 0;
			foreach (BodyPartRecord part in from x in diffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.groups.Contains(bodyPartGroup)
			select x)
			{
				if (!diffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
				{
					num += PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, null);
				}
				num2++;
			}
			if (num2 == 0 || num < 0f)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		
		public static float CalculateTagEfficiency(HediffSet diffSet, BodyPartTagDef tag, float maximum = 3.40282347E+38f, FloatRange lerp = default(FloatRange), List<PawnCapacityUtility.CapacityImpactor> impactors = null, float bestPartEfficiencySpecialWeight = -1f)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			float num3 = 0f;
			List<PawnCapacityUtility.CapacityImpactor> list = null;
			foreach (BodyPartRecord part in body.GetPartsWithTag(tag))
			{
				float num4 = PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				if (impactors != null && num4 != 1f && list == null)
				{
					list = new List<PawnCapacityUtility.CapacityImpactor>();
					PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				}
				num += num4;
				num3 = Mathf.Max(num3, num4);
				num2++;
			}
			if (num2 == 0)
			{
				return 1f;
			}
			float num5;
			if (bestPartEfficiencySpecialWeight >= 0f && num2 >= 2)
			{
				num5 = num3 * bestPartEfficiencySpecialWeight + (num - num3) / (float)(num2 - 1) * (1f - bestPartEfficiencySpecialWeight);
			}
			else
			{
				num5 = num / (float)num2;
			}
			float num6 = num5;
			if (lerp != default(FloatRange))
			{
				num6 = lerp.LerpThroughRange(num6);
			}
			num6 = Mathf.Min(num6, maximum);
			if (impactors != null && list != null && (maximum != 1f || num5 <= 1f || num6 == 1f))
			{
				impactors.AddRange(list);
			}
			return num6;
		}

		
		public static float CalculateLimbEfficiency(HediffSet diffSet, BodyPartTagDef limbCoreTag, BodyPartTagDef limbSegmentTag, BodyPartTagDef limbDigitTag, float appendageWeight, out float functionalPercentage, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			
			foreach (BodyPartRecord bodyPartRecord in body.GetPartsWithTag(limbCoreTag))
			{
				float num4 = PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, bodyPartRecord, impactors);
				foreach (BodyPartRecord part in bodyPartRecord.GetConnectedParts(limbSegmentTag))
				{
					num4 *= PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, part, impactors);
				}
				if (bodyPartRecord.HasChildParts(limbDigitTag))
				{
					float a = num4;
					float num5 = num4;
					IEnumerable<BodyPartRecord> childParts = bodyPartRecord.GetChildParts(limbDigitTag);
					Func<BodyPartRecord, float> selector;
					if ((selector ) == null)
					{
						selector = (9__0 = ((BodyPartRecord digitPart) => PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors)));
					}
					num4 = Mathf.Lerp(a, num5 * childParts.Average(selector), appendageWeight);
				}
				num += num4;
				num2++;
				if (num4 > 0f)
				{
					num3++;
				}
			}
			if (num2 == 0)
			{
				functionalPercentage = 0f;
				return 0f;
			}
			functionalPercentage = (float)num3 / (float)num2;
			return num / (float)num2;
		}

		
		public abstract class CapacityImpactor
		{
			
			// (get) Token: 0x06007A18 RID: 31256 RVA: 0x0001028D File Offset: 0x0000E48D
			public virtual bool IsDirect
			{
				get
				{
					return true;
				}
			}

			
			public abstract string Readable(Pawn pawn);
		}

		
		public class CapacityImpactorBodyPartHealth : PawnCapacityUtility.CapacityImpactor
		{
			
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}

			
			public BodyPartRecord bodyPart;
		}

		
		public class CapacityImpactorCapacity : PawnCapacityUtility.CapacityImpactor
		{
			
			// (get) Token: 0x06007A1D RID: 31261 RVA: 0x00010306 File Offset: 0x0000E506
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.GetLabelFor(pawn).CapitalizeFirst(), (pawn.health.capacities.GetLevel(this.capacity) * 100f).ToString("F0"));
			}

			
			public PawnCapacityDef capacity;
		}

		
		public class CapacityImpactorHediff : PawnCapacityUtility.CapacityImpactor
		{
			
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}

			
			public Hediff hediff;
		}

		
		public class CapacityImpactorPain : PawnCapacityUtility.CapacityImpactor
		{
			
			// (get) Token: 0x06007A22 RID: 31266 RVA: 0x00010306 File Offset: 0x0000E506
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), (pawn.health.hediffSet.PainTotal * 100f).ToString("F0"));
			}
		}
	}
}
