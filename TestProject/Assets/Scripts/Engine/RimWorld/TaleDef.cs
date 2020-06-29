using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class TaleDef : Def
	{
		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		
		public TaleType type;

		
		public Type taleClass;

		
		public bool usableForArt = true;

		
		public bool colonistOnly = true;

		
		public int maxPerPawn = -1;

		
		public float ignoreChance;

		
		public float expireDays = -1f;

		
		public RulePack rulePack;

		
		[NoTranslate]
		public string firstPawnSymbol;

		
		[NoTranslate]
		public string secondPawnSymbol;

		
		[NoTranslate]
		public string defSymbol;

		
		public Type defType = typeof(ThingDef);

		
		public float baseInterest;

		
		public Color historyGraphColor = Color.white;
	}
}
