﻿using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	
	public struct TriggerSignal
	{
		
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x000F6AA8 File Offset: 0x000F4CA8
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		
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

		
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000F6B07 File Offset: 0x000F4D07
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		
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

		
		public TriggerSignalType type;

		
		public string memo;

		
		public Thing thing;

		
		public DamageInfo dinfo;

		
		public PawnLostCondition condition;

		
		public Faction faction;

		
		public FactionRelationKind? previousRelationKind;

		
		public ClamorDef clamorType;
	}
}
