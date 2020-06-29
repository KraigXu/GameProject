﻿using System;
using Verse;

namespace RimWorld
{
	
	public class TaleReference : IExposable
	{
		
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x00193F3C File Offset: 0x0019213C
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		
		public TaleReference()
		{
		}

		
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		
		public TaggedString GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"TaleReference(tale=",
				(this.tale == null) ? "null" : this.tale.ToString(),
				", seed=",
				this.seed,
				")"
			});
		}

		
		private Tale tale;

		
		private int seed;
	}
}
