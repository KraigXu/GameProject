using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x001FDBB7 File Offset: 0x001FBDB7
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		
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

		
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
