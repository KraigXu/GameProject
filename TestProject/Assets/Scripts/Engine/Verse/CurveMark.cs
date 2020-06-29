using System;
using UnityEngine;

namespace Verse
{
	
	public struct CurveMark
	{
		
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x000BF4FB File Offset: 0x000BD6FB
		public float X
		{
			get
			{
				return this.x;
			}
		}

		
		// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x000BF503 File Offset: 0x000BD703
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x000BF50B File Offset: 0x000BD70B
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		
		private float x;

		
		private string message;

		
		private Color color;
	}
}
