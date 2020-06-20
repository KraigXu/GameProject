using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000613 RID: 1555
	public struct TriggerSignal
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x000F6AA8 File Offset: 0x000F4CA8
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000F6AB8 File Offset: 0x000F4CB8
		public TriggerSignal(TriggerSignalType type)
		{
			this.type = type;
			this.memo = null;
			this.thing = null;
			this.dinfo = default(DamageInfo);
			this.condition = PawnLostCondition.Undefined;
			this.faction = null;
			this.clamorType = null;
			this.previousRelationKind = null;
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000F6B07 File Offset: 0x000F4D07
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000F6B10 File Offset: 0x000F4D10
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000F6B30 File Offset: 0x000F4D30
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			stringBuilder.Append(this.type.ToString());
			if (this.memo != null)
			{
				stringBuilder.Append(", memo=" + this.memo);
			}
			if (this.Pawn != null)
			{
				stringBuilder.Append(", pawn=" + this.Pawn);
			}
			if (this.dinfo.Def != null)
			{
				stringBuilder.Append(", dinfo=" + this.dinfo);
			}
			if (this.condition != PawnLostCondition.Undefined)
			{
				stringBuilder.Append(", condition=" + this.condition);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04001945 RID: 6469
		public TriggerSignalType type;

		// Token: 0x04001946 RID: 6470
		public string memo;

		// Token: 0x04001947 RID: 6471
		public Thing thing;

		// Token: 0x04001948 RID: 6472
		public DamageInfo dinfo;

		// Token: 0x04001949 RID: 6473
		public PawnLostCondition condition;

		// Token: 0x0400194A RID: 6474
		public Faction faction;

		// Token: 0x0400194B RID: 6475
		public FactionRelationKind? previousRelationKind;

		// Token: 0x0400194C RID: 6476
		public ClamorDef clamorType;
	}
}
