using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class SitePart : IExposable, IThingHolder
	{
		
		// (get) Token: 0x06006E25 RID: 28197 RVA: 0x00267930 File Offset: 0x00265B30
		public IThingHolder ParentHolder
		{
			get
			{
				return this.site;
			}
		}

		
		public SitePart()
		{
		}

		
		public SitePart(Site site, SitePartDef def, SitePartParams parms)
		{
			this.site = site;
			this.def = def;
			this.parms = parms;
			this.hidden = def.defaultHidden;
		}

		
		public void SitePartTick()
		{
			if (this.things != null)
			{
				if (this.things.contentsLookMode == LookMode.Deep)
				{
					this.things.ThingOwnerTick(true);
				}
				for (int i = 0; i < this.things.Count; i++)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn != null && !pawn.Destroyed && pawn.needs.food != null)
					{
						pawn.needs.food.CurLevelPercentage = 0.8f;
					}
				}
			}
		}

		
		public void PostDestroy()
		{
			if (this.things != null)
			{
				this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", -1, false);
			Scribe_Values.Look<bool>(ref this.conditionCauserWasSpawned, "conditionCauserWasSpawned", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			if (this.conditionCauserWasSpawned)
			{
				Scribe_References.Look<Thing>(ref this.conditionCauser, "conditionCauser", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.conditionCauser, "conditionCauser", Array.Empty<object>());
		}

		
		public Site site;

		
		public SitePartDef def;

		
		public bool hidden;

		
		public SitePartParams parms;

		
		public ThingOwner things;

		
		public int lastRaidTick = -1;

		
		public Thing conditionCauser;

		
		public bool conditionCauserWasSpawned;

		
		private const float AutoFoodLevel = 0.8f;
	}
}
