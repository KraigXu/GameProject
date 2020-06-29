﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x00087FD8 File Offset: 0x000861D8
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00087FE0 File Offset: 0x000861E0
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x00087FEC File Offset: 0x000861EC
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x00087FFA File Offset: 0x000861FA
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		
		public void StartDrag()
		{
			this.dragging = true;
			this.startDragCell = UI.MouseCell();
		}

		
		public void EndDrag()
		{
			this.dragging = false;
			this.lastDragRealTime = -99999f;
			this.lastFrameDragCellsDrawn = 0;
			if (this.sustainer != null)
			{
				this.sustainer.End();
				this.sustainer = null;
			}
		}

		
		public void DraggerUpdate()
		{
			if (this.dragging)
			{
				List<IntVec3> list = this.DragCells;
				this.SelDes.RenderHighlight(list);
				if (list.Count != this.lastFrameDragCellsDrawn)
				{
					this.lastDragRealTime = Time.realtimeSinceStartup;
					this.lastFrameDragCellsDrawn = list.Count;
					if (this.SelDes.soundDragChanged != null)
					{
						this.SelDes.soundDragChanged.PlayOneShotOnCamera(null);
					}
				}
				if (this.sustainer == null || this.sustainer.Ended)
				{
					if (this.SelDes.soundDragSustain != null)
					{
						this.sustainer = this.SelDes.soundDragSustain.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
						return;
					}
				}
				else
				{
					this.sustainer.externalParams["TimeSinceDrag"] = Time.realtimeSinceStartup - this.lastDragRealTime;
					this.sustainer.Maintain();
				}
			}
		}

		
		public void DraggerOnGUI()
		{
			if (this.dragging && this.SelDes != null && this.SelDes.DragDrawMeasurements)
			{
				IntVec3 intVec = this.startDragCell - UI.MouseCell();
				intVec.x = Mathf.Abs(intVec.x) + 1;
				intVec.z = Mathf.Abs(intVec.z) + 1;
				if (intVec.x >= 3)
				{
					Vector2 screenPos = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
					screenPos.y = this.startDragCell.ToUIPosition().y;
					Widgets.DrawNumberOnMap(screenPos, intVec.x, Color.white);
				}
				if (intVec.z >= 3)
				{
					Vector2 screenPos2 = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
					screenPos2.x = this.startDragCell.ToUIPosition().x;
					Widgets.DrawNumberOnMap(screenPos2, intVec.z, Color.white);
				}
			}
		}

		
		[Obsolete]
		private void DrawNumber(Vector2 screenPos, int number)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, 40f, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			rect.y += 3f;
			Widgets.Label(rect, number.ToStringCached());
		}

		
		private void UpdateDragCellsIfNeeded()
		{
			if (Time.frameCount == this.lastUpdateFrame)
			{
				return;
			}
			this.lastUpdateFrame = Time.frameCount;
			this.dragCells.Clear();
			this.failureReasonInt = null;
			IntVec3 intVec = this.startDragCell;
			IntVec3 intVec2 = UI.MouseCell();
			if (this.SelDes.DraggableDimensions == 1)
			{
				bool flag = true;
				if (Mathf.Abs(intVec.x - intVec2.x) < Mathf.Abs(intVec.z - intVec2.z))
				{
					flag = false;
				}
				if (flag)
				{
					int z = intVec.z;
					if (intVec.x > intVec2.x)
					{
						IntVec3 intVec3 = intVec;
						intVec = intVec2;
						intVec2 = intVec3;
					}
					for (int i = intVec.x; i <= intVec2.x; i++)
					{
						this.TryAddDragCell(new IntVec3(i, intVec.y, z));
					}
				}
				else
				{
					int x = intVec.x;
					if (intVec.z > intVec2.z)
					{
						IntVec3 intVec4 = intVec;
						intVec = intVec2;
						intVec2 = intVec4;
					}
					for (int j = intVec.z; j <= intVec2.z; j++)
					{
						this.TryAddDragCell(new IntVec3(x, intVec.y, j));
					}
				}
			}
			if (this.SelDes.DraggableDimensions == 2)
			{
				IntVec3 intVec5 = intVec;
				IntVec3 intVec6 = intVec2;
				if (intVec6.x > intVec5.x + 50)
				{
					intVec6.x = intVec5.x + 50;
				}
				if (intVec6.z > intVec5.z + 50)
				{
					intVec6.z = intVec5.z + 50;
				}
				if (intVec6.x < intVec5.x)
				{
					if (intVec6.x < intVec5.x - 50)
					{
						intVec6.x = intVec5.x - 50;
					}
					int x2 = intVec5.x;
					intVec5 = new IntVec3(intVec6.x, intVec5.y, intVec5.z);
					intVec6 = new IntVec3(x2, intVec6.y, intVec6.z);
				}
				if (intVec6.z < intVec5.z)
				{
					if (intVec6.z < intVec5.z - 50)
					{
						intVec6.z = intVec5.z - 50;
					}
					int z2 = intVec5.z;
					intVec5 = new IntVec3(intVec5.x, intVec5.y, intVec6.z);
					intVec6 = new IntVec3(intVec6.x, intVec6.y, z2);
				}
				for (int k = intVec5.x; k <= intVec6.x; k++)
				{
					for (int l = intVec5.z; l <= intVec6.z; l++)
					{
						this.TryAddDragCell(new IntVec3(k, intVec5.y, l));
					}
				}
			}
		}

		
		private void TryAddDragCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = this.SelDes.CanDesignateCell(c);
			if (acceptanceReport.Accepted)
			{
				this.dragCells.Add(c);
				return;
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				this.failureReasonInt = acceptanceReport.Reason;
			}
		}

		
		private bool dragging;

		
		private IntVec3 startDragCell;

		
		private int lastFrameDragCellsDrawn;

		
		private Sustainer sustainer;

		
		private float lastDragRealTime = -1000f;

		
		private List<IntVec3> dragCells = new List<IntVec3>();

		
		private string failureReasonInt;

		
		private int lastUpdateFrame = -1;

		
		private const int MaxSquareWidth = 50;
	}
}
