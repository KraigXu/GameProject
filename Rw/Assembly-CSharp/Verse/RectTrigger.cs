﻿using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E2 RID: 738
	public class RectTrigger : Thing
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x0007B324 File Offset: 0x00079524
		// (set) Token: 0x060014DE RID: 5342 RVA: 0x0007B32C File Offset: 0x0007952C
		public CellRect Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
				if (base.Spawned)
				{
					this.rect.ClipInsideMap(base.Map);
				}
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0007B34F File Offset: 0x0007954F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0007B36C File Offset: 0x0007956C
		public override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.IsHashIntervalTick(60))
			{
				Map map = base.Map;
				for (int i = this.rect.minZ; i <= this.rect.maxZ; i++)
				{
					for (int j = this.rect.minX; j <= this.rect.maxX; j++)
					{
						List<Thing> thingList = new IntVec3(j, 0, i).GetThingList(map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.category == ThingCategory.Pawn && thingList[k].def.race.intelligence == Intelligence.Humanlike && thingList[k].Faction == Faction.OfPlayer)
							{
								this.ActivatedBy((Pawn)thingList[k]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0007B479 File Offset: 0x00079679
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, p.Named("SUBJECT")));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0007B4AC File Offset: 0x000796AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04000DDF RID: 3551
		private CellRect rect;

		// Token: 0x04000DE0 RID: 3552
		public bool destroyIfUnfogged;

		// Token: 0x04000DE1 RID: 3553
		public bool activateOnExplosion;

		// Token: 0x04000DE2 RID: 3554
		public string signalTag;
	}
}
