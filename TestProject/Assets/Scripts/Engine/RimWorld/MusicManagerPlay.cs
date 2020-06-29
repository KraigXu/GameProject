﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class MusicManagerPlay
	{
		
		// (get) Token: 0x06004A54 RID: 19028 RVA: 0x00191EFB File Offset: 0x001900FB
		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		
		// (get) Token: 0x06004A55 RID: 19029 RVA: 0x00191F04 File Offset: 0x00190104
		private bool DangerMusicMode
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		// (get) Token: 0x06004A56 RID: 19030 RVA: 0x00191F40 File Offset: 0x00190140
		private float CurVolume
		{
			get
			{
				float num = this.ignorePrefsVolumeThisSong ? 1f : Prefs.VolumeMusic;
				if (this.lastStartedSong == null)
				{
					return num;
				}
				return this.lastStartedSong.volume * num * this.fadeoutFactor * this.instrumentProximityFadeFactor;
			}
		}

		
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x00191F87 File Offset: 0x00190187
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		
		// (get) Token: 0x06004A58 RID: 19032 RVA: 0x00191F99 File Offset: 0x00190199
		public bool IsPlaying
		{
			get
			{
				return this.audioSource.isPlaying;
			}
		}

		
		public void ForceSilenceFor(float time)
		{
			this.nextSongStartTime = this.CurTime + time;
		}

		
		public void MusicUpdate()
		{
			if (!this.gameObjectCreated)
			{
				this.gameObjectCreated = true;
				this.audioSource = new GameObject("MusicAudioSourceDummy")
				{
					transform = 
					{
						parent = Find.Root.soundRoot.sourcePool.sourcePoolCamera.cameraSourcesContainer.transform
					}
				}.AddComponent<AudioSource>();
				this.audioSource.bypassEffects = true;
				this.audioSource.bypassListenerEffects = true;
				this.audioSource.bypassReverbZones = true;
				this.audioSource.priority = 0;
			}
			this.UpdateSubtleAmbienceSoundVolumeMultiplier();
			if (this.disabled)
			{
				return;
			}
			if (this.songWasForced)
			{
				this.state = MusicManagerPlay.MusicManagerState.Normal;
				this.fadeoutFactor = 1f;
			}
			if (this.audioSource.isPlaying && !this.songWasForced && ((this.DangerMusicMode && !this.lastStartedSong.tense) || (!this.DangerMusicMode && this.lastStartedSong.tense)))
			{
				this.state = MusicManagerPlay.MusicManagerState.Fadeout;
			}
			this.audioSource.volume = this.CurSanitizedVolume;
			if (!this.audioSource.isPlaying)
			{
				if (this.DangerMusicMode && this.nextSongStartTime > this.CurTime + MusicManagerPlay.SongIntervalTension.max)
				{
					this.nextSongStartTime = this.CurTime + MusicManagerPlay.SongIntervalTension.RandomInRange;
				}
				if (this.nextSongStartTime < this.CurTime - 5f)
				{
					float randomInRange;
					if (this.DangerMusicMode)
					{
						randomInRange = MusicManagerPlay.SongIntervalTension.RandomInRange;
					}
					else
					{
						randomInRange = MusicManagerPlay.SongIntervalRelax.RandomInRange;
					}
					this.nextSongStartTime = this.CurTime + randomInRange;
				}
				if (this.CurTime >= this.nextSongStartTime)
				{
					this.ignorePrefsVolumeThisSong = false;
					this.StartNewSong();
				}
				return;
			}
			if (this.state == MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.fadeoutFactor -= Time.deltaTime / 10f;
				if (this.fadeoutFactor <= 0f)
				{
					this.audioSource.Stop();
					this.state = MusicManagerPlay.MusicManagerState.Normal;
					this.fadeoutFactor = 1f;
				}
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap != null && !WorldRendererUtility.WorldRenderedNow)
			{
				float num = 1f;
				Camera camera = Find.Camera;
				List<Thing> list = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicalInstrument);
				for (int i = 0; i < list.Count; i++)
				{
					Building_MusicalInstrument building_MusicalInstrument = (Building_MusicalInstrument)list[i];
					if (building_MusicalInstrument.IsBeingPlayed)
					{
						Vector3 vector = camera.transform.position - building_MusicalInstrument.Position.ToVector3Shifted();
						vector.y = Mathf.Max(vector.y - 15f, 0f);
						vector.y *= 3.5f;
						float magnitude = vector.magnitude;
						FloatRange soundRange = building_MusicalInstrument.SoundRange;
						float num2 = Mathf.Min(Mathf.Max(magnitude - soundRange.min, 0f) / (soundRange.max - soundRange.min), 1f);
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
				this.instrumentProximityFadeFactor = num;
				return;
			}
			this.instrumentProximityFadeFactor = 1f;
		}

		
		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurSanitizedVolume > 0.001f)
			{
				this.subtleAmbienceSoundVolumeMultiplier -= Time.deltaTime * 0.1f;
			}
			else
			{
				this.subtleAmbienceSoundVolumeMultiplier += Time.deltaTime * 0.1f;
			}
			this.subtleAmbienceSoundVolumeMultiplier = Mathf.Clamp01(this.subtleAmbienceSoundVolumeMultiplier);
		}

		
		private void StartNewSong()
		{
			this.lastStartedSong = this.ChooseNextSong();
			this.audioSource.clip = this.lastStartedSong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
			this.recentSongs.Enqueue(this.lastStartedSong);
		}

		
		public void ForceStartSong(SongDef song, bool ignorePrefsVolume)
		{
			this.forcedNextSong = song;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.StartNewSong();
		}

		
		private SongDef ChooseNextSong()
		{
			this.songWasForced = false;
			if (this.forcedNextSong != null)
			{
				SongDef result = this.forcedNextSong;
				this.forcedNextSong = null;
				this.songWasForced = true;
				return result;
			}
			IEnumerable<SongDef> source = from song in DefDatabase<SongDef>.AllDefs
			where this.AppropriateNow(song)
			select song;
			while (this.recentSongs.Count > 7)
			{
				this.recentSongs.Dequeue();
			}
			while (!source.Any<SongDef>() && this.recentSongs.Count > 0)
			{
				this.recentSongs.Dequeue();
			}
			if (!source.Any<SongDef>())
			{
				Log.Error("Could not get any appropriate song. Getting random and logging song selection data.", false);
				this.SongSelectionData();
				return DefDatabase<SongDef>.GetRandom();
			}
			return source.RandomElementByWeight((SongDef s) => s.commonality);
		}

		
		private bool AppropriateNow(SongDef song)
		{
			if (!song.playOnMap)
			{
				return false;
			}
			if (this.DangerMusicMode)
			{
				if (!song.tense)
				{
					return false;
				}
			}
			else if (song.tense)
			{
				return false;
			}
			Map map = Find.AnyPlayerHomeMap ?? Find.CurrentMap;
			if (!song.allowedSeasons.NullOrEmpty<Season>())
			{
				if (map == null)
				{
					return false;
				}
				if (!song.allowedSeasons.Contains(GenLocalDate.Season(map)))
				{
					return false;
				}
			}
			if (song.minRoyalTitle != null && !PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Any((Pawn p) => p.royalty.AllTitlesForReading.Any<RoyalTitle>() && p.royalty.MostSeniorTitle.def.seniority >= song.minRoyalTitle.seniority && !p.IsQuestLodger()))
			{
				return false;
			}
			if (this.recentSongs.Contains(song))
			{
				return false;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Any)
			{
				return true;
			}
			if (map == null)
			{
				return true;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Night)
			{
				return GenLocalDate.DayPercent(map) < 0.2f || GenLocalDate.DayPercent(map) > 0.7f;
			}
			return GenLocalDate.DayPercent(map) > 0.2f && GenLocalDate.DayPercent(map) < 0.7f;
		}

		
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MusicManagerMap");
			stringBuilder.AppendLine("state: " + this.state);
			stringBuilder.AppendLine("lastStartedSong: " + this.lastStartedSong);
			stringBuilder.AppendLine("fadeoutFactor: " + this.fadeoutFactor);
			stringBuilder.AppendLine("nextSongStartTime: " + this.nextSongStartTime);
			stringBuilder.AppendLine("CurTime: " + this.CurTime);
			stringBuilder.AppendLine("recentSongs: " + (from s in this.recentSongs
			select s.defName).ToCommaList(true));
			stringBuilder.AppendLine("disabled: " + this.disabled.ToString());
			return stringBuilder.ToString();
		}

		
		[DebugOutput]
		public void SongSelectionData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Most recent song: " + ((this.lastStartedSong != null) ? this.lastStartedSong.defName : "None"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Songs appropriate to play now:");
			foreach (SongDef songDef in from s in DefDatabase<SongDef>.AllDefs
			where this.AppropriateNow(s)
			select s)
			{
				stringBuilder.AppendLine("   " + songDef.defName);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Recently played songs:");
			foreach (SongDef songDef2 in this.recentSongs)
			{
				stringBuilder.AppendLine("   " + songDef2.defName);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		
		private AudioSource audioSource;

		
		private MusicManagerPlay.MusicManagerState state;

		
		private float fadeoutFactor = 1f;

		
		private float nextSongStartTime = 12f;

		
		private float instrumentProximityFadeFactor = 1f;

		
		private SongDef lastStartedSong;

		
		private Queue<SongDef> recentSongs = new Queue<SongDef>();

		
		public bool disabled;

		
		private SongDef forcedNextSong;

		
		private bool songWasForced;

		
		private bool ignorePrefsVolumeThisSong;

		
		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		
		private bool gameObjectCreated;

		
		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		
		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		
		private const float FadeoutDuration = 10f;

		
		private enum MusicManagerState
		{
			
			Normal,
			
			Fadeout
		}
	}
}
