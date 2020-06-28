using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095D RID: 2397
	public class QuestPart_ShuttleDelay : QuestPart_Delay
	{
		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060038B8 RID: 14520 RVA: 0x0012F166 File Offset: 0x0012D366
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.lodgers.Count; i = num + 1)
				{
					yield return this.lodgers[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0012F178 File Offset: 0x0012D378
		public override string ExtraInspectString(ISelectable target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null && this.lodgers.Contains(pawn))
			{
				return "ShuttleDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0012F1C4 File Offset: 0x0012D3C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0012F220 File Offset: 0x0012D420
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.lodgers.AddRange(Find.RandomPlayerHomeMap.mapPawns.FreeColonists);
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0012F249 File Offset: 0x0012D449
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.lodgers.Replace(replace, with);
		}

		// Token: 0x04002182 RID: 8578
		public List<Pawn> lodgers = new List<Pawn>();
	}
}
