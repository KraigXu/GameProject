using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F5 RID: 757
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0007D485 File Offset: 0x0007B685
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0007D49D File Offset: 0x0007B69D
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0007D022 File Offset: 0x0007B222
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0007D4B9 File Offset: 0x0007B6B9
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0007D4D1 File Offset: 0x0007B6D1
		public Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0007D4E8 File Offset: 0x0007B6E8
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

		// Token: 0x06001570 RID: 5488 RVA: 0x0007D51C File Offset: 0x0007B71C
		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			switch (this.subGraphics.Length)
			{
			case 1:
				return this.subGraphics[0];
			case 2:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				return this.subGraphics[1];
			case 3:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[2];
				}
				return this.subGraphics[1];
			default:
			{
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[this.subGraphics.Length - 1];
				}
				int num = Mathf.Min(1 + Mathf.RoundToInt((float)stackCount / (float)def.stackLimit * ((float)this.subGraphics.Length - 3f) + 1E-05f), this.subGraphics.Length - 2);
				return this.subGraphics[num];
			}
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0007D5F6 File Offset: 0x0007B7F6
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"StackCount(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
