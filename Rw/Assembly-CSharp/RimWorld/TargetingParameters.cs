using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001026 RID: 4134
	public class TargetingParameters
	{
		// Token: 0x060062F6 RID: 25334 RVA: 0x002260A0 File Offset: 0x002242A0
		public bool CanTarget(TargetInfo targ)
		{
			if (this.validator != null && !this.validator(targ))
			{
				return false;
			}
			if (targ.Thing == null)
			{
				return this.canTargetLocations;
			}
			if (this.neverTargetDoors && targ.Thing.def.IsDoor)
			{
				return false;
			}
			if (this.onlyTargetDamagedThings && targ.Thing.HitPoints == targ.Thing.MaxHitPoints)
			{
				return false;
			}
			if (this.onlyTargetFlammables && !targ.Thing.FlammableNow)
			{
				return false;
			}
			if (this.mustBeSelectable && !ThingSelectionUtility.SelectableByMapClick(targ.Thing))
			{
				return false;
			}
			if (this.targetSpecificThing != null && targ.Thing == this.targetSpecificThing)
			{
				return true;
			}
			if (this.canTargetFires && targ.Thing.def == ThingDefOf.Fire)
			{
				return true;
			}
			if (this.canTargetPawns && targ.Thing.def.category == ThingCategory.Pawn)
			{
				Pawn pawn = (Pawn)targ.Thing;
				if (pawn.Downed)
				{
					if (this.neverTargetIncapacitated)
					{
						return false;
					}
				}
				else if (this.onlyTargetIncapacitatedPawns)
				{
					return false;
				}
				if (this.onlyTargetFactions != null && !this.onlyTargetFactions.Contains(targ.Thing.Faction))
				{
					return false;
				}
				if (pawn.NonHumanlikeOrWildMan())
				{
					if (pawn.Faction == Faction.OfMechanoids)
					{
						if (!this.canTargetMechs)
						{
							return false;
						}
					}
					else if (!this.canTargetAnimals)
					{
						return false;
					}
				}
				return (pawn.NonHumanlikeOrWildMan() || this.canTargetHumans) && (!this.onlyTargetControlledPawns || pawn.IsColonistPlayerControlled) && (!this.onlyTargetColonists || (pawn.IsColonist && pawn.HostFaction == null));
			}
			else
			{
				if (this.canTargetBuildings && targ.Thing.def.category == ThingCategory.Building)
				{
					return (!this.onlyTargetThingsAffectingRegions || targ.Thing.def.AffectsRegions) && (this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction));
				}
				if (this.canTargetItems)
				{
					if (this.mapObjectTargetsMustBeAutoAttackable && !targ.Thing.def.isAutoAttackableMapObject)
					{
						return false;
					}
					if (this.thingCategory == ThingCategory.None || this.thingCategory == targ.Thing.def.category)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060062F7 RID: 25335 RVA: 0x002262FA File Offset: 0x002244FA
		public static TargetingParameters ForSelf(Pawn p)
		{
			return new TargetingParameters
			{
				targetSpecificThing = p,
				canTargetPawns = false,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x060062F8 RID: 25336 RVA: 0x00226320 File Offset: 0x00224520
		public static TargetingParameters ForArrest(Pawn arrester)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					if (!targ.HasThing)
					{
						return false;
					}
					Pawn pawn = targ.Thing as Pawn;
					return pawn != null && pawn != arrester && pawn.CanBeArrestedBy(arrester) && !pawn.Downed;
				}
			};
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x00226368 File Offset: 0x00224568
		public static TargetingParameters ForAttackHostile()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = true;
			targetingParameters.validator = delegate(TargetInfo targ)
			{
				if (!targ.HasThing)
				{
					return false;
				}
				if (targ.Thing.HostileTo(Faction.OfPlayer))
				{
					return true;
				}
				Pawn pawn = targ.Thing as Pawn;
				return pawn != null && pawn.NonHumanlikeOrWildMan();
			};
			return targetingParameters;
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x002263BB File Offset: 0x002245BB
		public static TargetingParameters ForAttackAny()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = true
			};
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x002263DE File Offset: 0x002245DE
		public static TargetingParameters ForRescue(Pawn p)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				onlyTargetIncapacitatedPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x00226404 File Offset: 0x00224604
		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = ((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

		// Token: 0x060062FD RID: 25341 RVA: 0x00226450 File Offset: 0x00224650
		public static TargetingParameters ForTrade()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				ITrader trader = x.Thing as ITrader;
				return trader != null && trader.CanTradeNow;
			};
			return targetingParameters;
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x0022649C File Offset: 0x0022469C
		public static TargetingParameters ForDropPodsDestination()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = ((TargetInfo x) => DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true, true));
			return targetingParameters;
		}

		// Token: 0x060062FF RID: 25343 RVA: 0x00226500 File Offset: 0x00224700
		public static TargetingParameters ForQuestPawnsWhoWillJoinColony(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				Pawn pawn = x.Thing as Pawn;
				return pawn != null && !pawn.Dead && pawn.mindState.WillJoinColonyIfRescued;
			};
			return targetingParameters;
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x0022654C File Offset: 0x0022474C
		public static TargetingParameters ForOpen(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				IOpenable openable = x.Thing as IOpenable;
				return openable != null && openable.CanOpen;
			};
			return targetingParameters;
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x00226598 File Offset: 0x00224798
		public static TargetingParameters ForShuttle(Pawn hauler)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					if (!targ.HasThing)
					{
						return false;
					}
					Pawn pawn = targ.Thing as Pawn;
					if (pawn == null || pawn.Dead || pawn == hauler)
					{
						return false;
					}
					if (pawn.Downed)
					{
						return true;
					}
					if (pawn.IsPrisonerOfColony)
					{
						return pawn.guest.PrisonerIsSecure;
					}
					return pawn.AnimalOrWildMan();
				}
			};
		}

		// Token: 0x04003C1D RID: 15389
		public bool canTargetLocations;

		// Token: 0x04003C1E RID: 15390
		public bool canTargetSelf;

		// Token: 0x04003C1F RID: 15391
		public bool canTargetPawns = true;

		// Token: 0x04003C20 RID: 15392
		public bool canTargetFires;

		// Token: 0x04003C21 RID: 15393
		public bool canTargetBuildings = true;

		// Token: 0x04003C22 RID: 15394
		public bool canTargetItems;

		// Token: 0x04003C23 RID: 15395
		public bool canTargetAnimals = true;

		// Token: 0x04003C24 RID: 15396
		public bool canTargetHumans = true;

		// Token: 0x04003C25 RID: 15397
		public bool canTargetMechs = true;

		// Token: 0x04003C26 RID: 15398
		public List<Faction> onlyTargetFactions;

		// Token: 0x04003C27 RID: 15399
		public Predicate<TargetInfo> validator;

		// Token: 0x04003C28 RID: 15400
		public bool onlyTargetFlammables;

		// Token: 0x04003C29 RID: 15401
		public Thing targetSpecificThing;

		// Token: 0x04003C2A RID: 15402
		public bool mustBeSelectable;

		// Token: 0x04003C2B RID: 15403
		public bool neverTargetDoors;

		// Token: 0x04003C2C RID: 15404
		public bool neverTargetIncapacitated;

		// Token: 0x04003C2D RID: 15405
		public bool onlyTargetThingsAffectingRegions;

		// Token: 0x04003C2E RID: 15406
		public bool onlyTargetDamagedThings;

		// Token: 0x04003C2F RID: 15407
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		// Token: 0x04003C30 RID: 15408
		public bool onlyTargetIncapacitatedPawns;

		// Token: 0x04003C31 RID: 15409
		public bool onlyTargetControlledPawns;

		// Token: 0x04003C32 RID: 15410
		public bool onlyTargetColonists;

		// Token: 0x04003C33 RID: 15411
		public ThingCategory thingCategory;
	}
}
