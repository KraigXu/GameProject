using System;
using UnityEngine;

namespace Verse
{
	
	public struct TextureAndColor
	{
		
		
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		
		
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		
		
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		
		
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		
		private Texture2D texture;

		
		private Color color;
	}
}
