using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A4 RID: 2468
	public class QuestPart_GameCondition : QuestPart
	{
		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x0013653C File Offset: 0x0013473C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
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

		// Token: 0x06003A9C RID: 15004 RVA: 0x0013654C File Offset: 0x0013474C
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

		// Token: 0x06003A9D RID: 15005 RVA: 0x00136638 File Offset: 0x00134838
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<GameCondition>(ref this.gameCondition, "gameCondition", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.targetWorld, "targetWorld", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x001366A8 File Offset: 0x001348A8
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

		// Token: 0x04002294 RID: 8852
		public string inSignal;

		// Token: 0x04002295 RID: 8853
		public GameCondition gameCondition;

		// Token: 0x04002296 RID: 8854
		public bool targetWorld;

		// Token: 0x04002297 RID: 8855
		public MapParent mapParent;

		// Token: 0x04002298 RID: 8856
		public bool sendStandardLetter = true;
	}
}
