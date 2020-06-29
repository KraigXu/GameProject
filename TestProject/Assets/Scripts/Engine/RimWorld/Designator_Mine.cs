﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Designator_Mine : Designator
	{
		
		// (get) Token: 0x060057AB RID: 22443 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x060057AD RID: 22445 RVA: 0x001D1CAB File Offset: 0x001CFEAB
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Mine;
			}
		}

		
		public Designator_Mine()
		{
			this.defaultLabel = "DesignatorMine".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true);
			this.defaultDesc = "DesignatorMineDesc".Translate();
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "Mine";
		}

		
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (c.Fogged(base.Map))
			{
				return true;
			}
			Mineable firstMineable = c.GetFirstMineable(base.Map);
			if (firstMineable == null)
			{
				return "MessageMustDesignateMineable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstMineable);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		
		public override void DesignateSingleCell(IntVec3 loc)
		{
			base.Map.designationManager.AddDesignation(new Designation(loc, this.Designation));
			base.Map.designationManager.TryRemoveDesignation(loc, DesignationDefOf.SmoothWall);
			if (DebugSettings.godMode)
			{
				Mineable firstMineable = loc.GetFirstMineable(base.Map);
				if (firstMineable != null)
				{
					firstMineable.DestroyMined(null);
				}
			}
		}

		
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
		}

		
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
