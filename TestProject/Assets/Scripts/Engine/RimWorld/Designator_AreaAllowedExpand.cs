using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E17 RID: 3607
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x06005735 RID: 22325 RVA: 0x001CFF6C File Offset: 0x001CE16C
		public Designator_AreaAllowedExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorExpandAreaAllowed".Translate();
			this.defaultDesc = "DesignatorExpandAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedExpand", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.hotKey = KeyBindingDefOf.Misc8;
			this.tutorTag = "AreaAllowedExpand";
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x001CFFEE File Offset: 0x001CE1EE
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x001D001B File Offset: 0x001CE21B
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
