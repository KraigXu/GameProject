﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1C RID: 2844
	public static class PawnWeaponGenerator
	{
		// Token: 0x060042E8 RID: 17128 RVA: 0x00167D10 File Offset: 0x00165F10
		public static void Reset()
		{
			Predicate<ThingDef> isWeapon = (ThingDef td) => td.equipmentType == EquipmentType.Primary && !td.weaponTags.NullOrEmpty<string>();
			PawnWeaponGenerator.allWeaponPairs = ThingStuffPair.AllWith(isWeapon);
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef td) => isWeapon(td)));
			}
			using (IEnumerator<ThingDef> enumerator = allDefs.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef thingDef = enumerator.Current;
					float num = (from pa in PawnWeaponGenerator.allWeaponPairs
					where pa.thing == thingDef
					select pa).Sum((ThingStuffPair pa) => pa.Commonality);
					float num2 = thingDef.generateCommonality / num;
					if (num2 != 1f)
					{
						for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
						{
							ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
							if (thingStuffPair.thing == thingDef)
							{
								PawnWeaponGenerator.allWeaponPairs[i] = new ThingStuffPair(thingStuffPair.thing, thingStuffPair.stuff, thingStuffPair.commonalityMultiplier * num2);
							}
						}
					}
				}
			}
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x00167E7C File Offset: 0x0016607C
		public static void TryGenerateWeaponFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnWeaponGenerator.workingWeapons.Clear();
			if (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Count == 0)
			{
				return;
			}
			if (!pawn.RaceProps.ToolUser)
			{
				return;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return;
			}
			float randomInRange = pawn.kindDef.weaponMoney.RandomInRange;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair w = PawnWeaponGenerator.allWeaponPairs[i];
				if (w.Price <= randomInRange && (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Any((string tag) => w.thing.weaponTags.Contains(tag))) && (w.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(w.thing.generateAllowChance, pawn.thingIDNumber ^ (int)w.thing.shortHash ^ 28554824)))
				{
					PawnWeaponGenerator.workingWeapons.Add(w);
				}
			}
			if (PawnWeaponGenerator.workingWeapons.Count == 0)
			{
				return;
			}
			pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			ThingStuffPair thingStuffPair;
			if (PawnWeaponGenerator.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => w.Commonality * w.Price, out thingStuffPair))
			{
				ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				PawnGenerator.PostProcessGeneratedGear(thingWithComps, pawn);
				float num = (request.BiocodeWeaponChance > 0f) ? request.BiocodeWeaponChance : pawn.kindDef.biocodeWeaponChance;
				if (Rand.Value < num)
				{
					CompBiocodableWeapon compBiocodableWeapon = thingWithComps.TryGetComp<CompBiocodableWeapon>();
					if (compBiocodableWeapon != null)
					{
						compBiocodableWeapon.CodeFor(pawn);
					}
				}
				pawn.equipment.AddEquipment(thingWithComps);
			}
			PawnWeaponGenerator.workingWeapons.Clear();
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x0016807C File Offset: 0x0016627C
		public static bool IsDerpWeapon(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (thing.IsMeleeWeapon)
			{
				if (thing.tools.NullOrEmpty<Tool>())
				{
					return false;
				}
				DamageDef damageDef = ThingUtility.PrimaryMeleeWeaponDamageType(thing);
				if (damageDef == null)
				{
					return false;
				}
				DamageArmorCategoryDef armorCategory = damageDef.armorCategory;
				if (armorCategory != null && armorCategory.multStat != null && stuff.GetStatValueAbstract(armorCategory.multStat, null) < 0.7f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x001680DC File Offset: 0x001662DC
		public static float CheapestNonDerpPriceFor(ThingDef weaponDef)
		{
			float num = 9999999f;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
				if (thingStuffPair.thing == weaponDef && !PawnWeaponGenerator.IsDerpWeapon(thingStuffPair.thing, thingStuffPair.stuff) && thingStuffPair.Price < num)
				{
					num = thingStuffPair.Price;
				}
			}
			return num;
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00168140 File Offset: 0x00166340
		[DebugOutput]
		private static void WeaponPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnWeaponGenerator.allWeaponPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[7];
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
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => p.Commonality.ToString("F5"));
			array[4] = new TableDataGetter<ThingStuffPair>("commMult", (ThingStuffPair p) => p.commonalityMultiplier.ToString("F5"));
			array[5] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F2"));
			array[6] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnWeaponGenerator.IsDerpWeapon(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x001682B5 File Offset: 0x001664B5
		[DebugOutput]
		private static void WeaponPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnWeaponGenerator.allWeaponPairs);
		}

		// Token: 0x0400267A RID: 9850
		private static List<ThingStuffPair> allWeaponPairs;

		// Token: 0x0400267B RID: 9851
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();
	}
}
