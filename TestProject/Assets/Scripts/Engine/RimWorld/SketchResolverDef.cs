using System;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	
	public class SketchResolverDef : Def
	{
		
		public void Resolve(ResolveParams parms)
		{
			this.resolver.Resolve(parms);
		}

		
		public bool CanResolve(ResolveParams parms)
		{
			return this.resolver.CanResolve(parms);
		}

		
		public SketchResolver resolver;

		
		public bool isRoot;
	}
}
