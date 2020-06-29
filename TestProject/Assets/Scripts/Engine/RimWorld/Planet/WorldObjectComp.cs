using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class WorldObjectComp
	{
		
		// (get) Token: 0x06006F1E RID: 28446 RVA: 0x0026B1E5 File Offset: 0x002693E5
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		
		// (get) Token: 0x06006F1F RID: 28447 RVA: 0x0026B1F4 File Offset: 0x002693F4
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		
		public virtual void CompTick()
		{
		}

		
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			yield break;
		}

		
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			yield break;
		}

		
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield break;
		}

		
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		
		public virtual void PostPostRemove()
		{
		}

		
		public virtual void PostDestroy()
		{
		}

		
		public virtual void PostMyMapRemoved()
		{
		}

		
		public virtual void PostMapGenerate()
		{
		}

		
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		
		public virtual void PostExposeData()
		{
		}

		
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

		
		public WorldObject parent;

		
		public WorldObjectCompProperties props;
	}
}
