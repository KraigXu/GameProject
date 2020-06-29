﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ((Map)parms.target).passingShipManager.passingShips.Count < 5;
		}

		
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

		
		private const int MaxShips = 5;
	}
}
