using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAA RID: 2986
	public struct VerbEntry
	{
		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x0017AE6E File Offset: 0x0017906E
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x0017AE7B File Offset: 0x0017907B
		public VerbEntry(Verb verb, Pawn pawn)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn);
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x0017AE97 File Offset: 0x00179097
		public VerbEntry(Verb verb, Pawn pawn, List<Verb> allVerbs, float highestSelWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = VerbUtility.FinalSelectionWeight(verb, pawn, allVerbs, highestSelWeight);
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0017AEB0 File Offset: 0x001790B0
		public float GetSelectionWeight(Thing target)
		{
			if (!this.verb.IsUsableOn(target))
			{
				return 0f;
			}
			return this.cachedSelectionWeight;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x0017AECC File Offset: 0x001790CC
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}

		// Token: 0x04002846 RID: 10310
		public Verb verb;

		// Token: 0x04002847 RID: 10311
		private float cachedSelectionWeight;
	}
}
