using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		
		// (get) Token: 0x0600572C RID: 22316 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600572E RID: 22318 RVA: 0x001CFE9E File Offset: 0x001CE09E
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		
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
				this.n__0(ev);
			}, false, true, base.Map);
		}

		
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		
		private static Area selectedArea;
	}
}
