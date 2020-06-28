using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010CC RID: 4300
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x06006567 RID: 25959 RVA: 0x002373EC File Offset: 0x002355EC
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x06006568 RID: 25960 RVA: 0x00237418 File Offset: 0x00235618
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
			hive = (Hive)GenSpawn.Spawn(hive, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
			for (int i = 0; i < num - 1; i++)
			{
				Hive hive2;
				if (hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(true, out hive2))
				{
					hive = hive2;
				}
			}
		}

		// Token: 0x06006569 RID: 25961 RVA: 0x002374E0 File Offset: 0x002356E0
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Token: 0x04003DC3 RID: 15811
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
