using System;
using UnityEngine;

namespace Verse
{
	
	public class ShadowData
	{
		
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000421BA File Offset: 0x000403BA
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x000421C7 File Offset: 0x000403C7
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x000421D4 File Offset: 0x000403D4
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		
		public Vector3 volume = Vector3.one;

		
		public Vector3 offset = Vector3.zero;
	}
}
