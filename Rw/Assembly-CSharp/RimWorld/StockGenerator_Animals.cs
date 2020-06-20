using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAE RID: 3502
	public class StockGenerator_Animals : StockGenerator
	{
		// Token: 0x06005512 RID: 21778 RVA: 0x001C4EB2 File Offset: 0x001C30B2
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			int randomInRange = this.kindCountRange.RandomInRange;
			int count = this.countRange.RandomInRange;
			List<PawnKindDef> kinds = new List<PawnKindDef>();
			Func<PawnKindDef, bool> <>9__0;
			Func<PawnKindDef, float> <>9__1;
			for (int j = 0; j < randomInRange; j++)
			{
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((PawnKindDef k) => !kinds.Contains(k) && this.PawnKindAllowed(k, forTile)));
				}
				IEnumerable<PawnKindDef> source = allDefs.Where(predicate);
				Func<PawnKindDef, float> weightSelector;
				if ((weightSelector = <>9__1) == null)
				{
					weightSelector = (<>9__1 = ((PawnKindDef k) => this.SelectionChance(k)));
				}
				PawnKindDef item;
				if (!source.TryRandomElementByWeight(weightSelector, out item))
				{
					break;
				}
				kinds.Add(item);
			}
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PawnKindDef kind;
				if (!kinds.TryRandomElement(out kind))
				{
					yield break;
				}
				PawnGenerationRequest request = new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
				yield return PawnGenerator.GeneratePawn(request);
				num = i;
			}
			yield break;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x001C4EC9 File Offset: 0x001C30C9
		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x001C4EE0 File Offset: 0x001C30E0
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)));
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x001C4F5C File Offset: 0x001C315C
		private bool PawnKindAllowed(PawnKindDef kind, int forTile)
		{
			if (!kind.RaceProps.Animal || kind.RaceProps.wildness < this.minWildness || kind.RaceProps.wildness > this.maxWildness || kind.RaceProps.wildness > 1f)
			{
				return false;
			}
			if (this.checkTemperature)
			{
				int num = forTile;
				if (num == -1 && Find.AnyPlayerHomeMap != null)
				{
					num = Find.AnyPlayerHomeMap.Tile;
				}
				if (num != -1 && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(num, kind.race))
				{
					return false;
				}
			}
			return kind.race.tradeTags != null && this.tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)) && kind.race.tradeability.TraderCanSell();
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x001C5060 File Offset: 0x001C3260
		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x001C50E4 File Offset: 0x001C32E4
		[DebugOutput]
		private static void StockGenerationAnimals()
		{
			new StockGenerator_Animals
			{
				tradeTagsSell = new List<string>(),
				tradeTagsSell = 
				{
					"AnimalCommon",
					"AnimalUncommon"
				}
			}.LogAnimalChances();
		}

		// Token: 0x04002E98 RID: 11928
		[NoTranslate]
		private List<string> tradeTagsSell = new List<string>();

		// Token: 0x04002E99 RID: 11929
		[NoTranslate]
		private List<string> tradeTagsBuy = new List<string>();

		// Token: 0x04002E9A RID: 11930
		private IntRange kindCountRange = new IntRange(1, 1);

		// Token: 0x04002E9B RID: 11931
		private float minWildness;

		// Token: 0x04002E9C RID: 11932
		private float maxWildness = 1f;

		// Token: 0x04002E9D RID: 11933
		private bool checkTemperature = true;

		// Token: 0x04002E9E RID: 11934
		private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 100f),
				true
			},
			{
				new CurvePoint(0.25f, 60f),
				true
			},
			{
				new CurvePoint(0.5f, 30f),
				true
			},
			{
				new CurvePoint(0.75f, 12f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};
	}
}
