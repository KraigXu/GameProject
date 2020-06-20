using System;

namespace RimWorld
{
	// Token: 0x020009B6 RID: 2486
	public class GameCondition_DisableElectricity : GameCondition
	{
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ElectricityDisabled
		{
			get
			{
				return true;
			}
		}
	}
}
