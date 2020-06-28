using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC5 RID: 3525
	public class Tradeable_RoyalFavor : Tradeable
	{
		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x0600558C RID: 21900 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsFavor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x0600558D RID: 21901 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsCurrency
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x0600558E RID: 21902 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TraderWillTrade
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x0600558F RID: 21903 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool IsThing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06005590 RID: 21904 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06005591 RID: 21905 RVA: 0x00019EA1 File Offset: 0x000180A1
		public override Thing AnyThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06005592 RID: 21906 RVA: 0x001C69C0 File Offset: 0x001C4BC0
		public override string Label
		{
			get
			{
				return TradeSession.trader.Faction.def.royalFavorLabel;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06005593 RID: 21907 RVA: 0x001C69D6 File Offset: 0x001C4BD6
		public override string TipDescription
		{
			get
			{
				return "RoyalFavorDescription".Translate(TradeSession.trader.Faction.Named("FACTION"));
			}
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x001C69FB File Offset: 0x001C4BFB
		public override int CostToInt(float cost)
		{
			return Mathf.CeilToInt(cost);
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x001C6A03 File Offset: 0x001C4C03
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				TradeSession.playerNegotiator.royalty.GainFavor(TradeSession.trader.Faction, base.CountToTransferToSource);
			}
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x001C6A30 File Offset: 0x001C4C30
		public override void DrawIcon(Rect iconRect)
		{
			Faction faction = TradeSession.trader.Faction;
			GUI.color = faction.Color;
			Widgets.DrawTextureRotated(iconRect, faction.def.FactionIcon, 0f);
			GUI.color = Color.white;
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x001C6A73 File Offset: 0x001C4C73
		public override int CountHeldBy(Transactor trans)
		{
			if (trans == Transactor.Trader)
			{
				return 99999;
			}
			return 0;
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
