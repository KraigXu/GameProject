using System;

namespace Verse
{
	// Token: 0x020003DB RID: 987
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x06001D53 RID: 7507 RVA: 0x000B4407 File Offset: 0x000B2607
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000B4424 File Offset: 0x000B2624
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.area.Map.areaManager.AllAreas.Any((Area a) => a != this.area && a.Label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000B449B File Offset: 0x000B269B
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		// Token: 0x040011C7 RID: 4551
		private Area area;
	}
}
