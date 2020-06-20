using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000856 RID: 2134
	public static class DefGenerator
	{
		// Token: 0x060034CF RID: 13519 RVA: 0x00120FB8 File Offset: 0x0011F1B8
		public static void GenerateImpliedDefs_PreResolve()
		{
			foreach (ThingDef def in ThingDefGenerator_Buildings.ImpliedBlueprintAndFrameDefs().Concat(ThingDefGenerator_Meat.ImpliedMeatDefs()).Concat(ThingDefGenerator_Techprints.ImpliedTechprintDefs()).Concat(ThingDefGenerator_Corpses.ImpliedCorpseDefs()))
			{
				DefGenerator.AddImpliedDef<ThingDef>(def);
			}
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			foreach (TerrainDef def2 in TerrainDefGenerator_Stone.ImpliedTerrainDefs())
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def2);
			}
			foreach (RecipeDef def3 in RecipeDefGenerator.ImpliedRecipeDefs())
			{
				DefGenerator.AddImpliedDef<RecipeDef>(def3);
			}
			foreach (PawnColumnDef def4 in PawnColumnDefgenerator.ImpliedPawnColumnDefs())
			{
				DefGenerator.AddImpliedDef<PawnColumnDef>(def4);
			}
			foreach (ThingDef def5 in NeurotrainerDefGenerator.ImpliedThingDefs())
			{
				DefGenerator.AddImpliedDef<ThingDef>(def5);
			}
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x00121108 File Offset: 0x0011F308
		public static void GenerateImpliedDefs_PostResolve()
		{
			foreach (KeyBindingCategoryDef def in KeyBindingDefGenerator.ImpliedKeyBindingCategoryDefs())
			{
				DefGenerator.AddImpliedDef<KeyBindingCategoryDef>(def);
			}
			foreach (KeyBindingDef def2 in KeyBindingDefGenerator.ImpliedKeyBindingDefs())
			{
				DefGenerator.AddImpliedDef<KeyBindingDef>(def2);
			}
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x0012118C File Offset: 0x0011F38C
		public static void AddImpliedDef<T>(T def) where T : Def, new()
		{
			def.generated = true;
			ModContentPack modContentPack = def.modContentPack;
			if (modContentPack != null)
			{
				modContentPack.AddDef(def, "ImpliedDefs");
			}
			def.PostLoad();
			DefDatabase<T>.Add(def);
		}
	}
}
