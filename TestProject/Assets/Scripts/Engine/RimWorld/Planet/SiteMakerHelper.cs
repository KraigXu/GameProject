using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	
	public static class SiteMakerHelper
	{
		
		public static bool TryFindSiteParams_SingleSitePart(IEnumerable<SitePartDef> singleSitePartCandidates, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			faction = factionToUse;
			if (singleSitePartCandidates != null)
			{
				if (!SiteMakerHelper.TryFindNewRandomSitePartFor(null, singleSitePartCandidates, faction, out sitePart, disallowNonHostileFactions, extraFactionValidator))
				{
					return false;
				}
			}
			else
			{
				sitePart = null;
			}
			return faction != null || SiteMakerHelper.TryFindRandomFactionFor((sitePart != null) ? Gen.YieldSingle<SitePartDef>(sitePart) : null, out faction, disallowNonHostileFactions, extraFactionValidator);
		}

		
		public static bool TryFindSiteParams_SingleSitePart(string singleSitePartTag, out SitePartDef sitePart, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			return SiteMakerHelper.TryFindSiteParams_SingleSitePart(SiteMakerHelper.SitePartDefsWithTag(singleSitePartTag), out sitePart, out faction, factionToUse, disallowNonHostileFactions, extraFactionValidator);
		}

		
		public static bool TryFindSiteParams_MultipleSiteParts(IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates, out List<SitePartDef> siteParts, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			faction = factionToUse;
			siteParts = new List<SitePartDef>();
			if (sitePartsCandidates != null)
			{
				foreach (IEnumerable<SitePartDef> enumerable in sitePartsCandidates)
				{
					if (enumerable != null)
					{
						SitePartDef sitePartDef;
						if (!SiteMakerHelper.TryFindNewRandomSitePartFor(siteParts, enumerable, faction, out sitePartDef, disallowNonHostileFactions, extraFactionValidator))
						{
							return false;
						}
						if (sitePartDef != null)
						{
							siteParts.Add(sitePartDef);
						}
					}
				}
			}
			return faction != null || SiteMakerHelper.TryFindRandomFactionFor(siteParts, out faction, disallowNonHostileFactions, extraFactionValidator);
		}

		
		public static bool TryFindSiteParams_MultipleSiteParts(IEnumerable<string> sitePartsTags, out List<SitePartDef> siteParts, out Faction faction, Faction factionToUse = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			return SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(from x in sitePartsTags
			where x != null
			select SiteMakerHelper.SitePartDefsWithTag(x), out siteParts, out faction, factionToUse, disallowNonHostileFactions, extraFactionValidator);
		}

		
		public static bool TryFindNewRandomSitePartFor(IEnumerable<SitePartDef> existingSiteParts, IEnumerable<SitePartDef> possibleSiteParts, Faction faction, out SitePartDef sitePart, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			if (faction != null)
			{
				if ((from x in possibleSiteParts
				where x == null || ((existingSiteParts == null || existingSiteParts.All((SitePartDef p) => p != x && p.CompatibleWith(x))) && SiteMakerHelper.FactionCanOwn(x, faction, disallowNonHostileFactions, extraFactionValidator))
				select x).TryRandomElement(out sitePart))
				{
					return true;
				}
			}
			else
			{
				SiteMakerHelper.possibleFactions.Clear();
				SiteMakerHelper.possibleFactions.Add(null);
				SiteMakerHelper.possibleFactions.AddRange(Find.FactionManager.AllFactionsListForReading);
				if ((from x in possibleSiteParts
				where x == null || ((existingSiteParts == null || existingSiteParts.All((SitePartDef p) => p != x && p.CompatibleWith(x))) && SiteMakerHelper.possibleFactions.Any((Faction fac) => SiteMakerHelper.FactionCanOwn(existingSiteParts, fac, disallowNonHostileFactions, extraFactionValidator) && SiteMakerHelper.FactionCanOwn(x, fac, disallowNonHostileFactions, extraFactionValidator)))
				select x).TryRandomElement(out sitePart))
				{
					SiteMakerHelper.possibleFactions.Clear();
					return true;
				}
				SiteMakerHelper.possibleFactions.Clear();
			}
			sitePart = null;
			return false;
		}

		
		public static bool TryFindRandomFactionFor(IEnumerable<SitePartDef> parts, out Faction faction, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null)
		{
			if (SiteMakerHelper.FactionCanOwn(parts, null, disallowNonHostileFactions, extraFactionValidator))
			{
				faction = null;
				return true;
			}
			if ((from x in Find.FactionManager.AllFactionsListForReading
			where SiteMakerHelper.FactionCanOwn(parts, x, disallowNonHostileFactions, extraFactionValidator)
			select x).TryRandomElement(out faction))
			{
				return true;
			}
			faction = null;
			return false;
		}

		
		public static void GenerateDefaultParams(float points, int tile, Faction faction, IEnumerable<SitePartDef> siteParts, out List<SitePartDefWithParams> sitePartDefsWithParams)
		{
			int num = 0;
			if (siteParts != null)
			{
				using (IEnumerator<SitePartDef> enumerator = siteParts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.wantsThreatPoints)
						{
							num++;
						}
					}
				}
			}
			float num2 = (num != 0) ? (points / (float)num) : 0f;
			if (siteParts != null)
			{
				List<SitePartDefWithParams> list = new List<SitePartDefWithParams>();
				foreach (SitePartDef sitePartDef in siteParts)
				{
					float myThreatPoints = sitePartDef.wantsThreatPoints ? num2 : 0f;
					list.Add(new SitePartDefWithParams(sitePartDef, sitePartDef.Worker.GenerateDefaultParams(myThreatPoints, tile, faction)));
				}
				sitePartDefsWithParams = list;
				return;
			}
			sitePartDefsWithParams = null;
		}

		
		public static bool FactionCanOwn(IEnumerable<SitePartDef> parts, Faction faction, bool disallowNonHostileFactions, Predicate<Faction> extraFactionValidator)
		{
			if (parts != null)
			{
				using (IEnumerator<SitePartDef> enumerator = parts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!SiteMakerHelper.FactionCanOwn(enumerator.Current, faction, disallowNonHostileFactions, extraFactionValidator))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		
		public static IEnumerable<SitePartDef> SitePartDefsWithTag(string tag)
		{
			if (tag == null)
			{
				return null;
			}
			return from x in DefDatabase<SitePartDef>.AllDefsListForReading
			where x.tags.Contains(tag)
			select x;
		}

		
		private static bool FactionCanOwn(SitePartDef sitePart, Faction faction, bool disallowNonHostileFactions, Predicate<Faction> extraFactionValidator)
		{
			if (sitePart == null)
			{
				Log.Error("Called FactionCanOwn() with null SitePartDef.", false);
				return false;
			}
			return sitePart.FactionCanOwn(faction) && (!disallowNonHostileFactions || faction == null || faction.HostileTo(Faction.OfPlayer)) && (extraFactionValidator == null || extraFactionValidator(faction));
		}

		
		private static List<Faction> possibleFactions = new List<Faction>();
	}
}
