using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_MoodBelow : QuestPartActivable
	{
		
		
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

		
		
		public override bool AlertCritical
		{
			get
			{
				return true;
			}
		}

		
		
		public override string AlertLabel
		{
			get
			{
				return "QuestPartMoodBelowThreshold".Translate();
			}
		}

		
		
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartMoodBelowThresholdDesc".Translate(this.quest.name, GenLabel.ThingsLabel(this.pawns.Where(new Func<Pawn, bool>(this.MoodBelowThreshold)).Cast<Thing>(), "  - "));
			}
		}

		
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

		
		private bool MoodBelowThreshold(Pawn pawn)
		{
			return pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.CurLevelPercentage < this.threshold;
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public List<Pawn> pawns = new List<Pawn>();

		
		public float threshold;

		
		public int minTicksBelowThreshold;

		
		public bool showAlert = true;

		
		private List<int> moodBelowThresholdTicks = new List<int>();

		
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
