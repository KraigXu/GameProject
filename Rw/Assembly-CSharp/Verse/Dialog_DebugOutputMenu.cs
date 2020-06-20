using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000360 RID: 864
	public class Dialog_DebugOutputMenu : Dialog_DebugOptionLister
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000A0784 File Offset: 0x0009E984
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

		// Token: 0x06001A1E RID: 6686 RVA: 0x000A0868 File Offset: 0x0009EA68
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

		// Token: 0x06001A1F RID: 6687 RVA: 0x000A08F0 File Offset: 0x0009EAF0
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

		// Token: 0x04000F3F RID: 3903
		private List<Dialog_DebugOutputMenu.DebugOutputOption> debugOutputs = new List<Dialog_DebugOutputMenu.DebugOutputOption>();

		// Token: 0x04000F40 RID: 3904
		public const string DefaultCategory = "General";

		// Token: 0x0200160B RID: 5643
		private struct DebugOutputOption
		{
			// Token: 0x040054FB RID: 21755
			public string label;

			// Token: 0x040054FC RID: 21756
			public string category;

			// Token: 0x040054FD RID: 21757
			public Action action;
		}
	}
}
