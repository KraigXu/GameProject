using System;

namespace Verse
{
	// Token: 0x020002D2 RID: 722
	public static class Scribe
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x00076F5B File Offset: 0x0007515B
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00076F78 File Offset: 0x00075178
		public static bool EnterNode(string nodeName)
		{
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				return false;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return Scribe.saver.EnterNode(nodeName);
			}
			return (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00076FC7 File Offset: 0x000751C7
		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.loader.ExitNode();
			}
		}

		// Token: 0x04000DAC RID: 3500
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x04000DAD RID: 3501
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x04000DAE RID: 3502
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
