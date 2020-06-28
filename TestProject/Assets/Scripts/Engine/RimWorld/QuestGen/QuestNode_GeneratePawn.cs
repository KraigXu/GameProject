using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001117 RID: 4375
	public class QuestNode_GeneratePawn : QuestNode
	{
		// Token: 0x06006676 RID: 26230 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006677 RID: 26231 RVA: 0x0023DF70 File Offset: 0x0023C170
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

		// Token: 0x04003E87 RID: 16007
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003E88 RID: 16008
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04003E89 RID: 16009
		[NoTranslate]
		public SlateRef<IEnumerable<string>> addToLists;

		// Token: 0x04003E8A RID: 16010
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x04003E8B RID: 16011
		public SlateRef<Faction> faction;

		// Token: 0x04003E8C RID: 16012
		public SlateRef<bool> ensureNonNumericName;

		// Token: 0x04003E8D RID: 16013
		public SlateRef<IEnumerable<TraitDef>> forcedTraits;

		// Token: 0x04003E8E RID: 16014
		public SlateRef<Pawn> extraPawnForExtraRelationChance;

		// Token: 0x04003E8F RID: 16015
		public SlateRef<float> relationWithExtraPawnChanceFactor;

		// Token: 0x04003E90 RID: 16016
		public SlateRef<bool?> allowAddictions;

		// Token: 0x04003E91 RID: 16017
		public SlateRef<float> biocodeWeaponChance;

		// Token: 0x04003E92 RID: 16018
		public SlateRef<float> biocodeApparelChance;

		// Token: 0x04003E93 RID: 16019
		public SlateRef<bool> mustBeCapableOfViolence;

		// Token: 0x04003E94 RID: 16020
		private const int MinExpertSkill = 11;
	}
}
