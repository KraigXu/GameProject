using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200011B RID: 283
	public sealed class TickManager : IExposable
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00024D31 File Offset: 0x00022F31
		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00024D39 File Offset: 0x00022F39
		public int TicksAbs
		{
			get
			{
				if (this.gameStartAbsTick == 0)
				{
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet (you most likely want to use GenTicks.TicksAbs instead).", 1049580013, false);
					return this.ticksGameInt;
				}
				return this.ticksGameInt + this.gameStartAbsTick;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00024D67 File Offset: 0x00022F67
		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00024D70 File Offset: 0x00022F70
		public float TickRateMultiplier
		{
			get
			{
				if (this.slower.ForcedNormalSpeed)
				{
					if (this.curTimeSpeed == TimeSpeed.Paused)
					{
						return 0f;
					}
					return 1f;
				}
				else
				{
					switch (this.curTimeSpeed)
					{
					case TimeSpeed.Paused:
						return 0f;
					case TimeSpeed.Normal:
						return 1f;
					case TimeSpeed.Fast:
						return 3f;
					case TimeSpeed.Superfast:
						if (Find.Maps.Count == 0)
						{
							return 120f;
						}
						if (this.NothingHappeningInGame())
						{
							return 12f;
						}
						return 6f;
					case TimeSpeed.Ultrafast:
						if (Find.Maps.Count == 0)
						{
							return 150f;
						}
						return 15f;
					default:
						return -1f;
					}
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00024E16 File Offset: 0x00023016
		private float CurTimePerTick
		{
			get
			{
				if (this.TickRateMultiplier == 0f)
				{
					return 0f;
				}
				return 1f / (60f * this.TickRateMultiplier);
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00024E3D File Offset: 0x0002303D
		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || Find.WindowStack.WindowsForcePause || LongEventHandler.ForcePause;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00024E5A File Offset: 0x0002305A
		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x00024E70 File Offset: 0x00023070
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x00024E78 File Offset: 0x00023078
		public TimeSpeed CurTimeSpeed
		{
			get
			{
				return this.curTimeSpeed;
			}
			set
			{
				this.curTimeSpeed = value;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00024E81 File Offset: 0x00023081
		public void TogglePaused()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.prePauseTimeSpeed = this.curTimeSpeed;
				this.curTimeSpeed = TimeSpeed.Paused;
				return;
			}
			if (this.prePauseTimeSpeed != this.curTimeSpeed)
			{
				this.curTimeSpeed = this.prePauseTimeSpeed;
				return;
			}
			this.curTimeSpeed = TimeSpeed.Normal;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00024EC1 File Offset: 0x000230C1
		public void Pause()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.TogglePaused();
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00024ED4 File Offset: 0x000230D4
		private bool NothingHappeningInGame()
		{
			if (this.lastNothingHappeningCheckTick != this.TicksGame)
			{
				this.nothingHappeningCached = true;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list.Count; j++)
					{
						Pawn pawn = list[j];
						if (pawn.HostFaction == null && pawn.RaceProps.Humanlike && pawn.Awake())
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
					if (!this.nothingHappeningCached)
					{
						break;
					}
				}
				if (this.nothingHappeningCached)
				{
					for (int k = 0; k < maps.Count; k++)
					{
						if (maps[k].IsPlayerHome && maps[k].dangerWatcher.DangerRating >= StoryDanger.Low)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
				}
				this.lastNothingHappeningCheckTick = this.TicksGame;
			}
			return this.nothingHappeningCached;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00024FCE File Offset: 0x000231CE
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00025008 File Offset: 0x00023208
		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00025028 File Offset: 0x00023228
		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00025048 File Offset: 0x00023248
		private TickList TickListFor(Thing t)
		{
			switch (t.def.tickerType)
			{
			case TickerType.Never:
				return null;
			case TickerType.Normal:
				return this.tickListNormal;
			case TickerType.Rare:
				return this.tickListRare;
			case TickerType.Long:
				return this.tickListLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00025098 File Offset: 0x00023298
		public void TickManagerUpdate()
		{
			if (!this.Paused)
			{
				float curTimePerTick = this.CurTimePerTick;
				if (Mathf.Abs(Time.deltaTime - curTimePerTick) < curTimePerTick * 0.1f)
				{
					this.realTimeToTickThrough += curTimePerTick;
				}
				else
				{
					this.realTimeToTickThrough += Time.deltaTime;
				}
				int num = 0;
				float tickRateMultiplier = this.TickRateMultiplier;
				this.clock.Reset();
				this.clock.Start();
				while (this.realTimeToTickThrough > 0f && (float)num < tickRateMultiplier * 2f)
				{
					this.DoSingleTick();
					this.realTimeToTickThrough -= curTimePerTick;
					num++;
					if (this.Paused || (float)this.clock.ElapsedMilliseconds > 1000f / this.WorstAllowedFPS)
					{
						break;
					}
				}
				if (this.realTimeToTickThrough > 0f)
				{
					this.realTimeToTickThrough = 0f;
				}
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002517C File Offset: 0x0002337C
		public void DoSingleTick()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].MapPreTick();
			}
			if (!DebugSettings.fastEcology)
			{
				this.ticksGameInt++;
			}
			else
			{
				this.ticksGameInt += 2000;
			}
			Shader.SetGlobalFloat(ShaderPropertyIDs.GameSeconds, this.TicksGame.TicksToSeconds());
			this.tickListNormal.Tick();
			this.tickListRare.Tick();
			this.tickListLong.Tick();
			try
			{
				Find.DateNotifier.DateNotifierTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			try
			{
				Find.TaleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			try
			{
				Find.QuestManager.QuestManagerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			for (int j = 0; j < maps.Count; j++)
			{
				maps[j].MapPostTick();
			}
			try
			{
				Find.History.HistoryTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			GameComponentUtility.GameComponentTick();
			try
			{
				Find.LetterStack.LetterStackTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			if (DebugViewSettings.logHourlyScreenshot && Find.TickManager.TicksGame >= this.lastAutoScreenshot + 2500)
			{
				ScreenshotTaker.QueueSilentScreenshot();
				this.lastAutoScreenshot = Find.TickManager.TicksGame / 2500 * 2500;
			}
			try
			{
				FilthMonitor.FilthMonitorTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString(), false);
			}
			UnityEngine.Debug.developerConsoleVisible = false;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00025460 File Offset: 0x00023660
		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000254BF File Offset: 0x000236BF
		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000254C8 File Offset: 0x000236C8
		public void Notify_GeneratedPotentiallyHostileMap()
		{
			this.Pause();
			this.slower.SignalForceNormalSpeedShort();
		}

		// Token: 0x04000718 RID: 1816
		private int ticksGameInt;

		// Token: 0x04000719 RID: 1817
		public int gameStartAbsTick;

		// Token: 0x0400071A RID: 1818
		private float realTimeToTickThrough;

		// Token: 0x0400071B RID: 1819
		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		// Token: 0x0400071C RID: 1820
		public TimeSpeed prePauseTimeSpeed;

		// Token: 0x0400071D RID: 1821
		private int startingYearInt = 5500;

		// Token: 0x0400071E RID: 1822
		private Stopwatch clock = new Stopwatch();

		// Token: 0x0400071F RID: 1823
		private TickList tickListNormal = new TickList(TickerType.Normal);

		// Token: 0x04000720 RID: 1824
		private TickList tickListRare = new TickList(TickerType.Rare);

		// Token: 0x04000721 RID: 1825
		private TickList tickListLong = new TickList(TickerType.Long);

		// Token: 0x04000722 RID: 1826
		public TimeSlower slower = new TimeSlower();

		// Token: 0x04000723 RID: 1827
		private int lastAutoScreenshot;

		// Token: 0x04000724 RID: 1828
		private float WorstAllowedFPS = 22f;

		// Token: 0x04000725 RID: 1829
		private int lastNothingHappeningCheckTick = -1;

		// Token: 0x04000726 RID: 1830
		private bool nothingHappeningCached;
	}
}
