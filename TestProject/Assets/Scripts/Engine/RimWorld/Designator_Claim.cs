using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E24 RID: 3620
	public class Designator_Claim : Designator
	{
		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x0600576B RID: 22379 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x001D0E14 File Offset: 0x001CF014
		public Designator_Claim()
		{
			this.defaultLabel = "DesignatorClaim".Translate();
			this.defaultDesc = "DesignatorClaimDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Claim", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x001D0E98 File Offset: 0x001CF098
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (!(from t in c.GetThingList(base.Map)
			where this.CanDesignateThing(t).Accepted
			select t).Any<Thing>())
			{
				return "MessageMustDesignateClaimable".Translate();
			}
			return true;
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x001D0F0C File Offset: 0x001CF10C
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

		// Token: 0x0600576F RID: 22383 RVA: 0x001D0F5C File Offset: 0x001CF15C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			return building != null && building.Faction != Faction.OfPlayer && building.ClaimableBy(Faction.OfPlayer);
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x001D0F94 File Offset: 0x001CF194
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				MoteMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}
	}
}
