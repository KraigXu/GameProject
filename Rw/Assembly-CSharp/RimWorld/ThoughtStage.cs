using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000913 RID: 2323
	public class ThoughtStage
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x0600372F RID: 14127 RVA: 0x00129228 File Offset: 0x00127428
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06003730 RID: 14128 RVA: 0x00129249 File Offset: 0x00127449
		public string LabelSocialCap
		{
			get
			{
				if (this.cachedLabelSocialCap == null)
				{
					this.cachedLabelSocialCap = this.labelSocial.CapitalizeFirst();
				}
				return this.cachedLabelSocialCap;
			}
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x0012926A File Offset: 0x0012746A
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00129284 File Offset: 0x00127484
		public IEnumerable<string> ConfigErrors()
		{
			if (!this.labelSocial.NullOrEmpty() && this.labelSocial == this.label)
			{
				yield return "labelSocial is the same as label. labelSocial is unnecessary in this case";
			}
			if (this.baseMoodEffect != 0f && this.description.NullOrEmpty())
			{
				yield return "affects mood but doesn't have a description";
			}
			yield break;
		}

		// Token: 0x0400204D RID: 8269
		[MustTranslate]
		public string label;

		// Token: 0x0400204E RID: 8270
		[MustTranslate]
		public string labelSocial;

		// Token: 0x0400204F RID: 8271
		[MustTranslate]
		public string description;

		// Token: 0x04002050 RID: 8272
		public float baseMoodEffect;

		// Token: 0x04002051 RID: 8273
		public float baseOpinionOffset;

		// Token: 0x04002052 RID: 8274
		public bool visible = true;

		// Token: 0x04002053 RID: 8275
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04002054 RID: 8276
		[Unsaved(false)]
		private string cachedLabelSocialCap;

		// Token: 0x04002055 RID: 8277
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04002056 RID: 8278
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelSocial;
	}
}
