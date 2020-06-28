using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000175 RID: 373
	public sealed class ListerBuildings
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x00039020 File Offset: 0x00037220
		public void Add(Building b)
		{
			if (b.def.building != null && b.def.building.isNaturalRock)
			{
				return;
			}
			if (b.Faction == Faction.OfPlayer)
			{
				this.allBuildingsColonist.Add(b);
				if (b is IAttackTarget)
				{
					this.allBuildingsColonistCombatTargets.Add(b);
				}
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Add(b);
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000390A0 File Offset: 0x000372A0
		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x000390F4 File Offset: 0x000372F4
		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00039130 File Offset: 0x00037330
		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0003916C File Offset: 0x0003736C
		public bool ColonistsHaveResearchBench()
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i] is Building_ResearchBench)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000391A8 File Offset: 0x000373A8
		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					CompPowerTrader compPowerTrader = this.allBuildingsColonist[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000391FF File Offset: 0x000373FF
		public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					yield return this.allBuildingsColonist[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00039216 File Offset: 0x00037416
		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				T t = this.allBuildingsColonist[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04000852 RID: 2130
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04000853 RID: 2131
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04000854 RID: 2132
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();
	}
}
