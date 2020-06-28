using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200014D RID: 333
	public class AreaManager : IExposable
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x000334D5 File Offset: 0x000316D5
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x000334DD File Offset: 0x000316DD
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x000334E5 File Offset: 0x000316E5
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x000334ED File Offset: 0x000316ED
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x000334F5 File Offset: 0x000316F5
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000334FD File Offset: 0x000316FD
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00033518 File Offset: 0x00031718
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00033572 File Offset: 0x00031772
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00033598 File Offset: 0x00031798
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000335CC File Offset: 0x000317CC
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + area, false);
				return;
			}
			this.areas.Remove(area);
			this.NotifyEveryoneAreaRemoved(area);
			if (Designator_AreaAllowed.SelectedArea == area)
			{
				Designator_AreaAllowed.ClearSelectedArea();
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0003360C File Offset: 0x0003180C
		public Area GetLabeled(string s)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				if (this.areas[i].Label == s)
				{
					return this.areas[i];
				}
			}
			return null;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00033658 File Offset: 0x00031858
		public T Get<T>() where T : Area
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				T t = this.areas[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000336A5 File Offset: 0x000318A5
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x000336D4 File Offset: 0x000318D4
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0003370C File Offset: 0x0003190C
		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0003376C File Offset: 0x0003196C
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000337A1 File Offset: 0x000319A1
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000337D6 File Offset: 0x000319D6
		public bool TryMakeNewAllowed(out Area_Allowed area)
		{
			if (!this.CanMakeNewAllowed())
			{
				area = null;
				return false;
			}
			area = new Area_Allowed(this, null);
			this.areas.Add(area);
			this.SortAreas();
			return true;
		}

		// Token: 0x040007C8 RID: 1992
		public Map map;

		// Token: 0x040007C9 RID: 1993
		private List<Area> areas = new List<Area>();

		// Token: 0x040007CA RID: 1994
		public const int MaxAllowedAreas = 10;
	}
}
