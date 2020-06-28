using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000927 RID: 2343
	public interface IArchivable : IExposable, ILoadReferenceable
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060037AE RID: 14254
		Texture ArchivedIcon { get; }

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060037AF RID: 14255
		Color ArchivedIconColor { get; }

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060037B0 RID: 14256
		string ArchivedLabel { get; }

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060037B1 RID: 14257
		string ArchivedTooltip { get; }

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060037B2 RID: 14258
		int CreatedTicksGame { get; }

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060037B3 RID: 14259
		bool CanCullArchivedNow { get; }

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060037B4 RID: 14260
		LookTargets LookTargets { get; }

		// Token: 0x060037B5 RID: 14261
		void OpenArchived();
	}
}
