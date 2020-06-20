using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097C RID: 2428
	public class QuestPart_InspectString : QuestPartActivable
	{
		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x0013196F File Offset: 0x0012FB6F
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.targets.Count; i = num + 1)
				{
					ISelectable selectable = this.targets[i];
					if (selectable is Thing)
					{
						yield return (Thing)selectable;
					}
					else if (selectable is WorldObject)
					{
						yield return (WorldObject)selectable;
					}
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x0013197F File Offset: 0x0012FB7F
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedInspectString = receivedArgs.GetFormattedText(this.inspectString);
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x001319A5 File Offset: 0x0012FBA5
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.targets.Contains(target))
			{
				return this.resolvedInspectString;
			}
			return null;
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x001319C0 File Offset: 0x0012FBC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ISelectable>(ref this.targets, "targets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Values.Look<string>(ref this.resolvedInspectString, "resolvedInspectString", null, false);
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00131A0D File Offset: 0x0012FC0D
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.targets.Add(Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.inspectString = "Debug inspect string.";
			}
		}

		// Token: 0x040021E1 RID: 8673
		public List<ISelectable> targets = new List<ISelectable>();

		// Token: 0x040021E2 RID: 8674
		public string inspectString;

		// Token: 0x040021E3 RID: 8675
		private string resolvedInspectString;

		// Token: 0x040021E4 RID: 8676
		private ILoadReferenceable targetRef;
	}
}
