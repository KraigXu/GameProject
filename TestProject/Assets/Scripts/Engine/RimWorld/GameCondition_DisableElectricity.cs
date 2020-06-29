using System;

namespace RimWorld
{
	
	public class GameCondition_DisableElectricity : GameCondition
	{
		
		
		public override bool ElectricityDisabled
		{
			get
			{
				return true;
			}
		}
	}
}
