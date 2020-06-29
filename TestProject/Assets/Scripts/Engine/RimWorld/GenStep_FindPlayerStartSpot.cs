using System;
using Verse;

namespace RimWorld
{
	
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		
		
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
