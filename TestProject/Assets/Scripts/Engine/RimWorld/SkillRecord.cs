using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBF RID: 3007
	public class SkillRecord : IExposable
	{
		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004710 RID: 18192 RVA: 0x00180A30 File Offset: 0x0017EC30
		// (set) Token: 0x06004711 RID: 18193 RVA: 0x00180A42 File Offset: 0x0017EC42
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

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004712 RID: 18194 RVA: 0x00180A53 File Offset: 0x0017EC53
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004713 RID: 18195 RVA: 0x00180A60 File Offset: 0x0017EC60
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004714 RID: 18196 RVA: 0x00180A70 File Offset: 0x0017EC70
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

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004715 RID: 18197 RVA: 0x00180A9E File Offset: 0x0017EC9E
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

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004716 RID: 18198 RVA: 0x00180AC4 File Offset: 0x0017ECC4
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

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004717 RID: 18199 RVA: 0x00180C8C File Offset: 0x0017EE8C
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x00180C9B File Offset: 0x0017EE9B
		public SkillRecord()
		{
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x00180CAA File Offset: 0x0017EEAA
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x00180CC0 File Offset: 0x0017EEC0
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x00180CE0 File Offset: 0x0017EEE0
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x00180D50 File Offset: 0x0017EF50
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

		// Token: 0x0600471D RID: 18205 RVA: 0x00180E67 File Offset: 0x0017F067
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x00180E78 File Offset: 0x0017F078
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
			goto IL_188;
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x0018101C File Offset: 0x0017F21C
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

		// Token: 0x06004720 RID: 18208 RVA: 0x001810AC File Offset: 0x0017F2AC
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

		// Token: 0x06004721 RID: 18209 RVA: 0x00181101 File Offset: 0x0017F301
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0018110A File Offset: 0x0017F30A
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.story.DisabledWorkTagsBackstoryAndTraits, this.pawn.GetDisabledWorkTypes(true));
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x00181134 File Offset: 0x0017F334
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

		// Token: 0x040028CF RID: 10447
		private Pawn pawn;

		// Token: 0x040028D0 RID: 10448
		public SkillDef def;

		// Token: 0x040028D1 RID: 10449
		public int levelInt;

		// Token: 0x040028D2 RID: 10450
		public Passion passion;

		// Token: 0x040028D3 RID: 10451
		public float xpSinceLastLevel;

		// Token: 0x040028D4 RID: 10452
		public float xpSinceMidnight;

		// Token: 0x040028D5 RID: 10453
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		// Token: 0x040028D6 RID: 10454
		public const int IntervalTicks = 200;

		// Token: 0x040028D7 RID: 10455
		public const int MinLevel = 0;

		// Token: 0x040028D8 RID: 10456
		public const int MaxLevel = 20;

		// Token: 0x040028D9 RID: 10457
		public const int MaxFullRateXpPerDay = 4000;

		// Token: 0x040028DA RID: 10458
		public const int MasterSkillThreshold = 14;

		// Token: 0x040028DB RID: 10459
		public const float SaturatedLearningFactor = 0.2f;

		// Token: 0x040028DC RID: 10460
		public const float LearnFactorPassionNone = 0.35f;

		// Token: 0x040028DD RID: 10461
		public const float LearnFactorPassionMinor = 1f;

		// Token: 0x040028DE RID: 10462
		public const float LearnFactorPassionMajor = 1.5f;

		// Token: 0x040028DF RID: 10463
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
