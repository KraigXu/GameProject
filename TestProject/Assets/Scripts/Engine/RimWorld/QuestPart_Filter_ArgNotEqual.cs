using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Filter_ArgNotEqual : QuestPart_Filter
	{
		
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return !args.TryGetArg(this.name, out namedArgument) || !object.Equals(this.obj, namedArgument.arg);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		
		public string name;

		
		public object obj;

		
		private Type objType;

		
		private LookMode objLookMode;
	}
}
