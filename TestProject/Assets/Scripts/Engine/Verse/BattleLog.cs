using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000104 RID: 260
	public class BattleLog : IExposable
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x000206FF File Offset: 0x0001E8FF
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00020708 File Offset: 0x0001E908
		public void Add(LogEntry entry)
		{
			Battle battle = null;
			foreach (Thing thing in entry.GetConcerns())
			{
				Battle battleActive = ((Pawn)thing).records.BattleActive;
				if (battle == null)
				{
					battle = battleActive;
				}
				else if (battleActive != null)
				{
					battle = ((battle.Importance > battleActive.Importance) ? battle : battleActive);
				}
			}
			if (battle == null)
			{
				battle = Battle.Create();
				this.battles.Insert(0, battle);
			}
			foreach (Thing thing2 in entry.GetConcerns())
			{
				Pawn pawn = (Pawn)thing2;
				Battle battleActive2 = pawn.records.BattleActive;
				if (battleActive2 != null && battleActive2 != battle)
				{
					battle.Absorb(battleActive2);
					this.battles.Remove(battleActive2);
				}
				pawn.records.EnterBattle(battle);
			}
			battle.Add(entry);
			this.activeEntries = null;
			this.ReduceToCapacity();
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00020814 File Offset: 0x0001EA14
		private void ReduceToCapacity()
		{
			int num = this.battles.Count((Battle btl) => btl.AbsorbedBy == null);
			while (num > 20 && this.battles[this.battles.Count - 1].LastEntryTimestamp + Mathf.Max(420000, 5000) < Find.TickManager.TicksGame)
			{
				if (this.battles[this.battles.Count - 1].AbsorbedBy == null)
				{
					num--;
				}
				this.battles.RemoveAt(this.battles.Count - 1);
				this.activeEntries = null;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000208CE File Offset: 0x0001EACE
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00020904 File Offset: 0x0001EB04
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.battles.Count; i++)
			{
				if (this.battles[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00020940 File Offset: 0x0001EB40
		public bool IsEntryActive(LogEntry log)
		{
			if (this.activeEntries == null)
			{
				this.activeEntries = new HashSet<LogEntry>();
				for (int i = 0; i < this.battles.Count; i++)
				{
					List<LogEntry> entries = this.battles[i].Entries;
					for (int j = 0; j < entries.Count; j++)
					{
						this.activeEntries.Add(entries[j]);
					}
				}
			}
			return this.activeEntries.Contains(log);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000209B8 File Offset: 0x0001EBB8
		public void RemoveEntry(LogEntry log)
		{
			int num = 0;
			while (num < this.battles.Count && !this.battles[num].Entries.Remove(log))
			{
				num++;
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000209F4 File Offset: 0x0001EBF4
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
		}

		// Token: 0x04000693 RID: 1683
		private List<Battle> battles = new List<Battle>();

		// Token: 0x04000694 RID: 1684
		private const int BattleHistoryLength = 20;

		// Token: 0x04000695 RID: 1685
		private HashSet<LogEntry> activeEntries;
	}
}
