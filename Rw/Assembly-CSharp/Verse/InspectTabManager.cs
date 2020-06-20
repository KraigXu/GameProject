﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200004A RID: 74
	public static class InspectTabManager
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00012E54 File Offset: 0x00011054
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				return inspectTabBase;
			}
			inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
			InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
			return inspectTabBase;
		}

		// Token: 0x04000104 RID: 260
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}
