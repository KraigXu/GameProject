using System;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x020001F4 RID: 500
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06000E1A RID: 3610 RVA: 0x00050C5F File Offset: 0x0004EE5F
		public LoadedContentItem(VirtualFile internalFile, T contentItem, IDisposable extraDisposable = null)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
			this.extraDisposable = extraDisposable;
		}

		// Token: 0x04000AA9 RID: 2729
		public VirtualFile internalFile;

		// Token: 0x04000AAA RID: 2730
		public T contentItem;

		// Token: 0x04000AAB RID: 2731
		public IDisposable extraDisposable;
	}
}
