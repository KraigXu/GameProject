using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101F RID: 4127
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x060062D9 RID: 25305 RVA: 0x002254C6 File Offset: 0x002236C6
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x002254D0 File Offset: 0x002236D0
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return "Wildness".Translate() + " " + wildness.ToStringPercent() + ": " + this.ValueFromReq(req).ToString("F0");
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x0022553C File Offset: 0x0022373C
		private float ValueFromReq(StatRequest req)
		{
			float wildness = ((ThingDef)req.Def).race.wildness;
			return Mathf.Clamp(GenMath.LerpDouble(0.15f, 1f, 0f, 10f, wildness), 0f, 20f);
		}
	}
}
