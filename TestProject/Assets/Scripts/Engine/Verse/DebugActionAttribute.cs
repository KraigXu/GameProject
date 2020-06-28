using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000357 RID: 855
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0009FF44 File Offset: 0x0009E144
		public bool IsAllowedInCurrentGameState
		{
			get
			{
				bool flag = (this.allowedGameStates & AllowedGameStates.Entry) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Entry;
				bool flag2 = (this.allowedGameStates & AllowedGameStates.Playing) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Playing;
				bool flag3 = (this.allowedGameStates & AllowedGameStates.WorldRenderedNow) == AllowedGameStates.Invalid || WorldRendererUtility.WorldRenderedNow;
				bool flag4 = (this.allowedGameStates & AllowedGameStates.IsCurrentlyOnMap) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null);
				bool flag5 = (this.allowedGameStates & AllowedGameStates.HasGameCondition) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ActiveConditions.Count > 0);
				return flag && flag2 && flag3 && flag4 && flag5;
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0009FFEE File Offset: 0x0009E1EE
		public DebugActionAttribute(string category = null, string name = null)
		{
			this.name = name;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		// Token: 0x04000F2F RID: 3887
		public string name;

		// Token: 0x04000F30 RID: 3888
		public string category = "General";

		// Token: 0x04000F31 RID: 3889
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		// Token: 0x04000F32 RID: 3890
		public DebugActionType actionType;
	}
}
