using System;
using System.Collections.Generic;

namespace Verse
{
	
	public static class MapMeshFlagUtility
	{
		
		static MapMeshFlagUtility()
		{
			foreach (object obj in Enum.GetValues(typeof(MapMeshFlag)))
			{
				MapMeshFlag mapMeshFlag = (MapMeshFlag)obj;
				if (mapMeshFlag != MapMeshFlag.None)
				{
					MapMeshFlagUtility.allFlags.Add(mapMeshFlag);
				}
			}
		}

		
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();
	}
}
