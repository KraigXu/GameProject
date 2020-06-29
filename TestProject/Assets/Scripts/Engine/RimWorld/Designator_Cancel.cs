using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Designator_Cancel : Designator
	{
		
		// (get) Token: 0x06005761 RID: 22369 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		
		public Designator_Cancel()
		{
			this.defaultLabel = "DesignatorCancel".Translate();
			this.defaultDesc = "DesignatorCancelDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Cancel;
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
			this.tutorTag = "Cancel";
		}

		
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (this.CancelableDesignationsAt(c).Count<Designation>() > 0)
			{
				return true;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Designation designation in this.CancelableDesignationsAt(c).ToList<Designation>())
			{
				if (designation.def.designateCancelable)
				{
					base.Map.designationManager.RemoveDesignation(designation);
				}
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t) != null)
			{
				using (IEnumerator<Designation> enumerator = base.Map.designationManager.AllDesignationsOn(t).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.designateCancelable)
						{
							return true;
						}
					}
				}
			}
			if (t.def.mineable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
			{
				return true;
			}
			if (t.def.IsSmoothable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.SmoothWall) != null)
			{
				return true;
			}
			return t.Faction == Faction.OfPlayer && (t is Frame || t is Blueprint);
		}

		
		public override void DesignateThing(Thing t)
		{
			if (t is Frame || t is Blueprint)
			{
				t.Destroy(DestroyMode.Cancel);
				return;
			}
			base.Map.designationManager.RemoveAllDesignationsOn(t, true);
			if (t.def.mineable)
			{
				Designation designation = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine);
				if (designation != null)
				{
					base.Map.designationManager.RemoveDesignation(designation);
				}
			}
			if (t.def.IsSmoothable)
			{
				Designation designation2 = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.SmoothWall);
				if (designation2 != null)
				{
					base.Map.designationManager.RemoveDesignation(designation2);
				}
			}
		}

		
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		
		private IEnumerable<Designation> CancelableDesignationsAt(IntVec3 c)
		{
			return from x in base.Map.designationManager.AllDesignationsAt(c)
			where x.def != DesignationDefOf.Plan
			select x;
		}

		
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			Designator_Cancel.seenThings.Clear();
			int i = 0;
			while (i < dragCells.Count)
			{
				if (!base.Map.designationManager.HasMapDesignationAt(dragCells[i]))
				{
					goto IL_76;
				}
				Graphics.DrawMesh(MeshPool.plane10, dragCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
				if (base.Map.designationManager.DesignationAt(dragCells[i], DesignationDefOf.Mine) == null)
				{
					goto IL_76;
				}
				IL_FF:
				i++;
				continue;
				IL_76:
				List<Thing> thingList = dragCells[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing = thingList[j];
					if (!Designator_Cancel.seenThings.Contains(thing) && this.CanDesignateThing(thing).Accepted)
					{
						Vector3 drawPos = thing.DrawPos;
						drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
						Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, DesignatorUtility.DragHighlightThingMat, 0);
						Designator_Cancel.seenThings.Add(thing);
					}
				}
				goto IL_FF;
			}
			Designator_Cancel.seenThings.Clear();
		}

		
		private static HashSet<Thing> seenThings = new HashSet<Thing>();
	}
}
