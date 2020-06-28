using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A05 RID: 2565
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x001436E8 File Offset: 0x001418E8
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x001436F5 File Offset: 0x001418F5
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target is World)
			{
				yield break;
			}
			List<IncidentDef> allIncidents = DefDatabase<IncidentDef>.AllDefsListForReading;
			int num2;
			for (int i = 0; i < allIncidents.Count; i = num2 + 1)
			{
				IncidentDef incidentDef = allIncidents[i];
				if (incidentDef.category == this.Props.category)
				{
					BiomeDef biome = Find.WorldGrid[target.Tile].biome;
					if (incidentDef.mtbDaysByBiome != null)
					{
						MTBByBiome mtbbyBiome = incidentDef.mtbDaysByBiome.Find((MTBByBiome x) => x.biome == biome);
						if (mtbbyBiome != null)
						{
							float num = mtbbyBiome.mtbDays;
							if (this.Props.applyCaravanVisibility)
							{
								Caravan caravan = target as Caravan;
								if (caravan != null)
								{
									num /= caravan.Visibility;
								}
								else
								{
									Map map = target as Map;
									if (map != null && map.Parent.def.isTempIncidentMapOwner)
									{
										IEnumerable<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Concat(map.mapPawns.PrisonersOfColonySpawned);
										num /= CaravanVisibilityCalculator.Visibility(pawns, false, null);
									}
								}
							}
							if (Rand.MTBEventOccurs(num, 60000f, 1000f))
							{
								IncidentParms parms = this.GenerateParms(incidentDef.category, target);
								if (incidentDef.Worker.CanFireNow(parms, false))
								{
									yield return new FiringIncident(incidentDef, this, parms);
								}
							}
						}
					}
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0014370C File Offset: 0x0014190C
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
