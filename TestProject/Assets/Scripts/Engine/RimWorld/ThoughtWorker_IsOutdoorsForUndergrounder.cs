using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083C RID: 2108
	public class ThoughtWorker_IsOutdoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06003488 RID: 13448 RVA: 0x00120160 File Offset: 0x0011E360
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (p.Position.UsesOutdoorTemperature(p.Map) || !p.Position.Roofed(p.Map));
		}
	}
}
