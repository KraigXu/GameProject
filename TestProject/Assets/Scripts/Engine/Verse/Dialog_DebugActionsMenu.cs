using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		
		
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		
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

		
		private List<Dialog_DebugActionsMenu.DebugActionOption> debugActions = new List<Dialog_DebugActionsMenu.DebugActionOption>();

		
		public struct DebugActionOption
		{
			
			public DebugActionType actionType;

			
			public string label;

			
			public string category;

			
			public Action action;

			
			public Action<Pawn> pawnAction;
		}
	}
}
