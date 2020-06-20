using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1E RID: 3102
	public class ScenPart_PlayerFaction : ScenPart
	{
		// Token: 0x060049F9 RID: 18937 RVA: 0x00190714 File Offset: 0x0018E914
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0019072C File Offset: 0x0018E92C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.factionDef.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef localFd2 in from d in DefDatabase<FactionDef>.AllDefs
				where d.isPlayer
				select d)
				{
					FactionDef localFd = localFd2;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate
					{
						this.factionDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0019081C File Offset: 0x0018EA1C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(this.factionDef.label);
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x0019083D File Offset: 0x0018EA3D
		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00190873 File Offset: 0x0018EA73
		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(this.factionDef);
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
			FactionGenerator.EnsureRequiredEnemies(Find.GameInitData.playerFaction);
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x001908B0 File Offset: 0x0018EAB0
		public override void PreMapGenerate()
		{
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			settlement.SetFaction(Find.GameInitData.playerFaction);
			settlement.Tile = Find.GameInitData.startingTile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x00190918 File Offset: 0x0018EB18
		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x00190925 File Offset: 0x0018EB25
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factionDef == null)
			{
				yield return "factionDef is null";
			}
			yield break;
		}

		// Token: 0x04002A0D RID: 10765
		internal FactionDef factionDef;
	}
}
