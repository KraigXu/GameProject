using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		// Token: 0x06003002 RID: 12290 RVA: 0x0010E3DF File Offset: 0x0010C5DF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x0010E3EF File Offset: 0x0010C5EF
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x0010E40C File Offset: 0x0010C60C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter) == null)
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is("IsIncapableOfViolenceShort".Translate(pawn), null);
				return false;
			}
			return true;
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x0010E4A1 File Offset: 0x0010C6A1
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Slaughter, t);
		}
	}
}
