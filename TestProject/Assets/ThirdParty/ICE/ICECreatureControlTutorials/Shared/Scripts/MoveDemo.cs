using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using ICE;

using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Shared;

namespace ICE.Creatures.Demo
{
	public class MoveDemo : MonoBehaviour {

		public Slider MoveSegmentLength;
		public Slider MoveDeviationLength;
		public Slider MoveDeviationVariance;
		public Slider MoveSegmentVariance;
		public Slider MoveStopDistance;

		public Slider ForwardSpeed;
		public Slider TurnSpeed;

		public Text TextMoveSegmentLength;
		public Text TextMoveSegmentVariance;
		public Text TextMoveDeviationLength;
		public Text TextMoveDeviationVariance;
		public Text TextMoveStopDistance;
		
		public Text TextForwardSpeed;
		public Text TextTurnSpeed;


		private ICECreatureControl _controller_01 = null;
		private ICECreatureControlDebug _controller_debug_01 = null;

		public GameObject RandomPositioningRange;

		public GameObject NPC1;

		public void ExampleDefault()
		{
			MoveSegmentLength.value = 0;
			MoveSegmentVariance.value = 0;
			MoveDeviationLength.value = 0;
			MoveDeviationVariance.value = 0;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 7;
			TurnSpeed.value = 3;
		}

		public void ExampleStraightFast()
		{
			MoveSegmentLength.value = 0;
			MoveSegmentVariance.value = 0;
			MoveDeviationLength.value = 0;
			MoveDeviationVariance.value = 0;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 7;
			TurnSpeed.value = 25;
		}

		public void ExampleStraightSlow()
		{
			MoveSegmentLength.value = 0;
			MoveSegmentVariance.value = 0;
			MoveDeviationLength.value = 0;
			MoveDeviationVariance.value = 0;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 7;
			TurnSpeed.value = 1;

		}

		public void ExampleDrunkenSailor()
		{
			MoveSegmentLength.value = 2;
			MoveSegmentVariance.value = 0.8f;
			MoveDeviationLength.value = 3;
			MoveDeviationVariance.value = 1;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 1.5f;
			TurnSpeed.value = 2;
			
		}

		public void ExampleDrunkenSailorExtrem()
		{
			MoveSegmentLength.value = 2;
			MoveSegmentVariance.value = 0.8f;
			MoveDeviationLength.value = 8;
			MoveDeviationVariance.value = 0.9f;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 2.5f;
			TurnSpeed.value = 2;
			
		}

		public void ExampleRabbit()
		{
			MoveSegmentLength.value = 4;
			MoveSegmentVariance.value = 0.8f;
			MoveDeviationLength.value = 15;
			MoveDeviationVariance.value = 0.9f;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 1.5f;
			TurnSpeed.value = 1.5f;
			
		}

		public void ExampleRabbitEscape()
		{
			MoveSegmentLength.value = 3;
			MoveSegmentVariance.value = 0.8f;
			MoveDeviationLength.value = 10;
			MoveDeviationVariance.value = 0.9f;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 8;
			TurnSpeed.value = 25;
			
		}

		public void ExampleElephant()
		{
			MoveSegmentLength.value = 18;
			MoveSegmentVariance.value = 0.7f;
			MoveDeviationLength.value = 15;
			MoveDeviationVariance.value = 0.6f;
			MoveStopDistance.value = 2;
			ForwardSpeed.value = 1.5f;
			TurnSpeed.value = 0.5f;
			
		}
		
		public void ExampleElephantEscape()
		{
			MoveSegmentLength.value = 20;
			MoveSegmentVariance.value = 0.3f;
			MoveDeviationLength.value = 4;
			MoveDeviationVariance.value = 0.6f;
			MoveStopDistance.value = 4;
			ForwardSpeed.value = 6;
			TurnSpeed.value = 0.75f;
			
		}

