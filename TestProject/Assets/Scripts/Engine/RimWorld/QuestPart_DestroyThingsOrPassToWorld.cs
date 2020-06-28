using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096E RID: 2414
	public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x001308C2 File Offset: 0x0012EAC2
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

		// Token: 0x06003932 RID: 14642 RVA: 0x001308D2 File Offset: 0x0012EAD2
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
			}
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x001308FC File Offset: 0x0012EAFC
		public static void Destroy(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Thing thing;
				if (things[i].ParentHolder is MinifiedThing)
				{
					thing = (Thing)things[i].ParentHolder;
				}
				else
				{
					thing = things[i];
				}
				if (!thing.Destroyed)
				{
					thing.DestroyOrPassToWorld(DestroyMode.QuestLogic);
				}
			}
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x00130958 File Offset: 0x0012EB58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x001309D8 File Offset: 0x0012EBD8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				List<Thing> source = Find.RandomPlayerHomeMap.listerThings.ThingsInGroup(ThingRequestGroup.Plant);
				this.things.Clear();
				this.things.Add(source.FirstOrDefault<Thing>());
			}
		}

		// Token: 0x040021B5 RID: 8629
		public string inSignal;

		// Token: 0x040021B6 RID: 8630
		public List<Thing> things = new List<Thing>();

		// Token: 0x040021B7 RID: 8631
		public bool questLookTargets = true;
	}
}
