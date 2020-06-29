using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class PowerBeam : OrbitalStrike
	{
		
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		
		public override void Tick()
		{
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.StartRandomFireAndDoFlameDamage();
			}
		}

		
		private void StartRandomFireAndDoFlameDamage()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => 1f - Mathf.Min(x.DistanceTo(base.Position) / 15f, 1f) + 0.05f);
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			PowerBeam.tmpThings.Clear();
			PowerBeam.tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < PowerBeam.tmpThings.Count; i++)
			{
				int num = (PowerBeam.tmpThings[i] is Corpse) ? PowerBeam.CorpseFlameDamageAmountRange.RandomInRange : PowerBeam.FlameDamageAmountRange.RandomInRange;
				Pawn pawn = PowerBeam.tmpThings[i] as Pawn;
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, this.instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}
				PowerBeam.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator, null, this.weaponDef, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			PowerBeam.tmpThings.Clear();
		}

		
		public const float Radius = 15f;

		
		private const int FiresStartedPerTick = 4;

		
		private static readonly IntRange FlameDamageAmountRange = new IntRange(65, 100);

		
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
