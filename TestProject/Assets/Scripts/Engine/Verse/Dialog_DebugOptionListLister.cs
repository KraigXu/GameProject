using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200035C RID: 860
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x06001A0D RID: 6669 RVA: 0x000A037B File Offset: 0x0009E57B
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000A0390 File Offset: 0x0009E590
		protected override void DoListingItems()
		{
			foreach (DebugMenuOption debugMenuOption in this.options)
			{
				if (debugMenuOption.mode == DebugMenuOptionMode.Action)
				{
					base.DebugAction(debugMenuOption.label, debugMenuOption.method);
				}
				if (debugMenuOption.mode == DebugMenuOptionMode.Tool)
				{
					base.DebugToolMap(debugMenuOption.label, debugMenuOption.method);
				}
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000A0414 File Offset: 0x0009E614
		public static void ShowSimpleDebugMenu<T>(IEnumerable<T> elements, Func<T, string> label, Action<T> chosen)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<T> enumerator = elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					list.Add(new DebugMenuOption(label(t), DebugMenuOptionMode.Action, delegate
					{
						chosen(t);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x04000F3A RID: 3898
		protected List<DebugMenuOption> options;
	}
}
