using System;

namespace Verse
{
	
	public class WeakReference<T> : WeakReference where T : class
	{
		
		public WeakReference(T target) : base(target)
		{
		}

		
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
