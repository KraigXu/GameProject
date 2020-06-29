using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class GameCondition_PsychicSuppression : GameCondition
	{
		
		
		public override string LetterText
		{
			get
			{
				return base.LetterText.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		
		
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		
		public override void Init()
		{
			base.Init();
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		
		public static void CheckPawn(Pawn pawn, Gender targetGender)
		{
			if (pawn.RaceProps.Humanlike && pawn.gender == targetGender && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicSuppression, false))
			{
				pawn.health.AddHediff(HediffDefOf.PsychicSuppression, null, null, null);
			}
		}

		
		public override void GameConditionTick()
		{
			foreach (Map map in base.AffectedMaps)
			{
				foreach (Pawn pawn in map.mapPawns.AllPawns)
				{
					GameCondition_PsychicSuppression.CheckPawn(pawn, this.gender);
				}
			}
		}

		
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicSuppressorGender", this.gender.GetLabel(false)));
		}

		
		public Gender gender;
	}
}
