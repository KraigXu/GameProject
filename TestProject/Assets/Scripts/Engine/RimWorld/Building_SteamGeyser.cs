using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CA5 RID: 3237
	public class Building_SteamGeyser : Building
	{
		// Token: 0x06004E48 RID: 20040 RVA: 0x001A500C File Offset: 0x001A320C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this);
			this.steamSprayer.startSprayCallback = new Action(this.StartSpray);
			this.steamSprayer.endSprayCallback = new Action(this.EndSpray);
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x001A505C File Offset: 0x001A325C
		private void StartSpray()
		{
			SnowUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x001A50C3 File Offset: 0x001A32C3
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x001A50E0 File Offset: 0x001A32E0
		public override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null && Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
			{
				Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.", false);
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x04002BF6 RID: 11254
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04002BF7 RID: 11255
		public Building harvester;

		// Token: 0x04002BF8 RID: 11256
		private Sustainer spraySustainer;

		// Token: 0x04002BF9 RID: 11257
		private int spraySustainerStartTick = -999;
	}
}
