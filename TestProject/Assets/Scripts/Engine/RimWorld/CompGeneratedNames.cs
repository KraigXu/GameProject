using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class CompGeneratedNames : ThingComp
	{
		
		
		public CompProperties_GeneratedName Props
		{
			get
			{
				return (CompProperties_GeneratedName)this.props;
			}
		}

		
		public override string TransformLabel(string label)
		{
			if (this.parent.GetComp<CompBladelinkWeapon>() != null)
			{
				return this.name + ", " + label;
			}
			return this.name + " (" + label + ")";
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.name = GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_weapon_name", new GrammarRequest
			{
				Includes = 
				{
					this.Props.nameMaker
				}
			}, null, false, null, null, null, true));
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
		}

		
		private string name;
	}
}
