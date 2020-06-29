using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ThoughtStage
	{
		
		
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
