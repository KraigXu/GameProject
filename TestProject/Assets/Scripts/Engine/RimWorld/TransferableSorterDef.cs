using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091B RID: 2331
	public class TransferableSorterDef : Def
	{
		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x00129AE8 File Offset: 0x00127CE8
		public TransferableComparer Comparer
		{
			get
			{
				if (this.comparerInt == null)
				{
					this.comparerInt = (TransferableComparer)Activator.CreateInstance(this.comparerClass);
				}
				return this.comparerInt;
			}
		}

		// Token: 0x040020B3 RID: 8371
		public Type comparerClass;

		// Token: 0x040020B4 RID: 8372
		[Unsaved(false)]
		private TransferableComparer comparerInt;
	}
}
