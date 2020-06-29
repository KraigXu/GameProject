using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class Transferable : IExposable
	{
		
		// (get) Token: 0x060059EC RID: 23020
		public abstract Thing AnyThing { get; }

		
		// (get) Token: 0x060059ED RID: 23021
		public abstract ThingDef ThingDef { get; }

		
		// (get) Token: 0x060059EE RID: 23022
		public abstract bool Interactive { get; }

		
		// (get) Token: 0x060059EF RID: 23023
		public abstract bool HasAnyThing { get; }

		
		// (get) Token: 0x060059F0 RID: 23024 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsThing
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x060059F1 RID: 23025
		public abstract string Label { get; }

		
		// (get) Token: 0x060059F2 RID: 23026 RVA: 0x001E6B93 File Offset: 0x001E4D93
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.ThingDef);
			}
		}

		
		// (get) Token: 0x060059F3 RID: 23027
		public abstract string TipDescription { get; }

		
		// (get) Token: 0x060059F4 RID: 23028
		public abstract TransferablePositiveCountDirection PositiveCountDirection { get; }

		
		// (get) Token: 0x060059F5 RID: 23029
		// (set) Token: 0x060059F6 RID: 23030
		public abstract int CountToTransfer { get; protected set; }

		
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

		
		public abstract int GetMinimumToTransfer();

		
		public abstract int GetMaximumToTransfer();

		
		public int GetRange()
		{
			return this.GetMaximumToTransfer() - this.GetMinimumToTransfer();
		}

		
		public int ClampAmount(int amount)
		{
			return Mathf.Clamp(amount, this.GetMinimumToTransfer(), this.GetMaximumToTransfer());
		}

		
		public AcceptanceReport CanAdjustBy(int adjustment)
		{
			return this.CanAdjustTo(this.CountToTransfer + adjustment);
		}

		
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

		
		public void AdjustBy(int adjustment)
		{
			this.AdjustTo(this.CountToTransfer + adjustment);
		}

		
		public void AdjustTo(int destination)
		{
			if (!this.CanAdjustTo(destination).Accepted)
			{
				Log.Error("Failed to adjust transferable counts", false);
				return;
			}
			this.CountToTransfer = this.ClampAmount(destination);
		}

		
		public void ForceTo(int value)
		{
			this.CountToTransfer = value;
		}

		
		public void ForceToSource(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(value);
				return;
			}
			this.ForceTo(-value);
		}

		
		public void ForceToDestination(int value)
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Source)
			{
				this.ForceTo(-value);
				return;
			}
			this.ForceTo(value);
		}

		
		public virtual void DrawIcon(Rect iconRect)
		{
		}

		
		public virtual AcceptanceReport UnderflowReport()
		{
			return false;
		}

		
		public virtual AcceptanceReport OverflowReport()
		{
			return false;
		}

		
		public virtual void ExposeData()
		{
		}

		
		private string editBuffer = "0";
	}
}
