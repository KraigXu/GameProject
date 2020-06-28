using System;

namespace Verse
{
	// Token: 0x0200036B RID: 875
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x000A189D File Offset: 0x0009FA9D
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		// Token: 0x04000F50 RID: 3920
		public bool allowLoading;
	}
}
