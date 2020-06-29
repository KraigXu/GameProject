using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_CallAid : RoyalTitlePermitWorker, ITargetingSource
	{
		
		
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		
		
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		
		
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		
		
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		
		
		public Thing Caster
		{
			get
			{
				return this.caller;
			}
		}

		
		
		public Pawn CasterPawn
		{
			get
			{
				return this.caller;
			}
		}

		
		
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		
		
		public Texture2D UIIcon
		{
			get
			{
				return RoyalTitlePermitWorker_CallAid.CommandTex;
			}
		}

		
		
		public TargetingParameters targetParams
		{
			get
			{
				return this.targetingParameters;
			}
		}

		
		
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return !this.def.royalAid.targetingRequireLOS || GenSight.LineOfSight(this.caller.Position, target.Cell, this.map, true, null, 0, 0);
		}

		
		public bool ValidateTarget(LocalTargetInfo target)
		{
			if (!this.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		
		public void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallAid_NewTemp(this.caller, this.map, target.Cell, this.calledFaction, this.free, this.biocodeChance);
		}

		
		public void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(pawn.MapHeld.mapTemperature.SeasonalTemp))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "BadTemperature".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(pawn.MapHeld, faction))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "HostileVisitorsPresent".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			int permitLastUsedTick = pawn.royalty.GetPermitLastUsedTick(this.def);
			int num = Math.Max(GenTicks.TicksGame - permitLastUsedTick, 0);
			Action action = null;
			bool flag = permitLastUsedTick < 0 || num >= this.def.CooldownTicks;
			int numTicks = (permitLastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
			string text = this.def.LabelCap + ": ";
			if (flag)
			{
				text += "CommandCallRoyalAidFreeOption".Translate();
				action = delegate
				{
					this.BeginCallAid(pawn, map, faction, true, 1f);
				};
			}
			else
			{
				if (pawn.royalty.GetFavor(faction) >= this.def.royalAid.favorCost)
				{
					action = delegate
					{
						this.BeginCallAid(pawn, map, faction, false, 1f);
					};
				}
				text += "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION"));
			}
			yield return new FloatMenuOption(text, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		
		private void BeginCallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = false;
			this.targetingParameters.canTargetPawns = false;
			this.targetingParameters.canTargetFires = false;
			this.targetingParameters.canTargetBuildings = false;
			this.targetingParameters.canTargetItems = false;
			this.targetingParameters.validator = ((TargetInfo target) => (this.def.royalAid.targetingRange <= 0f || target.Cell.DistanceTo(caller.Position) <= this.def.royalAid.targetingRange) && !target.Cell.Fogged(map) && DropCellFinder.CanPhysicallyDropInto(target.Cell, map, true, true) && target.Cell.GetEdifice(map) == null && !target.Cell.Impassable(map));
			this.caller = caller;
			this.map = map;
			this.calledFaction = faction;
			this.free = free;
			this.biocodeChance = biocodeChance;
			Find.Targeter.BeginTargeting(this, null);
		}

		
		[Obsolete]
		private void CallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			this.CallAid_NewTemp(caller, map, caller.Position, faction, free, biocodeChance);
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
			if (this.def.royalAid.pawnKindDef != null)
			{
				incidentParms.pawnKind = this.def.royalAid.pawnKindDef;
				incidentParms.pawnCount = this.def.royalAid.pawnCount;
			}
			else
			{
				incidentParms.points = (float)this.def.royalAid.points;
			}
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			if (IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				if (!free)
				{
					caller.royalty.TryRemoveFavor(faction, this.def.royalAid.favorCost);
				}
				caller.royalty.Notify_PermitUsed(this.def);
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Could not send aid to map ",
				map,
				" from faction ",
				faction
			}), false);
		}

		
		private Pawn caller;

		
		private Map map;

		
		private bool free;

		
		private Faction calledFaction;

		
		private TargetingParameters targetingParameters;

		
		private float biocodeChance;

		
		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
