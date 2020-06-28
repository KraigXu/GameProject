using System;

namespace Verse
{
	// Token: 0x020002F6 RID: 758
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x0007D634 File Offset: 0x0007B834
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0007D640 File Offset: 0x0007B840
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Terrain(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				")"
			});
		}
	}
}
