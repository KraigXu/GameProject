using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020011B3 RID: 4531
	public interface ISlateRef
	{
		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x060068B6 RID: 26806
		// (set) Token: 0x060068B7 RID: 26807
		string SlateRef { get; set; }

		// Token: 0x060068B8 RID: 26808
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
