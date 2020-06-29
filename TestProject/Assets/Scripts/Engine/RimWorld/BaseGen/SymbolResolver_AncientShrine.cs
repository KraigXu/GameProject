﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_AncientShrine : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 bottomLeft = rp.rect.BottomLeft;
			Map map = BaseGen.globalSettings.map;
			CellRect rect = new CellRect(bottomLeft.x + rp.rect.Width / 2 - 1, bottomLeft.z + rp.rect.Height / 2, 2, 1);
			foreach (IntVec3 c in rect)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return;
					}
				}
			}
			if (Rand.Chance(this.techprintChance))
			{
				ResolveParams resolveParams = rp;
				if ((from t in DefDatabase<ThingDef>.AllDefsListForReading
				where t.tradeTags != null && t.tradeTags.Contains("Techprint")
				select t).TryRandomElement(out resolveParams.singleThingDef))
				{
					BaseGen.symbolStack.Push("thing", resolveParams, null);
				}
			}
			if (ModsConfig.RoyaltyActive && Rand.Chance(this.bladelinkChance))
			{
				ResolveParams resolveParams2 = rp;
				if ((from t in DefDatabase<ThingDef>.AllDefsListForReading
				where t.weaponTags != null && t.weaponTags.Contains("Bladelink")
				select t).TryRandomElement(out resolveParams2.singleThingDef))
				{
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
				}
			}
			if (ModsConfig.RoyaltyActive && Rand.Chance(this.psychicChance))
			{
				ResolveParams resolveParams3 = rp;
				if ((from t in DefDatabase<ThingDef>.AllDefsListForReading
				where t.tradeTags != null && t.tradeTags.Contains("Psychic")
				select t).TryRandomElement(out resolveParams3.singleThingDef))
				{
					BaseGen.symbolStack.Push("thing", resolveParams3, null);
				}
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = CellRect.SingleCell(rect.BottomLeft);
			resolveParams4.thingRot = new Rot4?(Rot4.East);
			BaseGen.symbolStack.Push("ancientCryptosleepCasket", resolveParams4, null);
			ResolveParams resolveParams5 = rp;
			resolveParams5.rect = rect;
			resolveParams5.floorDef = TerrainDefOf.Concrete;
			BaseGen.symbolStack.Push("floor", resolveParams5, null);
			ResolveParams resolveParams6 = rp;
			resolveParams6.floorDef = (rp.floorDef ?? TerrainDefOf.MetalTile);
			BaseGen.symbolStack.Push("floor", resolveParams6, null);
		}

		
		public float techprintChance;

		
		public float bladelinkChance;

		
		public float psychicChance;
	}
}
