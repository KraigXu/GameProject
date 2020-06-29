using System;
using UnityEngine;

namespace Verse
{
	
	public sealed class IngredientCount
	{
		
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x000C0D9B File Offset: 0x000BEF9B
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		
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

		
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x000C0DD1 File Offset: 0x000BEFD1
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(this.count / num);
		}

		
		public float GetBaseCount()
		{
			return this.count;
		}

		
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		
		public ThingFilter filter = new ThingFilter();

		
		private float count = 1f;
	}
}
