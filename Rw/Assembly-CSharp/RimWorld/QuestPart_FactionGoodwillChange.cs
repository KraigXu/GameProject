using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099C RID: 2460
	[StaticConstructorOnStartup]
	public class QuestPart_FactionGoodwillChange : QuestPart
	{
		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003A67 RID: 14951 RVA: 0x00135557 File Offset: 0x00133757
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.lookTarget;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x00135567 File Offset: 0x00133767
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__1())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00135578 File Offset: 0x00133778
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				GlobalTargetInfo value;
				if (this.lookTarget.IsValid)
				{
					value = this.lookTarget;
				}
				else if (this.getLookTargetFromSignal)
				{
					LookTargets lookTargets;
					if (SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
					{
						value = lookTargets.TryGetPrimaryTarget();
					}
					else
					{
						value = GlobalTargetInfo.Invalid;
					}
				}
				else
				{
					value = GlobalTargetInfo.Invalid;
				}
				FactionRelationKind playerRelationKind = this.faction.PlayerRelationKind;
				int goodwillChange = 0;
				if (!signal.args.TryGetArg<int>("GOODWILL", out goodwillChange))
				{
					goodwillChange = this.change;
				}
				this.faction.TryAffectGoodwillWith(Faction.OfPlayer, goodwillChange, this.canSendMessage, this.canSendHostilityLetter, signal.args.GetFormattedText(this.reason), new GlobalTargetInfo?(value));
				TaggedString t = "";
				this.faction.TryAppendRelationKindChangedInfo(ref t, playerRelationKind, this.faction.PlayerRelationKind, null);
				if (!t.NullOrEmpty())
				{
					t = "\n\n" + t;
				}
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x001356AC File Offset: 0x001338AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.change, "change", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetFromSignal, "getLookTargetFromSignal", true, false);
			Scribe_TargetInfo.Look(ref this.lookTarget, "lookTarget");
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x0013574C File Offset: 0x0013394C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.change = -15;
			this.faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
		}

		// Token: 0x0400226E RID: 8814
		public string inSignal;

		// Token: 0x0400226F RID: 8815
		public int change;

		// Token: 0x04002270 RID: 8816
		public Faction faction;

		// Token: 0x04002271 RID: 8817
		public bool canSendMessage = true;

		// Token: 0x04002272 RID: 8818
		public bool canSendHostilityLetter = true;

		// Token: 0x04002273 RID: 8819
		public string reason;

		// Token: 0x04002274 RID: 8820
		public bool getLookTargetFromSignal = true;

		// Token: 0x04002275 RID: 8821
		public GlobalTargetInfo lookTarget;
	}
}
