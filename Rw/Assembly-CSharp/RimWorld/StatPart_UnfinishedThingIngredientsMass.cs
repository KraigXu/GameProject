using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100F RID: 4111
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x0600625F RID: 25183 RVA: 0x00221998 File Offset: 0x0021FB98
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x002219B8 File Offset: 0x0021FBB8
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x06006261 RID: 25185 RVA: 0x002219F8 File Offset: 0x0021FBF8
		private bool TryGetValue(StatRequest req, out float value)
		{
			UnfinishedThing unfinishedThing = req.Thing as UnfinishedThing;
			if (unfinishedThing == null)
			{
				value = 0f;
				return false;
			}
			float num = 0f;
			for (int i = 0; i < unfinishedThing.ingredients.Count; i++)
			{
				num += unfinishedThing.ingredients[i].GetStatValue(StatDefOf.Mass, true) * (float)unfinishedThing.ingredients[i].stackCount;
			}
			value = num;
			return true;
		}
	}
}
