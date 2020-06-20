using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C39 RID: 3129
	public class TaleReference : IExposable
	{
		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x00193F3C File Offset: 0x0019213C
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public TaleReference()
		{
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x00193F44 File Offset: 0x00192144
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00193F64 File Offset: 0x00192164
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x00193F89 File Offset: 0x00192189
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x00193FA5 File Offset: 0x001921A5
		public TaggedString GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00193FBC File Offset: 0x001921BC
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

		// Token: 0x04002A61 RID: 10849
		private Tale tale;

		// Token: 0x04002A62 RID: 10850
		private int seed;
	}
}
