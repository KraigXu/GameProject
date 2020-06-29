using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		
		// (get) Token: 0x06005C4E RID: 23630 RVA: 0x001FDEAA File Offset: 0x001FC0AA
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
