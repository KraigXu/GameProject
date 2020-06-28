using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EE RID: 750
	public class Graphic_MotePulse : Graphic_Mote
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0007C9D0 File Offset: 0x0007ABD0
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0007CA1C File Offset: 0x0007AC1C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_MotePulse(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
