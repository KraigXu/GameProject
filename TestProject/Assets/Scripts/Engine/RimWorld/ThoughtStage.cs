using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThoughtStage
	{
		
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

		
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		
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

		
		[MustTranslate]
		public string label;

		
		[MustTranslate]
		public string labelSocial;

		
		[MustTranslate]
		public string description;

		
		public float baseMoodEffect;

		
		public float baseOpinionOffset;

		
		public bool visible = true;

		
		[Unsaved(false)]
		private string cachedLabelCap;

		
		[Unsaved(false)]
		private string cachedLabelSocialCap;

		
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelSocial;
	}
}
