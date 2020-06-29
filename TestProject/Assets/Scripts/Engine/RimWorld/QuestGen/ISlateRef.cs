using System;

namespace RimWorld.QuestGen
{
	
	public interface ISlateRef
	{
		
		// (get) Token: 0x060068B6 RID: 26806
		// (set) Token: 0x060068B7 RID: 26807
		string SlateRef { get; set; }

		
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
