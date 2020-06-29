using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class FactionManager : IExposable
	{
		
		
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		
		
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		
		
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.def.hidden
				select fa;
			}
		}

		
		
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		
		
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		
		
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		
		
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		
		
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		
		
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		
		
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		
		
		public Faction Empire
		{
			get
			{
				return this.empire;
			}
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.FactionManagerPostLoadInit();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.allFactions.RemoveAll((Faction x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some factions were null after loading.", false);
				}
				this.RecacheFactions();
			}
		}

		
		public void Add(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Add(faction);
			this.RecacheFactions();
		}

		
		public void Remove(Faction faction)
		{
			if (!this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Remove(faction);
			this.RecacheFactions();
		}

		
		public void FactionManagerTick()
		{
			SettlementProximityGoodwillUtility.CheckSettlementProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
		}

		
		public Faction FirstFactionOfDef(FactionDef facDef)
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].def == facDef)
				{
					return this.allFactions[i];
				}
			}
			return null;
		}

		
		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined)
		{
			return (from x in this.AllFactions
			where !x.IsPlayer && !x.def.hidden && x.def.humanlikeFaction && (allowDefeated || !x.defeated) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel)
			select x).TryRandomElementByWeight(delegate(Faction x)
			{
				if (tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval)
				{
					return 0.1f;
				}
				return 1f;
			}, out faction);
		}

		
		public IEnumerable<Faction> GetFactions(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			int num;
			for (int i = 0; i < this.allFactions.Count; i = num + 1)
			{
				Faction faction = this.allFactions[i];
				if (!faction.IsPlayer && (allowHidden || !faction.def.hidden) && (allowDefeated || !faction.defeated) && (allowNonHumanlike || faction.def.humanlikeFaction) && (minTechLevel == TechLevel.Undefined || faction.def.techLevel >= minTechLevel))
				{
					yield return faction;
				}
				num = i;
			}
			yield break;
		}

		
		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel)
			where x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		
		public Faction RandomNonHostileFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel)
			where !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		
		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel)
			where x.PlayerRelationKind == FactionRelationKind.Ally
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		
		public Faction RandomRoyalFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel)
			where x.def.HasRoyalTitles
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:", false);
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		
		private void RecacheFactions()
		{
			this.ofPlayer = null;
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].IsPlayer)
				{
					this.ofPlayer = this.allFactions[i];
					break;
				}
			}
			this.ofMechanoids = this.FirstFactionOfDef(FactionDefOf.Mechanoid);
			this.ofInsects = this.FirstFactionOfDef(FactionDefOf.Insect);
			this.ofAncients = this.FirstFactionOfDef(FactionDefOf.Ancients);
			this.ofAncientsHostile = this.FirstFactionOfDef(FactionDefOf.AncientsHostile);
			this.empire = this.FirstFactionOfDef(FactionDefOf.Empire);
		}

		
		private List<Faction> allFactions = new List<Faction>();

		
		private Faction ofPlayer;

		
		private Faction ofMechanoids;

		
		private Faction ofInsects;

		
		private Faction ofAncients;

		
		private Faction ofAncientsHostile;

		
		private Faction empire;
	}
}
