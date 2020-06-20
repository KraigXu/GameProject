using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098F RID: 2447
	public class QuestPart_SetAllApparelLocked : QuestPart
	{
		// Token: 0x060039E6 RID: 14822 RVA: 0x00133790 File Offset: 0x00131990
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].apparel != null)
					{
						this.pawns[i].apparel.LockAll();
					}
				}
			}
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x001337F8 File Offset: 0x001319F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x00133866 File Offset: 0x00131A66
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400221F RID: 8735
		public string inSignal;

		// Token: 0x04002220 RID: 8736
		public List<Pawn> pawns = new List<Pawn>();
	}
}
