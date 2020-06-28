using System;

namespace Verse
{
	// Token: 0x02000323 RID: 803
	public class CompWindSource : ThingComp
	{
		// Token: 0x0600176C RID: 5996 RVA: 0x00085B5F File Offset: 0x00083D5F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x04000EA9 RID: 3753
		public float wind;
	}
}
