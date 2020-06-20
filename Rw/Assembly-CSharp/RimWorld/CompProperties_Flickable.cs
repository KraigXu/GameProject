using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x06003542 RID: 13634 RVA: 0x00123197 File Offset: 0x00121397
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x04001CAD RID: 7341
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x04001CAE RID: 7342
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x04001CAF RID: 7343
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
