using System;
using Verse;

namespace RimWorld
{
	
	public class CompBiocodable : ThingComp
	{
		
		
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		
		
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		
		
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
