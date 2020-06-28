using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001000 RID: 4096
	public class StatPart_Glow : StatPart
	{
		// Token: 0x0600621B RID: 25115 RVA: 0x002209FE File Offset: 0x0021EBFE
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x0600621C RID: 25116 RVA: 0x00220A0E File Offset: 0x0021EC0E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x00220A3C File Offset: 0x0021EC3C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_LightMultiplier".Translate(this.GlowLevel(req.Thing).ToStringPercent()) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x00220AAC File Offset: 0x0021ECAC
		private bool ActiveFor(Thing t)
		{
			if (this.humanlikeOnly)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && !pawn.RaceProps.Humanlike)
				{
					return false;
				}
			}
			return t.Spawned;
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00220AE0 File Offset: 0x0021ECE0
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GameGlowAt(t.Position, false);
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x00220AF9 File Offset: 0x0021ECF9
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x04003BDF RID: 15327
		private bool humanlikeOnly;

		// Token: 0x04003BE0 RID: 15328
		private SimpleCurve factorFromGlowCurve;
	}
}
