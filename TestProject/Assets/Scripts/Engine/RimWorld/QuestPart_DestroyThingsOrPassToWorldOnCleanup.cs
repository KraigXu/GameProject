using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DestroyThingsOrPassToWorldOnCleanup : QuestPart
	{
		
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

		
		public override void Cleanup()
		{
			base.Cleanup();
			QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
		}

		
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

		
		public List<Thing> things = new List<Thing>();

		
		public bool questLookTargets = true;
	}
}
