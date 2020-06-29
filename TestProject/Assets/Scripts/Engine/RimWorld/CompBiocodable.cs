using System;
using Verse;

namespace RimWorld
{
	
	public class CompBiocodable : ThingComp
	{
		
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x001B16E4 File Offset: 0x001AF8E4
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		
		// (get) Token: 0x06005090 RID: 20624 RVA: 0x001B16EC File Offset: 0x001AF8EC
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		
		// (get) Token: 0x06005091 RID: 20625 RVA: 0x001B16F4 File Offset: 0x001AF8F4
		public string CodedPawnLabel
		{
			get
			{
				return this.codedPawnLabel;
			}
		}

		
		public void CodeFor(Pawn p)
		{
			this.biocoded = true;
			this.codedPawn = p;
			this.codedPawnLabel = p.Name.ToStringFull;
		}

		
		public override string TransformLabel(string label)
		{
			if (!this.biocoded)
			{
				return label;
			}
			return "Biocoded".Translate(label, this.parent.def).Resolve();
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.biocoded)
			{
				return string.Empty;
			}
			return "CodedFor".Translate(this.codedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.biocoded, "biocoded", false, false);
			Scribe_Values.Look<string>(ref this.codedPawnLabel, "biocodedPawnLabel", null, false);
			Scribe_References.Look<Pawn>(ref this.codedPawn, "codedPawn", true);
		}

		
		protected bool biocoded;

		
		protected string codedPawnLabel;

		
		protected Pawn codedPawn;
	}
}
