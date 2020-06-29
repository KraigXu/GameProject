using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public struct PawnGenerationRequest
	{
		
		
		
		public PawnKindDef KindDef { get; set; }

		
		
		
		public PawnGenerationContext Context { get; set; }

		
		
		
		public Faction Faction { get; set; }

		
		
		
		public int Tile { get; set; }

		
		
		
		public bool ForceGenerateNewPawn { get; set; }

		
		
		
		public bool Newborn { get; set; }

		
		
		
		public bool AllowDead { get; set; }

		
		
		
		public bool AllowDowned { get; set; }

		
		
		
		public bool CanGeneratePawnRelations { get; set; }

		
		
		
		public bool MustBeCapableOfViolence { get; set; }

		
		
		
		public float ColonistRelationChanceFactor { get; set; }

		
		
		
		public bool ForceAddFreeWarmLayerIfNeeded { get; set; }

		
		
		
		public bool AllowGay { get; set; }

		
		
		
		public bool AllowFood { get; set; }

		
		
		
		public bool AllowAddictions { get; set; }

		
		
		
		public IEnumerable<TraitDef> ForcedTraits { get; set; }

		
		
		
		public IEnumerable<TraitDef> ProhibitedTraits { get; set; }

		
		
		
		public bool Inhabitant { get; set; }

		
		
		
		public bool CertainlyBeenInCryptosleep { get; set; }

		
		
		
		public bool ForceRedressWorldPawnIfFormerColonist { get; set; }

		
		
		
		public bool WorldPawnFactionDoesntMatter { get; set; }

		
		
		
		public float BiocodeWeaponChance { get; set; }

		
		
		
		public float BiocodeApparelChance { get; set; }

		
		
		
		public Pawn ExtraPawnForExtraRelationChance { get; set; }

		
		
		
		public float RelationWithExtraPawnChanceFactor { get; set; }

		
		
		
		public Predicate<Pawn> RedressValidator { get; set; }

		
		
		
		public Predicate<Pawn> ValidatorPreGear { get; set; }

		
		
		
		public Predicate<Pawn> ValidatorPostGear { get; set; }

		
		
		
		public float? MinChanceToRedressWorldPawn { get; set; }

		
		
		
		public float? FixedBiologicalAge { get; set; }

		
		
		
		public float? FixedChronologicalAge { get; set; }

		
		
		
		public Gender? FixedGender { get; set; }

		
		
		
		public float? FixedMelanin { get; set; }

		
		
		
		public string FixedLastName { get; set; }

		
		
		
		public string FixedBirthName { get; set; }

		
		
		
		public RoyalTitleDef FixedTitle { get; set; }

		
		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, int tile = -1, bool forceGenerateNewPawn = false, bool newborn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowFood = true, bool allowAddictions = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0f, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1f, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, float? fixedMelanin = null, string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null)
		{
			this = default(PawnGenerationRequest);
			if (context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'", false);
				context = PawnGenerationContext.NonPlayer;
			}
			if (inhabitant && (tile == -1 || Current.Game.FindMap(tile) == null))
			{
				Log.Error("Trying to generate an inhabitant but map is null.", false);
				inhabitant = false;
			}
			this.KindDef = kind;
			this.Context = context;
			this.Faction = faction;
			this.Tile = tile;
			this.ForceGenerateNewPawn = forceGenerateNewPawn;
			this.Newborn = newborn;
			this.AllowDead = allowDead;
			this.AllowDowned = allowDowned;
			this.CanGeneratePawnRelations = canGeneratePawnRelations;
			this.MustBeCapableOfViolence = mustBeCapableOfViolence;
			this.ColonistRelationChanceFactor = colonistRelationChanceFactor;
			this.ForceAddFreeWarmLayerIfNeeded = forceAddFreeWarmLayerIfNeeded;
			this.AllowGay = allowGay;
			this.AllowFood = allowFood;
			this.AllowAddictions = allowAddictions;
			this.ForcedTraits = forcedTraits;
			this.ProhibitedTraits = prohibitedTraits;
			this.Inhabitant = inhabitant;
			this.CertainlyBeenInCryptosleep = certainlyBeenInCryptosleep;
			this.ForceRedressWorldPawnIfFormerColonist = forceRedressWorldPawnIfFormerColonist;
			this.WorldPawnFactionDoesntMatter = worldPawnFactionDoesntMatter;
			this.ExtraPawnForExtraRelationChance = extraPawnForExtraRelationChance;
			this.RelationWithExtraPawnChanceFactor = relationWithExtraPawnChanceFactor;
			this.BiocodeWeaponChance = biocodeWeaponChance;
			this.ValidatorPreGear = validatorPreGear;
			this.ValidatorPostGear = validatorPostGear;
			this.MinChanceToRedressWorldPawn = minChanceToRedressWorldPawn;
			this.FixedBiologicalAge = fixedBiologicalAge;
			this.FixedChronologicalAge = fixedChronologicalAge;
			this.FixedGender = fixedGender;
			this.FixedMelanin = fixedMelanin;
			this.FixedLastName = fixedLastName;
			this.FixedBirthName = fixedBirthName;
			this.FixedTitle = fixedTitle;
		}

		
		public static PawnGenerationRequest MakeDefault()
		{
			return new PawnGenerationRequest
			{
				Context = PawnGenerationContext.NonPlayer,
				Tile = -1,
				CanGeneratePawnRelations = true,
				ColonistRelationChanceFactor = 1f,
				AllowGay = true,
				AllowFood = true,
				AllowAddictions = true,
				RelationWithExtraPawnChanceFactor = 1f
			};
		}

		
		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".", false);
				return;
			}
			this.FixedLastName = fixedLastName;
		}

		
		public void SetFixedBirthName(string fixedBirthName)
		{
			if (this.FixedBirthName != null)
			{
				Log.Error("birth name is already a fixed value: " + this.FixedBirthName + ".", false);
				return;
			}
			this.FixedBirthName = fixedBirthName;
		}

		
		public void SetFixedMelanin(float fixedMelanin)
		{
			if (this.FixedMelanin != null)
			{
				Log.Error("Melanin is already a fixed value: " + this.FixedMelanin + ".", false);
				return;
			}
			this.FixedMelanin = new float?(fixedMelanin);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"kindDef=",
				this.KindDef,
				", context=",
				this.Context,
				", faction=",
				this.Faction,
				", tile=",
				this.Tile,
				", forceGenerateNewPawn=",
				this.ForceGenerateNewPawn.ToString(),
				", newborn=",
				this.Newborn.ToString(),
				", allowDead=",
				this.AllowDead.ToString(),
				", allowDowned=",
				this.AllowDowned.ToString(),
				", canGeneratePawnRelations=",
				this.CanGeneratePawnRelations.ToString(),
				", mustBeCapableOfViolence=",
				this.MustBeCapableOfViolence.ToString(),
				", colonistRelationChanceFactor=",
				this.ColonistRelationChanceFactor,
				", forceAddFreeWarmLayerIfNeeded=",
				this.ForceAddFreeWarmLayerIfNeeded.ToString(),
				", allowGay=",
				this.AllowGay.ToString(),
				", prohibitedTraits=",
				this.ProhibitedTraits,
				", allowFood=",
				this.AllowFood.ToString(),
				", allowAddictions=",
				this.AllowAddictions.ToString(),
				", inhabitant=",
				this.Inhabitant.ToString(),
				", certainlyBeenInCryptosleep=",
				this.CertainlyBeenInCryptosleep.ToString(),
				", biocodeWeaponChance=",
				this.BiocodeWeaponChance,
				", validatorPreGear=",
				this.ValidatorPreGear,
				", validatorPostGear=",
				this.ValidatorPostGear,
				", fixedBiologicalAge=",
				this.FixedBiologicalAge,
				", fixedChronologicalAge=",
				this.FixedChronologicalAge,
				", fixedGender=",
				this.FixedGender,
				", fixedMelanin=",
				this.FixedMelanin,
				", fixedLastName=",
				this.FixedLastName,
				", fixedBirthName=",
				this.FixedBirthName
			});
		}
	}
}
