using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000E1 RID: 225
	public class RoomRoleDef : Def
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001DA77 File Offset: 0x0001BC77
		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
		public bool IsStatRelated(RoomStatDef def)
		{
			if (this.relatedStats == null)
			{
				return false;
			}
			for (int i = 0; i < this.relatedStats.Count; i++)
			{
				if (this.relatedStats[i] == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000549 RID: 1353
		public Type workerClass;

		// Token: 0x0400054A RID: 1354
		private List<RoomStatDef> relatedStats;

		// Token: 0x0400054B RID: 1355
		[Unsaved(false)]
		private RoomRoleWorker workerInt;
	}
}
