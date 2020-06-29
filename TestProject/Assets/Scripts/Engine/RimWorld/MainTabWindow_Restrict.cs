using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x002038D7 File Offset: 0x00201AD7
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));
		}

		
		private const int TimeAssignmentSelectorWidth = 191;

		
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
