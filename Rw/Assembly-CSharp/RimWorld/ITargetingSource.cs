using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB9 RID: 3769
	public interface ITargetingSource
	{
		// Token: 0x06005C10 RID: 23568
		bool CanHitTarget(LocalTargetInfo target);

		// Token: 0x06005C11 RID: 23569
		bool ValidateTarget(LocalTargetInfo target);

		// Token: 0x06005C12 RID: 23570
		void DrawHighlight(LocalTargetInfo target);

		// Token: 0x06005C13 RID: 23571
		void OrderForceTarget(LocalTargetInfo target);

		// Token: 0x06005C14 RID: 23572
		void OnGUI(LocalTargetInfo target);

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06005C15 RID: 23573
		bool CasterIsPawn { get; }

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06005C16 RID: 23574
		bool IsMeleeAttack { get; }

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06005C17 RID: 23575
		bool Targetable { get; }

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06005C18 RID: 23576
		bool MultiSelect { get; }

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06005C19 RID: 23577
		Thing Caster { get; }

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06005C1A RID: 23578
		Pawn CasterPawn { get; }

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06005C1B RID: 23579
		Verb GetVerb { get; }

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06005C1C RID: 23580
		Texture2D UIIcon { get; }

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06005C1D RID: 23581
		TargetingParameters targetParams { get; }

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06005C1E RID: 23582
		ITargetingSource DestinationSelector { get; }
	}
}
