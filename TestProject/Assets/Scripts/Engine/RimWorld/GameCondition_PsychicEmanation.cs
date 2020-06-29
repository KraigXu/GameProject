using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class GameCondition_PsychicEmanation : GameCondition
	{
		
		// (get) Token: 0x06003B65 RID: 15205 RVA: 0x00139F54 File Offset: 0x00138154
		public override string Label
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.label + ": " + this.gender.GetLabel(false).CapitalizeFirst();
				}
				if (this.gender != Gender.None)
				{
					return string.Concat(new string[]
					{
						this.def.label,
						": ",
						this.level.GetLabel().CapitalizeFirst(),
						" (",
						this.gender.GetLabel(false).ToLower(),
						")"
					});
				}
				return this.def.label + ": " + this.level.GetLabel().CapitalizeFirst();
			}
		}

		
		// (get) Token: 0x06003B66 RID: 15206 RVA: 0x0013A018 File Offset: 0x00138218
		public override string LetterText
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower());
				}
				return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower(), this.level.GetLabel());
			}
		}

		
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x0013A095 File Offset: 0x00138295
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		
		public override void PostMake()
		{
			base.PostMake();
			this.level = this.def.defaultDroneLevel;
		}

		
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			if (this.def.defaultDroneLevel == PsychicDroneLevel.GoodMedium)
			{
				this.level = PsychicDroneLevel.GoodMedium;
			}
			else if (points < 800f)
			{
				this.level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				this.level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				this.level = PsychicDroneLevel.BadHigh;
			}
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneLevel", this.level.GetLabel()));
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneGender", this.gender.GetLabel(false)));
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.level, "level", PsychicDroneLevel.None, false);
		}

		
		public Gender gender;

		
		public PsychicDroneLevel level = PsychicDroneLevel.BadMedium;
	}
}
