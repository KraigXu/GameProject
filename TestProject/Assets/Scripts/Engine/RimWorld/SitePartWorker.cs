using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000902 RID: 2306
	public class SitePartWorker
	{
		// Token: 0x060036E9 RID: 14057 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SitePartWorkerTick(SitePart sitePart)
		{
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x00128884 File Offset: 0x00126A84
		public virtual void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("", part.def));
			outExtraDescriptionConstants.Add("sitePart", part.def.defName);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x001288B3 File Offset: 0x00126AB3
		public virtual string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x001288D0 File Offset: 0x00126AD0
		public virtual string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return this.def.label;
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x001288DD File Offset: 0x00126ADD
		public virtual SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			return new SitePartParams
			{
				randomValue = Rand.Int,
				threatPoints = (this.def.wantsThreatPoints ? myThreatPoints : 0f)
			};
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x0012890A File Offset: 0x00126B0A
		public virtual bool IncreasesPopulation(SitePartParams parms)
		{
			return this.def.increasesPopulation;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Init(Site site, SitePart sitePart)
		{
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDestroy(SitePart sitePart)
		{
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
		}

		// Token: 0x04001FC0 RID: 8128
		public SitePartDef def;
	}
}
