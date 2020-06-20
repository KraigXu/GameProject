using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF3 RID: 4083
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x060061E6 RID: 25062 RVA: 0x0021FFE8 File Offset: 0x0021E1E8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x00220008 File Offset: 0x0021E208
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetValue(req, out num) && num != 0f)
			{
				return "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x0022004E File Offset: 0x0021E24E
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => this.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x00220084 File Offset: 0x0021E284
		private float GetAddedBodyPartsMass(Pawn p)
		{
			float num = 0f;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_AddedPart hediff_AddedPart = hediffs[i] as Hediff_AddedPart;
				if (hediff_AddedPart != null && hediff_AddedPart.def.spawnThingOnRemoved != null)
				{
					num += hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.9f;
				}
			}
			return num;
		}

		// Token: 0x04003BD4 RID: 15316
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
