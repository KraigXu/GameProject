using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	
	public class ShaderParameter
	{
		
		public void Apply(Material mat)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				mat.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				mat.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce(string.Format("Texture for {0} is not yet loaded; file may be invalid, or main thread may not have loaded it yet", this.name), 27929440, false);
				}
				mat.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml, false);
				return;
			}
			this.name = xmlRoot.Name;
			string valstr = xmlRoot.FirstChild.Value;
			if (!valstr.NullOrEmpty() && valstr[0] == '(')
			{
				this.value = ParseHelper.FromStringVector4Adaptive(valstr);
				this.type = ShaderParameter.Type.Vector;
				return;
			}
			if (!valstr.NullOrEmpty() && valstr[0] == '/')
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart(new char[]
					{
						'/'
					}), true);
				});
				this.type = ShaderParameter.Type.Texture;
				return;
			}
			this.value = Vector4.one * ParseHelper.FromString<float>(valstr);
			this.type = ShaderParameter.Type.Float;
		}

		
		[NoTranslate]
		private string name;

		
		private Vector4 value;

		
		private Texture2D valueTex;

		
		private ShaderParameter.Type type;

		
		private enum Type
		{
			
			Float,
			
			Vector,
			
			Matrix,
			
			Texture
		}
	}
}
