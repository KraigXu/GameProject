using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Need_Chemical_Any : Need
	{
		
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x00176AD9 File Offset: 0x00174CD9
		private Trait TraitDrugDesire
		{
			get
			{
				return this.pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
			}
		}

		
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

		
		// (get) Token: 0x06004555 RID: 17749 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		
		// (get) Token: 0x06004556 RID: 17750 RVA: 0x00176BDD File Offset: 0x00174DDD
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		
		// (get) Token: 0x06004557 RID: 17751 RVA: 0x00176BE8 File Offset: 0x00174DE8
		private bool Disabled
		{
			get
			{
				return this.TraitDrugDesire == null || this.TraitDrugDesire.Degree < 1;
			}
		}

		
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

		
		public Need_Chemical_Any(Pawn pawn) : base(pawn)
		{
		}

		
		public override void SetInitialLevel()
		{
			this.CurLevel = 0.5f;
		}

		
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

		
		public const int InterestTraitDegree = 1;

		
		public const int FascinationTraitDegree = 2;

		
		private const float FallPerTickFactorForChemicalFascination = 1.25f;

		
		public const float GainForHardDrugIngestion = 0.3f;

		
		public const float GainForSocialDrugIngestion = 0.2f;

		
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

		
		private static readonly Need_Chemical_Any.LevelThresholds FascinationDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.1f,
			veryNegative = 0.25f,
			negative = 0.4f,
			positive = 0.7f,
			veryPositive = 0.85f
		};

		
		private static readonly Need_Chemical_Any.LevelThresholds InterestDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.01f,
			veryNegative = 0.15f,
			negative = 0.3f,
			positive = 0.6f,
			veryPositive = 0.75f
		};

		
		private Trait lastThresholdUpdateTraitRef;

		
		public enum MoodBuff
		{
			
			ExtremelyNegative,
			
			VeryNegative,
			
			Negative,
			
			Neutral,
			
			Positive,
			
			VeryPositive
		}

		
		public struct LevelThresholds
		{
			
			public float extremelyNegative;

			
			public float veryNegative;

			
			public float negative;

			
			public float positive;

			
			public float veryPositive;
		}
	}
}
