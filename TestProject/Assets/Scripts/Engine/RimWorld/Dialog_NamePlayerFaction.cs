using System;

namespace RimWorld
{
	// Token: 0x02000E5D RID: 3677
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06005928 RID: 22824 RVA: 0x001DC5D0 File Offset: 0x001DA7D0
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x001DC627 File Offset: 0x001DA827
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x001DC62F File Offset: 0x001DA82F
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
