using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000950 RID: 2384
	public class QuestPart_Filter_ArgNotEqual : QuestPart_Filter
	{
		// Token: 0x06003874 RID: 14452 RVA: 0x0012E2D4 File Offset: 0x0012C4D4
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return !args.TryGetArg(this.name, out namedArgument) || !object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0012E308 File Offset: 0x0012C508
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0012E33F File Offset: 0x0012C53F
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x0400215F RID: 8543
		public string name;

		// Token: 0x04002160 RID: 8544
		public object obj;

		// Token: 0x04002161 RID: 8545
		private Type objType;

		// Token: 0x04002162 RID: 8546
		private LookMode objLookMode;
	}
}
