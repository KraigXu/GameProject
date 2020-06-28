using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAA RID: 3498
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x060054EF RID: 21743 RVA: 0x001C4890 File Offset: 0x001C2A90
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x060054F0 RID: 21744 RVA: 0x001C4897 File Offset: 0x001C2A97
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x060054F1 RID: 21745 RVA: 0x001C48A5 File Offset: 0x001C2AA5
		public Map Map
		{
			get
			{
				if (this.passingShipManager == null)
				{
					return null;
				}
				return this.passingShipManager.map;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x060054F2 RID: 21746 RVA: 0x001C48BC File Offset: 0x001C2ABC
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x001C48C4 File Offset: 0x001C2AC4
		public PassingShip()
		{
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x001C48E9 File Offset: 0x001C2AE9
		public PassingShip(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x001C4918 File Offset: 0x001C2B18
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x001C496C File Offset: 0x001C2B6C
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x001C498C File Offset: 0x001C2B8C
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(this.FullTitle), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x001C49FB File Offset: 0x001C2BFB
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x001C4A03 File Offset: 0x001C2C03
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x00019EA1 File Offset: 0x000180A1
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x00044240 File Offset: 0x00042440
		protected virtual AcceptanceReport CanCommunicateWith_NewTemp(Pawn negotiator)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x001C4A0C File Offset: 0x001C2C0C
		protected virtual bool CanCommunicateWith(Pawn negotiator)
		{
			return this.CanCommunicateWith_NewTemp(negotiator).Accepted;
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x001C4A28 File Offset: 0x001C2C28
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			string label = "CallOnRadio".Translate(this.GetCallLabel());
			Action action = null;
			AcceptanceReport canCommunicate = this.CanCommunicateWith_NewTemp(negotiator);
			if (!canCommunicate.Accepted)
			{
				if (!canCommunicate.Reason.NullOrEmpty())
				{
					action = delegate
					{
						Messages.Message(canCommunicate.Reason, console, MessageTypeDefOf.RejectInput, false);
					};
				}
			}
			else
			{
				action = delegate
				{
					if (!Building_OrbitalTradeBeacon.AllPowered(this.Map).Any<Building_OrbitalTradeBeacon>())
					{
						Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), console, MessageTypeDefOf.RejectInput, false);
						return;
					}
					console.GiveUseCommsJob(negotiator, this);
				};
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), negotiator, console, "ReservedBy");
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x001C4AE4 File Offset: 0x001C2CE4
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		// Token: 0x04002E89 RID: 11913
		public PassingShipManager passingShipManager;

		// Token: 0x04002E8A RID: 11914
		private Faction faction;

		// Token: 0x04002E8B RID: 11915
		public string name = "Nameless";

		// Token: 0x04002E8C RID: 11916
		protected int loadID = -1;

		// Token: 0x04002E8D RID: 11917
		public int ticksUntilDeparture = 40000;
	}
}
