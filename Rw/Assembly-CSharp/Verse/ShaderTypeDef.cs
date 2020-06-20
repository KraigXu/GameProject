using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E8 RID: 232
	public class ShaderTypeDef : Def
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0001DE02 File Offset: 0x0001C002
		public Shader Shader
		{
			get
			{
				if (this.shaderInt == null)
				{
					this.shaderInt = ShaderDatabase.LoadShader(this.shaderPath);
				}
				return this.shaderInt;
			}
		}

		// Token: 0x04000563 RID: 1379
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04000564 RID: 1380
		[Unsaved(false)]
		private Shader shaderInt;
	}
}
