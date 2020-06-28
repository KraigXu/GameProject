﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D07 RID: 3335
	public class CompFacility : ThingComp
	{
		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x0600511F RID: 20767 RVA: 0x001B38F8 File Offset: 0x001B1AF8
		public bool CanBeActive
		{
			get
			{
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				return compPowerTrader == null || compPowerTrader.PowerOn;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06005120 RID: 20768 RVA: 0x001B391F File Offset: 0x001B1B1F
		public CompProperties_Facility Props
		{
			get
			{
				return (CompProperties_Facility)this.props;
			}
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x001B392C File Offset: 0x001B1B2C
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						GenDraw.DrawLineBetween(a, thing.TrueCenter());
						compAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility(myDef, myPos, myRot);
					}
				}
			}
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x001B39F0 File Offset: 0x001B1BF0
		public void Notify_NewLink(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					Log.Error("Notify_NewLink was called but the link is already here.", false);
					return;
				}
			}
			this.linkedBuildings.Add(thing);
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x001B3A3C File Offset: 0x001B1C3C
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					this.linkedBuildings.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.", false);
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x001B3A86 File Offset: 0x001B1C86
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x001B3A86 File Offset: 0x001B1C86
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x001B3A8E File Offset: 0x001B1C8E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x001B3A98 File Offset: 0x001B1C98
		public override void PostDeSpawn(Map map)
		{
			this.thingsToNotify.Clear();
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.thingsToNotify.Add(this.linkedBuildings[i]);
			}
			this.UnlinkAll();
			foreach (Thing thing in this.thingsToNotify)
			{
				thing.TryGetComp<CompAffectedByFacilities>().Notify_FacilityDespawned();
			}
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001B3B2C File Offset: 0x001B1D2C
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
				}
			}
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001B3BB8 File Offset: 0x001B1DB8
		public override string CompInspectStringExtra()
		{
			CompProperties_Facility props = this.Props;
			if (props.statOffsets == null)
			{
				return null;
			}
			bool flag = this.AmIActiveForAnyone();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < props.statOffsets.Count; i++)
			{
				StatModifier statModifier = props.statOffsets[i];
				StatDef stat = statModifier.stat;
				stringBuilder.Append(stat.LabelCap);
				stringBuilder.Append(": ");
				stringBuilder.Append(statModifier.value.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
				if (!flag)
				{
					stringBuilder.Append(" (");
					stringBuilder.Append("InactiveFacility".Translate());
					stringBuilder.Append(")");
				}
				if (i < props.statOffsets.Count - 1)
				{
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001B3A8E File Offset: 0x001B1C8E
		private void RelinkAll()
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001B3CA0 File Offset: 0x001B1EA0
		private void LinkToNearbyBuildings()
		{
			this.UnlinkAll();
			CompProperties_Facility props = this.Props;
			if (props.linkableBuildings == null)
			{
				return;
			}
			for (int i = 0; i < props.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(props.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanLinkTo(this.parent))
					{
						this.linkedBuildings.Add(thing);
						compAffectedByFacilities.Notify_NewLink(this.parent);
					}
				}
			}
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001B3D70 File Offset: 0x001B1F70
		private bool AmIActiveForAnyone()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x001B3DB4 File Offset: 0x001B1FB4
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().Notify_LinkRemoved(this.parent);
			}
			this.linkedBuildings.Clear();
		}

		// Token: 0x04002CF9 RID: 11513
		private List<Thing> linkedBuildings = new List<Thing>();

		// Token: 0x04002CFA RID: 11514
		private HashSet<Thing> thingsToNotify = new HashSet<Thing>();
	}
}
