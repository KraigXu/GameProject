using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000847 RID: 2119
	public class ThoughtWorker_PsychicEmanatorSoothe : ThoughtWorker
	{
		// Token: 0x0600349F RID: 13471 RVA: 0x001206FC File Offset: 0x0011E8FC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.PsychicEmanator);
			for (int i = 0; i < list.Count; i++)
			{
				CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
				if ((compPowerTrader == null || compPowerTrader.PowerOn) && p.Position.InHorDistOf(list[i].Position, 15f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001BB9 RID: 7097
		private const float Radius = 15f;
	}
}
