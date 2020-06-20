using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2D RID: 3373
	public class CompProperties_MoteEmitter : CompProperties
	{
		// Token: 0x060051FD RID: 20989 RVA: 0x001B65F7 File Offset: 0x001B47F7
		public CompProperties_MoteEmitter()
		{
			this.compClass = typeof(CompMoteEmitter);
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x001B6616 File Offset: 0x001B4816
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x04002D2C RID: 11564
		public ThingDef mote;

		// Token: 0x04002D2D RID: 11565
		public Vector3 offset;

		// Token: 0x04002D2E RID: 11566
		public int emissionInterval = -1;

		// Token: 0x04002D2F RID: 11567
		public bool maintain;

		// Token: 0x04002D30 RID: 11568
		public string saveKeysPrefix;
	}
}
