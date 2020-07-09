using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GenerateShuttle : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<IEnumerable<Pawn>> requiredPawns;

		
		public SlateRef<IEnumerable<ThingDefCount>> requiredItems;

		
		public SlateRef<int> requireColonistCount;

		
		public SlateRef<bool> acceptColonists;

		
		public SlateRef<bool> onlyAcceptColonists;

		
		public SlateRef<bool> onlyAcceptHealthy;

		
		public SlateRef<bool> leaveImmediatelyWhenSatisfied;

		
		public SlateRef<bool> dropEverythingIfUnsatisfied;

		
		public SlateRef<bool> dropEverythingOnArrival;

		
		public SlateRef<Faction> owningFaction;
	}
}
