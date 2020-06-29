using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("doors", rp, null);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGenCore.symbolStack.Push(this.interior, resolveParams, null);
			}
			BaseGenCore.symbolStack.Push("emptyRoom", rp, null);
		}

		
		public string interior;
	}
}
