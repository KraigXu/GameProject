using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000EA6 RID: 3750
	public class ITab_ContentsTransporter : ITab_ContentsBase
	{
		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x001F8372 File Offset: 0x001F6572
		public override IList<Thing> container
		{
			get
			{
				return this.Transporter.innerContainer;
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x001F837F File Offset: 0x001F657F
		public CompTransporter Transporter
		{
			get
			{
				return base.SelThing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x001F838C File Offset: 0x001F658C
		public override bool IsVisible
		{
			get
			{
				return (base.SelThing.Faction == null || base.SelThing.Faction == Faction.OfPlayer) && this.Transporter != null && (this.Transporter.LoadingInProgressOrReadyToLaunch || this.Transporter.innerContainer.Any);
			}
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x001F83E1 File Offset: 0x001F65E1
		public ITab_ContentsTransporter()
		{
			this.labelKey = "TabTransporterContents";
			this.containedItemsKey = "ContainedItems";
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x001F8400 File Offset: 0x001F6600
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			CompTransporter transporter = this.Transporter;
			Rect position = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			Text.Font = GameFont.Small;
			bool flag = false;
			float a = 0f;
			GUI.BeginGroup(position);
			Widgets.ListSeparator(ref a, position.width, "ItemsToLoad".Translate());
			if (transporter.leftToLoad != null)
			{
				for (int i = 0; i < transporter.leftToLoad.Count; i++)
				{
					TransferableOneWay t = transporter.leftToLoad[i];
					if (t.CountToTransfer > 0 && t.HasAnyThing)
					{
						flag = true;
						base.DoThingRow(t.ThingDef, t.CountToTransfer, t.things, position.width, ref a, delegate(int x)
						{
							this.OnDropToLoadThing(t, x);
						});
					}
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, position.width, null);
			}
			GUI.EndGroup();
			Rect inRect2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			base.DoItemsLists(inRect2, ref b);
			curY += Mathf.Max(a, b);
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x001F8588 File Offset: 0x001F6788
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x001F85B0 File Offset: 0x001F67B0
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x001F85E8 File Offset: 0x001F67E8
		private void OnDropToLoadThing(TransferableOneWay t, int count)
		{
			t.ForceTo(t.CountToTransfer - count);
			this.EndJobForEveryoneHauling(t);
			foreach (Thing thing in t.things)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					this.RemovePawnFromLoadLord(pawn);
				}
			}
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x001F8658 File Offset: 0x001F6858
		private void EndJobForEveryoneHauling(TransferableOneWay t)
		{
			List<Pawn> allPawnsSpawned = base.SelThing.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
				{
					JobDriver_HaulToTransporter jobDriver_HaulToTransporter = (JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver;
					if (jobDriver_HaulToTransporter.Transporter == this.Transporter && jobDriver_HaulToTransporter.ThingToCarry != null && jobDriver_HaulToTransporter.ThingToCarry.def == t.ThingDef)
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}
	}
}
