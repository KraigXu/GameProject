using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000410 RID: 1040
	public struct CurveMark
	{
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x000BF4FB File Offset: 0x000BD6FB
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x000BF503 File Offset: 0x000BD703
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x000BF50B File Offset: 0x000BD70B
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000BF513 File Offset: 0x000BD713
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x040012ED RID: 4845
		private float x;

		// Token: 0x040012EE RID: 4846
		private string message;

		// Token: 0x040012EF RID: 4847
		private Color color;
	}
}
