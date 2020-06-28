using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABD RID: 2749
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06004130 RID: 16688 RVA: 0x0015D3D4 File Offset: 0x0015B5D4
		public static Command_SetPlantToGrow SetPlantToGrowCommand(IPlantToGrowSettable settable)
		{
			return new Command_SetPlantToGrow
			{
				defaultDesc = "CommandSelectPlantToGrowDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc12,
				settable = settable
			};
		}
	}
}
