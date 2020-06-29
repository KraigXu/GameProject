using System;
using Verse;

namespace RimWorld
{
	
	public class CompSpawnerFilth : ThingComp
	{
		
		// (get) Token: 0x06005335 RID: 21301 RVA: 0x001BD74E File Offset: 0x001BB94E
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x001BD75C File Offset: 0x001BB95C
		private bool CanSpawnFilth
		{
			get
			{
				Hive hive = this.parent as Hive;
				if (hive != null && !hive.CompDormant.Awake)
				{
					return false;
				}
				if (this.Props.requiredRotStage != null)
				{
					RotStage rotStage = this.parent.GetRotStage();
					RotStage? requiredRotStage = this.Props.requiredRotStage;
					if (!(rotStage == requiredRotStage.GetValueOrDefault() & requiredRotStage != null))
					{
						return false;
					}
				}
				return true;
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				for (int i = 0; i < this.Props.spawnCountOnSpawn; i++)
				{
					this.TrySpawnFilth();
				}
			}
		}

		
		public override void CompTick()
		{
			base.CompTick();
			this.TickInterval(1);
		}

		
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickInterval(250);
		}

		
		private void TickInterval(int interval)
		{
			if (this.CanSpawnFilth)
			{
				if (this.Props.spawnMtbHours > 0f && Rand.MTBEventOccurs(this.Props.spawnMtbHours, 2500f, (float)interval))
				{
					this.TrySpawnFilth();
				}
				if (this.Props.spawnEveryDays >= 0f && Find.TickManager.TicksGame >= this.nextSpawnTimestamp)
				{
					if (this.nextSpawnTimestamp != -1)
					{
						this.TrySpawnFilth();
					}
					this.nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(this.Props.spawnEveryDays * 60000f);
				}
			}
		}

		
		public void TrySpawnFilth()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableCellNear(this.parent.Position, this.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 x) => x.Standable(this.parent.Map), (Region x) => true, out c, 999999))
			{
				return;
			}
			FilthMaker.TryMakeFilth(c, this.parent.Map, this.Props.filthDef, 1, FilthSourceFlags.None);
		}

		
		private int nextSpawnTimestamp = -1;
	}
}
