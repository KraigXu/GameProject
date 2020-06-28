using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200015B RID: 347
	public class Designation : IExposable
	{
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00034B4C File Offset: 0x00032D4C
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00034B59 File Offset: 0x00032D59
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public Designation()
		{
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00034B62 File Offset: 0x00032D62
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00034B78 File Offset: 0x00032D78
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

		// Token: 0x060009B7 RID: 2487 RVA: 0x00034BD8 File Offset: 0x00032DD8
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00034C02 File Offset: 0x00032E02
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00034C3C File Offset: 0x00032E3C
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

		// Token: 0x060009BA RID: 2490 RVA: 0x00034CD4 File Offset: 0x00032ED4
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00034CE8 File Offset: 0x00032EE8
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

		// Token: 0x040007F7 RID: 2039
		public DesignationManager designationManager;

		// Token: 0x040007F8 RID: 2040
		public DesignationDef def;

		// Token: 0x040007F9 RID: 2041
		public LocalTargetInfo target;

		// Token: 0x040007FA RID: 2042
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
