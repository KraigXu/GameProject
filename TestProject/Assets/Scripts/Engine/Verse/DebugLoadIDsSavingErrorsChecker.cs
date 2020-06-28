using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D0 RID: 720
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x06001445 RID: 5189 RVA: 0x00076845 File Offset: 0x00074A45
		public void Clear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			this.deepSaved.Clear();
			this.referenced.Clear();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00076868 File Offset: 0x00074A68
		public void CheckForErrorsAndClear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (!Scribe.saver.savingForDebug)
			{
				foreach (DebugLoadIDsSavingErrorsChecker.ReferencedObject referencedObject in this.referenced)
				{
					if (!this.deepSaved.Contains(referencedObject.loadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"Object with load ID ",
							referencedObject.loadID,
							" is referenced (xml node name: ",
							referencedObject.label,
							") but is not deep-saved. This will cause errors during loading."
						}), false);
					}
				}
			}
			this.Clear();
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0007691C File Offset: 0x00074B1C
		public void RegisterDeepSaved(object obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					obj,
					", but current mode is ",
					Scribe.mode
				}), false);
				return;
			}
			if (obj == null)
			{
				return;
			}
			ILoadReferenceable loadReferenceable = obj as ILoadReferenceable;
			if (loadReferenceable != null)
			{
				try
				{
					if (!this.deepSaved.Add(loadReferenceable.GetUniqueLoadID()))
					{
						Log.Warning(string.Concat(new string[]
						{
							"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
							loadReferenceable.GetUniqueLoadID(),
							", but it's already here. label=",
							label,
							" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
						}), false);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error in GetUniqueLoadID(): " + arg, false);
				}
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x000769EC File Offset: 0x00074BEC
		public void RegisterReferenced(ILoadReferenceable obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					obj,
					", but current mode is ",
					Scribe.mode
				}), false);
				return;
			}
			if (obj == null)
			{
				return;
			}
			try
			{
				this.referenced.Add(new DebugLoadIDsSavingErrorsChecker.ReferencedObject(obj.GetUniqueLoadID(), label));
			}
			catch (Exception arg)
			{
				Log.Error("Error in GetUniqueLoadID(): " + arg, false);
			}
		}

		// Token: 0x04000DA2 RID: 3490
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x04000DA3 RID: 3491
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x02001493 RID: 5267
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x06007B1C RID: 31516 RVA: 0x0029A277 File Offset: 0x00298477
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06007B1D RID: 31517 RVA: 0x0029A287 File Offset: 0x00298487
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06007B1E RID: 31518 RVA: 0x0029A29F File Offset: 0x0029849F
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06007B1F RID: 31519 RVA: 0x0029A2C7 File Offset: 0x002984C7
			public override int GetHashCode()
			{
				return Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.loadID), this.label);
			}

			// Token: 0x06007B20 RID: 31520 RVA: 0x0029A2E0 File Offset: 0x002984E0
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06007B21 RID: 31521 RVA: 0x0029A2EA File Offset: 0x002984EA
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04004DF6 RID: 19958
			public string loadID;

			// Token: 0x04004DF7 RID: 19959
			public string label;
		}
	}
}
