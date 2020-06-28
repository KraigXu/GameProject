using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E25 RID: 3621
	public class Designator_Deconstruct : Designator
	{
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06005772 RID: 22386 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x000FB242 File Offset: 0x000F9442
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x001D101C File Offset: 0x001CF21C
		public Designator_Deconstruct()
		{
			this.defaultLabel = "DesignatorDeconstruct".Translate();
			this.defaultDesc = "DesignatorDeconstructDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Deconstruct", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x001D10A0 File Offset: 0x001CF2A0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				return false;
			}
			AcceptanceReport result;
			if (this.TopDeconstructibleInCell(c, out result) == null)
			{
				return result;
			}
			return true;
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001D10F4 File Offset: 0x001CF2F4
		public override void DesignateSingleCell(IntVec3 loc)
		{
			AcceptanceReport acceptanceReport;
			this.DesignateThing(this.TopDeconstructibleInCell(loc, out acceptanceReport));
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001D1110 File Offset: 0x001CF310
		private Thing TopDeconstructibleInCell(IntVec3 loc, out AcceptanceReport reportToDisplay)
		{
			reportToDisplay = AcceptanceReport.WasRejected;
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				AcceptanceReport acceptanceReport = this.CanDesignateThing(thing);
				if (this.CanDesignateThing(thing).Accepted)
				{
					reportToDisplay = AcceptanceReport.WasAccepted;
					return thing;
				}
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					reportToDisplay = acceptanceReport;
				}
			}
			return null;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x001D11D4 File Offset: 0x001CF3D4
		public override void DesignateThing(Thing t)
		{
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (DebugSettings.godMode || innerIfMinified.GetStatValue(StatDefOf.WorkToBuild, true) == 0f || t.def.IsFrame)
			{
				t.Destroy(DestroyMode.Deconstruct);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x001D1238 File Offset: 0x001CF438
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t.GetInnerIfMinified() as Building;
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			if (!building.DeconstructibleBy(Faction.OfPlayer))
			{
				if (building.Faction == Faction.OfMechanoids && building.def.building.IsDeconstructible)
				{
					return new AcceptanceReport("MessageMustDesignateDeconstructibleMechCluster".Translate());
				}
				return false;
			}
			else
			{
				if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
				{
					return false;
				}
				if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
				{
					return false;
				}
				return true;
			}
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x001D0CA1 File Offset: 0x001CEEA1
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
