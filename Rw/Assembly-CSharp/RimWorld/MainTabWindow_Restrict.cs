using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECA RID: 3786
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x002038D7 File Offset: 0x00201AD7
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x06005CD2 RID: 23762 RVA: 0x001FDBF8 File Offset: 0x001FBDF8
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x002038DE File Offset: 0x00201ADE
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));
		}

		// Token: 0x040032AF RID: 12975
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x040032B0 RID: 12976
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
