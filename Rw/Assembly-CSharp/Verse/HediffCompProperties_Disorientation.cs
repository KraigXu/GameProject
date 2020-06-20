using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200024F RID: 591
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x0005DAAA File Offset: 0x0005BCAA
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0005DAD4 File Offset: 0x0005BCD4
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.wanderMtbHours <= 0f)
			{
				yield return "wanderMtbHours must be greater than zero";
			}
			if (this.singleWanderDurationTicks <= 0)
			{
				yield return "singleWanderDurationTicks must be greater than zero";
			}
			if (this.wanderRadius <= 0f)
			{
				yield return "wanderRadius must be greater than zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000BF0 RID: 3056
		public float wanderMtbHours = -1f;

		// Token: 0x04000BF1 RID: 3057
		public float wanderRadius;

		// Token: 0x04000BF2 RID: 3058
		public int singleWanderDurationTicks = -1;
	}
}
