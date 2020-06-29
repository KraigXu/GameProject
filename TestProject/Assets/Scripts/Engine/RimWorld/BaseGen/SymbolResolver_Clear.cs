using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Clear : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			foreach (IntVec3 c in rp.rect)
			{
				if (rp.clearEdificeOnly != null && rp.clearEdificeOnly.Value)
				{
					Building edifice = c.GetEdifice(BaseGenCore.globalSettings.map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
				else if (rp.clearFillageOnly != null && rp.clearFillageOnly.Value)
				{
					SymbolResolver_Clear.tmpThingsToDestroy.Clear();
					SymbolResolver_Clear.tmpThingsToDestroy.AddRange(c.GetThingList(BaseGenCore.globalSettings.map));
					for (int i = 0; i < SymbolResolver_Clear.tmpThingsToDestroy.Count; i++)
					{
						if (SymbolResolver_Clear.tmpThingsToDestroy[i].def.destroyable && SymbolResolver_Clear.tmpThingsToDestroy[i].def.Fillage != FillCategory.None)
						{
							SymbolResolver_Clear.tmpThingsToDestroy[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				else
				{
					SymbolResolver_Clear.tmpThingsToDestroy.Clear();
					SymbolResolver_Clear.tmpThingsToDestroy.AddRange(c.GetThingList(BaseGenCore.globalSettings.map));
					for (int j = 0; j < SymbolResolver_Clear.tmpThingsToDestroy.Count; j++)
					{
						if (SymbolResolver_Clear.tmpThingsToDestroy[j].def.destroyable)
						{
							SymbolResolver_Clear.tmpThingsToDestroy[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
				if (rp.clearRoof != null && rp.clearRoof.Value)
				{
					BaseGenCore.globalSettings.map.roofGrid.SetRoof(c, null);
				}
			}
		}

		
		private static List<Thing> tmpThingsToDestroy = new List<Thing>();
	}
}
