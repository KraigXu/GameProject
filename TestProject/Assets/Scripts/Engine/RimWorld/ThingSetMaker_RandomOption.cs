using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCA RID: 3274
	public class ThingSetMaker_RandomOption : ThingSetMaker
	{
		// Token: 0x06004F57 RID: 20311 RVA: 0x001AB73C File Offset: 0x001A993C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].thingSetMaker.CanGenerate(parms) && this.GetSelectionWeight(this.options[i], parms) > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001AB798 File Offset: 0x001A9998
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_RandomOption.Option option;
			if (!(from x in this.options
			where x.thingSetMaker.CanGenerate(parms)
			select x).TryRandomElementByWeight((ThingSetMaker_RandomOption.Option x) => this.GetSelectionWeight(x, parms), out option))
			{
				return;
			}
			outThings.AddRange(option.thingSetMaker.Generate(parms));
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001AB800 File Offset: 0x001A9A00
		private float GetSelectionWeight(ThingSetMaker_RandomOption.Option option, ThingSetMakerParams parms)
		{
			if (option.weightIfPlayerHasNoItem != null && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(option.weightIfPlayerHasNoItemItem, 1))
			{
				return option.weightIfPlayerHasNoItem.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			return option.weight * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x001AB854 File Offset: 0x001A9A54
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x001AB893 File Offset: 0x001A9A93
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			int num2;
			for (int i = 0; i < this.options.Count; i = num2 + 1)
			{
				float num = this.options[i].weight;
				if (this.options[i].weightIfPlayerHasNoItem != null)
				{
					num = Mathf.Max(num, this.options[i].weightIfPlayerHasNoItem.Value);
				}
				if (num > 0f)
				{
					foreach (ThingDef thingDef in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return thingDef;
					}
					IEnumerator<ThingDef> enumerator = null;
				}
				num2 = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04002C88 RID: 11400
		public List<ThingSetMaker_RandomOption.Option> options;

		// Token: 0x02001C17 RID: 7191
		public class Option
		{
			// Token: 0x04006A77 RID: 27255
			public ThingSetMaker thingSetMaker;

			// Token: 0x04006A78 RID: 27256
			public float weight;

			// Token: 0x04006A79 RID: 27257
			public float? weightIfPlayerHasNoItem;

			// Token: 0x04006A7A RID: 27258
			public ThingDef weightIfPlayerHasNoItemItem;
		}
	}
}
