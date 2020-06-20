using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E09 RID: 3593
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x060056CD RID: 22221
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x060056CE RID: 22222 RVA: 0x001CC900 File Offset: 0x001CAB00
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

		// Token: 0x17000F7F RID: 3967
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

		// Token: 0x060056D0 RID: 22224 RVA: 0x001CCA4C File Offset: 0x001CAC4C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns);
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x001CCA5C File Offset: 0x001CAC5C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}

		// Token: 0x04002F49 RID: 12105
		protected string explanationKey;

		// Token: 0x04002F4A RID: 12106
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x04002F4B RID: 12107
		private List<Pawn> affectedPawnsResult = new List<Pawn>();
	}
}
