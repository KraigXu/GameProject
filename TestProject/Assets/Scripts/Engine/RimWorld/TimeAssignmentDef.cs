using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000915 RID: 2325
	public class TimeAssignmentDef : Def
	{
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x001294F3 File Offset: 0x001276F3
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0012951A File Offset: 0x0012771A
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightNotSelectedTag = "TimeAssignmentButton-" + this.defName + "-NotSelected";
		}

		// Token: 0x04002072 RID: 8306
		public Color color;

		// Token: 0x04002073 RID: 8307
		public bool allowRest = true;

		// Token: 0x04002074 RID: 8308
		public bool allowJoy = true;

		// Token: 0x04002075 RID: 8309
		[Unsaved(false)]
		public string cachedHighlightNotSelectedTag;

		// Token: 0x04002076 RID: 8310
		private Texture2D colorTextureInt;
	}
}
