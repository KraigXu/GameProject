using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			resolveParams.chanceToSkipWallBlock = new float?(rp.chanceToSkipWallBlock ?? 0.1f);
			resolveParams.clearEdificeOnly = new bool?(rp.clearEdificeOnly ?? true);
			resolveParams.noRoof = new bool?(rp.noRoof ?? true);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams, null);
		}
	}
}
