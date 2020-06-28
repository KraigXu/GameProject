using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000865 RID: 2149
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x06003506 RID: 13574 RVA: 0x00122745 File Offset: 0x00120945
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		// Token: 0x04001C3A RID: 7226
		public float mag = 0.05f;
	}
}
