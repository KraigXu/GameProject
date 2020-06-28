﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C7F RID: 3199
	public class Building_MusicalInstrument : Building
	{
		// Token: 0x06004CED RID: 19693 RVA: 0x0019CB45 File Offset: 0x0019AD45
		public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
		{
			return instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange && instrumentPos.GetRoom(map, RegionType.Set_Passable) == pawnPos.GetRoom(map, RegionType.Set_Passable);
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x0019CB6F File Offset: 0x0019AD6F
		public bool IsBeingPlayed
		{
			get
			{
				return this.currentPlayer != null;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004CEF RID: 19695 RVA: 0x0019CB7C File Offset: 0x0019AD7C
		public FloatRange SoundRange
		{
			get
			{
				if (this.soundPlaying == null)
				{
					return FloatRange.Zero;
				}
				if (this.soundPlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
				{
					return FloatRange.Zero;
				}
				return this.soundPlaying.def.subSounds.First<SubSoundDef>().distRange;
			}
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x0019CBCE File Offset: 0x0019ADCE
		public void StartPlaying(Pawn player)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Musical instruments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 19285, false);
				return;
			}
			this.currentPlayer = player;
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0019CBF0 File Offset: 0x0019ADF0
		public override void Tick()
		{
			base.Tick();
			if (this.currentPlayer != null)
			{
				if (this.def.soundPlayInstrument != null && this.soundPlaying == null)
				{
					this.soundPlaying = this.def.soundPlayInstrument.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick));
				}
			}
			else
			{
				this.soundPlaying = null;
			}
			if (this.soundPlaying != null)
			{
				this.soundPlaying.Maintain();
			}
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x0019CC6A File Offset: 0x0019AE6A
		public void StopPlaying()
		{
			this.currentPlayer = null;
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x0019CC73 File Offset: 0x0019AE73
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentPlayer, "currentPlayer", false);
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0019CC8C File Offset: 0x0019AE8C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Musical instruments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 19285, false);
				yield break;
			}
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Toggle is playing",
					action = delegate
					{
						this.currentPlayer = ((this.currentPlayer == null) ? PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>() : null);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04002B1F RID: 11039
		private Pawn currentPlayer;

		// Token: 0x04002B20 RID: 11040
		private Sustainer soundPlaying;
	}
}
