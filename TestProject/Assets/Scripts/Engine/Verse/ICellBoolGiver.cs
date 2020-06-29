using System;
using UnityEngine;

namespace Verse
{
	
	public interface ICellBoolGiver
	{
		
		// (get) Token: 0x0600097E RID: 2430
		Color Color { get; }

		
		bool GetCellBool(int index);

		
		Color GetCellExtraColor(int index);
	}
}
