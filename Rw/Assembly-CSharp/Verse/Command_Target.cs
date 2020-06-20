using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200038F RID: 911
	public class Command_Target : Command
	{
		// Token: 0x06001AE3 RID: 6883 RVA: 0x000A5583 File Offset: 0x000A3783
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x04000FE7 RID: 4071
		public Action<Thing> action;

		// Token: 0x04000FE8 RID: 4072
		public TargetingParameters targetingParams;
	}
}
