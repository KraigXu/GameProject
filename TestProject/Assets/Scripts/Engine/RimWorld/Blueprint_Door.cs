using System;

namespace RimWorld
{
	// Token: 0x02000C4C RID: 3148
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x06004B22 RID: 19234 RVA: 0x001958C4 File Offset: 0x00193AC4
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
