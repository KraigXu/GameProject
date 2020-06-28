using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A4C RID: 2636
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x00149078 File Offset: 0x00147278
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x00149080 File Offset: 0x00147280
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}

		// Token: 0x04002456 RID: 9302
		private const int MinRoomCellCount = 10;
	}
}
