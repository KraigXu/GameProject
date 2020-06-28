using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E40 RID: 3648
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06005847 RID: 22599 RVA: 0x001D4C0A File Offset: 0x001D2E0A
		// (set) Token: 0x06005848 RID: 22600 RVA: 0x001D4C16 File Offset: 0x001D2E16
		private Zone SelectedZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
			set
			{
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
				}
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06005849 RID: 22601
		protected abstract string NewZoneLabel { get; }

		// Token: 0x0600584A RID: 22602
		protected abstract Zone MakeNewZone();

		// Token: 0x0600584B RID: 22603 RVA: 0x001D4C32 File Offset: 0x001D2E32
		public Designator_ZoneAdd()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x0600584C RID: 22604 RVA: 0x001D4C6C File Offset: 0x001D2E6C
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x001D4CBC File Offset: 0x001D2EBC
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = "";
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					Zone selectedZone = Find.Selector.SelectedZone;
					if (selectedZone != null)
					{
						text = "ExpandOrCreateZone".Translate(selectedZone.label, this.NewZoneLabel);
					}
					else
					{
						text = "CreateNewZone".Translate(this.NewZoneLabel);
					}
				}
				GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null, false, default(Color));
			}
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x001D4D68 File Offset: 0x001D2F68
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (c.InNoZoneEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			if (zone != null && zone.GetType() != this.zoneTypeToPlace)
			{
				return false;
			}
			using (IEnumerator<Thing> enumerator = base.Map.thingGrid.ThingsAt(c).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.def.CanOverlapZones)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x001D4E4C File Offset: 0x001D304C
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			List<IntVec3> list = cells.ToList<IntVec3>();
			if (list.Count == 1)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(list[0]);
				if (zone != null)
				{
					if (zone.GetType() == this.zoneTypeToPlace)
					{
						this.SelectedZone = zone;
					}
					return;
				}
			}
			if (this.SelectedZone == null)
			{
				Zone zone2 = null;
				foreach (IntVec3 c3 in cells)
				{
					Zone zone3 = base.Map.zoneManager.ZoneAt(c3);
					if (zone3 != null && zone3.GetType() == this.zoneTypeToPlace)
					{
						if (zone2 == null)
						{
							zone2 = zone3;
						}
						else if (zone3 != zone2)
						{
							zone2 = null;
							break;
						}
					}
				}
				this.SelectedZone = zone2;
			}
			list.RemoveAll((IntVec3 c) => base.Map.zoneManager.ZoneAt(c) != null);
			if (list.Count == 0)
			{
				return;
			}
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, list)))
			{
				return;
			}
			if (this.SelectedZone == null)
			{
				this.SelectedZone = this.MakeNewZone();
				base.Map.zoneManager.RegisterZone(this.SelectedZone);
				this.SelectedZone.AddCell(list[0]);
				list.RemoveAt(0);
			}
			bool somethingSucceeded;
			for (;;)
			{
				somethingSucceeded = true;
				int count = list.Count;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = list[i] + GenAdj.CardinalDirections[j];
						if (c2.InBounds(base.Map) && base.Map.zoneManager.ZoneAt(c2) == this.SelectedZone)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this.SelectedZone.AddCell(list[i]);
						list.RemoveAt(i);
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				if (list.Count == count)
				{
					this.SelectedZone = this.MakeNewZone();
					base.Map.zoneManager.RegisterZone(this.SelectedZone);
					this.SelectedZone.AddCell(list[0]);
					list.RemoveAt(0);
				}
			}
			this.SelectedZone.CheckContiguous();
			base.Finalize(somethingSucceeded);
			TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, list));
		}

		// Token: 0x04002FB3 RID: 12211
		protected Type zoneTypeToPlace;
	}
}
