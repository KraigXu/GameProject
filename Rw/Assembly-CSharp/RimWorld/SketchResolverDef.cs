using System;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000903 RID: 2307
	public class SketchResolverDef : Def
	{
		// Token: 0x060036F6 RID: 14070 RVA: 0x00128917 File Offset: 0x00126B17
		public void Resolve(ResolveParams parms)
		{
			this.resolver.Resolve(parms);
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x00128925 File Offset: 0x00126B25
		public bool CanResolve(ResolveParams parms)
		{
			return this.resolver.CanResolve(parms);
		}

		// Token: 0x04001FC1 RID: 8129
		public SketchResolver resolver;

		// Token: 0x04001FC2 RID: 8130
		public bool isRoot;
	}
}
