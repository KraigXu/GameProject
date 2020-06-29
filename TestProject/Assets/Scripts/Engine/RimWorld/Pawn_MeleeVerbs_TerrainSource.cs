using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x0017AEEE File Offset: 0x001790EE
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		
		// (get) Token: 0x06004624 RID: 17956 RVA: 0x00019EA1 File Offset: 0x000180A1
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x0017AEF6 File Offset: 0x001790F6
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		
		// (get) Token: 0x06004626 RID: 17958 RVA: 0x0017AF03 File Offset: 0x00179103
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.parent.Pawn;
			}
		}

		
		// (get) Token: 0x06004627 RID: 17959 RVA: 0x0017AF10 File Offset: 0x00179110
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Terrain;
			}
		}

		
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.parent.Pawn && p.Spawned && this.def == p.Position.GetTerrain(p.Map) && Find.TickManager.TicksGame >= this.parent.lastTerrainBasedVerbUseTick + 1200;
		}

		
		public Pawn_MeleeVerbs parent;

		
		public TerrainDef def;

		
		public VerbTracker tracker;
	}
}
