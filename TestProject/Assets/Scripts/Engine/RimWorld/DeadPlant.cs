using System;

namespace RimWorld
{
	// Token: 0x02000CAB RID: 3243
	public class DeadPlant : Plant
	{
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06004E89 RID: 20105 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06004E8A RID: 20106 RVA: 0x0005AC15 File Offset: 0x00058E15
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06004E8B RID: 20107 RVA: 0x0005AC15 File Offset: 0x00058E15
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06004E8C RID: 20108 RVA: 0x000255AA File Offset: 0x000237AA
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004E8D RID: 20109 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x00019EA1 File Offset: 0x000180A1
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x0004EE9A File Offset: 0x0004D09A
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x00002681 File Offset: 0x00000881
		public override void CropBlighted()
		{
		}
	}
}
