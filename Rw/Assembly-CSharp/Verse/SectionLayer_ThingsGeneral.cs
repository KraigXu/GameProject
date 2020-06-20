using System;

namespace Verse
{
	// Token: 0x0200019D RID: 413
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06000BB6 RID: 2998 RVA: 0x000427E4 File Offset: 0x000409E4
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000427FC File Offset: 0x000409FC
		protected override void TakePrintFrom(Thing t)
		{
			try
			{
				t.Print(this);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception printing ",
					t,
					" at ",
					t.Position,
					": ",
					ex.ToString()
				}), false);
			}
		}
	}
}
