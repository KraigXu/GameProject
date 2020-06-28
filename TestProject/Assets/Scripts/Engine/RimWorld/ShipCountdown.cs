using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D87 RID: 3463
	public static class ShipCountdown
	{
		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06005462 RID: 21602 RVA: 0x001C2A30 File Offset: 0x001C0C30
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x001C2A41 File Offset: 0x001C0C41
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = null;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x001C2A73 File Offset: 0x001C0C73
		public static void InitiateCountdown(string launchString)
		{
			ShipCountdown.shipRoot = null;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = launchString;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x001C2A9A File Offset: 0x001C0C9A
		public static void ShipCountdownUpdate()
		{
			if (ShipCountdown.timeLeft > 0f)
			{
				ShipCountdown.timeLeft -= Time.deltaTime;
				if (ShipCountdown.timeLeft <= 0f)
				{
					ShipCountdown.CountdownEnded();
				}
			}
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x001C2AC9 File Offset: 0x001C0CC9
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x001C2AD8 File Offset: 0x001C0CD8
		private static void CountdownEnded()
		{
			if (ShipCountdown.shipRoot != null)
			{
				List<Building> list = ShipUtility.ShipBuildingsAttachedTo(ShipCountdown.shipRoot).ToList<Building>();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Building building in list)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						stringBuilder.AppendLine("   " + building_CryptosleepCasket.ContainedThing.LabelCap);
						Find.StoryWatcher.statsRecord.colonistsLaunched++;
						TaleRecorder.RecordTale(TaleDefOf.LaunchedShip, new object[]
						{
							building_CryptosleepCasket.ContainedThing
						});
					}
				}
				GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), stringBuilder.ToString()));
				using (List<Building>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Building building2 = enumerator.Current;
						building2.Destroy(DestroyMode.Vanish);
					}
					return;
				}
			}
			if (!ShipCountdown.customLaunchString.NullOrEmpty())
			{
				GameVictoryUtility.ShowCredits(ShipCountdown.customLaunchString);
				return;
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), null));
		}

		// Token: 0x04002E74 RID: 11892
		private static float timeLeft = -1000f;

		// Token: 0x04002E75 RID: 11893
		private static Building shipRoot;

		// Token: 0x04002E76 RID: 11894
		private static string customLaunchString;

		// Token: 0x04002E77 RID: 11895
		private const float InitialTime = 7.2f;
	}
}
