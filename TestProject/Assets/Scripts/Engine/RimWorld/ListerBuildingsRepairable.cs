using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A3D RID: 2621
	public class ListerBuildingsRepairable
	{
		// Token: 0x06003DE7 RID: 15847 RVA: 0x001466F4 File Offset: 0x001448F4
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x001466FD File Offset: 0x001448FD
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0014670C File Offset: 0x0014490C
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0014671E File Offset: 0x0014491E
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.ListFor(b.Faction).Remove(b);
			this.HashSetFor(b.Faction).Remove(b);
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0014670C File Offset: 0x0014490C
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0014670C File Offset: 0x0014490C
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x00146750 File Offset: 0x00144950
		private void UpdateBuilding(Building b)
		{
			if (b.Faction == null || !b.def.building.repairable)
			{
				return;
			}
			List<Thing> list = this.ListFor(b.Faction);
			HashSet<Thing> hashSet = this.HashSetFor(b.Faction);
			if (b.HitPoints < b.MaxHitPoints)
			{
				if (!list.Contains(b))
				{
					list.Add(b);
				}
				hashSet.Add(b);
				return;
			}
			list.Remove(b);
			hashSet.Remove(b);
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x001467CC File Offset: 0x001449CC
		private List<Thing> ListFor(Faction fac)
		{
			List<Thing> list;
			if (!this.repairables.TryGetValue(fac, out list))
			{
				list = new List<Thing>();
				this.repairables.Add(fac, list);
			}
			return list;
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00146800 File Offset: 0x00144A00
		private HashSet<Thing> HashSetFor(Faction fac)
		{
			HashSet<Thing> hashSet;
			if (!this.repairablesSet.TryGetValue(fac, out hashSet))
			{
				hashSet = new HashSet<Thing>();
				this.repairablesSet.Add(fac, hashSet);
			}
			return hashSet;
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00146834 File Offset: 0x00144A34
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				List<Thing> list = this.ListFor(faction);
				if (!list.NullOrEmpty<Thing>())
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"=======",
						faction.Name,
						" (",
						faction.def,
						")"
					}));
					foreach (Thing thing in list)
					{
						stringBuilder.AppendLine(thing.ThingID);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002420 RID: 9248
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x04002421 RID: 9249
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
