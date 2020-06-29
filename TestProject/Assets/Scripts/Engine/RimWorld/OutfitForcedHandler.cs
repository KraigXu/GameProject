using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class OutfitForcedHandler : IExposable
	{
		
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x00178F85 File Offset: 0x00177185
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x00178F95 File Offset: 0x00177195
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, Array.Empty<object>());
		}

		
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

		
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
