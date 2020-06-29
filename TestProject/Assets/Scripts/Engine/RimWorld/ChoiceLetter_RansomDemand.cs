using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		
		// (get) Token: 0x06005B25 RID: 23333 RVA: 0x001F6029 File Offset: 0x001F4229
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
					yield break;
				}
				DiaOption diaOption = new DiaOption("RansomDemand_Accept".Translate());
				diaOption.action = delegate
				{
					this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
					Find.WorldPawns.RemovePawn(this.kidnapped);
					IntVec3 intVec;
					if (this.faction.def.techLevel < TechLevel.Industrial)
					{
						if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
						{
							Log.Warning("Could not find any edge cell.", false);
							intVec = DropCellFinder.TradeDropSpot(this.map);
						}
						GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
					}
					else
					{
						intVec = DropCellFinder.TradeDropSpot(this.map);
						TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
					}
					CameraJumper.TryJump(intVec, this.map);
					TradeUtility.LaunchSilver(this.map, this.fee);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					diaOption.Disable("NeedSilverLaunchable".Translate(this.fee.ToString()));
				}
				yield return diaOption;
				yield return base.Option_Reject;
				yield return base.Option_Postpone;
				yield break;
			}
		}

		
		// (get) Token: 0x06005B26 RID: 23334 RVA: 0x001F6039 File Offset: 0x001F4239
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}

		
		public Map map;

		
		public Faction faction;

		
		public Pawn kidnapped;

		
		public int fee;
	}
}
