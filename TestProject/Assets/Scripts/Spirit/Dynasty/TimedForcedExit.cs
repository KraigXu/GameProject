using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200127E RID: 4734
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06006EFD RID: 28413 RVA: 0x0026AADA File Offset: 0x00268CDA
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06006EFE RID: 28414 RVA: 0x0026AAE8 File Offset: 0x00268CE8
		public string ForceExitAndRemoveMapCountdownTimeLeftString
		{
			get
			{
				if (!this.ForceExitAndRemoveMapCountdownActive)
				{
					return "";
				}
				return TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(this.ticksLeftToForceExitAndRemoveMap);
			}
		}

		// Token: 0x06006EFF RID: 28415 RVA: 0x0026AB03 File Offset: 0x00268D03
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06006F00 RID: 28416 RVA: 0x0026AB1D File Offset: 0x00268D1D
		public void ResetForceExitAndRemoveMapCountdown()
		{
			this.ticksLeftToForceExitAndRemoveMap = -1;
		}

		// Token: 0x06006F01 RID: 28417 RVA: 0x0026AB26 File Offset: 0x00268D26
		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		// Token: 0x06006F02 RID: 28418 RVA: 0x0026AB33 File Offset: 0x00268D33
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06006F03 RID: 28419 RVA: 0x0026AB3C File Offset: 0x00268D3C
		public override string CompInspectStringExtra()
		{
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				return "ForceExitAndRemoveMapCountdown".Translate(this.ForceExitAndRemoveMapCountdownTimeLeftString) + ".";
			}
			return null;
		}

		// Token: 0x06006F04 RID: 28420 RVA: 0x0026AB6C File Offset: 0x00268D6C
		public override void CompTick()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (mapParent.HasMap)
				{
					this.ticksLeftToForceExitAndRemoveMap--;
					if (this.ticksLeftToForceExitAndRemoveMap <= 0)
					{
						TimedForcedExit.ForceReform(mapParent);
						return;
					}
				}
				else
				{
					this.ticksLeftToForceExitAndRemoveMap = -1;
				}
			}
		}

		// Token: 0x06006F05 RID: 28421 RVA: 0x0026ABBA File Offset: 0x00268DBA
		public static string GetForceExitAndRemoveMapCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true);
		}

		// Token: 0x06006F06 RID: 28422 RVA: 0x0026ABD0 File Offset: 0x00268DD0
		public static void ForceReform(MapParent mapParent)
		{
			if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
			{
				Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
				Current.Game.CurrentMap = mapParent.Map;
				Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate
				{
					if (mapParent.HasMap)
					{
						mapParent.Destroy();
					}
				}, true);
				Find.WindowStack.Add(window);
				return;
			}
			TimedForcedExit.tmpPawns.Clear();
			TimedForcedExit.tmpPawns.AddRange(from x in mapParent.Map.mapPawns.AllPawns
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			if (TimedForcedExit.tmpPawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				CaravanExitMapUtility.ExitMapAndCreateCaravan(TimedForcedExit.tmpPawns, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, -1, true);
			}
			TimedForcedExit.tmpPawns.Clear();
			mapParent.Destroy();
		}

		// Token: 0x0400444C RID: 17484
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x0400444D RID: 17485
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
