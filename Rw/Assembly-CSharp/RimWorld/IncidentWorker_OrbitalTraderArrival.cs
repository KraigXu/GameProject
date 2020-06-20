using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009EA RID: 2538
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		// Token: 0x06003C6D RID: 15469 RVA: 0x0013F4B1 File Offset: 0x0013D6B1
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ((Map)parms.target).passingShipManager.passingShips.Count < 5;
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x0013F4E0 File Offset: 0x0013D6E0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (map.passingShipManager.passingShips.Count >= 5)
			{
				return false;
			}
			TraderKindDef traderKindDef;
			if ((from x in DefDatabase<TraderKindDef>.AllDefs
			where this.CanSpawn(map, x)
			select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out traderKindDef))
			{
				TradeShip tradeShip = new TradeShip(traderKindDef, this.GetFaction(traderKindDef));
				if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.IsCommsConsole && (b.GetComp<CompPowerTrader>() == null || b.GetComp<CompPowerTrader>().PowerOn)))
				{
					base.SendStandardLetter(tradeShip.def.LabelCap, "TraderArrival".Translate(tradeShip.name, tradeShip.def.label, (tradeShip.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(tradeShip.Faction.Named("FACTION"))), LetterDefOf.PositiveEvent, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
				}
				map.passingShipManager.AddShip(tradeShip);
				tradeShip.GenerateThings();
				return true;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x0013F640 File Offset: 0x0013D840
		private Faction GetFaction(TraderKindDef trader)
		{
			if (trader.faction == null)
			{
				return null;
			}
			Faction result;
			if (!(from f in Find.FactionManager.AllFactions
			where f.def == trader.faction
			select f).TryRandomElement(out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x0013F690 File Offset: 0x0013D890
		private bool CanSpawn(Map map, TraderKindDef trader)
		{
			if (!trader.orbital)
			{
				return false;
			}
			if (trader.faction == null)
			{
				return true;
			}
			Faction faction = this.GetFaction(trader);
			if (faction == null)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonists.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanTradeWith(faction, trader))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400238D RID: 9101
		private const int MaxShips = 5;
	}
}
