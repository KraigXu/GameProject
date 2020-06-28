using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000D13 RID: 3347
	public class CompGeneratedNames : ThingComp
	{
		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x001B4867 File Offset: 0x001B2A67
		public CompProperties_GeneratedName Props
		{
			get
			{
				return (CompProperties_GeneratedName)this.props;
			}
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x001B4874 File Offset: 0x001B2A74
		public override string TransformLabel(string label)
		{
			if (this.parent.GetComp<CompBladelinkWeapon>() != null)
			{
				return this.name + ", " + label;
			}
			return this.name + " (" + label + ")";
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001B48AC File Offset: 0x001B2AAC
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

		// Token: 0x06005165 RID: 20837 RVA: 0x001B48FB File Offset: 0x001B2AFB
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
		}

		// Token: 0x04002D0D RID: 11533
		private string name;
	}
}
