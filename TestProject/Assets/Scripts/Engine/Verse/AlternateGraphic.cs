using System;
using UnityEngine;

namespace Verse
{
	
	public class AlternateGraphic
	{
		
		
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		
		public Graphic GetGraphic(Graphic other)
		{
			if (this.graphicData == null)
			{
				this.graphicData = new GraphicData();
			}
			this.graphicData.CopyFrom(other.data);
			if (!this.texPath.NullOrEmpty())
			{
				this.graphicData.texPath = this.texPath;
			}
			this.graphicData.color = (this.color ?? other.color);
			this.graphicData.colorTwo = (this.colorTwo ?? other.colorTwo);
			return this.graphicData.Graphic;
		}

		
		private float weight = 0.5f;

		
		private string texPath;

		
		private Color? color;

		
		private Color? colorTwo;

		
		private GraphicData graphicData;
	}
}
