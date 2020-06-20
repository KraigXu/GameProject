using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEF RID: 3055
	public static class FactionUtility
	{
		// Token: 0x060048A6 RID: 18598 RVA: 0x0018B401 File Offset: 0x00189601
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x0018B41F File Offset: 0x0018961F
		public static bool AllyOrNeutralTo(this Faction fac, Faction other)
		{
			return !fac.HostileTo(other);
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x0018B42C File Offset: 0x0018962C
		public static AcceptanceReport CanTradeWith_NewTemp(this Pawn p, Faction faction, TraderKindDef traderKind = null)
		{
			if (p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				return AcceptanceReport.WasRejected;
			}
			if (faction != null)
			{
				if (faction.HostileTo(p.Faction))
				{
					return AcceptanceReport.WasRejected;
				}
				if (traderKind == null || traderKind.permitRequiredForTrading == null)
				{
					return AcceptanceReport.WasAccepted;
				}
				if (p.royalty == null || !p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction))
				{
					return new AcceptanceReport("MessageNeedRoyalTitleToCallWithShip".Translate(traderKind.TitleRequiredToTrade));
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x0018B4C0 File Offset: 0x001896C0
		public static bool CanTradeWith(this Pawn p, Faction faction, TraderKindDef traderKind = null)
		{
			return p.CanTradeWith_NewTemp(faction, traderKind).Accepted;
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x0018B4E0 File Offset: 0x001896E0
		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			if (ft == null)
			{
				return null;
			}
			if (ft.isPlayer)
			{
				return Faction.OfPlayer;
			}
			Faction result;
			if ((from fac in Find.FactionManager.AllFactions
			where fac.def == ft
			select fac).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x0018B53E File Offset: 0x0018973E
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}
	}
}
