using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000A37 RID: 2615
	public class DangerWatcher
	{
		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x00145F8F File Offset: 0x0014418F
		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					this.dangerRatingInt = this.CalculateDangerRating();
					this.lastUpdateTick = Find.TickManager.TicksGame;
				}
				return this.dangerRatingInt;
			}
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00145FC8 File Offset: 0x001441C8
		private StoryDanger CalculateDangerRating()
		{
			float num = (from x in this.map.attackTargetsCache.TargetsHostileToColony
			where this.AffectsStoryDanger(x)
			select x).Sum(delegate(IAttackTarget t)
			{
				Pawn pawn;
				if ((pawn = (t as Pawn)) != null)
				{
					return pawn.kindDef.combatPower;
				}
				Building_TurretGun building_TurretGun;
				if ((building_TurretGun = (t as Building_TurretGun)) != null && building_TurretGun.def.building.IsMortar && !building_TurretGun.IsMannable)
				{
					return building_TurretGun.def.building.combatPower;
				}
				return 0f;
			});
			if (num == 0f)
			{
				return StoryDanger.None;
			}
			int num2 = (from p in this.map.mapPawns.FreeColonistsSpawned
			where !p.Downed
			select p).Count<Pawn>();
			if (num < 150f && num <= (float)num2 * 18f)
			{
				return StoryDanger.Low;
			}
			if (num > 400f)
			{
				return StoryDanger.High;
			}
			if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
			{
				return StoryDanger.High;
			}
			foreach (Lord lord in this.map.lordManager.lords)
			{
				if (lord.faction.HostileTo(Faction.OfPlayer) && lord.CurLordToil.ForceHighStoryDanger && lord.AnyActivePawn)
				{
					return StoryDanger.High;
				}
			}
			return StoryDanger.Low;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00146114 File Offset: 0x00144314
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00146139 File Offset: 0x00144339
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0014614C File Offset: 0x0014434C
		private bool AffectsStoryDanger(IAttackTarget t)
		{
			Pawn pawn = t.Thing as Pawn;
			if (pawn != null)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && (lord.LordJob is LordJob_DefendPoint || lord.LordJob is LordJob_MechanoidDefendBase) && pawn.CurJobDef != JobDefOf.AttackMelee && pawn.CurJobDef != JobDefOf.AttackStatic)
				{
					return false;
				}
				CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
				if (comp != null && !comp.Awake)
				{
					return false;
				}
			}
			return GenHostility.IsActiveThreatToPlayer(t);
		}

		// Token: 0x0400240D RID: 9229
		private Map map;

		// Token: 0x0400240E RID: 9230
		private StoryDanger dangerRatingInt;

		// Token: 0x0400240F RID: 9231
		private int lastUpdateTick = -10000;

		// Token: 0x04002410 RID: 9232
		private int lastColonistHarmedTick = -10000;

		// Token: 0x04002411 RID: 9233
		private const int UpdateInterval = 101;
	}
}
