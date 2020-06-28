using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010D4 RID: 4308
	public class SymbolResolver_Refuel : SymbolResolver
	{
		// Token: 0x06006583 RID: 25987 RVA: 0x00238228 File Offset: 0x00236428
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_Refuel.refuelables.Clear();
			foreach (IntVec3 c in rp.rect)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					CompRefuelable compRefuelable = thingList[i].TryGetComp<CompRefuelable>();
					if (compRefuelable != null && !SymbolResolver_Refuel.refuelables.Contains(compRefuelable))
					{
						SymbolResolver_Refuel.refuelables.Add(compRefuelable);
					}
				}
			}
			for (int j = 0; j < SymbolResolver_Refuel.refuelables.Count; j++)
			{
				float fuelCapacity = SymbolResolver_Refuel.refuelables[j].Props.fuelCapacity;
				float amount = Rand.Range(fuelCapacity / 2f, fuelCapacity);
				SymbolResolver_Refuel.refuelables[j].Refuel(amount);
			}
			SymbolResolver_Refuel.refuelables.Clear();
		}

		// Token: 0x04003DCB RID: 15819
		private static List<CompRefuelable> refuelables = new List<CompRefuelable>();
	}
}
