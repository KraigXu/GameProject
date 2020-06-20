using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094E RID: 2382
	public class QuestPart_Filter_AnyPawnAlive : QuestPart_Filter
	{
		// Token: 0x0600386D RID: 14445 RVA: 0x0012E180 File Offset: 0x0012C380
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Destroyed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x0012E1E8 File Offset: 0x0012C3E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400215A RID: 8538
		public List<Pawn> pawns;
	}
}
