using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E28 RID: 3624
	public class Designator_Forbid : Designator
	{
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06005792 RID: 22418 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x001D1644 File Offset: 0x001CF844
		public Designator_Forbid()
		{
			this.defaultLabel = "DesignatorForbid".Translate();
			this.defaultDesc = "DesignatorForbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Command_ItemForbid;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "ForbidAllItems".Translate();
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x001D16E4 File Offset: 0x001CF8E4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateForbiddable".Translate();
			}
			return true;
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x001D174C File Offset: 0x001CF94C
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x001D179C File Offset: 0x001CF99C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && !compForbiddable.Forbidden;
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x001D17D9 File Offset: 0x001CF9D9
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(true, false);
		}
	}
}
