using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C2 RID: 194
	public class LetterDef : Def
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001BC09 File Offset: 0x00019E09
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001BC3E File Offset: 0x00019E3E
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		// Token: 0x0400042C RID: 1068
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x0400042D RID: 1069
		public Color color = Color.white;

		// Token: 0x0400042E RID: 1070
		public Color flashColor = Color.white;

		// Token: 0x0400042F RID: 1071
		public float flashInterval = 90f;

		// Token: 0x04000430 RID: 1072
		public bool bounce;

		// Token: 0x04000431 RID: 1073
		public SoundDef arriveSound;

		// Token: 0x04000432 RID: 1074
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x04000433 RID: 1075
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		// Token: 0x04000434 RID: 1076
		public bool forcedSlowdown;

		// Token: 0x04000435 RID: 1077
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
