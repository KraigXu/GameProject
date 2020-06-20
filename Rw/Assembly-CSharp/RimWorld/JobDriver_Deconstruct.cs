using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000632 RID: 1586
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x000FB242 File Offset: 0x000F9442
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002B72 RID: 11122 RVA: 0x000FB249 File Offset: 0x000F9449
		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true), 20f, 3000f);
			}
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000FB26B File Offset: 0x000F946B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Building == null || !base.Building.DeconstructibleBy(this.pawn.Faction));
			foreach (Toil toil in this.<>n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000FB27B File Offset: 0x000F947B
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000FB2A0 File Offset: 0x000F94A0
		protected override void TickAction()
		{
			if (base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f, false);
			}
		}

		// Token: 0x0400199E RID: 6558
		private const float MaxDeconstructWork = 3000f;

		// Token: 0x0400199F RID: 6559
		private const float MinDeconstructWork = 20f;
	}
}
