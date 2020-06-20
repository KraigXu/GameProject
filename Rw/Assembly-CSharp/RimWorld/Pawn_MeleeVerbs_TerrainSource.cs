using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAB RID: 2987
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x0017AEEE File Offset: 0x001790EE
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004624 RID: 17956 RVA: 0x00019EA1 File Offset: 0x000180A1
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x0017AEF6 File Offset: 0x001790F6
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004626 RID: 17958 RVA: 0x0017AF03 File Offset: 0x00179103
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.parent.Pawn;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004627 RID: 17959 RVA: 0x0017AF10 File Offset: 0x00179110
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Terrain;
			}
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x0017AF17 File Offset: 0x00179117
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0017AF38 File Offset: 0x00179138
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x0017AF64 File Offset: 0x00179164
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0017AF80 File Offset: 0x00179180
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.parent.Pawn && p.Spawned && this.def == p.Position.GetTerrain(p.Map) && Find.TickManager.TicksGame >= this.parent.lastTerrainBasedVerbUseTick + 1200;
		}

		// Token: 0x04002848 RID: 10312
		public Pawn_MeleeVerbs parent;

		// Token: 0x04002849 RID: 10313
		public TerrainDef def;

		// Token: 0x0400284A RID: 10314
		public VerbTracker tracker;
	}
}
