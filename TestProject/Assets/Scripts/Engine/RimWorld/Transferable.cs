using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7A RID: 3706
	public abstract class Transferable : IExposable
	{
		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x060059EC RID: 23020
		public abstract Thing AnyThing { get; }

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x060059ED RID: 23021
		public abstract ThingDef ThingDef { get; }

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x060059EE RID: 23022
		public abstract bool Interactive { get; }

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x060059EF RID: 23023
		public abstract bool HasAnyThing { get; }

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x060059F0 RID: 23024 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsThing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x060059F1 RID: 23025
		public abstract string Label { get; }

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x060059F2 RID: 23026 RVA: 0x001E6B93 File Offset: 0x001E4D93
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x060059F3 RID: 23027
		public abstract string TipDescription { get; }

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x060059F4 RID: 23028
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x060059F5 RID: 23029
		// (set) Token: 0x060059F6 RID: 23030
		public abstract int CountToTransfer { get; protected set; }

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x060059F7 RID: 23031 RVA: 0x001E6BA6 File Offset: 0x001E4DA6
		public int CountToTransferToSource
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return -this.CountToTransfer;
				}
				return this.CountToTransfer;
			}
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x060059F8 RID: 23032 RVA: 0x001E6BBE File Offset: 0x001E4DBE
		public int CountToTransferToDestination
		{
			get
			{
				if (this.PositiveCountDirection != TransferablePositiveCountDirection.Source)
				{
					return this.CountToTransfer;
				}
				return -this.CountToTransfer;
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x060059F9 RID: 23033 RVA: 0x001E6BD6 File Offset: 0x001E4DD6
		// (set) Token: 0x060059FA RID: 23034 RVA: 0x001E6BDE File Offset: 0x001E4DDE
		public string EditBuffer
		{
			get
			{
				return this.editBuffer;
			}
			set
			{
				this.editBuffer = value;
			}
		}

		// Token: 0x060059FB RID: 23035
		public abstract int GetMinimumToTransfer();

		// Token: 0x060059FC RID: 23036
		public abstract int GetMaximumToTransfer();

		// Token: 0x060059FD RID: 23037 RVA: 0x001E6BE7 File Offset: 0x001E4DE7
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x001E6BF6 File Offset: 0x001E4DF6
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x001E6C0A File Offset: 0x001E4E0A
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x001E6C1A File Offset: 0x001E4E1A
		public AcceptanceReport CanAdjustTo(int destination)
		{
			if (destination == this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (this.ClampAmount(destination) != this.CountToTransfer)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (destination < this.CountToTransfer)
			{
				return this.UnderflowReport();
			}
			return this.OverflowReport();
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x001E6C56 File Offset: 0x001E4E56
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x001E6C68 File Offset: 0x001E4E68
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts", false);
				return;
			}
			this.CountToTransfer = this.ClampAmount(destination);
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x001E6C9F File Offset: 0x001E4E9F
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x001E6CA8 File Offset: 0x001E4EA8
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
				return;
			}
			this.ForceTo(-value);
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x001E6CC2 File Offset: 0x001E4EC2
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
				return;
			}
			this.ForceTo(value);
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawIcon(Rect iconRect)
		{
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x001E6CDC File Offset: 0x001E4EDC
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x001E6CDC File Offset: 0x001E4EDC
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x040030E2 RID: 12514
		private string editBuffer = "0";
	}
}
