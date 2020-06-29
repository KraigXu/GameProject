using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		
		public float Evaluate(float x, float y)
		{
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.", false);
				return 0f;
			}
			if (x <= this.columns[0].x)
			{
				return this.columns[0].y.Evaluate(y);
			}
			if (x >= this.columns[this.columns.Count - 1].x)
			{
				return this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			SurfaceColumn surfaceColumn = this.columns[0];
			SurfaceColumn surfaceColumn2 = this.columns[this.columns.Count - 1];
			int i = 0;
			while (i < this.columns.Count)
			{
				if (x <= this.columns[i].x)
				{
					surfaceColumn2 = this.columns[i];
					if (i > 0)
					{
						surfaceColumn = this.columns[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
			return Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
		}

		
		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		
		public IEnumerator<SurfaceColumn> GetEnumerator()
		{
			foreach (SurfaceColumn surfaceColumn in this.columns)
			{
				yield return surfaceColumn;
			}
			List<SurfaceColumn>.Enumerator enumerator = default(List<SurfaceColumn>.Enumerator);
			yield break;
			yield break;
		}

		
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.columns.Count - 1; i++)
			{
				if (this.columns[i + 1].x < this.columns[i].x)
				{
					yield return prefix + ": columns are out of order";
					break;
				}
			}
			yield break;
		}

		
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();
	}
}
