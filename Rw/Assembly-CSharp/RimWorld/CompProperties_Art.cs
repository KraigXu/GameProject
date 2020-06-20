using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000863 RID: 2147
	public class CompProperties_Art : CompProperties
	{
		// Token: 0x06003504 RID: 13572 RVA: 0x001226FF File Offset: 0x001208FF
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}

		// Token: 0x04001C33 RID: 7219
		public RulePackDef nameMaker;

		// Token: 0x04001C34 RID: 7220
		public RulePackDef descriptionMaker;

		// Token: 0x04001C35 RID: 7221
		public QualityCategory minQualityForArtistic;

		// Token: 0x04001C36 RID: 7222
		public bool mustBeFullGrave;

		// Token: 0x04001C37 RID: 7223
		public bool canBeEnjoyedAsArt;
	}
}
