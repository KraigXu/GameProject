using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F42 RID: 3906
	public static class GenLocalDate
	{
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x06006001 RID: 24577 RVA: 0x00215AA3 File Offset: 0x00213CA3
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x00215AAA File Offset: 0x00213CAA
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x00215AB7 File Offset: 0x00213CB7
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x00215AC4 File Offset: 0x00213CC4
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x00215AD1 File Offset: 0x00213CD1
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x00215ADE File Offset: 0x00213CDE
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x00215AEB File Offset: 0x00213CEB
		public static int Year(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenLocalDate.Year(map.Tile);
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x00215B06 File Offset: 0x00213D06
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x00215B13 File Offset: 0x00213D13
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x00215B20 File Offset: 0x00213D20
		public static int DayTick(Map map)
		{
			return GenLocalDate.DayTick(map.Tile);
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x00215B2D File Offset: 0x00213D2D
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x00215B3A File Offset: 0x00213D3A
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x00215B47 File Offset: 0x00213D47
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x00215B54 File Offset: 0x00213D54
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x00215B61 File Offset: 0x00213D61
		public static int DayOfYear(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return 0;
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x00215B7E File Offset: 0x00213D7E
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x00215B91 File Offset: 0x00213D91
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x00215BA4 File Offset: 0x00213DA4
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x00215BB7 File Offset: 0x00213DB7
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x00215BCA File Offset: 0x00213DCA
		public static int Year(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x00215BEB File Offset: 0x00213DEB
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x00215BFE File Offset: 0x00213DFE
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x00215C11 File Offset: 0x00213E11
		public static int DayTick(Thing thing)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x00215C24 File Offset: 0x00213E24
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x00215C37 File Offset: 0x00213E37
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x00215C4A File Offset: 0x00213E4A
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x00215C5D File Offset: 0x00213E5D
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x00215C70 File Offset: 0x00213E70
		public static int DayOfYear(int tile)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			return 0;
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x00215C97 File Offset: 0x00213E97
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x00215CB4 File Offset: 0x00213EB4
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x00215CD1 File Offset: 0x00213ED1
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006020 RID: 24608 RVA: 0x00215CEE File Offset: 0x00213EEE
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x06006021 RID: 24609 RVA: 0x00215D06 File Offset: 0x00213F06
		public static int Year(int tile)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x00215D31 File Offset: 0x00213F31
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x00215D4E File Offset: 0x00213F4E
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006024 RID: 24612 RVA: 0x00215D6B File Offset: 0x00213F6B
		public static int DayTick(int tile)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006025 RID: 24613 RVA: 0x00215D88 File Offset: 0x00213F88
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x00215DA5 File Offset: 0x00213FA5
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006027 RID: 24615 RVA: 0x00215DC2 File Offset: 0x00213FC2
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006028 RID: 24616 RVA: 0x00215DDF File Offset: 0x00213FDF
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x00215DFC File Offset: 0x00213FFC
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x0600602A RID: 24618 RVA: 0x00215E0C File Offset: 0x0021400C
		private static Vector2 LocationForDate(Thing thing)
		{
			int tile = thing.Tile;
			if (tile >= 0)
			{
				return Find.WorldGrid.LongLatOf(tile);
			}
			return Vector2.zero;
		}
	}
}
