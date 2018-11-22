using UnityEngine;
using System.Collections;
using ICE;
using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Shared;
using UnityEngine.UI;

namespace ICE.Creatures.Demo
{
	public class CollectDemo : MonoBehaviour {

		public Text InventoryCount1;
		public Text InventoryCount2;

		public GameObject NPC1;

		private ICECreatureControl _controller_01 = null;

		//public GameObject NPC2;
		// Use this for initialization
		void Start () {
			
			if( NPC1 == null )
				return;
			
			_controller_01 = NPC1.GetComponent<ICECreatureControl>();
		}
		
		// Update is called once per frame
		void Update () {
		
			if( _controller_01 == null )
				return;
			
			InventoryCount1.text = _controller_01.Creature.Status.Inventory.SlotItemAmount( 0 ).ToString();
			InventoryCount2.text = _controller_01.Creature.Status.Inventory.SlotItemAmount( 1 ).ToString();
		}
	}
}
