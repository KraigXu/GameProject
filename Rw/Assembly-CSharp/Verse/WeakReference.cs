using System;

namespace Verse
{
	// Token: 0x02000453 RID: 1107
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06002113 RID: 8467 RVA: 0x000CB067 File Offset: 0x000C9267
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x000CB075 File Offset: 0x000C9275
		// (set) Token: 0x06002115 RID: 8469 RVA: 0x000CB082 File Offset: 0x000C9282
		public new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
