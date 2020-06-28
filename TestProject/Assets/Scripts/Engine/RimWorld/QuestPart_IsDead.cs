using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000957 RID: 2391
	public class QuestPart_IsDead : QuestPartActivable
	{
		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003890 RID: 14480 RVA: 0x0012E8F0 File Offset: 0x0012CAF0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x0012E900 File Offset: 0x0012CB00
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.Destroyed)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x0012E933 File Offset: 0x0012CB33
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x0012E94C File Offset: 0x0012CB4C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0012E975 File Offset: 0x0012CB75
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002170 RID: 8560
		public Pawn pawn;
	}
}
