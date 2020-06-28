using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A1A RID: 2586
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x00143D18 File Offset: 0x00141F18
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00143D42 File Offset: 0x00141F42
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

		// Token: 0x040023C6 RID: 9158
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
