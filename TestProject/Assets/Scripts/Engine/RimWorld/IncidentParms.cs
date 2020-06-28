using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C9 RID: 2505
	public class IncidentParms : IExposable
	{
		// Token: 0x06003BCD RID: 15309 RVA: 0x0013B668 File Offset: 0x00139868
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

		// Token: 0x06003BCE RID: 15310 RVA: 0x0013B8AC File Offset: 0x00139AAC
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

		// Token: 0x0400233E RID: 9022
		public IIncidentTarget target;

		// Token: 0x0400233F RID: 9023
		public float points = -1f;

		// Token: 0x04002340 RID: 9024
		public Faction faction;

		// Token: 0x04002341 RID: 9025
		public bool forced;

		// Token: 0x04002342 RID: 9026
		public string customLetterLabel;

		// Token: 0x04002343 RID: 9027
		public string customLetterText;

		// Token: 0x04002344 RID: 9028
		public LetterDef customLetterDef;

		// Token: 0x04002345 RID: 9029
		public List<ThingDef> letterHyperlinkThingDefs;

		// Token: 0x04002346 RID: 9030
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04002347 RID: 9031
		public string inSignalEnd;

		// Token: 0x04002348 RID: 9032
		public IntVec3 spawnCenter = IntVec3.Invalid;

		// Token: 0x04002349 RID: 9033
		public Rot4 spawnRotation = Rot4.South;

		// Token: 0x0400234A RID: 9034
		public bool generateFightersOnly;

		// Token: 0x0400234B RID: 9035
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x0400234C RID: 9036
		public RaidStrategyDef raidStrategy;

		// Token: 0x0400234D RID: 9037
		public PawnsArrivalModeDef raidArrivalMode;

		// Token: 0x0400234E RID: 9038
		public bool raidForceOneIncap;

		// Token: 0x0400234F RID: 9039
		public bool raidNeverFleeIndividual;

		// Token: 0x04002350 RID: 9040
		public bool raidArrivalModeForQuickMilitaryAid;

		// Token: 0x04002351 RID: 9041
		public float biocodeApparelChance;

		// Token: 0x04002352 RID: 9042
		public float biocodeWeaponsChance;

		// Token: 0x04002353 RID: 9043
		public Dictionary<Pawn, int> pawnGroups;

		// Token: 0x04002354 RID: 9044
		public int? pawnGroupMakerSeed;

		// Token: 0x04002355 RID: 9045
		public PawnKindDef pawnKind;

		// Token: 0x04002356 RID: 9046
		public int pawnCount;

		// Token: 0x04002357 RID: 9047
		public TraderKindDef traderKind;

		// Token: 0x04002358 RID: 9048
		public int podOpenDelay = 140;

		// Token: 0x04002359 RID: 9049
		public Quest quest;

		// Token: 0x0400235A RID: 9050
		public QuestScriptDef questScriptDef;

		// Token: 0x0400235B RID: 9051
		public string questTag;

		// Token: 0x0400235C RID: 9052
		public MechClusterSketch mechClusterSketch;

		// Token: 0x0400235D RID: 9053
		private List<Pawn> tmpPawns;

		// Token: 0x0400235E RID: 9054
		private List<int> tmpGroups;
	}
}
