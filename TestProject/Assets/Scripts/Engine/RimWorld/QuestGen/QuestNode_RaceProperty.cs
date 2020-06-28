using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001159 RID: 4441
	public abstract class QuestNode_RaceProperty : QuestNode
	{
		// Token: 0x0600677E RID: 26494 RVA: 0x00243350 File Offset: 0x00241550
		protected override bool TestRunInt(Slate slate)
		{
			if (this.Matches(this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x002433A0 File Offset: 0x002415A0
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

		// Token: 0x06006780 RID: 26496 RVA: 0x002433F0 File Offset: 0x002415F0
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

		// Token: 0x06006781 RID: 26497
		protected abstract bool Matches(RaceProperties raceProperties);

		// Token: 0x04003F9E RID: 16286
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x04003F9F RID: 16287
		public QuestNode node;

		// Token: 0x04003FA0 RID: 16288
		public QuestNode elseNode;
	}
}
