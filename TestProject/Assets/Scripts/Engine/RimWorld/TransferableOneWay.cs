using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class TransferableOneWay : Transferable
	{
		
		// (get) Token: 0x06005A1C RID: 23068 RVA: 0x001E6E62 File Offset: 0x001E5062
		public override Thing AnyThing
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.things[0];
			}
		}

		
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x001C5D00 File Offset: 0x001C3F00
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		
		// (get) Token: 0x06005A1E RID: 23070 RVA: 0x001E6E7A File Offset: 0x001E507A
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		
		// (get) Token: 0x06005A1F RID: 23071 RVA: 0x001C5C54 File Offset: 0x001C3E54
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Interactive
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06005A21 RID: 23073 RVA: 0x0001028D File Offset: 0x0000E48D
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		
		// (get) Token: 0x06005A22 RID: 23074 RVA: 0x001C5D2E File Offset: 0x001C3F2E
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x001E6E8A File Offset: 0x001E508A
		// (set) Token: 0x06005A24 RID: 23076 RVA: 0x001E6E92 File Offset: 0x001E5092
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		
		// (get) Token: 0x06005A25 RID: 23077 RVA: 0x001E6EA8 File Offset: 0x001E50A8
		public int MaxCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.", false);
				}
			}
		}

		
		public List<Thing> things = new List<Thing>();

		
		private int countToTransfer;
	}
}
