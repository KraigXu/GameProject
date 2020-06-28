using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000968 RID: 2408
	public class QuestPart_BiocodeWeapons : QuestPart
	{
		// Token: 0x0600390C RID: 14604 RVA: 0x0012FFC8 File Offset: 0x0012E1C8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].equipment != null)
					{
						foreach (ThingWithComps thingWithComps in this.pawns[i].equipment.AllEquipmentListForReading)
						{
							CompBiocodableWeapon comp = thingWithComps.GetComp<CompBiocodableWeapon>();
							if (comp != null && !comp.Biocoded)
							{
								comp.CodeFor(this.pawns[i]);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x00130090 File Offset: 0x0012E290
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

		// Token: 0x0600390E RID: 14606 RVA: 0x001300FE File Offset: 0x0012E2FE
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040021A3 RID: 8611
		public string inSignal;

		// Token: 0x040021A4 RID: 8612
		public List<Pawn> pawns = new List<Pawn>();
	}
}
