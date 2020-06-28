using System;

namespace Verse
{
	// Token: 0x020000D9 RID: 217
	public abstract class IngredientValueGetter
	{
		// Token: 0x060005FC RID: 1532
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x060005FD RID: 1533
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x060005FE RID: 1534 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
