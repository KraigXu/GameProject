using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028F RID: 655
	public class SummaryHealthHandler
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x0006348C File Offset: 0x0006168C
		public float SummaryHealthPercent
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.dirty)
				{
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					float num = 1f;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (!(hediffs[i] is Hediff_MissingPart))
						{
							float num2 = Mathf.Min(hediffs[i].SummaryHealthPercentImpact, 0.95f);
							num *= 1f - num2;
						}
					}
					List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
					{
						float num3 = Mathf.Min(missingPartsCommonAncestors[j].SummaryHealthPercentImpact, 0.95f);
						num *= 1f - num3;
					}
					this.cachedSummaryHealthPercent = Mathf.Clamp(num, 0.05f, 1f);
					this.dirty = false;
				}
				return this.cachedSummaryHealthPercent;
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00063587 File Offset: 0x00061787
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000635A8 File Offset: 0x000617A8
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x04000C78 RID: 3192
		private Pawn pawn;

		// Token: 0x04000C79 RID: 3193
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x04000C7A RID: 3194
		private bool dirty = true;
	}
}
