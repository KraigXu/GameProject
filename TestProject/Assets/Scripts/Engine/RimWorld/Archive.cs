using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Archive : IExposable
	{
		
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x0012AAB6 File Offset: 0x00128CB6
		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<IArchivable>(ref this.archivables, "archivables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<IArchivable>(ref this.pinnedArchivables, "pinnedArchivables", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.archivables.RemoveAll((IArchivable x) => x == null);
				this.pinnedArchivables.RemoveWhere((IArchivable x) => x == null);
			}
		}

		
		public bool Add(IArchivable archivable)
		{
			if (archivable == null)
			{
				Log.Error("Tried to add null archivable.", false);
				return false;
			}
			if (this.Contains(archivable))
			{
				return false;
			}
			this.archivables.Add(archivable);
			this.archivables.SortBy((IArchivable x) => x.CreatedTicksGame);
			this.CheckCullArchivables();
			return true;
		}

		
		public bool Remove(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return false;
			}
			this.archivables.Remove(archivable);
			this.pinnedArchivables.Remove(archivable);
			return true;
		}

		
		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

		
		public void Pin(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return;
			}
			if (this.IsPinned(archivable))
			{
				return;
			}
			this.pinnedArchivables.Add(archivable);
		}

		
		public void Unpin(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return;
			}
			if (!this.IsPinned(archivable))
			{
				return;
			}
			this.pinnedArchivables.Remove(archivable);
		}

		
		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

		
		private void CheckCullArchivables()
		{
			int num = 0;
			for (int i = 0; i < this.archivables.Count; i++)
			{
				if (!this.IsPinned(this.archivables[i]) && this.archivables[i].CanCullArchivedNow)
				{
					num++;
				}
			}
			int num2 = num - 200;
			int num3 = 0;
			while (num3 < this.archivables.Count && num2 > 0)
			{
				if (!this.IsPinned(this.archivables[num3]) && this.archivables[num3].CanCullArchivedNow && this.Remove(this.archivables[num3]))
				{
					num2--;
					num3--;
				}
				num3++;
			}
		}

		
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.archivables.Count; i++)
			{
				LookTargets lookTargets = this.archivables[i].LookTargets;
				if (lookTargets != null)
				{
					lookTargets.Notify_MapRemoved(map);
				}
			}
		}

		
		private List<IArchivable> archivables = new List<IArchivable>();

		
		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		
		public const int MaxNonPinnedArchivables = 200;
	}
}
