using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class PawnApparelGenerator
	{
		
		static PawnApparelGenerator()
		{
			PawnApparelGenerator.Reset();
		}

		
		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.3f));
		}

		
		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.ToolUser || !pawn.RaceProps.IsFlesh)
			{
				return;
			}
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			float randomInRange = pawn.kindDef.apparelMoney.RandomInRange;
			float mapTemperature;
			NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request, out mapTemperature);
			bool allowHeadgear = Rand.Value < pawn.kindDef.apparelAllowHeadgearChance;
			PawnApparelGenerator.debugSb = null;
			if (DebugViewSettings.logApparelGeneration)
			{
				PawnApparelGenerator.debugSb = new StringBuilder();
				PawnApparelGenerator.debugSb.AppendLine("Generating apparel for " + pawn);
				PawnApparelGenerator.debugSb.AppendLine("Money: " + randomInRange.ToString("F0"));
				PawnApparelGenerator.debugSb.AppendLine("Needed warmth: " + neededWarmth);
				PawnApparelGenerator.debugSb.AppendLine("Headgear allowed: " + allowHeadgear.ToString());
			}
			int @int = Rand.Int;
			PawnApparelGenerator.tmpApparelCandidates.Clear();
			for (int i = 0; i < PawnApparelGenerator.allApparelPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnApparelGenerator.allApparelPairs[i];
				if (PawnApparelGenerator.CanUsePair(thingStuffPair, pawn, randomInRange, allowHeadgear, @int))
				{
					PawnApparelGenerator.tmpApparelCandidates.Add(thingStuffPair);
				}
			}
			if (randomInRange < 0.001f)
			{
				PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
			}
			else
			{
				int num = 0;
				for (;;)
				{
					PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.Append(num.ToString().PadRight(5) + "Trying: " + PawnApparelGenerator.workingSet.ToString());
					}
					if (num >= 10 || Rand.Value >= 0.85f || randomInRange >= 9999999f)
					{
						goto IL_234;
					}
					float num2 = Rand.Range(0.45f, 0.8f);
					float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
					if (totalPrice >= randomInRange * num2)
					{
						goto IL_234;
					}
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine(string.Concat(new string[]
						{
							" -- Failed: Spent $",
							totalPrice.ToString("F0"),
							", < ",
							(num2 * 100f).ToString("F0"),
							"% of money."
						}));
					}
					IL_37D:
					num++;
					continue;
					IL_234:
					if (num < 20 && Rand.Value < 0.97f && !PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Does not cover torso.");
							goto IL_37D;
						}
						goto IL_37D;
					}
					else if (num < 30 && Rand.Value < 0.8f && PawnApparelGenerator.workingSet.CoatButNoShirt())
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Coat but no shirt.");
							goto IL_37D;
						}
						goto IL_37D;
					}
					else
					{
						if (num < 50)
						{
							bool mustBeSafe = num < 17;
							if (!PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, mustBeSafe, mapTemperature))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Wrong warmth.");
									goto IL_37D;
								}
								goto IL_37D;
							}
						}
						if (num >= 80 || !PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
						{
							break;
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Naked.");
							goto IL_37D;
						}
						goto IL_37D;
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.Append(string.Concat(new object[]
					{
						" -- Approved! Total price: $",
						PawnApparelGenerator.workingSet.TotalPrice.ToString("F0"),
						", TotalInsulationCold: ",
						PawnApparelGenerator.workingSet.TotalInsulationCold
					}));
				}
			}
			if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, true, mapTemperature))
			{
				PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth, mapTemperature);
			}
			if (DebugViewSettings.logApparelGeneration)
			{
				Log.Message(PawnApparelGenerator.debugSb.ToString(), false);
			}
			PawnApparelGenerator.workingSet.GiveToPawn(pawn);
			PawnApparelGenerator.workingSet.Reset(null, null);
			if (pawn.kindDef.apparelColor != Color.white)
			{
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					wornApparel[j].SetColor(pawn.kindDef.apparelColor, false);
				}
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				foreach (SpecificApparelRequirement specificApparelRequirement in from x in specificApparelRequirements
				where x.Color != default(Color)
				select x)
				{
					List<Apparel> wornApparel2 = pawn.apparel.WornApparel;
					for (int k = 0; k < wornApparel2.Count; k++)
					{
						if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirement, wornApparel2[k].def))
						{
							wornApparel2[k].SetColor(specificApparelRequirement.Color, false);
						}
					}
				}
			}
			foreach (Apparel thing in pawn.apparel.WornApparel)
			{
				CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
				if (compBiocodable != null && Rand.Chance(request.BiocodeApparelChance))
				{
					compBiocodable.CodeFor(pawn);
				}
			}
		}

		
		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, List<ThingStuffPair> apparelCandidates)
		{
			PawnApparelGenerator.workingSet.Reset(pawn.RaceProps.body, pawn.def);
			float num = money;
			List<ThingDef> reqApparel = pawn.kindDef.apparelRequired;
			if (reqApparel != null)
			{
				int l;
				int i;
				for (i = 0; i < reqApparel.Count; i = l + 1)
				{
					ThingStuffPair pair = (from pa in PawnApparelGenerator.allApparelPairs
					where pa.thing == reqApparel[i] && PawnApparelGenerator.CanUseStuff(pawn, pa)
					select pa).RandomElementByWeight((ThingStuffPair pa) => pa.Commonality);
					PawnApparelGenerator.workingSet.Add(pair);
					num -= pair.Price;
					l = i;
				}
			}
			List<SpecificApparelRequirement> att = pawn.kindDef.specificApparelRequirements;
			if (att != null)
			{
				int l;
				int i;
				for (i = 0; i < att.Count; i = l + 1)
				{
					if (!att[i].RequiredTag.NullOrEmpty() || !att[i].AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>())
					{
						ThingStuffPair pair2;
						if ((from pa in PawnApparelGenerator.allApparelPairs
						where PawnApparelGenerator.ApparelRequirementTagsMatch(att[i], pa.thing) && PawnApparelGenerator.ApparelRequirementHandlesThing(att[i], pa.thing) && PawnApparelGenerator.CanUseStuff(pawn, pa) && pa.thing.apparel.CorrectGenderForWearing(pawn.gender) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)
						select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair2))
						{
							PawnApparelGenerator.workingSet.Add(pair2);
							num -= pair2.Price;
						}
					}
					l = i;
				}
			}
			PawnApparelGenerator.usableApparel.Clear();
			for (int j = 0; j < apparelCandidates.Count; j++)
			{
				if (!PawnApparelGenerator.workingSet.PairOverlapsAnything(apparelCandidates[j]))
				{
					PawnApparelGenerator.usableApparel.Add(apparelCandidates[j]);
				}
			}
			
			while (Rand.Value >= 0.1f || money >= 9999999f)
			{
				IEnumerable<ThingStuffPair> source = PawnApparelGenerator.usableApparel;
				Func<ThingStuffPair, bool> predicate;
				if ((predicate ) == null)
				{
					predicate = (9__4 = ((ThingStuffPair pa) => PawnApparelGenerator.CanUseStuff(pawn, pa)));
				}
				ThingStuffPair pair3;
				if (!source.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair3))
				{
					break;
				}
				PawnApparelGenerator.workingSet.Add(pair3);
				num -= pair3.Price;
				for (int k = PawnApparelGenerator.usableApparel.Count - 1; k >= 0; k--)
				{
					if (PawnApparelGenerator.usableApparel[k].Price > num || PawnApparelGenerator.workingSet.PairOverlapsAnything(PawnApparelGenerator.usableApparel[k]))
					{
						PawnApparelGenerator.usableApparel.RemoveAt(k);
					}
				}
			}
		}

		
		private static bool CanUseStuff(Pawn pawn, ThingStuffPair pair)
		{
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (!PawnApparelGenerator.ApparelRequirementCanUseStuff(specificApparelRequirements[i], pair))
					{
						return false;
					}
				}
			}
			return pair.stuff == null || pawn.Faction == null || pawn.Faction.def.CanUseStuffForApparel(pair.stuff);
		}

		
		public static bool IsDerpApparel(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (!thing.IsApparel)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < thing.stuffCategories.Count; i++)
			{
				if (thing.stuffCategories[i] != StuffCategoryDefOf.Woody && thing.stuffCategories[i] != StuffCategoryDefOf.Stony)
				{
					flag = true;
					break;
				}
			}
			return flag && (stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Woody) || stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)) && (thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso) || thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs));
		}

		
		public static bool ApparelRequirementHandlesThing(SpecificApparelRequirement req, ThingDef thing)
		{
			return (req.BodyPartGroup == null || thing.apparel.bodyPartGroups.Contains(req.BodyPartGroup)) && (req.ApparelLayer == null || thing.apparel.layers.Contains(req.ApparelLayer));
		}

		
		public static bool ApparelRequirementTagsMatch(SpecificApparelRequirement req, ThingDef thing)
		{
			return (!req.RequiredTag.NullOrEmpty() && thing.apparel.tags.Contains(req.RequiredTag)) || (!req.AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && (from x in req.AlternateTagChoices
			where thing.apparel.tags.Contains(x.tag) && Rand.Value < x.chance
			select x).Any<SpecificApparelRequirement.TagChance>());
		}

		
		private static bool ApparelRequirementCanUseStuff(SpecificApparelRequirement req, ThingStuffPair pair)
		{
			return req.Stuff == null || !PawnApparelGenerator.ApparelRequirementHandlesThing(req, pair.thing) || (pair.stuff != null && req.Stuff == pair.stuff);
		}

		
		private static bool CanUsePair(ThingStuffPair pair, Pawn pawn, float moneyLeft, bool allowHeadgear, int fixedSeed)
		{
			if (pair.Price > moneyLeft)
			{
				return false;
			}
			if (!allowHeadgear && PawnApparelGenerator.IsHeadgear(pair.thing))
			{
				return false;
			}
			if (!pair.thing.apparel.CorrectGenderForWearing(pawn.gender))
			{
				return false;
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				bool flag = false;
				for (int i = 0; i < pawn.kindDef.apparelTags.Count; i++)
				{
					for (int j = 0; j < pair.thing.apparel.tags.Count; j++)
					{
						if (pawn.kindDef.apparelTags[i] == pair.thing.apparel.tags[j])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>())
			{
				for (int k = 0; k < pawn.kindDef.apparelDisallowTags.Count; k++)
				{
					if (pair.thing.apparel.tags.Contains(pawn.kindDef.apparelDisallowTags[k]))
					{
						return false;
					}
				}
			}
			return pair.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(pair.thing.generateAllowChance, fixedSeed ^ (int)pair.thing.shortHash ^ 64128343);
		}

		
		public static bool IsHeadgear(ThingDef td)
		{
			return td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead);
		}

		
		private static NeededWarmth ApparelWarmthNeededNow(Pawn pawn, PawnGenerationRequest request, out float mapTemperature)
		{
			int tile = request.Tile;
			if (tile == -1)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			if (tile == -1)
			{
				mapTemperature = 21f;
				return NeededWarmth.Any;
			}
			NeededWarmth neededWarmth = NeededWarmth.Any;
			Twelfth twelfth = GenLocalDate.Twelfth(tile);
			mapTemperature = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			for (int i = 0; i < 2; i++)
			{
				NeededWarmth neededWarmth2 = PawnApparelGenerator.CalculateNeededWarmth(pawn, tile, twelfth);
				if (neededWarmth2 != NeededWarmth.Any)
				{
					neededWarmth = neededWarmth2;
					break;
				}
				twelfth = twelfth.NextTwelfth();
			}
			if (!pawn.kindDef.apparelIgnoreSeasons)
			{
				return neededWarmth;
			}
			if (request.ForceAddFreeWarmLayerIfNeeded && neededWarmth == NeededWarmth.Warm)
			{
				return neededWarmth;
			}
			return NeededWarmth.Any;
		}

		
		public static NeededWarmth CalculateNeededWarmth(Pawn pawn, int tile, Twelfth twelfth)
		{
			float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			if (num < pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 4f)
			{
				return NeededWarmth.Warm;
			}
			if (num > pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) + 4f)
			{
				return NeededWarmth.Cool;
			}
			return NeededWarmth.Any;
		}

		
		[DebugOutput]
		private static void ApparelPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnApparelGenerator.allApparelPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[8];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", delegate(ThingStuffPair p)
			{
				if (p.stuff == null)
				{
					return "";
				}
				return p.stuff.defName;
			});
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => (p.Commonality * 100f).ToString("F4"));
			array[4] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F4"));
			array[5] = new TableDataGetter<ThingStuffPair>("insulationCold", delegate(ThingStuffPair p)
			{
				if (p.InsulationCold != 0f)
				{
					return p.InsulationCold.ToString();
				}
				return "";
			});
			array[6] = new TableDataGetter<ThingStuffPair>("headgear", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsHeadgear(p.thing))
				{
					return "";
				}
				return "*";
			});
			array[7] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsDerpApparel(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		
		[DebugOutput]
		private static void ApparelPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		
		private static List<ThingStuffPair> allApparelPairs = new List<ThingStuffPair>();

		
		private static float freeWarmParkaMaxPrice;

		
		private static float freeWarmHatMaxPrice;

		
		private static PawnApparelGenerator.PossibleApparelSet workingSet = new PawnApparelGenerator.PossibleApparelSet();

		
		private static StringBuilder debugSb = null;

		
		private const int PracticallyInfinity = 9999999;

		
		private static List<ThingStuffPair> tmpApparelCandidates = new List<ThingStuffPair>();

		
		private static List<ThingStuffPair> usableApparel = new List<ThingStuffPair>();

		
		private class PossibleApparelSet
		{
			
			// (get) Token: 0x06009896 RID: 39062 RVA: 0x002EDA9C File Offset: 0x002EBC9C
			public int Count
			{
				get
				{
					return this.aps.Count;
				}
			}

			
			// (get) Token: 0x06009897 RID: 39063 RVA: 0x002EDAA9 File Offset: 0x002EBCA9
			public float TotalPrice
			{
				get
				{
					return this.aps.Sum((ThingStuffPair pa) => pa.Price);
				}
			}

			
			// (get) Token: 0x06009898 RID: 39064 RVA: 0x002EDAD5 File Offset: 0x002EBCD5
			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
				}
			}

			
			// (get) Token: 0x06009899 RID: 39065 RVA: 0x002EDB01 File Offset: 0x002EBD01
			public List<ThingStuffPair> ApparelsForReading
			{
				get
				{
					return this.aps;
				}
			}

			
			public void Reset(BodyDef body, ThingDef raceDef)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.body = body;
				this.raceDef = raceDef;
			}

			
			public void Add(ThingStuffPair pair)
			{
				this.aps.Add(pair);
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						this.lgps.Add(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
					}
				}
			}

			
			public bool PairOverlapsAnything(ThingStuffPair pair)
			{
				if (!this.lgps.Any<ApparelUtility.LayerGroupPair>())
				{
					return false;
				}
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						if (this.lgps.Contains(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j])))
						{
							return true;
						}
					}
				}
				return false;
			}

			
			public bool CoatButNoShirt()
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						for (int j = 0; j < this.aps[i].thing.apparel.layers.Count; j++)
						{
							ApparelLayerDef apparelLayerDef = this.aps[i].thing.apparel.layers[j];
							if (apparelLayerDef == ApparelLayerDefOf.OnSkin)
							{
								flag2 = true;
							}
							if (apparelLayerDef == ApparelLayerDefOf.Shell || apparelLayerDef == ApparelLayerDefOf.Middle)
							{
								flag = true;
							}
						}
					}
				}
				return flag && !flag2;
			}

			
			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			
			public bool IsNaked(Gender gender)
			{
				switch (gender)
				{
				case Gender.None:
					return false;
				case Gender.Male:
					return !this.Covers(BodyPartGroupDefOf.Legs);
				case Gender.Female:
					return !this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso);
				default:
					return false;
				}
			}

			
			public bool SatisfiesNeededWarmth(NeededWarmth warmth, bool mustBeSafe = false, float mapTemperature = 21f)
			{
				if (warmth == NeededWarmth.Any)
				{
					return true;
				}
				if (mustBeSafe && !GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
				{
					return false;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationHeat) >= -2f;
				}
				if (warmth == NeededWarmth.Warm)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold) >= 52f;
				}
				throw new NotImplementedException();
			}

			
			public void AddFreeWarmthAsNeeded(NeededWarmth warmth, float mapTemperature)
			{
				if (warmth == NeededWarmth.Any)
				{
					return;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine();
					PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
				}
				for (int i = 0; i < 3; i++)
				{
					if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("Checking to give free torso-cover at max price " + PawnApparelGenerator.freeWarmParkaMaxPrice);
						}
						Predicate<ThingStuffPair> parkaPairValidator = (ThingStuffPair pa) => pa.Price <= PawnApparelGenerator.freeWarmParkaMaxPrice && pa.InsulationCold > 0f && pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso) && pa.thing.apparel.canBeGeneratedToSatisfyWarmth && this.GetReplacedInsulationCold(pa) < pa.InsulationCold;
						int j = 0;
						;
						
						while (j < 2)
						{
							ThingStuffPair candidate;
							if (j == 0)
							{
								IEnumerable<ThingStuffPair> allApparelPairs = PawnApparelGenerator.allApparelPairs;
								Func<ThingStuffPair, bool> predicate = ((ThingStuffPair pa) => parkaPairValidator(pa) && pa.InsulationCold < 40f);
		
								if (allApparelPairs.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out candidate))
								{
									goto IL_15D;
								}
							}
							else
							{
								IEnumerable<ThingStuffPair> allApparelPairs2 = PawnApparelGenerator.allApparelPairs;
								Func<ThingStuffPair, bool> predicate2 = ((ThingStuffPair pa) => parkaPairValidator(pa));
	
								if (allApparelPairs2.Where(predicate2).TryMaxBy((ThingStuffPair x) => x.InsulationCold - this.GetReplacedInsulationCold(x), out candidate))
								{
									goto IL_15D;
								}
							}
							j++;
							continue;
							IL_15D:
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
								{
									"Giving free torso-cover: ",
									candidate,
									" insulation=",
									candidate.InsulationCold
								}));
								IEnumerable<ThingStuffPair> source = this.aps;
								Func<ThingStuffPair, bool> predicate3 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, candidate.thing, this.body));
								

								foreach (ThingStuffPair thingStuffPair in source.Where(predicate3))
								{
									PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
									{
										"    -replaces ",
										thingStuffPair.ToString(),
										" InsulationCold=",
										thingStuffPair.InsulationCold
									}));
								}
							}
							this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, candidate.thing, this.body));
							this.aps.Add(candidate);
							break;
						}
					}
					if (GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
					{
						break;
					}
				}
				if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
				{
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine("Checking to give free hat at max price " + PawnApparelGenerator.freeWarmHatMaxPrice);
					}
					Predicate<ThingStuffPair> hatPairValidator = (ThingStuffPair pa) => pa.Price <= PawnApparelGenerator.freeWarmHatMaxPrice && pa.InsulationCold >= 7f && (pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead)) && this.GetReplacedInsulationCold(pa) < pa.InsulationCold;
					ThingStuffPair hatPair;
					if ((from pa in PawnApparelGenerator.allApparelPairs
					where hatPairValidator(pa)
					select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out hatPair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
							{
								"Giving free hat: ",
								hatPair,
								" insulation=",
								hatPair.InsulationCold
							}));
							IEnumerable<ThingStuffPair> source2 = this.aps;
							Func<ThingStuffPair, bool> predicate4 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, hatPair.thing, this.body));

							foreach (ThingStuffPair thingStuffPair2 in source2.Where(predicate4))
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
								{
									"    -replaces ",
									thingStuffPair2.ToString(),
									" InsulationCold=",
									thingStuffPair2.InsulationCold
								}));
							}
						}
						this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, hatPair.thing, this.body));
						this.aps.Add(hatPair);
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine("New TotalInsulationCold: " + this.TotalInsulationCold);
				}
			}

			
			public void GiveToPawn(Pawn pawn)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					Apparel apparel = (Apparel)ThingMaker.MakeThing(this.aps[i].thing, this.aps[i].stuff);
					PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
					if (ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						pawn.apparel.Wear(apparel, false, false);
					}
				}
				for (int j = 0; j < this.aps.Count; j++)
				{
					for (int k = 0; k < this.aps.Count; k++)
					{
						if (j != k && !ApparelUtility.CanWearTogether(this.aps[j].thing, this.aps[k].thing, pawn.RaceProps.body))
						{
							Log.Error(string.Concat(new object[]
							{
								pawn,
								" generated with apparel that cannot be worn together: ",
								this.aps[j],
								", ",
								this.aps[k]
							}), false);
							return;
						}
					}
				}
			}

			
			private float GetReplacedInsulationCold(ThingStuffPair newAp)
			{
				float num = 0f;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(this.aps[i].thing, newAp.thing, this.body))
					{
						num += this.aps[i].InsulationCold;
					}
				}
				return num;
			}

			
			public override string ToString()
			{
				string str = "[";
				for (int i = 0; i < this.aps.Count; i++)
				{
					str = str + this.aps[i].ToString() + ", ";
				}
				return str + "]";
			}

			
			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			
			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

			
			private BodyDef body;

			
			private ThingDef raceDef;

			
			private const float StartingMinTemperature = 12f;

			
			private const float TargetMinTemperature = -40f;

			
			private const float StartingMaxTemperature = 32f;

			
			private const float TargetMaxTemperature = 30f;
		}
	}
}
