using System;

namespace Verse
{
	// Token: 0x020003A6 RID: 934
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x06001B7D RID: 7037 RVA: 0x000A85A3 File Offset: 0x000A67A3
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x04001045 RID: 4165
		public float lineHeight = 20f;
	}
}
