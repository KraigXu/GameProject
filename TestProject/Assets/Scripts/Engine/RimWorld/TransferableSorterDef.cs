using System;
using Verse;

namespace RimWorld
{
	
	public class TransferableSorterDef : Def
	{
		
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

		
		public Type comparerClass;

		
		[Unsaved(false)]
		private TransferableComparer comparerInt;
	}
}
