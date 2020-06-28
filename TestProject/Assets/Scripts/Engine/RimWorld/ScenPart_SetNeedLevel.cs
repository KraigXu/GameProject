using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C14 RID: 3092
	public class ScenPart_SetNeedLevel : ScenPart_PawnModifier
	{
		// Token: 0x060049A2 RID: 18850 RVA: 0x0018F94C File Offset: 0x0018DB4C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.need.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<NeedDef>(this.PossibleNeeds(), (NeedDef hd) => hd.LabelCap, (NeedDef n) => delegate
				{
					this.need = n;
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.levelRange, 0f, 1f, "ConfigurableLevel", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x0018FA32 File Offset: 0x0018DC32
		private IEnumerable<NeedDef> PossibleNeeds()
		{
			return from x in DefDatabase<NeedDef>.AllDefsListForReading
			where x.major
			select x;
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x0018FA60 File Offset: 0x0018DC60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<FloatRange>(ref this.levelRange, "levelRange", default(FloatRange), false);
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x0018FAA0 File Offset: 0x0018DCA0
		public override string Summary(Scenario scen)
		{
			return "ScenPart_SetNeed".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.need.label, this.levelRange.min.ToStringPercent(), this.levelRange.max.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x0018FB20 File Offset: 0x0018DD20
		public override void Randomize()
		{
			base.Randomize();
			this.need = this.PossibleNeeds().RandomElement<NeedDef>();
			this.levelRange.max = Rand.Range(0f, 1f);
			this.levelRange.min = this.levelRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x0018FB84 File Offset: 0x0018DD84
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_SetNeedLevel scenPart_SetNeedLevel = other as ScenPart_SetNeedLevel;
			if (scenPart_SetNeedLevel != null && this.need == scenPart_SetNeedLevel.need)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_SetNeedLevel.chance);
				return true;
			}
			return false;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x0018FBC4 File Offset: 0x0018DDC4
		protected override void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
			if (p.needs != null)
			{
				Need need = p.needs.TryGetNeed(this.need);
				if (need != null)
				{
					need.CurLevelPercentage = this.levelRange.RandomInRange;
				}
			}
		}

		// Token: 0x040029F6 RID: 10742
		private NeedDef need;

		// Token: 0x040029F7 RID: 10743
		private FloatRange levelRange;
	}
}
