﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000426 RID: 1062
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x06001FC3 RID: 8131 RVA: 0x000C22F0 File Offset: 0x000C04F0
		static ThreadLocalDeepProfiler()
		{
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = "";
				for (int j = 0; j < i; j++)
				{
					string[] prefixes = ThreadLocalDeepProfiler.Prefixes;
					int num = i;
					prefixes[num] += " -";
				}
			}
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000C2346 File Offset: 0x000C0546
		public void Start(string label = null)
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000C2364 File Offset: 0x000C0564
		public void End()
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			if (this.watchers.Count == 0)
			{
				Log.Error("Ended deep profiling while not profiling.", false);
				return;
			}
			ThreadLocalDeepProfiler.Watcher watcher = this.watchers.Pop();
			watcher.Watch.Stop();
			if (this.watchers.Count > 0)
			{
				this.watchers.Peek().AddChildResult(watcher);
				return;
			}
			this.Output(watcher);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000C23D0 File Offset: 0x000C05D0
		private void Output(ThreadLocalDeepProfiler.Watcher root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (UnityData.IsInMainThread)
			{
				stringBuilder.AppendLine("--- Main thread ---");
			}
			else
			{
				stringBuilder.AppendLine("--- Thread " + Thread.CurrentThread.ManagedThreadId + " ---");
			}
			List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
			list.Add(root);
			this.AppendStringRecursive(stringBuilder, root.Label, root.Children, root.ElapsedMilliseconds, 0, list);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			this.HotspotAnalysis(stringBuilder, list);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000C2468 File Offset: 0x000C0668
		private void HotspotAnalysis(StringBuilder sb, List<ThreadLocalDeepProfiler.Watcher> allWatchers)
		{
			List<ThreadLocalDeepProfiler.LabelTimeTuple> list = new List<ThreadLocalDeepProfiler.LabelTimeTuple>();
			foreach (IGrouping<string, ThreadLocalDeepProfiler.Watcher> grouping in from w in allWatchers
			group w by w.Label)
			{
				double num = 0.0;
				double num2 = 0.0;
				int num3 = 0;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in grouping)
				{
					num3++;
					num += watcher.ElapsedMilliseconds;
					if (watcher.Children != null)
					{
						foreach (ThreadLocalDeepProfiler.Watcher watcher2 in watcher.Children)
						{
							num2 += watcher2.ElapsedMilliseconds;
						}
					}
				}
				list.Add(new ThreadLocalDeepProfiler.LabelTimeTuple
				{
					label = num3 + "x " + grouping.Key,
					totalTime = num,
					selfTime = num - num2
				});
			}
			sb.AppendLine("Hotspot analysis");
			sb.AppendLine("----------------------------------------");
			foreach (ThreadLocalDeepProfiler.LabelTimeTuple labelTimeTuple in from e in list
			orderby e.selfTime descending
			select e)
			{
				string[] array = new string[6];
				array[0] = labelTimeTuple.label;
				array[1] = " -> ";
				int num4 = 2;
				double num5 = labelTimeTuple.selfTime;
				array[num4] = num5.ToString("0.0000");
				array[3] = " ms (total (w/children): ";
				int num6 = 4;
				num5 = labelTimeTuple.totalTime;
				array[num6] = num5.ToString("0.0000");
				array[5] = " ms)";
				sb.AppendLine(string.Concat(array));
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000C26A4 File Offset: 0x000C08A4
		private void AppendStringRecursive(StringBuilder sb, string label, List<ThreadLocalDeepProfiler.Watcher> children, double elapsedTime, int depth, List<ThreadLocalDeepProfiler.Watcher> allWatchers)
		{
			if (children != null)
			{
				double num = elapsedTime;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in children)
				{
					num -= watcher.ElapsedMilliseconds;
				}
				sb.AppendLine(string.Concat(new string[]
				{
					ThreadLocalDeepProfiler.Prefixes[depth],
					" ",
					elapsedTime.ToString("0.0000"),
					"ms (self: ",
					num.ToString("0.0000"),
					" ms) ",
					label
				}));
			}
			else
			{
				sb.AppendLine(string.Concat(new string[]
				{
					ThreadLocalDeepProfiler.Prefixes[depth],
					" ",
					elapsedTime.ToString("0.0000"),
					"ms ",
					label
				}));
			}
			if (children != null)
			{
				allWatchers.AddRange(children);
				foreach (IGrouping<string, ThreadLocalDeepProfiler.Watcher> grouping in from c in children
				group c by c.Label)
				{
					List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
					double num2 = 0.0;
					int num3 = 0;
					foreach (ThreadLocalDeepProfiler.Watcher watcher2 in grouping)
					{
						if (watcher2.Children != null)
						{
							foreach (ThreadLocalDeepProfiler.Watcher item in watcher2.Children)
							{
								list.Add(item);
							}
						}
						num2 += watcher2.ElapsedMilliseconds;
						num3++;
					}
					if (num3 <= 1)
					{
						this.AppendStringRecursive(sb, grouping.Key, list, num2, depth + 1, allWatchers);
					}
					else
					{
						this.AppendStringRecursive(sb, num3 + "x " + grouping.Key, list, num2, depth + 1, allWatchers);
					}
				}
			}
		}

		// Token: 0x0400139E RID: 5022
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x0400139F RID: 5023
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x040013A0 RID: 5024
		private const int MaxDepth = 50;

		// Token: 0x02001672 RID: 5746
		private class Watcher
		{
			// Token: 0x17001500 RID: 5376
			// (get) Token: 0x060084B8 RID: 33976 RVA: 0x002B0A04 File Offset: 0x002AEC04
			public double ElapsedMilliseconds
			{
				get
				{
					return this.watch.Elapsed.TotalMilliseconds;
				}
			}

			// Token: 0x17001501 RID: 5377
			// (get) Token: 0x060084B9 RID: 33977 RVA: 0x002B0A24 File Offset: 0x002AEC24
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17001502 RID: 5378
			// (get) Token: 0x060084BA RID: 33978 RVA: 0x002B0A2C File Offset: 0x002AEC2C
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17001503 RID: 5379
			// (get) Token: 0x060084BB RID: 33979 RVA: 0x002B0A34 File Offset: 0x002AEC34
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x060084BC RID: 33980 RVA: 0x002B0A3C File Offset: 0x002AEC3C
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x060084BD RID: 33981 RVA: 0x002B0A5D File Offset: 0x002AEC5D
			public Watcher(string label, Stopwatch stopwatch)
			{
				this.label = label;
				this.watch = stopwatch;
				this.children = null;
			}

			// Token: 0x060084BE RID: 33982 RVA: 0x002B0A7A File Offset: 0x002AEC7A
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x04005602 RID: 22018
			private string label;

			// Token: 0x04005603 RID: 22019
			private Stopwatch watch;

			// Token: 0x04005604 RID: 22020
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}

		// Token: 0x02001673 RID: 5747
		private struct LabelTimeTuple
		{
			// Token: 0x04005605 RID: 22021
			public string label;

			// Token: 0x04005606 RID: 22022
			public double totalTime;

			// Token: 0x04005607 RID: 22023
			public double selfTime;
		}
	}
}
