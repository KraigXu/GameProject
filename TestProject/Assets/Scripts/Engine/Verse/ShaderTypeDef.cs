using System;
using UnityEngine;

namespace Verse
{
	
	public class ShaderTypeDef : Def
	{
		
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

		
		[NoTranslate]
		public string shaderPath;

		
		[Unsaved(false)]
		private Shader shaderInt;
	}
}
