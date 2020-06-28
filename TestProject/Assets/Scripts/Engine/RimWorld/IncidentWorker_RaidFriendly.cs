using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F7 RID: 2551
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		// Token: 0x06003CA3 RID: 15523 RVA: 0x001406A4 File Offset: 0x0013E8A4
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(x)
			select x into p
			select ((Thing)p).Faction).Distinct<Faction>();
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.def.hidden && f.PlayerRelationKind == FactionRelationKind.Ally && (!source.Any<Faction>() || source.Any((Faction hf) => hf.HostileTo(f)));
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x00140768 File Offset: 0x0013E968
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			return (from p in ((Map)parms.target).attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(p)
			select p).Sum(delegate(IAttackTarget p)
			{
				Pawn pawn = p as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				return 0f;
			}) > 120f;
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x001407E4 File Offset: 0x0013E9E4
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.faction != null)
			{
				return true;
			}
			if (!base.CandidateFactions(map, false).Any<Faction>())
			{
				return false;
			}
			parms.faction = base.CandidateFactions(map, false).RandomElementByWeight((Faction fac) => (float)fac.PlayerGoodwill + 120.000008f);
			return true;
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0014084B File Offset: 0x0013EA4B
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			parms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x00140861 File Offset: 0x0013EA61
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00140881 File Offset: 0x0013EA81
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly + ": " + parms.faction.Name;
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x001408A4 File Offset: 0x0013EAA4
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textFriendly, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction));
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextFriendly;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "FriendlyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER"));
			}
			return text;
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00140982 File Offset: 0x0013EB82
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00140989 File Offset: 0x0013EB89
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}
	}
}
