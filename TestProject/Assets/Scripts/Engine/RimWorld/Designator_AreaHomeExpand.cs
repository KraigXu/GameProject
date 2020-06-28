using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1C RID: 3612
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		// Token: 0x06005749 RID: 22345 RVA: 0x001D03B4 File Offset: 0x001CE5B4
		public Designator_AreaHomeExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaHomeExpand".Translate();
			this.defaultDesc = "DesignatorAreaHomeExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.tutorTag = "AreaHomeExpand";
			this.hotKey = KeyBindingDefOf.Misc4;
		}
	}
}
