using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C23 RID: 3107
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x06004A18 RID: 18968 RVA: 0x001910BC File Offset: 0x0018F2BC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.project.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<ResearchProjectDef>(this.NonRedundantResearchProjects(), (ResearchProjectDef d) => d.LabelCap, (ResearchProjectDef d) => delegate
				{
					this.project = d;
				});
			}
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x00191125 File Offset: 0x0018F325
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x00191138 File Offset: 0x0018F338
		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return DefDatabase<ResearchProjectDef>.AllDefs.Where(delegate(ResearchProjectDef d)
			{
				if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
				{
					return true;
				}
				return !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
			});
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x00191163 File Offset: 0x0018F363
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0019117B File Offset: 0x0018F37B
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(this.project.LabelCap);
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x0019119C File Offset: 0x0018F39C
		public override void PostGameStart()
		{
			Find.ResearchManager.FinishProject(this.project, false, null);
		}

		// Token: 0x04002A17 RID: 10775
		private ResearchProjectDef project;
	}
}
