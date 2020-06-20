using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001250 RID: 4688
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06006D53 RID: 27987 RVA: 0x00264438 File Offset: 0x00262638
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x00264440 File Offset: 0x00262640
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x0026445A File Offset: 0x0026265A
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06006D56 RID: 27990 RVA: 0x00264477 File Offset: 0x00262677
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x06006D57 RID: 27991 RVA: 0x00264490 File Offset: 0x00262690
		private void CheckWonBattle()
		{
			if (this.wonBattle)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false))
			{
				return;
			}
			string forceExitAndRemoveMapCountdownTimeLeftString = TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(60000);
			Find.LetterStack.ReceiveLetter("LetterLabelCaravansBattlefieldVictory".Translate(), "LetterCaravansBattlefieldVictory".Translate(forceExitAndRemoveMapCountdownTimeLeftString), LetterDefOf.PositiveEvent, this, null, null, null, null);
			TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushDefeated, new object[]
			{
				base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
			this.wonBattle = true;
			base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown();
		}

		// Token: 0x040043DD RID: 17373
		private bool wonBattle;
	}
}
