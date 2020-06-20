using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001118 RID: 4376
	public class QuestNode_GenerateShuttle : QuestNode
	{
		// Token: 0x06006679 RID: 26233 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x0023E164 File Offset: 0x0023C364
		protected override void RunInt()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return;
			}
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
			if (this.owningFaction.GetValue(slate) != null)
			{
				thing.SetFaction(this.owningFaction.GetValue(slate), null);
			}
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (this.requiredPawns.GetValue(slate) != null)
			{
				compShuttle.requiredPawns.AddRange(this.requiredPawns.GetValue(slate));
			}
			if (this.requiredItems.GetValue(slate) != null)
			{
				compShuttle.requiredItems.AddRange(this.requiredItems.GetValue(slate));
			}
			compShuttle.acceptColonists = this.acceptColonists.GetValue(slate);
			compShuttle.onlyAcceptColonists = this.onlyAcceptColonists.GetValue(slate);
			compShuttle.onlyAcceptHealthy = this.onlyAcceptHealthy.GetValue(slate);
			compShuttle.requiredColonistCount = this.requireColonistCount.GetValue(slate);
			compShuttle.dropEverythingIfUnsatisfied = this.dropEverythingIfUnsatisfied.GetValue(slate);
			compShuttle.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied.GetValue(slate);
			compShuttle.dropEverythingOnArrival = this.dropEverythingOnArrival.GetValue(slate);
			QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
		}

		// Token: 0x04003E95 RID: 16021
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003E96 RID: 16022
		public SlateRef<IEnumerable<Pawn>> requiredPawns;

		// Token: 0x04003E97 RID: 16023
		public SlateRef<IEnumerable<ThingDefCount>> requiredItems;

		// Token: 0x04003E98 RID: 16024
		public SlateRef<int> requireColonistCount;

		// Token: 0x04003E99 RID: 16025
		public SlateRef<bool> acceptColonists;

		// Token: 0x04003E9A RID: 16026
		public SlateRef<bool> onlyAcceptColonists;

		// Token: 0x04003E9B RID: 16027
		public SlateRef<bool> onlyAcceptHealthy;

		// Token: 0x04003E9C RID: 16028
		public SlateRef<bool> leaveImmediatelyWhenSatisfied;

		// Token: 0x04003E9D RID: 16029
		public SlateRef<bool> dropEverythingIfUnsatisfied;

		// Token: 0x04003E9E RID: 16030
		public SlateRef<bool> dropEverythingOnArrival;

		// Token: 0x04003E9F RID: 16031
		public SlateRef<Faction> owningFaction;
	}
}
