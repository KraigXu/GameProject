using System;

namespace Verse
{
	// Token: 0x020001D4 RID: 468
	public static class MapExposeUtility
	{
		// Token: 0x06000D46 RID: 3398 RVA: 0x0004BA6C File Offset: 0x00049C6C
		public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUshort(map, shortReader);
			}
			DataExposeUtility.ByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUshort(arr, map, shortWriter);
			}
		}
	}
}
