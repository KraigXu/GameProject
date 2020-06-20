using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEF RID: 3311
	public class CompBiocodable : ThingComp
	{
		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x001B16E4 File Offset: 0x001AF8E4
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06005090 RID: 20624 RVA: 0x001B16EC File Offset: 0x001AF8EC
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06005091 RID: 20625 RVA: 0x001B16F4 File Offset: 0x001AF8F4
		public string CodedPawnLabel
		{
			get
			{
				return this.codedPawnLabel;
			}
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001B16FC File Offset: 0x001AF8FC
		public void CodeFor(Pawn p)
		{
			this.biocoded = true;
			this.codedPawn = p;
			this.codedPawnLabel = p.Name.ToStringFull;
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x001B1720 File Offset: 0x001AF920
		public override string TransformLabel(string label)
		{
			if (!this.biocoded)
			{
				return label;
			}
			return "Biocoded".Translate(label, this.parent.def).Resolve();
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x001B1760 File Offset: 0x001AF960
		public override string CompInspectStringExtra()
		{
			if (!this.biocoded)
			{
				return string.Empty;
			}
			return "CodedFor".Translate(this.codedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001B179F File Offset: 0x001AF99F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.biocoded, "biocoded", false, false);
			Scribe_Values.Look<string>(ref this.codedPawnLabel, "biocodedPawnLabel", null, false);
			Scribe_References.Look<Pawn>(ref this.codedPawn, "codedPawn", true);
		}

		// Token: 0x04002CC7 RID: 11463
		protected bool biocoded;

		// Token: 0x04002CC8 RID: 11464
		protected string codedPawnLabel;

		// Token: 0x04002CC9 RID: 11465
		protected Pawn codedPawn;
	}
}
