using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AD RID: 2477
	public struct Signal
	{
		// Token: 0x06003AF1 RID: 15089 RVA: 0x00138411 File Offset: 0x00136611
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = default(SignalArgs);
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x00138426 File Offset: 0x00136626
		public Signal(string tag, SignalArgs args)
		{
			this.tag = tag;
			this.args = args;
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x00138436 File Offset: 0x00136636
		public Signal(string tag, NamedArgument arg1)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1);
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x0013844B File Offset: 0x0013664B
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2);
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x00138461 File Offset: 0x00136661
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3);
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00138479 File Offset: 0x00136679
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x00138493 File Offset: 0x00136693
		public Signal(string tag, params NamedArgument[] args)
		{
			this.tag = tag;
			this.args = new SignalArgs(args);
		}

		// Token: 0x040022E3 RID: 8931
		public string tag;

		// Token: 0x040022E4 RID: 8932
		public SignalArgs args;
	}
}
