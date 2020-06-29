using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Interior_AncientTemple : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientTempleContents.root.Generate();
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingToSpawn = list[i];
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
			if (!Find.Storyteller.difficulty.peacefulTemples)
			{
				if (Rand.Chance(0.65f))
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.mechanoidsCount = new int?(rp.mechanoidsCount ?? SymbolResolver_Interior_AncientTemple.MechanoidCountRange.RandomInRange);
					BaseGen.symbolStack.Push("randomMechanoidGroup", resolveParams2, null);
				}
				else
				{
					ResolveParams resolveParams3 = rp;
					resolveParams3.hivesCount = new int?(rp.hivesCount ?? SymbolResolver_Interior_AncientTemple.HivesCountRange.RandomInRange);
					BaseGen.symbolStack.Push("hives", resolveParams3, null);
				}
			}
			if (rp.rect.Width >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.x && rp.rect.Height >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.z)
			{
				BaseGen.symbolStack.Push("ancientShrinesGroup", rp, null);
			}
		}

		
		private const float MechanoidsChance = 0.65f;

		
		private static readonly IntRange MechanoidCountRange = new IntRange(1, 5);

		
		private static readonly IntRange HivesCountRange = new IntRange(1, 2);

		
		private static readonly IntVec2 MinSizeForShrines = new IntVec2(4, 3);
	}
}
