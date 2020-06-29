using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	
	public static class BaseGenCore
	{
		
		
		public static string CurrentSymbolPath
		{
			get
			{
				return BaseGenCore.currentSymbolPath;
			}
		}

		
		public static void Reset()
		{
			BaseGenCore.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list;
				if (!BaseGenCore.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGenCore.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		
		public static void Generate()
		{
			if (BaseGenCore.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
				return;
			}
			BaseGenCore.working = true;
			BaseGenCore.currentSymbolPath = "";
			try
			{
				if (BaseGenCore.symbolStack.Empty)
				{
					Log.Warning("Symbol stack is empty.", false);
				}
				else if (BaseGenCore.globalSettings.map == null)
				{
					Log.Error("Called BaseGen.Resolve() with null map.", false);
				}
				else
				{
					int num = BaseGenCore.symbolStack.Count - 1;
					int num2 = 0;
					while (!BaseGenCore.symbolStack.Empty)
					{
						num2++;
						if (num2 > 100000)
						{
							Log.Error("Error in BaseGen: Too many iterations. Infinite loop?", false);
							break;
						}
						SymbolStack.Element element = BaseGenCore.symbolStack.Pop();
						BaseGenCore.currentSymbolPath = element.symbolPath;
						if (BaseGenCore.symbolStack.Count == num)
						{
							BaseGenCore.globalSettings.mainRect = element.resolveParams.rect;
							num--;
						}
						try
						{
							BaseGenCore.Resolve(element);
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
				BaseGenCore.working = false;
				BaseGenCore.symbolStack.Clear();
				BaseGenCore.globalSettings.Clear();
			}
		}

		
		private static void Resolve(SymbolStack.Element toResolve)
		{
			string symbol = toResolve.symbol;
			ResolveParams resolveParams = toResolve.resolveParams;
			BaseGenCore.tmpResolvers.Clear();
			List<RuleDef> list;
			if (BaseGenCore.rulesBySymbol.TryGetValue(symbol, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					RuleDef ruleDef = list[i];
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						SymbolResolver symbolResolver = ruleDef.resolvers[j];
						if (symbolResolver.CanResolve(resolveParams))
						{
							BaseGenCore.tmpResolvers.Add(symbolResolver);
						}
					}
				}
			}
			if (!BaseGenCore.tmpResolvers.Any<SymbolResolver>())
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
			BaseGenCore.tmpResolvers.RandomElementByWeight((SymbolResolver x) => x.selectionWeight).Resolve(resolveParams);
		}

		
		public static GlobalSettings globalSettings = new GlobalSettings();

		
		public static SymbolStack symbolStack = new SymbolStack();

		
		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		
		private static bool working;

		
		private static string currentSymbolPath;

		
		private const int MaxResolvedSymbols = 100000;

		
		private static List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();
	}
}
