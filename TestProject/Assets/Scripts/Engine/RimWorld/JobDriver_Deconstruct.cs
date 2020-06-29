using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x000FB242 File Offset: 0x000F9442
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		
		// (get) Token: 0x06002B72 RID: 11122 RVA: 0x000FB249 File Offset: 0x000F9449
		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true), 20f, 3000f);
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Building == null || !base.Building.DeconstructibleBy(this.pawn.Faction));
			foreach (Toil toil in this.n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		
		protected override void TickAction()
		{
			if (base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f, false);
			}
		}

		
		private const float MaxDeconstructWork = 3000f;

		
		private const float MinDeconstructWork = 20f;
	}
}
