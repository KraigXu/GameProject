using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000110 RID: 272
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x0600079F RID: 1951 RVA: 0x000237F4 File Offset: 0x000219F4
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00023804 File Offset: 0x00021A04
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

		// Token: 0x060007A1 RID: 1953 RVA: 0x000238EA File Offset: 0x00021AEA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x040006E8 RID: 1768
		public bool sendAICoreRequestReminder = true;
	}
}
