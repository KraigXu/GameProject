using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class ITab_ContentsTransporter : ITab_ContentsBase
	{
		
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x001F8372 File Offset: 0x001F6572
		public override IList<Thing> container
		{
			get
			{
				return this.Transporter.innerContainer;
			}
		}

		
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x001F837F File Offset: 0x001F657F
		public CompTransporter Transporter
		{
			get
			{
				return base.SelThing.TryGetComp<CompTransporter>();
			}
		}

		
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x001F838C File Offset: 0x001F658C
		public override bool IsVisible
		{
			get
			{
				return (base.SelThing.Faction == null || base.SelThing.Faction == Faction.OfPlayer) && this.Transporter != null && (this.Transporter.LoadingInProgressOrReadyToLaunch || this.Transporter.innerContainer.Any);
			}
		}

		
		public ITab_ContentsTransporter()
		{
			this.labelKey = "TabTransporterContents";
			this.containedItemsKey = "ContainedItems";
		}

		
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

		
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
		}

		
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		
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
