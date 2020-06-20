using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C3 RID: 2243
	public class FleshTypeDef : Def
	{
		// Token: 0x0600360A RID: 13834 RVA: 0x001257BC File Offset: 0x001239BC
		public Material ChooseWoundOverlay()
		{
			if (this.wounds == null)
			{
				return null;
			}
			if (this.woundsResolved == null)
			{
				this.woundsResolved = (from wound in this.wounds
				select wound.GetMaterial()).ToList<Material>();
			}
			return this.woundsResolved.RandomElement<Material>();
		}

		// Token: 0x04001E3A RID: 7738
		public ThoughtDef ateDirect;

		// Token: 0x04001E3B RID: 7739
		public ThoughtDef ateAsIngredient;

		// Token: 0x04001E3C RID: 7740
		public ThingCategoryDef corpseCategory;

		// Token: 0x04001E3D RID: 7741
		public EffecterDef damageEffecter;

		// Token: 0x04001E3E RID: 7742
		public List<FleshTypeDef.Wound> wounds;

		// Token: 0x04001E3F RID: 7743
		private List<Material> woundsResolved;

		// Token: 0x0200191F RID: 6431
		public class Wound
		{
			// Token: 0x060090BC RID: 37052 RVA: 0x002D9D03 File Offset: 0x002D7F03
			public Material GetMaterial()
			{
				return MaterialPool.MatFrom(this.texture, ShaderDatabase.Cutout, this.color);
			}

			// Token: 0x04005FAA RID: 24490
			[NoTranslate]
			public string texture;

			// Token: 0x04005FAB RID: 24491
			public Color color = Color.white;
		}
	}
}
