using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ITab_ContentsCasket : ITab_ContentsBase
	{
		
		
		public override IList<Thing> container
		{
			get
			{
				Building_Casket building_Casket = base.SelThing as Building_Casket;
				this.listInt.Clear();
				if (building_Casket != null && building_Casket.ContainedThing != null)
				{
					this.listInt.Add(building_Casket.ContainedThing);
				}
				return this.listInt;
			}
		}

		
		public ITab_ContentsCasket()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "ContainedItems";
			this.canRemoveThings = false;
		}

		
		private List<Thing> listInt = new List<Thing>();
	}
}
