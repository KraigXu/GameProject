using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		
		
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		
		public Dialog_DebugOutputMenu()
		{
			this.forcePause = true;
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugOutputAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
				}
			}
			this.debugOutputs = (from r in this.debugOutputs
			orderby r.category, r.label
			select r).ToList<Dialog_DebugOutputMenu.DebugOutputOption>();
		}

		
		private void GenerateCacheForMethod(MethodInfo method, DebugOutputAttribute attribute)
		{
			if (attribute.onlyWhenPlaying && Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			string label = attribute.name ?? GenText.SplitCamelCase(method.Name);
			Action action = Delegate.CreateDelegate(typeof(Action), method) as Action;
			string text = attribute.category;
			if (text == null)
			{
				text = "General";
			}
			this.debugOutputs.Add(new Dialog_DebugOutputMenu.DebugOutputOption
			{
				label = label,
				category = text,
				action = action
			});
		}

		
		protected override void DoListingItems()
		{
			string b = null;
			foreach (Dialog_DebugOutputMenu.DebugOutputOption debugOutputOption in this.debugOutputs)
			{
				if (debugOutputOption.category != b)
				{
					base.DoLabel(debugOutputOption.category);
					b = debugOutputOption.category;
				}
				Log.openOnMessage = true;
				try
				{
					base.DebugAction(debugOutputOption.label, debugOutputOption.action);
				}
				finally
				{
					Log.openOnMessage = false;
				}
			}
		}

		
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		
		public const string DefaultCategory = "General";

		
		private struct DebugOutputOption
		{
			
			public string label;

			
			public string category;

			
			public Action action;
		}
	}
}
