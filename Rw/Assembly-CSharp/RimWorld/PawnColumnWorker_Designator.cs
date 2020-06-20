using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECF RID: 3791
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06005CF4 RID: 23796
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x06005CF5 RID: 23797 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x06005CF6 RID: 23798 RVA: 0x002041BF File Offset: 0x002023BF
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x06005CF7 RID: 23799 RVA: 0x002041CC File Offset: 0x002023CC
		protected override void SetValue(Pawn pawn, bool value)
		{
			if (value == this.GetValue(pawn))
			{
				return;
			}
			if (value)
			{
				pawn.MapHeld.designationManager.AddDesignation(new Designation(pawn, this.DesignationType));
				this.Notify_DesignationAdded(pawn);
				return;
			}
			Designation designation = this.GetDesignation(pawn);
			if (designation != null)
			{
				pawn.MapHeld.designationManager.RemoveDesignation(designation);
			}
		}

		// Token: 0x06005CF8 RID: 23800 RVA: 0x0020422C File Offset: 0x0020242C
		private Designation GetDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return null;
			}
			return mapHeld.designationManager.DesignationOn(pawn, this.DesignationType);
		}
	}
}
