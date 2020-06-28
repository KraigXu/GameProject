using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200116D RID: 4461
	public class QuestNode_ExtraFaction : QuestNode
	{
		// Token: 0x060067BF RID: 26559 RVA: 0x00244194 File Offset: 0x00242394
		protected override void RunInt()
		{
			Faction value = this.faction.GetValue(QuestGen.slate);
			if (value == null)
			{
				Thing value2 = this.factionOf.GetValue(QuestGen.slate);
				if (value2 != null)
				{
					value = value2.Faction;
				}
				if (value == null)
				{
					return;
				}
			}
			QuestGen.quest.AddPart(new QuestPart_ExtraFaction
			{
				affectedPawns = this.pawns.GetValue(QuestGen.slate).ToList<Pawn>(),
				extraFaction = new ExtraFaction(value, this.factionType.GetValue(QuestGen.slate)),
				areHelpers = this.areHelpers.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04003FE5 RID: 16357
		public SlateRef<Thing> factionOf;

		// Token: 0x04003FE6 RID: 16358
		public SlateRef<Faction> faction;

		// Token: 0x04003FE7 RID: 16359
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04003FE8 RID: 16360
		public SlateRef<ExtraFactionType> factionType;

		// Token: 0x04003FE9 RID: 16361
		public SlateRef<bool> areHelpers;
	}
}
