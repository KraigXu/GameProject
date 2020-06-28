using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB4 RID: 3252
	public class Spark : Projectile
	{
		// Token: 0x06004EDE RID: 20190 RVA: 0x001A8D54 File Offset: 0x001A6F54
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
