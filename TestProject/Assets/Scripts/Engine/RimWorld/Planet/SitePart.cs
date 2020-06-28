using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001261 RID: 4705
	public class SitePart : IExposable, IThingHolder
	{
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06006E25 RID: 28197 RVA: 0x00267930 File Offset: 0x00265B30
		public IThingHolder ParentHolder
		{
			get
			{
				return this.site;
			}
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x00267938 File Offset: 0x00265B38
		public SitePart()
		{
		}

		// Token: 0x06006E27 RID: 28199 RVA: 0x00267947 File Offset: 0x00265B47
		public SitePart(Site site, SitePartDef def, SitePartParams parms)
		{
			this.site = site;
			this.def = def;
			this.parms = parms;
			this.hidden = def.defaultHidden;
		}

		// Token: 0x06006E28 RID: 28200 RVA: 0x00267978 File Offset: 0x00265B78
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

		// Token: 0x06006E29 RID: 28201 RVA: 0x002679FC File Offset: 0x00265BFC
		public void PostDestroy()
		{
			if (this.things != null)
			{
				this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x00267A12 File Offset: 0x00265C12
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x00267A20 File Offset: 0x00265C20
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x00267A28 File Offset: 0x00265C28
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

		// Token: 0x0400440B RID: 17419
		public Site site;

		// Token: 0x0400440C RID: 17420
		public SitePartDef def;

		// Token: 0x0400440D RID: 17421
		public bool hidden;

		// Token: 0x0400440E RID: 17422
		public SitePartParams parms;

		// Token: 0x0400440F RID: 17423
		public ThingOwner things;

		// Token: 0x04004410 RID: 17424
		public int lastRaidTick = -1;

		// Token: 0x04004411 RID: 17425
		public Thing conditionCauser;

		// Token: 0x04004412 RID: 17426
		public bool conditionCauserWasSpawned;

		// Token: 0x04004413 RID: 17427
		private const float AutoFoodLevel = 0.8f;
	}
}
