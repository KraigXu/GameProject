using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEB RID: 3051
	public class FactionManager : IExposable
	{
		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004886 RID: 18566 RVA: 0x0018AD26 File Offset: 0x00188F26
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004887 RID: 18567 RVA: 0x0018AD26 File Offset: 0x00188F26
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004888 RID: 18568 RVA: 0x0018AD2E File Offset: 0x00188F2E
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.def.hidden
				select fa;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x0018AD5A File Offset: 0x00188F5A
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x0600488A RID: 18570 RVA: 0x0018AD67 File Offset: 0x00188F67
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x0600488B RID: 18571 RVA: 0x0018AD74 File Offset: 0x00188F74
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x0600488C RID: 18572 RVA: 0x0018AD7C File Offset: 0x00188F7C
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x0018AD84 File Offset: 0x00188F84
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x0600488E RID: 18574 RVA: 0x0018AD8C File Offset: 0x00188F8C
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x0018AD94 File Offset: 0x00188F94
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004890 RID: 18576 RVA: 0x0018AD9C File Offset: 0x00188F9C
		public Faction Empire
		{
			get
			{
				return this.empire;
			}
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x0018ADA4 File Offset: 0x00188FA4
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

		// Token: 0x06004892 RID: 18578 RVA: 0x0018AE29 File Offset: 0x00189029
		public void Add(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Add(faction);
			this.RecacheFactions();
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x0018AE4C File Offset: 0x0018904C
		public void Remove(Faction faction)
		{
			if (!this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Remove(faction);
			this.RecacheFactions();
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x0018AE70 File Offset: 0x00189070
		public void FactionManagerTick()
		{
			SettlementProximityGoodwillUtility.CheckSettlementProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x0018AEAC File Offset: 0x001890AC
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

		// Token: 0x06004896 RID: 18582 RVA: 0x0018AEF4 File Offset: 0x001890F4
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

		// Token: 0x06004897 RID: 18583 RVA: 0x0018AF46 File Offset: 0x00189146
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

		// Token: 0x06004898 RID: 18584 RVA: 0x0018AF74 File Offset: 0x00189174
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

		// Token: 0x06004899 RID: 18585 RVA: 0x0018AFBC File Offset: 0x001891BC
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

		// Token: 0x0600489A RID: 18586 RVA: 0x0018B004 File Offset: 0x00189204
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

		// Token: 0x0600489B RID: 18587 RVA: 0x0018B04C File Offset: 0x0018924C
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

		// Token: 0x0600489C RID: 18588 RVA: 0x0018B094 File Offset: 0x00189294
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:", false);
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x0018B0D8 File Offset: 0x001892D8
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x0018B130 File Offset: 0x00189330
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

		// Token: 0x04002993 RID: 10643
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x04002994 RID: 10644
		private Faction ofPlayer;

		// Token: 0x04002995 RID: 10645
		private Faction ofMechanoids;

		// Token: 0x04002996 RID: 10646
		private Faction ofInsects;

		// Token: 0x04002997 RID: 10647
		private Faction ofAncients;

		// Token: 0x04002998 RID: 10648
		private Faction ofAncientsHostile;

		// Token: 0x04002999 RID: 10649
		private Faction empire;
	}
}
