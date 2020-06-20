using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D4 RID: 212
	public class AlternateGraphic
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001C3E4 File Offset: 0x0001A5E4
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001C3EC File Offset: 0x0001A5EC
		public Graphic GetGraphic(Graphic other)
		{
			if (this.graphicData == null)
			{
				this.graphicData = new GraphicData();
			}
			this.graphicData.CopyFrom(other.data);
			if (!this.texPath.NullOrEmpty())
			{
				this.graphicData.texPath = this.texPath;
			}
			this.graphicData.color = (this.color ?? other.color);
			this.graphicData.colorTwo = (this.colorTwo ?? other.colorTwo);
			return this.graphicData.Graphic;
		}

		// Token: 0x040004E7 RID: 1255
		private float weight = 0.5f;

		// Token: 0x040004E8 RID: 1256
		private string texPath;

		// Token: 0x040004E9 RID: 1257
		private Color? color;

		// Token: 0x040004EA RID: 1258
		private Color? colorTwo;

		// Token: 0x040004EB RID: 1259
		private GraphicData graphicData;
	}
}
