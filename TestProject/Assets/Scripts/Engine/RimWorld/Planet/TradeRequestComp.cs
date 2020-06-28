using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001280 RID: 4736
	[StaticConstructorOnStartup]
	public class TradeRequestComp : WorldObjectComp
	{
		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06006F13 RID: 28435 RVA: 0x0026AE59 File Offset: 0x00269059
		public bool ActiveRequest
		{
			get
			{
				return this.expiration > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x0026AE70 File Offset: 0x00269070
		public override string CompInspectStringExtra()
		{
			if (this.ActiveRequest)
			{
				return "CaravanRequestInfo".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount).CapitalizeFirst(), (this.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F1"), (this.requestThingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)this.requestCount).ToStringMoney(null));
			}
			return null;
		}

		// Token: 0x06006F15 RID: 28437 RVA: 0x0026AEF5 File Offset: 0x002690F5
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == this.parent)
			{
				yield return this.FulfillRequestCommand(caravan);
			}
			yield break;
		}

		// Token: 0x06006F16 RID: 28438 RVA: 0x0026AF0C File Offset: 0x0026910C
		public void Disable()
		{
			this.expiration = -1;
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x0026AF18 File Offset: 0x00269118
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.requestThingDef, "requestThingDef");
			Scribe_Values.Look<int>(ref this.requestCount, "requestCount", 0, false);
			Scribe_Values.Look<int>(ref this.expiration, "expiration", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x0026AF68 File Offset: 0x00269168
		private Command FulfillRequestCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandFulfillTradeOffer".Translate();
			command_Action.defaultDesc = "CommandFulfillTradeOfferDesc".Translate();
			command_Action.icon = TradeRequestComp.TradeCommandTex;
			Action <>9__1;
			command_Action.action = delegate
			{
				if (!this.ActiveRequest)
				{
					Log.Error("Attempted to fulfill an unavailable request", false);
					return;
				}
				if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
				{
					Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)), MessageTypeDefOf.RejectInput, false);
					return;
				}
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = "CommandFulfillTradeOfferConfirm".Translate(GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount));
				Action confirmedAct;
				if ((confirmedAct = <>9__1) == null)
				{
					confirmedAct = (<>9__1 = delegate
					{
						this.Fulfill(caravan);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null));
			};
			if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
			{
				command_Action.Disable("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)));
			}
			return command_Action;
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x0026B028 File Offset: 0x00269228
		private void Fulfill(Caravan caravan)
		{
			int remaining = this.requestCount;
			List<Thing> list = CaravanInventoryUtility.TakeThings(caravan, delegate(Thing thing)
			{
				if (this.requestThingDef != thing.def)
				{
					return 0;
				}
				if (!this.PlayerCanGive(thing))
				{
					return 0;
				}
				int num = Mathf.Min(remaining, thing.stackCount);
				remaining -= num;
				return num;
			});
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
			if (this.parent.Faction != null)
			{
				this.parent.Faction.TryAffectGoodwillWith(Faction.OfPlayer, 12, true, true, "GoodwillChangedReason_FulfilledTradeRequest".Translate(), new GlobalTargetInfo?(this.parent));
			}
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "TradeRequestFulfilled", this.parent.Named("SUBJECT"), caravan.Named("CARAVAN"));
			this.Disable();
		}

		// Token: 0x06006F1A RID: 28442 RVA: 0x0026B0FC File Offset: 0x002692FC
		private bool PlayerCanGive(Thing thing)
		{
			if (thing.GetRotStage() != RotStage.Fresh)
			{
				return false;
			}
			Apparel apparel = thing as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			return compQuality == null || compQuality.Quality >= QualityCategory.Normal;
		}

		// Token: 0x0400444F RID: 17487
		public ThingDef requestThingDef;

		// Token: 0x04004450 RID: 17488
		public int requestCount;

		// Token: 0x04004451 RID: 17489
		public int expiration = -1;

		// Token: 0x04004452 RID: 17490
		public string outSignalFulfilled;

		// Token: 0x04004453 RID: 17491
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest", true);
	}
}
