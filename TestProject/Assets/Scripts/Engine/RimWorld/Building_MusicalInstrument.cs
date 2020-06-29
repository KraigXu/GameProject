using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class Building_MusicalInstrument : Building
	{
		
		public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
		{
			return instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange && instrumentPos.GetRoom(map, RegionType.Set_Passable) == pawnPos.GetRoom(map, RegionType.Set_Passable);
		}

		
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x0019CB6F File Offset: 0x0019AD6F
		public bool IsBeingPlayed
		{
			get
			{
				return this.currentPlayer != null;
			}
		}

		
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

		
		public void StartPlaying(Pawn player)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Musical instruments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 19285, false);
				return;
			}
			this.currentPlayer = player;
		}

		
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

		
		public void StopPlaying()
		{
			this.currentPlayer = null;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentPlayer, "currentPlayer", false);
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Musical instruments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 19285, false);
				yield break;
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

		
		private Pawn currentPlayer;

		
		private Sustainer soundPlaying;
	}
}
