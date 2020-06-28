using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010C6 RID: 4294
	public class SymbolResolver_EdgeMannedMortar : SymbolResolver
	{
		// Token: 0x06006557 RID: 25943 RVA: 0x00236B50 File Offset: 0x00234D50
		public override bool CanResolve(ResolveParams rp)
		{
			CellRect cellRect;
			return base.CanResolve(rp) && this.TryFindRandomInnerRectTouchingEdge(rp.rect, out cellRect);
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x00236B78 File Offset: 0x00234D78
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (!this.TryFindRandomInnerRectTouchingEdge(rp.rect, out rect))
			{
				return;
			}
			Rot4 value;
			if (rect.Cells.Any((IntVec3 x) => x.x == rp.rect.minX))
			{
				value = Rot4.West;
			}
			else if (rect.Cells.Any((IntVec3 x) => x.x == rp.rect.maxX))
			{
				value = Rot4.East;
			}
			else if (rect.Cells.Any((IntVec3 x) => x.z == rp.rect.minZ))
			{
				value = Rot4.South;
			}
			else
			{
				value = Rot4.North;
			}
			ResolveParams rp2 = rp;
			rp2.rect = rect;
			rp2.thingRot = new Rot4?(value);
			BaseGen.symbolStack.Push("mannedMortar", rp2, null);
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x00236C44 File Offset: 0x00234E44
		private bool TryFindRandomInnerRectTouchingEdge(CellRect rect, out CellRect mortarRect)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec2 size = new IntVec2(3, 3);
			Func<IntVec3, bool> <>9__2;
			Func<IntVec3, bool> <>9__3;
			return rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, delegate(CellRect x)
			{
				IEnumerable<IntVec3> cells = x.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null));
				}
				return cells.All(predicate) && GenConstruct.TerrainCanSupport(x, map, ThingDefOf.Turret_Mortar);
			}) || rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, delegate(CellRect x)
			{
				IEnumerable<IntVec3> cells = x.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null));
				}
				return cells.All(predicate);
			});
		}
	}
}
