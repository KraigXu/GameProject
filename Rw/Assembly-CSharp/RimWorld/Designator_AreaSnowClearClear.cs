using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E22 RID: 3618
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x06005760 RID: 22368 RVA: 0x001D08BC File Offset: 0x001CEABC
		public Designator_AreaSnowClearClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaSnowClearClear".Translate();
			this.defaultDesc = "DesignatorAreaSnowClearClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
		}
	}
}
