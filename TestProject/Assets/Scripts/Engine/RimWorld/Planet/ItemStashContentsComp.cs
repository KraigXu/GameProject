using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200127C RID: 4732
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06006EF4 RID: 28404 RVA: 0x0026AA25 File Offset: 0x00268C25
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06006EF5 RID: 28405 RVA: 0x0026AA39 File Offset: 0x00268C39
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06006EF6 RID: 28406 RVA: 0x0026AA5B File Offset: 0x00268C5B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006EF7 RID: 28407 RVA: 0x0026AA69 File Offset: 0x00268C69
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06006EF8 RID: 28408 RVA: 0x0026AA71 File Offset: 0x00268C71
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0400444B RID: 17483
		public ThingOwner contents;
	}
}
