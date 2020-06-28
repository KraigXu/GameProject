using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000975 RID: 2421
	public class QuestPart_EndGame : QuestPart
	{
		// Token: 0x0600395B RID: 14683 RVA: 0x00131334 File Offset: 0x0012F534
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !ShipCountdown.CountingDown)
			{
				if (!Find.TickManager.Paused)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
				}
				List<Pawn> list;
				if (!signal.args.TryGetArg<List<Pawn>>("SENTCOLONISTS", out list))
				{
					list = null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine("   " + list[i].LabelCap);
					}
					Find.StoryWatcher.statsRecord.colonistsLaunched += list.Count;
				}
				ShipCountdown.InitiateCountdown(GameVictoryUtility.MakeEndCredits(this.introText, this.endingText, stringBuilder.ToString()));
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].Destroyed)
						{
							list[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x00131434 File Offset: 0x0012F634
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.introText, "introText", null, false);
			Scribe_Values.Look<string>(ref this.endingText, "endingText", null, false);
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x00131472 File Offset: 0x0012F672
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x040021CC RID: 8652
		public string inSignal;

		// Token: 0x040021CD RID: 8653
		public string introText;

		// Token: 0x040021CE RID: 8654
		public string endingText;
	}
}
