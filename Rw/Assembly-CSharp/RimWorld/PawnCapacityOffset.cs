using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000908 RID: 2312
	public class PawnCapacityOffset
	{
		// Token: 0x0600370B RID: 14091 RVA: 0x00128CDE File Offset: 0x00126EDE
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		// Token: 0x04002000 RID: 8192
		public PawnCapacityDef capacity;

		// Token: 0x04002001 RID: 8193
		public float scale = 1f;

		// Token: 0x04002002 RID: 8194
		public float max = 9999f;
	}
}
