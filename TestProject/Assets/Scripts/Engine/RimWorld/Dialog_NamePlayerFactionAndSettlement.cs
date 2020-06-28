using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5F RID: 3679
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x0600592E RID: 22830 RVA: 0x001DC704 File Offset: 0x001DA904
		public Dialog_NamePlayerFactionAndSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			this.useSecondName = true;
			this.secondNameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidSecondName), false, null, null));
			this.curSecondName = this.secondNameGenerator();
			this.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			this.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			this.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x001DC7D7 File Offset: 0x001DA9D7
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x001DC627 File Offset: 0x001DA827
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x001DC801 File Offset: 0x001DAA01
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x001DC62F File Offset: 0x001DA82F
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x001DC809 File Offset: 0x001DAA09
		protected override void NamedSecond(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x04003049 RID: 12361
		private Settlement settlement;
	}
}
