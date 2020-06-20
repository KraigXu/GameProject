using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000901 RID: 2305
	public class SitePartDef : Def
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060036E3 RID: 14051 RVA: 0x00128661 File Offset: 0x00126861
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

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060036E4 RID: 14052 RVA: 0x00128694 File Offset: 0x00126894
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

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060036E5 RID: 14053 RVA: 0x001286E4 File Offset: 0x001268E4
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

		// Token: 0x060036E6 RID: 14054 RVA: 0x00128744 File Offset: 0x00126944
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x001287A4 File Offset: 0x001269A4
		public bool FactionCanOwn(Faction faction)
		{
			return (!this.requiresFaction || faction != null) && (this.minFactionTechLevel == TechLevel.Undefined || (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) && (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.def.hidden)) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x00128814 File Offset: 0x00126A14
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

		// Token: 0x04001FA5 RID: 8101
		public ThingDef conditionCauserDef;

		// Token: 0x04001FA6 RID: 8102
		public float activeThreatDisturbanceFactor = 1f;

		// Token: 0x04001FA7 RID: 8103
		public bool defaultHidden;

		// Token: 0x04001FA8 RID: 8104
		public Type workerClass = typeof(SitePartWorker);

		// Token: 0x04001FA9 RID: 8105
		[NoTranslate]
		public string siteTexture;

		// Token: 0x04001FAA RID: 8106
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04001FAB RID: 8107
		public bool applyFactionColorToSiteTexture;

		// Token: 0x04001FAC RID: 8108
		public bool showFactionInInspectString;

		// Token: 0x04001FAD RID: 8109
		public bool requiresFaction;

		// Token: 0x04001FAE RID: 8110
		public TechLevel minFactionTechLevel;

		// Token: 0x04001FAF RID: 8111
		[MustTranslate]
		public string approachOrderString;

		// Token: 0x04001FB0 RID: 8112
		[MustTranslate]
		public string approachingReportString;

		// Token: 0x04001FB1 RID: 8113
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04001FB2 RID: 8114
		[NoTranslate]
		public List<string> excludesTags = new List<string>();

		// Token: 0x04001FB3 RID: 8115
		[MustTranslate]
		public string arrivedLetter;

		// Token: 0x04001FB4 RID: 8116
		[MustTranslate]
		public string arrivedLetterLabelPart;

		// Token: 0x04001FB5 RID: 8117
		public List<HediffDef> arrivedLetterHediffHyperlinks;

		// Token: 0x04001FB6 RID: 8118
		public LetterDef arrivedLetterDef;

		// Token: 0x04001FB7 RID: 8119
		public bool wantsThreatPoints;

		// Token: 0x04001FB8 RID: 8120
		public float minThreatPoints;

		// Token: 0x04001FB9 RID: 8121
		public bool increasesPopulation;

		// Token: 0x04001FBA RID: 8122
		public bool badEvenIfNoMap;

		// Token: 0x04001FBB RID: 8123
		public float forceExitAndRemoveMapCountdownDurationDays = 3f;

		// Token: 0x04001FBC RID: 8124
		public bool handlesWorldObjectTimeoutInspectString;

		// Token: 0x04001FBD RID: 8125
		[Unsaved(false)]
		private SitePartWorker workerInt;

		// Token: 0x04001FBE RID: 8126
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04001FBF RID: 8127
		[Unsaved(false)]
		private List<GenStepDef> extraGenSteps;
	}
}
