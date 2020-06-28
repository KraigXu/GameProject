using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104B RID: 4171
	public class RoyalTitlePermitWorker
	{
		// Token: 0x06006399 RID: 25497 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600639A RID: 25498 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual IEnumerable<DiaOption> GetFactionCommDialogOptions(Map map, Pawn pawn, Faction factionInFavor)
		{
			return null;
		}

		// Token: 0x0600639B RID: 25499 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x04003CA0 RID: 15520
		public RoyalTitlePermitDef def;
	}
}
