using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9B RID: 3227
	public class Filth : Thing
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06004DDF RID: 19935 RVA: 0x001A32DF File Offset: 0x001A14DF
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x001A3316 File Offset: 0x001A1516
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x001A3321 File Offset: 0x001A1521
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x001A3334 File Offset: 0x001A1534
		public int DisappearAfterTicks
		{
			get
			{
				return this.disappearAfterTicks;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06004DE3 RID: 19939 RVA: 0x001A333C File Offset: 0x001A153C
		public override string Label
		{
			get
			{
				if (this.sources.NullOrEmpty<string>())
				{
					return "FilthLabel".Translate(base.Label, this.thickness.ToString());
				}
				return "FilthLabelWithSource".Translate(base.Label, this.sources.ToCommaList(true), this.thickness.ToString());
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x001A33BC File Offset: 0x001A15BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.thickness, "thickness", 1, false);
			Scribe_Values.Look<int>(ref this.growTick, "growTick", 0, false);
			Scribe_Values.Look<int>(ref this.disappearAfterTicks, "disappearAfterTicks", 0, false);
			if (Scribe.mode != LoadSaveMode.Saving || this.sources != null)
			{
				Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, Array.Empty<object>());
			}
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x001A342C File Offset: 0x001A162C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing)
			{
				base.Map.listerFilthInHomeArea.Notify_FilthSpawned(this);
			}
			if (!respawningAfterLoad)
			{
				this.growTick = Find.TickManager.TicksGame;
				this.disappearAfterTicks = (int)(this.def.filth.disappearsInDays.RandomInRange * 60000f);
			}
			if (!FilthMaker.TerrainAcceptsFilth(base.Map.terrainGrid.TerrainAt(base.Position), this.def, FilthSourceFlags.None))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x001A34BC File Offset: 0x001A16BC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x001A34EC File Offset: 0x001A16EC
		public void AddSource(string newSource)
		{
			if (this.sources == null)
			{
				this.sources = new List<string>();
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i] == newSource)
				{
					return;
				}
			}
			while (this.sources.Count > 3)
			{
				this.sources.RemoveAt(0);
			}
			this.sources.Add(newSource);
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x001A3560 File Offset: 0x001A1760
		public void AddSources(IEnumerable<string> sources)
		{
			if (sources == null)
			{
				return;
			}
			foreach (string newSource in sources)
			{
				this.AddSource(newSource);
			}
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x001A35AC File Offset: 0x001A17AC
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x001A35EA File Offset: 0x001A17EA
		public void ThinFilth()
		{
			this.thickness--;
			if (base.Spawned)
			{
				if (this.thickness == 0)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				this.UpdateMesh();
			}
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x001A3618 File Offset: 0x001A1818
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x001A3639 File Offset: 0x001A1839
		public bool CanDropAt(IntVec3 c, Map map, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.CanMakeFilth(c, map, this.def, additionalFlags);
		}

		// Token: 0x04002BBA RID: 11194
		public int thickness = 1;

		// Token: 0x04002BBB RID: 11195
		public List<string> sources;

		// Token: 0x04002BBC RID: 11196
		private int growTick;

		// Token: 0x04002BBD RID: 11197
		private int disappearAfterTicks;

		// Token: 0x04002BBE RID: 11198
		private const int MaxThickness = 5;

		// Token: 0x04002BBF RID: 11199
		private const int MinAgeToPickUp = 400;

		// Token: 0x04002BC0 RID: 11200
		private const int MaxNumSources = 3;
	}
}
