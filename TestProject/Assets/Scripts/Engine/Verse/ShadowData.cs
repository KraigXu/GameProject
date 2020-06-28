using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019A RID: 410
	public class ShadowData
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000421BA File Offset: 0x000403BA
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x000421C7 File Offset: 0x000403C7
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x000421D4 File Offset: 0x000403D4
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04000956 RID: 2390
		public Vector3 volume = Vector3.one;

		// Token: 0x04000957 RID: 2391
		public Vector3 offset = Vector3.zero;
	}
}
