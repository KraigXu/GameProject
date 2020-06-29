using System;
using Verse;

namespace RimWorld
{
	
	public class TransferableSorterDef : Def
	{
		
		
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
