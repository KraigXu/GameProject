using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C46 RID: 3142
	public class Building_Art : Building
	{
		// Token: 0x06004AEB RID: 19179 RVA: 0x00194CAC File Offset: 0x00192EAC
		public override string GetInspectString()
		{
			return base.GetInspectString() + ("\n" + StatDefOf.Beauty.LabelCap + ": " + StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true), ToStringNumberSense.Absolute, true));
		}
	}
}
