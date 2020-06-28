using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000CF RID: 207
	public class PawnInventoryOption
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x0001C257 File Offset: 0x0001A457
		public IEnumerable<Thing> GenerateThings()
		{
			if (Rand.Value < this.skipChance)
			{
				yield break;
			}
			if (this.thingDef != null && this.countRange.max > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.thingDef, null);
				thing.stackCount = this.countRange.RandomInRange;
				yield return thing;
			}
			if (this.subOptionsTakeAll != null)
			{
				foreach (PawnInventoryOption pawnInventoryOption in this.subOptionsTakeAll)
				{
					foreach (Thing thing2 in pawnInventoryOption.GenerateThings())
					{
						yield return thing2;
					}
					IEnumerator<Thing> enumerator2 = null;
				}
				List<PawnInventoryOption>.Enumerator enumerator = default(List<PawnInventoryOption>.Enumerator);
			}
			if (this.subOptionsChooseOne != null)
			{
				PawnInventoryOption pawnInventoryOption2 = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
				foreach (Thing thing3 in pawnInventoryOption2.GenerateThings())
				{
					yield return thing3;
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x040004C7 RID: 1223
		public ThingDef thingDef;

		// Token: 0x040004C8 RID: 1224
		public IntRange countRange = IntRange.one;

		// Token: 0x040004C9 RID: 1225
		public float choiceChance = 1f;

		// Token: 0x040004CA RID: 1226
		public float skipChance;

		// Token: 0x040004CB RID: 1227
		public List<PawnInventoryOption> subOptionsTakeAll;

		// Token: 0x040004CC RID: 1228
		public List<PawnInventoryOption> subOptionsChooseOne;
	}
}
