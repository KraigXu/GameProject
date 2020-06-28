using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1D RID: 3613
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x0600574A RID: 22346 RVA: 0x001D0438 File Offset: 0x001CE638
		public Designator_AreaHomeClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaHomeClear".Translate();
			this.defaultDesc = "DesignatorAreaHomeClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc7;
		}
	}
}
