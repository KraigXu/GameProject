using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		
		
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

		
		
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
			yield break;
		}

		
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		
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

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		
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

		
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		
		private ThingDef plantDefToGrow;

		
		private CompPowerTrader compPower;
	}
}
