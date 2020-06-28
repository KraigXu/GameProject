using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A76 RID: 2678
	public class CompFlickable : ThingComp
	{
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x0014FD8B File Offset: 0x0014DF8B
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x0014FD98 File Offset: 0x0014DF98
		private Texture2D CommandTex
		{
			get
			{
				if (this.cachedCommandTex == null)
				{
					this.cachedCommandTex = ContentFinder<Texture2D>.Get(this.Props.commandTexture, true);
				}
				return this.cachedCommandTex;
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x0014FDC5 File Offset: 0x0014DFC5
		// (set) Token: 0x06003F24 RID: 16164 RVA: 0x0014FDD0 File Offset: 0x0014DFD0
		public bool SwitchIsOn
		{
			get
			{
				return this.switchOnInt;
			}
			set
			{
				if (this.switchOnInt == value)
				{
					return;
				}
				this.switchOnInt = value;
				if (this.switchOnInt)
				{
					this.parent.BroadcastCompSignal("FlickedOn");
				}
				else
				{
					this.parent.BroadcastCompSignal("FlickedOff");
				}
				if (this.parent.Spawned)
				{
					this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
				}
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x0014FE48 File Offset: 0x0014E048
		public Graphic CurrentGraphic
		{
			get
			{
				if (this.SwitchIsOn)
				{
					return this.parent.DefaultGraphic;
				}
				if (this.offGraphic == null)
				{
					this.offGraphic = GraphicDatabase.Get(this.parent.def.graphicData.graphicClass, this.parent.def.graphicData.texPath + "_Off", this.parent.def.graphicData.shaderType.Shader, this.parent.def.graphicData.drawSize, this.parent.DrawColor, this.parent.DrawColorTwo);
				}
				return this.offGraphic;
			}
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0014FEFE File Offset: 0x0014E0FE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x0014FF2A File Offset: 0x0014E12A
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0014FF3D File Offset: 0x0014E13D
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0014FF79 File Offset: 0x0014E179
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0014FF89 File Offset: 0x0014E189
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_TogglePower,
					icon = this.CommandTex,
					defaultLabel = this.Props.commandLabelKey.Translate(),
					defaultDesc = this.Props.commandDescKey.Translate(),
					isActive = (() => this.wantSwitchOn),
					toggleAction = delegate
					{
						this.wantSwitchOn = !this.wantSwitchOn;
						FlickUtility.UpdateFlickDesignation(this.parent);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x040024BB RID: 9403
		private bool switchOnInt = true;

		// Token: 0x040024BC RID: 9404
		private bool wantSwitchOn = true;

		// Token: 0x040024BD RID: 9405
		private Graphic offGraphic;

		// Token: 0x040024BE RID: 9406
		private Texture2D cachedCommandTex;

		// Token: 0x040024BF RID: 9407
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x040024C0 RID: 9408
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x040024C1 RID: 9409
		public const string FlickedOffSignal = "FlickedOff";
	}
}
