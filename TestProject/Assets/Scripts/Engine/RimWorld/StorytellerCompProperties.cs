using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090B RID: 2315
	public class StorytellerCompProperties
	{
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06003713 RID: 14099 RVA: 0x00128DD8 File Offset: 0x00126FD8
		public bool Enabled
		{
			get
			{
				if (!this.enableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int i = 0; i < this.enableIfAnyModActive.Count; i++)
					{
						if (ModsConfig.IsActive(this.enableIfAnyModActive[i]))
						{
							return true;
						}
					}
					return false;
				}
				if (!this.disableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int j = 0; j < this.disableIfAnyModActive.Count; j++)
					{
						if (ModsConfig.IsActive(this.disableIfAnyModActive[j]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x00128E58 File Offset: 0x00127058
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x00128E6B File Offset: 0x0012706B
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x00128E85 File Offset: 0x00127085
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "a StorytellerCompProperties has null compClass.";
			}
			if (!this.enableIfAnyModActive.NullOrEmpty<string>() && !this.disableIfAnyModActive.NullOrEmpty<string>())
			{
				yield return "enableIfAnyModActive and disableIfAnyModActive can't be used simultaneously";
			}
			yield break;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		// Token: 0x0400201A RID: 8218
		[TranslationHandle]
		public Type compClass;

		// Token: 0x0400201B RID: 8219
		public float minDaysPassed;

		// Token: 0x0400201C RID: 8220
		public List<IncidentTargetTagDef> allowedTargetTags;

		// Token: 0x0400201D RID: 8221
		public List<IncidentTargetTagDef> disallowedTargetTags;

		// Token: 0x0400201E RID: 8222
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x0400201F RID: 8223
		public List<string> enableIfAnyModActive;

		// Token: 0x04002020 RID: 8224
		public List<string> disableIfAnyModActive;
	}
}
