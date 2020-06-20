﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200037B RID: 891
	public static class DebugTables
	{
		// Token: 0x06001A69 RID: 6761 RVA: 0x000A2974 File Offset: 0x000A0B74
		public static void MakeTablesDialog<T>(IEnumerable<T> dataSources, params TableDataGetter<T>[] getters)
		{
			List<TableDataGetter<T>> list = getters.ToList<TableDataGetter<T>>();
			int num = dataSources.Count<T>() + 1;
			int count = list.Count;
			string[,] array = new string[count, num];
			int num2 = 0;
			foreach (TableDataGetter<T> tableDataGetter in getters)
			{
				array[num2, 0] = tableDataGetter.label;
				num2++;
			}
			int num3 = 1;
			foreach (T arg in dataSources)
			{
				for (int j = 0; j < count; j++)
				{
					array[j, num3] = list[j].getter(arg);
				}
				num3++;
			}
			Find.WindowStack.Add(new Window_DebugTable(array));
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x000A2A58 File Offset: 0x000A0C58
		public static void MakeTablesDialog<TColumn, TRow>(IEnumerable<TColumn> colValues, Func<TColumn, string> colLabelFormatter, IEnumerable<TRow> rowValues, Func<TRow, string> rowLabelFormatter, Func<TColumn, TRow, string> func, string tlLabel = "")
		{
			int num = colValues.Count<TColumn>() + 1;
			int num2 = rowValues.Count<TRow>() + 1;
			string[,] array = new string[num, num2];
			array[0, 0] = tlLabel;
			int num3 = 1;
			foreach (TColumn arg in colValues)
			{
				array[num3, 0] = colLabelFormatter(arg);
				num3++;
			}
			int num4 = 1;
			foreach (TRow arg2 in rowValues)
			{
				array[0, num4] = rowLabelFormatter(arg2);
				num4++;
			}
			int num5 = 1;
			foreach (TRow arg3 in rowValues)
			{
				int num6 = 1;
				foreach (TColumn arg4 in colValues)
				{
					array[num6, num5] = func(arg4, arg3);
					num6++;
				}
				num5++;
			}
			Find.WindowStack.Add(new Window_DebugTable(array));
		}
	}
}
