using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008DB RID: 2267
	public class JoyGiverDef : Def
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06003658 RID: 13912 RVA: 0x00126A18 File Offset: 0x00124C18
		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x00126A4A File Offset: 0x00124C4A
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.jobDef != null && this.jobDef.joyKind != this.joyKind)
			{
				yield return string.Concat(new object[]
				{
					"jobDef ",
					this.jobDef,
					" has joyKind ",
					this.jobDef.joyKind,
					" which does not match our joyKind ",
					this.joyKind
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x04001EBC RID: 7868
		public Type giverClass;

		// Token: 0x04001EBD RID: 7869
		public float baseChance;

		// Token: 0x04001EBE RID: 7870
		public bool requireChair = true;

		// Token: 0x04001EBF RID: 7871
		public List<ThingDef> thingDefs;

		// Token: 0x04001EC0 RID: 7872
		public JobDef jobDef;

		// Token: 0x04001EC1 RID: 7873
		public bool desireSit = true;

		// Token: 0x04001EC2 RID: 7874
		public float pctPawnsEverDo = 1f;

		// Token: 0x04001EC3 RID: 7875
		public bool unroofedOnly;

		// Token: 0x04001EC4 RID: 7876
		public JoyKindDef joyKind;

		// Token: 0x04001EC5 RID: 7877
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04001EC6 RID: 7878
		public bool canDoWhileInBed;

		// Token: 0x04001EC7 RID: 7879
		private JoyGiver workerInt;
	}
}
