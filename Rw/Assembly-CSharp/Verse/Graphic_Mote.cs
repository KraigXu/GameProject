using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002ED RID: 749
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x00010306 File Offset: 0x0000E506
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0007C85E File Offset: 0x0007AA5E
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0007C86C File Offset: 0x0007AA6C
		public void DrawMoteInternal(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer)
		{
			Mote mote = (Mote)thing;
			float alpha = mote.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Color color = base.Color * mote.instanceColor;
			color.a *= alpha;
			Vector3 exactScale = mote.exactScale;
			exactScale.x *= this.data.drawSize.x;
			exactScale.z *= this.data.drawSize.y;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
			Material matSingle = this.MatSingle;
			if (!this.ForcePropertyBlock && color.IndistinguishableFrom(matSingle.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, layer, null, 0, Graphic_Mote.propertyBlock);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0007C96C File Offset: 0x0007AB6C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Mote(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x04000DFE RID: 3582
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
