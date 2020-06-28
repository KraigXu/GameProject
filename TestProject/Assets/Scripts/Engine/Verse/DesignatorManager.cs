using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000330 RID: 816
	public class DesignatorManager
	{
		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x00088ABD File Offset: 0x00086CBD
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x00088AC5 File Offset: 0x00086CC5
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00088ACD File Offset: 0x00086CCD
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00088AE7 File Offset: 0x00086CE7
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00088B03 File Offset: 0x00086D03
		private bool CheckSelectedDesignatorValid()
		{
			if (this.selectedDesignator == null)
			{
				return false;
			}
			if (!this.selectedDesignator.CanRemainSelected())
			{
				this.Deselect();
				return false;
			}
			return true;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00088B28 File Offset: 0x00086D28
		public void ProcessInputEvents()
		{
			if (!this.CheckSelectedDesignatorValid())
			{
				return;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				if (this.selectedDesignator.DraggableDimensions == 0)
				{
					Designator designator = this.selectedDesignator;
					AcceptanceReport acceptanceReport = this.selectedDesignator.CanDesignateCell(UI.MouseCell());
					if (acceptanceReport.Accepted)
					{
						designator.DesignateSingleCell(UI.MouseCell());
						designator.Finalize(true);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.SilentInput, false);
						this.selectedDesignator.Finalize(false);
					}
				}
				else
				{
					this.dragger.StartDrag();
				}
				Event.current.Use();
			}
			if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.Deselect();
				this.dragger.EndDrag();
				Event.current.Use();
				TutorSystem.Notify_Event("ClearDesignatorSelection");
			}
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && this.dragger.Dragging)
			{
				this.selectedDesignator.DesignateMultiCell(this.dragger.DragCells);
				this.dragger.EndDrag();
				Event.current.Use();
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00088C7B File Offset: 0x00086E7B
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00088C9B File Offset: 0x00086E9B
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x04000F03 RID: 3843
		private Designator selectedDesignator;

		// Token: 0x04000F04 RID: 3844
		private DesignationDragger dragger = new DesignationDragger();
	}
}
