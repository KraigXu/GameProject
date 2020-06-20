using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200040F RID: 1039
	public static class LookTargetsUtility
	{
		// Token: 0x06001EF4 RID: 7924 RVA: 0x000BF4CE File Offset: 0x000BD6CE
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000BF4DB File Offset: 0x000BD6DB
		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			if (lookTargets == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			return lookTargets.PrimaryTarget;
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000BF4EC File Offset: 0x000BD6EC
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets == null)
			{
				return;
			}
			lookTargets.Highlight(arrow, colonistBar, circleOverlay);
		}
	}
}
