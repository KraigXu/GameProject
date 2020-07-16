using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_CallAid : RoyalTitlePermitWorker, ITargetingSource
	{
		private Pawn caller;

		private Map map;

		private bool free;

		private Faction calledFaction;

		private TargetingParameters targetingParameters;

		private float biocodeChance;

		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement");

		public bool CasterIsPawn => true;

		public bool IsMeleeAttack => false;

		public bool Targetable => true;

		public bool MultiSelect => false;

		public Thing Caster => caller;

		public Pawn CasterPawn => caller;

		public Verb GetVerb => null;

		public Texture2D UIIcon => CommandTex;

		public TargetingParameters targetParams => targetingParameters;

		public ITargetingSource DestinationSelector => null;

		public bool CanHitTarget(LocalTargetInfo target)
		{
			if (def.royalAid.targetingRequireLOS && !GenSight.LineOfSight(caller.Position, target.Cell, map, skipFirstCell: true))
			{
				return false;
			}
			return true;
		}

		public bool ValidateTarget(LocalTargetInfo target)
		{
			if (!CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput);
				}
				return false;
			}
			return true;
		}

		public void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(caller.Position, def.royalAid.targetingRange, Color.white);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		public void OrderForceTarget(LocalTargetInfo target)
		{
			CallAid_NewTemp(caller, map, target.Cell, calledFaction, free, biocodeChance);
		}

		public void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon = (!target.IsValid) ? TexCommand.CannotShoot : ((!(UIIcon != BaseContent.BadTex)) ? TexCommand.Attack : UIIcon);
			GenUI.DrawMouseAttachment(icon);
		}

		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null);
				yield break;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(pawn.MapHeld.mapTemperature.SeasonalTemp))
			{
				yield return new FloatMenuOption(def.LabelCap + ": " + "BadTemperature".Translate(), null);
				yield break;
			}
			if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(pawn.MapHeld, faction))
			{
				yield return new FloatMenuOption(def.LabelCap + ": " + "HostileVisitorsPresent".Translate(), null);
				yield break;
			}
			int permitLastUsedTick = pawn.royalty.GetPermitLastUsedTick(def);
			int num = Math.Max(GenTicks.TicksGame - permitLastUsedTick, 0);
			Action action = null;
			bool num2 = permitLastUsedTick < 0 || num >= def.CooldownTicks;
			int numTicks = (permitLastUsedTick > 0) ? Math.Max(def.CooldownTicks - num, 0) : 0;
			string t = def.LabelCap + ": ";
			if (num2)
			{
				t += "CommandCallRoyalAidFreeOption".Translate();
				action = delegate
				{
					BeginCallAid(pawn, map, faction, free: true);
				};
			}
			else
			{
				if (pawn.royalty.GetFavor(faction) >= def.royalAid.favorCost)
				{
					action = delegate
					{
						BeginCallAid(pawn, map, faction, free: false);
					};
				}
				t += "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), def.royalAid.favorCost, faction.Named("FACTION"));
			}
			yield return new FloatMenuOption(t, action, faction.def.FactionIcon, faction.Color);
		}

		private void BeginCallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = delegate(TargetInfo target)
			{
				if (def.royalAid.targetingRange > 0f && target.Cell.DistanceTo(caller.Position) > def.royalAid.targetingRange)
				{
					return false;
				}
				if (target.Cell.Fogged(map) || !DropCellFinder.CanPhysicallyDropInto(target.Cell, map, canRoofPunch: true))
				{
					return false;
				}
				return target.Cell.GetEdifice(map) == null && !target.Cell.Impassable(map);
			};
			this.caller = caller;
			this.map = map;
			calledFaction = faction;
			this.free = free;
			this.biocodeChance = biocodeChance;
			Find.Targeter.BeginTargeting(this);
		}

		[Obsolete]
		private void CallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			CallAid_NewTemp(caller, map, caller.Position, faction, free, biocodeChance);
		}

		private void CallAid_NewTemp(Pawn caller, Map map, IntVec3 spawnPos, Faction faction, bool free, float biocodeChance = 1f)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.biocodeApparelChance = biocodeChance;
			incidentParms.biocodeWeaponsChance = biocodeChance;
			incidentParms.spawnCenter = spawnPos;
			if (def.royalAid.pawnKindDef != null)
			{
				incidentParms.pawnKind = def.royalAid.pawnKindDef;
				incidentParms.pawnCount = def.royalAid.pawnCount;
			}
			else
			{
				incidentParms.points = def.royalAid.points;
			}
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			if (IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				if (!free)
				{
					caller.royalty.TryRemoveFavor(faction, def.royalAid.favorCost);
				}
				caller.royalty.Notify_PermitUsed(def);
			}
			else
			{
				Log.Error("Could not send aid to map " + map + " from faction " + faction);
			}
		}
	}
}
