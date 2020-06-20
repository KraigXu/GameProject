using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D59 RID: 3417
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005335 RID: 21301 RVA: 0x001BD74E File Offset: 0x001BB94E
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x17000ECA RID: 3786
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

		// Token: 0x06005337 RID: 21303 RVA: 0x001BD7C6 File Offset: 0x001BB9C6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x001BD7E0 File Offset: 0x001BB9E0
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

		// Token: 0x06005339 RID: 21305 RVA: 0x001BD80C File Offset: 0x001BBA0C
		public override void CompTick()
		{
			base.CompTick();
			this.TickInterval(1);
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x001BD81B File Offset: 0x001BBA1B
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickInterval(250);
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x001BD830 File Offset: 0x001BBA30
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

		// Token: 0x0600533C RID: 21308 RVA: 0x001BD8D0 File Offset: 0x001BBAD0
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

		// Token: 0x04002DF1 RID: 11761
		private int nextSpawnTimestamp = -1;
	}
}
