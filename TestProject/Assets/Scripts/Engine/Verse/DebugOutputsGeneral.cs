using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	
	public static class DebugOutputsGeneral
	{
		
		private static float damage(ThingDef d)
		{
			return (float)((d.Verbs[0].defaultProjectile != null) ? d.Verbs[0].defaultProjectile.projectile.GetDamageAmount(null, null) : 0);
		}

		
		private static float armorPenetration(ThingDef d)
		{
			if (d.Verbs[0].defaultProjectile == null)
			{
				return 0f;
			}
			return d.Verbs[0].defaultProjectile.projectile.GetArmorPenetration(null, null);
		}

		
		private static float stoppingPower(ThingDef d)
		{
			if (d.Verbs[0].defaultProjectile == null)
			{
				return 0f;
			}
			return d.Verbs[0].defaultProjectile.projectile.stoppingPower;
		}

		
		private static float warmup(ThingDef d)
		{
			return d.Verbs[0].warmupTime;
		}

		
		private static float cooldown(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, null);
		}

		
		private static int burstShots(ThingDef d)
		{
			return d.Verbs[0].burstShotCount;
		}

		
		private static float fullcycle(ThingDef d)
		{
			return DebugOutputsGeneral.warmup(d) + DebugOutputsGeneral.cooldown(d) + ((d.Verbs[0].burstShotCount - 1) * d.Verbs[0].ticksBetweenBurstShots).TicksToSeconds();
		}

		
		private static float accTouch(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyTouch, null);
		}

		
		private static float accShort(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyShort, null);
		}

		
		private static float accMed(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyMedium, null);
		}

		
		private static float accLong(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyLong, null);
		}

		
		private static float accAvg(ThingDef d)
		{
			return (DebugOutputsGeneral.accTouch(d) + DebugOutputsGeneral.accShort(d) + DebugOutputsGeneral.accMed(d) + DebugOutputsGeneral.accLong(d)) / 4f;
		}

		
		private static float dpsAvg(ThingDef d)
		{
			return DebugOutputsGeneral.dpsMissless(d) * DebugOutputsGeneral.accAvg(d);
		}

		
		private static float dpsMissless(ThingDef d)
		{
			int num = DebugOutputsGeneral.burstShots(d);
			float num2 = DebugOutputsGeneral.warmup(d) + DebugOutputsGeneral.cooldown(d);
			num2 += (float)(num - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60f);
			return DebugOutputsGeneral.damage(d) * (float)num / num2;
		}

		
		[DebugOutput]
		public static void WeaponsRanged()
		{
			IEnumerable<ThingDef> dataSources = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsRangedWeapon
			select d).OrderByDescending(new Func<ThingDef, float>(DebugOutputsGeneral.dpsAvg));
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[25];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("damage", (ThingDef d) => DebugOutputsGeneral.damage(d).ToString());
			array[2] = new TableDataGetter<ThingDef>("AP", (ThingDef d) => DebugOutputsGeneral.armorPenetration(d).ToStringPercent());
			array[3] = new TableDataGetter<ThingDef>("stop\npower", delegate(ThingDef d)
			{
				if (DebugOutputsGeneral.stoppingPower(d) <= 0f)
				{
					return "";
				}
				return DebugOutputsGeneral.stoppingPower(d).ToString("F1");
			});
			array[4] = new TableDataGetter<ThingDef>("warmup", (ThingDef d) => DebugOutputsGeneral.warmup(d).ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("burst\nshots", (ThingDef d) => DebugOutputsGeneral.burstShots(d).ToString());
			array[6] = new TableDataGetter<ThingDef>("cooldown", (ThingDef d) => DebugOutputsGeneral.cooldown(d).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("full\ncycle", (ThingDef d) => DebugOutputsGeneral.fullcycle(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("range", (ThingDef d) => d.Verbs[0].range.ToString("F1"));
			array[9] = new TableDataGetter<ThingDef>("projectile\nspeed", delegate(ThingDef d)
			{
				if (d.projectile == null)
				{
					return "";
				}
				return d.projectile.speed.ToString("F0");
			});
			array[10] = new TableDataGetter<ThingDef>("dps\nmissless", (ThingDef d) => DebugOutputsGeneral.dpsMissless(d).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("accuracy\ntouch (" + 3f + ")", (ThingDef d) => DebugOutputsGeneral.accTouch(d).ToStringPercent());
			array[12] = new TableDataGetter<ThingDef>("accuracy\nshort (" + 12f + ")", (ThingDef d) => DebugOutputsGeneral.accShort(d).ToStringPercent());
			array[13] = new TableDataGetter<ThingDef>("accuracy\nmed (" + 25f + ")", (ThingDef d) => DebugOutputsGeneral.accMed(d).ToStringPercent());
			array[14] = new TableDataGetter<ThingDef>("accuracy\nlong (" + 40f + ")", (ThingDef d) => DebugOutputsGeneral.accLong(d).ToStringPercent());
			array[15] = new TableDataGetter<ThingDef>("accuracy\navg", (ThingDef d) => DebugOutputsGeneral.accAvg(d).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("forced\nmiss\nradius", delegate(ThingDef d)
			{
				if (d.Verbs[0].forcedMissRadius <= 0f)
				{
					return "";
				}
				return d.Verbs[0].forcedMissRadius.ToString();
			});
			array[17] = new TableDataGetter<ThingDef>("dps\ntouch", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d) * DebugOutputsGeneral.accTouch(d)).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("dps\nshort", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d) * DebugOutputsGeneral.accShort(d)).ToString("F2"));
			array[19] = new TableDataGetter<ThingDef>("dps\nmed", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d) * DebugOutputsGeneral.accMed(d)).ToString("F2"));
			array[20] = new TableDataGetter<ThingDef>("dps\nlong", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d) * DebugOutputsGeneral.accLong(d)).ToString("F2"));
			array[21] = new TableDataGetter<ThingDef>("dps\navg", (ThingDef d) => DebugOutputsGeneral.dpsAvg(d).ToString("F2"));
			array[22] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[23] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0"));
			array[24] = new TableDataGetter<ThingDef>("dpsAvg*100 / market value", (ThingDef d) => (DebugOutputsGeneral.dpsAvg(d) * 100f / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void Turrets()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.building.IsTurret
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[31];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[2] = new TableDataGetter<ThingDef>("cost\nlist", delegate(ThingDef d)
			{
				if (!d.costList.NullOrEmpty<ThingDefCountClass>())
				{
					return (from x in d.costList
					select x.Summary).ToCommaList(false);
				}
				return "";
			});
			array[3] = new TableDataGetter<ThingDef>("cost\nstuff\ncount", delegate(ThingDef d)
			{
				if (!d.MadeFromStuff)
				{
					return "";
				}
				return d.costStuffCount.ToString();
			});
			array[4] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToBuild, null).ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("hp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MaxHitPoints, null).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("damage", (ThingDef d) => DebugOutputsGeneral.damage(d.building.turretGunDef).ToString());
			array[7] = new TableDataGetter<ThingDef>("AP", (ThingDef d) => DebugOutputsGeneral.armorPenetration(d.building.turretGunDef).ToStringPercent());
			array[8] = new TableDataGetter<ThingDef>("stop\npower", delegate(ThingDef d)
			{
				if (DebugOutputsGeneral.stoppingPower(d.building.turretGunDef) <= 0f)
				{
					return "";
				}
				return DebugOutputsGeneral.stoppingPower(d.building.turretGunDef).ToString("F1");
			});
			array[9] = new TableDataGetter<ThingDef>("warmup", (ThingDef d) => DebugOutputsGeneral.warmup(d.building.turretGunDef).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("burst\nshots", (ThingDef d) => DebugOutputsGeneral.burstShots(d.building.turretGunDef).ToString());
			array[11] = new TableDataGetter<ThingDef>("cooldown", (ThingDef d) => DebugOutputsGeneral.cooldown(d.building.turretGunDef).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("full\ncycle", (ThingDef d) => DebugOutputsGeneral.fullcycle(d.building.turretGunDef).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("range", (ThingDef d) => d.building.turretGunDef.Verbs[0].range.ToString("F1"));
			array[14] = new TableDataGetter<ThingDef>("projectile\nspeed", delegate(ThingDef d)
			{
				if (d.building.turretGunDef.projectile == null)
				{
					return "";
				}
				return d.building.turretGunDef.projectile.speed.ToString("F0");
			});
			array[15] = new TableDataGetter<ThingDef>("dps\nmissless", (ThingDef d) => DebugOutputsGeneral.dpsMissless(d.building.turretGunDef).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("accuracy\ntouch (" + 3f + ")", (ThingDef d) => DebugOutputsGeneral.accTouch(d.building.turretGunDef).ToStringPercent());
			array[17] = new TableDataGetter<ThingDef>("accuracy\nshort (" + 12f + ")", (ThingDef d) => DebugOutputsGeneral.accShort(d.building.turretGunDef).ToStringPercent());
			array[18] = new TableDataGetter<ThingDef>("accuracy\nmed (" + 25f + ")", (ThingDef d) => DebugOutputsGeneral.accMed(d.building.turretGunDef).ToStringPercent());
			array[19] = new TableDataGetter<ThingDef>("accuracy\nlong (" + 40f + ")", (ThingDef d) => DebugOutputsGeneral.accLong(d.building.turretGunDef).ToStringPercent());
			array[20] = new TableDataGetter<ThingDef>("accuracy\navg", (ThingDef d) => DebugOutputsGeneral.accAvg(d.building.turretGunDef).ToString("F2"));
			array[21] = new TableDataGetter<ThingDef>("forced\nmiss\nradius", delegate(ThingDef d)
			{
				if (d.building.turretGunDef.Verbs[0].forcedMissRadius <= 0f)
				{
					return "";
				}
				return d.building.turretGunDef.Verbs[0].forcedMissRadius.ToString();
			});
			array[22] = new TableDataGetter<ThingDef>("dps\ntouch", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d.building.turretGunDef) * DebugOutputsGeneral.accTouch(d.building.turretGunDef)).ToString("F2"));
			array[23] = new TableDataGetter<ThingDef>("dps\nshort", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d.building.turretGunDef) * DebugOutputsGeneral.accShort(d.building.turretGunDef)).ToString("F2"));
			array[24] = new TableDataGetter<ThingDef>("dps\nmed", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d.building.turretGunDef) * DebugOutputsGeneral.accMed(d.building.turretGunDef)).ToString("F2"));
			array[25] = new TableDataGetter<ThingDef>("dps\nlong", (ThingDef d) => (DebugOutputsGeneral.dpsMissless(d.building.turretGunDef) * DebugOutputsGeneral.accLong(d.building.turretGunDef)).ToString("F2"));
			//array[26] = new TableDataGetter<ThingDef>("dps\navg", (ThingDef d) => DebugOutputsGeneral.dpsAvg(d.building.turretGunDef).ToString("F2"));
			//array[27] = new TableDataGetter<ThingDef>("dpsAvg / $100", (ThingDef d) => (DebugOutputsGeneral.dpsAvg(d.building.turretGunDef) / (d.GetStatValueAbstract(StatDefOf.MarketValue, null) / 100f)).ToString("F3"));
			//array[28] = new TableDataGetter<ThingDef>("fuel\nshot capacity", (ThingDef d) => DebugOutputsGeneral.<Turrets>g__fuelCapacity|15_0(d).ToString());
			//array[29] = new TableDataGetter<ThingDef>("fuel\ntype", (ThingDef d) => DebugOutputsGeneral.<Turrets>g__fuelType|15_1(d));
			//array[30] = new TableDataGetter<ThingDef>("fuel to\nreload", (ThingDef d) => DebugOutputsGeneral.<Turrets>g__fuelToReload|15_2(d).ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void WeaponsMelee()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Stuffless", delegate
			{
				DebugOutputsGeneral.DoTablesInternalMelee(null, false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			foreach (ThingDef localStuff2 in from st in DefDatabase<ThingDef>.AllDefs
			where st.IsStuff
			where DefDatabase<ThingDef>.AllDefs.Any((ThingDef wd) => wd.IsMeleeWeapon && st.stuffProps.CanMake(wd))
			select st into td
			orderby td.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null) descending
			select td)
			{
				ThingDef localStuff = localStuff2;
				float statValueAbstract = localStuff.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
				float statValueAbstract2 = localStuff.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null);
				float statFactorFromList = localStuff.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
				list.Add(new FloatMenuOption(string.Concat(new object[]
				{
					localStuff.defName,
					" (sharp ",
					statValueAbstract,
					", blunt ",
					statValueAbstract2,
					", cooldown ",
					statFactorFromList,
					")"
				}), delegate
				{
					DebugOutputsGeneral.DoTablesInternalMelee(localStuff, false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		
		private static void DoTablesInternalMelee(ThingDef stuff, bool doRaces = false)
		{
			//DebugOutputsGeneral.c__DisplayClass17_0 c__DisplayClass17_ = new DebugOutputsGeneral.c__DisplayClass17_0();
			//c__DisplayClass17_.stuff = stuff;
			//IEnumerable<Def> enumerable = (from d in DefDatabase<ThingDef>.AllDefs
			//where d.IsWeapon
			//select d).Cast<Def>().Concat((from h in DefDatabase<HediffDef>.AllDefs
			//where h.CompProps<HediffCompProperties_VerbGiver>() != null
			//select h).Cast<Def>());
			//if (doRaces)
			//{
			//	enumerable = enumerable.Concat((from d in DefDatabase<ThingDef>.AllDefs
			//	where d.race != null
			//	select d).Cast<Def>());
			//}
			//enumerable = from h in enumerable
			//orderby c__DisplayClass17_.<DoTablesInternalMelee>g__meleeDpsGetter|6(h) descending
			//select h;
			//IEnumerable<Def> dataSources = enumerable;
			//TableDataGetter<Def>[] array = new TableDataGetter<Def>[12];
			//array[0] = new TableDataGetter<Def>("defName", (Def d) => d.defName);
			//array[1] = new TableDataGetter<Def>("melee\nDPS", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__meleeDpsGetter|6(d).ToString("F2"));
			//array[2] = new TableDataGetter<Def>("melee\ndamage\naverage", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__meleeDamageGetter|0(d).ToString("F2"));
			//array[3] = new TableDataGetter<Def>("melee\ncooldown\naverage", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__meleeCooldownGetter|4(d).ToString("F2"));
			//array[4] = new TableDataGetter<Def>("melee\nAP", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__meleeAPGetter|1(d).ToString("F2"));
			//array[5] = new TableDataGetter<Def>("ranged\ndamage", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__rangedDamageGetter|2(d).ToString());
			//array[6] = new TableDataGetter<Def>("ranged\nwarmup", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__rangedWarmupGetter|3(d).ToString("F2"));
			//array[7] = new TableDataGetter<Def>("ranged\ncooldown", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__rangedCooldownGetter|5(d).ToString("F2"));
			//array[8] = new TableDataGetter<Def>("market value", (Def d) => c__DisplayClass17_.<DoTablesInternalMelee>g__marketValueGetter|7(d).ToStringMoney(null));
			//array[9] = new TableDataGetter<Def>("work to make", delegate(Def d)
			//{
			//	ThingDef thingDef = d as ThingDef;
			//	if (thingDef == null)
			//	{
			//		return "-";
			//	}
			//	return thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, c__DisplayClass17_.stuff).ToString("F0");
			//});
			//array[10] = new TableDataGetter<Def>((c__DisplayClass17_.stuff != null) ? (c__DisplayClass17_.stuff.defName + " CanMake") : "CanMake", delegate(Def d)
			//{
			//	if (c__DisplayClass17_.stuff == null)
			//	{
			//		return "n/a";
			//	}
			//	ThingDef thingDef = d as ThingDef;
			//	if (thingDef == null)
			//	{
			//		return "-";
			//	}
			//	return c__DisplayClass17_.stuff.stuffProps.CanMake(thingDef).ToStringCheckBlank();
			//});
			//array[11] = new TableDataGetter<Def>("assumed\nmelee\nhit chance", (Def d) => 0.82f.ToStringPercent());
			//DebugTables.MakeTablesDialog<Def>(dataSources, array);
		}

		
		[DebugOutput]
		public static void Tools()
		{
			//List<f__AnonymousType0<Def, Tool>> tools = (from x in (from x in DefDatabase<ThingDef>.AllDefs
			//where !x.tools.NullOrEmpty<Tool>()
			//select x).SelectMany((ThingDef x) => from y in x.tools
			//select new
			//{
			//	Parent = x,
			//	Tool = y
			//}).Concat((from x in DefDatabase<TerrainDef>.AllDefs
			//where !x.tools.NullOrEmpty<Tool>()
			//select x).SelectMany((TerrainDef x) => from y in x.tools
			//select new
			//{
			//	Parent = x,
			//	Tool = y
			//})).Concat((from x in DefDatabase<HediffDef>.AllDefs
			//where x.HasComp(typeof(HediffComp_VerbGiver)) && !x.CompProps<HediffCompProperties_VerbGiver>().tools.NullOrEmpty<Tool>()
			//select x).SelectMany((HediffDef x) => from y in x.CompProps<HediffCompProperties_VerbGiver>().tools
			//select new
			//{
			//	Parent = x,
			//	Tool = y
			//}))
			//orderby x.Parent.defName, x.Tool.power descending
			//select x).ToList();
			//Dictionary<Tool, float> selWeight = tools.ToDictionary(x => x.Tool, x => x.Tool.VerbsProperties.Average((VerbProperties y) => y.AdjustedMeleeSelectionWeight(x.Tool, null, null, null, x.Parent is ThingDef && ((ThingDef)x.Parent).category == ThingCategory.Pawn)));
			//Func<f__AnonymousType0<Def, Tool>, float> 9__34;
			//Dictionary<Def, float> selWeightSumInGroup = (from x in tools
			//select x.Parent).Distinct<Def>().ToDictionary((Def x) => x, delegate(Def x)
			//{
			//	var source = from y in tools
			//	where y.Parent == x
			//	select y;
			//	var selector;
			//	if ((selector 4) == null)
			//	{
			//		selector = (9__34 = (y => selWeight[y.Tool]));
			//	}
			//	return source.Sum(selector);
			//});
			//DebugTables.MakeTablesDialog<int>(tools.Select((x, int index) => index), new TableDataGetter<int>[]
			//{
			//	new TableDataGetter<int>("label", (int x) => tools[x].Tool.label),
			//	new TableDataGetter<int>("source", (int x) => tools[x].Parent.defName),
			//	new TableDataGetter<int>("power", (int x) => tools[x].Tool.power.ToString("0.##")),
			//	new TableDataGetter<int>("AP", delegate(int x)
			//	{
			//		float num = tools[x].Tool.armorPenetration;
			//		if (num < 0f)
			//		{
			//			num = tools[x].Tool.power * 0.015f;
			//		}
			//		return num.ToStringPercent();
			//	}),
			//	new TableDataGetter<int>("cooldown", (int x) => tools[x].Tool.cooldownTime.ToString("0.##")),
			//	new TableDataGetter<int>("selection weight", (int x) => selWeight[tools[x].Tool].ToString("0.##")),
			//	new TableDataGetter<int>("selection weight\nwithin def", (int x) => (selWeight[tools[x].Tool] / selWeightSumInGroup[tools[x].Parent]).ToStringPercent()),
			//	new TableDataGetter<int>("chance\nfactor", delegate(int x)
			//	{
			//		if (tools[x].Tool.chanceFactor != 1f)
			//		{
			//			return tools[x].Tool.chanceFactor.ToString("0.##");
			//		}
			//		return "";
			//	}),
			//	new TableDataGetter<int>("adds hediff", delegate(int x)
			//	{
			//		if (tools[x].Tool.hediff == null)
			//		{
			//			return "";
			//		}
			//		return tools[x].Tool.hediff.defName;
			//	}),
			//	new TableDataGetter<int>("linked body parts", delegate(int x)
			//	{
			//		if (tools[x].Tool.linkedBodyPartsGroup == null)
			//		{
			//			return "";
			//		}
			//		return tools[x].Tool.linkedBodyPartsGroup.defName;
			//	}),
			//	new TableDataGetter<int>("surprise attack", delegate(int x)
			//	{
			//		if (tools[x].Tool.surpriseAttack == null || tools[x].Tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>())
			//		{
			//			return "";
			//		}
			//		return tools[x].Tool.surpriseAttack.extraMeleeDamages[0].amount.ToString("0.##") + " (" + tools[x].Tool.surpriseAttack.extraMeleeDamages[0].def.defName + ")";
			//	}),
			//	new TableDataGetter<int>("capacities", (int x) => tools[x].Tool.capacities.ToStringSafeEnumerable()),
			//	new TableDataGetter<int>("maneuvers", (int x) => tools[x].Tool.Maneuvers.ToStringSafeEnumerable()),
			//	new TableDataGetter<int>("always weapon", delegate(int x)
			//	{
			//		if (!tools[x].Tool.alwaysTreatAsWeapon)
			//		{
			//			return "";
			//		}
			//		return "always wep";
			//	}),
			//	new TableDataGetter<int>("id", (int x) => tools[x].Tool.id)
			//});
		}

		
		[DebugOutput]
		public static void ApparelByStuff()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Stuffless", delegate
			{
				DebugOutputsGeneral.DoTableInternalApparel(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			foreach (ThingDef localStuff2 in from td in DefDatabase<ThingDef>.AllDefs
			where td.IsStuff
			select td)
			{
				ThingDef localStuff = localStuff2;
				list.Add(new FloatMenuOption(localStuff.defName, delegate
				{
					DebugOutputsGeneral.DoTableInternalApparel(localStuff);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		
		[DebugOutput]
		public static void ApparelArmor()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("stuff", (ThingDef x) => x.MadeFromStuff.ToStringCheckBlank()));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef x) => x.BaseMass));
			list.Add(new TableDataGetter<ThingDef>("mrkt\nvalue", (ThingDef x) => x.BaseMarketValue.ToString("F0")));
			list.Add(new TableDataGetter<ThingDef>("hp", (ThingDef x) => x.BaseMaxHitPoints));
			list.Add(new TableDataGetter<ThingDef>("flama\nbility", (ThingDef x) => x.BaseFlammability));
			list.Add(new TableDataGetter<ThingDef>("recipe\nmin\nskill", delegate(ThingDef x)
			{
				if (x.recipeMaker == null || x.recipeMaker.skillRequirements.NullOrEmpty<SkillRequirement>())
				{
					return "";
				}
				return x.recipeMaker.skillRequirements[0].skill.defName + " " + x.recipeMaker.skillRequirements[0].minLevel;
			}));
			list.Add(new TableDataGetter<ThingDef>("equip\ndelay", (ThingDef x) => x.GetStatValueAbstract(StatDefOf.EquipDelay, null)));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				if (x.MadeFromStuff)
				{
					return "";
				}
				return string.Concat(new string[]
				{
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null).ToStringPercent(),
					" / ",
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null).ToStringPercent(),
					" / ",
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, null).ToStringPercent()
				});
			}));
			foreach (ThingDef stuffLocal2 in new List<ThingDef>
			{
				ThingDefOf.Steel,
				ThingDefOf.Plasteel,
				ThingDefOf.Cloth,
				ThingDef.Named("Leather_Patch"),
				ThingDefOf.Leather_Plain,
				ThingDef.Named("Leather_Heavy"),
				ThingDef.Named("Leather_Thrumbo"),
				ThingDef.Named("Synthread"),
				ThingDef.Named("Hyperweave"),
				ThingDef.Named("DevilstrandCloth"),
				ThingDef.Named("WoolSheep"),
				ThingDef.Named("WoolMegasloth"),
				ThingDefOf.BlocksGranite,
				ThingDefOf.Silver,
				ThingDefOf.Gold
			})
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							return "";
						}
						return string.Concat(new string[]
						{
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuffLocal).ToStringPercent(),
							" / ",
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuffLocal).ToStringPercent(),
							" / ",
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, stuffLocal).ToStringPercent()
						});
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		
		[DebugOutput]
		public static void ApparelInsulation()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				if (x.MadeFromStuff)
				{
					return "";
				}
				return x.GetStatValueAbstract(StatDefOf.Insulation_Heat, null).ToStringTemperature("F1") + " / " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, null).ToStringTemperature("F1");
			}));
			foreach (ThingDef stuffLocal2 in from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			orderby x.BaseMarketValue
			select x)
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							return "";
						}
						return x.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuffLocal).ToString("F1") + ", " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuffLocal).ToString("F1");
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		
		private static void DoTableInternalApparel(ThingDef stuff)
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel && (stuff == null || (d.MadeFromStuff && stuff.stuffProps.CanMake(d)))
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("bodyParts", (ThingDef d) => GenText.ToSpaceList(from bp in d.apparel.bodyPartGroups
			select bp.defName));
			array[2] = new TableDataGetter<ThingDef>("layers", (ThingDef d) => GenText.ToSpaceList(from l in d.apparel.layers
			select l.ToString()));
			array[3] = new TableDataGetter<ThingDef>("tags", (ThingDef d) => GenText.ToSpaceList(from t in d.apparel.tags
			select t.ToString()));
			array[4] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("mktval", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("insCold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("insHeat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("blunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("sharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("SEMultArmor", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor, stuff).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("SEMultInsuCold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Cold, stuff).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("SEMultInsuHeat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Heat, stuff).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("equipTime", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.EquipDelay, stuff).ToString("F1"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void ResearchProjects()
		{
			IEnumerable<ResearchProjectDef> allDefs = DefDatabase<ResearchProjectDef>.AllDefs;
			TableDataGetter<ResearchProjectDef>[] array = new TableDataGetter<ResearchProjectDef>[9];
			array[0] = new TableDataGetter<ResearchProjectDef>("defName", (ResearchProjectDef d) => d.defName);
			array[1] = new TableDataGetter<ResearchProjectDef>("label", (ResearchProjectDef d) => d.label);
			array[2] = new TableDataGetter<ResearchProjectDef>("baseCost", (ResearchProjectDef d) => d.baseCost);
			array[3] = new TableDataGetter<ResearchProjectDef>("techLevel", (ResearchProjectDef d) => d.techLevel.ToString());
			array[4] = new TableDataGetter<ResearchProjectDef>("prerequisites", delegate(ResearchProjectDef d)
			{
				if (d.prerequisites != null && d.prerequisites.Count != 0)
				{
					return string.Join(",", (from p in d.prerequisites
					select d.defName).ToArray<string>());
				}
				return "NONE";
			});
			array[5] = new TableDataGetter<ResearchProjectDef>("hiddenPrerequisites", delegate(ResearchProjectDef d)
			{
				if (d.hiddenPrerequisites != null && d.hiddenPrerequisites.Count != 0)
				{
					return string.Join(",", (from p in d.hiddenPrerequisites
					select d.defName).ToArray<string>());
				}
				return "NONE";
			});
			array[6] = new TableDataGetter<ResearchProjectDef>("requiredResearchBuilding", (ResearchProjectDef d) => d.requiredResearchBuilding);
			array[7] = new TableDataGetter<ResearchProjectDef>("techprintCount", (ResearchProjectDef d) => d.techprintCount);
			array[8] = new TableDataGetter<ResearchProjectDef>("heldByFactionCategoryTags", delegate(ResearchProjectDef d)
			{
				if (d.heldByFactionCategoryTags != null)
				{
					return string.Join(",", (from fc in d.heldByFactionCategoryTags
					select fc).ToArray<string>());
				}
				return "NONE";
			});
			DebugTables.MakeTablesDialog<ResearchProjectDef>(allDefs, array);
		}

		
		[DebugOutput]
		public static void ThingsExistingList()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (object obj in Enum.GetValues(typeof(ThingRequestGroup)))
			{
				ThingRequestGroup localRg2 = (ThingRequestGroup)obj;
				ThingRequestGroup localRg = localRg2;
				FloatMenuOption item = new FloatMenuOption(localRg.ToString(), delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					List<Thing> list2 = Find.CurrentMap.listerThings.ThingsInGroup(localRg);
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Global things in group ",
						localRg,
						" (count ",
						list2.Count,
						")"
					}));
					Log.Message(DebugLogsUtility.ThingListToUniqueCountString(list2), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		
		[DebugOutput]
		public static void ThingFillageAndPassability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.passability != Traversability.Standable || thingDef.fillPercent > 0f)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						thingDef.defName,
						" - pass=",
						thingDef.passability.ToString(),
						", fill=",
						thingDef.fillPercent.ToStringPercent()
					}));
					if (thingDef.passability == Traversability.Impassable && thingDef.fillPercent < 0.1f)
					{
						stringBuilder.Append("   ALERT, impassable with low fill");
					}
					if (thingDef.passability != Traversability.Impassable && thingDef.fillPercent > 0.8f)
					{
						stringBuilder.Append("    ALERT, passabile with very high fill");
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		
		[DebugOutput]
		public static void ThingDamageData()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.useHitPoints
			orderby d.category, d.defName
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("hp", (ThingDef d) => d.BaseMaxHitPoints.ToString());
			array[3] = new TableDataGetter<ThingDef>("flammability", delegate(ThingDef d)
			{
				if (d.BaseFlammability <= 0f)
				{
					return "";
				}
				return d.BaseFlammability.ToString();
			});
			array[4] = new TableDataGetter<ThingDef>("uses stuff", (ThingDef d) => d.MadeFromStuff.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("deterioration rate", delegate(ThingDef d)
			{
				if (d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) <= 0f)
				{
					return "";
				}
				return d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null).ToString();
			});
			array[6] = new TableDataGetter<ThingDef>("days to deterioriate", delegate(ThingDef d)
			{
				if (d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) <= 0f)
				{
					return "";
				}
				return ((float)d.BaseMaxHitPoints / d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null)).ToString();
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void UnfinishedThings()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.isUnfinishedThing
			orderby d.category, d.defName
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("billGivers", (ThingDef d) => string.Join(", ", (from td in (from r in DefDatabase<RecipeDef>.AllDefsListForReading
			where r.unfinishedThingDef == d
			select r).SelectMany((RecipeDef r) => from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.AllRecipes != null && td.AllRecipes.Contains(r)
			select td)
			select td.defName).Distinct<string>()));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void ThingMasses()
		{
			IEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Item || x.Minifiable
			where x.thingClass != typeof(MinifiedThing) && x.thingClass != typeof(UnfinishedThing)
			orderby x.GetStatValueAbstract(StatDefOf.Mass, null), x.GetStatValueAbstract(StatDefOf.MarketValue, null)
			select x;
			Func<ThingDef, float, string> perPawn = (ThingDef d, float bodySize) => (bodySize * 35f / d.GetStatValueAbstract(StatDefOf.Mass, null)).ToString("F0");
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("defName", delegate(ThingDef d)
			{
				if (d.Minifiable)
				{
					return d.defName + " (minified)";
				}
				string text = d.defName;
				if (!d.EverHaulable)
				{
					text += " (not haulable)";
				}
				return text;
			});
			array[1] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Mass, null).ToString());
			array[2] = new TableDataGetter<ThingDef>("per human", (ThingDef d) => perPawn(d, ThingDefOf.Human.race.baseBodySize));
			array[3] = new TableDataGetter<ThingDef>("per muffalo", (ThingDef d) => perPawn(d, ThingDefOf.Muffalo.race.baseBodySize));
			array[4] = new TableDataGetter<ThingDef>("per dromedary", (ThingDef d) => perPawn(d, ThingDefOf.Dromedary.race.baseBodySize));
			//array[5] = new TableDataGetter<ThingDef>("per nutrition", (ThingDef d) => DebugOutputsGeneral.<ThingMasses>g__perNutrition|28_5(d));
			array[6] = new TableDataGetter<ThingDef>("small volume", delegate(ThingDef d)
			{
				if (!d.smallVolume)
				{
					return "";
				}
				return "small";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void ThingFillPercents()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.fillPercent > 0f
			orderby d.fillPercent descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("fillPercent", (ThingDef d) => d.fillPercent.ToStringPercent());
			array[2] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		[DebugOutput]
		public static void ThingNutritions()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.ingestible != null
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F1"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("nutrition per value", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.BaseMarketValue).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		public static void MakeTablePairsByThing(List<ThingStuffPair> pairList)
		{
			DefMap<ThingDef, float> totalCommMult = new DefMap<ThingDef, float>();
			DefMap<ThingDef, float> totalComm = new DefMap<ThingDef, float>();
			DefMap<ThingDef, int> pairCount = new DefMap<ThingDef, int>();
			foreach (ThingStuffPair thingStuffPair in pairList)
			{
				DefMap<ThingDef, float> defMap = totalCommMult;
				ThingDef thing = thingStuffPair.thing;
				defMap[thing] += thingStuffPair.commonalityMultiplier;
				defMap = totalComm;
				thing = thingStuffPair.thing;
				defMap[thing] += thingStuffPair.Commonality;
				DefMap<ThingDef, int> pairCount2 = pairCount;
				thing = thingStuffPair.thing;
				pairCount2[thing]++;
			}
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where pairList.Any((ThingStuffPair pa) => pa.thing == d)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("thing", (ThingDef t) => t.defName);
			array[1] = new TableDataGetter<ThingDef>("pair count", (ThingDef t) => pairCount[t].ToString());
			array[2] = new TableDataGetter<ThingDef>("total commonality multiplier ", (ThingDef t) => totalCommMult[t].ToString("F4"));
			array[3] = new TableDataGetter<ThingDef>("total commonality", (ThingDef t) => totalComm[t].ToString("F4"));
			array[4] = new TableDataGetter<ThingDef>("generateCommonality", (ThingDef t) => t.generateCommonality.ToString("F4"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		
		public static string ToStringEmptyZero(this float f, string format)
		{
			if (f <= 0f)
			{
				return "";
			}
			return f.ToString(format);
		}

		
		public static string ToStringPercentEmptyZero(this float f, string format = "F0")
		{
			if (f <= 0f)
			{
				return "";
			}
			return f.ToStringPercent(format);
		}

		
		public static string ToStringCheckBlank(this bool b)
		{
			if (!b)
			{
				return "";
			}
			return "✓";
		}
	}
}
