using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200003F RID: 63
	public class ShaderParameter
	{
		// Token: 0x06000373 RID: 883 RVA: 0x00012528 File Offset: 0x00010728
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

		// Token: 0x06000374 RID: 884 RVA: 0x000125BC File Offset: 0x000107BC
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

		// Token: 0x040000F0 RID: 240
		[NoTranslate]
		private string name;

		// Token: 0x040000F1 RID: 241
		private Vector4 value;

		// Token: 0x040000F2 RID: 242
		private Texture2D valueTex;

		// Token: 0x040000F3 RID: 243
		private ShaderParameter.Type type;

		// Token: 0x02001314 RID: 4884
		private enum Type
		{
			// Token: 0x0400482F RID: 18479
			Float,
			// Token: 0x04004830 RID: 18480
			Vector,
			// Token: 0x04004831 RID: 18481
			Matrix,
			// Token: 0x04004832 RID: 18482
			Texture
		}
	}
}
