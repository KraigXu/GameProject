using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECB RID: 3787
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06005CD5 RID: 23765 RVA: 0x00203905 File Offset: 0x00201B05
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06005CD6 RID: 23766 RVA: 0x0020390C File Offset: 0x00201B0C
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && (p.Faction == null || p.Faction == Faction.OfInsects) && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map) && !p.IsPrisonerInPrisonCell()
				select p;
			}
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x001FDBF8 File Offset: 0x001FBDF8
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
