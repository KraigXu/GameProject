using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GeneratePawn : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnKindDef value = this.kindDef.GetValue(slate);
			Faction value2 = this.faction.GetValue(slate);
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			int tile = -1;
			bool forceGenerateNewPawn = false;
			bool newborn = false;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			bool flag = this.allowAddictions.GetValue(slate) ?? true;
			IEnumerable<TraitDef> value3 = this.forcedTraits.GetValue(slate);
			float value4 = this.biocodeWeaponChance.GetValue(slate);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(value, value2, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, this.mustBeCapableOfViolence.GetValue(slate), 1f, false, true, true, flag, false, false, false, false, value4, this.extraPawnForExtraRelationChance.GetValue(slate), this.relationWithExtraPawnChanceFactor.GetValue(slate), null, null, value3, null, null, null, null, null, null, null, null, null)
			{
				BiocodeApparelChance = this.biocodeApparelChance.GetValue(slate)
			});
			if (this.ensureNonNumericName.GetValue(slate) && (pawn.Name == null || pawn.Name.Numerical))
			{
				pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, null);
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), pawn);
			}
			if (this.addToLists.GetValue(slate) != null)
			{
				foreach (string name in this.addToLists.GetValue(slate))
				{
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, name, pawn);
				}
			}
			QuestGen.AddToGeneratedPawns(pawn);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		[NoTranslate]
		public SlateRef<string> addToList;

		
		[NoTranslate]
		public SlateRef<IEnumerable<string>> addToLists;

		
		public SlateRef<PawnKindDef> kindDef;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<bool> ensureNonNumericName;

		
		public SlateRef<IEnumerable<TraitDef>> forcedTraits;

		
		public SlateRef<Pawn> extraPawnForExtraRelationChance;

		
		public SlateRef<float> relationWithExtraPawnChanceFactor;

		
		public SlateRef<bool?> allowAddictions;

		
		public SlateRef<float> biocodeWeaponChance;

		
		public SlateRef<float> biocodeApparelChance;

		
		public SlateRef<bool> mustBeCapableOfViolence;

		
		private const int MinExpertSkill = 11;
	}
}
