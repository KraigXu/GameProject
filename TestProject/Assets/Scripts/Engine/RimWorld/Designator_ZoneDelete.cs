using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E47 RID: 3655
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x06005860 RID: 22624 RVA: 0x001D53BC File Offset: 0x001D35BC
		public Designator_ZoneDelete()
		{
			this.defaultLabel = "DesignatorZoneDelete".Translate();
			this.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			this.hotKey = KeyBindingDefOf.Misc3;
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x001D5444 File Offset: 0x001D3644
		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			if (!sq.InBounds(base.Map))
			{
				return false;
			}
			if (sq.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.zoneManager.ZoneAt(sq) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x001D549C File Offset: 0x001D369C
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x001D54DC File Offset: 0x001D36DC
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x04002FB5 RID: 12213
		private List<Zone> justDesignated = new List<Zone>();
	}
}
