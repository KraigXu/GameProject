using System;

namespace RimWorld
{
	// Token: 0x02000C16 RID: 3094
	public abstract class ScenPart_Rule : ScenPart
	{
		// Token: 0x060049B5 RID: 18869 RVA: 0x0018FDEC File Offset: 0x0018DFEC
		public override void PostGameStart()
		{
			this.ApplyRule();
		}

		// Token: 0x060049B6 RID: 18870
		protected abstract void ApplyRule();
	}
}
