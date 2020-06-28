using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001282 RID: 4738
	public abstract class WorldObjectComp
	{
		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06006F1E RID: 28446 RVA: 0x0026B1E5 File Offset: 0x002693E5
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06006F1F RID: 28447 RVA: 0x0026B1F4 File Offset: 0x002693F4
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x06006F20 RID: 28448 RVA: 0x0026B218 File Offset: 0x00269418
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06006F21 RID: 28449 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompTick()
		{
		}

		// Token: 0x06006F22 RID: 28450 RVA: 0x0026B221 File Offset: 0x00269421
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06006F23 RID: 28451 RVA: 0x0026B22A File Offset: 0x0026942A
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06006F24 RID: 28452 RVA: 0x0026B233 File Offset: 0x00269433
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		// Token: 0x06006F25 RID: 28453 RVA: 0x0026B23C File Offset: 0x0026943C
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		// Token: 0x06006F26 RID: 28454 RVA: 0x0026B245 File Offset: 0x00269445
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield break;
		}

		// Token: 0x06006F27 RID: 28455 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06006F28 RID: 28456 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06006F29 RID: 28457 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06006F2A RID: 28458 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06006F2B RID: 28459 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDestroy()
		{
		}

		// Token: 0x06006F2C RID: 28460 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x06006F2D RID: 28461 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x06006F2E RID: 28462 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x06006F2F RID: 28463 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06006F30 RID: 28464 RVA: 0x0026B250 File Offset: 0x00269450
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.Tile : -1,
				")"
			});
		}

		// Token: 0x04004454 RID: 17492
		public WorldObject parent;

		// Token: 0x04004455 RID: 17493
		public WorldObjectCompProperties props;
	}
}
