using RimWorld;
using System;
using System.Xml;

namespace Verse
{
	public class BackCompatibilityConverter_Universal : BackCompatibilityConverter
	{
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return true;
		}

		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef))
			{
				if (defName == "WoolYak" || defName == "WoolCamel")
				{
					return "WoolSheep";
				}
				if (defName == "Plant_TreeAnimus" || defName == "Plant_TreeAnimusSmall" || defName == "Plant_TreeAnimaSmall" || defName == "Plant_TreeAnimaNormal" || defName == "Plant_TreeAnimaHardy")
				{
					return "Plant_TreeAnima";
				}
				if (defName == "Psytrainer_EntropyLink")
				{
					return "Psytrainer_EntropyDump";
				}
				if (defName == "PsylinkNeuroformer")
				{
					return "PsychicAmplifier";
				}
			}
			if (defType == typeof(AbilityDef) && defName == "EntropyLink")
			{
				return "EntropyDump";
			}
			if (defType == typeof(HediffDef) && defName == "Psylink")
			{
				return "PsychicAmplifier";
			}
			return null;
		}

		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "Hediff_PsychicAmplifier")
			{
				return typeof(Hediff_Psylink);
			}
			return null;
		}

		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int num = VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
				Pawn_RoyaltyTracker pawn_RoyaltyTracker;
				if ((pawn_RoyaltyTracker = (obj as Pawn_RoyaltyTracker)) != null && num <= 2575)
				{
					foreach (RoyalTitle item in pawn_RoyaltyTracker.AllTitlesForReading)
					{
						item.conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(pawn_RoyaltyTracker.pawn);
					}
				}
				Pawn_NeedsTracker pawn_NeedsTracker;
				if ((pawn_NeedsTracker = (obj as Pawn_NeedsTracker)) != null)
				{
					pawn_NeedsTracker.AllNeeds.RemoveAll((Need n) => n.def.defName == "Authority");
				}
			}
			Pawn pawn;
			if ((pawn = (obj as Pawn)) == null)
			{
				return;
			}
			if (pawn.abilities == null)
			{
				pawn.abilities = new Pawn_AbilityTracker(pawn);
			}
			if (pawn.health != null)
			{
				if (pawn.health.hediffSet.hediffs.RemoveAll((Hediff x) => x == null) != 0)
				{
					Log.Error(pawn.ToStringSafe() + " had some null hediffs.");
				}
				Hediff hediff = pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.PsychicHangover);
				if (hediff != null)
				{
					pawn.health.hediffSet.hediffs.Remove(hediff);
				}
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.WakeUpTolerance);
				if (firstHediffOfDef != null)
				{
					pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef);
				}
				Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.GoJuiceTolerance);
				if (firstHediffOfDef2 != null)
				{
					pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef2);
				}
			}
		}
	}
}
