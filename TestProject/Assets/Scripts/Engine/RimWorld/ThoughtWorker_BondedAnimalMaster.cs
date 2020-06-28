using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000808 RID: 2056
	public class ThoughtWorker_BondedAnimalMaster : ThoughtWorker
	{
		// Token: 0x06003418 RID: 13336 RVA: 0x0011EB80 File Offset: 0x0011CD80
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			if (!ThoughtWorker_BondedAnimalMaster.tmpAnimals.Any<string>())
			{
				return false;
			}
			if (ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count == 1)
			{
				return ThoughtState.ActiveAtStage(0, ThoughtWorker_BondedAnimalMaster.tmpAnimals[0]);
			}
			return ThoughtState.ActiveAtStage(1, ThoughtWorker_BondedAnimalMaster.tmpAnimals.ToCommaList(true));
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x0011EBE6 File Offset: 0x0011CDE6
		protected virtual bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster == p;
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x0011EBF8 File Offset: 0x0011CDF8
		public void GetAnimals(Pawn p, List<string> outAnimals)
		{
			outAnimals.Clear();
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				Pawn otherPawn = directPawnRelation.otherPawn;
				if (directPawnRelation.def == PawnRelationDefOf.Bond && !otherPawn.Dead && otherPawn.Spawned && otherPawn.Faction == Faction.OfPlayer && otherPawn.training.HasLearned(TrainableDefOf.Obedience) && p.skills.GetSkill(SkillDefOf.Animals).Level >= TrainableUtility.MinimumHandlingSkill(otherPawn) && this.AnimalMasterCheck(p, otherPawn))
				{
					outAnimals.Add(otherPawn.LabelShort);
				}
			}
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x0011ECAA File Offset: 0x0011CEAA
		public int GetAnimalsCount(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			return ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count;
		}

		// Token: 0x04001BB2 RID: 7090
		private static List<string> tmpAnimals = new List<string>();
	}
}
