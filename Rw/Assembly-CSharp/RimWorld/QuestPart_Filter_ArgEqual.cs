using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094F RID: 2383
	public class QuestPart_Filter_ArgEqual : QuestPart_Filter
	{
		// Token: 0x06003870 RID: 14448 RVA: 0x0012E24C File Offset: 0x0012C44C
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return args.TryGetArg(this.name, out namedArgument) && object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0012E27D File Offset: 0x0012C47D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0012E2B4 File Offset: 0x0012C4B4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x0400215B RID: 8539
		public string name;

		// Token: 0x0400215C RID: 8540
		public object obj;

		// Token: 0x0400215D RID: 8541
		public LookMode objLookMode;

		// Token: 0x0400215E RID: 8542
		private Type objType;
	}
}
