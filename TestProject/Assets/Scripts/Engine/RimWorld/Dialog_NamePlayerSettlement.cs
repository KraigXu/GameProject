using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E60 RID: 3680
	public class Dialog_NamePlayerSettlement : Dialog_GiveName
	{
		// Token: 0x06005936 RID: 22838 RVA: 0x001DC840 File Offset: 0x001DAA40
		public Dialog_NamePlayerSettlement(Settlement settlement)
		{
			this.settlement = settlement;
			if (settlement.HasMap && settlement.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = settlement.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, new Predicate<string>(this.IsValidName), false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionBaseMessage";
			this.gainedNameMessageKey = "PlayerFactionBaseGainsName";
			this.invalidNameMessageKey = "PlayerFactionBaseNameIsInvalid";
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x001DC8D3 File Offset: 0x001DAAD3
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.settlement.Map != null)
			{
				Current.Game.CurrentMap = this.settlement.Map;
			}
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x001DC801 File Offset: 0x001DAA01
		protected override bool IsValidName(string s)
		{
			return NamePlayerSettlementDialogUtility.IsValidName(s);
		}

		// Token: 0x06005939 RID: 22841 RVA: 0x001DC8FD File Offset: 0x001DAAFD
		protected override void Named(string s)
		{
			NamePlayerSettlementDialogUtility.Named(this.settlement, s);
		}

		// Token: 0x0400304A RID: 12362
		private Settlement settlement;
	}
}
