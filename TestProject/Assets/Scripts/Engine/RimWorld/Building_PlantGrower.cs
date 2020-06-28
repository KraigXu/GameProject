using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C82 RID: 3202
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004D0E RID: 19726 RVA: 0x0019D1A8 File Offset: 0x0019B3A8
		public IEnumerable<Plant> PlantsOnMe
		{
			get
			{
				if (!base.Spawned)
				{
					yield break;
				}
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = base.Map.thingGrid.ThingsListAt(c);
					int num;
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						Plant plant = thingList[i] as Plant;
						if (plant != null)
						{
							yield return plant;
						}
						num = i;
					}
					thingList = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0019D1B8 File Offset: 0x0019B3B8
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x0019D1D3 File Offset: 0x0019B3D3
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
			yield break;
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x0019D1E3 File Offset: 0x0019B3E3
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x0019D201 File Offset: 0x0019B401
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x0019D222 File Offset: 0x0019B422
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x0019D23C File Offset: 0x0019B43C
		public override void TickRare()
		{
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Thing thing in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
					thing.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x0019D2C0 File Offset: 0x0019B4C0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x0019D320 File Offset: 0x0019B520
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, true))
				{
					text += "\n" + "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "\n" + "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0019D393 File Offset: 0x0019B593
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0019D39B File Offset: 0x0019B59B
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0019D3A4 File Offset: 0x0019B5A4
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x04002B26 RID: 11046
		private ThingDef plantDefToGrow;

		// Token: 0x04002B27 RID: 11047
		private CompPowerTrader compPower;
	}
}
