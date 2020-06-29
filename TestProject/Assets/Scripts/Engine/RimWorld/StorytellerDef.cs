using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class StorytellerDef : Def
	{
		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.portraitTiny.NullOrEmpty())
				{
					this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
					this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
				}
			});
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.pointsFactorFromAdaptDays == null)
			{
				yield return "pointsFactorFromAdaptDays is null";
			}
			if (this.adaptDaysLossFromColonistLostByPostPopulation == null)
			{
				yield return "adaptDaysLossFromColonistLostByPostPopulation is null";
			}
			if (this.adaptDaysLossFromColonistViolentlyDownedByPopulation == null)
			{
				yield return "adaptDaysLossFromColonistViolentlyDownedByPopulation is null";
			}
			if (this.adaptDaysGrowthRateCurve == null)
			{
				yield return "adaptDaysGrowthRateCurve is null";
			}
			if (this.pointsFactorFromDaysPassed == null)
			{
				yield return "pointsFactorFromDaysPassed is null";
			}
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		
		public int listOrder = 9999;

		
		public bool listVisible = true;

		
		public bool tutorialMode;

		
		public bool disableAdaptiveTraining;

		
		public bool disableAlerts;

		
		public bool disablePermadeath;

		
		public DifficultyDef forcedDifficulty;

		
		[NoTranslate]
		private string portraitLarge;

		
		[NoTranslate]
		private string portraitTiny;

		
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		
		public SimpleCurve populationIntentFactorFromPopCurve;

		
		public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

		
		public SimpleCurve pointsFactorFromDaysPassed;

		
		public float adaptDaysMin;

		
		public float adaptDaysMax = 100f;

		
		public float adaptDaysGameStartGraceDays;

		
		public SimpleCurve pointsFactorFromAdaptDays;

		
		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

		
		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

		
		public SimpleCurve adaptDaysGrowthRateCurve;

		
		[Unsaved(false)]
		public Texture2D portraitLargeTex;

		
		[Unsaved(false)]
		public Texture2D portraitTinyTex;
	}
}
