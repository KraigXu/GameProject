using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StorytellerCompProperties
	{
		
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

		
		public StorytellerCompProperties()
		{
		}

		
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		
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

		
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		
		[TranslationHandle]
		public Type compClass;

		
		public float minDaysPassed;

		
		public List<IncidentTargetTagDef> allowedTargetTags;

		
		public List<IncidentTargetTagDef> disallowedTargetTags;

		
		public float minIncChancePopulationIntentFactor = 0.05f;

		
		public List<string> enableIfAnyModActive;

		
		public List<string> disableIfAnyModActive;
	}
}
