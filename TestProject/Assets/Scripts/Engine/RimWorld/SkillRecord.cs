using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class SkillRecord : IExposable
	{
		
		
		
		public int Level
		{
			get
			{
				if (this.TotallyDisabled)
				{
					return 0;
				}
				return this.levelInt;
			}
			set
			{
				this.levelInt = Mathf.Clamp(value, 0, 20);
			}
		}

		
		
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		
		
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		
		
		public float XpTotalEarned
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.levelInt; i++)
				{
					num += SkillRecord.XpRequiredToLevelUpFrom(i);
				}
				return num;
			}
		}

		
		
		public bool TotallyDisabled
		{
			get
			{
				if (this.cachedTotallyDisabled == BoolUnknown.Unknown)
				{
					this.cachedTotallyDisabled = (this.CalculateTotallyDisabled() ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.cachedTotallyDisabled == BoolUnknown.True;
			}
		}

		
		
		public string LevelDescriptor
		{
			get
			{
				switch (this.levelInt)
				{
				case 0:
					return "Skill0".Translate();
				case 1:
					return "Skill1".Translate();
				case 2:
					return "Skill2".Translate();
				case 3:
					return "Skill3".Translate();
				case 4:
					return "Skill4".Translate();
				case 5:
					return "Skill5".Translate();
				case 6:
					return "Skill6".Translate();
				case 7:
					return "Skill7".Translate();
				case 8:
					return "Skill8".Translate();
				case 9:
					return "Skill9".Translate();
				case 10:
					return "Skill10".Translate();
				case 11:
					return "Skill11".Translate();
				case 12:
					return "Skill12".Translate();
				case 13:
					return "Skill13".Translate();
				case 14:
					return "Skill14".Translate();
				case 15:
					return "Skill15".Translate();
				case 16:
					return "Skill16".Translate();
				case 17:
					return "Skill17".Translate();
				case 18:
					return "Skill18".Translate();
				case 19:
					return "Skill19".Translate();
				case 20:
					return "Skill20".Translate();
				default:
					return "Unknown";
				}
			}
		}

		
		
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		
		public SkillRecord()
		{
		}

		
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		
		public void Interval()
		{
			float num = this.pawn.story.traits.HasTrait(TraitDefOf.GreatMemory) ? 0.5f : 1f;
			switch (this.levelInt)
			{
			case 10:
				this.Learn(-0.1f * num, false);
				return;
			case 11:
				this.Learn(-0.2f * num, false);
				return;
			case 12:
				this.Learn(-0.4f * num, false);
				return;
			case 13:
				this.Learn(-0.6f * num, false);
				return;
			case 14:
				this.Learn(-1f * num, false);
				return;
			case 15:
				this.Learn(-1.8f * num, false);
				return;
			case 16:
				this.Learn(-2.8f * num, false);
				return;
			case 17:
				this.Learn(-4f * num, false);
				return;
			case 18:
				this.Learn(-6f * num, false);
				return;
			case 19:
				this.Learn(-8f * num, false);
				return;
			case 20:
				this.Learn(-12f * num, false);
				return;
			default:
				return;
			}
		}

		
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		
		public void Learn(float xp, bool direct = false)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (xp < 0f && this.levelInt == 0)
			{
				return;
			}
			if (xp > 0f)
			{
				xp *= this.LearnRateFactor(direct);
			}
			this.xpSinceLastLevel += xp;
			if (!direct)
			{
				this.xpSinceMidnight += xp;
			}
			if (this.levelInt == 20 && this.xpSinceLastLevel > this.XpRequiredForLevelUp - 1f)
			{
				this.xpSinceLastLevel = this.XpRequiredForLevelUp - 1f;
			}
			while (this.xpSinceLastLevel >= this.XpRequiredForLevelUp)
			{
				this.xpSinceLastLevel -= this.XpRequiredForLevelUp;
				this.levelInt++;
				if (this.levelInt == 14)
				{
					if (this.passion == Passion.None)
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithoutPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
					else
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
				}
				if (this.levelInt >= 20)
				{
					this.levelInt = 20;
					this.xpSinceLastLevel = Mathf.Clamp(this.xpSinceLastLevel, 0f, this.XpRequiredForLevelUp - 1f);
					IL_188:
					while (this.xpSinceLastLevel < 0f)
					{
						this.levelInt--;
						this.xpSinceLastLevel += this.XpRequiredForLevelUp;
						if (this.levelInt <= 0)
						{
							this.levelInt = 0;
							this.xpSinceLastLevel = 0f;
							return;
						}
					}
					return;
				}
			}
			//goto IL_188;
		}

		
		public float LearnRateFactor(bool direct = false)
		{
			if (DebugSettings.fastLearning)
			{
				return 200f;
			}
			float num;
			switch (this.passion)
			{
			case Passion.None:
				num = 0.35f;
				break;
			case Passion.Minor:
				num = 1f;
				break;
			case Passion.Major:
				num = 1.5f;
				break;
			default:
				throw new NotImplementedException("Passion level " + this.passion);
			}
			if (!direct)
			{
				num *= this.pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true);
				if (this.LearningSaturatedToday)
				{
					num *= 0.2f;
				}
			}
			return num;
		}

		
		public void EnsureMinLevelWithMargin(int minLevel)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (this.Level < minLevel || (this.Level == minLevel && this.xpSinceLastLevel < this.XpRequiredForLevelUp / 2f))
			{
				this.Level = minLevel;
				this.xpSinceLastLevel = this.XpRequiredForLevelUp / 2f;
			}
		}

		
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
		}

		
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.story.DisabledWorkTagsBackstoryAndTraits, this.pawn.GetDisabledWorkTypes(true));
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.def.defName,
				": ",
				this.levelInt,
				" (",
				this.xpSinceLastLevel,
				"xp)"
			});
		}

		
		private Pawn pawn;

		
		public SkillDef def;

		
		public int levelInt;

		
		public Passion passion;

		
		public float xpSinceLastLevel;

		
		public float xpSinceMidnight;

		
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		
		public const int IntervalTicks = 200;

		
		public const int MinLevel = 0;

		
		public const int MaxLevel = 20;

		
		public const int MaxFullRateXpPerDay = 4000;

		
		public const int MasterSkillThreshold = 14;

		
		public const float SaturatedLearningFactor = 0.2f;

		
		public const float LearnFactorPassionNone = 0.35f;

		
		public const float LearnFactorPassionMinor = 1f;

		
		public const float LearnFactorPassionMajor = 1.5f;

		
		private static readonly SimpleCurve XpForLevelUpCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1000f),
				true
			},
			{
				new CurvePoint(9f, 10000f),
				true
			},
			{
				new CurvePoint(19f, 30000f),
				true
			}
		};
	}
}
