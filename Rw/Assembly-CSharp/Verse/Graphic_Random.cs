using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F1 RID: 753
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x0007BCD4 File Offset: 0x00079ED4
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0007CFE5 File Offset: 0x0007B1E5
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0007D022 File Offset: 0x0007B222
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0007D035 File Offset: 0x0007B235
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0007D050 File Offset: 0x0007B250
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0007D082 File Offset: 0x0007B282
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			return this.subGraphics[thing.thingIDNumber % this.subGraphics.Length];
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0007D0A6 File Offset: 0x0007B2A6
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0007D0B0 File Offset: 0x0007B2B0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Random(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
