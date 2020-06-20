using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF2 RID: 3826
	public class PawnTable
	{
		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x06005DD5 RID: 24021 RVA: 0x002066C8 File Offset: 0x002048C8
		public List<PawnColumnDef> ColumnsListForReading
		{
			get
			{
				return this.def.columns;
			}
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06005DD6 RID: 24022 RVA: 0x002066D5 File Offset: 0x002048D5
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06005DD7 RID: 24023 RVA: 0x002066DD File Offset: 0x002048DD
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x06005DD8 RID: 24024 RVA: 0x002066EF File Offset: 0x002048EF
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x06005DD9 RID: 24025 RVA: 0x002066FD File Offset: 0x002048FD
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06005DDA RID: 24026 RVA: 0x0020670B File Offset: 0x0020490B
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06005DDB RID: 24027 RVA: 0x00206719 File Offset: 0x00204919
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x00206728 File Offset: 0x00204928
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x002067A4 File Offset: 0x002049A4
		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.RecacheIfDirty();
			float num = this.cachedSize.x - 16f;
			int num2 = 0;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				int num3;
				if (i == this.def.columns.Count - 1)
				{
					num3 = (int)(num - (float)num2);
				}
				else
				{
					num3 = (int)this.cachedColumnWidths[i];
				}
				Rect rect = new Rect((float)((int)position.x + num2), (float)((int)position.y), (float)num3, (float)((int)this.cachedHeaderHeight));
				this.def.columns[i].Worker.DoHeader(rect, this);
				num2 += num3;
			}
			Rect outRect = new Rect((float)((int)position.x), (float)((int)position.y + (int)this.cachedHeaderHeight), (float)((int)this.cachedSize.x), (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)((int)this.cachedHeightNoScrollbar - (int)this.cachedHeaderHeight));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			int num4 = 0;
			for (int j = 0; j < this.cachedPawns.Count; j++)
			{
				num2 = 0;
				if ((float)num4 - this.scrollPosition.y + (float)((int)this.cachedRowHeights[j]) >= 0f && (float)num4 - this.scrollPosition.y <= outRect.height)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.2f);
					Widgets.DrawLineHorizontal(0f, (float)num4, viewRect.width);
					GUI.color = Color.white;
					if (!this.CanAssignPawn(this.cachedPawns[j]))
					{
						GUI.color = Color.gray;
					}
					Rect rect2 = new Rect(0f, (float)num4, viewRect.width, (float)((int)this.cachedRowHeights[j]));
					if (Mouse.IsOver(rect2))
					{
						GUI.DrawTexture(rect2, TexUI.HighlightTex);
						this.cachedLookTargets[j].Highlight(true, this.cachedPawns[j].IsColonist, false);
					}
					for (int k = 0; k < this.def.columns.Count; k++)
					{
						int num5;
						if (k == this.def.columns.Count - 1)
						{
							num5 = (int)(num - (float)num2);
						}
						else
						{
							num5 = (int)this.cachedColumnWidths[k];
						}
						Rect rect3 = new Rect((float)num2, (float)num4, (float)num5, (float)((int)this.cachedRowHeights[j]));
						this.def.columns[k].Worker.DoCell(rect3, this.cachedPawns[j], this);
						num2 += num5;
					}
					if (this.cachedPawns[j].Downed)
					{
						GUI.color = new Color(1f, 0f, 0f, 0.5f);
						Widgets.DrawLineHorizontal(0f, rect2.center.y, viewRect.width);
					}
					GUI.color = Color.white;
				}
				num4 += (int)this.cachedRowHeights[j];
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x00206B29 File Offset: 0x00204D29
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x00206B32 File Offset: 0x00204D32
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x00206B5E File Offset: 0x00204D5E
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x00206B74 File Offset: 0x00204D74
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanAssignPawn(Pawn p)
		{
			return true;
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x00206B8C File Offset: 0x00204D8C
		private void RecacheIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.RecachePawns();
			this.RecacheRowHeights();
			this.cachedHeaderHeight = this.CalculateHeaderHeight();
			this.cachedHeightNoScrollbar = this.CalculateTotalRequiredHeight();
			this.RecacheSize();
			this.RecacheColumnWidths();
			this.RecacheLookTargets();
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x00206BE0 File Offset: 0x00204DE0
		private void RecachePawns()
		{
			this.cachedPawns.Clear();
			this.cachedPawns.AddRange(this.pawnsGetter());
			this.cachedPawns = this.LabelSortFunction(this.cachedPawns).ToList<Pawn>();
			if (this.sortByColumn != null)
			{
				if (this.sortDescending)
				{
					this.cachedPawns.SortStable(new Func<Pawn, Pawn, int>(this.sortByColumn.Worker.Compare));
				}
				else
				{
					this.cachedPawns.SortStable((Pawn a, Pawn b) => this.sortByColumn.Worker.Compare(b, a));
				}
			}
			this.cachedPawns = this.PrimarySortFunction(this.cachedPawns).ToList<Pawn>();
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x00206C87 File Offset: 0x00204E87
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		// Token: 0x06005DE6 RID: 24038 RVA: 0x0002D90A File Offset: 0x0002BB0A
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x00206CB0 File Offset: 0x00204EB0
		private void RecacheColumnWidths()
		{
			float num = this.cachedSize.x - 16f;
			float num2 = 0f;
			this.RecacheColumnWidths_StartWithMinWidths(out num2);
			if (num2 == num)
			{
				return;
			}
			if (num2 > num)
			{
				this.SubtractProportionally(num2 - num, num2);
				return;
			}
			bool flag;
			this.RecacheColumnWidths_DistributeUntilOptimal(num, ref num2, out flag);
			if (flag)
			{
				return;
			}
			this.RecacheColumnWidths_DistributeAboveOptimal(num, ref num2);
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x00206D0C File Offset: 0x00204F0C
		private void RecacheColumnWidths_StartWithMinWidths(out float minWidthsSum)
		{
			minWidthsSum = 0f;
			this.cachedColumnWidths.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				float minWidth = this.GetMinWidth(this.def.columns[i]);
				this.cachedColumnWidths.Add(minWidth);
				minWidthsSum += minWidth;
			}
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x00206D70 File Offset: 0x00204F70
		private void RecacheColumnWidths_DistributeUntilOptimal(float totalAvailableSpaceForColumns, ref float usedWidth, out bool noMoreFreeSpace)
		{
			this.columnAtOptimalWidth.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				this.columnAtOptimalWidth.Add(this.cachedColumnWidths[i] >= this.GetOptimalWidth(this.def.columns[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = float.MinValue;
				for (int j = 0; j < this.def.columns.Count; j++)
				{
					if (!this.columnAtOptimalWidth[j])
					{
						num2 = Mathf.Max(num2, (float)this.def.columns[j].widthPriority);
					}
				}
				float num3 = 0f;
				for (int k = 0; k < this.cachedColumnWidths.Count; k++)
				{
					if (!this.columnAtOptimalWidth[k] && (float)this.def.columns[k].widthPriority == num2)
					{
						num3 += this.GetOptimalWidth(this.def.columns[k]);
					}
				}
				float num4 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				bool flag2 = false;
				for (int l = 0; l < this.cachedColumnWidths.Count; l++)
				{
					if (!this.columnAtOptimalWidth[l])
					{
						if ((float)this.def.columns[l].widthPriority != num2)
						{
							flag = true;
						}
						else
						{
							float num5 = num4 * this.GetOptimalWidth(this.def.columns[l]) / num3;
							float num6 = this.GetOptimalWidth(this.def.columns[l]) - this.cachedColumnWidths[l];
							if (num5 >= num6)
							{
								num5 = num6;
								this.columnAtOptimalWidth[l] = true;
								flag2 = true;
							}
							else
							{
								flag = true;
							}
							if (num5 > 0f)
							{
								List<float> list = this.cachedColumnWidths;
								int index = l;
								list[index] += num5;
								usedWidth += num5;
							}
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					goto Block_13;
				}
				if (!flag || !flag2)
				{
					goto IL_243;
				}
			}
			Log.Error("Too many iterations.", false);
			goto IL_243;
			Block_13:
			noMoreFreeSpace = true;
			IL_243:
			noMoreFreeSpace = false;
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x00206FC4 File Offset: 0x002051C4
		private void RecacheColumnWidths_DistributeAboveOptimal(float totalAvailableSpaceForColumns, ref float usedWidth)
		{
			this.columnAtMaxWidth.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				this.columnAtMaxWidth.Add(this.cachedColumnWidths[i] >= this.GetMaxWidth(this.def.columns[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = 0f;
				for (int j = 0; j < this.def.columns.Count; j++)
				{
					if (!this.columnAtMaxWidth[j])
					{
						num2 += Mathf.Max(this.GetOptimalWidth(this.def.columns[j]), 1f);
					}
				}
				float num3 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				for (int k = 0; k < this.def.columns.Count; k++)
				{
					if (!this.columnAtMaxWidth[k])
					{
						float num4 = num3 * Mathf.Max(this.GetOptimalWidth(this.def.columns[k]), 1f) / num2;
						float num5 = this.GetMaxWidth(this.def.columns[k]) - this.cachedColumnWidths[k];
						if (num4 >= num5)
						{
							num4 = num5;
							this.columnAtMaxWidth[k] = true;
						}
						else
						{
							flag = true;
						}
						if (num4 > 0f)
						{
							List<float> list = this.cachedColumnWidths;
							int index = k;
							list[index] += num4;
							usedWidth += num4;
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					return;
				}
				if (!flag)
				{
					goto Block_10;
				}
			}
			Log.Error("Too many iterations.", false);
			return;
			Block_10:
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x00207198 File Offset: 0x00205398
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		// Token: 0x06005DEC RID: 24044 RVA: 0x002071E4 File Offset: 0x002053E4
		private void RecacheSize()
		{
			if (this.hasFixedSize)
			{
				this.cachedSize = this.fixedSize;
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				if (!this.def.columns[i].ignoreWhenCalculatingOptimalTableSize)
				{
					num += this.GetOptimalWidth(this.def.columns[i]);
				}
			}
			float num2 = Mathf.Clamp(num + 16f, (float)this.minTableWidth, (float)this.maxTableWidth);
			float num3 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
			num2 = Mathf.Min(num2, (float)UI.screenWidth);
			num3 = Mathf.Min(num3, (float)UI.screenHeight);
			this.cachedSize = new Vector2(num2, num3);
		}

		// Token: 0x06005DED RID: 24045 RVA: 0x002072B8 File Offset: 0x002054B8
		private void RecacheLookTargets()
		{
			this.cachedLookTargets.Clear();
			this.cachedLookTargets.AddRange(from p in this.cachedPawns
			select new LookTargets(p));
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x00207308 File Offset: 0x00205508
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = i;
				list[index] -= toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x00207354 File Offset: 0x00205554
		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num += Mathf.Max(this.GetOptimalWidth(this.def.columns[i]), 1f);
			}
			for (int j = 0; j < this.def.columns.Count; j++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = j;
				list[index] += toDistribute * Mathf.Max(this.GetOptimalWidth(this.def.columns[j]), 1f) / num;
			}
		}

		// Token: 0x06005DF0 RID: 24048 RVA: 0x00207401 File Offset: 0x00205601
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		// Token: 0x06005DF1 RID: 24049 RVA: 0x0020741A File Offset: 0x0020561A
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		// Token: 0x06005DF2 RID: 24050 RVA: 0x00207433 File Offset: 0x00205633
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		// Token: 0x06005DF3 RID: 24051 RVA: 0x0020744C File Offset: 0x0020564C
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		// Token: 0x06005DF4 RID: 24052 RVA: 0x002074A0 File Offset: 0x002056A0
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		// Token: 0x06005DF5 RID: 24053 RVA: 0x002074F4 File Offset: 0x002056F4
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		// Token: 0x040032D5 RID: 13013
		private PawnTableDef def;

		// Token: 0x040032D6 RID: 13014
		private Func<IEnumerable<Pawn>> pawnsGetter;

		// Token: 0x040032D7 RID: 13015
		private int minTableWidth;

		// Token: 0x040032D8 RID: 13016
		private int maxTableWidth;

		// Token: 0x040032D9 RID: 13017
		private int minTableHeight;

		// Token: 0x040032DA RID: 13018
		private int maxTableHeight;

		// Token: 0x040032DB RID: 13019
		private Vector2 fixedSize;

		// Token: 0x040032DC RID: 13020
		private bool hasFixedSize;

		// Token: 0x040032DD RID: 13021
		private bool dirty;

		// Token: 0x040032DE RID: 13022
		private List<bool> columnAtMaxWidth = new List<bool>();

		// Token: 0x040032DF RID: 13023
		private List<bool> columnAtOptimalWidth = new List<bool>();

		// Token: 0x040032E0 RID: 13024
		private Vector2 scrollPosition;

		// Token: 0x040032E1 RID: 13025
		private PawnColumnDef sortByColumn;

		// Token: 0x040032E2 RID: 13026
		private bool sortDescending;

		// Token: 0x040032E3 RID: 13027
		private Vector2 cachedSize;

		// Token: 0x040032E4 RID: 13028
		private List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x040032E5 RID: 13029
		private List<float> cachedColumnWidths = new List<float>();

		// Token: 0x040032E6 RID: 13030
		private List<float> cachedRowHeights = new List<float>();

		// Token: 0x040032E7 RID: 13031
		private List<LookTargets> cachedLookTargets = new List<LookTargets>();

		// Token: 0x040032E8 RID: 13032
		private float cachedHeaderHeight;

		// Token: 0x040032E9 RID: 13033
		private float cachedHeightNoScrollbar;
	}
}
