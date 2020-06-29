using System;
using RimWorld.Planet;

namespace Verse
{
	
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		
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

		
		public DebugActionAttribute(string category = null, string name = null)
		{
			this.name = name;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		
		public string name;

		
		public string category = "General";

		
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		
		public DebugActionType actionType;
	}
}
