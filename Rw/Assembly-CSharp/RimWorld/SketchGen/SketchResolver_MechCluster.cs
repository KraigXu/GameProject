using System;

namespace RimWorld.SketchGen
{
	// Token: 0x0200108E RID: 4238
	public class SketchResolver_MechCluster : SketchResolver
	{
		// Token: 0x06006485 RID: 25733 RVA: 0x0022EA8D File Offset: 0x0022CC8D
		protected override void ResolveInt(ResolveParams parms)
		{
			MechClusterGenerator.ResolveSketch(parms);
		}

		// Token: 0x06006486 RID: 25734 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}
	}
}
