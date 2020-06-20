using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC2 RID: 3778
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06005C4E RID: 23630 RVA: 0x001FDEAA File Offset: 0x001FC0AA
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x001FDBF8 File Offset: 0x001FBDF8
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
