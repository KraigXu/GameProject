using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9E RID: 2974
	public class Pawn_NeedsTracker : IExposable
	{
		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x060045A5 RID: 17829 RVA: 0x0017857E File Offset: 0x0017677E
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x00178586 File Offset: 0x00176786
		public Pawn_NeedsTracker()
		{
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x00178599 File Offset: 0x00176799
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x001785BC File Offset: 0x001767BC
		public void ExposeData()
		{
			Scribe_Collections.Look<Need>(ref this.needs, "needs", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.needs.RemoveAll((Need x) => x == null || x.def == null) != 0)
				{
					Log.Error("Pawn " + this.pawn.ToStringSafe<Pawn>() + " had some null needs after loading.", false);
				}
				this.BindDirectNeedFields();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x00178654 File Offset: 0x00176854
		private void BindDirectNeedFields()
		{
			this.mood = this.TryGetNeed<Need_Mood>();
			this.food = this.TryGetNeed<Need_Food>();
			this.rest = this.TryGetNeed<Need_Rest>();
			this.joy = this.TryGetNeed<Need_Joy>();
			this.beauty = this.TryGetNeed<Need_Beauty>();
			this.comfort = this.TryGetNeed<Need_Comfort>();
			this.roomsize = this.TryGetNeed<Need_RoomSize>();
			this.outdoors = this.TryGetNeed<Need_Outdoors>();
			this.drugsDesire = this.TryGetNeed<Need_Chemical_Any>();
			this.authority = null;
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x001786D4 File Offset: 0x001768D4
		public void NeedsTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(150))
			{
				for (int i = 0; i < this.needs.Count; i++)
				{
					this.needs[i].NeedInterval();
				}
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x0017871C File Offset: 0x0017691C
		public T TryGetNeed<T>() where T : Need
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].GetType() == typeof(T))
				{
					return (T)((object)this.needs[i]);
				}
			}
			return default(T);
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x0017877C File Offset: 0x0017697C
		public Need TryGetNeed(NeedDef def)
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].def == def)
				{
					return this.needs[i];
				}
			}
			return null;
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x001787C4 File Offset: 0x001769C4
		public void SetInitialLevels()
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x001787F8 File Offset: 0x001769F8
		public void AddOrRemoveNeedsAsAppropriate()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				try
				{
					NeedDef needDef = allDefsListForReading[i];
					if (this.ShouldHaveNeed(needDef))
					{
						if (this.TryGetNeed(needDef) == null)
						{
							this.AddNeed(needDef);
						}
					}
					else if (this.TryGetNeed(needDef) != null)
					{
						this.RemoveNeed(needDef);
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while determining if ",
						this.pawn.ToStringSafe<Pawn>(),
						" should have Need ",
						allDefsListForReading[i].ToStringSafe<NeedDef>(),
						": ",
						ex
					}), false);
				}
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x001788B8 File Offset: 0x00176AB8
		private bool ShouldHaveNeed(NeedDef nd)
		{
			if (this.pawn.RaceProps.intelligence < nd.minIntelligence)
			{
				return false;
			}
			if (nd.colonistsOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer))
			{
				return false;
			}
			if (nd.colonistAndPrisonersOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || this.pawn.HostFaction != Faction.OfPlayer))
			{
				return false;
			}
			if (this.pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.disablesNeed == nd))
			{
				return false;
			}
			if (nd.onlyIfCausedByHediff && !this.pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.causesNeed == nd))
			{
				return false;
			}
			if (nd.neverOnPrisoner && this.pawn.IsPrisoner)
			{
				return false;
			}
			if (nd.titleRequiredAny != null)
			{
				if (this.pawn.royalty == null)
				{
					return false;
				}
				bool flag = false;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
					if (nd.titleRequiredAny.Contains(royalTitle.def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (nd.hediffRequiredAny != null)
			{
				bool flag2 = false;
				foreach (HediffDef def in nd.hediffRequiredAny)
				{
					if (this.pawn.health.hediffSet.HasHediff(def, false))
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			if (nd.defName == "Authority")
			{
				return false;
			}
			if (nd == NeedDefOf.Food)
			{
				return this.pawn.RaceProps.EatsFood;
			}
			return nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x00178B44 File Offset: 0x00176D44
		private void AddNeed(NeedDef nd)
		{
			Need need = (Need)Activator.CreateInstance(nd.needClass, new object[]
			{
				this.pawn
			});
			need.def = nd;
			this.needs.Add(need);
			need.SetInitialLevel();
			this.BindDirectNeedFields();
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00178B90 File Offset: 0x00176D90
		private void RemoveNeed(NeedDef nd)
		{
			Need item = this.TryGetNeed(nd);
			this.needs.Remove(item);
			this.BindDirectNeedFields();
		}

		// Token: 0x0400280F RID: 10255
		private Pawn pawn;

		// Token: 0x04002810 RID: 10256
		private List<Need> needs = new List<Need>();

		// Token: 0x04002811 RID: 10257
		public Need_Mood mood;

		// Token: 0x04002812 RID: 10258
		public Need_Food food;

		// Token: 0x04002813 RID: 10259
		public Need_Rest rest;

		// Token: 0x04002814 RID: 10260
		public Need_Joy joy;

		// Token: 0x04002815 RID: 10261
		public Need_Beauty beauty;

		// Token: 0x04002816 RID: 10262
		public Need_RoomSize roomsize;

		// Token: 0x04002817 RID: 10263
		public Need_Outdoors outdoors;

		// Token: 0x04002818 RID: 10264
		public Need_Chemical_Any drugsDesire;

		// Token: 0x04002819 RID: 10265
		public Need_Comfort comfort;

		// Token: 0x0400281A RID: 10266
		public Need_Authority authority;
	}
}
