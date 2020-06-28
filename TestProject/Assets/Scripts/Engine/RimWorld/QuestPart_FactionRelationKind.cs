using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	public class QuestPart_FactionRelationKind : QuestPartActivable
	{
		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x0012E819 File Offset: 0x0012CA19
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__0())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction1 != null)
				{
					yield return this.faction1;
				}
				if (this.faction2 != null)
				{
					yield return this.faction2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0012E82C File Offset: 0x0012CA2C
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.faction1 != null && this.faction2 != null && this.faction1.RelationKindWith(this.faction2) == this.relationKind)
			{
				base.Complete(this.faction1.Named("SUBJECT"));
			}
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x0012E87E File Offset: 0x0012CA7E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0012E8BA File Offset: 0x0012CABA
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.faction1 = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.faction2 = Faction.OfPlayer;
			this.relationKind = FactionRelationKind.Neutral;
		}

		// Token: 0x0400216D RID: 8557
		public Faction faction1;

		// Token: 0x0400216E RID: 8558
		public Faction faction2;

		// Token: 0x0400216F RID: 8559
		public FactionRelationKind relationKind;
	}
}
