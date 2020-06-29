﻿using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Hives : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindFirstHivePos(rp.rect, out loc))
			{
				return;
			}
			int num = rp.hivesCount ?? SymbolResolver_Hives.DefaultHivesCountRange.RandomInRange;
			Hive hive = (Hive)ThingMaker.MakeThing(ThingDefOf.Hive, null);
			hive.SetFaction(Faction.OfInsects, null);
			if (rp.disableHives != null && rp.disableHives.Value)
			{
				hive.CompDormant.ToSleep();
			}
			hive = (Hive)GenSpawn.Spawn(hive, loc, BaseGenCore.globalSettings.map, WipeMode.Vanish);
			for (int i = 0; i < num - 1; i++)
			{
				Hive hive2;
				if (hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(true, out hive2))
				{
					hive = hive2;
				}
			}
		}

		
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGenCore.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
