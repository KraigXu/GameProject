using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003CD RID: 973
	public interface ISelectable
	{
		// Token: 0x06001C9F RID: 7327
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x06001CA0 RID: 7328
		string GetInspectString();

		// Token: 0x06001CA1 RID: 7329
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}
