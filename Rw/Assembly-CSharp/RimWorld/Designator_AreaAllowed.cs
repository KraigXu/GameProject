using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E16 RID: 3606
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x0600572C RID: 22316 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x0600572E RID: 22318 RVA: 0x001CFE9E File Offset: 0x001CE09E
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x001CFEA5 File Offset: 0x001CE0A5
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x001CFECA File Offset: 0x001CE0CA
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x001CFED2 File Offset: 0x001CE0D2
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x001CFEF8 File Offset: 0x001CE0F8
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			if (Designator_AreaAllowed.selectedArea != null)
			{
				base.ProcessInput(ev);
			}
			AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
			{
				Designator_AreaAllowed.selectedArea = a;
				this.<>n__0(ev);
			}, false, true, base.Map);
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x001CFF4E File Offset: 0x001CE14E
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x04002F94 RID: 12180
		private static Area selectedArea;
	}
}
