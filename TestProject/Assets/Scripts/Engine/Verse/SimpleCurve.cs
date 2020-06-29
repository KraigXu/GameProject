using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable
	{
		
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00005F93 File Offset: 0x00004193
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00005FA0 File Offset: 0x000041A0
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00005FA8 File Offset: 0x000041A8
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00005FB3 File Offset: 0x000041B3
		public SimpleCurveView View
		{
			get
			{
				if (this.view == null)
				{
					this.view = new SimpleCurveView();
					this.view.SetViewRectAround(this);
				}
				return this.view;
			}
		}

		
		public SimpleCurve(IEnumerable<CurvePoint> points)
		{
			this.SetPoints(points);
		}

		
		public SimpleCurve()
		{
		}

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			foreach (CurvePoint curvePoint in this.points)
			{
				yield return curvePoint;
			}
			List<CurvePoint>.Enumerator enumerator = default(List<CurvePoint>.Enumerator);
			yield break;
			yield break;
		}

		
		public CurvePoint this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
			}
		}

		
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		
		public float ClampToCurve(float value)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Clamping a value to an empty SimpleCurve.", false);
				return value;
			}
			return Mathf.Clamp(value, this.points[0].y, this.points[this.points.Count - 1].y);
		}

		
		public void RemovePointNear(CurvePoint point)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				if ((this.points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
				{
					this.points.RemoveAt(i);
					return;
				}
			}
		}

		
		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.", false);
				return 0f;
			}
			if (x <= this.points[0].x)
			{
				return this.points[0].y;
			}
			if (x >= this.points[this.points.Count - 1].x)
			{
				return this.points[this.points.Count - 1].y;
			}
			CurvePoint curvePoint = this.points[0];
			CurvePoint curvePoint2 = this.points[this.points.Count - 1];
			int i = 0;
			while (i < this.points.Count)
			{
				if (x <= this.points[i].x)
				{
					curvePoint2 = this.points[i];
					if (i > 0)
					{
						curvePoint = this.points[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
			return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
		}

		
		public float EvaluateInverted(float y)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.", false);
				return 0f;
			}
			if (this.points.Count == 1)
			{
				return this.points[0].x;
			}
			int i = 0;
			while (i < this.points.Count - 1)
			{
				if ((y >= this.points[i].y && y <= this.points[i + 1].y) || (y <= this.points[i].y && y >= this.points[i + 1].y))
				{
					if (y == this.points[i].y)
					{
						return this.points[i].x;
					}
					if (y == this.points[i + 1].y)
					{
						return this.points[i + 1].x;
					}
					return GenMath.LerpDouble(this.points[i].y, this.points[i + 1].y, this.points[i].x, this.points[i + 1].x, y);
				}
				else
				{
					i++;
				}
			}
			if (y < this.points[0].y)
			{
				float result = 0f;
				float num = 0f;
				for (int j = 0; j < this.points.Count; j++)
				{
					if (j == 0 || this.points[j].y < num)
					{
						num = this.points[j].y;
						result = this.points[j].x;
					}
				}
				return result;
			}
			float result2 = 0f;
			float num2 = 0f;
			for (int k = 0; k < this.points.Count; k++)
			{
				if (k == 0 || this.points[k].y > num2)
				{
					num2 = this.points[k].y;
					result2 = this.points[k].x;
				}
			}
			return result2;
		}

		
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0f)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.", false);
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0f)
			{
				Log.Error("PeriodicProbability got negative probability from " + this + ": slope should never be negative.", false);
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.points.Count - 1; i++)
			{
				if (this.points[i + 1].x < this.points[i].x)
				{
					yield return prefix + ": points are out of order";
					break;
				}
			}
			yield break;
		}

		
		private List<CurvePoint> points = new List<CurvePoint>();

		
		[Unsaved(false)]
		private SimpleCurveView view;

		
		private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
		{
			if (a.x < b.x)
			{
				return -1;
			}
			if (b.x < a.x)
			{
				return 1;
			}
			return 0;
		};
	}
}
