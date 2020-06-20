using System;

namespace RimWorld
{
	// Token: 0x02000BCC RID: 3020
	public static class ThoughtMaker
	{
		// Token: 0x060047A0 RID: 18336 RVA: 0x00184E9E File Offset: 0x0018309E
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x00184EBD File Offset: 0x001830BD
		public static Thought_Memory MakeThought(ThoughtDef def, int forcedStage)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.SetForcedStage(forcedStage);
			thought_Memory.Init();
			return thought_Memory;
		}
	}
}
