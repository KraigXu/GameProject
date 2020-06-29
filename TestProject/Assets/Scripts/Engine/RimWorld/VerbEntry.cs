using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct VerbEntry
	{
		
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x0017AE6E File Offset: 0x0017906E
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		
		public VerbEntry(Verb verb, Pawn pawn)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn);
		}

		
		public VerbEntry(Verb verb, Pawn pawn, List<Verb> allVerbs, float highestSelWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = VerbUtility.FinalSelectionWeight(verb, pawn, allVerbs, highestSelWeight);
		}

		
		public float GetSelectionWeight(Thing target)
		{
			if (!this.verb.IsUsableOn(target))
			{
				return 0f;
			}
			return this.cachedSelectionWeight;
		}

		
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}

		
		public Verb verb;

		
		private float cachedSelectionWeight;
	}
}
