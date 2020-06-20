using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C13 RID: 3091
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x06004998 RID: 18840 RVA: 0x0018F634 File Offset: 0x0018D834
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x0018F684 File Offset: 0x0018D884
		protected void DoPawnModifierEditInterface(Rect rect)
		{
			Rect rect2 = rect.TopHalf();
			Rect rect3 = rect2.LeftPart(0.333f).Rounded();
			Rect rect4 = rect2.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect3, "chance".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect4, ref this.chance, ref this.chanceBuf, 0f, 1f);
			Rect rect5 = rect.BottomHalf();
			Rect rect6 = rect5.LeftPart(0.333f).Rounded();
			Rect rect7 = rect5.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect6, "context".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonText(rect7, this.context.ToStringHuman(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PawnGenerationContext)))
				{
					PawnGenerationContext localCont2 = (PawnGenerationContext)obj;
					PawnGenerationContext localCont = localCont2;
					list.Add(new FloatMenuOption(localCont.ToStringHuman(), delegate
					{
						this.context = localCont;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x0018F7F8 File Offset: 0x0018D9F8
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x0018F826 File Offset: 0x0018DA26
		public override void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyNewPawn(pawn);
			}
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x0018F865 File Offset: 0x0018DA65
		public override void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyPawnPostGenerate(pawn, redressed);
			}
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x0018F8A8 File Offset: 0x0018DAA8
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.hideOffMap && this.context.Includes(PawnGenerationContext.PlayerStarter))
			{
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
					{
						this.ModifyHideOffMapStartingPawnPostMapGenerate(pawn);
					}
				}
			}
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}

		// Token: 0x040029F2 RID: 10738
		protected float chance = 1f;

		// Token: 0x040029F3 RID: 10739
		protected PawnGenerationContext context;

		// Token: 0x040029F4 RID: 10740
		protected bool hideOffMap;

		// Token: 0x040029F5 RID: 10741
		private string chanceBuf;
	}
}
