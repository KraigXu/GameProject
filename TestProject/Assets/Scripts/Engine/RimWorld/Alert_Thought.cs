using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public abstract class Alert_Thought : Alert
	{
		
		// (get) Token: 0x060056CD RID: 22221
		protected abstract ThoughtDef Thought { get; }

		
		public override string GetLabel()
		{
			int count = this.AffectedPawns.Count;
			string label = base.GetLabel();
			if (count > 1)
			{
				return string.Format("{0} x{1}", label, count.ToString());
			}
			return label;
		}

		
		// (get) Token: 0x060056CF RID: 22223 RVA: 0x001CC938 File Offset: 0x001CAB38
		private List<Pawn> AffectedPawns
		{
			get
			{
				this.affectedPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (pawn.Dead)
					{
						Log.Error("Dead pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists:" + pawn, false);
					}
					else if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
						try
						{
							ThoughtDef thought = this.Thought;
							for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
							{
								if (Alert_Thought.tmpThoughts[i].def == thought && !ThoughtUtility.ThoughtNullified(pawn, Alert_Thought.tmpThoughts[i].def))
								{
									this.affectedPawnsResult.Add(pawn);
								}
							}
						}
						finally
						{
							Alert_Thought.tmpThoughts.Clear();
						}
					}
				}
				return this.affectedPawnsResult;
			}
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns);
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}

		
		protected string explanationKey;

		
		private static List<Thought> tmpThoughts = new List<Thought>();

		
		private List<Pawn> affectedPawnsResult = new List<Pawn>();
	}
}
