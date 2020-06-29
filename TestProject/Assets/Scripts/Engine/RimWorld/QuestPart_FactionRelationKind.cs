using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_FactionRelationKind : QuestPartActivable
	{
		
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x0012E819 File Offset: 0x0012CA19
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__0())
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

		
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.faction1 != null && this.faction2 != null && this.faction1.RelationKindWith(this.faction2) == this.relationKind)
			{
				base.Complete(this.faction1.Named("SUBJECT"));
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.faction1 = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.faction2 = Faction.OfPlayer;
			this.relationKind = FactionRelationKind.Neutral;
		}

		
		public Faction faction1;

		
		public Faction faction2;

		
		public FactionRelationKind relationKind;
	}
}
