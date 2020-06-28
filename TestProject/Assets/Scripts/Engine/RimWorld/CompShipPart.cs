using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D86 RID: 3462
	public class CompShipPart : ThingComp
	{
		// Token: 0x0600545F RID: 21599 RVA: 0x001C2950 File Offset: 0x001C0B50
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				action = new Action(this.ShowReport),
				defaultLabel = "CommandShipLaunchReport".Translate(),
				defaultDesc = "CommandShipLaunchReportDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true)
			};
			yield break;
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x001C2960 File Offset: 0x001C0B60
		public void ShowReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!ShipUtility.LaunchFailReasons((Building)this.parent).Any<string>())
			{
				stringBuilder.AppendLine("ShipReportCanLaunch".Translate());
			}
			else
			{
				stringBuilder.AppendLine("ShipReportCannotLaunch".Translate());
				foreach (string value in ShipUtility.LaunchFailReasons((Building)this.parent))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(value);
				}
			}
			Dialog_MessageBox window = new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null);
			Find.WindowStack.Add(window);
		}
	}
}
