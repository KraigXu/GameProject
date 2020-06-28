using System;

namespace Verse
{
	// Token: 0x020002D6 RID: 726
	public static class Scribe_Defs
	{
		// Token: 0x06001462 RID: 5218 RVA: 0x000780E0 File Offset: 0x000762E0
		public static void Look<T>(ref T value, string label) where T : Def, new()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text;
				if (value == null)
				{
					text = "null";
				}
				else
				{
					text = value.defName;
				}
				Scribe_Values.Look<string>(ref text, label, "null", false);
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.DefFromNode<T>(Scribe.loader.curXmlParent[label]);
			}
		}
	}
}
