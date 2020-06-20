using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095C RID: 2396
	public class QuestPart_PawnsAvailable : QuestPartActivable
	{
		// Token: 0x060038B4 RID: 14516 RVA: 0x0012F03C File Offset: 0x0012D23C
		public override void QuestPartTick()
		{
			if (this.requiredCount > 0 && Find.TickManager.TicksAbs % 500 == 0)
			{
				int num = 0;
				List<Pawn> allPawnsSpawned = this.mapParent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].def == this.race && allPawnsSpawned[i].Faction == null)
					{
						num++;
					}
				}
				if (num < this.requiredCount)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalPawnsNotAvailable));
				}
			}
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0012F0D1 File Offset: 0x0012D2D1
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalDecrement)
			{
				this.requiredCount--;
			}
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0012F0FC File Offset: 0x0012D2FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.requiredCount, "requiredCount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDecrement, "inSignalChangeCount", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnsNotAvailable, "outSignalPawnsNotAvailable", null, false);
		}

		// Token: 0x0400217C RID: 8572
		public ThingDef race;

		// Token: 0x0400217D RID: 8573
		public int requiredCount;

		// Token: 0x0400217E RID: 8574
		public MapParent mapParent;

		// Token: 0x0400217F RID: 8575
		public string inSignalDecrement;

		// Token: 0x04002180 RID: 8576
		public string outSignalPawnsNotAvailable;

		// Token: 0x04002181 RID: 8577
		private const int CheckInterval = 500;
	}
}
