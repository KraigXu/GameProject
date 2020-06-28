using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCA RID: 3018
	public sealed class SituationalThoughtHandler
	{
		// Token: 0x06004788 RID: 18312 RVA: 0x001844BC File Offset: 0x001826BC
		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0018450D File Offset: 0x0018270D
		public void SituationalThoughtInterval()
		{
			this.RemoveExpiredThoughtsFromCache();
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x00184518 File Offset: 0x00182718
		public void AppendMoodThoughts(List<Thought> outThoughts)
		{
			this.CheckRecalculateMoodThoughts();
			for (int i = 0; i < this.cachedThoughts.Count; i++)
			{
				Thought_Situational thought_Situational = this.cachedThoughts[i];
				if (thought_Situational.Active)
				{
					outThoughts.Add(thought_Situational);
				}
			}
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x00184560 File Offset: 0x00182760
		public void AppendSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.CheckRecalculateSocialThoughts(otherPawn);
			SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts = this.cachedSocialThoughts[otherPawn];
			cachedSocialThoughts.lastQueryTick = Find.TickManager.TicksGame;
			List<Thought_SituationalSocial> activeThoughts = cachedSocialThoughts.activeThoughts;
			for (int i = 0; i < activeThoughts.Count; i++)
			{
				outThoughts.Add(activeThoughts[i]);
			}
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x001845B4 File Offset: 0x001827B4
		private void CheckRecalculateMoodThoughts()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastMoodThoughtsRecalculationTick < 100)
			{
				return;
			}
			this.lastMoodThoughtsRecalculationTick = ticksGame;
			try
			{
				this.tmpCachedThoughts.Clear();
				for (int i = 0; i < this.cachedThoughts.Count; i++)
				{
					this.cachedThoughts[i].RecalculateState();
					this.tmpCachedThoughts.Add(this.cachedThoughts[i].def);
				}
				List<ThoughtDef> situationalNonSocialThoughtDefs = ThoughtUtility.situationalNonSocialThoughtDefs;
				int j = 0;
				int count = situationalNonSocialThoughtDefs.Count;
				while (j < count)
				{
					if (!this.tmpCachedThoughts.Contains(situationalNonSocialThoughtDefs[j]))
					{
						Thought_Situational thought_Situational = this.TryCreateThought(situationalNonSocialThoughtDefs[j]);
						if (thought_Situational != null)
						{
							this.cachedThoughts.Add(thought_Situational);
						}
					}
					j++;
				}
			}
			finally
			{
			}
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x00184694 File Offset: 0x00182894
		private void CheckRecalculateSocialThoughts(Pawn otherPawn)
		{
			try
			{
				SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts;
				if (!this.cachedSocialThoughts.TryGetValue(otherPawn, out cachedSocialThoughts))
				{
					cachedSocialThoughts = new SituationalThoughtHandler.CachedSocialThoughts();
					this.cachedSocialThoughts.Add(otherPawn, cachedSocialThoughts);
				}
				if (cachedSocialThoughts.ShouldRecalculateState)
				{
					cachedSocialThoughts.lastRecalculationTick = Find.TickManager.TicksGame;
					this.tmpCachedSocialThoughts.Clear();
					for (int i = 0; i < cachedSocialThoughts.thoughts.Count; i++)
					{
						Thought_SituationalSocial thought_SituationalSocial = cachedSocialThoughts.thoughts[i];
						thought_SituationalSocial.RecalculateState();
						this.tmpCachedSocialThoughts.Add(thought_SituationalSocial.def);
					}
					List<ThoughtDef> situationalSocialThoughtDefs = ThoughtUtility.situationalSocialThoughtDefs;
					int j = 0;
					int count = situationalSocialThoughtDefs.Count;
					while (j < count)
					{
						if (!this.tmpCachedSocialThoughts.Contains(situationalSocialThoughtDefs[j]))
						{
							Thought_SituationalSocial thought_SituationalSocial2 = this.TryCreateSocialThought(situationalSocialThoughtDefs[j], otherPawn);
							if (thought_SituationalSocial2 != null)
							{
								cachedSocialThoughts.thoughts.Add(thought_SituationalSocial2);
							}
						}
						j++;
					}
					cachedSocialThoughts.activeThoughts.Clear();
					for (int k = 0; k < cachedSocialThoughts.thoughts.Count; k++)
					{
						Thought_SituationalSocial thought_SituationalSocial3 = cachedSocialThoughts.thoughts[k];
						if (thought_SituationalSocial3.Active)
						{
							cachedSocialThoughts.activeThoughts.Add(thought_SituationalSocial3);
						}
					}
				}
			}
			finally
			{
			}
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x001847E8 File Offset: 0x001829E8
		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				thought_Situational.RecalculateState();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while recalculating ",
					def,
					" thought state for pawn ",
					this.pawn,
					": ",
					ex
				}), false);
			}
			return thought_Situational;
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0018489C File Offset: 0x00182A9C
		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def))
				{
					return null;
				}
				if (!def.Worker.CurrentSocialState(this.pawn, otherPawn).ActiveFor(def))
				{
					return null;
				}
				thought_SituationalSocial = (Thought_SituationalSocial)ThoughtMaker.MakeThought(def);
				thought_SituationalSocial.pawn = this.pawn;
				thought_SituationalSocial.otherPawn = otherPawn;
				thought_SituationalSocial.RecalculateState();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while recalculating ",
					def,
					" thought state for pawn ",
					this.pawn,
					": ",
					ex
				}), false);
			}
			return thought_SituationalSocial;
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x00184958 File Offset: 0x00182B58
		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
			this.lastMoodThoughtsRecalculationTick = -99999;
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0018497B File Offset: 0x00182B7B
		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, SituationalThoughtHandler.CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}

		// Token: 0x0400291D RID: 10525
		public Pawn pawn;

		// Token: 0x0400291E RID: 10526
		private List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		// Token: 0x0400291F RID: 10527
		private int lastMoodThoughtsRecalculationTick = -99999;

		// Token: 0x04002920 RID: 10528
		private Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts>();

		// Token: 0x04002921 RID: 10529
		private const int RecalculateStateEveryTicks = 100;

		// Token: 0x04002922 RID: 10530
		private HashSet<ThoughtDef> tmpCachedThoughts = new HashSet<ThoughtDef>();

		// Token: 0x04002923 RID: 10531
		private HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		// Token: 0x02001B41 RID: 6977
		private class CachedSocialThoughts
		{
			// Token: 0x1700181D RID: 6173
			// (get) Token: 0x06009ADD RID: 39645 RVA: 0x002F3B4E File Offset: 0x002F1D4E
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			// Token: 0x1700181E RID: 6174
			// (get) Token: 0x06009ADE RID: 39646 RVA: 0x002F3B6B File Offset: 0x002F1D6B
			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}

			// Token: 0x04006753 RID: 26451
			public List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04006754 RID: 26452
			public List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04006755 RID: 26453
			public int lastRecalculationTick = -99999;

			// Token: 0x04006756 RID: 26454
			public int lastQueryTick = -99999;

			// Token: 0x04006757 RID: 26455
			private const int ExpireAfterTicks = 300;
		}
	}
}
