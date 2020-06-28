using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010C4 RID: 4292
	public class SymbolResolver_ChargeBatteries : SymbolResolver
	{
		// Token: 0x06006552 RID: 25938 RVA: 0x00236524 File Offset: 0x00234724
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_ChargeBatteries.batteries.Clear();
			foreach (IntVec3 c in rp.rect)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					CompPowerBattery compPowerBattery = thingList[i].TryGetComp<CompPowerBattery>();
					if (compPowerBattery != null && !SymbolResolver_ChargeBatteries.batteries.Contains(compPowerBattery))
					{
						SymbolResolver_ChargeBatteries.batteries.Add(compPowerBattery);
					}
				}
			}
			for (int j = 0; j < SymbolResolver_ChargeBatteries.batteries.Count; j++)
			{
				float num = Rand.Range(0.1f, 0.3f);
				SymbolResolver_ChargeBatteries.batteries[j].SetStoredEnergyPct(Mathf.Min(SymbolResolver_ChargeBatteries.batteries[j].StoredEnergyPct + num, 1f));
			}
			SymbolResolver_ChargeBatteries.batteries.Clear();
		}

		// Token: 0x04003DBF RID: 15807
		private static List<CompPowerBattery> batteries = new List<CompPowerBattery>();
	}
}
