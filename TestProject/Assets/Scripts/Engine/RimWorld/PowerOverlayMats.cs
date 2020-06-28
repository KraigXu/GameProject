using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A84 RID: 2692
	[StaticConstructorOnStartup]
	public static class PowerOverlayMats
	{
		// Token: 0x06003FAA RID: 16298 RVA: 0x00152A68 File Offset: 0x00150C68
		static PowerOverlayMats()
		{
			Graphic graphic = GraphicDatabase.Get<Graphic_Single>("Things/Special/Power/TransmitterAtlas", PowerOverlayMats.TransmitterShader);
			PowerOverlayMats.LinkedOverlayGraphic = GraphicUtility.WrapLinked(graphic, LinkDrawerType.TransmitterOverlay);
			graphic.MatSingle.renderQueue = 3600;
			PowerOverlayMats.MatConnectorBase.renderQueue = 3600;
			PowerOverlayMats.MatConnectorLine.renderQueue = 3600;
		}

		// Token: 0x04002510 RID: 9488
		private const string TransmitterAtlasPath = "Things/Special/Power/TransmitterAtlas";

		// Token: 0x04002511 RID: 9489
		private static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;

		// Token: 0x04002512 RID: 9490
		public static readonly Graphic LinkedOverlayGraphic;

		// Token: 0x04002513 RID: 9491
		public static readonly Material MatConnectorBase = MaterialPool.MatFrom("Things/Special/Power/OverlayBase", ShaderDatabase.MetaOverlay);

		// Token: 0x04002514 RID: 9492
		public static readonly Material MatConnectorLine = MaterialPool.MatFrom("Things/Special/Power/OverlayWire", ShaderDatabase.MetaOverlay);

		// Token: 0x04002515 RID: 9493
		public static readonly Material MatConnectorAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayWireAnticipated", ShaderDatabase.MetaOverlay);

		// Token: 0x04002516 RID: 9494
		public static readonly Material MatConnectorBaseAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayBaseAnticipated", ShaderDatabase.MetaOverlay);
	}
}
