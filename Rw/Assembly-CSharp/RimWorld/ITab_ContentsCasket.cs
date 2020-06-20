using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA5 RID: 3749
	public class ITab_ContentsCasket : ITab_ContentsBase
	{
		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06005B84 RID: 23428 RVA: 0x001F82FC File Offset: 0x001F64FC
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

		// Token: 0x06005B85 RID: 23429 RVA: 0x001F8342 File Offset: 0x001F6542
		public ITab_ContentsCasket()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "ContainedItems";
			this.canRemoveThings = false;
		}

		// Token: 0x040031F9 RID: 12793
		private List<Thing> listInt = new List<Thing>();
	}
}
