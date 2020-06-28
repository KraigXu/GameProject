using System;

namespace RimWorld
{
	// Token: 0x02000C51 RID: 3153
	public interface IOpenable
	{
		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004B4F RID: 19279
		bool CanOpen { get; }

		// Token: 0x06004B50 RID: 19280
		void Open();
	}
}
