using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class IncidentParms : IExposable
	{
		
		public void ExposeData()
		{
			Scribe_References.Look<IIncidentTarget>(ref this.target, "target", false);
			Scribe_Values.Look<float>(ref this.points, "threatPoints", 0f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.forced, "forced", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Collections.Look<ThingDef>(ref this.letterHyperlinkThingDefs, "letterHyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.letterHyperlinkHediffDefs, "letterHyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalEnd, "inSignalEnd", null, false);
			Scribe_Values.Look<IntVec3>(ref this.spawnCenter, "spawnCenter", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.spawnRotation, "spawnRotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.dontUseSingleUseRocketLaunchers, "dontUseSingleUseRocketLaunchers", false, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.raidArrivalMode, "raidArrivalMode");
			Scribe_Values.Look<bool>(ref this.raidForceOneIncap, "raidForceIncap", false, false);
			Scribe_Values.Look<bool>(ref this.raidNeverFleeIndividual, "raidNeverFleeIndividual", false, false);
			Scribe_Values.Look<bool>(ref this.raidArrivalModeForQuickMilitaryAid, "raidArrivalModeForQuickMilitaryAid", false, false);
			Scribe_Collections.Look<Pawn, int>(ref this.pawnGroups, "pawnGroups", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpGroups);
			Scribe_Values.Look<int?>(ref this.pawnGroupMakerSeed, "pawnGroupMakerSeed", null, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.pawnKind, "pawnKind");
			Scribe_Values.Look<float>(ref this.biocodeWeaponsChance, "biocodeWeaponsChance", 0f, false);
			Scribe_Values.Look<float>(ref this.biocodeApparelChance, "biocodeApparelChance", 0f, false);
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Values.Look<int>(ref this.podOpenDelay, "podOpenDelay", 140, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScriptDef, "questScriptDef");
		}

		
		public override string ToString()
		{
			string text = "";
			if (this.target != null)
			{
				text = text + "target=" + this.target;
			}
			if (this.points >= 0f)
			{
				text = text + ", points=" + this.points;
			}
			if (this.generateFightersOnly)
			{
				text = text + ", generateFightersOnly=" + this.generateFightersOnly.ToString();
			}
			if (this.raidStrategy != null)
			{
				text = text + ", raidStrategy=" + this.raidStrategy.defName;
			}
			if (this.questScriptDef != null)
			{
				text = text + ", questScriptDef=" + this.questScriptDef;
			}
			return text;
		}

		
		public IIncidentTarget target;

		
		public float points = -1f;

		
		public Faction faction;

		
		public bool forced;

		
		public string customLetterLabel;

		
		public string customLetterText;

		
		public LetterDef customLetterDef;

		
		public List<ThingDef> letterHyperlinkThingDefs;

		
		public List<HediffDef> letterHyperlinkHediffDefs;

		
		public string inSignalEnd;

		
		public IntVec3 spawnCenter = IntVec3.Invalid;

		
		public Rot4 spawnRotation = Rot4.South;

		
		public bool generateFightersOnly;

		
		public bool dontUseSingleUseRocketLaunchers;

		
		public RaidStrategyDef raidStrategy;

		
		public PawnsArrivalModeDef raidArrivalMode;

		
		public bool raidForceOneIncap;

		
		public bool raidNeverFleeIndividual;

		
		public bool raidArrivalModeForQuickMilitaryAid;

		
		public float biocodeApparelChance;

		
		public float biocodeWeaponsChance;

		
		public Dictionary<Pawn, int> pawnGroups;

		
		public int? pawnGroupMakerSeed;

		
		public PawnKindDef pawnKind;

		
		public int pawnCount;

		
		public TraderKindDef traderKind;

		
		public int podOpenDelay = 140;

		
		public Quest quest;

		
		public QuestScriptDef questScriptDef;

		
		public string questTag;

		
		public MechClusterSketch mechClusterSketch;

		
		private List<Pawn> tmpPawns;

		
		private List<int> tmpGroups;
	}
}
