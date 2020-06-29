using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_ClassicIntro : StorytellerComp
	{
		
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x0013B2B7 File Offset: 0x001394B7
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target != Find.Maps.Find((Map x) => x.IsPlayerHome))
			{
				yield break;
			}
			if (this.IntervalsPassed == 150)
			{
				IncidentDef visitorGroup = IncidentDefOf.VisitorGroup;
				if (visitorGroup.TargetAllowed(target))
				{
					yield return new FiringIncident(visitorGroup, this, null)
					{
						parms = 
						{
							target = target,
							points = (float)Rand.Range(40, 100)
						}
					};
				}
			}
			if (this.IntervalsPassed == 204)
			{
				IncidentCategoryDef threatCategory = Find.Storyteller.difficulty.allowIntroThreats ? IncidentCategoryDefOf.ThreatSmall : IncidentCategoryDefOf.Misc;
				IncidentDef incidentDef;
				if ((from def in DefDatabase<IncidentDef>.AllDefs
				where def.TargetAllowed(target) && def.category == threatCategory
				select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incidentDef))
				{
					yield return new FiringIncident(incidentDef, this, null)
					{
						parms = StorytellerUtility.DefaultParmsNow(incidentDef.category, target)
					};
				}
			}
			IncidentDef incidentDef2;
			if (this.IntervalsPassed == 264 && (from def in DefDatabase<IncidentDef>.AllDefs
			where def.TargetAllowed(target) && def.category == IncidentCategoryDefOf.Misc
			select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incidentDef2))
			{
				yield return new FiringIncident(incidentDef2, this, null)
				{
					parms = StorytellerUtility.DefaultParmsNow(incidentDef2.category, target)
				};
			}
			if (this.IntervalsPassed == 324)
			{
				IncidentDef incidentDef3 = IncidentDefOf.RaidEnemy;
				if (!Find.Storyteller.difficulty.allowIntroThreats)
				{
					incidentDef3 = (from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(target) && def.category == IncidentCategoryDefOf.Misc
					select def).RandomElementByWeightWithFallback(new Func<IncidentDef, float>(base.IncidentChanceFinal), null);
				}
				if (incidentDef3 != null && incidentDef3.TargetAllowed(target))
				{
					yield return new FiringIncident(incidentDef3, this, null)
					{
						parms = this.GenerateParms(incidentDef3.category, target),
						parms = 
						{
							points = 40f,
							raidForceOneIncap = true,
							raidNeverFleeIndividual = true
						}
					};
				}
			}
			yield break;
		}
	}
}
