using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public static class ShipCountdown
	{
		
		// (get) Token: 0x06005462 RID: 21602 RVA: 0x001C2A30 File Offset: 0x001C0C30
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = null;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		
		public static void InitiateCountdown(string launchString)
		{
			ShipCountdown.shipRoot = null;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = launchString;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		
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

		
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
		}

		
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

		
		private static float timeLeft = -1000f;

		
		private static Building shipRoot;

		
		private static string customLaunchString;

		
		private const float InitialTime = 7.2f;
	}
}
