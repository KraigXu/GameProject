using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class TargetingParameters
	{
		
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

		
		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = ((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

		
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

		
		public bool canTargetLocations;

		
		public bool canTargetSelf;

		
		public bool canTargetPawns = true;

		
		public bool canTargetFires;

		
		public bool canTargetBuildings = true;

		
		public bool canTargetItems;

		
		public bool canTargetAnimals = true;

		
		public bool canTargetHumans = true;

		
		public bool canTargetMechs = true;

		
		public List<Faction> onlyTargetFactions;

		
		public Predicate<TargetInfo> validator;

		
		public bool onlyTargetFlammables;

		
		public Thing targetSpecificThing;

		
		public bool mustBeSelectable;

		
		public bool neverTargetDoors;

		
		public bool neverTargetIncapacitated;

		
		public bool onlyTargetThingsAffectingRegions;

		
		public bool onlyTargetDamagedThings;

		
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		
		public bool onlyTargetIncapacitatedPawns;

		
		public bool onlyTargetControlledPawns;

		
		public bool onlyTargetColonists;

		
		public ThingCategory thingCategory;
	}
}
