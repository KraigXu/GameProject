using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public interface IArchivable : IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x060037AE RID: 14254
		Texture ArchivedIcon { get; }

		
		// (get) Token: 0x060037AF RID: 14255
		Color ArchivedIconColor { get; }

		
		// (get) Token: 0x060037B0 RID: 14256
		string ArchivedLabel { get; }

		
		// (get) Token: 0x060037B1 RID: 14257
		string ArchivedTooltip { get; }

		
		// (get) Token: 0x060037B2 RID: 14258
		int CreatedTicksGame { get; }

		
		// (get) Token: 0x060037B3 RID: 14259
		bool CanCullArchivedNow { get; }

		
		// (get) Token: 0x060037B4 RID: 14260
		LookTargets LookTargets { get; }

		
		void OpenArchived();
	}
}
