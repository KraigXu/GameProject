using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFF RID: 4095
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x06006217 RID: 25111 RVA: 0x0022094C File Offset: 0x0021EB4C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06006218 RID: 25112 RVA: 0x0022096C File Offset: 0x0021EB6C
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x002209AC File Offset: 0x0021EBAC
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}
	}
}
