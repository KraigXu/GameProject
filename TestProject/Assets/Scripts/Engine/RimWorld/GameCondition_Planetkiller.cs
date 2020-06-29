using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class GameCondition_Planetkiller : GameCondition
	{
		
		
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

		
		public override void End()
		{
			base.End();
			this.Impact();
		}

		
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}

		
		private const int SoundDuration = 179;

		
		private const int FadeDuration = 90;

		
		private static readonly Color FadeColor = Color.white;
	}
}
