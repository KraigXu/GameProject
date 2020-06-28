using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6F RID: 3439
	public class CompProperties_ThrownMoteEmitter : CompProperties
	{
		// Token: 0x060053BD RID: 21437 RVA: 0x001BFA36 File Offset: 0x001BDC36
		public CompProperties_ThrownMoteEmitter()
		{
			this.compClass = typeof(CompThrownMoteEmitter);
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x001BFA72 File Offset: 0x001BDC72
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompThrownMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x04002E37 RID: 11831
		public ThingDef mote;

		// Token: 0x04002E38 RID: 11832
		public Vector3 offsetMin;

		// Token: 0x04002E39 RID: 11833
		public Vector3 offsetMax;

		// Token: 0x04002E3A RID: 11834
		public int emissionInterval = -1;

		// Token: 0x04002E3B RID: 11835
		public int burstCount = 1;

		// Token: 0x04002E3C RID: 11836
		public Color colorA = Color.white;

		// Token: 0x04002E3D RID: 11837
		public Color colorB = Color.white;

		// Token: 0x04002E3E RID: 11838
		public FloatRange scale;

		// Token: 0x04002E3F RID: 11839
		public FloatRange rotationRate;

		// Token: 0x04002E40 RID: 11840
		public FloatRange velocityX;

		// Token: 0x04002E41 RID: 11841
		public FloatRange velocityY;
	}
}
