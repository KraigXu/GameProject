using System;
using UnityEngine;

namespace Verse
{
	
	public class DesignationDef : Def
	{
		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		
		[NoTranslate]
		public string texturePath;

		
		public TargetType targetType;

		
		public bool removeIfBuildingDespawned;

		
		public bool designateCancelable = true;

		
		[Unsaved(false)]
		public Material iconMat;
	}
}
