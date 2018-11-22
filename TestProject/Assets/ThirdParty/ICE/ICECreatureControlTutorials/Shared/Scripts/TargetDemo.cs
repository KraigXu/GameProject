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
	public class TargetDemo : MonoBehaviour {


		public Slider TargetRandomRange;
		public Slider OffsetDistance;
		public Slider OffsetAngle;
		public Slider TargetStopDistance;
		public Slider TargetSmoothingMultiplier;

		public Toggle UseDynamicOffsetDistance;
		public Toggle UseRandomOffsetDistance;
		public Toggle UseDynamicOffsetAngle;
		public Toggle UseRandomOffsetAngle;

		public Toggle UpdateOnActivate;
		public Toggle UpdateOnReached;
		public Toggle TargetStopDistanceBan;

		public Toggle UseNPC2AsTarget;

		public Slider ForwardSpeed;
		public Slider TurnSpeed;

		public Text TextTargetRandomRange;
		public Text TextOffsetDistance;
		public Text TextOffsetAngle;
		public Text TextTargetStopDistance;
		public Text TextTargetSmoothingMultiplier;
		
		public Text TextForwardSpeed;
		public Text TextTurnSpeed;


		private ICECreatureControl _controller_01 = null;
		private ICECreatureControlDebug _controller_debug_01 = null;

		public GameObject RandomPositioningRange;
		public GameObject TargetOffsetObject;
		public GameObject TargetObject;

		public GameObject NPC1;
		public GameObject NPC2;


		// Use this for initialization
		void Start () {
		
			if( NPC1 != null )
			{
				_controller_01 = NPC1.GetComponent<ICECreatureControl>();
				_controller_debug_01 = NPC1.GetComponent<ICECreatureControlDebug>();
			}

		}


		// Update is called once per frame
		void Update () {
		
			if( _controller_01 == null || _controller_01.Creature.ActiveTarget == null )
				return;
			
		
				_controller_01.Creature.ActiveTarget.Move.UseDynamicOffsetDistance = UseDynamicOffsetDistance.isOn;
				_controller_01.Creature.ActiveTarget.Move.UseRandomOffsetDistance = UseRandomOffsetDistance.isOn;
				_controller_01.Creature.ActiveTarget.Move.UseDynamicOffsetAngle = UseDynamicOffsetAngle.isOn;
				_controller_01.Creature.ActiveTarget.Move.UseRandomOffsetAngle = UseRandomOffsetAngle.isOn;

				if( _controller_01.Creature.ActiveTarget.Move.UseDynamicOffsetDistance || _controller_01.Creature.ActiveTarget.Move.UseRandomOffsetDistance )
					OffsetDistance.value = _controller_01.Creature.ActiveTarget.Move.OffsetDistance;

				if( _controller_01.Creature.ActiveTarget.Move.UseDynamicOffsetAngle || _controller_01.Creature.ActiveTarget.Move.UseRandomOffsetAngle )
					OffsetAngle.value = _controller_01.Creature.ActiveTarget.Move.OffsetAngle;

				_controller_01.Creature.ActiveTarget.Move.RandomRange = TargetRandomRange.value;

				_controller_01.Creature.ActiveTarget.Move.UseUpdateOffsetOnActivateTarget = UpdateOnActivate.isOn;
				_controller_01.Creature.ActiveTarget.Move.UseUpdateOffsetOnMovePositionReached = UpdateOnReached.isOn;
				if( TargetStopDistanceBan != null ) _controller_01.Creature.ActiveTarget.Move.StoppingDistanceZoneRestricted = TargetStopDistanceBan.isOn;

				_controller_01.Creature.ActiveTarget.Move.StoppingDistance = TargetStopDistance.value;
				if( TargetSmoothingMultiplier != null ) _controller_01.Creature.ActiveTarget.Move.SmoothingSpeed = TargetSmoothingMultiplier.value;
				_controller_01.Creature.ActiveTarget.Move.UpdateOffset( OffsetAngle.value , OffsetDistance.value );

				if( _controller_01.Creature.Move.CurrentBehaviourModeRule != null ) 
				{
					_controller_01.Creature.Move.CurrentBehaviourModeRule.Move.Motion.Velocity.z = ForwardSpeed.value;
					_controller_01.Creature.Move.CurrentBehaviourModeRule.Move.Motion.AngularVelocity.y = TurnSpeed.value;

					_controller_01.Creature.Move.CurrentMove.Motion.Velocity.z = ForwardSpeed.value;
					_controller_01.Creature.Move.CurrentMove.Motion.AngularVelocity.y = TurnSpeed.value;
				}

				TextTargetRandomRange.text = TargetRandomRange.value.ToString();
				TextTargetStopDistance.text = TargetStopDistance.value.ToString();
				if( TargetSmoothingMultiplier != null ) TextTargetSmoothingMultiplier.text = TargetSmoothingMultiplier.value.ToString();
				TextOffsetAngle.text = ((int)OffsetAngle.value ).ToString() + "°";
				TextOffsetDistance.text = ((int)OffsetDistance.value ).ToString() + "m";			
				TextForwardSpeed.text = ForwardSpeed.value.ToString();
				TextTurnSpeed.text = TurnSpeed.value.ToString();



				/*
				_controller_01.Creature.Move.CurrentMove.MoveSegmentLength = MoveSegmentLenght.value;
				_controller_01.Creature.Move.CurrentMove.MoveSegmentVariance = MoveSegmentVariance.value;
				_controller_01.Creature.Move.CurrentMove.MoveLateralVariance = MoveLateralVariance.value;
				_controller_01.Creature.Move.CurrentMove.MoveStopDistance = MoveStopDistance.value;
				*/

			if( _controller_debug_01 != null )
			{
				if( _controller_debug_01.CreatureDebug.MovePointer.Pointer != null )
				{
					float _stop_distance = _controller_01.Creature.Move.CurrentMove.StoppingDistance;
					_controller_debug_01.CreatureDebug.MovePointer.PointerSize = new Vector3( _stop_distance,0.025f,_stop_distance );
				}

				if( _controller_debug_01.CreatureDebug.TargetMovePositionPointer.Pointer != null )
				{
					float _stop_distance = _controller_01.Creature.ActiveTarget.Move.StoppingDistance;
					_controller_debug_01.CreatureDebug.TargetMovePositionPointer.PointerSize = new Vector3( _stop_distance,0.025f,_stop_distance );
				}

				if( _controller_debug_01.CreatureDebug.DesiredTargetMovePositionPointer.Pointer != null )
				{
					float _stop_distance = _controller_01.Creature.ActiveTarget.Move.StoppingDistance;
					_controller_debug_01.CreatureDebug.DesiredTargetMovePositionPointer.PointerSize = new Vector3( _stop_distance,0.025f,_stop_distance );
				}
			}

			if( RandomPositioningRange != null )
			{
				float _random_range = _controller_01.Creature.ActiveTarget.Move.RandomRange;
				RandomPositioningRange.transform.position = _controller_01.Creature.ActiveTarget.TargetOffsetPosition;
				RandomPositioningRange.transform.localScale = new Vector3( _random_range*2,0.1f,_random_range*2 );

				if( TargetOffsetObject != null )
					TargetOffsetObject.transform.position = _controller_01.Creature.ActiveTarget.TargetOffsetPosition; 

			}

			if( UseNPC2AsTarget != null )
			{
				if( _controller_01.Creature.Essentials.Target.AccessType == TargetAccessType.OBJECT )
				{
					if( UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != NPC2 )
						_controller_01.Creature.Essentials.Target.SetTargetByGameObject( NPC2 );
					else if( ! UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != TargetObject )
						_controller_01.Creature.Essentials.Target.SetTargetByGameObject( TargetObject );
				}
				else if( _controller_01.Creature.Essentials.Target.AccessType == TargetAccessType.NAME )
				{
					if( UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != NPC2 )
						_controller_01.Creature.Essentials.Target.SetTargetByName( NPC2.name, _controller_01.gameObject );
					else if( ! UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != TargetObject )
						_controller_01.Creature.Essentials.Target.SetTargetByName( TargetObject.name, _controller_01.gameObject );
				}
				/*
				else if( _controller_01.Creature.Essentials.Target.AccessType == TargetAccessType.TYPE )
				{
					if( UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != NPC2 )
						_controller_01.Creature.Essentials.Target.SetTargetByName( NPC2.name, _controller_01.gameObject );
					else if( ! UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != TargetObject )
						_controller_01.Creature.Essentials.Target.SetTargetByName( TargetObject.name, _controller_01.gameObject );
				}*/
				else
				{
					if( UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != NPC2 )
						_controller_01.Creature.Essentials.Target.SetTargetByTag( NPC2.tag, _controller_01.gameObject );
					else if( ! UseNPC2AsTarget.isOn && _controller_01.Creature.Essentials.Target.TargetGameObject != TargetObject )
						_controller_01.Creature.Essentials.Target.SetTargetByTag( TargetObject.tag, _controller_01.gameObject );
				}
			}
		}
	}
}