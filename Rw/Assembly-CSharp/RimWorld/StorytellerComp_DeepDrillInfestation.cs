using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A0B RID: 2571
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06003D21 RID: 15649 RVA: 0x001437DC File Offset: 0x001419DC
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06003D22 RID: 15650 RVA: 0x001437EC File Offset: 0x001419EC
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

		// Token: 0x06003D23 RID: 15651 RVA: 0x00143829 File Offset: 0x00141A29
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

		// Token: 0x040023A9 RID: 9129
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
