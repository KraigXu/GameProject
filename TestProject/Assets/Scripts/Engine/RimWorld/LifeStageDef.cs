using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008DD RID: 2269
	public class LifeStageDef : Def
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600365E RID: 13918 RVA: 0x00126B7B File Offset: 0x00124D7B
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x00126B8D File Offset: 0x00124D8D
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.icon.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				});
			}
		}

		// Token: 0x04001ECA RID: 7882
		[MustTranslate]
		private string adjective;

		// Token: 0x04001ECB RID: 7883
		public bool visible = true;

		// Token: 0x04001ECC RID: 7884
		public bool reproductive;

		// Token: 0x04001ECD RID: 7885
		public bool milkable;

		// Token: 0x04001ECE RID: 7886
		public bool shearable;

		// Token: 0x04001ECF RID: 7887
		public float voxPitch = 1f;

		// Token: 0x04001ED0 RID: 7888
		public float voxVolume = 1f;

		// Token: 0x04001ED1 RID: 7889
		[NoTranslate]
		public string icon;

		// Token: 0x04001ED2 RID: 7890
		[Unsaved(false)]
		public Texture2D iconTex;

		// Token: 0x04001ED3 RID: 7891
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x04001ED4 RID: 7892
		public float bodySizeFactor = 1f;

		// Token: 0x04001ED5 RID: 7893
		public float healthScaleFactor = 1f;

		// Token: 0x04001ED6 RID: 7894
		public float hungerRateFactor = 1f;

		// Token: 0x04001ED7 RID: 7895
		public float marketValueFactor = 1f;

		// Token: 0x04001ED8 RID: 7896
		public float foodMaxFactor = 1f;

		// Token: 0x04001ED9 RID: 7897
		public float meleeDamageFactor = 1f;
	}
}
