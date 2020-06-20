using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC0 RID: 2752
	public static class ZonePresetNames
	{
		// Token: 0x06004148 RID: 16712 RVA: 0x0015D53D File Offset: 0x0015B73D
		public static string PresetName(this StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				return "DumpingStockpile".Translate();
			}
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				return "Stockpile".Translate();
			}
			return "Zone".Translate();
		}
	}
}
