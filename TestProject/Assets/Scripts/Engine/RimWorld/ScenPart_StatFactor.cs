using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C24 RID: 3108
	public class ScenPart_StatFactor : ScenPart
	{
		// Token: 0x06004A20 RID: 18976 RVA: 0x001911D0 File Offset: 0x0018F3D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<StatDef>(ref this.stat, "stat");
			Scribe_Values.Look<float>(ref this.factor, "factor", 0f, false);
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x00191200 File Offset: 0x0018F400
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			if (Widgets.ButtonText(scenPartRect.TopHalf(), this.stat.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (StatDef statDef in DefDatabase<StatDef>.AllDefs)
				{
					if (!statDef.forInformationOnly)
					{
						StatDef localSd = statDef;
						list.Add(new FloatMenuOption(localSd.LabelForFullStatListCap, delegate
						{
							this.stat = localSd;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Rect rect = scenPartRect.BottomHalf();
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "multiplier".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect3, ref this.factor, ref this.factorBuf, 0f, 100f);
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x00191334 File Offset: 0x0018F534
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StatFactor".Translate(this.stat.label, this.factor.ToStringPercent());
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x00191368 File Offset: 0x0018F568
		public override void Randomize()
		{
			this.stat = (from d in DefDatabase<StatDef>.AllDefs
			where d.scenarioRandomizable
			select d).RandomElement<StatDef>();
			this.factor = GenMath.RoundedHundredth(Rand.Range(0.1f, 3f));
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x001913C4 File Offset: 0x0018F5C4
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StatFactor scenPart_StatFactor = other as ScenPart_StatFactor;
			if (scenPart_StatFactor != null && scenPart_StatFactor.stat == this.stat)
			{
				this.factor *= scenPart_StatFactor.factor;
				return true;
			}
			return false;
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x001913FF File Offset: 0x0018F5FF
		public float GetStatFactor(StatDef stat)
		{
			if (stat == this.stat)
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x04002A18 RID: 10776
		private StatDef stat;

		// Token: 0x04002A19 RID: 10777
		private float factor;

		// Token: 0x04002A1A RID: 10778
		private string factorBuf;
	}
}
