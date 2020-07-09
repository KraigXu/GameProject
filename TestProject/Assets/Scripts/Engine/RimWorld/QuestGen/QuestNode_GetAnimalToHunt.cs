using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetAnimalToHunt : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
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

		
		private float AnimalCommonalityByDifficulty(PawnKindDef animalKind, float animalDifficultyFromPoints)
		{
			float num = Mathf.Abs(animalKind.GetAnimalPointsToHuntOrSlaughter() - animalDifficultyFromPoints);
			return 1f / num;
		}

		
		[NoTranslate]
		public SlateRef<string> storeAnimalToHuntAs;

		
		[NoTranslate]
		public SlateRef<string> storeCountToHuntAs;

		
		public SlateRef<SimpleCurve> pointsToAnimalsToHuntCountCurve;

		
		public SlateRef<SimpleCurve> pointsToAnimalDifficultyCurve;

		
		public SlateRef<FloatRange?> animalsToHuntCountRandomFactorRange;
	}
}
