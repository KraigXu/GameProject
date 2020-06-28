using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C3 RID: 195
	public class LogEntryDef : Def
	{
		// Token: 0x060005A3 RID: 1443 RVA: 0x0001BCB2 File Offset: 0x00019EB2
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.iconMiss.NullOrEmpty())
				{
					this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
				}
				if (!this.iconDamaged.NullOrEmpty())
				{
					this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
				}
				if (!this.iconDamagedFromInstigator.NullOrEmpty())
				{
					this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
				}
			});
		}

		// Token: 0x04000436 RID: 1078
		[NoTranslate]
		public string iconMiss;

		// Token: 0x04000437 RID: 1079
		[NoTranslate]
		public string iconDamaged;

		// Token: 0x04000438 RID: 1080
		[NoTranslate]
		public string iconDamagedFromInstigator;

		// Token: 0x04000439 RID: 1081
		[Unsaved(false)]
		public Texture2D iconMissTex;

		// Token: 0x0400043A RID: 1082
		[Unsaved(false)]
		public Texture2D iconDamagedTex;

		// Token: 0x0400043B RID: 1083
		[Unsaved(false)]
		public Texture2D iconDamagedFromInstigatorTex;
	}
}
