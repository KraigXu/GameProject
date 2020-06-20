using System;

namespace Verse
{
	// Token: 0x0200024C RID: 588
	public class HediffComp_DisappearsOnDeath : HediffComp
	{
		// Token: 0x06001049 RID: 4169 RVA: 0x0005D5C7 File Offset: 0x0005B7C7
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			base.Pawn.health.RemoveHediff(this.parent);
		}
	}
}
