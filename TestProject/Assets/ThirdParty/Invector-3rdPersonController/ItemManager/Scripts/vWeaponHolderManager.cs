using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using System;

namespace Invector.vItemManager
{
    [vClassHeader("Weapon Holder Manager", "Create a new empty object inside a bone and add the vWeaponHolder script")]
    public class vWeaponHolderManager : vMonoBehaviour
    {
        public vWeaponHolder[] holders = new vWeaponHolder[0];
        internal bool inEquip;
        internal bool inUnequip;
        internal vItemManager itemManager;
        internal vThirdPersonController cc;
        public Dictionary<string, List<vWeaponHolder>> holderAreas = new Dictionary<string, List<vWeaponHolder>>();
        protected float equipTime;
        private float currentUnsheatheTimer;

        void OnDrawGizmosSelected()
        {
            holders = GetComponentsInChildren<vWeaponHolder>(true);
        }
        float timeOut;

        protected virtual bool IsEquipping
        {
            get
            {
                if (cc) return cc.IsAnimatorTag("IsEquipping");
                return false;
            }
        }

        void Start()
        {
            itemManager = GetComponent<vItemManager>();
            cc = GetComponent<vThirdPersonController>();
            if (itemManager)
            {
                itemManager.onEquipItem.AddListener(EquipWeapon);
                itemManager.onUnequipItem.AddListener(UnequipWeapon);

                holders = GetComponentsInChildren<vWeaponHolder>(true);
                if (holders != null)
                {
                    foreach (vWeaponHolder holder in holders)
                    {
                        if (!holderAreas.ContainsKey(holder.equipPointName))
                        {
                            holderAreas.Add(holder.equipPointName, new List<vWeaponHolder>());
                            holderAreas[holder.equipPointName].Add(holder);
                        }
                        else
                        {
                            holderAreas[holder.equipPointName].Add(holder);
                        }

                        holder.SetActiveHolder(false);
                        holder.SetActiveWeapon(false);
                    }
                }
            }
        }

        public void EquipWeapon(vEquipArea equipArea, vItem item)
        {
            var slotsInArea = equipArea.ValidSlots;

            if (slotsInArea != null && slotsInArea.Count > 0 && holderAreas.ContainsKey(equipArea.equipPointName))
            {
                // Check All Holders to Display
                for (int i = 0; i < slotsInArea.Count; i++)
                {
                    if (slotsInArea[i].item != null)
                    {
                        var holder = holderAreas[equipArea.equipPointName].Find(h => slotsInArea[i].item && slotsInArea[i].item.id == h.itemID
                        && ((equipArea.currentEquipedItem
                        && equipArea.currentEquipedItem != item
                        && equipArea.currentEquipedItem != slotsInArea[i].item
                        && equipArea.currentEquipedItem.id != item.id) || !equipArea.currentEquipedItem));

                        if (holder)
                        {
                            holder.SetActiveHolder(true);
                            holder.SetActiveWeapon(true);
                        }
                    }
                }
                // Check Current Item to Equip with time
                if (equipArea.currentEquipedItem != null)
                {
                    var holder = holderAreas[equipArea.equipPointName].Find(h => h.itemID == equipArea.currentEquipedItem.id);
                    if (holder)
                    {
                        // Unhide Holder and hide Equiped weapon
                        StartCoroutine(EquipRoutine(equipArea.currentEquipedItem.equipDelayTime, (itemManager.inventory != null && itemManager.inventory.isOpen),
                           () => { holder.SetActiveHolder(true); }, () => { holder.SetActiveWeapon(false); }));
                    }
                }
            }
        }

        public void UnequipWeapon(vEquipArea equipArea, vItem item)
        {
            if (holders.Length == 0 || item == null) return;

            if ((itemManager.inventory != null) && holderAreas.ContainsKey(equipArea.equipPointName))
            {
                var holder = holderAreas[equipArea.equipPointName].Find(h => item.id == h.itemID);
                if (holder)
                {
                    // Check if EquipArea contains unequipped item
                    var containsItem = equipArea.ValidSlots.Find(slot => slot.item == item) != null;
                    // Hide or unhide holder and weapon if contains item
                    StartCoroutine(UnequipRoutine(item.unequipDelayTime, (itemManager.inventory != null && itemManager.inventory.isOpen),
                           () => { holder.SetActiveHolder(containsItem); }, () => { holder.SetActiveWeapon(containsItem); }));
                }
            }
        }

        internal vWeaponHolder GetHolder(GameObject equipment, int id)
        {
            var equipPoint = itemManager.equipPoints.Find(e => e.equipmentReference != null
                                                          && e.equipmentReference.item && e.equipmentReference.item.id == id
                                                          && e.equipmentReference.equipedObject == equipment);
            var holder = holderAreas[equipPoint.equipPointName].Find(h => id == h.itemID);
            return holder;
        }

        internal IEnumerator UnequipRoutine(float equipDelay, bool immediat = false, UnityEngine.Events.UnityAction onStart = null, UnityEngine.Events.UnityAction onFinish = null)
        {
            if (!immediat) inUnequip = true;
            var time = Time.time;
            while ((!IsEquipping || cc.upperBodyInfo.normalizedTime >= equipDelay) && !immediat && timeOut < 1f)
            {
                timeOut += Time.deltaTime;
                yield return null;
            }
            timeOut = 0;
            if (onStart != null) onStart.Invoke();
            if (!inEquip && !immediat) // ignore time if inEquip or immediate unequip
            {
                equipTime = equipDelay;
                while (!immediat && cc.upperBodyInfo.normalizedTime < equipDelay && !inEquip)
                {
                    yield return null;
                }
            }
            inUnequip = false;
            if (onFinish != null) onFinish.Invoke();
        }

        internal IEnumerator EquipRoutine(float equipDelay, bool immediat = false, UnityEngine.Events.UnityAction onStart = null, UnityEngine.Events.UnityAction onFinish = null)
        {
            if (!immediat)
                inEquip = true;
            while ((!IsEquipping || cc.upperBodyInfo.normalizedTime >= equipDelay) && !immediat && timeOut < 1f)
            {
                timeOut += Time.deltaTime;
                yield return null;
            }
            timeOut = 0;
            if (onStart != null) onStart.Invoke();
            while (!immediat && cc.upperBodyInfo.normalizedTime < equipDelay) // ignore time if  immediate equip
            {
                yield return null;
            }
            inEquip = false;
            if (onFinish != null) onFinish.Invoke();
        }
    }
}
