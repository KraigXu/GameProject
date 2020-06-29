using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	
	public abstract class Rule
	{
		
		// (get) Token: 0x06002404 RID: 9220
		public abstract float BaseSelectionWeight { get; }

		
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		
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

		
		public abstract string Generate();

		
		public virtual void Init()
		{
		}

		
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

		
		[MayTranslate]
		public string keyword;

		
		[NoTranslate]
		public string tag;

		
		[NoTranslate]
		public string requiredTag;

		
		public List<Rule.ConstantConstraint> constantConstraints;

		
		public struct ConstantConstraint
		{
			
			[MayTranslate]
			public string key;

			
			[MayTranslate]
			public string value;

			
			public Rule.ConstantConstraint.Type type;

			
			public enum Type
			{
				
				Equal,
				
				NotEqual,
				
				Less,
				
				Greater,
				
				LessOrEqual,
				
				GreaterOrEqual
			}
		}
	}
}
