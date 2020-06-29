using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public class QuestTextRequest
	{
		
		public string keyword;

		
		public Action<string> setter;

		
		public List<Rule> extraRules;
	}
}
