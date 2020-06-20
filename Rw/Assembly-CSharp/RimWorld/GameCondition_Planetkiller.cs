using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020009B9 RID: 2489
	public class GameCondition_Planetkiller : GameCondition
	{
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003B5F RID: 15199 RVA: 0x00139DB4 File Offset: 0x00137FB4
		public override string TooltipString
		{
			get
			{
				Vector2 location;
				if (Find.CurrentMap != null)
				{
					location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
				}
				else
				{
					location = default(Vector2);
				}
				return this.def.LabelCap + "\n" + "\n" + this.Description + ("\n" + "ImpactDate".Translate().CapitalizeFirst() + ": " + GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick + base.Duration), location)) + ("\n" + "TimeLeft".Translate().CapitalizeFirst() + ": " + base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00139EA4 File Offset: 0x001380A4
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksLeft <= 179)
			{
				Find.ActiveLesson.Deactivate();
				if (base.TicksLeft == 179)
				{
					SoundDefOf.PlanetkillerImpact.PlayOneShotOnCamera(null);
				}
				if (base.TicksLeft == 90)
				{
					ScreenFader.StartFade(GameCondition_Planetkiller.FadeColor, 1f);
				}
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x00139EFF File Offset: 0x001380FF
		public override void End()
		{
			base.End();
			this.Impact();
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x00139F0D File Offset: 0x0013810D
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}

		// Token: 0x0400231C RID: 8988
		private const int SoundDuration = 179;

		// Token: 0x0400231D RID: 8989
		private const int FadeDuration = 90;

		// Token: 0x0400231E RID: 8990
		private static readonly Color FadeColor = Color.white;
	}
}
