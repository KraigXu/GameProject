using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAB RID: 3499
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x06005500 RID: 21760 RVA: 0x001C4AFB File Offset: 0x001C2CFB
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x001C4B18 File Offset: 0x001C2D18
		public void ExposeData()
		{
			Scribe_Collections.Look<PassingShip>(ref this.passingShips, "passingShips", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.passingShips.Count; i++)
				{
					this.passingShips[i].passingShipManager = this;
				}
			}
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x001C4B6B File Offset: 0x001C2D6B
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x001C4B80 File Offset: 0x001C2D80
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x001C4B98 File Offset: 0x001C2D98
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x001C4BD0 File Offset: 0x001C2DD0
		public void RemoveAllShipsOfFaction(Faction faction)
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				if (this.passingShips[i].Faction == faction)
				{
					this.passingShips[i].Depart();
				}
			}
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x001C4C1C File Offset: 0x001C2E1C
		internal void DebugSendAllShipsAway()
		{
			PassingShipManager.tmpPassingShips.Clear();
			PassingShipManager.tmpPassingShips.AddRange(this.passingShips);
			for (int i = 0; i < PassingShipManager.tmpPassingShips.Count; i++)
			{
				PassingShipManager.tmpPassingShips[i].Depart();
			}
			Messages.Message("All passing ships sent away.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x04002E8E RID: 11918
		public Map map;

		// Token: 0x04002E8F RID: 11919
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x04002E90 RID: 11920
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();
	}
}
