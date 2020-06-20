using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000EA RID: 234
	public class SongDef : Def
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x0001DE2C File Offset: 0x0001C02C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001DE7A File Offset: 0x0001C07A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		// Token: 0x04000569 RID: 1385
		[NoTranslate]
		public string clipPath;

		// Token: 0x0400056A RID: 1386
		public float volume = 1f;

		// Token: 0x0400056B RID: 1387
		public bool playOnMap = true;

		// Token: 0x0400056C RID: 1388
		public float commonality = 1f;

		// Token: 0x0400056D RID: 1389
		public bool tense;

		// Token: 0x0400056E RID: 1390
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x0400056F RID: 1391
		public List<Season> allowedSeasons;

		// Token: 0x04000570 RID: 1392
		public RoyalTitleDef minRoyalTitle;

		// Token: 0x04000571 RID: 1393
		[Unsaved(false)]
		public AudioClip clip;
	}
}
