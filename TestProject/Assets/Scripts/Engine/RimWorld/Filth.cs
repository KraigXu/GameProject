using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Filth : Thing
	{
		
		// (get) Token: 0x06004DDF RID: 19935 RVA: 0x001A32DF File Offset: 0x001A14DF
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x001A3316 File Offset: 0x001A1516
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		
		// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x001A3321 File Offset: 0x001A1521
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x001A3334 File Offset: 0x001A1534
		public int DisappearAfterTicks
		{
			get
			{
				return this.disappearAfterTicks;
			}
		}

		
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

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		
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

		
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		
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

		
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		
		public bool CanDropAt(IntVec3 c, Map map, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.CanMakeFilth(c, map, this.def, additionalFlags);
		}

		
		public int thickness = 1;

		
		public List<string> sources;

		
		private int growTick;

		
		private int disappearAfterTicks;

		
		private const int MaxThickness = 5;

		
		private const int MinAgeToPickUp = 400;

		
		private const int MaxNumSources = 3;
	}
}
