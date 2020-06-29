using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class BoolGrid : IExposable
	{
		
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00036A80 File Offset: 0x00034C80
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00036A88 File Offset: 0x00034C88
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				if (this.trueCountInt == 0)
				{
					yield break;
				}
				int yieldedCount = 0;
				bool canSetMinPossibleTrueIndex = this.minPossibleTrueIndexDirty;
				int num = this.minPossibleTrueIndexDirty ? 0 : this.minPossibleTrueIndexCached;
				int num2;
				for (int i = num; i < this.arr.Length; i = num2 + 1)
				{
					if (this.arr[i])
					{
						if (canSetMinPossibleTrueIndex && this.minPossibleTrueIndexDirty)
						{
							canSetMinPossibleTrueIndex = false;
							this.minPossibleTrueIndexDirty = false;
							this.minPossibleTrueIndexCached = i;
						}
						yield return CellIndicesUtility.IndexToCell(i, this.mapSizeX);
						num2 = yieldedCount;
						yieldedCount = num2 + 1;
						if (yieldedCount >= this.trueCountInt)
						{
							yield break;
						}
					}
					num2 = i;
				}
				yield break;
			}
		}

		
		public bool this[int index]
		{
			get
			{
				return this.arr[index];
			}
			set
			{
				this.Set(index, value);
			}
		}

		
		public bool this[IntVec3 c]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		
		public bool this[int x, int z]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.Set(CellIndicesUtility.CellToIndex(x, z, this.mapSizeX), value);
			}
		}

		
		public BoolGrid()
		{
		}

		
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.arr != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.arr = new bool[this.mapSizeX * this.mapSizeZ];
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.minPossibleTrueIndexDirty = true;
			}
		}

		
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		
		public virtual void Set(int index, bool value)
		{
			if (this.arr[index] == value)
			{
				return;
			}
			this.arr[index] = value;
			if (value)
			{
				this.trueCountInt++;
				if (this.trueCountInt == 1 || index < this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexCached = index;
					return;
				}
			}
			else
			{
				this.trueCountInt--;
				if (index == this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexDirty = true;
				}
			}
		}

		
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
			this.minPossibleTrueIndexDirty = true;
		}

		
		private bool[] arr;

		
		private int trueCountInt;

		
		private int mapSizeX;

		
		private int mapSizeZ;

		
		private int minPossibleTrueIndexCached = -1;

		
		private bool minPossibleTrueIndexDirty;
	}
}
