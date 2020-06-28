using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AA RID: 682
	public static class MaterialLoader
	{
		// Token: 0x06001385 RID: 4997 RVA: 0x00070118 File Offset: 0x0006E318
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			return (from Texture2D tex in Resources.LoadAll("Textures/" + dirPath, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00070170 File Offset: 0x0006E370
		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault<Material>();
			if (material == null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending, false);
				return BaseContent.BadMat;
			}
			return material;
		}
	}
}
