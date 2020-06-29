﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		
		
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		
		public MaterialRequest(Texture2D tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		
		public MaterialRequest(Texture2D tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		
		public MaterialRequest(Texture2D tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture2D>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters);
		}

		
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MaterialRequest(",
				this.shader.name,
				", ",
				this.mainTex.name,
				", ",
				this.color.ToString(),
				", ",
				this.colorTwo.ToString(),
				", ",
				this.maskTex.ToString(),
				", ",
				this.renderQueue.ToString(),
				")"
			});
		}

		
		public Shader shader;

		
		public Texture2D mainTex;

		
		public Color color;

		
		public Color colorTwo;

		
		public Texture2D maskTex;

		
		public int renderQueue;

		
		public List<ShaderParameter> shaderParameters;
	}
}
