using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class RewardsGenerator
	{
		
		public static void ResetStaticData()
		{
			RewardsGenerator.MarketValueFillers.Clear();
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Silver);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Gold);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Uranium);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Jade);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Plasteel);
		}

		
		public static List<Reward> Generate(RewardsGeneratorParams parms)
		{
			float num;
			return RewardsGenerator.Generate(parms, out num);
		}

		
		public static List<Reward> Generate(RewardsGeneratorParams parms, out float generatedRewardValue)
		{
			List<Reward> result;
			try
			{
				result = RewardsGenerator.DoGenerate(parms, out generatedRewardValue);
			}
			finally
			{
			}
			return result;
		}

		
		private static List<Reward> DoGenerate(RewardsGeneratorParams parms, out float generatedRewardValue)
		{
			List<Reward> list = new List<Reward>();
			string text = parms.ConfigError();
			if (text != null)
			{
				Log.Error("Invalid reward generation params: " + text, false);
				generatedRewardValue = 0f;
				return list;
			}
			parms.rewardValue = Mathf.Max(parms.rewardValue, parms.minGeneratedRewardValue);
			bool flag = parms.allowGoodwill && parms.giverFaction != null && parms.giverFaction != Faction.OfPlayer && parms.giverFaction.CanEverGiveGoodwillRewards && parms.giverFaction.allowGoodwillRewards && parms.giverFaction.PlayerGoodwill <= 92;
			bool flag2 = parms.allowRoyalFavor && parms.giverFaction != null && parms.giverFaction.allowRoyalFavorRewards && parms.giverFaction.def.HasRoyalTitles;
			bool flag3 = flag2 || flag;
			bool flag4 = parms.giverFaction != null && parms.giverFaction.HostileTo(Faction.OfPlayer);
			bool flag5 = flag2 && parms.giverFaction == Faction.Empire && !parms.thingRewardItemsOnly;
			bool flag6;
			bool flag7;
			if (!parms.thingRewardDisallowed && !flag3)
			{
				flag6 = true;
				flag7 = false;
			}
			else if (parms.thingRewardDisallowed && flag3)
			{
				flag6 = false;
				flag7 = true;
			}
			else if (parms.thingRewardDisallowed && !flag3)
			{
				flag6 = false;
				flag7 = false;
			}
			else
			{
				float num = flag2 ? 0.6f : 0.3f;
				float value = Rand.Value;
				if (value < 0.3f && !flag5 && !flag4)
				{
					flag6 = true;
					flag7 = false;
				}
				else if (parms.thingRewardRequired)
				{
					flag6 = true;
					flag7 = true;
				}
				else if (value < 0.3f + num)
				{
					flag6 = false;
					flag7 = true;
				}
				else
				{
					flag6 = !flag4;
					flag7 = true;
				}
			}
			float num2;
			float num3;
			if (flag6 && !flag7)
			{
				num2 = parms.rewardValue;
				num3 = 0f;
			}
			else if (!flag6 && flag7)
			{
				num2 = 0f;
				num3 = parms.rewardValue;
			}
			else
			{
				if (!flag6 || !flag7)
				{
					generatedRewardValue = 0f;
					return list;
				}
				float num4 = Rand.Range(0.3f, 0.7f);
				float num5 = 1f - num4;
				num2 = parms.rewardValue * num4;
				num3 = parms.rewardValue * num5;
			}
			float num6 = 0f;
			float num7 = 0f;
			Reward reward = null;
			Reward reward2 = null;
			if (Rand.Value < 0.65f && !flag5)
			{
				if (num2 > 0f)
				{
					reward = RewardsGenerator.GenerateThingReward(num2, parms, out num6);
					if (flag7 || (flag3 && num2 - num6 >= 600f))
					{
						num3 += num2 - num6;
					}
				}
				if (num3 > 0f)
				{
					reward2 = RewardsGenerator.GenerateSocialReward(num3, parms, flag, flag2, out num7);
				}
			}
			else
			{
				if (num3 > 0f)
				{
					reward2 = RewardsGenerator.GenerateSocialReward(num3, parms, flag, flag2, out num7);
					if (flag6 || (!parms.thingRewardDisallowed && num3 - num7 >= 600f && !flag4))
					{
						num2 += num3 - num7;
					}
				}
				if (num2 > 0f)
				{
					reward = RewardsGenerator.GenerateThingReward(num2, parms, out num6);
				}
			}
			generatedRewardValue = num6 + num7;
			Reward_Items reward_Items = null;
			float num8 = parms.rewardValue - num6 - num7;
			if ((num8 >= 200f || num6 + num7 < parms.minGeneratedRewardValue) && !parms.thingRewardDisallowed)
			{
				reward_Items = RewardsGenerator.AddMarketValueFillers(num8, ref generatedRewardValue, reward);
			}
			if (reward != null)
			{
				list.Add(reward);
			}
			if (reward2 != null)
			{
				list.Add(reward2);
			}
			if (reward_Items != null)
			{
				list.Add(reward_Items);
			}
			return list;
		}

		
		private static Reward GenerateSocialReward(float rewardValue, RewardsGeneratorParams parms, bool allowGoodwill, bool allowRoyalFavor, out float valueActuallyUsed)
		{
			if (!allowGoodwill && !allowRoyalFavor)
			{
				Log.Error("GenerateSocialReward could not generate any reward for parms=" + parms, false);
				allowGoodwill = true;
			}
			float valueActuallyUsedLocal = 0f;
			Func<Reward> func = () => RewardsGenerator.GenerateReward<Reward_Goodwill>(rewardValue, parms, out valueActuallyUsedLocal);
			Func<Reward> b = () => RewardsGenerator.GenerateReward<Reward_RoyalFavor>(rewardValue, parms, out valueActuallyUsedLocal);
			if (allowGoodwill && parms.giverFaction != null && parms.giverFaction.HostileTo(Faction.OfPlayer))
			{
				Reward result = func();
				valueActuallyUsed = valueActuallyUsedLocal;
				return result;
			}
			float weightA = allowGoodwill ? 1f : 0f;
			float weightB = allowRoyalFavor ? 9f : 0f;
			Reward result2 = Rand.ElementByWeight<Func<Reward>>(func, weightA, b, weightB)();
			valueActuallyUsed = valueActuallyUsedLocal;
			return result2;
		}

		
		private static Reward GenerateThingReward(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			float valueActuallyUsedLocal = 0f;
			Func<Reward> func = () => RewardsGenerator.GenerateReward<Reward_Items>(rewardValue, parms, out valueActuallyUsedLocal);
			Func<Reward> b = () => RewardsGenerator.GenerateReward<Reward_Pawn>(rewardValue, parms, out valueActuallyUsedLocal);
			if (parms.thingRewardItemsOnly)
			{
				Reward result = func();
				valueActuallyUsed = valueActuallyUsedLocal;
				return result;
			}
			float weightB = Mathf.Max(0f, 1f * parms.populationIntent);
			Reward result2 = Rand.ElementByWeight<Func<Reward>>(func, 3f, b, weightB)();
			valueActuallyUsed = valueActuallyUsedLocal;
			return result2;
		}

		
		private static T GenerateReward<T>(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed) where T : Reward, new()
		{
			T result;
			try
			{
				T t = Activator.CreateInstance<T>();
				t.InitFromValue(rewardValue, parms, out valueActuallyUsed);
				result = t;
			}
			finally
			{
			}
			return result;
		}

		
		private static Reward_Items AddMarketValueFillers(float remainingValue, ref float generatedRewardValue, Reward thingReward)
		{
			IEnumerable<ThingDef> source = from x in RewardsGenerator.MarketValueFillers
			where remainingValue / x.BaseMarketValue >= 15f
			select x;
			if (!source.Any<ThingDef>())
			{
				return null;
			}
			ThingDef thingDef = null;
			Reward_Items existingItemsReward = thingReward as Reward_Items;
			if (existingItemsReward != null)
			{
				thingDef = source.FirstOrDefault((ThingDef x) => existingItemsReward.items.Any((Thing y) => y.def == x));
			}
			if (thingDef == null)
			{
				thingDef = source.RandomElement<ThingDef>();
			}
			int num = GenMath.RoundRandom(remainingValue / thingDef.BaseMarketValue);
			if (num >= 1)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = num;
				generatedRewardValue += thing.MarketValue * (float)thing.stackCount;
				Reward_Items reward_Items = thingReward as Reward_Items;
				if (reward_Items == null)
				{
					return new Reward_Items
					{
						items = 
						{
							thing
						}
					};
				}
				reward_Items.items.Add(thing);
			}
			return null;
		}

		
		private const float ThingRewardOnlyChance = 0.3f;

		
		private const float SocialRewardOnlyChance = 0.3f;

		
		private const float SocialRewardOnlyChance_RoyalFavorPossible = 0.6f;

		
		private const float ThingRewardMinFractionOfTotal = 0.3f;

		
		private const float SocialRewardMinFractionOfTotal = 0.3f;

		
		public const float ItemsValueFractionMaxVariance = 0.3f;

		
		private const float ThingRewardGenerateFirstChance = 0.65f;

		
		private const float ThingRewardSelectionWeight_Items = 3f;

		
		private const float ThingRewardSelectionWeight_Pawn = 1f;

		
		private const float SocialRewardSelectionWeight_RoyalFavor = 9f;

		
		private const float SocialRewardSelectionWeight_Goodwill = 1f;

		
		private const float MinValueForExtraSilverReward = 200f;

		
		private const float MinValueToGenerateSecondRewardType = 600f;

		
		private static readonly List<ThingDef> MarketValueFillers = new List<ThingDef>();

		
		public static readonly SimpleCurve RewardValueToRoyalFavorCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 2f),
				true
			},
			{
				new CurvePoint(500f, 4f),
				true
			},
			{
				new CurvePoint(2000f, 10f),
				true
			},
			{
				new CurvePoint(5000f, 18f),
				true
			}
		};

		
		public static readonly SimpleCurve RewardValueToGoodwillCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 10f),
				true
			},
			{
				new CurvePoint(500f, 15f),
				true
			},
			{
				new CurvePoint(1000f, 20f),
				true
			},
			{
				new CurvePoint(2000f, 35f),
				true
			},
			{
				new CurvePoint(5000f, 50f),
				true
			}
		};
	}
}
