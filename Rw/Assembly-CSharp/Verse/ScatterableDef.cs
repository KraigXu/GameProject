using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E7 RID: 231
	public class ScatterableDef : Def
	{
		// Token: 0x06000649 RID: 1609 RVA: 0x0001DD80 File Offset: 0x0001BF80
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x0400055D RID: 1373
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400055E RID: 1374
		public float minSize;

		// Token: 0x0400055F RID: 1375
		public float maxSize;

		// Token: 0x04000560 RID: 1376
		public float selectionWeight = 100f;

		// Token: 0x04000561 RID: 1377
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04000562 RID: 1378
		public Material mat;
	}
}
