using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010CA RID: 4298
	public class SymbolResolver_FillWithThings : SymbolResolver
	{
		// Token: 0x06006562 RID: 25954 RVA: 0x00237170 File Offset: 0x00235370
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingToSpawn != null)
			{
				return false;
			}
			if (rp.singleThingDef != null)
			{
				Rot4 rot = rp.thingRot ?? Rot4.North;
				IntVec3 zero = IntVec3.Zero;
				IntVec2 size = rp.singleThingDef.size;
				GenAdj.AdjustForRotation(ref zero, ref size, rot);
				if (rp.rect.Width < size.x || rp.rect.Height < size.z)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x00237200 File Offset: 0x00235400
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef;
			if ((thingDef = rp.singleThingDef) == null)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsWeapon || x.IsMedicine || x.IsDrug
				select x).RandomElement<ThingDef>();
			}
			ThingDef thingDef2 = thingDef;
			Rot4 rot = rp.thingRot ?? Rot4.North;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef2.size;
			int num = rp.fillWithThingsPadding ?? 0;
			if (num < 0)
			{
				num = 0;
			}
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			if (size.x <= 0 || size.z <= 0)
			{
				Log.Error("Thing has 0 size.", false);
				return;
			}
			for (int i = rp.rect.minX; i <= rp.rect.maxX - size.x + 1; i += size.x + num)
			{
				for (int j = rp.rect.minZ; j <= rp.rect.maxZ - size.z + 1; j += size.z + num)
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = new CellRect(i, j, size.x, size.z);
					resolveParams.singleThingDef = thingDef2;
					resolveParams.thingRot = new Rot4?(rot);
					BaseGen.symbolStack.Push("thing", resolveParams, null);
				}
			}
			BaseGen.symbolStack.Push("clear", rp, null);
		}
	}
}
