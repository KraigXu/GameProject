using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000969 RID: 2409
	public class QuestPart_CameraJump : QuestPart
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003910 RID: 14608 RVA: 0x00130121 File Offset: 0x0012E321
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x00130134 File Offset: 0x0012E334
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				if (lookTargets.IsValid())
				{
					if (this.select)
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget());
						return;
					}
					CameraJumper.TryJump(lookTargets.TryGetPrimaryTarget());
				}
			}
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x001301AC File Offset: 0x0012E3AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.select, "select", true, false);
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0013020C File Offset: 0x0012E40C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.lookTargets = Find.Maps.SelectMany((Map x) => x.mapPawns.FreeColonistsSpawned).RandomElementWithFallback(null);
		}

		// Token: 0x040021A5 RID: 8613
		public string inSignal;

		// Token: 0x040021A6 RID: 8614
		public LookTargets lookTargets;

		// Token: 0x040021A7 RID: 8615
		public bool getLookTargetsFromSignal = true;

		// Token: 0x040021A8 RID: 8616
		public bool select = true;
	}
}
