using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public struct PawnGenerationRequest
	{
		
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0006B2D3 File Offset: 0x000694D3
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x0006B2DB File Offset: 0x000694DB
		public PawnKindDef KindDef { get; set; }

		
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x0006B2E4 File Offset: 0x000694E4
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x0006B2EC File Offset: 0x000694EC
		public PawnGenerationContext Context { get; set; }

		
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x0006B2F5 File Offset: 0x000694F5
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x0006B2FD File Offset: 0x000694FD
		public Faction Faction { get; set; }

		
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x0006B306 File Offset: 0x00069506
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x0006B30E File Offset: 0x0006950E
		public int Tile { get; set; }

		
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x0006B317 File Offset: 0x00069517
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x0006B31F File Offset: 0x0006951F
		public bool ForceGenerateNewPawn { get; set; }

		
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0006B328 File Offset: 0x00069528
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x0006B330 File Offset: 0x00069530
		public bool Newborn { get; set; }

		
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0006B339 File Offset: 0x00069539
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x0006B341 File Offset: 0x00069541
		public bool AllowDead { get; set; }

		
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0006B34A File Offset: 0x0006954A
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x0006B352 File Offset: 0x00069552
		public bool AllowDowned { get; set; }

		
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x0006B35B File Offset: 0x0006955B
		// (set) Token: 0x0600127D RID: 4733 RVA: 0x0006B363 File Offset: 0x00069563
		public bool CanGeneratePawnRelations { get; set; }

		
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x0006B36C File Offset: 0x0006956C
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x0006B374 File Offset: 0x00069574
		public bool MustBeCapableOfViolence { get; set; }

		
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0006B37D File Offset: 0x0006957D
		// (set) Token: 0x06001281 RID: 4737 RVA: 0x0006B385 File Offset: 0x00069585
		public float ColonistRelationChanceFactor { get; set; }

		
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0006B38E File Offset: 0x0006958E
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x0006B396 File Offset: 0x00069596
		public bool ForceAddFreeWarmLayerIfNeeded { get; set; }

		
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0006B39F File Offset: 0x0006959F
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0006B3A7 File Offset: 0x000695A7
		public bool AllowGay { get; set; }

		
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0006B3B0 File Offset: 0x000695B0
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x0006B3B8 File Offset: 0x000695B8
		public bool AllowFood { get; set; }

		
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0006B3C1 File Offset: 0x000695C1
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0006B3C9 File Offset: 0x000695C9
		public bool AllowAddictions { get; set; }

		
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0006B3D2 File Offset: 0x000695D2
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x0006B3DA File Offset: 0x000695DA
		public IEnumerable<TraitDef> ForcedTraits { get; set; }

		
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0006B3E3 File Offset: 0x000695E3
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x0006B3EB File Offset: 0x000695EB
		public IEnumerable<TraitDef> ProhibitedTraits { get; set; }

		
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0006B3F4 File Offset: 0x000695F4
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x0006B3FC File Offset: 0x000695FC
		public bool Inhabitant { get; set; }

		
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0006B405 File Offset: 0x00069605
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x0006B40D File Offset: 0x0006960D
		public bool CertainlyBeenInCryptosleep { get; set; }

		
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x0006B416 File Offset: 0x00069616
		// (set) Token: 0x06001293 RID: 4755 RVA: 0x0006B41E File Offset: 0x0006961E
		public bool ForceRedressWorldPawnIfFormerColonist { get; set; }

		
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0006B427 File Offset: 0x00069627
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x0006B42F File Offset: 0x0006962F
		public bool WorldPawnFactionDoesntMatter { get; set; }

		
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x0006B438 File Offset: 0x00069638
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x0006B440 File Offset: 0x00069640
		public float BiocodeWeaponChance { get; set; }

		
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0006B449 File Offset: 0x00069649
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x0006B451 File Offset: 0x00069651
		public float BiocodeApparelChance { get; set; }

		
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0006B45A File Offset: 0x0006965A
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x0006B462 File Offset: 0x00069662
		public Pawn ExtraPawnForExtraRelationChance { get; set; }

		
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0006B46B File Offset: 0x0006966B
		// (set) Token: 0x0600129D RID: 4765 RVA: 0x0006B473 File Offset: 0x00069673
		public float RelationWithExtraPawnChanceFactor { get; set; }

		
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0006B47C File Offset: 0x0006967C
		// (set) Token: 0x0600129F RID: 4767 RVA: 0x0006B484 File Offset: 0x00069684
		public Predicate<Pawn> RedressValidator { get; set; }

		
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0006B48D File Offset: 0x0006968D
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x0006B495 File Offset: 0x00069695
		public Predicate<Pawn> ValidatorPreGear { get; set; }

		
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0006B49E File Offset: 0x0006969E
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x0006B4A6 File Offset: 0x000696A6
		public Predicate<Pawn> ValidatorPostGear { get; set; }

		
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0006B4AF File Offset: 0x000696AF
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0006B4B7 File Offset: 0x000696B7
		public float? MinChanceToRedressWorldPawn { get; set; }

		
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0006B4C0 File Offset: 0x000696C0
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x0006B4C8 File Offset: 0x000696C8
		public float? FixedBiologicalAge { get; set; }

		
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0006B4D1 File Offset: 0x000696D1
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x0006B4D9 File Offset: 0x000696D9
		public float? FixedChronologicalAge { get; set; }

		
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0006B4E2 File Offset: 0x000696E2
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x0006B4EA File Offset: 0x000696EA
		public Gender? FixedGender { get; set; }

		
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0006B4F3 File Offset: 0x000696F3
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x0006B4FB File Offset: 0x000696FB
		public float? FixedMelanin { get; set; }

		
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0006B504 File Offset: 0x00069704
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x0006B50C File Offset: 0x0006970C
		public string FixedLastName { get; set; }

		
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x0006B515 File Offset: 0x00069715
		// (set) Token: 0x060012B1 RID: 4785 RVA: 0x0006B51D File Offset: 0x0006971D
		public string FixedBirthName { get; set; }

		
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0006B526 File Offset: 0x00069726
		// (set) Token: 0x060012B3 RID: 4787 RVA: 0x0006B52E File Offset: 0x0006972E
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
