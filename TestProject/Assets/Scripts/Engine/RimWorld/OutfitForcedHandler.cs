using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA4 RID: 2980
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x00178F85 File Offset: 0x00177185
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x00178F95 File Offset: 0x00177195
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00178F9D File Offset: 0x0017719D
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x00178FAA File Offset: 0x001771AA
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x00178FBB File Offset: 0x001771BB
		public void SetForced(Apparel ap, bool forced)
		{
			if (forced)
			{
				if (!this.forcedAps.Contains(ap))
				{
					this.forcedAps.Add(ap);
					return;
				}
			}
			else if (this.forcedAps.Contains(ap))
			{
				this.forcedAps.Remove(ap);
			}
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00178FF6 File Offset: 0x001771F6
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x00179010 File Offset: 0x00177210
		public bool IsForced(Apparel ap)
		{
			if (ap.Destroyed)
			{
				Log.Error("Apparel was forced while Destroyed: " + ap, false);
				if (this.forcedAps.Contains(ap))
				{
					this.forcedAps.Remove(ap);
				}
				return false;
			}
			return this.forcedAps.Contains(ap);
		}

		// Token: 0x04002829 RID: 10281
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