		public void ExampleSnoopyDog()
		{
			MoveSegmentLength.value = 5;
			MoveSegmentVariance.value = 0.5f;
			MoveDeviationLength.value = 10;
			MoveDeviationVariance.value = 0.9f;
			MoveStopDistance.value = 1.5f;
			ForwardSpeed.value = 1.5f;
			TurnSpeed.value = 0.75f;
			
		}

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
			
			if( _controller_01 != null )
			{
				_controller_01.Creature.Move.DefaultMove.SegmentLength = MoveSegmentLength.value;
				_controller_01.Creature.Move.DefaultMove.SegmentVariance = MoveSegmentVariance.value;
				_controller_01.Creature.Move.DefaultMove.DeviationLength = MoveDeviationLength.value;
				_controller_01.Creature.Move.DefaultMove.DeviationVariance = MoveDeviationVariance.value;
				_controller_01.Creature.Move.DefaultMove.StoppingDistance = MoveStopDistance.value;

				_controller_01.Creature.Move.CurrentMove.SegmentLength = MoveSegmentLength.value;
				_controller_01.Creature.Move.CurrentMove.SegmentVariance = MoveSegmentVariance.value;
				_controller_01.Creature.Move.CurrentMove.DeviationLength = MoveDeviationLength.value;
				_controller_01.Creature.Move.CurrentMove.DeviationVariance = MoveDeviationVariance.value;
				_controller_01.Creature.Move.CurrentMove.StoppingDistance = MoveStopDistance.value;

				if( _controller_01.Creature.Move.CurrentBehaviourModeRule != null ) 
				{
					_controller_01.Creature.Move.CurrentBehaviourModeRule.Move.Motion.Velocity.z = ForwardSpeed.value;
					_controller_01.Creature.Move.CurrentBehaviourModeRule.Move.Motion.AngularVelocity.y = TurnSpeed.value;

					_controller_01.Creature.Move.CurrentMove.Motion.Velocity.z = ForwardSpeed.value;
					_controller_01.Creature.Move.CurrentMove.Motion.AngularVelocity.y = TurnSpeed.value;
				}

				TextMoveSegmentLength.text = MoveSegmentLength.value.ToString();
				TextMoveDeviationLength.text = MoveDeviationLength.value.ToString();
				TextMoveDeviationVariance.text = MoveDeviationVariance.value.ToString();
				TextMoveSegmentVariance.text = MoveSegmentVariance.value.ToString();
				TextMoveStopDistance.text = MoveStopDistance.value.ToString();			
				TextForwardSpeed.text = ForwardSpeed.value.ToString();
				TextTurnSpeed.text = TurnSpeed.value.ToString();



				/*
				_controller_01.Creature.Move.CurrentMove.MoveSegmentLength = MoveSegmentLenght.value;
				_controller_01.Creature.Move.CurrentMove.MoveSegmentVariance = MoveSegmentVariance.value;
				_controller_01.Creature.Move.CurrentMove.MoveLateralVariance = MoveLateralVariance.value;
				_controller_01.Creature.Move.CurrentMove.MoveStopDistance = MoveStopDistance.value;
				*/
			}

			if( _controller_debug_01 != null )
			{
				if( _controller_debug_01.CreatureDebug.MovePointer.Pointer != null )
				{
					float _stop_distance = _controller_01.Creature.Move.CurrentMove.StoppingDistance;
					_controller_debug_01.CreatureDebug.MovePointer.PointerSize = new Vector3( _stop_distance,0.25f,_stop_distance );
				}

				if( _controller_debug_01.CreatureDebug.TargetMovePositionPointer.Pointer != null )
				{
					float _stop_distance = _controller_01.Creature.ActiveTarget.Move.StoppingDistance;
					_controller_debug_01.CreatureDebug.TargetMovePositionPointer.PointerSize = new Vector3( _stop_distance,0.25f,_stop_distance );
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
				RandomPositioningRange.transform.localScale = new Vector3( _random_range*2,0.1f,_random_range*2 );



			}


		}
	}
}
