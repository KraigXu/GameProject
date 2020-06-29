using System;
using Verse;

namespace RimWorld
{
	
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x00149078 File Offset: 0x00147278
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}

		
		private const int MinRoomCellCount = 10;
	}
}
