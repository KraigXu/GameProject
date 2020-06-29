using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlayerPawnsArriveMethod>(ref this.method, "method", PlayerPawnsArriveMethod.Standing, false);
		}

		
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.method.ToStringHuman(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PlayerPawnsArriveMethod)))
				{
					PlayerPawnsArriveMethod localM2 = (PlayerPawnsArriveMethod)obj;
					PlayerPawnsArriveMethod localM = localM2;
					list.Add(new FloatMenuOption(localM.ToStringHuman(), delegate
					{
						this.method = localM;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		
		public override string Summary(Scenario scen)
		{
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				return "ScenPart_ArriveInDropPods".Translate();
			}
			return null;
		}

		
		public override void Randomize()
		{
			this.method = ((Rand.Value < 0.5f) ? PlayerPawnsArriveMethod.DropPods : PlayerPawnsArriveMethod.Standing);
		}

		
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			List<List<Thing>> list = new List<List<Thing>>();
			foreach (Pawn item in Find.GameInitData.startingAndOptionalPawns)
			{
				list.Add(new List<Thing>
				{
					item
				});
			}
			List<Thing> list2 = new List<Thing>();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				list2.AddRange(scenPart.PlayerStartingThings());
			}
			int num = 0;
			foreach (Thing thing in list2)
			{
				if (thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(Faction.OfPlayer);
				}
				list[num].Add(thing);
				num++;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
			DropPodUtility.DropThingGroupsNear_NewTmp(MapGenerator.PlayerStartSpot, map, list, 110, Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods, true, true, true, false);
		}

		
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
			}
		}

		
		private PlayerPawnsArriveMethod method;
	}
}
