using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200090E RID: 2318
	public class TaleDef : Def
	{
		// Token: 0x06003719 RID: 14105 RVA: 0x00128E95 File Offset: 0x00127095
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.taleClass == null)
			{
				yield return this.defName + " taleClass is null.";
			}
			if (this.expireDays < 0f)
			{
				if (this.type == TaleType.Expirable)
				{
					yield return "Expirable tale type is used but expireDays<0";
				}
			}
			else if (this.type != TaleType.Expirable)
			{
				yield return "Non expirable tale type is used but expireDays>=0";
			}
			if (this.baseInterest > 1E-06f && !this.usableForArt)
			{
				yield return "Non-zero baseInterest but not usable for art";
			}
			if (this.firstPawnSymbol == "pawn" || this.secondPawnSymbol == "pawn")
			{
				yield return "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x00128EA5 File Offset: 0x001270A5
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		// Token: 0x04002025 RID: 8229
		public TaleType type;

		// Token: 0x04002026 RID: 8230
		public Type taleClass;

		// Token: 0x04002027 RID: 8231
		public bool usableForArt = true;

		// Token: 0x04002028 RID: 8232
		public bool colonistOnly = true;

		// Token: 0x04002029 RID: 8233
		public int maxPerPawn = -1;

		// Token: 0x0400202A RID: 8234
		public float ignoreChance;

		// Token: 0x0400202B RID: 8235
		public float expireDays = -1f;

		// Token: 0x0400202C RID: 8236
		public RulePack rulePack;

		// Token: 0x0400202D RID: 8237
		[NoTranslate]
		public string firstPawnSymbol;

		// Token: 0x0400202E RID: 8238
		[NoTranslate]
		public string secondPawnSymbol;

		// Token: 0x0400202F RID: 8239
		[NoTranslate]
		public string defSymbol;

		// Token: 0x04002030 RID: 8240
		public Type defType = typeof(ThingDef);

		// Token: 0x04002031 RID: 8241
		public float baseInterest;

		// Token: 0x04002032 RID: 8242
		public Color historyGraphColor = Color.white;
	}
}
