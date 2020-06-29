﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		
		public ThingOwner contents;
	}
}
