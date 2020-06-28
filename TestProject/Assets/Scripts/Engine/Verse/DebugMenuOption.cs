using System;

namespace Verse
{
	// Token: 0x0200035B RID: 859
	public struct DebugMenuOption
	{
		// Token: 0x06001A0C RID: 6668 RVA: 0x000A0364 File Offset: 0x0009E564
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x04000F37 RID: 3895
		public DebugMenuOptionMode mode;

		// Token: 0x04000F38 RID: 3896
		public string label;

		// Token: 0x04000F39 RID: 3897
		public Action method;
	}
}
