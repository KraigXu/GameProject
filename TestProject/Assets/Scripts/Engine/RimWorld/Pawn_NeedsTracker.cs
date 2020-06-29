using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Pawn_NeedsTracker : IExposable
	{
		
		// (get) Token: 0x060045A5 RID: 17829 RVA: 0x0017857E File Offset: 0x0017677E
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		
		public Pawn_NeedsTracker()
		{
		}

		
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		
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

		
		public void SetInitialLevels()
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		
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

		
		private void RemoveNeed(NeedDef nd)
		{
			Need item = this.TryGetNeed(nd);
			this.needs.Remove(item);
			this.BindDirectNeedFields();
		}

		
		private Pawn pawn;

		
		private List<Need> needs = new List<Need>();

		
		public Need_Mood mood;

		
		public Need_Food food;

		
		public Need_Rest rest;

		
		public Need_Joy joy;

		
		public Need_Beauty beauty;

		
		public Need_RoomSize roomsize;

		
		public Need_Outdoors outdoors;

		
		public Need_Chemical_Any drugsDesire;

		
		public Need_Comfort comfort;

		
		public Need_Authority authority;
	}
}
