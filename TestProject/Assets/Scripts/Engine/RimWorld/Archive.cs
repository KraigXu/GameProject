using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000925 RID: 2341
	public class Archive : IExposable
	{
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x0012AAB6 File Offset: 0x00128CB6
		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0012AAC0 File Offset: 0x00128CC0
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

		// Token: 0x06003799 RID: 14233 RVA: 0x0012AB54 File Offset: 0x00128D54
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

		// Token: 0x0600379A RID: 14234 RVA: 0x0012ABB9 File Offset: 0x00128DB9
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

		// Token: 0x0600379B RID: 14235 RVA: 0x0012ABE1 File Offset: 0x00128DE1
		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x0012ABEF File Offset: 0x00128DEF
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

		// Token: 0x0600379D RID: 14237 RVA: 0x0012AC12 File Offset: 0x00128E12
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

		// Token: 0x0600379E RID: 14238 RVA: 0x0012AC35 File Offset: 0x00128E35
		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0012AC44 File Offset: 0x00128E44
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

		// Token: 0x060037A0 RID: 14240 RVA: 0x0012ACFC File Offset: 0x00128EFC
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

		// Token: 0x040020F0 RID: 8432
		private List<IArchivable> archivables = new List<IArchivable>();

		// Token: 0x040020F1 RID: 8433
		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		// Token: 0x040020F2 RID: 8434
		public const int MaxNonPinnedArchivables = 200;
	}
}
