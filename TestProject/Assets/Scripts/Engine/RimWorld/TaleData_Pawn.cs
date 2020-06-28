using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C32 RID: 3122
	public class TaleData_Pawn : TaleData
	{
		// Token: 0x06004A6E RID: 19054 RVA: 0x0019294C File Offset: 0x00190B4C
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Defs.Look<PawnKindDef>(ref this.kind, "kind");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.chronologicalAge, "chronologicalAge", 0, false);
			Scribe_Values.Look<string>(ref this.relationInfo, "relationInfo", null, false);
			Scribe_Values.Look<bool>(ref this.everBeenColonistOrTameAnimal, "everBeenColonistOrTameAnimal", false, false);
			Scribe_Values.Look<bool>(ref this.everBeenQuestLodger, "everBeenQuestLodger", false, false);
			Scribe_Values.Look<bool>(ref this.isFactionLeader, "isFactionLeader", false, false);
			Scribe_Collections.Look<RoyalTitle>(ref this.royalTitles, "royalTitles", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.primaryEquipment, "peq");
			Scribe_Defs.Look<ThingDef>(ref this.notableApparel, "app");
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x00192A68 File Offset: 0x00190C68
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			return GrammarUtility.RulesForPawn(prefix, this.name, this.title, this.kind, this.gender, this.faction, this.age, this.chronologicalAge, this.relationInfo, this.everBeenColonistOrTameAnimal, this.everBeenQuestLodger, this.isFactionLeader, this.royalTitles, null, true);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00192AC8 File Offset: 0x00190CC8
		public static TaleData_Pawn GenerateFrom(Pawn pawn)
		{
			TaleData_Pawn taleData_Pawn = new TaleData_Pawn();
			taleData_Pawn.pawn = pawn;
			taleData_Pawn.kind = pawn.kindDef;
			taleData_Pawn.faction = pawn.Faction;
			taleData_Pawn.gender = (pawn.RaceProps.hasGenders ? pawn.gender : Gender.None);
			taleData_Pawn.age = pawn.ageTracker.AgeBiologicalYears;
			taleData_Pawn.chronologicalAge = pawn.ageTracker.AgeChronologicalYears;
			taleData_Pawn.everBeenColonistOrTameAnimal = PawnUtility.EverBeenColonistOrTameAnimal(pawn);
			taleData_Pawn.everBeenQuestLodger = PawnUtility.EverBeenQuestLodger(pawn);
			taleData_Pawn.isFactionLeader = (pawn.Faction != null && pawn.Faction.leader == pawn);
			if (pawn.royalty != null)
			{
				taleData_Pawn.royalTitles = new List<RoyalTitle>();
				foreach (RoyalTitle other in pawn.royalty.AllTitlesForReading)
				{
					taleData_Pawn.royalTitles.Add(new RoyalTitle(other));
				}
			}
			TaggedString taggedString = "";
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			taleData_Pawn.relationInfo = taggedString.Resolve();
			if (pawn.story != null)
			{
				taleData_Pawn.title = pawn.story.title;
			}
			if (pawn.RaceProps.Humanlike)
			{
				taleData_Pawn.name = pawn.Name;
				if (pawn.equipment.Primary != null)
				{
					taleData_Pawn.primaryEquipment = pawn.equipment.Primary.def;
				}
				Apparel apparel;
				if (pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					taleData_Pawn.notableApparel = apparel.def;
				}
			}
			return taleData_Pawn;
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00192C70 File Offset: 0x00190E70
		public static TaleData_Pawn GenerateRandom()
		{
			PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
			Faction faction = FactionUtility.DefaultFactionFrom(random.defaultFactionType);
			return TaleData_Pawn.GenerateFrom(PawnGenerator.GeneratePawn(random, faction));
		}

		// Token: 0x04002A3E RID: 10814
		public Pawn pawn;

		// Token: 0x04002A3F RID: 10815
		public PawnKindDef kind;

		// Token: 0x04002A40 RID: 10816
		public Faction faction;

		// Token: 0x04002A41 RID: 10817
		public Gender gender;

		// Token: 0x04002A42 RID: 10818
		public int age;

		// Token: 0x04002A43 RID: 10819
		public int chronologicalAge;

		// Token: 0x04002A44 RID: 10820
		public string relationInfo;

		// Token: 0x04002A45 RID: 10821
		public bool everBeenColonistOrTameAnimal;

		// Token: 0x04002A46 RID: 10822
		public bool everBeenQuestLodger;

		// Token: 0x04002A47 RID: 10823
		public bool isFactionLeader;

		// Token: 0x04002A48 RID: 10824
		public List<RoyalTitle> royalTitles;

		// Token: 0x04002A49 RID: 10825
		public Name name;

		// Token: 0x04002A4A RID: 10826
		public string title;

		// Token: 0x04002A4B RID: 10827
		public ThingDef primaryEquipment;

		// Token: 0x04002A4C RID: 10828
		public ThingDef notableApparel;

		// Token: 0x04002A4D RID: 10829
		private List<Faction> tmpFactions;

		// Token: 0x04002A4E RID: 10830
		private List<RoyalTitleDef> tmpRoyalTitles;
	}
}
