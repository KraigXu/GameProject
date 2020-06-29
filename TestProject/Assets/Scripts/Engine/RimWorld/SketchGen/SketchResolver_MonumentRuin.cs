using System;
using Verse;

namespace RimWorld.SketchGen
{
	
	public class SketchResolver_MonumentRuin : SketchResolver
	{
		
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		
		protected override void ResolveInt(ResolveParams parms)
		{
			ResolveParams resolveParams = parms;
			resolveParams.allowWood = new bool?(parms.allowWood ?? false);
			if (resolveParams.allowedMonumentThings == null)
			{
				resolveParams.allowedMonumentThings = new ThingFilter();
				resolveParams.allowedMonumentThings.SetAllowAll(null, true);
			}
			if (ModsConfig.RoyaltyActive)
			{
				resolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
			}
			SketchResolverDefOf.Monument.Resolve(resolveParams);
			SketchResolverDefOf.DamageBuildings.Resolve(parms);
		}
	}
}
