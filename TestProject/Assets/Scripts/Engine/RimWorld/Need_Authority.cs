using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B85 RID: 2949
	public class Need_Authority : Need
	{
		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06004532 RID: 17714 RVA: 0x001761CE File Offset: 0x001743CE
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (this.IsCurrentlyReigning || this.IsCurrentlyGivingSpeech)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06004533 RID: 17715 RVA: 0x001761F0 File Offset: 0x001743F0
		public AuthorityCategory CurCategory
		{
			get
			{
				float curLevel = this.CurLevel;
				if (curLevel < 0.01f)
				{
					return AuthorityCategory.Gone;
				}
				if (curLevel < 0.15f)
				{
					return AuthorityCategory.Weak;
				}
				if (curLevel < 0.3f)
				{
					return AuthorityCategory.Uncertain;
				}
				if (curLevel > 0.7f && curLevel < 0.85f)
				{
					return AuthorityCategory.Strong;
				}
				if (curLevel >= 0.85f)
				{
					return AuthorityCategory.Total;
				}
				return AuthorityCategory.Normal;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06004534 RID: 17716 RVA: 0x00176244 File Offset: 0x00174444
		public bool IsActive
		{
			get
			{
				return this.pawn.royalty != null && this.pawn.Spawned && this.pawn.Map != null && this.pawn.Map.IsPlayerHome && this.pawn.royalty.CanRequireThroneroom();
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06004535 RID: 17717 RVA: 0x001762A3 File Offset: 0x001744A3
		protected override bool IsFrozen
		{
			get
			{
				return this.pawn.Map == null || !this.pawn.Map.IsPlayerHome || this.FallPerDay <= 0f;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004536 RID: 17718 RVA: 0x001762D8 File Offset: 0x001744D8
		public float FallPerDay
		{
			get
			{
				if (this.pawn.royalty == null || !this.pawn.Spawned)
				{
					return 0f;
				}
				if (this.pawn.Map == null || !this.pawn.Map.IsPlayerHome)
				{
					return 0f;
				}
				float num = 0f;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
				}
				int num2 = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count<Pawn>();
				return num * this.FallFactorCurve.Evaluate((float)num2);
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x001763B0 File Offset: 0x001745B0
		public override bool ShowOnNeedList
		{
			get
			{
				return this.IsActive;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06004538 RID: 17720 RVA: 0x001763B8 File Offset: 0x001745B8
		public bool IsCurrentlyReigning
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.Reign;
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06004539 RID: 17721 RVA: 0x001763CC File Offset: 0x001745CC
		public bool IsCurrentlyGivingSpeech
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.GiveSpeech;
			}
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x001763E0 File Offset: 0x001745E0
		public Need_Authority(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00176444 File Offset: 0x00174644
		public override void NeedInterval()
		{
			float num = 400f;
			float num2 = this.FallPerDay / num;
			if (this.IsFrozen)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count <= 1)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsCurrentlyReigning)
			{
				this.CurLevel += 2f / num;
				return;
			}
			if (this.IsCurrentlyGivingSpeech)
			{
				this.CurLevel += 3f / num;
				return;
			}
			this.CurLevel -= num2;
		}

		// Token: 0x0400278C RID: 10124
		public const float LevelGainPerDayOfReigning = 2f;

		// Token: 0x0400278D RID: 10125
		public const float LevelGainPerDayOfGivingSpeech = 3f;

		// Token: 0x0400278E RID: 10126
		private readonly SimpleCurve FallFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			},
			{
				new CurvePoint(5f, 1f),
				true
			}
		};
	}
}
