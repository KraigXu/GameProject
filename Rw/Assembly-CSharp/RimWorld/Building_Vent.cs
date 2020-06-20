using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6F RID: 3183
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x001997C0 File Offset: 0x001979C0
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x001997CD File Offset: 0x001979CD
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x001997E3 File Offset: 0x001979E3
		public override void TickRare()
		{
			if (FlickUtility.WantsToBeOn(this))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			}
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x001997FC File Offset: 0x001979FC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (!FlickUtility.WantsToBeOn(this))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("VentClosed".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002AEE RID: 10990
		private CompFlickable flickableComp;
	}
}
