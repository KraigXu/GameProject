using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class CompGeneratedNames : ThingComp
	{
		
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x001B4867 File Offset: 0x001B2A67
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
