using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000359 RID: 857
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000A0020 File Offset: 0x0009E220
		public Dialog_DebugActionsMenu()
		{
			this.forcePause = true;
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugActionAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
					DebugActionYielderAttribute debugActionYielderAttribute;
					if (methodInfo.TryGetAttribute(out debugActionYielderAttribute))
					{
						foreach (Dialog_DebugActionsMenu.DebugActionOption item in ((IEnumerable<Dialog_DebugActionsMenu.DebugActionOption>)methodInfo.Invoke(null, null)))
						{
							this.debugActions.Add(item);
						}
					}
				}
			}
			this.debugActions = (from r in this.debugActions
			orderby DebugActionCategories.GetOrderFor(r.category), r.category
			select r).ToList<Dialog_DebugActionsMenu.DebugActionOption>();
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x000A0164 File Offset: 0x0009E364
		private void GenerateCacheForMethod(MethodInfo method, DebugActionAttribute attribute)
		{
			if (!attribute.IsAllowedInCurrentGameState)
			{
				return;
			}
			string text = string.IsNullOrEmpty(attribute.name) ? GenText.SplitCamelCase(method.Name) : attribute.name;
			if (attribute.actionType == DebugActionType.ToolMap || attribute.actionType == DebugActionType.ToolMapForPawns || attribute.actionType == DebugActionType.ToolWorld)
			{
				text = "T: " + text;
			}
			string category = attribute.category;
			Dialog_DebugActionsMenu.DebugActionOption item = new Dialog_DebugActionsMenu.DebugActionOption
			{
				label = text,
				category = category,
				actionType = attribute.actionType
			};
			if (attribute.actionType == DebugActionType.ToolMapForPawns)
			{
				item.pawnAction = (Delegate.CreateDelegate(typeof(Action<Pawn>), method) as Action<Pawn>);
			}
			else
			{
				item.action = (Delegate.CreateDelegate(typeof(Action), method) as Action);
			}
			this.debugActions.Add(item);
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x000A0240 File Offset: 0x0009E440
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			string b = null;
			foreach (Dialog_DebugActionsMenu.DebugActionOption debugActionOption in this.debugActions)
			{
				if (debugActionOption.category != b)
				{
					base.DoGap();
					base.DoLabel(debugActionOption.category);
					b = debugActionOption.category;
				}
				Log.openOnMessage = true;
				try
				{
					switch (debugActionOption.actionType)
					{
					case DebugActionType.Action:
						base.DebugAction(debugActionOption.label, debugActionOption.action);
						break;
					case DebugActionType.ToolMap:
						base.DebugToolMap(debugActionOption.label, debugActionOption.action);
						break;
					case DebugActionType.ToolMapForPawns:
						base.DebugToolMapForPawns(debugActionOption.label, debugActionOption.pawnAction);
						break;
					case DebugActionType.ToolWorld:
						base.DebugToolWorld(debugActionOption.label, debugActionOption.action);
						break;
					}
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		// Token: 0x04000F33 RID: 3891
		private List<Dialog_DebugActionsMenu.DebugActionOption> debugActions = new List<Dialog_DebugActionsMenu.DebugActionOption>();

		// Token: 0x02001605 RID: 5637
		public struct DebugActionOption
		{
			// Token: 0x040054ED RID: 21741
			public DebugActionType actionType;

			// Token: 0x040054EE RID: 21742
			public string label;

			// Token: 0x040054EF RID: 21743
			public string category;

			// Token: 0x040054F0 RID: 21744
			public Action action;

			// Token: 0x040054F1 RID: 21745
			public Action<Pawn> pawnAction;
		}
	}
}
