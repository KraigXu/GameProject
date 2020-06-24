using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Spirit
{
	public class GameInitData
	{

		public int startingTile = -1;

		public int mapSize = 250;


		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		public int startingPawnCount = -1;

		public Faction playerFaction;

		public Season startingSeason;

		public bool permadeathChosen;

	
		public bool permadeath;


		public bool startedFromEntry;


		public string gameToLoad;

		public const int DefaultMapSize = 250;

		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00023BFD File Offset: 0x00021DFD
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00023C0A File Offset: 0x00021E0A
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00023C30 File Offset: 0x00021E30
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"startedFromEntry: ",
				this.startedFromEntry.ToString(),
				"\nstartingAndOptionalPawns: ",
				this.startingAndOptionalPawns.Count
			});
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00023C70 File Offset: 0x00021E70
		public void PrepForMapGen()
		{
			while (this.startingAndOptionalPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingAndOptionalPawns[this.startingPawnCount]);
				Find.WorldPawns.PassToWorld(this.startingAndOptionalPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				this.startingAndOptionalPawns.RemoveAt(this.startingPawnCount);
			}
			List<Pawn> list = this.startingAndOptionalPawns;
			foreach (Pawn pawn in list)
			{
				pawn.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, false);
			}
			foreach (Pawn pawn2 in list)
			{
				pawn2.workSettings.DisableAll();
			}
			using (IEnumerator<WorkTypeDef> enumerator2 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					WorkTypeDef w = enumerator2.Current;
					if (w.alwaysStartActive)
					{
						IEnumerable<Pawn> source = list;
						Func<Pawn, bool> predicate;
						Func<Pawn, bool> <> 9__0;
						if ((predicate = <> 9__0) == null)
						{
							predicate = (<> 9__0 = ((Pawn col) => !col.WorkTypeIsDisabled(w)));
						}
						using (IEnumerator<Pawn> enumerator3 = source.Where(predicate).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn3 = enumerator3.Current;
								pawn3.workSettings.SetPriority(w, 3);
							}
							continue;
						}
					}
					bool flag = false;
					foreach (Pawn pawn4 in list)
					{
						if (!pawn4.WorkTypeIsDisabled(w) && pawn4.skills.AverageOfRelevantSkillsFor(w) >= 6f)
						{
							pawn4.workSettings.SetPriority(w, 3);
							flag = true;
						}
					}
					if (!flag)
					{
						IEnumerable<Pawn> source2 = from col in list
													where !col.WorkTypeIsDisabled(w)
													select col;
						if (source2.Any<Pawn>())
						{
							source2.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w)).workSettings.SetPriority(w, 3);
						}
					}
				}
			}
		}


	}
}
