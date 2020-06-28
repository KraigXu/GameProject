using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD8 RID: 3288
	public class ThingSetMaker_TraderStock : ThingSetMaker
	{
		// Token: 0x06004FAD RID: 20397 RVA: 0x001AD900 File Offset: 0x001ABB00
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement<TraderKindDef>();
			Faction makingFaction = parms.makingFaction;
			int forTile;
			if (parms.tile != null)
			{
				forTile = parms.tile.Value;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				forTile = Find.AnyPlayerHomeMap.Tile;
			}
			else if (Find.CurrentMap != null)
			{
				forTile = Find.CurrentMap.Tile;
			}
			else
			{
				forTile = -1;
			}
			for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
			{
				foreach (Thing thing in traderKindDef.stockGenerators[i].GenerateThings(forTile, parms.makingFaction))
				{
					if (!thing.def.tradeability.TraderCanSell())
					{
						Log.Error(string.Concat(new object[]
						{
							traderKindDef,
							" generated carrying ",
							thing,
							" which can't be sold by traders. Ignoring..."
						}), false);
					}
					else
					{
						thing.PostGeneratedForTrader(traderKindDef, forTile, makingFaction);
						outThings.Add(thing);
					}
				}
			}
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x001ADA30 File Offset: 0x001ABC30
		public float DebugAverageTotalStockValue(TraderKindDef td)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = td;
			parms.tile = new int?(-1);
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				foreach (Thing thing in base.Generate(parms))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			return num / 50f;
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x001ADAC8 File Offset: 0x001ABCC8
		public string DebugGenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.DebugAverageTotalStockValue(td).ToString("F0"));
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = td;
			parms.tile = new int?(-1);
			(from x in Find.FactionManager.AllFactionsListForReading
			where x.def.baseTraderKinds.Contains(td) || x.def.visitorTraderKinds.Contains(td) || x.def.caravanTraderKinds.Contains(td)
			select x).TryRandomElement(out parms.makingFaction);
			stringBuilder.AppendLine("Example generated stock:\n\n");
			foreach (Thing thing in base.Generate(parms))
			{
				MinifiedThing minifiedThing = thing as MinifiedThing;
				Thing thing2;
				if (minifiedThing != null)
				{
					thing2 = minifiedThing.InnerThing;
				}
				else
				{
					thing2 = thing;
				}
				string text = thing2.LabelCap;
				text = text + " [" + (thing2.MarketValue * (float)thing2.stackCount).ToString("F0") + "]";
				stringBuilder.AppendLine(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x001ADC20 File Offset: 0x001ABE20
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			if (parms.traderDef == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < parms.traderDef.stockGenerators.Count; i = num + 1)
			{
				StockGenerator stock = parms.traderDef.stockGenerators[i];
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate= x => x.tradeability.TraderCanSell() && stock.HandlesThingDef(x);

				foreach (ThingDef thingDef in allDefs.Where(predicate))
				{
					yield return thingDef;
				}
				IEnumerator<ThingDef> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}
	}
}
