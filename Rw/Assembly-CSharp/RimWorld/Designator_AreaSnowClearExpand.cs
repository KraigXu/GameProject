using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E21 RID: 3617
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		// Token: 0x0600575F RID: 22367 RVA: 0x001D0850 File Offset: 0x001CEA50
		public Designator_AreaSnowClearExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaSnowClearExpand".Translate();
			this.defaultDesc = "DesignatorAreaSnowClearExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
		}
	}
}
