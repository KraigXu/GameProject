using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001073 RID: 4211
	public static class ApparelGraphicRecordGetter
	{
		// Token: 0x0600640D RID: 25613 RVA: 0x0022AA2C File Offset: 0x00228C2C
		public static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, out ApparelGraphicRecord rec)
		{
			if (bodyType == null)
			{
				Log.Error("Getting apparel graphic with undefined body type.", false);
				bodyType = BodyTypeDefOf.Male;
			}
			if (apparel.def.apparel.wornGraphicPath.NullOrEmpty())
			{
				rec = new ApparelGraphicRecord(null, null);
				return false;
			}
			string path;
			if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || apparel.def.apparel.wornGraphicPath == BaseContent.PlaceholderImagePath)
			{
				path = apparel.def.apparel.wornGraphicPath;
			}
			else
			{
				path = apparel.def.apparel.wornGraphicPath + "_" + bodyType.defName;
			}
			Shader shader = ShaderDatabase.Cutout;
			if (apparel.def.apparel.useWornGraphicMask)
			{
				shader = ShaderDatabase.CutoutComplex;
			}
			Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, shader, apparel.def.graphicData.drawSize, apparel.DrawColor);
			rec = new ApparelGraphicRecord(graphic, apparel);
			return true;
		}
	}
}
