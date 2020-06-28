using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class Building_Casket : Building, IThingHolder, IOpenable
	{
		// Token: 0x06004C6E RID: 19566 RVA: 0x0019AAB0 File Offset: 0x00198CB0
		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x0019AAC6 File Offset: 0x00198CC6
		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06004C70 RID: 19568 RVA: 0x0019AAD6 File Offset: 0x00198CD6
		public Thing ContainedThing
		{
			get
			{
				if (this.innerContainer.Count != 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x0019AAF3 File Offset: 0x00198CF3
		public bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x0019AAFB File Offset: 0x00198CFB
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x0019AB03 File Offset: 0x00198D03
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x0019AB11 File Offset: 0x00198D11
		public override void TickRare()
		{
			base.TickRare();
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x0019AB25 File Offset: 0x00198D25
		public override void Tick()
		{
			base.Tick();
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x0019AB39 File Offset: 0x00198D39
		public virtual void Open()
		{
			if (!this.HasAnyContents)
			{
				return;
			}
			this.EjectContents();
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x0019AB4A File Offset: 0x00198D4A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x0019AB7E File Offset: 0x00198D7E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x0019ABA4 File Offset: 0x00198DA4
		public override bool ClaimableBy(Faction fac)
		{
			if (this.innerContainer.Any)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					if (this.innerContainer[i].Faction == fac)
					{
						return true;
					}
				}
				return false;
			}
			return base.ClaimableBy(fac);
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x0019ABF3 File Offset: 0x00198DF3
		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x0019AC04 File Offset: 0x00198E04
		public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (!this.Accepts(thing))
			{
				return false;
			}
			bool flag;
			if (thing.holdingOwner != null)
			{
				thing.holdingOwner.TryTransferToContainer(thing, this.innerContainer, thing.stackCount, true);
				flag = true;
			}
			else
			{
				flag = this.innerContainer.TryAdd(thing, true);
			}
			if (flag)
			{
				if (thing.Faction != null && thing.Faction.IsPlayer)
				{
					this.contentsKnown = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x0019AC74 File Offset: 0x00198E74
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.innerContainer.Count > 0 && (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize))
			{
				if (mode != DestroyMode.Deconstruct)
				{
					List<Pawn> list = new List<Pawn>();
					foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							list.Add(pawn);
						}
					}
					foreach (Pawn p in list)
					{
						HealthUtility.DamageUntilDowned(p, true);
					}
				}
				this.EjectContents();
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.Destroy(mode);
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x0019AD40 File Offset: 0x00198F40
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
			this.contentsKnown = true;
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x0019AD64 File Offset: 0x00198F64
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string str;
			if (!this.contentsKnown)
			{
				str = "UnknownLower".Translate();
			}
			else
			{
				str = this.innerContainer.ContentsString;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("CasketContains".Translate() + ": " + str.CapitalizeFirst());
		}

		// Token: 0x04002B01 RID: 11009
		protected ThingOwner innerContainer;

		// Token: 0x04002B02 RID: 11010
		protected bool contentsKnown;
	}
}
