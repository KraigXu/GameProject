using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class Designation : IExposable
	{
		
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00034B4C File Offset: 0x00032D4C
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00034B59 File Offset: 0x00032D59
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		
		public Designation()
		{
		}

		
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == DesignationDefOf.Haul && !this.target.HasThing)
			{
				Log.Error("Haul designation has no target! Deleting.", false);
				this.Delete();
			}
		}

		
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		
		public virtual void DesignationDraw()
		{
			if (this.target.HasThing && !this.target.Thing.Spawned)
			{
				return;
			}
			Vector3 position = default(Vector3);
			if (this.target.HasThing)
			{
				position = this.target.Thing.DrawPos;
				position.y = this.DesignationDrawAltitude;
			}
			else
			{
				position = this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
			}
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, this.def.iconMat, 0);
		}

		
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		
		public override string ToString()
		{
			return string.Format(string.Concat(new object[]
			{
				"(",
				this.def.defName,
				" target=",
				this.target,
				")"
			}), Array.Empty<object>());
		}

		
		public DesignationManager designationManager;

		
		public DesignationDef def;

		
		public LocalTargetInfo target;

		
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
