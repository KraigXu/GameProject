using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D19 RID: 3353
	public class CompIngredients : ThingComp
	{
		// Token: 0x06005183 RID: 20867 RVA: 0x001B4F81 File Offset: 0x001B3181
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.ingredients, "ingredients", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ingredients == null)
			{
				this.ingredients = new List<ThingDef>();
			}
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001B4FBA File Offset: 0x001B31BA
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
			}
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x001B4FD8 File Offset: 0x001B31D8
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (piece != this.parent)
			{
				CompIngredients compIngredients = piece.TryGetComp<CompIngredients>();
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					compIngredients.ingredients.Add(this.ingredients[i]);
				}
			}
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x001B502C File Offset: 0x001B322C
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			List<ThingDef> list = otherStack.TryGetComp<CompIngredients>().ingredients;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.ingredients.Contains(list[i]))
				{
					this.ingredients.Add(list[i]);
				}
			}
			if (this.ingredients.Count > 3)
			{
				this.ingredients.Shuffle<ThingDef>();
				while (this.ingredients.Count > 3)
				{
					this.ingredients.Remove(this.ingredients[this.ingredients.Count - 1]);
				}
			}
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x001B50D4 File Offset: 0x001B32D4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ingredients.Count > 0)
			{
				stringBuilder.Append("Ingredients".Translate() + ": ");
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					stringBuilder.Append((i == 0) ? this.ingredients[i].LabelCap.Resolve() : this.ingredients[i].label);
					if (i < this.ingredients.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002D17 RID: 11543
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x04002D18 RID: 11544
		private const int MaxNumIngredients = 3;
	}
}
