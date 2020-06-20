using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003DA RID: 986
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x06001D50 RID: 7504 RVA: 0x000B433D File Offset: 0x000B253D
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x000B4358 File Offset: 0x000B2558
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.zone.Map.zoneManager.AllZones.Any((Zone z) => z != this.zone && z.label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x000B43CF File Offset: 0x000B25CF
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(this.curName), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x040011C6 RID: 4550
		private Zone zone;
	}
}
