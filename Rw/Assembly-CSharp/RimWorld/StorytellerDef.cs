using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090A RID: 2314
	public class StorytellerDef : Def
	{
		// Token: 0x0600370E RID: 14094 RVA: 0x00128D18 File Offset: 0x00126F18
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

		// Token: 0x0600370F RID: 14095 RVA: 0x00128D64 File Offset: 0x00126F64
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
			foreach (string text in this.<>n__0())
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

		// Token: 0x04002004 RID: 8196
		public int listOrder = 9999;

		// Token: 0x04002005 RID: 8197
		public bool listVisible = true;

		// Token: 0x04002006 RID: 8198
		public bool tutorialMode;

		// Token: 0x04002007 RID: 8199
		public bool disableAdaptiveTraining;

		// Token: 0x04002008 RID: 8200
		public bool disableAlerts;

		// Token: 0x04002009 RID: 8201
		public bool disablePermadeath;

		// Token: 0x0400200A RID: 8202
		public DifficultyDef forcedDifficulty;

		// Token: 0x0400200B RID: 8203
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x0400200C RID: 8204
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x0400200D RID: 8205
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x0400200E RID: 8206
		public SimpleCurve populationIntentFactorFromPopCurve;

		// Token: 0x0400200F RID: 8207
		public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

		// Token: 0x04002010 RID: 8208
		public SimpleCurve pointsFactorFromDaysPassed;

		// Token: 0x04002011 RID: 8209
		public float adaptDaysMin;

		// Token: 0x04002012 RID: 8210
		public float adaptDaysMax = 100f;

		// Token: 0x04002013 RID: 8211
		public float adaptDaysGameStartGraceDays;

		// Token: 0x04002014 RID: 8212
		public SimpleCurve pointsFactorFromAdaptDays;

		// Token: 0x04002015 RID: 8213
		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

		// Token: 0x04002016 RID: 8214
		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

		// Token: 0x04002017 RID: 8215
		public SimpleCurve adaptDaysGrowthRateCurve;

		// Token: 0x04002018 RID: 8216
		[Unsaved(false)]
		public Texture2D portraitLargeTex;

		// Token: 0x04002019 RID: 8217
		[Unsaved(false)]
		public Texture2D portraitTinyTex;
	}
}
