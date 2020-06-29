using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGenNew;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class SitePartWorker
	{
		
		public virtual void SitePartWorkerTick(SitePart sitePart)
		{
		}

		
		public virtual void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("", part.def));
			outExtraDescriptionConstants.Add("sitePart", part.def.defName);
		}

		
		public virtual void PostMapGenerate(Map map)
		{
		}

		
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		
		public virtual string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}

		
		public virtual string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return this.def.label;
		}

		
		public virtual SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			return new SitePartParams
			{
				randomValue = Rand.Int,
				threatPoints = (this.def.wantsThreatPoints ? myThreatPoints : 0f)
			};
		}

		
		public virtual bool IncreasesPopulation(SitePartParams parms)
		{
			return this.def.increasesPopulation;
		}

		
		public virtual void Init(Site site, SitePart sitePart)
		{
		}

		
		public virtual void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
		}

		
		public virtual void PostDestroy(SitePart sitePart)
		{
		}

		
		public virtual void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
		}

		
		public SitePartDef def;
	}
}
