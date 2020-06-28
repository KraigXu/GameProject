using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EC RID: 748
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0007C4F7 File Offset: 0x0007A6F7
		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0007C500 File Offset: 0x0007A700
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedCornerFiller(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0007C524 File Offset: 0x0007A724
		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			IntVec3 position = thing.Position;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.DiagonalDirectionsAround[i];
				if (this.ShouldLinkWith(intVec, thing) && (i != 0 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))) && (i != 1 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 2 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 3 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))))
				{
					Vector3 center = thing.DrawPos + GenAdj.DiagonalDirectionsAround[i].ToVector3().normalized * Graphic_LinkedCornerFiller.CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.09f);
					Vector2 size = new Vector2(0.5f, 0.5f);
					if (!intVec.InBounds(thing.Map))
					{
						if (intVec.x == -1)
						{
							center.x -= 1f;
							size.x *= 5f;
						}
						if (intVec.z == -1)
						{
							center.z -= 1f;
							size.y *= 5f;
						}
						if (intVec.x == thing.Map.Size.x)
						{
							center.x += 1f;
							size.x *= 5f;
						}
						if (intVec.z == thing.Map.Size.z)
						{
							center.z += 1f;
							size.y *= 5f;
						}
					}
					Printer_Plane.PrintPlane(layer, center, size, base.LinkedDrawMatFrom(thing, thing.Position), 0f, false, Graphic_LinkedCornerFiller.CornerFillUVs, null, 0.01f, 0f);
				}
			}
		}

		// Token: 0x04000DF8 RID: 3576
		private const float ShiftUp = 0.09f;

		// Token: 0x04000DF9 RID: 3577
		private const float CoverSize = 0.5f;

		// Token: 0x04000DFA RID: 3578
		private static readonly float CoverSizeCornerCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x04000DFB RID: 3579
		private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x04000DFC RID: 3580
		private static readonly float CoverOffsetDist = Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5f;

		// Token: 0x04000DFD RID: 3581
		private static readonly Vector2[] CornerFillUVs = new Vector2[]
		{
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f)
		};
	}
}
