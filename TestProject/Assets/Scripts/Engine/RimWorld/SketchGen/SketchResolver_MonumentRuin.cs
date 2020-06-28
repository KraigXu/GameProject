using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001091 RID: 4241
	public class SketchResolver_MonumentRuin : SketchResolver
	{
		// Token: 0x06006495 RID: 25749 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x06006496 RID: 25750 RVA: 0x0022FB48 File Offset: 0x0022DD48
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
