using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class CompRoyalImplant : ThingComp
	{
		
		// (get) Token: 0x060052BE RID: 21182 RVA: 0x001BA426 File Offset: 0x001B8626
		public CompProperties_RoyalImplant Props
		{
			get
			{
				return (CompProperties_RoyalImplant)this.props;
			}
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				Faction first = minTitleForImplantAllFactions.First;
				StringBuilder stringBuilder = new StringBuilder("Stat_Thing_MinimumRoyalTitle_Desc".Translate(first.Named("FACTION")));
				if (typeof(Hediff_ImplantWithLevel).IsAssignableFrom(this.Props.implantHediff.hediffClass))
				{
					stringBuilder.Append("\n\n" + "Stat_Thing_MinimumRoyalTitle_ImplantWithLevel_Desc".Translate(first.Named("FACTION")) + ":\n\n");
					int num = 1;
					while ((float)num <= this.Props.implantHediff.maxSeverity)
					{
						stringBuilder.Append(string.Concat(new object[]
						{
							" -  x",
							num,
							", ",
							first.GetMinTitleForImplant(this.Props.implantHediff, num).GetLabelCapForBothGenders(),
							"\n"
						}));
						num++;
					}
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(first.Named("FACTION")).Resolve(), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders(), stringBuilder.ToTaggedString().Resolve(), 2100, null, null, false);
			}
			yield break;
		}

		
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				stringBuilder.AppendLine("MinimumRoyalTitleInspectString".Translate(minTitleForImplantAllFactions.First.Named("FACTION"), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders().Named("TITLE")).Resolve());
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		
		public static TaggedString CheckForViolations(Pawn pawn, HediffDef hediff, int levelOffset)
		{
			if (levelOffset < 0)
			{
				return "";
			}
			if (pawn.Faction != Faction.OfPlayer || !hediff.HasComp(typeof(HediffComp_RoyalImplant)))
			{
				return "";
			}
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = (Hediff_ImplantWithLevel)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == hediff);
			int num = (levelOffset != 0 && hediff_ImplantWithLevel != null) ? (hediff_ImplantWithLevel.level + levelOffset) : 0;
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (pawn.Faction != null && !faction.def.hidden && !faction.HostileTo(Faction.OfPlayer) && ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(hediff, pawn, faction, num))
				{
					RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(hediff, num);
					HediffCompProperties_RoyalImplant hediffCompProperties_RoyalImplant = hediff.CompProps<HediffCompProperties_RoyalImplant>();
					string arg = hediff.label + ((num == 0) ? "" : (" (" + num + "x)"));
					TaggedString taggedString = hediffCompProperties_RoyalImplant.violationTriggerDescriptionKey.Translate(pawn.Named("PAWN"));
					TaggedString taggedString2 = "RoyalImplantIllegalUseWarning".Translate(pawn.Named("PAWN"), arg.Named("IMPLANT"), faction.Named("FACTION"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE"), taggedString.Named("VIOLATIONTRIGGER"));
					if (levelOffset != 0)
					{
						taggedString2 += "\n\n" + "RoyalImplantUpgradeConfirmation".Translate();
					}
					else
					{
						taggedString2 += "\n\n" + "RoyalImplantInstallConfirmation".Translate();
					}
					return taggedString2;
				}
			}
			return "";
		}
	}
}
