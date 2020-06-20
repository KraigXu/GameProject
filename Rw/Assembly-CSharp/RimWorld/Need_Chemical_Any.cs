using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8B RID: 2955
	public class Need_Chemical_Any : Need
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x00176AD9 File Offset: 0x00174CD9
		private Trait TraitDrugDesire
		{
			get
			{
				return this.pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06004551 RID: 17745 RVA: 0x00176AF5 File Offset: 0x00174CF5
		private SimpleCurve FallCurve
		{
			get
			{
				if (this.TraitDrugDesire.Degree == 2)
				{
					return Need_Chemical_Any.FascinationDegreeFallCurve;
				}
				return Need_Chemical_Any.InterestDegreeFallCurve;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x00176B10 File Offset: 0x00174D10
		private float FallPerNeedIntervalTick
		{
			get
			{
				Trait traitDrugDesire = this.TraitDrugDesire;
				float num = 1f;
				if (traitDrugDesire.Degree == 2)
				{
					num = 1.25f;
				}
				num *= this.FallCurve.Evaluate(this.CurLevel);
				return this.def.fallPerDay * num / 60000f * 150f;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06004553 RID: 17747 RVA: 0x00176B64 File Offset: 0x00174D64
		private Need_Chemical_Any.LevelThresholds CurrentLevelThresholds
		{
			get
			{
				if (this.TraitDrugDesire.Degree == 2)
				{
					return Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood;
				}
				return Need_Chemical_Any.InterestDegreeLevelThresholdsForMood;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x00176B80 File Offset: 0x00174D80
		public Need_Chemical_Any.MoodBuff MoodBuffForCurrentLevel
		{
			get
			{
				if (this.Disabled)
				{
					return Need_Chemical_Any.MoodBuff.Neutral;
				}
				Need_Chemical_Any.LevelThresholds currentLevelThresholds = this.CurrentLevelThresholds;
				float curLevel = this.CurLevel;
				if (curLevel <= currentLevelThresholds.extremelyNegative)
				{
					return Need_Chemical_Any.MoodBuff.ExtremelyNegative;
				}
				if (curLevel <= currentLevelThresholds.veryNegative)
				{
					return Need_Chemical_Any.MoodBuff.VeryNegative;
				}
				if (curLevel <= currentLevelThresholds.negative)
				{
					return Need_Chemical_Any.MoodBuff.Negative;
				}
				if (curLevel <= currentLevelThresholds.positive)
				{
					return Need_Chemical_Any.MoodBuff.Neutral;
				}
				if (curLevel <= currentLevelThresholds.veryPositive)
				{
					return Need_Chemical_Any.MoodBuff.Positive;
				}
				return Need_Chemical_Any.MoodBuff.VeryPositive;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06004555 RID: 17749 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06004556 RID: 17750 RVA: 0x00176BDD File Offset: 0x00174DDD
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004557 RID: 17751 RVA: 0x00176BE8 File Offset: 0x00174DE8
		private bool Disabled
		{
			get
			{
				return this.TraitDrugDesire == null || this.TraitDrugDesire.Degree < 1;
			}
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x00176C04 File Offset: 0x00174E04
		public void Notify_IngestedDrug(Thing drug)
		{
			if (this.Disabled)
			{
				return;
			}
			DrugCategory drugCategory = drug.def.ingestible.drugCategory;
			if (drugCategory == DrugCategory.Social)
			{
				this.CurLevel += 0.2f;
				return;
			}
			if (drugCategory != DrugCategory.Hard)
			{
				return;
			}
			this.CurLevel += 0.3f;
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x00176C59 File Offset: 0x00174E59
		public Need_Chemical_Any(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x00176C62 File Offset: 0x00174E62
		public override void SetInitialLevel()
		{
			this.CurLevel = 0.5f;
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x00176C70 File Offset: 0x00174E70
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			Trait traitDrugDesire = this.TraitDrugDesire;
			if (traitDrugDesire != null && this.lastThresholdUpdateTraitRef != traitDrugDesire)
			{
				this.lastThresholdUpdateTraitRef = traitDrugDesire;
				this.threshPercents = new List<float>();
				Need_Chemical_Any.LevelThresholds currentLevelThresholds = this.CurrentLevelThresholds;
				this.threshPercents.Add(currentLevelThresholds.extremelyNegative);
				this.threshPercents.Add(currentLevelThresholds.veryNegative);
				this.threshPercents.Add(currentLevelThresholds.negative);
				this.threshPercents.Add(currentLevelThresholds.positive);
				this.threshPercents.Add(currentLevelThresholds.veryPositive);
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x00176D0B File Offset: 0x00174F0B
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			this.CurLevel -= this.FallPerNeedIntervalTick;
		}

		// Token: 0x040027A8 RID: 10152
		public const int InterestTraitDegree = 1;

		// Token: 0x040027A9 RID: 10153
		public const int FascinationTraitDegree = 2;

		// Token: 0x040027AA RID: 10154
		private const float FallPerTickFactorForChemicalFascination = 1.25f;

		// Token: 0x040027AB RID: 10155
		public const float GainForHardDrugIngestion = 0.3f;

		// Token: 0x040027AC RID: 10156
		public const float GainForSocialDrugIngestion = 0.2f;

		// Token: 0x040027AD RID: 10157
		private static readonly SimpleCurve InterestDegreeFallCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative, 0.6f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative + 0.001f, 1f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.positive, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x040027AE RID: 10158
		private static readonly SimpleCurve FascinationDegreeFallCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.4f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative, 0.7f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative + 0.001f, 1f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.positive, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.15f),
				true
			}
		};

		// Token: 0x040027AF RID: 10159
		private static readonly Need_Chemical_Any.LevelThresholds FascinationDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.1f,
			veryNegative = 0.25f,
			negative = 0.4f,
			positive = 0.7f,
			veryPositive = 0.85f
		};

		// Token: 0x040027B0 RID: 10160
		private static readonly Need_Chemical_Any.LevelThresholds InterestDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.01f,
			veryNegative = 0.15f,
			negative = 0.3f,
			positive = 0.6f,
			veryPositive = 0.75f
		};

		// Token: 0x040027B1 RID: 10161
		private Trait lastThresholdUpdateTraitRef;

		// Token: 0x02001B09 RID: 6921
		public enum MoodBuff
		{
			// Token: 0x04006690 RID: 26256
			ExtremelyNegative,
			// Token: 0x04006691 RID: 26257
			VeryNegative,
			// Token: 0x04006692 RID: 26258
			Negative,
			// Token: 0x04006693 RID: 26259
			Neutral,
			// Token: 0x04006694 RID: 26260
			Positive,
			// Token: 0x04006695 RID: 26261
			VeryPositive
		}

		// Token: 0x02001B0A RID: 6922
		public struct LevelThresholds
		{
			// Token: 0x04006696 RID: 26262
			public float extremelyNegative;

			// Token: 0x04006697 RID: 26263
			public float veryNegative;

			// Token: 0x04006698 RID: 26264
			public float negative;

			// Token: 0x04006699 RID: 26265
			public float positive;

			// Token: 0x0400669A RID: 26266
			public float veryPositive;
		}
	}
}
