using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011FE RID: 4606
	public static class HillinessUtility
	{
		// Token: 0x06006A7F RID: 27263 RVA: 0x00252320 File Offset: 0x00250520
		public static string GetLabel(this Hilliness h)
		{
			switch (h)
			{
			case Hilliness.Flat:
				return "Hilliness_Flat".Translate();
			case Hilliness.SmallHills:
				return "Hilliness_SmallHills".Translate();
			case Hilliness.LargeHills:
				return "Hilliness_LargeHills".Translate();
			case Hilliness.Mountainous:
				return "Hilliness_Mountainous".Translate();
			case Hilliness.Impassable:
				return "Hilliness_Impassable".Translate();
			default:
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362, false);
				return h.ToString();
			}
		}

		// Token: 0x06006A80 RID: 27264 RVA: 0x002523CA File Offset: 0x002505CA
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
