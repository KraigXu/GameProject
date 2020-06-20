using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD8 RID: 2776
	public class CompAbilityEffect_Teleport : CompAbilityEffect_WithDest
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x0015F086 File Offset: 0x0015D286
		public new CompProperties_AbilityTeleport Props
		{
			get
			{
				return (CompProperties_AbilityTeleport)this.props;
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0015F094 File Offset: 0x0015D294
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				LocalTargetInfo destination = base.GetDestination(dest.IsValid ? dest : target);
				if (destination.IsValid)
				{
					Pawn pawn = this.parent.pawn;
					Vector3 drawPos = target.Thing.DrawPos;
					target.Thing.Position = destination.Cell;
					Pawn pawn2 = target.Thing as Pawn;
					if (pawn2 != null)
					{
						pawn2.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false);
						pawn2.Notify_Teleported(true, true);
					}
					if (this.Props.destClamorType != null)
					{
						GenClamor.DoClamor(pawn, target.Cell, (float)this.Props.destClamorRadius, this.Props.destClamorType);
					}
					MoteMaker.MakeConnectingLine(drawPos, target.Thing.DrawPos, ThingDefOf.Mote_PsycastSkipLine, pawn.Map, 1f);
					MoteMaker.MakeStaticMote(drawPos, pawn.Map, ThingDefOf.Mote_PsycastSkipEffectSource, 1f);
				}
			}
		}
	}
}
