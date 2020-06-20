using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EF RID: 751
	public class Graphic_MoteSplash : Graphic_Mote
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0007CA7C File Offset: 0x0007AC7C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteSplash moteSplash = (MoteSplash)thing;
			float alpha = moteSplash.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, alpha));
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, moteSplash.CalculatedShockwaveSpan());
			base.DrawMoteInternal(loc, rot, thingDef, thing, SubcameraDefOf.WaterDepth.LayerId);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0007CAF0 File Offset: 0x0007ACF0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"MoteSplash(path=",
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
