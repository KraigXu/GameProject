using System;
using UnityEngine;

namespace Verse
{
	
	public static class MatsFromSpectrum
	{
		
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}
