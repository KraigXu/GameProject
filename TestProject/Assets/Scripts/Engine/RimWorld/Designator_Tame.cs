using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E37 RID: 3639
	public class Designator_Tame : Designator
	{
		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x060057F0 RID: 22512 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x060057F1 RID: 22513 RVA: 0x001D2BD1 File Offset: 0x001D0DD1
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x001D2BD8 File Offset: 0x001D0DD8
		public Designator_Tame()
		{
			this.defaultLabel = "DesignatorTame".Translate();
			this.defaultDesc = "DesignatorTameDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.tutorTag = "Tame";
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x001D2C6F File Offset: 0x001D0E6F
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.TameablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateTameable".Translate();
			}
			return true;
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x001D2CAC File Offset: 0x001D0EAC
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.TameablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x001D2CFC File Offset: 0x001D0EFC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && TameUtility.CanTame(pawn) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null;
		}

		// Token: 0x060057F6 RID: 22518 RVA: 0x001D2D40 File Offset: 0x001D0F40
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					TameUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind), true);
				}
			}
			this.justDesignated.Clear();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTaming, KnowledgeAmount.Total);
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x001D2DF4 File Offset: 0x001D0FF4
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x001D2E45 File Offset: 0x001D1045
		private IEnumerable<Pawn> TameablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return (Pawn)thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04002FA5 RID: 12197
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
