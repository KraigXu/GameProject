﻿using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x02001266 RID: 4710
	public class SitePartWorker_PrisonerWillingToJoin : SitePartWorker
	{
		// Token: 0x06006E38 RID: 28216 RVA: 0x00267F28 File Offset: 0x00266128
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			Pawn pawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(part.site.Tile, part.site.Faction);
			part.things = new ThingOwner<Pawn>(part, true, LookMode.Deep);
			part.things.TryAdd(pawn, true);
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(Gen.YieldSingle<Pawn>(pawn), out text, true, false);
			string output;
			if (!text.NullOrEmpty())
			{
				output = "\n\n" + "PawnHasTheseRelationshipsWithColonists".Translate(pawn.LabelShort, pawn) + "\n\n" + text;
			}
			else
			{
				output = "";
			}
			slate.Set<Pawn>("prisoner", pawn, false);
			outExtraDescriptionRules.Add(new Rule_String("prisonerFullRelationInfo", output));
		}

		// Token: 0x06006E39 RID: 28217 RVA: 0x00267FF0 File Offset: 0x002661F0
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
	}
}
