using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E6 RID: 742
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0007BCD4 File Offset: 0x00079ED4
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0007BCF0 File Offset: 0x00079EF0
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243, false);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0007BD04 File Offset: 0x00079F04
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 a = thing.TrueCenter();
			Rand.PushState();
			Rand.Seed = thing.Position.GetHashCode();
			Filth filth = thing as Filth;
			int num;
			if (filth == null)
			{
				num = 3;
			}
			else
			{
				num = filth.thickness;
			}
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = a + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
				Vector2 size = new Vector2(Rand.Range(this.data.drawSize.x * 0.8f, this.data.drawSize.x * 1.2f), Rand.Range(this.data.drawSize.y * 0.8f, this.data.drawSize.y * 1.2f));
				float rot = (float)Rand.RangeInclusive(0, 360);
				bool flipUv = Rand.Value < 0.5f;
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, null, null, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0007BE40 File Offset: 0x0007A040
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Scatter(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}

		// Token: 0x04000DED RID: 3565
		private const float PositionVariance = 0.45f;

		// Token: 0x04000DEE RID: 3566
		private const float SizeVariance = 0.2f;

		// Token: 0x04000DEF RID: 3567
		private const float SizeFactorMin = 0.8f;

		// Token: 0x04000DF0 RID: 3568
		private const float SizeFactorMax = 1.2f;
	}
}
