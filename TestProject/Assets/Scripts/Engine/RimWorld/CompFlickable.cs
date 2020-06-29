using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompFlickable : ThingComp
	{
		
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x0014FD8B File Offset: 0x0014DF8B
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		
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

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{

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

		
		private bool switchOnInt = true;

		
		private bool wantSwitchOn = true;

		
		private Graphic offGraphic;

		
		private Texture2D cachedCommandTex;

		
		private const string OffGraphicSuffix = "_Off";

		
		public const string FlickedOnSignal = "FlickedOn";

		
		public const string FlickedOffSignal = "FlickedOff";
	}
}
