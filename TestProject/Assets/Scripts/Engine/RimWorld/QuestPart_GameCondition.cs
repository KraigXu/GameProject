using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_GameCondition : QuestPart
	{
		
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x0013653C File Offset: 0x0013473C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && (this.targetWorld || this.mapParent != null) && this.gameCondition != null)
			{
				this.gameCondition.quest = this.quest;
				if (this.targetWorld)
				{
					Find.World.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				else
				{
					this.mapParent.Map.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				if (this.sendStandardLetter)
				{
					Find.LetterStack.ReceiveLetter(this.gameCondition.LabelCap, this.gameCondition.LetterText, this.gameCondition.def.letterDef, LookTargets.Invalid, null, this.quest, null, null);
				}
				this.gameCondition = null;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<GameCondition>(ref this.gameCondition, "gameCondition", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.targetWorld, "targetWorld", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.gameCondition = GameConditionMaker.MakeCondition(GameConditionDefOf.ColdSnap, Rand.RangeInclusive(2500, 7500));
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		
		public string inSignal;

		
		public GameCondition gameCondition;

		
		public bool targetWorld;

		
		public MapParent mapParent;

		
		public bool sendStandardLetter = true;
	}
}
