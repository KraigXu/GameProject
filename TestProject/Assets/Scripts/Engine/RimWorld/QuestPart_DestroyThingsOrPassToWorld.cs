using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
	{
		
		
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

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
			}
		}

		
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

		
		public string inSignal;

		
		public List<Thing> things = new List<Thing>();

		
		public bool questLookTargets = true;
	}
}
