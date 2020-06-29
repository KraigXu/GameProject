﻿using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		
		
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		
		
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		
		
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

		
		
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		
		public PassingShip()
		{
		}

		
		public PassingShip(Faction faction)
		{
			this.faction = faction;
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(this.FullTitle), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		
		protected virtual AcceptanceReport CanCommunicateWith_NewTemp(Pawn negotiator)
		{
			return AcceptanceReport.WasAccepted;
		}

		
		protected virtual bool CanCommunicateWith(Pawn negotiator)
		{
			return this.CanCommunicateWith_NewTemp(negotiator).Accepted;
		}

		
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

		
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		
		public PassingShipManager passingShipManager;

		
		private Faction faction;

		
		public string name = "Nameless";

		
		protected int loadID = -1;

		
		public int ticksUntilDeparture = 40000;
	}
}
