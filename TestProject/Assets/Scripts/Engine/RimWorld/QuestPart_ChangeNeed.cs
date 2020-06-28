using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096A RID: 2410
	public class QuestPart_ChangeNeed : QuestPart
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003916 RID: 14614 RVA: 0x00130289 File Offset: 0x0012E489
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

		// Token: 0x06003917 RID: 14615 RVA: 0x0013029C File Offset: 0x0012E49C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				Need need = this.pawn.needs.TryGetNeed(this.need);
				if (need != null)
				{
					need.CurLevel += this.offset;
				}
			}
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x00130308 File Offset: 0x0012E508
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<float>(ref this.offset, "offset", 0f, false);
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x00130364 File Offset: 0x0012E564
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.need = NeedDefOf.Food;
			this.offset = 0.5f;
			if (Find.AnyPlayerHomeMap != null)
			{
				Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0013039E File Offset: 0x0012E59E
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x040021A9 RID: 8617
		public string inSignal;

		// Token: 0x040021AA RID: 8618
		public Pawn pawn;

		// Token: 0x040021AB RID: 8619
		public NeedDef need;

		// Token: 0x040021AC RID: 8620
		public float offset;
	}
}
