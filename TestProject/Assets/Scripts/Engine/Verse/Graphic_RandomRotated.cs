using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F2 RID: 754
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0007D0EE File Offset: 0x0007B2EE
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0007D0FB File Offset: 0x0007B2FB
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0007D114 File Offset: 0x0007B314
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Material matSingle = this.subGraphic.MatSingle;
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), matSingle, 0, null, 0);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0007D17C File Offset: 0x0007B37C
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0007D198 File Offset: 0x0007B398
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data
			};
		}

		// Token: 0x04000E03 RID: 3587
		private Graphic subGraphic;

		// Token: 0x04000E04 RID: 3588
		private float maxAngle;
	}
}
