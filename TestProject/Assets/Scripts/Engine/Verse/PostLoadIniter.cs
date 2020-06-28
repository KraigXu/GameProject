using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002CB RID: 715
	public class PostLoadIniter
	{
		// Token: 0x0600142C RID: 5164 RVA: 0x00075628 File Offset: 0x00073828
		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for post load init, but current mode is ",
					Scribe.mode
				}), false);
				return;
			}
			if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.", false);
				return;
			}
			if (this.saveablesToPostLoad.Contains(s))
			{
				Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + s, false);
				return;
			}
			this.saveablesToPostLoad.Add(s);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000756AC File Offset: 0x000738AC
		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			foreach (IExposable exposable in this.saveablesToPostLoad)
			{
				try
				{
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					exposable.ExposeData();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not do PostLoadInit on ",
						exposable.ToStringSafe<IExposable>(),
						": ",
						ex
					}), false);
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0007577C File Offset: 0x0007397C
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x04000D8C RID: 3468
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
