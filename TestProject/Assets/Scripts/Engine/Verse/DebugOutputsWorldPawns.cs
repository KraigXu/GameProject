using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200034C RID: 844
	public class DebugOutputsWorldPawns
	{
		// Token: 0x060019DC RID: 6620 RVA: 0x0009ECC4 File Offset: 0x0009CEC4
		[DebugOutput("World pawns", true)]
		public static void ColonistRelativeChance()
		{
			HashSet<Pawn> hashSet = new HashSet<Pawn>(Find.WorldPawns.AllPawnsAliveOrDead);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < 500; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
				list.Add(pawn);
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.KeepForever);
				}
			}
			int num = list.Count((Pawn x) => PawnRelationUtility.GetMostImportantColonyRelative(x) != null);
			Log.Message(string.Concat(new object[]
			{
				"Colony relatives: ",
				((float)num / 500f).ToStringPercent(),
				" (",
				num,
				" of ",
				500,
				")"
			}), false);
			foreach (Pawn pawn2 in Find.WorldPawns.AllPawnsAliveOrDead.ToList<Pawn>())
			{
				if (!hashSet.Contains(pawn2))
				{
					Find.WorldPawns.RemovePawn(pawn2);
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
				}
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0009EE74 File Offset: 0x0009D074
		[DebugOutput("World pawns", true)]
		public static void KidnappedPawns()
		{
			Find.FactionManager.LogKidnappedPawns();
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0009EE80 File Offset: 0x0009D080
		[DebugOutput("World pawns", true)]
		public static void WorldPawnList()
		{
			Find.WorldPawns.LogWorldPawns();
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0009EE8C File Offset: 0x0009D08C
		[DebugOutput("World pawns", true)]
		public static void WorldPawnMothballInfo()
		{
			Find.WorldPawns.LogWorldPawnMothballPrevention();
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0009EE98 File Offset: 0x0009D098
		[DebugOutput("World pawns", true)]
		public static void WorldPawnGcBreakdown()
		{
			Find.WorldPawns.gc.LogGC();
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0009EEA9 File Offset: 0x0009D0A9
		[DebugOutput("World pawns", true)]
		public static void WorldPawnDotgraph()
		{
			Find.WorldPawns.gc.LogDotgraph();
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0009EEBA File Offset: 0x0009D0BA
		[DebugOutput("World pawns", true)]
		public static void RunWorldPawnGc()
		{
			Find.WorldPawns.gc.RunGC();
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0009EECB File Offset: 0x0009D0CB
		[DebugOutput("World pawns", true)]
		public static void RunWorldPawnMothball()
		{
			Find.WorldPawns.DebugRunMothballProcessing();
		}
	}
}
