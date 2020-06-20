using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095A RID: 2394
	public class QuestPart_MoodBelow : QuestPartActivable
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x0012EC98 File Offset: 0x0012CE98
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.showAlert || this.minTicksBelowThreshold < 60)
				{
					return AlertReport.Inactive;
				}
				this.culpritsResult.Clear();
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.MoodBelowThreshold(this.pawns[i]))
					{
						this.culpritsResult.Add(this.pawns[i]);
					}
				}
				return AlertReport.CulpritsAre(this.culpritsResult);
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060038A5 RID: 14501 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool AlertCritical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060038A6 RID: 14502 RVA: 0x0012ED14 File Offset: 0x0012CF14
		public override string AlertLabel
		{
			get
			{
				return "QuestPartMoodBelowThreshold".Translate();
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060038A7 RID: 14503 RVA: 0x0012ED28 File Offset: 0x0012CF28
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartMoodBelowThresholdDesc".Translate(this.quest.name, GenLabel.ThingsLabel(this.pawns.Where(new Func<Pawn, bool>(this.MoodBelowThreshold)).Cast<Thing>(), "  - "));
			}
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x0012ED80 File Offset: 0x0012CF80
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			while (this.moodBelowThresholdTicks.Count < this.pawns.Count)
			{
				this.moodBelowThresholdTicks.Add(0);
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.MoodBelowThreshold(this.pawns[i]))
				{
					List<int> list = this.moodBelowThresholdTicks;
					int index = i;
					int num = list[index];
					list[index] = num + 1;
					if (this.moodBelowThresholdTicks[i] >= this.minTicksBelowThreshold)
					{
						base.Complete(this.pawns[i].Named("SUBJECT"));
						return;
					}
				}
				else
				{
					this.moodBelowThresholdTicks[i] = 0;
				}
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x0012EE3C File Offset: 0x0012D03C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.threshold, "threshold", 0f, false);
			Scribe_Values.Look<int>(ref this.minTicksBelowThreshold, "minTicksBelowThreshold", 0, false);
			Scribe_Values.Look<bool>(ref this.showAlert, "showAlert", true, false);
			Scribe_Collections.Look<int>(ref this.moodBelowThresholdTicks, "moodBelowThresholdTicks", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x0012EEE8 File Offset: 0x0012D0E8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.threshold = 0.5f;
				this.minTicksBelowThreshold = 2500;
			}
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x0012EF39 File Offset: 0x0012D139
		private bool MoodBelowThreshold(Pawn pawn)
		{
			return pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.CurLevelPercentage < this.threshold;
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x0012EF6A File Offset: 0x0012D16A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04002175 RID: 8565
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002176 RID: 8566
		public float threshold;

		// Token: 0x04002177 RID: 8567
		public int minTicksBelowThreshold;

		// Token: 0x04002178 RID: 8568
		public bool showAlert = true;

		// Token: 0x04002179 RID: 8569
		private List<int> moodBelowThresholdTicks = new List<int>();

		// Token: 0x0400217A RID: 8570
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
