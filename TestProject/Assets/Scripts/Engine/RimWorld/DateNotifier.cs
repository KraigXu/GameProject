using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class DateNotifier : IExposable
	{
		
		public void ExposeData()
		{
			Scribe_Values.Look<Season>(ref this.lastSeason, "lastSeason", Season.Undefined, false);
		}

		
		public void DateNotifierTick()
		{
			Map map = this.FindPlayerHomeWithMinTimezone();
			float latitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).y : 0f;
			float longitude = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f;
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, latitude, longitude);
			if (season != this.lastSeason && (this.lastSeason == Season.Undefined || season != this.lastSeason.GetPreviousSeason()))
			{
				if (this.lastSeason != Season.Undefined && this.AnyPlayerHomeSeasonsAreMeaningful())
				{
					if (GenDate.YearsPassed == 0 && season == Season.Summer && this.AnyPlayerHomeAvgTempIsLowInWinter())
					{
						Find.LetterStack.ReceiveLetter("LetterLabelFirstSummerWarning".Translate(), "FirstSummerWarning".Translate(), LetterDefOf.NeutralEvent, null);
					}
					else if (GenDate.DaysPassed > 5)
					{
						Messages.Message("MessageSeasonBegun".Translate(season.Label()).CapitalizeFirst(), MessageTypeDefOf.NeutralEvent, true);
					}
				}
				this.lastSeason = season;
			}
		}

		
		private Map FindPlayerHomeWithMinTimezone()
		{
			List<Map> maps = Find.Maps;
			Map map = null;
			int num = -1;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					int num2 = GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(maps[i].Tile).x);
					if (map == null || num2 < num)
					{
						map = maps[i];
						num = num2;
					}
				}
			}
			return map;
		}

		
		private bool AnyPlayerHomeSeasonsAreMeaningful()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapTemperature.LocalSeasonsAreMeaningful())
				{
					return true;
				}
			}
			return false;
		}

		
		private bool AnyPlayerHomeAvgTempIsLowInWinter()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && GenTemperature.AverageTemperatureAtTileForTwelfth(maps[i].Tile, Season.Winter.GetMiddleTwelfth(Find.WorldGrid.LongLatOf(maps[i].Tile).y)) < 8f)
				{
					return true;
				}
			}
			return false;
		}

		
		private Season lastSeason;
	}
}
