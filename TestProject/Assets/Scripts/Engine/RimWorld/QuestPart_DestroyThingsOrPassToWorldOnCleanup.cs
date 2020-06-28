using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096F RID: 2415
	public class QuestPart_DestroyThingsOrPassToWorldOnCleanup : QuestPart
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x00130A54 File Offset: 0x0012EC54
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				if (this.questLookTargets)
				{
					int num;
					for (int i = 0; i < this.things.Count; i = num + 1)
					{
						yield return this.things[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x00130A64 File Offset: 0x0012EC64
		public override void Cleanup()
		{
			base.Cleanup();
			QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x00130A78 File Offset: 0x0012EC78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040021B8 RID: 8632
		public List<Thing> things = new List<Thing>();

		// Token: 0x040021B9 RID: 8633
		public bool questLookTargets = true;
	}
}
