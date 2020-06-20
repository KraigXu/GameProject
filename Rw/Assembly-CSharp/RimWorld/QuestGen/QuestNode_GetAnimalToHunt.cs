using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200111E RID: 4382
	public class QuestNode_GetAnimalToHunt : QuestNode
	{
		// Token: 0x0600668D RID: 26253 RVA: 0x0023E969 File Offset: 0x0023CB69
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x0023E972 File Offset: 0x0023CB72
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x0023E980 File Offset: 0x0023CB80
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x2 = slate.Get<float>("points", 0f, false);
			float animalDifficultyFromPoints = this.pointsToAnimalDifficultyCurve.GetValue(slate).Evaluate(x2);
			PawnKindDef pawnKindDef;
			if (!map.Biome.AllWildAnimals.Where(delegate(PawnKindDef x)
			{
				if (map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race))
				{
					return map.listerThings.ThingsOfDef(x.race).Any((Thing p) => p.Faction == null);
				}
				return false;
			}).TryRandomElementByWeight((PawnKindDef x) => this.AnimalCommonalityByDifficulty(x, animalDifficultyFromPoints), out pawnKindDef))
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
				if (pawn.def == pawnKindDef.race && !pawn.IsQuestLodger() && pawn.Faction == null)
				{
					num++;
				}
			}
			SimpleCurve value = this.pointsToAnimalsToHuntCountCurve.GetValue(slate);
			float randomInRange = (this.animalsToHuntCountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			int num2 = Mathf.RoundToInt(value.Evaluate(x2) * randomInRange);
			num2 = Mathf.Min(num2, num);
			num2 = Mathf.Max(num2, 1);
			slate.Set<ThingDef>(this.storeAnimalToHuntAs.GetValue(slate), pawnKindDef.race, false);
			slate.Set<int>(this.storeCountToHuntAs.GetValue(slate), num2, false);
			return true;
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x0023EB0C File Offset: 0x0023CD0C
		private float AnimalCommonalityByDifficulty(PawnKindDef animalKind, float animalDifficultyFromPoints)
		{
			float num = Mathf.Abs(animalKind.GetAnimalPointsToHuntOrSlaughter() - animalDifficultyFromPoints);
			return 1f / num;
		}

		// Token: 0x04003EB4 RID: 16052
		[NoTranslate]
		public SlateRef<string> storeAnimalToHuntAs;

		// Token: 0x04003EB5 RID: 16053
		[NoTranslate]
		public SlateRef<string> storeCountToHuntAs;

		// Token: 0x04003EB6 RID: 16054
		public SlateRef<SimpleCurve> pointsToAnimalsToHuntCountCurve;

		// Token: 0x04003EB7 RID: 16055
		public SlateRef<SimpleCurve> pointsToAnimalDifficultyCurve;

		// Token: 0x04003EB8 RID: 16056
		public SlateRef<FloatRange?> animalsToHuntCountRandomFactorRange;
	}
}
