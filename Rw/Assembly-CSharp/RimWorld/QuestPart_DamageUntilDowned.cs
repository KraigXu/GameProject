using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096C RID: 2412
	public class QuestPart_DamageUntilDowned : QuestPart
	{
		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003925 RID: 14629 RVA: 0x001306FC File Offset: 0x0012E8FC
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
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x0013070C File Offset: 0x0012E90C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						HealthUtility.DamageUntilDowned(this.pawns[i], this.allowBleedingWounds);
					}
				}
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x00130774 File Offset: 0x0012E974
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.allowBleedingWounds, "allowBleedingWounds", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x001307F4 File Offset: 0x0012E9F4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0013082B File Offset: 0x0012EA2B
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040021B0 RID: 8624
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040021B1 RID: 8625
		public string inSignal;

		// Token: 0x040021B2 RID: 8626
		public bool allowBleedingWounds = true;
	}
}
