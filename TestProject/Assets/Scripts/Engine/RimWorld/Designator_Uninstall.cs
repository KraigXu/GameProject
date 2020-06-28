using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3A RID: 3642
	public class Designator_Uninstall : Designator
	{
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06005803 RID: 22531 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06005804 RID: 22532 RVA: 0x000FB6CA File Offset: 0x000F98CA
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x001D31F0 File Offset: 0x001D13F0
		public Designator_Uninstall()
		{
			this.defaultLabel = "DesignatorUninstall".Translate();
			this.defaultDesc = "DesignatorUninstallDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Uninstall", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x001D3274 File Offset: 0x001D1474
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
			if (this.TopUninstallableInCell(c) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x001D32C8 File Offset: 0x001D14C8
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x001D32D8 File Offset: 0x001D14D8
		private Thing TopUninstallableInCell(IntVec3 loc)
		{
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				if (this.CanDesignateThing(thing).Accepted)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x001D3368 File Offset: 0x001D1568
		public override void DesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				t.SetFaction(Faction.OfPlayer, null);
			}
			if (DebugSettings.godMode || t.GetStatValue(StatDefOf.WorkToBuild, true) == 0f || t.def.IsFrame)
			{
				t.Uninstall();
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x001D33E0 File Offset: 0x001D15E0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			if (!building.def.Minifiable)
			{
				return false;
			}
			if (!DebugSettings.godMode && building.Faction != Faction.OfPlayer)
			{
				if (building.Faction != null)
				{
					return false;
				}
				if (!building.ClaimableBy(Faction.OfPlayer))
				{
					return false;
				}
			}
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x001D0CA1 File Offset: 0x001CEEA1
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
