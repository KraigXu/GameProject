using System;

namespace RimWorld.QuestGenNew
{
	
	public interface ISlateRef
	{
		
		
		
		string SlateRef { get; set; }

		
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
