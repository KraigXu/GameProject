using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000958 RID: 2392
	public class QuestPart_IsPrisoner : QuestPartActivable
	{
		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06003897 RID: 14487 RVA: 0x0012E987 File Offset: 0x0012CB87
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

		// Token: 0x06003898 RID: 14488 RVA: 0x0012E997 File Offset: 0x0012CB97
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.IsPrisoner)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0012E9CA File Offset: 0x0012CBCA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x0012E9E3 File Offset: 0x0012CBE3
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0012EA0C File Offset: 0x0012CC0C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002171 RID: 8561
		public Pawn pawn;
	}
}
