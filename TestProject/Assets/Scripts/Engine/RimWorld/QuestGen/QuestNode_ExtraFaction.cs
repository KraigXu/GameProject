using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_ExtraFaction : QuestNode
	{
		
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

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		public SlateRef<Thing> factionOf;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<ExtraFactionType> factionType;

		
		public SlateRef<bool> areHelpers;
	}
}
