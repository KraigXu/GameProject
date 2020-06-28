using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBC RID: 4028
	internal static class FilthMonitor
	{
		// Token: 0x060060CA RID: 24778 RVA: 0x0021797C File Offset: 0x00215B7C
		public static void FilthMonitorTick()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			if (FilthMonitor.lastUpdate + 2500 <= Find.TickManager.TicksAbs)
			{
				int num = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer);
				int num2 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike);
				int num3 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && !pawn.RaceProps.Humanlike);
				Log.Message(string.Format("Filth data, per day:\n  {0} filth spawned per pawn\n  {1} filth human-generated per human\n  {2} filth animal-generated per animal\n  {3} filth accumulated per pawn\n  {4} filth dropped per pawn", new object[]
				{
					(float)FilthMonitor.filthSpawned / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthHumanGenerated / (float)num2 / 2500f * 60000f,
					(float)FilthMonitor.filthAnimalGenerated / (float)num3 / 2500f * 60000f,
					(float)FilthMonitor.filthAccumulated / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthDropped / (float)num / 2500f * 60000f
				}), false);
				FilthMonitor.filthSpawned = 0;
				FilthMonitor.filthAnimalGenerated = 0;
				FilthMonitor.filthHumanGenerated = 0;
				FilthMonitor.filthAccumulated = 0;
				FilthMonitor.filthDropped = 0;
				FilthMonitor.lastUpdate = Find.TickManager.TicksAbs;
			}
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x00217AFD File Offset: 0x00215CFD
		public static void Notify_FilthAccumulated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAccumulated++;
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x00217B13 File Offset: 0x00215D13
		public static void Notify_FilthDropped()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthDropped++;
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x00217B29 File Offset: 0x00215D29
		public static void Notify_FilthAnimalGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAnimalGenerated++;
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x00217B3F File Offset: 0x00215D3F
		public static void Notify_FilthHumanGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthHumanGenerated++;
		}

		// Token: 0x060060CF RID: 24783 RVA: 0x00217B55 File Offset: 0x00215D55
		public static void Notify_FilthSpawned()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthSpawned++;
		}

		// Token: 0x04003B00 RID: 15104
		private static int lastUpdate;

		// Token: 0x04003B01 RID: 15105
		private static int filthAccumulated;

		// Token: 0x04003B02 RID: 15106
		private static int filthDropped;

		// Token: 0x04003B03 RID: 15107
		private static int filthAnimalGenerated;

		// Token: 0x04003B04 RID: 15108
		private static int filthHumanGenerated;

		// Token: 0x04003B05 RID: 15109
		private static int filthSpawned;

		// Token: 0x04003B06 RID: 15110
		private const int SampleDuration = 2500;
	}
}
