using System;

namespace Verse
{
	// Token: 0x020001D5 RID: 469
	public static class MapSerializeUtility
	{
		// Token: 0x06000D47 RID: 3399 RVA: 0x0004BAA4 File Offset: 0x00049CA4
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0004BAE8 File Offset: 0x00049CE8
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
