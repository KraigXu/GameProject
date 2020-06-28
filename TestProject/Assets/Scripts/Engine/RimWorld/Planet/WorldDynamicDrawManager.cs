using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E4 RID: 4580
	public class WorldDynamicDrawManager
	{
		// Token: 0x060069FE RID: 27134 RVA: 0x0024FF7F File Offset: 0x0024E17F
		public void RegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + o + " while drawing is in progress. WorldObjects shouldn't be spawned in Draw methods.", false);
				}
				this.drawObjects.Add(o);
			}
		}

		// Token: 0x060069FF RID: 27135 RVA: 0x0024FFB9 File Offset: 0x0024E1B9
		public void DeRegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + o + " while drawing is in progress. WorldObjects shouldn't be despawned in Draw methods.", false);
				}
				this.drawObjects.Remove(o);
			}
		}

		// Token: 0x06006A00 RID: 27136 RVA: 0x0024FFF4 File Offset: 0x0024E1F4
		public void DrawDynamicWorldObjects()
		{
			this.drawingNow = true;
			try
			{
				foreach (WorldObject worldObject in this.drawObjects)
				{
					try
					{
						if (!worldObject.def.expandingIcon || ExpandableWorldObjectsUtility.TransitionPct < 1f)
						{
							worldObject.Draw();
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception drawing ",
							worldObject,
							": ",
							ex
						}), false);
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic world objects: " + arg, false);
			}
			this.drawingNow = false;
		}

		// Token: 0x04004212 RID: 16914
		private HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x04004213 RID: 16915
		private bool drawingNow;
	}
}
