using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class SitePartDef : Def
	{
		
		
		public SitePartWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SitePartWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (!this.expandingIconTexture.NullOrEmpty())
					{
						this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
					}
					else
					{
						this.expandingIconTextureInt = BaseContent.BadTex;
					}
				}
				return this.expandingIconTextureInt;
			}
		}

		
		
		public List<GenStepDef> ExtraGenSteps
		{
			get
			{
				if (this.extraGenSteps == null)
				{
					this.extraGenSteps = new List<GenStepDef>();
					List<GenStepDef> allDefsListForReading = DefDatabase<GenStepDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].linkWithSite == this)
						{
							this.extraGenSteps.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.extraGenSteps;
			}
		}

		
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		
		public bool FactionCanOwn(Faction faction)
		{
			return (!this.requiresFaction || faction != null) && (this.minFactionTechLevel == TechLevel.Undefined || (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) && (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.def.hidden)) && this.Worker.FactionCanOwn(faction);
		}

		
		public bool CompatibleWith(SitePartDef part)
		{
			for (int i = 0; i < part.excludesTags.Count; i++)
			{
				if (this.tags.Contains(part.excludesTags[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < this.excludesTags.Count; j++)
			{
				if (part.tags.Contains(this.excludesTags[j]))
				{
					return false;
				}
			}
			return true;
		}

		
		public ThingDef conditionCauserDef;

		
		public float activeThreatDisturbanceFactor = 1f;

		
		public bool defaultHidden;

		
		public Type workerClass = typeof(SitePartWorker);

		
		[NoTranslate]
		public string siteTexture;

		
		[NoTranslate]
		public string expandingIconTexture;

		
		public bool applyFactionColorToSiteTexture;

		
		public bool showFactionInInspectString;

		
		public bool requiresFaction;

		
		public TechLevel minFactionTechLevel;

		
		[MustTranslate]
		public string approachOrderString;

		
		[MustTranslate]
		public string approachingReportString;

		
		[NoTranslate]
		public List<string> tags = new List<string>();

		
		[NoTranslate]
		public List<string> excludesTags = new List<string>();

		
		[MustTranslate]
		public string arrivedLetter;

		
		[MustTranslate]
		public string arrivedLetterLabelPart;

		
		public List<HediffDef> arrivedLetterHediffHyperlinks;

		
		public LetterDef arrivedLetterDef;

		
		public bool wantsThreatPoints;

		
		public float minThreatPoints;

		
		public bool increasesPopulation;

		
		public bool badEvenIfNoMap;

		
		public float forceExitAndRemoveMapCountdownDurationDays = 3f;

		
		public bool handlesWorldObjectTimeoutInspectString;

		
		[Unsaved(false)]
		private SitePartWorker workerInt;

		
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		
		[Unsaved(false)]
		private List<GenStepDef> extraGenSteps;
	}
}
