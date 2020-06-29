using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	
	public struct GrammarRequest
	{
		
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000D6B87 File Offset: 0x000D4D87
		public List<Rule> RulesAllowNull
		{
			get
			{
				return this.rules;
			}
		}

		
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000D6B8F File Offset: 0x000D4D8F
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

		
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000D6BAA File Offset: 0x000D4DAA
		public List<RulePack> IncludesBareAllowNull
		{
			get
			{
				return this.includesBare;
			}
		}

		
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x000D6BB2 File Offset: 0x000D4DB2
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

		
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000D6BCD File Offset: 0x000D4DCD
		public List<RulePackDef> IncludesAllowNull
		{
			get
			{
				return this.includes;
			}
		}

		
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x000D6BD5 File Offset: 0x000D4DD5
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

		
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000D6BF0 File Offset: 0x000D4DF0
		public Dictionary<string, string> ConstantsAllowNull
		{
			get
			{
				return this.constants;
			}
		}

		
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x000D6BF8 File Offset: 0x000D4DF8
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
