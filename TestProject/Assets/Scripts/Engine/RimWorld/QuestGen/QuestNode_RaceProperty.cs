using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	
	public abstract class QuestNode_RaceProperty : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.Matches(this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.Matches(this.value.GetValue(slate)))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		
		private bool Matches(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is PawnKindDef)
			{
				return this.Matches(((PawnKindDef)obj).RaceProps);
			}
			if (obj is ThingDef)
			{
				return ((ThingDef)obj).race != null && this.Matches(((ThingDef)obj).race);
			}
			if (obj is Pawn)
			{
				return this.Matches(((Pawn)obj).RaceProps);
			}
			if (obj is Faction)
			{
				return ((Faction)obj).def.basicMemberKind != null && this.Matches(((Faction)obj).def.basicMemberKind);
			}
			if (obj is IEnumerable<Pawn>)
			{
				return ((IEnumerable<Pawn>)obj).Any<Pawn>() && ((IEnumerable<Pawn>)obj).All((Pawn x) => this.Matches(x.RaceProps));
			}
			if (obj is IEnumerable<Thing>)
			{
				return ((IEnumerable<Thing>)obj).Any<Thing>() && ((IEnumerable<Thing>)obj).All((Thing x) => x is Pawn && this.Matches(((Pawn)x).RaceProps));
			}
			if (obj is IEnumerable<object>)
			{
				return ((IEnumerable<object>)obj).Any<object>() && ((IEnumerable<object>)obj).All((object x) => x is Pawn && this.Matches(((Pawn)x).RaceProps));
			}
			return obj is string && !((string)obj).NullOrEmpty() && this.Matches(DefDatabase<PawnKindDef>.GetNamed((string)obj, true).RaceProps);
		}

		
		protected abstract bool Matches(RaceProperties raceProperties);

		
		[NoTranslate]
		public SlateRef<object> value;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
