using System;

namespace RimWorld.Planet
{
	// Token: 0x02001278 RID: 4728
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06006EE0 RID: 28384 RVA: 0x0026A7E8 File Offset: 0x002689E8
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06006EE1 RID: 28385 RVA: 0x0026A808 File Offset: 0x00268A08
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft;
		}
	}
}
