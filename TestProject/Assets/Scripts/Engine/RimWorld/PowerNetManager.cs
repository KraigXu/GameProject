using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A88 RID: 2696
	public class PowerNetManager
	{
		// Token: 0x06003FB8 RID: 16312 RVA: 0x00153231 File Offset: 0x00151431
		public PowerNetManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x00153256 File Offset: 0x00151456
		public List<PowerNet> AllNetsListForReading
		{
			get
			{
				return this.allNets;
			}
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x0015325E File Offset: 0x0015145E
		public void Notify_TransmitterSpawned(CompPower newTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, newTransmitter));
			this.NotifyDrawersForWireUpdate(newTransmitter.parent.Position);
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00153283 File Offset: 0x00151483
		public void Notify_TransmitterDespawned(CompPower oldTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, oldTransmitter));
			this.NotifyDrawersForWireUpdate(oldTransmitter.parent.Position);
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x001532A8 File Offset: 0x001514A8
		public void Notfiy_TransmitterTransmitsPowerNowChanged(CompPower transmitter)
		{
			if (!transmitter.parent.Spawned)
			{
				return;
			}
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, transmitter));
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, transmitter));
			this.NotifyDrawersForWireUpdate(transmitter.parent.Position);
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x001532F8 File Offset: 0x001514F8
		public void Notify_ConnectorWantsConnect(CompPower wantingCon)
		{
			if (Scribe.mode == LoadSaveMode.Inactive && !this.HasRegisterConnectorDuplicate(wantingCon))
			{
				this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterConnector, wantingCon));
			}
			this.NotifyDrawersForWireUpdate(wantingCon.parent.Position);
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0015332D File Offset: 0x0015152D
		public void Notify_ConnectorDespawned(CompPower oldCon)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterConnector, oldCon));
			this.NotifyDrawersForWireUpdate(oldCon.parent.Position);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00153352 File Offset: 0x00151552
		public void NotifyDrawersForWireUpdate(IntVec3 root)
		{
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.Things, true, false);
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.PowerGrid, true, false);
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00153380 File Offset: 0x00151580
		public void RegisterPowerNet(PowerNet newNet)
		{
			this.allNets.Add(newNet);
			newNet.powerNetManager = this;
			this.map.powerNetGrid.Notify_PowerNetCreated(newNet);
			PowerNetMaker.UpdateVisualLinkagesFor(newNet);
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x001533AC File Offset: 0x001515AC
		public void DeletePowerNet(PowerNet oldNet)
		{
			this.allNets.Remove(oldNet);
			this.map.powerNetGrid.Notify_PowerNetDeleted(oldNet);
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x001533CC File Offset: 0x001515CC
		public void PowerNetsTick()
		{
			for (int i = 0; i < this.allNets.Count; i++)
			{
				this.allNets[i].PowerNetTick();
			}
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00153400 File Offset: 0x00151600
		public void UpdatePowerNetsAndConnections_First()
		{
			int count = this.delayedActions.Count;
			int i = 0;
			while (i < count)
			{
				PowerNetManager.DelayedAction delayedAction = this.delayedActions[i];
				PowerNetManager.DelayedActionType type = this.delayedActions[i].type;
				if (type != PowerNetManager.DelayedActionType.RegisterTransmitter)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
					{
						goto IL_107;
					}
				}
				else if (delayedAction.position == delayedAction.compPower.parent.Position)
				{
					ThingWithComps parent = delayedAction.compPower.parent;
					if (this.map.powerNetGrid.TransmittedPowerNetAt(parent.Position) != null)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Tried to register trasmitter ",
							parent,
							" at ",
							parent.Position,
							", but there is already a power net here. There can't be two transmitters on the same cell."
						}), false);
					}
					delayedAction.compPower.SetUpPowerVars();
					using (IEnumerator<IntVec3> enumerator = GenAdj.CellsAdjacentCardinal(parent).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							this.TryDestroyNetAt(cell);
						}
						goto IL_12F;
					}
					goto IL_107;
				}
				IL_12F:
				i++;
				continue;
				IL_107:
				this.TryDestroyNetAt(delayedAction.position);
				PowerConnectionMaker.DisconnectAllFromTransmitterAndSetWantConnect(delayedAction.compPower, this.map);
				delayedAction.compPower.ResetPowerVars();
				goto IL_12F;
			}
			for (int j = 0; j < count; j++)
			{
				PowerNetManager.DelayedAction delayedAction2 = this.delayedActions[j];
				if ((delayedAction2.type == PowerNetManager.DelayedActionType.RegisterTransmitter && delayedAction2.position == delayedAction2.compPower.parent.Position) || delayedAction2.type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
				{
					this.TryCreateNetAt(delayedAction2.position);
					foreach (IntVec3 cell2 in GenAdj.CellsAdjacentCardinal(delayedAction2.position, delayedAction2.rotation, delayedAction2.compPower.parent.def.size))
					{
						this.TryCreateNetAt(cell2);
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				PowerNetManager.DelayedAction delayedAction3 = this.delayedActions[k];
				PowerNetManager.DelayedActionType type = this.delayedActions[k].type;
				if (type != PowerNetManager.DelayedActionType.RegisterConnector)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						PowerConnectionMaker.DisconnectFromPowerNet(delayedAction3.compPower);
						delayedAction3.compPower.ResetPowerVars();
					}
				}
				else if (delayedAction3.position == delayedAction3.compPower.parent.Position)
				{
					delayedAction3.compPower.SetUpPowerVars();
					PowerConnectionMaker.TryConnectToAnyPowerNet(delayedAction3.compPower, null);
				}
			}
			this.delayedActions.RemoveRange(0, count);
			if (DebugViewSettings.drawPower)
			{
				this.DrawDebugPowerNets();
			}
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x001536D0 File Offset: 0x001518D0
		private bool HasRegisterConnectorDuplicate(CompPower compPower)
		{
			for (int i = this.delayedActions.Count - 1; i >= 0; i--)
			{
				if (this.delayedActions[i].compPower == compPower)
				{
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						return false;
					}
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.RegisterConnector)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00153738 File Offset: 0x00151938
		private void TryCreateNetAt(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return;
			}
			if (this.map.powerNetGrid.TransmittedPowerNetAt(cell) == null)
			{
				Building transmitter = cell.GetTransmitter(this.map);
				if (transmitter != null && transmitter.TransmitsPowerNow)
				{
					PowerNet powerNet = PowerNetMaker.NewPowerNetStartingFrom(transmitter);
					this.RegisterPowerNet(powerNet);
					for (int i = 0; i < powerNet.transmitters.Count; i++)
					{
						PowerConnectionMaker.ConnectAllConnectorsToTransmitter(powerNet.transmitters[i]);
					}
				}
			}
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x001537B4 File Offset: 0x001519B4
		private void TryDestroyNetAt(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return;
			}
			PowerNet powerNet = this.map.powerNetGrid.TransmittedPowerNetAt(cell);
			if (powerNet != null)
			{
				this.DeletePowerNet(powerNet);
			}
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x001537EC File Offset: 0x001519EC
		private void DrawDebugPowerNets()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (Find.CurrentMap != this.map)
			{
				return;
			}
			int num = 0;
			foreach (PowerNet powerNet in this.allNets)
			{
				foreach (CompPower compPower in powerNet.transmitters.Concat(powerNet.connectors))
				{
					foreach (IntVec3 c in GenAdj.CellsOccupiedBy(compPower.parent))
					{
						CellRenderer.RenderCell(c, (float)num * 0.44f);
					}
				}
				num++;
			}
		}

		// Token: 0x0400251F RID: 9503
		public Map map;

		// Token: 0x04002520 RID: 9504
		private List<PowerNet> allNets = new List<PowerNet>();

		// Token: 0x04002521 RID: 9505
		private List<PowerNetManager.DelayedAction> delayedActions = new List<PowerNetManager.DelayedAction>();

		// Token: 0x02001A6F RID: 6767
		private enum DelayedActionType
		{
			// Token: 0x0400646D RID: 25709
			RegisterTransmitter,
			// Token: 0x0400646E RID: 25710
			DeregisterTransmitter,
			// Token: 0x0400646F RID: 25711
			RegisterConnector,
			// Token: 0x04006470 RID: 25712
			DeregisterConnector
		}

		// Token: 0x02001A70 RID: 6768
		private struct DelayedAction
		{
			// Token: 0x0600976B RID: 38763 RVA: 0x002EAF0F File Offset: 0x002E910F
			public DelayedAction(PowerNetManager.DelayedActionType type, CompPower compPower)
			{
				this.type = type;
				this.compPower = compPower;
				this.position = compPower.parent.Position;
				this.rotation = compPower.parent.Rotation;
			}

			// Token: 0x04006471 RID: 25713
			public PowerNetManager.DelayedActionType type;

			// Token: 0x04006472 RID: 25714
			public CompPower compPower;

			// Token: 0x04006473 RID: 25715
			public IntVec3 position;

			// Token: 0x04006474 RID: 25716
			public Rot4 rotation;
		}
	}
}
