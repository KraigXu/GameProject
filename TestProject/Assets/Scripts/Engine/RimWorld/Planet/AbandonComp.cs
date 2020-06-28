using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001273 RID: 4723
	public class AbandonComp : WorldObjectComp
	{
		// Token: 0x06006EBF RID: 28351 RVA: 0x0026A062 File Offset: 0x00268262
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = this.parent as MapParent;
			if (mapParent.HasMap && mapParent.Faction == Faction.OfPlayer)
			{
				yield return SettlementAbandonUtility.AbandonCommand(mapParent);
			}
			yield break;
		}
	}
}
