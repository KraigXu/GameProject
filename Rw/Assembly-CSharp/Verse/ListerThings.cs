using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000179 RID: 377
	public sealed class ListerThings
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00039287 File Offset: 0x00037487
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00039291 File Offset: 0x00037491
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x000392CF File Offset: 0x000374CF
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x000392DD File Offset: 0x000374DD
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x000392EC File Offset: 0x000374EC
		public List<Thing> ThingsMatching(ThingRequest req)
		{
			if (req.singleDef != null)
			{
				List<Thing> result;
				if (!this.listsByDef.TryGetValue(req.singleDef, out result))
				{
					return ListerThings.EmptyList;
				}
				return result;
			}
			else
			{
				if (req.group == ThingRequestGroup.Undefined)
				{
					throw new InvalidOperationException("Invalid ThingRequest " + req);
				}
				if (this.use == ListerThingsUse.Region && !req.group.StoreInRegion())
				{
					Log.ErrorOnce("Tried to get things in group " + req.group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132, false);
					return ListerThings.EmptyList;
				}
				return this.listsByGroup[(int)req.group] ?? ListerThings.EmptyList;
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00039393 File Offset: 0x00037593
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x000393A4 File Offset: 0x000375A4
		public void Add(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (!this.listsByDef.TryGetValue(t.def, out list))
			{
				list = new List<Thing>();
				this.listsByDef.Add(t.def, list);
			}
			list.Add(t);
			foreach (ThingRequestGroup thingRequestGroup in ThingListGroupHelper.AllGroups)
			{
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					List<Thing> list2 = this.listsByGroup[(int)thingRequestGroup];
					if (list2 == null)
					{
						list2 = new List<Thing>();
						this.listsByGroup[(int)thingRequestGroup] = list2;
					}
					list2.Add(t);
				}
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00039458 File Offset: 0x00037658
		public void Remove(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			this.listsByDef[t.def].Remove(t);
			ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
			for (int i = 0; i < allGroups.Length; i++)
			{
				ThingRequestGroup group = allGroups[i];
				if ((this.use != ListerThingsUse.Region || group.StoreInRegion()) && group.Includes(t.def))
				{
					this.listsByGroup[i].Remove(t);
				}
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x000394D7 File Offset: 0x000376D7
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region) && def.category != ThingCategory.Gas;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003950C File Offset: 0x0003770C
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
			}
		}

		// Token: 0x0400085B RID: 2139
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x0400085C RID: 2140
		private List<Thing>[] listsByGroup;

		// Token: 0x0400085D RID: 2141
		public ListerThingsUse use;

		// Token: 0x0400085E RID: 2142
		private static readonly List<Thing> EmptyList = new List<Thing>();
	}
}
