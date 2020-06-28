using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020009C1 RID: 2497
	public class GameCondition_PsychicSuppression : GameCondition
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x0013AEFD File Offset: 0x001390FD
		public override string LetterText
		{
			get
			{
				return base.LetterText.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x0013AF25 File Offset: 0x00139125
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x0013AF4D File Offset: 0x0013914D
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x0013AF55 File Offset: 0x00139155
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x0013AF70 File Offset: 0x00139170
		public static void CheckPawn(Pawn pawn, Gender targetGender)
		{
			if (pawn.RaceProps.Humanlike && pawn.gender == targetGender && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicSuppression, false))
			{
				pawn.health.AddHediff(HediffDefOf.PsychicSuppression, null, null, null);
			}
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x0013AFC8 File Offset: 0x001391C8
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

		// Token: 0x06003BAC RID: 15276 RVA: 0x0013B05C File Offset: 0x0013925C
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

		// Token: 0x04002333 RID: 9011
		public Gender gender;
	}
}
