using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C17 RID: 3095
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x060049B8 RID: 18872 RVA: 0x0018FDF4 File Offset: 0x0018DFF4
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
