using System;

namespace RimWorld
{
	// Token: 0x02000A36 RID: 2614
	public static class StoryDangerUtility
	{
		// Token: 0x06003DC8 RID: 15816 RVA: 0x00145F62 File Offset: 0x00144162
		public static float Scale(this StoryDanger d)
		{
			switch (d)
			{
			case StoryDanger.None:
				return 0f;
			case StoryDanger.Low:
				return 1f;
			case StoryDanger.High:
				return 2f;
			default:
				return 0f;
			}
		}
	}
}
