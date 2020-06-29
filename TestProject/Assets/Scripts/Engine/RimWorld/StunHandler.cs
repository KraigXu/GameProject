using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class StunHandler : IExposable
	{
		
		
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0;
			}
		}

		
		
		private int EMPAdaptationTicksDuration
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return 2200;
				}
				return 0;
			}
		}

		
		
		private bool AffectedByEMP
		{
			get
			{
				Pawn pawn;
				return (pawn = (this.parent as Pawn)) == null || !pawn.RaceProps.IsFlesh;
			}
		}

		
		
		public int StunTicksLeft
		{
			get
			{
				return this.stunTicksLeft;
			}
		}

		
		public StunHandler(Thing parent)
		{
			this.parent = parent;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.EMPAdaptedTicksLeft, "EMPAdaptedTicksLeft", 0, false);
		}

		
		public void StunHandlerTick()
		{
			if (this.EMPAdaptedTicksLeft > 0)
			{
				this.EMPAdaptedTicksLeft--;
			}
			if (this.stunTicksLeft > 0)
			{
				this.stunTicksLeft--;
				if (this.moteStun == null || this.moteStun.Destroyed)
				{
					this.moteStun = MoteMaker.MakeStunOverlay(this.parent);
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.Downed)
				{
					this.stunTicksLeft = 0;
				}
				if (this.moteStun != null)
				{
					this.moteStun.Maintain();
				}
				if (this.AffectedByEMP)
				{
					if (this.empEffecter == null)
					{
						this.empEffecter = EffecterDefOf.DisabledByEMP.Spawn();
					}
					this.empEffecter.EffectTick(this.parent, this.parent);
					return;
				}
			}
			else if (this.empEffecter != null)
			{
				this.empEffecter.Cleanup();
			}
		}

		
		public void Notify_DamageApplied(DamageInfo dinfo, bool affectedByEMP)
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && (pawn.Downed || pawn.Dead))
			{
				return;
			}
			if (dinfo.Def == DamageDefOf.Stun)
			{
				this.StunFor(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator, true);
				return;
			}
			if (dinfo.Def == DamageDefOf.EMP && this.AffectedByEMP)
			{
				if (this.EMPAdaptedTicksLeft <= 0)
				{
					this.StunFor(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator, true);
					this.EMPAdaptedTicksLeft = this.EMPAdaptationTicksDuration;
					return;
				}
				MoteMaker.ThrowText(new Vector3((float)this.parent.Position.x + 1f, (float)this.parent.Position.y, (float)this.parent.Position.z + 1f), this.parent.Map, "Adapted".Translate(), Color.white, -1f);
			}
		}

		
		public void StunFor(int ticks, Thing instigator, bool addBattleLog = true)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			if (addBattleLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
			}
		}

		
		public Thing parent;

		
		private int stunTicksLeft;

		
		private Mote moteStun;

		
		private int EMPAdaptedTicksLeft;

		
		private Effecter empEffecter;

		
		public const float StunDurationTicksPerDamage = 30f;
	}
}
