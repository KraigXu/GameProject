using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E29 RID: 3625
	public class Designator_Haul : Designator
	{
		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06005799 RID: 22425 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x0600579A RID: 22426 RVA: 0x001D1800 File Offset: 0x001CFA00
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Haul;
			}
		}

		// Token: 0x0600579B RID: 22427 RVA: 0x001D1808 File Offset: 0x001CFA08
		public Designator_Haul()
		{
			this.defaultLabel = "DesignatorHaulThings".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Haul", true);
			this.defaultDesc = "DesignatorHaulThingsDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Haul;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x001D188C File Offset: 0x001CFA8C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Thing firstHaulable = c.GetFirstHaulable(base.Map);
			if (firstHaulable == null)
			{
				return "MessageMustDesignateHaulable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstHaulable);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x001D18F5 File Offset: 0x001CFAF5
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x001D190C File Offset: 0x001CFB0C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.designateHaulable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			if (t.IsInValidStorage())
			{
				return "MessageAlreadyInStorage".Translate();
			}
			return true;
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x001D196B File Offset: 0x001CFB6B
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x001D0CA1 File Offset: 0x001CEEA1
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
