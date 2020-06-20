using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000508 RID: 1288
	public class SoundParams
	{
		// Token: 0x1700075E RID: 1886
		public float this[string key]
		{
			get
			{
				return this.storedParams[key];
			}
			set
			{
				this.storedParams[key] = value;
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000DB081 File Offset: 0x000D9281
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x04001667 RID: 5735
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x04001668 RID: 5736
		public SoundSizeAggregator sizeAggregator;
	}
}
