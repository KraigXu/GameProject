using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001093 RID: 4243
	public static class BaseGen
	{
		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x0600649E RID: 25758 RVA: 0x0022FFD0 File Offset: 0x0022E1D0
		public static string CurrentSymbolPath
		{
			get
			{
				return BaseGen.currentSymbolPath;
			}
		}

		// Token: 0x0600649F RID: 25759 RVA: 0x0022FFD8 File Offset: 0x0022E1D8
		public static void Reset()
		{
			BaseGen.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list;
				if (!BaseGen.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGen.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		// Token: 0x060064A0 RID: 25760 RVA: 0x0023004C File Offset: 0x0022E24C
		public static void Generate()
		{
			if (BaseGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
				return;
			}
			BaseGen.working = true;
			BaseGen.currentSymbolPath = "";
			try
			{
				if (BaseGen.symbolStack.Empty)
				{
					Log.Warning("Symbol stack is empty.", false);
				}
				else if (BaseGen.globalSettings.map == null)
				{
					Log.Error("Called BaseGen.Resolve() with null map.", false);
				}
				else
				{
					int num = BaseGen.symbolStack.Count - 1;
					int num2 = 0;
					while (!BaseGen.symbolStack.Empty)
					{
						num2++;
						if (num2 > 100000)
						{
							Log.Error("Error in BaseGen: Too many iterations. Infinite loop?", false);
							break;
						}
						SymbolStack.Element element = BaseGen.symbolStack.Pop();
						BaseGen.currentSymbolPath = element.symbolPath;
						if (BaseGen.symbolStack.Count == num)
						{
							BaseGen.globalSettings.mainRect = element.resolveParams.rect;
							num--;
						}
						try
						{
							BaseGen.Resolve(element);
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Error while resolving symbol \"",
								element.symbol,
								"\" with params=",
								element.resolveParams,
								"\n\nException: ",
								ex
							}), false);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Error in BaseGen: " + arg, false);
			}
			finally
			{
				BaseGen.working = false;
				BaseGen.symbolStack.Clear();
				BaseGen.globalSettings.Clear();
			}
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x00230200 File Offset: 0x0022E400
		private static void Resolve(SymbolStack.Element toResolve)
		{
			string symbol = toResolve.symbol;
			ResolveParams resolveParams = toResolve.resolveParams;
			BaseGen.tmpResolvers.Clear();
			List<RuleDef> list;
			if (BaseGen.rulesBySymbol.TryGetValue(symbol, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					RuleDef ruleDef = list[i];
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						SymbolResolver symbolResolver = ruleDef.resolvers[j];
						if (symbolResolver.CanResolve(resolveParams))
						{
							BaseGen.tmpResolvers.Add(symbolResolver);
						}
					}
				}
			}
			if (!BaseGen.tmpResolvers.Any<SymbolResolver>())
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not find any RuleDef for symbol \"",
					symbol,
					"\" with any resolver that could resolve ",
					resolveParams
				}), false);
				return;
			}
			BaseGen.tmpResolvers.RandomElementByWeight((SymbolResolver x) => x.selectionWeight).Resolve(resolveParams);
		}

		// Token: 0x04003D3D RID: 15677
		public static GlobalSettings globalSettings = new GlobalSettings();

		// Token: 0x04003D3E RID: 15678
		public static SymbolStack symbolStack = new SymbolStack();

		// Token: 0x04003D3F RID: 15679
		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		// Token: 0x04003D40 RID: 15680
		private static bool working;

		// Token: 0x04003D41 RID: 15681
		private static string currentSymbolPath;

		// Token: 0x04003D42 RID: 15682
		private const int MaxResolvedSymbols = 100000;

		// Token: 0x04003D43 RID: 15683
		private static List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();
	}
}
