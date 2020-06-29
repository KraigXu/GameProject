using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		
		
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		
		
		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				DifficultyDef difficulty = Find.Storyteller.difficulty;
				if (difficulty.deepDrillInfestationChanceFactor <= 0f)
				{
					return -1f;
				}
				return this.Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = (Map)target;
			StorytellerComp_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, StorytellerComp_DeepDrillInfestation.tmpDrills);
			if (!StorytellerComp_DeepDrillInfestation.tmpDrills.Any<Thing>())
			{
				yield break;
			}
			float mtb = this.DeepDrillInfestationMTBDaysPerDrill;
			if (mtb < 0f)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < StorytellerComp_DeepDrillInfestation.tmpDrills.Count; i = num + 1)
			{
				if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
				{
					IncidentParms parms = this.GenerateParms(IncidentCategoryDefOf.DeepDrillInfestation, target);
					IncidentDef def;
					if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, parms).TryRandomElement(out def))
					{
						yield return new FiringIncident(def, this, parms);
					}
				}
				num = i;
			}
			yield break;
		}

		
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
