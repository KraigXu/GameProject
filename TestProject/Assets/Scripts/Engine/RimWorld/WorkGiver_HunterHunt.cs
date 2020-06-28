using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074A RID: 1866
	public class WorkGiver_HunterHunt : WorkGiver_Scanner
	{
		// Token: 0x060030E4 RID: 12516 RVA: 0x001120A4 File Offset: 0x001102A4
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x001120B4 File Offset: 0x001102B4
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) || WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn) || !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Hunt);
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x001120E4 File Offset: 0x001102E4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2.AnimalOrWildMan() && pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) != null;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x00112134 File Offset: 0x00110334
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Hunt, t);
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x00112148 File Offset: 0x00110348
		public static bool HasHuntingWeapon(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon && p.equipment.PrimaryEq.PrimaryVerb.HarmsHealth() && !p.equipment.PrimaryEq.PrimaryVerb.UsesExplosiveProjectiles();
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x001121AC File Offset: 0x001103AC
		public static bool HasShieldAndRangedWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && p.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i] is ShieldBelt)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
