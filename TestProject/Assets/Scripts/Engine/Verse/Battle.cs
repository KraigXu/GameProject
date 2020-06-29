using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	
	public class Battle : IExposable, ILoadReferenceable
	{
		
		
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		
		
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		
		
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

		
		
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		
		
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		
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

		
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		
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

		
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID;
		}

		
		public const int TicksForBattleExit = 5000;

		
		private List<LogEntry> entries = new List<LogEntry>();

		
		private string battleName;

		
		private Battle absorbedBy;

		
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		
		private int loadID;

		
		private int creationTimestamp;
	}
}
