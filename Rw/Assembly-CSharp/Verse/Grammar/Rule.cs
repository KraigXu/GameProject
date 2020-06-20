using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x020004C6 RID: 1222
	public abstract class Rule
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002404 RID: 9220
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000D7C84 File Offset: 0x000D5E84
		public virtual Rule DeepCopy()
		{
			Rule rule = (Rule)Activator.CreateInstance(base.GetType());
			rule.keyword = this.keyword;
			rule.tag = this.tag;
			rule.requiredTag = this.requiredTag;
			if (this.constantConstraints != null)
			{
				rule.constantConstraints = this.constantConstraints.ToList<Rule.ConstantConstraint>();
			}
			return rule;
		}

		// Token: 0x06002407 RID: 9223
		public abstract string Generate();

		// Token: 0x06002408 RID: 9224 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Init()
		{
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000D7CE0 File Offset: 0x000D5EE0
		public void AddConstantConstraint(string key, string value, Rule.ConstantConstraint.Type type)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<Rule.ConstantConstraint>();
			}
			this.constantConstraints.Add(new Rule.ConstantConstraint
			{
				key = key,
				value = value,
				type = type
			});
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000D7D2C File Offset: 0x000D5F2C
		public void AddConstantConstraint(string key, string value, string op)
		{
			Rule.ConstantConstraint.Type type;
			if (!(op == "=="))
			{
				if (!(op == "!="))
				{
					if (!(op == "<"))
					{
						if (!(op == ">"))
						{
							if (!(op == "<="))
							{
								if (!(op == ">="))
								{
									type = Rule.ConstantConstraint.Type.Equal;
									Log.Error("Unknown ConstantConstraint type: " + op, false);
								}
								else
								{
									type = Rule.ConstantConstraint.Type.GreaterOrEqual;
								}
							}
							else
							{
								type = Rule.ConstantConstraint.Type.LessOrEqual;
							}
						}
						else
						{
							type = Rule.ConstantConstraint.Type.Greater;
						}
					}
					else
					{
						type = Rule.ConstantConstraint.Type.Less;
					}
				}
				else
				{
					type = Rule.ConstantConstraint.Type.NotEqual;
				}
			}
			else
			{
				type = Rule.ConstantConstraint.Type.Equal;
			}
			this.AddConstantConstraint(key, value, type);
		}

		// Token: 0x040015C5 RID: 5573
		[MayTranslate]
		public string keyword;

		// Token: 0x040015C6 RID: 5574
		[NoTranslate]
		public string tag;

		// Token: 0x040015C7 RID: 5575
		[NoTranslate]
		public string requiredTag;

		// Token: 0x040015C8 RID: 5576
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x020016D1 RID: 5841
		public struct ConstantConstraint
		{
			// Token: 0x040057AB RID: 22443
			[MayTranslate]
			public string key;

			// Token: 0x040057AC RID: 22444
			[MayTranslate]
			public string value;

			// Token: 0x040057AD RID: 22445
			public Rule.ConstantConstraint.Type type;

			// Token: 0x02002086 RID: 8326
			public enum Type
			{
				// Token: 0x040079B2 RID: 31154
				Equal,
				// Token: 0x040079B3 RID: 31155
				NotEqual,
				// Token: 0x040079B4 RID: 31156
				Less,
				// Token: 0x040079B5 RID: 31157
				Greater,
				// Token: 0x040079B6 RID: 31158
				LessOrEqual,
				// Token: 0x040079B7 RID: 31159
				GreaterOrEqual
			}
		}
	}
}
