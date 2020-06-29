﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Tradeable_Pawn : Tradeable
	{
		
		// (get) Token: 0x06005586 RID: 21894 RVA: 0x001C6816 File Offset: 0x001C4A16
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn);
			}
		}

		
		// (get) Token: 0x06005587 RID: 21895 RVA: 0x001C6824 File Offset: 0x001C4A24
		public override string Label
		{
			get
			{
				string text = base.Label;
				if (this.AnyPawn.Name != null && !this.AnyPawn.Name.Numerical)
				{
					text = text + ", " + this.AnyPawn.def.label;
				}
				return string.Concat(new string[]
				{
					text,
					" (",
					this.AnyPawn.GetGenderLabel(),
					", ",
					Mathf.FloorToInt(this.AnyPawn.ageTracker.AgeBiologicalYearsFloat).ToString(),
					")"
				});
			}
		}

		
		// (get) Token: 0x06005588 RID: 21896 RVA: 0x001C68CA File Offset: 0x001C4ACA
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyPawn.MainDesc(true) + "\n\n" + this.AnyPawn.def.description;
			}
		}

		
		// (get) Token: 0x06005589 RID: 21897 RVA: 0x001C6900 File Offset: 0x001C4B00
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerSells)
			{
				List<Pawn> list = this.thingsColony.Take(base.CountToTransferToDestination).Cast<Pawn>().ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					TradeSession.trader.GiveSoldThingToTrader(list[i], 1, TradeSession.playerNegotiator);
				}
				return;
			}
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				List<Pawn> list2 = this.thingsTrader.Take(base.CountToTransferToSource).Cast<Pawn>().ToList<Pawn>();
				for (int j = 0; j < list2.Count; j++)
				{
					TradeSession.trader.GiveSoldThingToPlayer(list2[j], 1, TradeSession.playerNegotiator);
				}
			}
		}
	}
}
