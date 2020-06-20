using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA7 RID: 3239
	public class StunHandler : IExposable
	{
		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004E4F RID: 20047 RVA: 0x001A524A File Offset: 0x001A344A
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004E50 RID: 20048 RVA: 0x001A5258 File Offset: 0x001A3458
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

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004E51 RID: 20049 RVA: 0x001A5288 File Offset: 0x001A3488
		private bool AffectedByEMP
		{
			get
			{
				Pawn pawn;
				return (pawn = (this.parent as Pawn)) == null || !pawn.RaceProps.IsFlesh;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06004E52 RID: 20050 RVA: 0x001A52B4 File Offset: 0x001A34B4
		public int StunTicksLeft
		{
			get
			{
				return this.stunTicksLeft;
			}
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x001A52BC File Offset: 0x001A34BC
		public StunHandler(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x001A52CB File Offset: 0x001A34CB
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.EMPAdaptedTicksLeft, "EMPAdaptedTicksLeft", 0, false);
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x001A52F4 File Offset: 0x001A34F4
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

		// Token: 0x06004E56 RID: 20054 RVA: 0x001A53E0 File Offset: 0x001A35E0
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

		// Token: 0x06004E57 RID: 20055 RVA: 0x001A54FB File Offset: 0x001A36FB
		public void StunFor(int ticks, Thing instigator, bool addBattleLog = true)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			if (addBattleLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
			}
		}

		// Token: 0x04002C04 RID: 11268
		public Thing parent;

		// Token: 0x04002C05 RID: 11269
		private int stunTicksLeft;

		// Token: 0x04002C06 RID: 11270
		private Mote moteStun;

		// Token: 0x04002C07 RID: 11271
		private int EMPAdaptedTicksLeft;

		// Token: 0x04002C08 RID: 11272
		private Effecter empEffecter;

		// Token: 0x04002C09 RID: 11273
		public const float StunDurationTicksPerDamage = 30f;
	}
}
