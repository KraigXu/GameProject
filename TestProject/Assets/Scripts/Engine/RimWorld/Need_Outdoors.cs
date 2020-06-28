using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B96 RID: 2966
	public class Need_Outdoors : Need
	{
		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600458A RID: 17802 RVA: 0x00177C18 File Offset: 0x00175E18
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600458B RID: 17803 RVA: 0x00177C30 File Offset: 0x00175E30
		public OutdoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.8f)
				{
					return OutdoorsCategory.Free;
				}
				if (this.CurLevel > 0.6f)
				{
					return OutdoorsCategory.NeedFreshAir;
				}
				if (this.CurLevel > 0.4f)
				{
					return OutdoorsCategory.CabinFeverLight;
				}
				if (this.CurLevel > 0.2f)
				{
					return OutdoorsCategory.CabinFeverSevere;
				}
				if (this.CurLevel > 0.05f)
				{
					return OutdoorsCategory.Trapped;
				}
				return OutdoorsCategory.Entombed;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x0600458C RID: 17804 RVA: 0x00177C89 File Offset: 0x00175E89
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x0600458D RID: 17805 RVA: 0x00177C94 File Offset: 0x00175E94
		private bool Disabled
		{
			get
			{
				return this.pawn.story.traits.HasTrait(TraitDefOf.Undergrounder);
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x00177CB0 File Offset: 0x00175EB0
		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00177D1F File Offset: 0x00175F1F
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x00177D2C File Offset: 0x00175F2C
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			float b = 0.2f;
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float num;
			if (!flag)
			{
				if (roofDef == null)
				{
					num = 5f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = -0.32f;
				}
				else
				{
					num = -0.45f;
					b = 0f;
				}
			}
			else if (roofDef == null)
			{
				num = 8f;
			}
			else if (roofDef.isThickRoof)
			{
				num = -0.4f;
			}
			else
			{
				num = 1f;
			}
			if (this.pawn.InBed() && num < 0f)
			{
				num *= 0.2f;
			}
			num *= 0.0025f;
			float curLevel = this.CurLevel;
			if (num < 0f)
			{
				this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num, b));
			}
			else
			{
				this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
			}
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x040027E8 RID: 10216
		private const float Delta_IndoorsThickRoof = -0.45f;

		// Token: 0x040027E9 RID: 10217
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x040027EA RID: 10218
		private const float Delta_IndoorsThinRoof = -0.32f;

		// Token: 0x040027EB RID: 10219
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x040027EC RID: 10220
		private const float Delta_OutdoorsThinRoof = 1f;

		// Token: 0x040027ED RID: 10221
		private const float Delta_IndoorsNoRoof = 5f;

		// Token: 0x040027EE RID: 10222
		private const float Delta_OutdoorsNoRoof = 8f;

		// Token: 0x040027EF RID: 10223
		private const float DeltaFactor_InBed = 0.2f;

		// Token: 0x040027F0 RID: 10224
		private float lastEffectiveDelta;
	}
}
