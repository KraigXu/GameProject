using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Tradeable_RoyalFavor : Tradeable
	{
		
		// (get) Token: 0x0600558C RID: 21900 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsFavor
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600558D RID: 21901 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsCurrency
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600558E RID: 21902 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TraderWillTrade
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x0600558F RID: 21903 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool IsThing
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06005590 RID: 21904 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06005591 RID: 21905 RVA: 0x00019EA1 File Offset: 0x000180A1
		public override Thing AnyThing
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06005592 RID: 21906 RVA: 0x001C69C0 File Offset: 0x001C4BC0
		public override string Label
		{
			get
			{
				return TradeSession.trader.Faction.def.royalFavorLabel;
			}
		}

		
		// (get) Token: 0x06005593 RID: 21907 RVA: 0x001C69D6 File Offset: 0x001C4BD6
		public override string TipDescription
		{
			get
			{
				return "RoyalFavorDescription".Translate(TradeSession.trader.Faction.Named("FACTION"));
			}
		}

		
		public override int CostToInt(float cost)
		{
			return Mathf.CeilToInt(cost);
		}

		
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				TradeSession.playerNegotiator.royalty.GainFavor(TradeSession.trader.Faction, base.CountToTransferToSource);
			}
		}

		
		public override void DrawIcon(Rect iconRect)
		{
			Faction faction = TradeSession.trader.Faction;
			GUI.color = faction.Color;
			Widgets.DrawTextureRotated(iconRect, faction.def.FactionIcon, 0f);
			GUI.color = Color.white;
		}

		
		public override int CountHeldBy(Transactor trans)
		{
			if (trans == Transactor.Trader)
			{
				return 99999;
			}
			return 0;
		}

		
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
