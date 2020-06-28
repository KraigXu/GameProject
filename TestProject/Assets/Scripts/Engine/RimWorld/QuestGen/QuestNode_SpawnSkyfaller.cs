using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001193 RID: 4499
	public class QuestNode_SpawnSkyfaller : QuestNode
	{
		// Token: 0x0600683F RID: 26687 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x00246BC4 File Offset: 0x00244DC4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Skyfaller thing = SkyfallerMaker.MakeSkyfaller(this.skyfallerDef.GetValue(slate), this.innerThings.GetValue(slate));
			QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
			questPart_SpawnThing.thing = thing;
			questPart_SpawnThing.mapParent = map.Parent;
			if (this.factionOfForSafeSpot.GetValue(slate) != null)
			{
				questPart_SpawnThing.factionForFindingSpot = this.factionOfForSafeSpot.GetValue(slate).Faction;
			}
			if (this.cell.GetValue(slate) != null)
			{
				questPart_SpawnThing.cell = this.cell.GetValue(slate).Value;
			}
			questPart_SpawnThing.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnThing.lookForSafeSpot = this.lookForSafeSpot.GetValue(slate);
			questPart_SpawnThing.tryLandInShipLandingZone = this.tryLandInShipLandingZone.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SpawnThing);
		}

		// Token: 0x0400408C RID: 16524
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400408D RID: 16525
		public SlateRef<ThingDef> skyfallerDef;

		// Token: 0x0400408E RID: 16526
		public SlateRef<IEnumerable<Thing>> innerThings;

		// Token: 0x0400408F RID: 16527
		public SlateRef<IntVec3?> cell;

		// Token: 0x04004090 RID: 16528
		public SlateRef<Pawn> factionOfForSafeSpot;

		// Token: 0x04004091 RID: 16529
		public SlateRef<bool> lookForSafeSpot;

		// Token: 0x04004092 RID: 16530
		public SlateRef<bool> tryLandInShipLandingZone;
	}
}
