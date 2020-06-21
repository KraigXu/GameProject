using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x02001264 RID: 4708
	public class SitePartWorker_DownedRefugee : SitePartWorker
	{
		// Token: 0x06006E31 RID: 28209 RVA: 0x00267B74 File Offset: 0x00265D74
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			Pawn pawn = DownedRefugeeQuestUtility.GenerateRefugee(part.site.Tile);
			part.things = new ThingOwner<Pawn>(part, true, LookMode.Deep);
			part.things.TryAdd(pawn, true);
			if (pawn.relations != null)
			{
				pawn.relations.everSeenByPlayer = true;
			}
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative != null)
			{
				PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
				TaggedString taggedString = "";
				if (mostImportantRelation != null && mostImportantRelation.opinionOffset > 0)
				{
					pawn.relations.relativeInvolvedInRescueQuest = mostImportantColonyRelative;
					taggedString = "\n\n" + "RelatedPawnInvolvedInQuest".Translate(mostImportantColonyRelative.LabelShort, mostImportantRelation.GetGenderSpecificLabel(pawn), mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
				}
				else
				{
					PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
				}
				outExtraDescriptionRules.Add(new Rule_String("pawnInvolvedInQuestInfo", taggedString));
			}
			slate.Set<Pawn>("refugee", pawn, false);
		}

		// Token: 0x06006E32 RID: 28210 RVA: 0x00267C88 File Offset: 0x00265E88
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			string text = base.GetPostProcessedThreatLabel(site, sitePart);
			if (sitePart.things != null && sitePart.things.Any)
			{
				text = text + ": " + sitePart.things[0].LabelShortCap;
			}
			if (site.HasWorldObjectTimeout)
			{
				text += " (" + "DurationLeft".Translate(site.WorldObjectTimeoutTicksLeft.ToStringTicksToPeriod(true, false, true, true)) + ")";
			}
			return text;
		}

		// Token: 0x06006E33 RID: 28211 RVA: 0x00267D18 File Offset: 0x00265F18
		public override void PostDestroy(SitePart sitePart)
		{
			base.PostDestroy(sitePart);
			if (sitePart.things != null && sitePart.things.Any)
			{
				Pawn pawn = (Pawn)sitePart.things[0];
				if (!pawn.Dead)
				{
					if (pawn.relations != null)
					{
						pawn.relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(pawn);
				}
			}
		}
	}
}
