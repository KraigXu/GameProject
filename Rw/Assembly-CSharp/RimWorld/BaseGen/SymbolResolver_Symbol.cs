using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010BD RID: 4285
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x06006541 RID: 25921 RVA: 0x002359B8 File Offset: 0x00233BB8
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != this.symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x00235A2F File Offset: 0x00233C2F
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp, null);
		}

		// Token: 0x04003DB6 RID: 15798
		public string symbol;
	}
}
