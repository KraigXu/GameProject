using System;

namespace RimWorld
{
	
	public class DeadPlant : Plant
	{
		
		// (get) Token: 0x06004E89 RID: 20105 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06004E8A RID: 20106 RVA: 0x0005AC15 File Offset: 0x00058E15
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06004E8B RID: 20107 RVA: 0x0005AC15 File Offset: 0x00058E15
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06004E8C RID: 20108 RVA: 0x000255AA File Offset: 0x000237AA
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		
		// (get) Token: 0x06004E8D RID: 20109 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		
		public override string GetInspectString()
		{
			return "";
		}

		
		public override void CropBlighted()
		{
		}
	}
}
