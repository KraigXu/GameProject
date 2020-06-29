using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_Disease : StorytellerComp
	{
		
		// (get) Token: 0x06003D28 RID: 15656 RVA: 0x0014387C File Offset: 0x00141A7C
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!DebugSettings.enableRandomDiseases)
			{
				yield break;
			}
			if (target is World)
			{
				yield break;
			}
			BiomeDef biome = Find.WorldGrid[target.Tile].biome;
			float num = biome.diseaseMtbDays;
			num *= Find.Storyteller.difficulty.diseaseIntervalFactor;
			if (target is Caravan)
			{
				num *= this.CaravanDiseaseMTBFactor;
			}
			if (Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.category, target);
				IncidentDef def;
				if (base.UsableIncidentsInCategory(this.Props.category, parms).TryRandomElementByWeight((IncidentDef d) => biome.CommonalityOfDisease(d), out def))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}

		
		private float CaravanDiseaseMTBFactor = 4f;
	}
}
