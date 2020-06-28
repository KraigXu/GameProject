using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC0 RID: 3776
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x001FDBB7 File Offset: 0x001FBDB7
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06005C41 RID: 23617 RVA: 0x001FDBBE File Offset: 0x001FBDBE
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x001FDBF8 File Offset: 0x001FBDF8
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
