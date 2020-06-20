using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000986 RID: 2438
	public class QuestPart_RemoveEquipmentFromPawns : QuestPart
	{
		// Token: 0x060039BC RID: 14780 RVA: 0x00132D0C File Offset: 0x00130F0C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i] != null && this.pawns[i].equipment != null)
					{
						this.pawns[i].equipment.DestroyAllEquipment(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x00132D84 File Offset: 0x00130F84
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400220B RID: 8715
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400220C RID: 8716
		public string inSignal;
	}
}
