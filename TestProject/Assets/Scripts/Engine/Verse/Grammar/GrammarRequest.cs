using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	
	public struct GrammarRequest
	{
		
		
		public List<Rule> RulesAllowNull
		{
			get
			{
				return this.rules;
			}
		}

		
		
		public List<Rule> Rules
		{
			get
			{
				if (this.rules == null)
				{
					this.rules = new List<Rule>();
				}
				return this.rules;
			}
		}

		
		
		public List<RulePack> IncludesBareAllowNull
		{
			get
			{
				return this.includesBare;
			}
		}

		
		
		public List<RulePack> IncludesBare
		{
			get
			{
				if (this.includesBare == null)
				{
					this.includesBare = new List<RulePack>();
				}
				return this.includesBare;
			}
		}

		
		
		public List<RulePackDef> IncludesAllowNull
		{
			get
			{
				return this.includes;
			}
		}

		
		
		public List<RulePackDef> Includes
		{
			get
			{
				if (this.includes == null)
				{
					this.includes = new List<RulePackDef>();
				}
				return this.includes;
			}
		}

		
		
		public Dictionary<string, string> ConstantsAllowNull
		{
			get
			{
				return this.constants;
			}
		}

		
		
		public Dictionary<string, string> Constants
		{
			get
			{
				if (this.constants == null)
				{
					this.constants = new Dictionary<string, string>();
				}
				return this.constants;
			}
		}

		
		public void Clear()
		{
			if (this.rules != null)
			{
				this.rules.Clear();
			}
			if (this.includesBare != null)
			{
				this.includesBare.Clear();
			}
			if (this.includes != null)
			{
				this.includes.Clear();
			}
			if (this.constants != null)
			{
				this.constants.Clear();
			}
		}

		
		private List<Rule> rules;

		
		private List<RulePack> includesBare;

		
		private List<RulePackDef> includes;

		
		private Dictionary<string, string> constants;
	}
}
