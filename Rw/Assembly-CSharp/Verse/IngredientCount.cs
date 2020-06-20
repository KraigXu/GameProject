using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000417 RID: 1047
	public sealed class IngredientCount
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x000C0D9B File Offset: 0x000BEF9B
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000C0DAB File Offset: 0x000BEFAB
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + this, false);
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x000C0DD1 File Offset: 0x000BEFD1
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000C0DF4 File Offset: 0x000BEFF4
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000C0E1B File Offset: 0x000BF01B
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000C0E23 File Offset: 0x000BF023
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000C0E2C File Offset: 0x000BF02C
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000C0E39 File Offset: 0x000BF039
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x04001316 RID: 4886
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04001317 RID: 4887
		private float count = 1f;
	}
}
