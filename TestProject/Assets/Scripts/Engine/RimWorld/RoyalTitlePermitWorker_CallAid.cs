using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104C RID: 4172
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_CallAid : RoyalTitlePermitWorker, ITargetingSource
	{
		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x0600639D RID: 25501 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x0600639E RID: 25502 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x0600639F RID: 25503 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x060063A0 RID: 25504 RVA: 0x00010306 File Offset: 0x0000E506
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x060063A1 RID: 25505 RVA: 0x00228DC2 File Offset: 0x00226FC2
		public Thing Caster
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x060063A2 RID: 25506 RVA: 0x00228DC2 File Offset: 0x00226FC2
		public Pawn CasterPawn
		{
			get
			{
				return this.caller;
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x060063A3 RID: 25507 RVA: 0x00019EA1 File Offset: 0x000180A1
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x060063A4 RID: 25508 RVA: 0x00228DCA File Offset: 0x00226FCA
		public Texture2D UIIcon
		{
			get
			{
				return RoyalTitlePermitWorker_CallAid.CommandTex;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x060063A5 RID: 25509 RVA: 0x00228DD1 File Offset: 0x00226FD1
		public TargetingParameters targetParams
		{
			get
			{
				return this.targetingParameters;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x060063A6 RID: 25510 RVA: 0x00019EA1 File Offset: 0x000180A1
		public ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060063A7 RID: 25511 RVA: 0x00228DD9 File Offset: 0x00226FD9
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return !this.def.royalAid.targetingRequireLOS || GenSight.LineOfSight(this.caller.Position, target.Cell, this.map, true, null, 0, 0);
		}

		// Token: 0x060063A8 RID: 25512 RVA: 0x00228E14 File Offset: 0x00227014
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

		// Token: 0x060063A9 RID: 25513 RVA: 0x00228E6A File Offset: 0x0022706A
		public void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x060063AA RID: 25514 RVA: 0x00228EA1 File Offset: 0x002270A1
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallAid_NewTemp(this.caller, this.map, target.Cell, this.calledFaction, this.free, this.biocodeChance);
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x00228ED0 File Offset: 0x002270D0
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

		// Token: 0x060063AC RID: 25516 RVA: 0x00228F15 File Offset: 0x00227115
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

		// Token: 0x060063AD RID: 25517 RVA: 0x00228F3C File Offset: 0x0022713C
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

		// Token: 0x060063AE RID: 25518 RVA: 0x00229009 File Offset: 0x00227209
		[Obsolete]
		private void CallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			this.CallAid_NewTemp(caller, map, caller.Position, faction, free, biocodeChance);
		}

		// Token: 0x060063AF RID: 25519 RVA: 0x00229020 File Offset: 0x00227220
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

		// Token: 0x04003CA1 RID: 15521
		private Pawn caller;

		// Token: 0x04003CA2 RID: 15522
		private Map map;

		// Token: 0x04003CA3 RID: 15523
		private bool free;

		// Token: 0x04003CA4 RID: 15524
		private Faction calledFaction;

		// Token: 0x04003CA5 RID: 15525
		private TargetingParameters targetingParameters;

		// Token: 0x04003CA6 RID: 15526
		private float biocodeChance;

		// Token: 0x04003CA7 RID: 15527
		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
