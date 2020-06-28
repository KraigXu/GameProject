using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002C8 RID: 712
	public class CrossRefHandler
	{
		// Token: 0x06001417 RID: 5143 RVA: 0x00074AB0 File Offset: 0x00072CB0
		public void RegisterForCrossRefResolve(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for cross ref resolve, but current mode is ",
					Scribe.mode
				}), false);
				return;
			}
			if (s == null)
			{
				return;
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("RegisterForCrossRefResolve " + ((s != null) ? s.GetType().ToString() : "null"));
			}
			this.crossReferencingExposables.Add(s);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00074B34 File Offset: 0x00072D34
		public void ResolveAllCrossReferences()
		{
			Scribe.mode = LoadSaveMode.ResolvingCrossRefs;
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Register the saveables all so we can find them later");
			}
			foreach (IExposable exposable in this.crossReferencingExposables)
			{
				ILoadReferenceable loadReferenceable = exposable as ILoadReferenceable;
				if (loadReferenceable != null)
				{
					if (DebugViewSettings.logMapLoad)
					{
						LogSimple.Message("RegisterLoaded " + loadReferenceable.GetType());
					}
					this.loadedObjectDirectory.RegisterLoaded(loadReferenceable);
				}
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Fill all cross-references to the saveables");
			}
			foreach (IExposable exposable2 in this.crossReferencingExposables)
			{
				if (DebugViewSettings.logMapLoad)
				{
					LogSimple.Message("ResolvingCrossRefs ExposeData " + exposable2.GetType());
				}
				try
				{
					Scribe.loader.curParent = exposable2;
					Scribe.loader.curPathRelToParent = null;
					exposable2.ExposeData();
				}
				catch (Exception arg)
				{
					Log.Error("Could not resolve cross refs: " + arg, false);
				}
			}
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
			this.Clear(true);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00074C94 File Offset: 0x00072E94
		public T TakeResolvedRef<T>(string pathRelToParent, IExposable parent) where T : ILoadReferenceable
		{
			string loadID = this.loadIDs.Take<T>(pathRelToParent, parent);
			return this.loadedObjectDirectory.ObjectWithLoadID<T>(loadID);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00074CBC File Offset: 0x00072EBC
		public T TakeResolvedRef<T>(string toAppendToPathRelToParent) where T : ILoadReferenceable
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRef<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00074CFC File Offset: 0x00072EFC
		public List<T> TakeResolvedRefList<T>(string pathRelToParent, IExposable parent)
		{
			List<string> list = this.loadIDs.TakeList(pathRelToParent, parent);
			List<T> list2 = new List<T>();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(this.loadedObjectDirectory.ObjectWithLoadID<T>(list[i]));
				}
			}
			return list2;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00074D4C File Offset: 0x00072F4C
		public List<T> TakeResolvedRefList<T>(string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRefList<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00074D8A File Offset: 0x00072F8A
		public void Clear(bool errorIfNotEmpty)
		{
			if (errorIfNotEmpty)
			{
				this.loadIDs.ConfirmClear();
			}
			else
			{
				this.loadIDs.Clear();
			}
			this.crossReferencingExposables.Clear();
			this.loadedObjectDirectory.Clear();
		}

		// Token: 0x04000D86 RID: 3462
		private LoadedObjectDirectory loadedObjectDirectory = new LoadedObjectDirectory();

		// Token: 0x04000D87 RID: 3463
		public LoadIDsWantedBank loadIDs = new LoadIDsWantedBank();

		// Token: 0x04000D88 RID: 3464
		public List<IExposable> crossReferencingExposables = new List<IExposable>();
	}
}
