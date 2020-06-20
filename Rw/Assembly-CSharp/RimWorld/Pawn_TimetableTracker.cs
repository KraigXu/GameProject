using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB5 RID: 2997
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060046C4 RID: 18116 RVA: 0x0017EF2C File Offset: 0x0017D12C
		public TimeAssignmentDef CurrentAssignment
		{
			get
			{
				if (!this.pawn.IsColonist)
				{
					return TimeAssignmentDefOf.Anything;
				}
				return this.times[GenLocalDate.HourOfDay(this.pawn)];
			}
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x0017EF58 File Offset: 0x0017D158
		public Pawn_TimetableTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.times = new List<TimeAssignmentDef>(24);
			for (int i = 0; i < 24; i++)
			{
				TimeAssignmentDef item;
				if (i <= 5 || i > 21)
				{
					item = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					item = TimeAssignmentDefOf.Anything;
				}
				this.times.Add(item);
			}
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0017EFB0 File Offset: 0x0017D1B0
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.times.Count; i++)
				{
					if (this.times[i] == TimeAssignmentDefOf.Meditate)
					{
						this.times[i] = TimeAssignmentDefOf.Anything;
					}
				}
			}
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x0017F01C File Offset: 0x0017D21C
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0017F02A File Offset: 0x0017D22A
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x040028A1 RID: 10401
		private Pawn pawn;

		// Token: 0x040028A2 RID: 10402
		public List<TimeAssignmentDef> times;
	}
}
