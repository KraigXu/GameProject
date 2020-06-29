using System;

namespace RimWorld
{
	
	public class GameCondition_DisableElectricity : GameCondition
	{
		
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
