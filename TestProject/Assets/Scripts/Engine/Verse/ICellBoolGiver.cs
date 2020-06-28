using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200014F RID: 335
	public interface ICellBoolGiver
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600097E RID: 2430
		Color Color { get; }

		// Token: 0x0600097F RID: 2431
		bool GetCellBool(int index);

		// Token: 0x06000980 RID: 2432
		Color GetCellExtraColor(int index);
	}
}
