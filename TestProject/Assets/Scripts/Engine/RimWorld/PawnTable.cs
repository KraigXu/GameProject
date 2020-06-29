using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class PawnTable
	{
		
		// (get) Token: 0x06005DD5 RID: 24021 RVA: 0x002066C8 File Offset: 0x002048C8
		public List<PawnColumnDef> ColumnsListForReading
		{
			get
			{
				return this.def.columns;
			}
		}

		
		// (get) Token: 0x06005DD6 RID: 24022 RVA: 0x002066D5 File Offset: 0x002048D5
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		
		// (get) Token: 0x06005DD7 RID: 24023 RVA: 0x002066DD File Offset: 0x002048DD
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		
		// (get) Token: 0x06005DD8 RID: 24024 RVA: 0x002066EF File Offset: 0x002048EF
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		
		// (get) Token: 0x06005DD9 RID: 24025 RVA: 0x002066FD File Offset: 0x002048FD
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		
		// (get) Token: 0x06005DDA RID: 24026 RVA: 0x0020670B File Offset: 0x0020490B
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		
		// (get) Token: 0x06005DDB RID: 24027 RVA: 0x00206719 File Offset: 0x00204919
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		
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

		
		public void SetDirty()
		{
			this.dirty = true;
		}

		
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		
		protected virtual bool CanAssignPawn(Pawn p)
		{
			return true;
		}

		
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

		
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		
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

		
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		
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

		
		private void RecacheLookTargets()
		{
			this.cachedLookTargets.Clear();
			this.cachedLookTargets.AddRange(from p in this.cachedPawns
			select new LookTargets(p));
		}

		
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = i;
				list[index] -= toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		
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

		
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		
		private PawnTableDef def;

		
		private Func<IEnumerable<Pawn>> pawnsGetter;

		
		private int minTableWidth;

		
		private int maxTableWidth;

		
		private int minTableHeight;

		
		private int maxTableHeight;

		
		private Vector2 fixedSize;

		
		private bool hasFixedSize;

		
		private bool dirty;

		
		private List<bool> columnAtMaxWidth = new List<bool>();

		
		private List<bool> columnAtOptimalWidth = new List<bool>();

		
		private Vector2 scrollPosition;

		
		private PawnColumnDef sortByColumn;

		
		private bool sortDescending;

		
		private Vector2 cachedSize;

		
		private List<Pawn> cachedPawns = new List<Pawn>();

		
		private List<float> cachedColumnWidths = new List<float>();

		
		private List<float> cachedRowHeights = new List<float>();

		
		private List<LookTargets> cachedLookTargets = new List<LookTargets>();

		
		private float cachedHeaderHeight;

		
		private float cachedHeightNoScrollbar;
	}
}
