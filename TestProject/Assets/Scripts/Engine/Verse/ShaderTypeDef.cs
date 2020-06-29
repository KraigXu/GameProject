using System;
using UnityEngine;

namespace Verse
{
	
	public class ShaderTypeDef : Def
	{
		
		
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
