using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E36 RID: 3638
	public class Designator_Strip : Designator
	{
		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x060057E8 RID: 22504 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x001D2A50 File Offset: 0x001D0C50
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Strip;
			}
		}

		// Token: 0x060057EA RID: 22506 RVA: 0x001D2A58 File Offset: 0x001D0C58
		public Designator_Strip()
		{
			this.defaultLabel = "DesignatorStrip".Translate();
			this.defaultDesc = "DesignatorStripDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x001D2AD9 File Offset: 0x001D0CD9
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.StrippablesInCell(c).Any<Thing>())
			{
				return "MessageMustDesignateStrippable".Translate();
			}
			return true;
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x001D2B14 File Offset: 0x001D0D14
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StrippablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x001D2B64 File Offset: 0x001D0D64
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x001D2B91 File Offset: 0x001D0D91
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(t);
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x001D2BBA File Offset: 0x001D0DBA
		private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}
	}
}
