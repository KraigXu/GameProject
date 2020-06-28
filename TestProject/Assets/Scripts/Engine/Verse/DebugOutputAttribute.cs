using System;

namespace Verse
{
	// Token: 0x0200035F RID: 863
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugOutputAttribute : Attribute
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x000A0747 File Offset: 0x0009E947
		public DebugOutputAttribute()
		{
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000A075A File Offset: 0x0009E95A
		public DebugOutputAttribute(bool onlyWhenPlaying)
		{
			this.onlyWhenPlaying = onlyWhenPlaying;
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000A0774 File Offset: 0x0009E974
		public DebugOutputAttribute(string category, bool onlyWhenPlaying = false) : this(onlyWhenPlaying)
		{
			this.category = category;
		}

		// Token: 0x04000F3C RID: 3900
		public string name;

		// Token: 0x04000F3D RID: 3901
		public string category = "General";

		// Token: 0x04000F3E RID: 3902
		public bool onlyWhenPlaying;
	}
}
