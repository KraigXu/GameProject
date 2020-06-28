using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BFA RID: 3066
	public class SitePartWorker_ConditionCauser : SitePartWorker
	{
		// Token: 0x060048F4 RID: 18676 RVA: 0x0018CA6C File Offset: 0x0018AC6C
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int worldRange = sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange;
			return base.GetPostProcessedThreatLabel(site, sitePart) + " (" + "ConditionCauserRadius".Translate(worldRange) + ")";
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x0018CAC5 File Offset: 0x0018ACC5
		public override void Init(Site site, SitePart sitePart)
		{
			sitePart.conditionCauser = ThingMaker.MakeThing(sitePart.def.conditionCauserDef, null);
			CompCauseGameCondition compCauseGameCondition = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition>();
			compCauseGameCondition.RandomizeSettings();
			compCauseGameCondition.LinkWithSite(sitePart.site);
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x0018CAFA File Offset: 0x0018ACFA
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			if (!sitePart.conditionCauser.DestroyedOrNull() && !sitePart.conditionCauser.Spawned)
			{
				sitePart.conditionCauser.Tick();
			}
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x0018CB21 File Offset: 0x0018AD21
		public override void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
			base.PostDrawExtraSelectionOverlays(sitePart);
			GenDraw.DrawWorldRadiusRing(sitePart.site.Tile, sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange);
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0018CB50 File Offset: 0x0018AD50
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.conditionCauser.Spawned && sitePart.conditionCauser.Map == sitePart.site.Map)
			{
				sitePart.conditionCauser.DeSpawn(DestroyMode.Vanish);
				sitePart.conditionCauserWasSpawned = false;
			}
		}
	}
}
