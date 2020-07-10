using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		
		
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		
		
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		
		
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
			if (CheckCanInteract())
			{
				if (selectedArea != null)
				{
					base.ProcessInput(ev);
				}
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate (Area a)
				{
					selectedArea = a;
					base.ProcessInput(ev);
				}, addNullAreaOption: false, addManageOption: true, base.Map);
			}
		}

		
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		
		private static Area selectedArea;
	}
}
