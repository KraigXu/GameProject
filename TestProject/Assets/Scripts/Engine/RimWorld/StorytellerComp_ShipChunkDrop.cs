using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x00143D18 File Offset: 0x00141F18
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.ShipChunkDropMTBDays, 60000f, 1000f))
			{
				IncidentDef shipChunkDrop = IncidentDefOf.ShipChunkDrop;
				IncidentParms parms = this.GenerateParms(shipChunkDrop.category, target);
				if (shipChunkDrop.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(shipChunkDrop, this, parms);
				}
			}
			yield break;
		}

		
		private static readonly SimpleCurve ShipChunkDropMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(1f, 40f),
				true
			},
			{
				new CurvePoint(2f, 80f),
				true
			},
			{
				new CurvePoint(2.75f, 135f),
				true
			}
		};
	}
}
