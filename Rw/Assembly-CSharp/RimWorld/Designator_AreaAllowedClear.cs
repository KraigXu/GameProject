using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E18 RID: 3608
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x06005738 RID: 22328 RVA: 0x001D002C File Offset: 0x001CE22C
		public Designator_AreaAllowedClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			this.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "AreaAllowedClear";
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x001D00AE File Offset: 0x001CE2AE
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x001D00D8 File Offset: 0x001CE2D8
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
