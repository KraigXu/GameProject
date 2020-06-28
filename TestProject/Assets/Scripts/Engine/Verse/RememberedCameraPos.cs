using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000069 RID: 105
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x00015918 File Offset: 0x00013B18
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001594C File Offset: 0x00013B4C
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x04000164 RID: 356
		public Vector3 rootPos;

		// Token: 0x04000165 RID: 357
		public float rootSize;
	}
}
