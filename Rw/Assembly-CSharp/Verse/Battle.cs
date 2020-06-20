using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000103 RID: 259
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000201AC File Offset: 0x0001E3AC
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000201B9 File Offset: 0x0001E3B9
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000201C1 File Offset: 0x0001E3C1
		public int LastEntryTimestamp
		{
			get
			{
				if (this.entries.Count <= 0)
				{
					return 0;
				}
				return this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000201F0 File Offset: 0x0001E3F0
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x000201F8 File Offset: 0x0001E3F8
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002021E File Offset: 0x0001E41E
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00020248 File Offset: 0x0001E448
		public string GetName()
		{
			if (this.battleName.NullOrEmpty())
			{
				HashSet<Faction> hashSet = new HashSet<Faction>(from p in this.concerns
				select p.Faction);
				GrammarRequest request = default(GrammarRequest);
				if (this.concerns.Count == 1)
				{
					if (hashSet.Count((Faction f) => f != null) < 2)
					{
						request.Includes.Add(RulePackDefOf.Battle_Solo);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
						goto IL_1C4;
					}
				}
				if (this.concerns.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_Duel);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT2", this.concerns.Last<Pawn>(), null, true, true));
				}
				else if (hashSet.Count == 1)
				{
					request.Includes.Add(RulePackDefOf.Battle_Internal);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), true));
				}
				else if (hashSet.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_War);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), true));
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION2", hashSet.Last<Faction>(), true));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.Battle_Brawl);
				}
				IL_1C4:
				this.battleName = GrammarResolver.Resolve("r_battlename", request, null, false, null, null, null, true);
			}
			return this.battleName;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00020438 File Offset: 0x0001E638
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			foreach (Thing thing in entry.GetConcerns())
			{
				if (thing is Pawn)
				{
					this.concerns.Add(thing as Pawn);
				}
			}
			this.battleName = null;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000204AC File Offset: 0x0001E6AC
		public void Absorb(Battle battle)
		{
			this.creationTimestamp = Mathf.Min(this.creationTimestamp, battle.creationTimestamp);
			this.entries.AddRange(battle.entries);
			this.concerns.AddRange(battle.concerns);
			this.entries = (from e in this.entries
			orderby e.Age
			select e).ToList<LogEntry>();
			battle.entries.Clear();
			battle.concerns.Clear();
			battle.absorbedBy = this;
			this.battleName = null;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002054B File Offset: 0x0001E74B
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002055C File Offset: 0x0001E75C
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			if (!this.concerns.Contains(p))
			{
				return;
			}
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
							", but he is referenced by a battle log entry ",
							this.entries[i],
							"."
						}), false);
					}
					this.entries.RemoveAt(i);
				}
			}
			this.concerns.Remove(p);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000205FC File Offset: 0x0001E7FC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.creationTimestamp, "creationTimestamp", 0, false);
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.absorbedBy, "absorbedBy", false);
			Scribe_Values.Look<string>(ref this.battleName, "battleName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.concerns.Clear();
				foreach (Pawn item in this.entries.SelectMany((LogEntry e) => e.GetConcerns()).OfType<Pawn>())
				{
					this.concerns.Add(item);
				}
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000206E8 File Offset: 0x0001E8E8
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID;
		}

		// Token: 0x0400068C RID: 1676
		public const int TicksForBattleExit = 5000;

		// Token: 0x0400068D RID: 1677
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x0400068E RID: 1678
		private string battleName;

		// Token: 0x0400068F RID: 1679
		private Battle absorbedBy;

		// Token: 0x04000690 RID: 1680
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x04000691 RID: 1681
		private int loadID;

		// Token: 0x04000692 RID: 1682
		private int creationTimestamp;
	}
}
