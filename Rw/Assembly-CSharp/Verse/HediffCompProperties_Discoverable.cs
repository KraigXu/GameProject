using System;

namespace Verse
{
	// Token: 0x0200024D RID: 589
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x0005D5E5 File Offset: 0x0005B7E5
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x04000BEA RID: 3050
		public bool sendLetterWhenDiscovered;

		// Token: 0x04000BEB RID: 3051
		public string discoverLetterLabel;

		// Token: 0x04000BEC RID: 3052
		public string discoverLetterText;

		// Token: 0x04000BED RID: 3053
		public MessageTypeDef messageType;

		// Token: 0x04000BEE RID: 3054
		public LetterDef letterType;
	}
}
