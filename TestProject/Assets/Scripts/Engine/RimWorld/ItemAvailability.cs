using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000728 RID: 1832
	public class ItemAvailability
	{
		// Token: 0x06003031 RID: 12337 RVA: 0x0010F0A3 File Offset: 0x0010D2A3
		public ItemAvailability(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x0010F0BD File Offset: 0x0010D2BD
		public void Tick()
		{
			this.cachedResults.Clear();
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x0010F0CC File Offset: 0x0010D2CC
		public bool ThingsAvailableAnywhere(ThingDefCountClass need, Pawn pawn)
		{
			int key = Gen.HashCombine<Faction>(need.GetHashCode(), pawn.Faction);
			bool flag;
			if (!this.cachedResults.TryGetValue(key, out flag))
			{
				List<Thing> list = this.map.listerThings.ThingsOfDef(need.thingDef);
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn))
					{
						num += list[i].stackCount;
						if (num >= need.count)
						{
							break;
						}
					}
				}
				flag = (num >= need.count);
				this.cachedResults.Add(key, flag);
			}
			return flag;
		}

		// Token: 0x04001AE4 RID: 6884
		private Map map;

		// Token: 0x04001AE5 RID: 6885
		private Dictionary<int, bool> cachedResults = new Dictionary<int, bool>();
	}
}
