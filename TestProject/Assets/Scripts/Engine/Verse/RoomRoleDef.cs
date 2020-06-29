﻿using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class RoomRoleDef : Def
	{
		
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

		
		public Type workerClass;

		
		private List<RoomStatDef> relatedStats;

		
		[Unsaved(false)]
		private RoomRoleWorker workerInt;
	}
}
