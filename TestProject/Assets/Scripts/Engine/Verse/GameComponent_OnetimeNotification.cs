﻿using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	
	public class GameComponent_OnetimeNotification : GameComponent
	{
		
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		
		public override void GameComponentTick()
		{
			if (Find.TickManager.TicksGame % 2000 != 0 || !Rand.Chance(0.05f))
			{
				return;
			}
			if (this.sendAICoreRequestReminder)
			{
				if (ResearchProjectTagDefOf.ShipRelated.CompletedProjects() < 2)
				{
					return;
				}
				if (PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.AIPersonaCore, 1) || PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.Ship_ComputerCore, 1))
				{
					return;
				}
				Faction faction = Find.FactionManager.RandomNonHostileFaction(false, false, true, TechLevel.Undefined);
				if (faction == null || faction.leader == null)
				{
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterLabelAICoreOffer".Translate(), "LetterAICoreOffer".Translate(faction.leader.LabelDefinite(), faction.Name, faction.leader.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, faction, null, null, null);
				this.sendAICoreRequestReminder = false;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		
		public bool sendAICoreRequestReminder = true;
	}
}
