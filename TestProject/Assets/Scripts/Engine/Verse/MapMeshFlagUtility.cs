using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000189 RID: 393
	public static class MapMeshFlagUtility
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x0003D158 File Offset: 0x0003B358
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

		// Token: 0x04000926 RID: 2342
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();
	}
}
