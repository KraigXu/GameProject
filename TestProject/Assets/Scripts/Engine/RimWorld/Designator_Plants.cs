using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E49 RID: 3657
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06005865 RID: 22629 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06005866 RID: 22630 RVA: 0x001D5553 File Offset: 0x001D3753
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x001CFE96 File Offset: 0x001CE096
		public Designator_Plants()
		{
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x001D555B File Offset: 0x001D375B
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.plant == null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.designationDef) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x001D5598 File Offset: 0x001D3798
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Plant plant = c.GetPlant(base.Map);
			if (plant == null)
			{
				return "MessageMustDesignatePlants".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(plant);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x001D5601 File Offset: 0x001D3801
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x001D5615 File Offset: 0x001D3815
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x001D0CA1 File Offset: 0x001CEEA1
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x04002FB6 RID: 12214
		protected DesignationDef designationDef;
	}
}
