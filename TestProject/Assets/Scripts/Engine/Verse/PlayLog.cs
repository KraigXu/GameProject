using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000116 RID: 278
	public class PlayLog : IExposable
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00024335 File Offset: 0x00022535
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0002433D File Offset: 0x0002253D
		public int LastTick
		{
			get
			{
				if (this.entries.Count == 0)
				{
					return 0;
				}
				return this.entries[0].Tick;
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002435F File Offset: 0x0002255F
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00024374 File Offset: 0x00022574
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000243A8 File Offset: 0x000225A8
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000243C0 File Offset: 0x000225C0
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by a play log entry ",
							this.entries[i],
							"."
						}), false);
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002444A File Offset: 0x0002264A
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002445C File Offset: 0x0002265C
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000708 RID: 1800
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000709 RID: 1801
		private const int Capacity = 150;
	}
}
