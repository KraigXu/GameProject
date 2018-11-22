// ##############################################################################
//
// ice_CreatureEditorText.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.EditorInfos
{

	public class Info : ICE.World.EditorInfos.Info
	{
		public new static string Version = "1.4.0";

		// ################################################################################
		// TARGET
		// ################################################################################
		public static readonly string TARGET = "";

		public static readonly string TARGET_OBJECT = "Targets represents potential destinations and interaction objects and contains as fundamental elements " +
			"all relevant information about motion and behaviour of your creature. Please note that the behaviour of your creature is target-driven, therefore " +
			"it is fundamental that your creature have at least one reachable target.A Target Object can be each static or movable GameObject in your scene or " +
			"a Prefab as well. The only requirement is here that the given position should be reachable for your creature and please consider also the typical " +
			"characteristics of scene objects and prefabs (e.g. nested prefabs etc.)";
		public static readonly string TARGET_PRESELECTION_REFRESH_INTERVAL = "The TargetRefreshInterval defines a time delay in seconds, in which the creature looks for " +
			"matching target game objects. If the TargetRefreshInterval is disabled or the time span is adjusted to zero, the creature will " +
			"search and replace appropriate target game objects according to the given perception time. Please note: especially if a creature has to handle " +
			"many targets of the same type, it is recommended to use the TargetRefreshInterval to increase the performance.";
		public static readonly string TARGET_PRESELECTION_USE_CHILD_OBJECTS = "By default, potential target objects within the creature's hierarchy will be ignored, but in " +
			"some cases (e.g. for weapons, tools or other useable objectes), it might be useful to allow such subordinated child objects as a target, so you can " +
			"enable this feature by activating the 'Allow Child Objects' option.";
		public static readonly string TARGET_PRESELECTION_USE_ALL_AVAILABLE_OBJECTS = "By default, a creature always uses just the next best GameObject of a specified type " +
			"as TargetGameObject for a defined target and will run the final target selection with the advanced conditions for all available target groups just with such " +
			"pre-selected objects. In cases where multiple GameObjects of the same type are available, the Advanced Target Selection Criteria will check object-related " +
			"conditions only for the above-mentioned preselected GameObject, which may lead to confusing and misleading behavior since other potential target candidates are " +
			"apparently ignored. In such cases you can activate 'Use All Available Objects' to consider all available GameObjects of the given type during the final target " +
			"selection process. Please note that this feature is not required whenever the specified TargetGameObject will be the only one of its type or if there are no " +
			"object-related conditions defined in the Target Selection Criteria, such as distance, direction or other individual attributes.";
		public static readonly string TARGET_PRESELECTION_ACTIVE_COUNTERPARTS_LIMIT = "By default, a creature always selects the next best target, which can cause that too many " +
			"creatures are focused on the same object while ignoring surrounding target-options. By activating and adapting the 'Active Counterpart Limit' the " +
			"creature takes into account how many other objects have currently selected the target and will only select it if the number of the active counterparts " +
			"is below the specified limit. If the limit is adjusted to -1 this feature will be ignored. This feature works similar to the ActiveCounterpartsLimit " +
			"expression of the advanced conditions.";
		public static readonly string TARGET_PREFER_ACTIVE_COUNTERPARTS = "By default, all available objects of the given type are taken into account in the target preselection, " +
			"but in some cases, it may be helpful to favor specific objects which have already selected our creature as their own target, so our creature can react " +
			"to objects, which have already a clear focus to our creature (e.g. an active attacker or something like this). By activating the 'Prefer Active Counterparts' " +
			"option, the target selection procedure will favor all objects that have been selected our creature as their own active target.";
		public static readonly string TARGET_SELECTION_CRITERIA = "Your creature could have several targets at the same time in such cases it can use a set of selection criteria to " +
			"evaluate the most suitable target related to the given situation. Here you can define the priority and relevance of a target.";

		public static readonly string TARGET_SELECTION_CRITERIA_HOME = "* Please consider, the HOME target should normally have the lowest priority, because it should be rather a " +
			"side show than the main event, a secluded place where your creature can spawn or become modified invisible to the player.";

		public static readonly string TARGET_SELECTION_CRITERIA_ADVANCED = "The advanced Target Selection Criteria provide you to define multiple selectors with customized " +
			"conditions";

		public static readonly string TARGET_SELECTION_CRITERIA_PRIORITY = "In cases that your creature will have several valid targets at the same time, the priority " +
			"determines the relevance of the targets and the creature will select the target with the highest priority. If there are two or more valid targets with the " +
			"same priority, the creature will select the nearest one and in case of standoff gaps the active target will be selected by chance.";
		public static readonly string TARGET_SELECTION_CRITERIA_OPTIONS = "In addition to the Selection Priority you can refine the desired selection criteria with additional " +
			"options and complex conditions rules.\n\n" +
			"" +
			"- Selection Range (SR)\n" +
			"The Selection Range defines the maximum distance in which the creature could detect the target. If the Selection Range is adjusted to zero, the Selection Range " +
			"will be ignored and the condition will be always true.\n\n" +
			"- Selection Angle (SA)\n" +
			"While the Field Of View defines the view angle of the creature, the Selection Angle deals with a notional view angle of the target, in which the creature must be " +
			"inside to fulfil the condition. To adjust this angle to zero will have the same effect as an adjustment of 360 degrees, in both cases the selection angle will be " +
			"ignored and the condition will be true.\n\n" +
			"- Retaining Time (RT)\n" +
			"The Retaining Time defines the time-span in which an active target will stay selected even if the given selection scenario failed. The Retaining Time will be " +
			"helpful to avoid flickering target changes, caused by changeable and/or unsteady conditions, which squeezes the creature to swap quickly between two or more targets. " +
			"Aside from that, a small coasting also refines the realistic behaviour of the creature, because a real creature needs time to evaluate situations and to correct its " +
			"course of motions as well.\n\n" +
			"- Break Time (BT)\n" +
			"The Break Time defines the minimum time-span in which an inactive target have to pause before it can be activated again. The Break Time is helpful to avoid unwanted " +
			"change cycles, caused by changeable and/or unsteady conditions, which squeezes the creature to swap quickly between two or more targets.\n\n" +
			"" +
			"Please note: the following sensoria options will be available only if the sensoria section of the status settings is enabled.\n\n" +
			"- Sensoria : Field Of View (FOV)\n" +
			"While Field Of View (FOV) is active the target must be inside the defined view angle of the creature. Please note, that FOV verifies the position of the creature only" +
			"and not the real visibility.\n\n" +
			"- Sensoria : Visibility Check (VC)\n" +
			"While Visibility Check is active the target must be visible for the creature, which means that the visual axis between creature and target may not be intersected by " +
			"another collider. Please note, that VC verifies the sighting line without consideration of current viewing direction.\n\n" +
			"- Sensoria : Audibility Check (AC)\n" +
			"While the Audibility Check is active the target must be hearable for the creature.\n\n" +
			"- Sensoria : Odour Check (OC)\n" +
			"While the Odour Check is active the target must be smellable for the creature.\n\n" +
			"- Sensoria : Tactile Check (TC)\n" +
			"While the Tactile Check is active the target must be palpable for the creature.\n\n" +
			"- Sensoria : Flavour Check (FC)\n" +
			"While the Flavour Check is active the target must be tasty for the creature.\n\n" +
			"" +
			"- Pre-Selection (PRE)\n" +
			"Basically the creature will preselect a GameObject for each defined target before running the final target selection process for all available targets. By using " +
			"the 'Pre-Selection' option you can adapt several conditions to optimize the pre-selection process. Please note that changes to the preselection settings are " +
			"normally only useful if several target objects of the same type are available simultaneously during the runtime.\n\n" +
			"- Advanced Selection Criteria\n" +
			"The advanced Target Selection Criteria provides you to define multiple selectors with customized conditions, to enhanced the selection criteria by a more " +
			"detailed fine-tuning.\n\n";
		public static readonly string TARGET_SELECTION_CRITERIA_RANGE = "The Selection Range defines the maximum distance in which the creature could detect the target. " +
			"If the Selection Range is adjusted to zero, the Selection Range will be ignored and the condition will be always true.";
		public static readonly string TARGET_SELECTION_CRITERIA_ANGLE = "While the Field Of View defines the view angle of the creature, the Selection Angle deals with a " +
			"notional view angle of the target, in which the creature must be inside to fulfil the condition. To adjust this angle to zero will have the same effect as " +
			"an adjustment of 360 degrees, in both cases the selection angle will be ignored and the condition will be true.";
		public static readonly string TARGET_SELECTION_CRITERIA_RETAINING_TIMER = "The Retaining Time defines the time-span in which an active target will stay selected " +
			"even if the given selection scenario failed. The Retaining Time will be helpful to avoid flickering target changes, caused by changeable and/or unsteady " +
			"conditions, which squeezes the creature to swap quickly between two or more targets. Aside from that, a small coasting also refines the realistic behaviour of " +
			"the creature, because a real creature needs time to evaluate situations and to correct its course of motions as well.";
		public static readonly string TARGET_SELECTION_CRITERIA_DELAY_TIMER = "The Delay Time defines the minimum time-span in which an inactive target have to wait before it " +
			"can be activated if all other conditions will be fulfilled. The Delay Time is helpful to avoid unwanted change cycles, caused by changeable and/or unsteady " +
			"conditions, which squeezes the creature to swap quickly between two or more targets.";

		public static readonly string TARGET_MOVE_SPECIFICATIONS = "Your creature always try to reach the TargetMovePosition of the given target " +
			"object. By default the Target Move Position will be the transform position of a target, but in the majority of cases the transform " +
			"position will be suboptimal or simply non-practical as access point and therefore the Target Move Specifications provides several " +
			"settings to adapt the TargetMovePosition as desired and allows you to define a fixed point related to the target or a dynamic and " +
			"randomized position as well.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_CIRCULAR_RANGE = "The Fixed Range defines a circular area around the target which the creature will try to leave.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET = "The Offset values specifies a local position related to the transform position of " +
			"the target. The offset settings are optional and allows you to adapt the target position if the original transform position of an object is not " +
			"reachable or in another way suboptimal or not usable. The TargetOffsetPosition contains the world coordinates of the local offset, which will " +
			"used as centre for the randomized positioning and finally as TargetMovePosition as well.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET_ANGLE = "Additional to adapt the offset position by enter the coordinates, you can use distance and angle to define the " +
			"desired position. Angle defines the offset angle related to the transform position of the target. Zero (or 360) degrees defines a position in front of " +
			"the target, 180 degrees consequently a position behind it etc.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET_DISTANCE = "Additional to adapt the offset position by enter the coordinates, you can use distance and angle to define the " +
			"desired position. Distance defines the offset distance related to the transform position of the target.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET_UPDATE = "While using a random position you can define the update conditions. Update On Active will refresh the " +
			"position whenever the target becomes active.\n" +
			"While using a random position you can define the update conditions. Update On Reached will refresh the " +
			"position whenever the creature has reached the given TargetMovePosition.\n" +
			"While using a random position you can define the update conditions. Update On Timer will refresh the position " +
			"according to the defined interval.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET_UPDATE_ACTIVATE = "While using a random position you can define the update conditions. Update On Active will refresh the " +
			"position whenever the target becomes active.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET_UPDATE_REACHED = "While using a random position you can define the update conditions. Update On Reached will refresh the " +
			"position whenever the creature has reached the given TargetMovePosition.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_OFFSET_UPDATE_TIMER = "While using a random position you can define the update conditions. Update On Timer will refresh the position " +
			"according to the defined interval.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_RANDOM_RANGE = "Random Positioning Range specifies the radius of a circular area around the TargetOffsetPosition to provide a " +
			"randomized positioning. The combination of TargetOffsetPosition and Random Range produced the TargetMovePosition as the final target position, which will used for all target " +
			"related moves. While using a random position you can define the update conditions to reposition the TargetMovePosition during the runtime. Please note, that you can also combine two " +
			"or all conditions as well.";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_STOP_DISTANCE = "The Target Stopping Distance defines the minimum distance related to the TargetMovePosition " +
			"to complete the current move. If your creature is within this distance, the TargetMovePosition was reached and the move is complete (that’s the precondition to " +
			"run a RENDEZVOUS behaviour).";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_IGNORE_LEVEL_DIFFERENCE = "While Ignore Level Differences is flagged, the distance between your creature and the selected target will " +
			"measured without differences in height. By default, this option is ON because it covers the most cases and tolerates also roughly target position settings, but in some cases (e.g. levels or " +
			"buildings with walkable surfaces on several elevations etc.) you will need also the differences of y-axis. ";
		public static readonly string TARGET_MOVE_SPECIFICATIONS_SMOOTHING = "The Smoothing Multiplier affects step-size and update speed of the TargetMovePosition. " +
			"If Smoothing is adjusted to zero the TargetMovePosition will relocated directly during an update, if Smoothing is adjusted to one the " +
			"TargetMovePosition will changed extremely slow and soft.";

		public static readonly string TARGET_EVENT = "Target Events allows your creature to send messages to the GameObject of an active target. You could use this feature for example to call " +
			"a damage handler or to activate specific functions (e.g. open a door etc.). To use the Target Events enable this feature and ADD your desired events.";
		
		public static readonly string TARGET_INFLUENCES = "The Target Inluences enables to adapt influences if the target is active (e.g. your prey creature have detected a predator target and therefore " +
			"its stress level goes up)";

		public static readonly string TARGET_BEHAVIOUR = "Whenever your creature select a target it needs to know how to react to the target or rather how to handle the changed situation. " +
			"A typical example would be to set a RUN behaviour so your creature will try to reach the Target Move Position of the target. That’s absolutely suitable as long as the target is " +
			"moving or if your creature is using a dynamic position around the target (see Random Positioning Range) or basically as long as your creature will hunting but never reach the " +
			"given Target Move Position, because in cases the creature reaches the target a RUN behaviour could force your creature to spinning around the target. To avoid such a " +
			"misbehaviour you can enable the advanced option (ADV button) to define a suitable Rendezvous Behaviour for cases your creature will reach the Target Move Position or rather " +
			"the given Stopping Distance.";

		public static readonly string TARGET_BEHAVIOUR_STANDARD = "Standard action behaviour, which will be used when the target becomes active outside of the specified Stopping Distance.";
		public static readonly string TARGET_BEHAVIOUR_RENDEZVOUS = "Advanced action behaviour, which will be used when the creature have reached the specified Stopping Distance.";

		public static readonly string ESSENTIALS_BEHAVIOURS_STANDARD = "";


		public static readonly string TARGET_GROUP_MESSAGE = "The Target Group Message is a new feature which allows your creature to communicate with other creatures in its " +
			"group.";
		public static readonly string TARGET_GROUP_MESSAGE_COMMAND = "";

		public static string GetTargetSelectionExpressionTypeHint( SelectionExpressionType _type )
		{
			string _hint = "";

			switch( _type )
			{
				case SelectionExpressionType.OwnAge:
					_hint = "Creature Age";
					break;
			case SelectionExpressionType.OwnerAltitude:
				_hint = "Creature Altitude";
				break;
			case SelectionExpressionType.OwnerPosition:
				_hint = "Creature Position";
					break;
			case SelectionExpressionType.OwnFitness:
				_hint = "Creature Fitness";
				break;

			case SelectionExpressionType.OwnHealth:
				_hint = "Creature Health";
				break;

			case SelectionExpressionType.OwnStamina:
				_hint = "Creature Stamina";
				break;

			case SelectionExpressionType.OwnPower:
				_hint = "Creature Power";
				break;

			case SelectionExpressionType.OwnDamage:
				_hint = "Creature Damage";
				break;

			case SelectionExpressionType.OwnStress:
				_hint = "Creature Stress";
				break;

			case SelectionExpressionType.OwnDebility:
				_hint = "Creature Debility";
				break;

			case SelectionExpressionType.OwnHunger:
				_hint = "Creature Hunger";
				break;

			case SelectionExpressionType.OwnThirst:
				_hint = "Creature Thirst";
				break;

			case SelectionExpressionType.OwnAggressivity:
				_hint = "Creature Aggressivity";
				break;

			case SelectionExpressionType.OwnBehaviour:
				_hint = "Creature Behaviour";
				break;

			default:
				_hint = _type.ToString();
				break;
			}

			return _hint;
		}


		// ################################################################################
		// COCKPIT
		// ################################################################################

		public static readonly string ENTITY_DEBUG_OPTIONS = "The debug options provide you several tools to monitoring the runtime" +
			"behaviour of your entity, so it’s easier to you to detect and avoid misfeature and nonconformities.";
		public static readonly string ENTITY_QUICK_SELECTION = "The Quick Selection popup allows you to switching directly between all your " +
			"registered creatures and objects. ";

		public static readonly string CREATURE_PRESETS = "Save and load creature settings to use it for other creatures or situations.";
		public static readonly string DISPLAY_OPTIONS = "Here you can choose your individual display options, dependent to your tasks and " +
			"requirements. Hide unneeded feature and reduce the component to the relevant parts, so you will never lose the track. " +
			"While ‘global’ display option is active, you will use the given display settings for all your creatures. Use this feature in " +
			"conjunction with the 'Quick Selection' to get a quick access to your focused settings sections.";
		
		public static readonly string DISPLAY_OPTIONS_RUNTIME_INFO = "While playing in the editor, many dynamic characteristics of the creature " +
			"are visualized in the Inspector in real time, which is very useful to check the correct behavior of a creature and adjust the " +
			"settings, but please note that the reading and displaying of these runtime data as well as updating the editor takes up a " +
			"lot of performance and has a negative effect on the framerate.";

		public static readonly string REGISTER_MISSING =  "Sorry, it looks like that there is no active Creature " +
			"Register in your Scene, do you want to add one?";
		public static readonly string REGISTER_DISABLED = "Sorry, it looks like that there is no active Creature " +
			"Register in your Scene, do you want to activate it?";


		// ################################################################################
		// WIZARD
		// ################################################################################
		public static readonly string WIZARD = "";
		

		// ################################################################################
		// ESSENTIALS
		// ################################################################################
		public static readonly string ESSENTIALS = "Essentials covers all fundamental settings your creature needs for its first steps, " +
			"such as a home location, basic behaviours and the general motion and pathfinding settings as well.";
		public static readonly string ESSENTIALS_HOME = "Here you have to define the home location of your creature. This place will be its starting " +
			"point and also the area where it will go to rest and to respawn after dying. Whenever your creature is not busy or for any reason not " +
				"ready for action (e.g. wounded, too weak etc.) it will return to this place.";
		public static readonly string ESSENTIALS_HOME_BEHAVIOUR = "By default a typical Target have two behaviours only - Standard and Rendezvous. One move " +
			"behaviour to reach the TargetMovePosition and one behaviour in cases this position was reached. The Home Target knows a further behaviour in " +
			"cases a creature will be within the Random Positioning Range, so it can do some additional idle activities while it is close to its home.";
		public static readonly string ESSENTIALS_HOME_BEHAVIOUR_TRAVEL = "The TRAVEL behavior is used by your creature to reach the TargetMovePosition, or " +
			"when using a random positioning range to reach the given area around the target. However, the TRAVEL behavior should be a move behaviour.";
		public static readonly string ESSENTIALS_HOME_BEHAVIOUR_RENDEZVOUS = "Idle behaviour after reaching the current target move position. Your creature " +
			"will always use the RENDEZVOUS behavior as soon as it reaches the current TargetMovePosition. In cases where the TargetMovePosition changes " +
			"immediately after this, this behavior change could be undesirable, here it is advisable to use the same behavior for the RENDEZVOUS behavior " +
			"as for TRAVEL in order to avoid interruptions.";
		public static readonly string ESSENTIALS_HOME_BEHAVIOUR_LEISURE = "Randomized leisure activities within the random positioning range of the home " +
			"location. If the Random Positioning Range of the Target Move Specifications is set to zero, this behavior is not available.";

		public static readonly string ESSENTIALS_BEHAVIOURS = "The Default Behaviours represents the proposed minimum performance requirements your " +
			"creature should be able to fulfil. Please note that you can define additional behaviours as required by using the Behaviour section of " +
			"your creature.";

		public static readonly string ESSENTIALS_BEHAVIOURS_IDLE = "Static default behaviour while the creature has reached a Target Move Position " +
			"and has no other tasks to do or if it needs to stop temporary its movement for some reasons.";
		public static readonly string ESSENTIALS_BEHAVIOURS_WALK = "Default move behaviour while approaching to a Target Move Position.";
		public static readonly string ESSENTIALS_BEHAVIOURS_RUN = "Default move behaviour for reaching the given Target Move Position.";

		public static readonly string ESSENTIALS_BEHAVIOURS_SPAWN = "Optional action behaviour while the creature is spawning. If the SPAWN behaviour " +
			"is disabled the creature will use its IDLE behaviour during the spawning process.";
		public static readonly string ESSENTIALS_BEHAVIOURS_WAIT = "Optional idle behaviour while the creature is temporarily blocked for some reason.";
		public static readonly string ESSENTIALS_BEHAVIOURS_REPOSE = "Optional action behaviour while the creature reached the specified vitality limit.";
		public static readonly string ESSENTIALS_BEHAVIOURS_WOUNDED = "Optional action behaviour while the creature get a hit and is wounded.";
		public static readonly string ESSENTIALS_BEHAVIOURS_IMPACT = "Optional action behaviour while the creature receives impact forces.";
		public static readonly string ESSENTIALS_BEHAVIOURS_DEAD = "Optional action behaviour while the creature is dead or dying. You can combine the " +
			"DEAD behaviour with the Corpse options to run a dying animation before spawning the corpse object. If the DEAD behaviour is disabled the " +
			"creature will spawn the Corpse only or run its IDLE behaviour in cases that the Corpse feature is disabled as well.";
		public static readonly string ESSENTIALS_BEHAVIOURS_MOUNTED = "TODO";

		public static readonly string ESSENTIALS_BEHAVIOURS_JUMP = "Optional action behaviour, which will be used if your creature needs to jump.";
		public static readonly string ESSENTIALS_BEHAVIOURS_FALL = "Optional action behaviour, which will be used if your creature is falling.";
		public static readonly string ESSENTIALS_BEHAVIOURS_SLIDE = "Optional action behaviour, which will be used if your creature needs to pass under an obstacle.";
		public static readonly string ESSENTIALS_BEHAVIOURS_VAULT = "Optional action behaviour, which will be used if your creature needs to cross over an obstacle.";
		public static readonly string ESSENTIALS_BEHAVIOURS_CLIMB = "Optional action behaviour, which will be used if your creature needs to climb.";
		public static readonly string ESSENTIALS_BEHAVIOURS_CLIMB_OFFSET = "The body offset defines the relative position of a climbing creature to the object. " +
			"You can change the value to adjust the position of your creature optimally to its climbing movements.";

		public static readonly string ESSENTIALS_BEHAVIOURS_LEISURE = "Optional action behaviour, which will be used whenever the creature is within the defined Random Positioning Range. " +
			"The creature could use this behaviour to run randomized leisure activities before reaching the Stopping Distance. " +
			"Please note, this behaviour is only available if the Random Positioning Range is not adjusted to zero.";
		public static readonly string ESSENTIALS_BEHAVIOURS_RENDEZVOUS = "Advanced action behaviour, which will be used when the creature have reached the specified Stopping Distance.";

		public static readonly string ESSENTIALS_SYSTEM = "Motion and Pathfinding contains the basic settings for physics, motion and pathfinding, " +
			"which are relevant for the correct technical behaviour, such as grounded movements, surface detection etc. ";



		public static readonly string ESSENTIALS_SYSTEM_GRAVITY = "Here you can activate/deactivate the internal gravity. While using the internal " +
			"gravity you don’t need additional components to handle it.";
		public static readonly string ESSENTIALS_SYSTEM_GRAVITY_VALUE = "";
		public static readonly string ESSENTIALS_SYSTEM_GRAVITY_FALL_VELOCITY = "";
		public static readonly string ESSENTIALS_SYSTEM_GRAVITY_INTERPOLATOR = "";

		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE = "While Obstacle Avoidance is active your creature can detect and avoid obstacles " +
			"automatically without additional pathfinding tools. Set the Obstacle Avoidance Popup to BASIC to activate this feature and add all available " +
			"and/or desired obstacle layers. In addition you can adapt the Scanning Range and Angle options to improve the result and/or to optimize the " +
			"performance.";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OFFSET = "The Raycast Offset defines the vertical offset of the scanning ray in relation " +
			"to the transform position. A suitable orientation value will be the third of the body height, so your creature can detect also low obstacles " +
			"which would block its legs or other lower body-parts. While using the Overcome feature you should adapt the offset to the half of the body height, " +
			"because the lower and upper body areas will be covered by separate raycasts to evaluate the best method to avoid or rather to overcome an obstacle.";
		
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_RANGE = "Scanning Range defines the maximum radius your creature will scan. The default " +
			"values should be suitable for the majority of cases but finally the result will be dependent on the given scenario and situation e.g. if there " +
			"are a lot of obstacles closely spaced you should decrease the range, but if the range is too small it could be that your creature will detect " +
			"an obstacle to late and can’t avoid in time. If you are activate the DYN button the creature will determine the scanning range automatically by " +
			"using its current speed and the given multiplier.";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_ANGLE = "The Scanning Angle defines the steps in degrees within a full-circle and with " +
			"this the density of the scan, a lower value will improve the result but also increase the required load, a higher value could be too imprecise. " +
			"In addition you can activate the FIX button to prevent needless course corrections.";

		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME = "While ‘Allow Overcome Obstacles’ is active the creature will try to overcome an " +
			"obstacle by crossing it over or below, so your creature will try to crawl or slide under an obstacle or vault and climb over it before avoid it by " +
			"going around. ";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_OFFSET_DIFFERENCE = "The Overcome Offset Difference specifies the level difference to " +
			"the Vertical Raycast Offset and thereby the vertical position of the additional rays, which handles the lower and upper scan areas while the overcome " +
			"feature is active. By default the difference will be the half of the defined Vertical Raycast Offset value, in which the value will be subtracted from " +
			"the Vertical Raycast Offset to specify the origin of the lower ray and added to specify origin of the upper ray. Activate the RAY flag to display the " +
			"rays in the editor.";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_BELOW = "The Starting Distance defines the desired distance to the contact point to trigger " +
			"the required procedures to overcome the obstacle. While the SPEED flag is enabled the Starting Distance will be determined automatically by using the current " +
			"speed and the given multiplier.";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_OVER = "The Starting Distance defines the desired distance to the contact point to trigger " +
			"the required procedures to overcome the obstacle. While the SPEED flag is enabled the Starting Distance will be determined automatically by using the current " +
			"speed and the given multiplier.";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_START_DISTANCE = "";
		public static readonly string ESSENTIALS_SYSTEM_AVOIDANCE_OVERCOME_START_MULTIPLIER = "";

		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION = "The Overlap Prevention can be used to obviate intersections with other objects but also to " +
			"bypass obstacles in a smart way. Compared or rather additional to the Obstacle Avoidance, which will avoid collisions with potential obstacles, the Overlap " +
			"Prevention will handle such collisions to avoid unrealistic mesh intersections. By default the Overlap Prevention will stop the creature if a collision with " +
			"another object was detected but you can activate the AVOID option, so your creature will change its direction and will try to find the best way along the " +
			"colliders’ surface to avoid further collisions.\n\n" +
			"To use the Overlap Prevention you have to select the desired type which will be suitable to cover the mesh of your creature in the best way. You could use the " +
			"AUTO button to handle the required settings automatically by using the settings of an existing collider.";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_SIZE = "TODO";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_RADIUS = "TODO";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_CENTER = "TODO";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_END = "TODO";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_AVOID_SPEED_MULTIPLIER = "TODO";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_ESCAPE_SPEED_MULTIPLIER = "TODO";
		public static readonly string ESSENTIALS_SYSTEM_OVERLAP_PREVENTION_ANGULAR_SPEED = "TODO";

		public static readonly string ESSENTIALS_BODYPARTS = "TODO";
		public static readonly string ESSENTIALS_BODYPART = "TODO";
		public static readonly string ESSENTIALS_BODYPART_ADD = "TODO";

		// ################################################################################
		// DEADLOCK
		// ################################################################################
		public static readonly string DEADLOCK = "A deadlock is a situation in which your creature can’t reach its desired move position. This could " +
			"have several reasons, such as the typical case that its route is blocked by obstacles etc., but it could also be that its forward velocity " +
			"is too high or the angular speed too low, so that your creature have - for the given situation - not the required manoeuvrability to reach " +
			"the Stopping Distance to complete this move. In such cases you can observe two typical behaviours – if the path is simply blocked your " +
			"creature will still walking or running on the same spot, but if the manoeuvrability is not suitable to the given stopping distance, your " +
			"creature will moving in a circle or a loop. The Deadlock Handling allows your creature to detect such mistakes and you can define how your " +
			"creature should react.";
		public static readonly string DEADLOCK_MOVE_DISTANCE = "Move Distance defines the minimum distance which your create have to covered within the " +
			"specified time. Please note that distance and interval should be suitable to the lowest forward speed of your creature.";
		public static readonly string DEADLOCK_MOVE_INTERVAL = "Test Interval defines the time in which your creature have to cover the Move Distance.";
		public static readonly string DEADLOCK_MOVE_CRITICAL_POSITION = "Max. Critical Positions defines the upper tolerance limit to trigger a deadlock. " +
			"If this value is adjusted to zero, each critical event will directly trigger a deadlock, otherwise the limit must be reached.";
		public static readonly string DEADLOCK_LOOP_RANGE = "Loop Range works analogue to the Move Distance Test but in larger dimensions. The Loop " +
			"Range should be larger than the given Stopping Distance and the interval suitable to the lowest walking speed of your creature. ";
		public static readonly string DEADLOCK_LOOP_INTERVAL = "Test Interval defines the time in which your creature have to leave the Loop Range.";
		public static readonly string DEADLOCK_LOOP_CRITICAL_POSITION = "Max. Critical Positions defines the upper tolerance limit to trigger a deadlock. " +
			"If this value is adjusted to zero, each critical event will directly trigger a deadlock, otherwise the limit must be reached.";
		public static readonly string DEADLOCK_ACTION = "Deadlock Action defines the desired procedure in cases of deadlocks.";
		public static readonly string DEADLOCK_ACTION_BEHAVIOUR = "";
		public static readonly string DEADLOCK_ACTION_DIE = "";
		public static readonly string DEADLOCK_ACTION_UPDATE = "";


		

		// ################################################################################
		// LEAN ANGLE
		// ################################################################################

		public static readonly string ESSENTIALS_SYSTEM_RAYCAST_VERTICAL_OFFSET = "The vertical Raycast Offset defines the height of the raycast origin " +
			"related to the given creature position. By default this value is 0.5 but dependent to the terrain it could be helpful to increase this value " +
			"to reach better results.";

		public static readonly string ESSENTIALS_SYSTEM_LEAN_ANGLE_WARNING = "Please note: This feature works well with Legacy Animations but " +
			"shows no results by using the Mecanim Animation System. That’s because Mecanim handles the Root Transform Rotations according to the given " +
			"animation curves and ignores external changes. I’ll fixed this in the course of the further integration of the Mecanim Animation System.";
		public static readonly string ESSENTIALS_SLOPE_LIMITS = "While Use Slope Limits is flagged you can define the maximum slope limit your creature " +
			"can use and in addition to that you can also adapt the maximum walkable slope angle, so that your creature will try to find a walkable way.";
		public static readonly string ESSENTIALS_SLOPE_LIMITS_SCANNING_RANGE = "Defines the distance which will be used to evaluate the slope values around the creature.";
		public static readonly string ESSENTIALS_SLOPE_LIMITS_SCANNING_ANGLE = "TODO";
		public static readonly string ESSENTIALS_SLOPE_LIMITS_MAX_SLOPE_ANGLE = "Maximum slope angle your creature can use.";
		public static readonly string ESSENTIALS_SLOPE_LIMITS_BEST_SLOPE_ANGLE = "Best slope angle your creature will use to find a walkable way.";
		public static readonly string ESSENTIALS_AVOID_WATER = "While Avoid Water is flagged your creature will avoid surfaces with the water layer.";
		public static readonly string ESSENTIALS_OUT_OF_AREA = "By default your creatures are able to leave intended game areas without to get lost in space while " +
			"passing the border of a restricted terrain or mesh. If a creature can’t detect a ground, it will continue with its current activities by keeping its last " +
			"known operating level. This behaviour should bridge potential leaks while loading or generating its surrounding environment. If you deactivate Out-Of-Area " +
			"moves, your creature will break its current activities by updating its move position to avoid border zones.";

		public static readonly string BODY_ORIENTATION_TYPE = "Here you can specify the vertical direction of your creature relative " +
			"to the ground, which is important for movements on slanted surfaces, hilly grounds or during the fly. The 'ADV' button activates a more " +
			"extensive method to handle the Body Orientation for Crawler, Quadruped and Glider creature types. Please note, that the Body Orientation will " +
			"be automatically adjusted to GLIDER whenever you allow vertical movements.";

		public static readonly string BODY_ORIENTATION_TYPE_OVERRIDE = "Here you can override the default body orientation of your creature according to the " +
			"given animation. The body orientatin defines the vertical direction of your creature relative to the ground, which is important for movements " +
			"on slanted surfaces, hilly grounds or during the fly. The 'ADV' button activates a more extensive method to handle the Body Orientation for " +
			"Crawler, Quadruped and Glider creature types. Please note, that the Body Orientation will be automatically adjusted to GLIDER whenever you allow " +
			"vertical movements.";
		
		public static readonly string BODY_ROLL_ANGLE = "The Roll Angle options allows your creature to use rotations along its z-axis to lean into a turn. The " +
			"roll angle is related to the current speed but the angle can adjusted and limited by changing the multiplier and the maximum value.";

		public static readonly string BODY_PITCH_ANGLE = "The Pitch Angle options allows your creature to to use rotations along its x-axis. The " +
			"pitch angle is related to the current speed but the angle can adjusted and limited by changing the multiplier and the maximum value.";

		// ################################################################################
		// MOVE
		// ################################################################################

		public static readonly string MOVE = "The spatial movements of your creature are basically just position changes from one point to " +
			"another or rather from the current transform position of your creature to the given TargetMovePosition. The raw results are consequently " +
			"straight-line paths between these two points, which are usually insufficient for realistic movements and so the control provides several " +
			"options to optimize movements.";

		public static readonly string MOVE_DEFAULT = "The Default Move settings will be used for all standard situations and describes the " +
			"manoeuvre form the current transform position to the TargetMovePosition.";

		public static readonly string MOVE_SEGMENT_LENGTH = "The final destination point is basically the given TargetMovePosition and as long as there " +
			"are no obstacles or other influences, your creature will follow a straight-line path to reach this position. If Move Segment Length is not " +
			"adjusted to zero, the linear path will subdivided in segments of the defined length and the outcome of this is a sub-ordinate MovePosition " +
			"which can be used to modulate the path.";
		public static readonly string MOVE_SEGMENT_VARIANCE = "Use the Segment Variance Multiplier to randomize the Segment Length during the runtime. The " +
			"length will be updated when the stopping distance at the end of a segment was reached.";
		public static readonly string MOVE_LATERAL_VARIANCE = "Use the Lateral Variance Multiplier to force a randomized sideward drift. The random value " +
			"will be refreshed when the stopping distance at the end of a segment was reached.";
		public static readonly string MOVE_STOPPING_DISTANCE = "The Move Stopping Distance determined the minimum distance related to the actual MovePosition " +
			"to complete the current move. If your creature is within this distance, the MovePosition was reached and the move is complete.";
		public static readonly string MOVE_IGNORE_LEVEL_DIFFERENCE = "While Ignore Level Differences is flagged, the distance between your creature and the " +
			"given MovePosition will measured without differences in height. By default, this option is ON because it covers the most cases and tolerates also " +
			"roughly target position settings, but in some cases (e.g. levels or buildings with walkable surfaces on several elevations etc.) you will need " +
			"also the differences of y-axis. ";


		// ################################################################################
		// EXTERNAL COMPONENTS
		// ################################################################################
		public static readonly string MOTION_CONTROL = "ICECreatureControl works fine without additional Components and can handle a lot of situations " +
			"autonomously, that’s particularly important if you need a large crowd of performance friendly supporting actors, but it will definitely not covering " +
			"all potential situations and for such cases ICECreatureControl supports several Unity Components to enhance the functionality.";
		public static readonly string MOTION_CONTROL_NAVMESHAGENT = "The internal pathfinding technics are sufficient for open environments, such as natural " +
			"terrains or large-scaled platforms, but less helpful for closed facilities, areas covered by buildings and/or constructions or walkable surfaces with " +
			"numerous obstacles. For such environments you could activate the NavMeshAgent, so that your creature will using Unity’s Navigation Mesh. Activating " +
			"‘Use NavMeshAgent’ will automatically add the required NavMeshAgent Component to your creature and handle the complete steering. The only things you " +
			"have to do is to check and adjust the ‘Agent Size’, the desired ‘Obstacle Avoidance’ and ‘Path Finding’ settings. Needless to say, that the " +
			"NavMeshAgent Component requires a valid Navigation Mesh.";
		public static readonly string MOTION_CONTROL_NAVMESHAGENT_MISSING = "NavMeshAgent component is missing, please assign this component to your creature!";
		public static readonly string MOTION_CONTROL_NAVMESHAGENT_SAMPLEPOSITION = "If UseSamplePosition is activated, the system checks whether the current " +
			"target position is located on the navigation mesh and corrects the positions if necessary. The float value determines the maximum distance to " +
			"the mesh, so make sure that a target position is never farther away from the navigation mesh and consider that the larger distances also requires " +
			"higher costs.";

		public static readonly string MOTION_CONTROL_NAVMESHAGENT_ANGULARSPEED = "The default angular speed of the NavMeshAgent is often too low to ensure proper " +
			"rotation according to the forward speed. In such cases, you can increase the angular speed to avoid lateral sliding of the creature.";
		public static readonly string MOTION_CONTROL_NAVMESHAGENT_RIGIDBODY_INFO = "Note: The creature has a non-kinematic Rigidbody and at least one active Collider. " +
			"While using the NavMeshAgent, this combination could affect the movements negative in case of collisions. To avoid such physical influences, all " +
			"axes of the Rigidbody are automatically frozen by ICE.";
			
		public static readonly string MOTION_CONTROL_RIGIDBODY = "Basically, each moveable object in your scene should have a Rigidbody and especially if it " +
			"has also a collider to detect collisions. Without Rigidbody Unity’s physics engine assumes that an object is static and static (immovable) objects " +
			"should consequently not have collisions with other static (immovable) objects and therefore Unity will not testing collision between such supposed " +
			"static objects, which means, that at least one of the colliding objects must have a Rigidbody additional to a collider, otherwise collisions will " +
			"not detected. But no rule without exception and there are absolutely some cases your creature really doesn’t need a Rigidbody or even a Collider as " +
			"well. \n\nApart from that is a Rigidbody more than a supporting element to detect collisions and you can use the physical attributes of the Rigidbody " +
			"to affect the behaviour of your creature but please note, that the steering by forces is not implemented in the current version (coming soon) and " +
			"using the physics with custom settings could yield funny results.\n\nFor a quick setup you can use the Preset Buttons. FULL (not implemented in the " +
			"current version) will prepare Rigidbody and ICECreatureControl to control your creature in a physically realistic way. SEMI deactivates the gravity, " +
			"enables the kinematic flag and allows position changes. OFF deactivates the gravity, checks the kinematic flag and restricts position changes. In " +
			"all cases rotations around all axes should be blocked.";
		public static readonly string MOTION_CONTROL_RIGIDBODY_MISSING = "Rigidbody component is missing, please assign this component to your creature!";
		public static readonly string MOTION_CONTROL_CHARACTER_CONTROLLER = "In addition to the other listed controller types you can also use Unity’s CharacterController " +
			"component to steering your creature.";
		public static readonly string MOTION_CONTROL_CHARACTER_CONTROLLER_MISSING = "CharacterController component is missing, please assign this component to your creature!";
		public static readonly string MOTION_CONTROL_CUSTOM = "While using CUSTOM motion control, ICECreatureControl will calculate the move positions but the final " +
			"movement of your creature will be handled by an external component, such as A*Pathfinding. Please add the desired pathfinding or character controller " +
			"component and the assiociated adapter to your creature. If there is no suitable adapter available please contact the developer for further information.";
		public static readonly string MOTION_CONTROL_IGNORE_ROOT_MOTION = "When using animations with root motions, no additional positional changes are required, but " +
			"if such changes are desired, they can be forced with Ignore Root Motions. In such a case, both position changes would add up and possibly reflect themselves " +
			"in an excessive speed and / or in unexpected movements.";


		// ################################################################################
		// STATUS
		// ################################################################################
		public static readonly string STATUS = "Status represents the mental and physical fitness of your creature, based on several settings, measurements and dynamic " +
			"values which will affect the durability and the behaviour of your creature during the runtime as well. You can use this component section to adapt species-typical " +
			"characteristics, such as the sense and reaction time, maximum age etc. \n\nDependent to your needs and requirements ICECreatureControl provides you a Basic and an " +
			"Advanced Status. The Basic Status contains the essential elements your creature will need for a basic life-cycle, any damage will directly affect the health and " +
			"with it the durability and your creature will die as soon as its durability is exhausted. This procedure is similar to the ‘hit point’ concept but you can expand " +
			"this functionality by activating the Advanced Status, which provides you an extensive Status System to picture the characteristics of a realistic life-form.\n\nWhile " +
			"using the Advanced Status the mental and physical fitness based primarily on three dynamic attributes (health, stamina and power) which in turn consists of further " +
			"detail settings (e.g. age, armor, aggressivity etc.) and several sets of indicators, multipliers and random variances for tuning the final influence. \n\n" +
			"Please note that all these settings are optional.";

		public static readonly string STATUS_BASICS = "The Basics Status contains the essential elements your creature will need for a basic life-cycle, " +
			"which allows your creature to sense and react, to receive damage and to recreate, to die and also to respawn. You can activate the Advanced " +
			"Status Settings to use the extensive Status System.";
		public static readonly string STATUS_ADVANCED = "Dependent to the desired complexity and the given requirements, ICECreatureControl provides you " +
			"additional to the Basic Settings an enhanced Status System. While using the Basic Status, the fitness of your creature will be always identical " +
			"with the health value and finally just the antipode of damage – increasing the damage will directly reduce the health and consequently the " +
			"Fitness as well. By using the Advanced Status the procedure to evaluate the Fitness is affected by several different initial values, indicators, " +
			"multipliers and random variances.";

		public static readonly string STATUS_GENDER = "Gender defines the biological characteristics of your creature and in this context also the social and/or " +
			"cultural role of the character.";
		public static readonly string STATUS_FEEDTYPE = "" +
			"Herbivores are animals which only eat plant material. This means leaves, flowers, fruits or even wood. Sheep, horses, rabbits and " +
			"snails are well known examples of herbivores which eat grass and leaves. A parrot, however, which eats fruits and nuts can also be " +
			"called a herbivore" +
			"Omnivores eat both plants and meat. Chickens are omnivores. They eat seeds, but they can also eat worms. human beings are also omnivores, " +
			"although some people choose not to eat meat. These people are called vegetarians." +
			"Carnivores eat meat. A carnivore is a predator because it has to find and catch its prey. Some carnivores, such as wolves, hunt in a " +
			"group called a pack. They move silently and slowly to form a circle around their prey before they attack.";
		public static readonly string STATUS_FEEDTYPE_CANNIBAL = "If 'Cannibal' is flagged Omnivores and Carnivores will be hunt, attack and eat its own species";



		public static readonly string STATUS_PERCEPTION_TIME = "The perception time (PT) defines the time span the creature needs to sense and analyse its given environment, " +
			"which in particular means the delay to detect potential interactor objects e.g. the typical human perception time is between ¼ to ½ a second. Please note: " +
			"you can overwrite this time for each behaviour just by using the influence options of a behaviour rule.";
		public static readonly string STATUS_PERCEPTION_TIME_OVERWRITE = "By activating this feature you can overwrite the default perception time. The perception time defines " +
			"the time span the creature needs to sense and analyse its given environment, which in particular means the delay to detect potential interactor objects " +
			"e.g. the typical human perception time is between ¼ to ½ a second.";
		public static readonly string STATUS_PERCEPTION_TIME_VARIANCE = "The Variance Multiplier defines the threshold variance value, which will be used to " +
			"randomize the associated interval during the runtime.";
		public static readonly string STATUS_PERCEPTION_TIME_MULTIPLIER = "The Fitness Multiplier defines the influence ratio of the fitness value on the " +
			"associated interval.";
		public static readonly string STATUS_REACTION_TIME = "The reaction time (RT) defines the delay in time between its behaviour- and movement corretions as well as other " +
			"motoric reactions, based on the given perception, which particular means a delay in recalculate of its position and orientation. Please note: you can overwrite " +
			"this time for each behaviour just by using the influence options of a behaviour rule.";
		public static readonly string STATUS_REACTION_TIME_OVERWRITE = "By activating this feature you can overwrite the default reaction time. The reaction time (RT) defines " +
			"the delay in time between its behaviour- and movement corretions as well as other motoric reactions, based on the given perception, which particular means a " +
			"delay in recalculate of its position and orientation.";
		public static readonly string STATUS_REACTION_TIME_VARIANCE = "The Variance Multiplier defines the threshold variance value, which will be used to " +
			"randomize the associated interval during the runtime.";
		public static readonly string STATUS_REACTION_TIME_MULTIPLIER = "The Fitness Multiplier defines the influence ratio of the fitness value on the " +
			"associated interval.";
		
		public static readonly string STATUS_RECOVERY_PHASE = "Recovery Phase defines a warm-up period in seconds after the spawning process in which the creature " +
			"will be completely defenceless while running its respawn behaviour without any target. You can adjust this value to provide your player to " +
			"detect a new spawned creature at the right time, otherwise it could be that a spawned creature appear from nowhere and will start its attack " +
			"without any heads-up.";
			
		public static readonly string STATUS_REMOVING_DELAY = "Removing Delay defines the delay time in seconds until a deceased creature will be added to the " +
			"respawn queue. Within this time-span the creature will be visible in the scene and can be used for loot activities. While ‘Use Corpse’ is active the " +
			"Removing Delay will also affect the spawning process of the corpse, therefore you should adjust this value to zero or you can adapt it to the dead " +
			"animation length to use a defined dead animation before spawning the corpse.";
		public static readonly string STATUS_REMOVING_DELAY_VARIANCE = "The Variance Multiplier defines the threshold variance value, which will be used to " +
			"randomize the associated delay time during the runtime.";


		public static readonly string STATUS_INFLUENCE_INDICATORS = "Influence Indicators represents all status attributes which can be affected by direct " +
			"influences, based on internal activities or external forces as well. You can modify this Indicators to customize initial values or to test the " +
			"status settings. By default all this indicators are adjusted to zero.";
		public static readonly string STATUS_DAMAGE_IN_PERCENT = "The Damage attribute represents the effective damage level of your creature in percent. " +
			"Depending on the associated multiplier the value will affect default indicators. ";
		public static readonly string STATUS_STRESS_IN_PERCENT = "The Stress attribute represents the effective stress level of your creature in percent. " +
			"Depending on the associated multiplier the value will affect default indicators. ";
		public static readonly string STATUS_DEBILITY_IN_PERCENT = "The Debility attribute represents the effective debility level of your creature in percent. " +
			"Depending on the associated multiplier the value will affect default indicators. ";
		public static readonly string STATUS_HUNGER_IN_PERCENT = "The Hunger attribute represents the effective hunger level of your creature in percent. " +
			"Depending on the associated multiplier the value will affect default indicators. ";
		public static readonly string STATUS_THIRST_IN_PERCENT = "The Thirst attribute represents the effective hunger level of your creature in percent. " +
			"Depending on the associated multiplier the value will affect default indicators. ";

		public static readonly string STATUS_DAMAGE_TRANSFER_MULTIPLIER = "Typically a creature should be the highest entity within its transform hierarchy, but it could " +
			"be that your creature needs to be a child of another object (e.g. horse and horseman) and in such cases both creatures would have their own independent " +
			"status which is mostly suitable but you could also want to merge two or more creature to a single individual (e.g. an alien, monster or zombie with one " +
			"or more agile abscesses etc.) and in such cases, if the child creature should be a fixed part of the main body, you could enable the Damage Transfer " +
			"Multiplier to forward received damages from the child to the main entity according to the specified multiplier value.";
			

		public static readonly string STATUS_INFLUENCES = "Influences defines the impact of a triggering event to your creature. These impacts could " +
			"be positive for your creature, such as a recreation processes by reducing the damage while your creature is sleeping or eating, or " +
				"negative through increasing the damage or stress values while your creature is fighting. Impacts will be directly affect the status " +
				"values of your creature and consequently also the selection criteria for a target or interactor. While a triggering event is active and the " +
				"interval is adjusted to zero, the influences will refresh the status values during the framerate-independent update cycle of FixedUpdate " +
				"(default 0.02 secs.), so please make sure that your defined impact values suitable to the short time-span or adjust the interval value, " +
				"otherwise your creature will not acting as expected (e.g. die in seconds or will not select the expected target etc.)";
		public static readonly string STATUS_INFLUENCES_INTERVAL = "Interval defines the time delay in seconds between two influence calls. By default this value " +
			"is adjusted to zero, which means that an influence call will affect your creature in each framerate-independent update cycle of FixedUpdate (default 0.02 secs. .), " +
			"so please make sure, that your defined impact values are suitable to this short time-span or increase the interval value, otherwise your " +
			"creature could die immediately.";
		public static readonly string STATUS_INFLUENCES_STRESS = "Stress specifies the impact to the stress status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_DEBILITY = "Debility specifies the impact to the debility status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_DAMAGE = "Damage specifies the impact to the damage status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_HUNGER = "Hunger specifies the impact to the hunger status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_THIRST = "Thirst specifies the impact to the thirst status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";

		public static readonly string STATUS_INFLUENCES_AGGRESSIVITY = "Aggressivity specifies the impact to the aggressivity status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_ANXIETY = "Anxiety specifies the impact to the anxiety status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_EXPERIENCE = "Experience specifies the impact to the experience status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";
		public static readonly string STATUS_INFLUENCES_NOSINESS = "Nosiness specifies the impact to the nosiness status attribute and depending on the associated " +
			"multiplier to the default indicators as well.";

		public static readonly string STATUS_VITAL_INDICATORS = "Vital Indicators represents calculated status attributes which will be indirect affected " +
			"by the influence indicators and the associated multipliers. By default the reference values are adjusted to 100, but you can modify this values " +
			"as desired to adapt the indicators to your existing environment. The calculated result will be expressed in percent.";

		public static readonly string STATUS_VITAL_INDICATOR_HEALTH = "";
		public static readonly string STATUS_VITAL_INDICATOR_STAMINA = "";
		public static readonly string STATUS_VITAL_INDICATOR_POWER = "";

		public static readonly string STATUS_CHARACTER_INDICATORS = "";
		public static readonly string STATUS_CHARACTER_DEFAULT_AGGRESSITY = "";
		public static readonly string STATUS_CHARACTER_DEFAULT_ANXIETY = "";
		public static readonly string STATUS_CHARACTER_DEFAULT_EXPERIENCE = ""; 
		public static readonly string STATUS_CHARACTER_DEFAULT_NOSINESS = "";


		public static readonly string STATUS_FITNESS_RECREATION_LIMIT = "The Recreation Limit defines the fitness threshold value at which your creature " +
			"will return automatically to its home location to recreate its fitness. If this value is adjusted to zero, the Recreation Limit will ignored, " +
			"otherwise the home target will be handled with the highest priority in cases the value goes below to the limit.";

		public static readonly string STATUS_FITNESS_VITALITY_LIMIT = "The Vitality Limit defines the fitness threshold value at which your creature " +
			"will be to weak for further activies.";

		public static readonly string STATUS_SHELTER = "While using ‘Use Shelter’ your creature will be protected against environment influences (e.g. rain, storm, " +
			"low temperatures etc.) or attacks while enter a safe area which you can define by a tagged trigger. If your creature enter such a trigger the IsSheltered " +
			"flag will be true and will reset to false again in cases it leaves such areas.";
		public static readonly string STATUS_SHELTER_TAG = "";

		public static readonly string STATUS_INDOOR = "While using ‘Use Indoor’ your creature will be protected against environment influences (e.g. rain, storm, " +
			"low temperatures etc.) or attacks while enter a safe area which you can define by a tagged trigger. If your creature enter such a trigger the IsIndoor " +
			"flag will be true and will reset to false again in cases it leaves such areas.";
		public static readonly string STATUS_INDOOR_TAG = "";

		public static readonly string STATUS_TEMPERATURE = "The ‘Use Temperature’ flag activates the thermal sensation of your creature, which will have additional " +
			"influence to the fitness and finally to the behaviour as well. While ‘Use Temperature’ is active, your creature will receive and evaluate temperature " +
			"values based on the environment data of the creature register, which you can easiest combine with your own scripts, third party products or external " +
			"data sources. You can find the ICECreatureUniStormAdapter attached to your ICECreatureControl package (please note, that this adapter requires a " +
			"valid licence of UniStorm)";
		
		public static readonly string STATUS_TEMPERATURE_SCALE = "Temperature Scale defines the desired measuring unit FAHRENHEIT or CELSIUS in degrees";
		public static readonly string STATUS_TEMPERATURE_SCOPE = "The Temperature Scope defines the lowest and highest environment temperature values your creature can survive.";
		public static readonly string STATUS_TEMPERATURE_BEST = "Comfort Temperature defines the ideal temperature value for your creature.";
		public static readonly string STATUS_TEMPERATURE_CURRENT = "Temperature represents the current environment temperature, which your creature receives via " +
			"the environment data of the creature register. This editor field is for testing only, the value will overwrite during the runtime.";

		public static readonly string STATUS_ARMOR = "The ‘Use Armour’ flag activates the Armour of your creature. Armour works as a buffer by absorbing incoming " +
			"damage values. As long as the armour value is larger zero the damage value will remain unaffected.";
		public static readonly string STATUS_ARMOR_IN_PERCENT = "Armour defines the initial armour value in percent and represents the armour during the runtime as " +
			"well. You can adapt this value to customize the initial armour.";

		public static readonly string STATUS_INFLUENCE = "";

		public static readonly string STATUS_DYNAMIC_INFLUENCES = "";

		public static readonly string STATUS_MEMORY = "The Memory represents different kind of memorizations your creature can use to remember e.g. specific situations, " +
			"other creatures or locations but also your player. This feature is currently under construction and will be full available in one of the next versions.";
		public static readonly string STATUS_MEMORY_SPATIAL = "";
		public static readonly string STATUS_MEMORY_SHORT = "";
		public static readonly string STATUS_MEMORY_LONG = "";

		public static readonly string CHARACTERISTICS_SPEED_RUNNING = "Default running speed of the creature!";
		public static readonly string CHARACTERISTICS_SPEED_WALKING = "Default walking speed of the creature!";
		public static readonly string CHARACTERISTICS_SPEED_TURNING = "Default turning speed of the creature!";


		// ################################################################################
		// MISSIONS
		// ################################################################################
		public static readonly string MISSIONS = "Missions represents the standard duties your creature have to fulfil.";

		public static readonly string MISSION_ENABLED = "The Enabled flag allows you to activate or deactivate the complete Mission, without losing the " +
			"data. As long as a Mission is disabled, the creature will ignore them during the runtime. You could use this feature also by your own scripts to " +
				"manipulate the gameplay.";

		// ################################################################################
		// MISSIONS OUTPOST
		// ################################################################################
		public static readonly string MISSION_OUTPOST = "The Outpost Mission is absolutely boring for any high motivated creature and as expected the " +
			"job-description is really short: go home and wait for action! But you could enlarge the Random Positioning Range to give your creature a " +
			"larger scope, add additional rules for LEISURE and RENDEZVOUS and your creature will spend its idle time with some leisure activities. " +
			"Furthermore you could use the Pool Management of the Creature Register to generate some clones, so that your creature isn’t alone. On this " +
			"way you could use the Outpost Mission to populate a village, to setup a camp with soldiers, some animals for a farm or a pack of wolves " +
			"somewhere in a forest etc.";

		public static readonly string MISSION_OUTPOST_TARGET = "The Outpost object could be any reachable object in the scene and btw. movable objects " +
			"as well. Adapt the distances so that your creature feel comfortable, have sufficient space for his idle activities and don't blunder into " +
			"a conflict with the object size.";
		public static readonly string MISSION_OUTPOST_BEHAVIOR = "TODO";

		// ################################################################################
		// MISSIONS ESCORT
		// ################################################################################
		public static readonly string MISSION_ESCORT = "The Escort Mission means entertainment for your creature. It have to search and follow the leader " +
			"wherever he is and goes! You could use this mission to specify a faithful and brave companion to your player or to any another NPC as well. " +
			"You could also combine this mission with other Targets, such as the Patrol Mission, to use your creature as a guide which can show your player " +
			"secret places.";
		public static readonly string MISSION_ESCORT_TARGET = "The Leader object could be any reachable object in the scene. Adapt the distances so that your " +
			"creature have enough space for his activities and don't blunder into a conflict with the leader moves. ";
		public static readonly string MISSION_ESCORT_BEHAVIOUR = "";

		public static readonly string MISSION_SCOUT = "";

		// ################################################################################
		// MISSIONS PATROL
		// ################################################################################
		public static readonly string MISSION_PATROL = "The Patrol Mission represents a typical Waypoint Scenario and is - up to now - the most varied standard " +
			"task for your creature, so the job-description is a little bit more comprehensive: Find out and TRAVEL to the nearest waypoint. If you are reach " +
			"the Max. Range (Random Positioning Range + Stopping Distance) and it’s a transit-point, ignore LEISURE and RENDEZVOUS, find out the next waypoint " +
			"accordind to the given path-type and start to PATROL. If it’s not a transit-point, follow the LEISURE rules until reaching the RENDEZVOUS position " +
			"(TargetMovePosition + Stopping Distance) and execute the RENDEZVOUS instructions over the given period of time (Duration Of Stay). Afterwards find " +
				"out the next waypoint accordind to the given path-type and start to PATROL. Repeat these instructions for each waypoint.";
		public static readonly string MISSION_PATROL_ORDER_TYPE = "The Order Type defines the desired sequence in which your creature have to visit the single " +
			"waypoints. Please consider, that your creature will always starts with the nearest waypoint, so if you want that it will start with a special one " +
			"you should place it in the near. By default the cycle sequence is ordered in ascending order, activate the DESC button to change it to descending.";
		public static readonly string MISSION_PATROL_TARGET = "";
		public static readonly string MISSION_PATROL_WAYPOINTS = "To prepare a Patrol Mission you can add single waypoints, which could be any reachable objects " +
			"in your scene, or you can add a complete waypoint group, which is a parent object with its children. By using this way, the children will be used as " +
			"waypoints, while the parent will be ignored.";
		public static readonly string MISSION_PATROL_ADD_WAYPOINT_GROUP = "";
		public static readonly string MISSION_PATROL_ADD_WAYPOINT = "";
		public static readonly string MISSION_PATROL_WAYPOINT = "A Patrol Mission can basically have any number of waypoints. Each waypoint represents a separate " +
			"target and will also be listed with all target features in the inspector. You can move each waypoint item within the list up or down to change the " +
			"order or you can delete completely as well.  Furthermore, you can activate and deactivate each single waypoint as desired, in such a case, your " +
			"creature will skip deactivated waypoints to visit the next ones. ";
		public static readonly string MISSION_PATROL_CUSTOM_BEHAVIOUR = "The ‘Use Custom Behaviour’ flag allows you to overwrite the default patrol behaviour " +
			"rules for the selected waypoint. Activate the ‘Custom Behaviour’ flag to define your additional behaviour rules. Please note, that these rules will " +
			"be used for the selected waypoint only.";

		// ################################################################################
		// INTERACTION
		// ################################################################################
		public static readonly string INTERACTION = "Additional to the standard situations defined in the home and mission settings, you can teach your creature " +
			"to interact with several other objects in your scene, such as the Player Character, other NPCs, static construction elements etc. The Interaction " +
			"Settings provides you to design complex interaction scenarios with each object in your scene.\n\n" +
			"To using the interaction system you have to add one or more Interactors. An Interactor represents another GameObject as potential Target for your " +
			"creature and contains a set of conditions and instructions to define the desired behaviour during a meeting. By default interactors are neutral, " +
			"they could be best friends or deadly enemies as well and basically interactors can be everything your creature has to interact with it, such as a " +
			"football your creature has to play with it or a door, which it has to destroy.\n\n" +
			"After adding a new interactor you will see primarily the familiar target settings as they will be used in the home and mission settings, but instead " +
			"of the object field the interactor settings provides a popup to select the target game object. That’s because, interactors are normally OOIs (objects " +
			"of interest), which could be also interesting for other objects of interest, such as the Player Character or other NPCs and therefore such objects " +
			"have to be registered in the creature register to provide a quick access to relevant data during the runtime. So you have to use the popup to add " +
			"the desired interactor.\n\n" +
			"But the pivotal difference to the home and missions settings is, that you can define an arbitrary number of additional interaction rules, which " +
			"allows you to overwrite the initial target related selection and position settings for each rule. By using this feature you could define a nearly " +
			"endless number of conditions and behaviours for each imaginable situation, but in the majority of cases 3-5 additional rules will be absolutely " +
			"sufficient to fulfil the desired requirements.";

		public static readonly string INTERACTION_INTERACTOR = "An Interactor represents another GameObject as potential Target for your creature and contains a " +
			"set of conditions and instructions to define the desired behaviour during a meeting. By default interactors are neutral, they could be best friends " +
			"or deadly enemies as well and basically interactors can be everything your creature has to interact with it, such as a football your creature has to " +
			"play with it or a door, which it has to destroy.";

		public static readonly string INTERACTION_INTERACTOR_TARGET = "Interactors are mostly objects of interest, which will be normally interesting for other " +
			"objects of interest as well, such as the Player Character or other NPCs and therefore such objects have to be registered in the creature register to " +
			"provide a quick access to relevant data during the runtime. For this reason, you have to use the popup to select your desired interactor. If your " +
			"desired interactor isn’t listed currently, switch to your creature register to add the interactor. Please note, that your interactor object doesn’t " +
			"need additional scripts to be listed in the register, unless your interactor is a player character or a NPCs, which is not controlled by " +
			"ICECreatureControl, in such a case you should add the ICECreatureResident Script to your interactor, which will handle the registration and " +
			"deregistration procedures during the runtime.";
		public static readonly string INTERACTION_INTERACTOR_ENABLED = "The Interactor Enabled flag allows you to activate or deactivate the Interactor, without " +
			"losing the data. As long as an Interactor is disabled, the creature will ignore it during the runtime. You could use this feature also by your own " +
			"scripts to manipulate the gameplay.";
		public static readonly string INTERACTION_INTERACTOR_RULE = "TODO";
		public static readonly string INTERACTION_INTERACTOR_RULE_TARGET = "";
		public static readonly string INTERACTION_INTERACTOR_ADD_RULE = "";

		public static readonly string INTERACTION_INTERACTOR_RULE_BLOCK = "If 'Block Next Rule' is flaged the rule will still active until your creature have " +
			"reached the given move-position. This feature allows you to define a move position outside of the respective Selection Range without influences of " +
			"further Selection Ranges and behaviour changes. Please make sure, that all potential positions " +
			"reachable for your creature, otherwise you will provoke a deadlock!";

		// ################################################################################
		// ENVIROMENT
		// ################################################################################
		public static readonly string ENVIROMENT = "Complementary to the HOME, MISSIONS and INTERACTION features, which are all dealing with the interaction " +
			"between your creature and other GameObjects, the Environment section handles the interaction with the surrounding environment. The current " +
			"Environment System provides your creature two different abilities to sense its surrounding space – SURFACE and COLLISION detection.";
		
		// ################################################################################
		// ENVIROMENT SURFACE
		// ################################################################################
		public static readonly string ENVIROMENT_SURFACE = "The Surface Rules specify the reaction to the specified textures. You could use this feature for " +
			"example to handle footstep sounds and/or footprint effects, but you could also start explosion effects to simulate a " +
				"minefield, or dust effects for a dessert, or you could define textures as fertile soil, where your creature can appease " +
				"one's hunger and thirst etc. ";
		public static readonly string ENVIROMENT_SURFACE_SCAN_INTERVAL = "The Ground Scan Interval value defines the desired time period in seconds to check the current " +
			"ground texture. It’s not required and recommended to do this scan in each frame (value adjusted to zero), by default the creature will do this scan each " +
			"second, which should be suitable for the most cases. You should increase this value if you are using large herds or crowds.";
		public static readonly string ENVIROMENT_SURFACE_RULE = "TODO";
		public static readonly string ENVIROMENT_SURFACE_RULE_NAME = "Name defines just the display name of the rule and have further impact. " +
			"You can rename it to use a more comprehensible and context related term.";
		public static readonly string ENVIROMENT_SURFACE_RULE_TEXTURES = "The Trigger Textures specifies the conditions to activate the assigned procedures. As " +
			"soon as your creature comes in contact with one of the defined textures, the specified procedures will start. Use the Interval settings " +
			"to adjust the desired repeating interval.";
		public static readonly string ENVIROMENT_SURFACE_RULE_PROCEDURES = "Each Surface Rule can initiate several procedures, in cases the given trigger conditions are " +
			"fulfilled. You can adapt the Procedure setting to define the desired behaviour. You could use the procedure settings for example to define footstep " +
			"sounds and/or footprint effects, but you could also start explosion effects to simulate a minefield, or dust effects for a dessert, or " +
			"you could define textures as fertile soil, where your creature can appease its hunger or thirst etc. ";
		public static readonly string ENVIROMENT_SURFACE_BEHAVIOUR = "TODO";
		public static readonly string ENVIROMENT_SURFACE_AUDIO = "TODO";
		public static readonly string ENVIROMENT_SURFACE_EFFECT = "TODO";
		public static readonly string ENVIROMENT_SURFACE_INFLUENCES = "TODO";
		
		// ################################################################################
		// ENVIROMENT IMPACT
		// ################################################################################
		public static readonly string ENVIROMENT_COLLISION = "The Collision Rules defines the reaction to detected collisions. You could use this " +
			"feature for example to adjust the damage if your creature was hit by a bullet, or comes in contact with a melee weapon or a spike wall.";
		public static readonly string ENVIROMENT_COLLISION_RULE_CONDITIONS = "";
		public static readonly string ENVIROMENT_COLLISION_RULE = "The Collision Rules defines the reaction to detected collisions. You could use this " +
			"feature for example to adjust the damage if your creature was hit by a bullet, or comes in contact with a melee weapon or a spike wall.";
		public static readonly string ENVIROMENT_COLLISION_RULE_NAME = "Name defines just the display name of the rule and have further impact. " +
			"You can rename it to use a more comprehensible and context related term.";
		public static readonly string ENVIROMENT_COLLISION_RULE_TYPE = "Type specifies the condition type, which will be using to filter the incoming " +
			"collisions. Currently you can filter incoming collision objects by TAG, LAYER or TAG&LAYER ";
		public static readonly string ENVIROMENT_COLLISION_RULE_TAG = "TODO";
		public static readonly string ENVIROMENT_COLLISION_RULE_BODYPART = "TODO";
		public static readonly string ENVIROMENT_COLLISION_RULE_TAG_PRIORITY = "TODO";
		public static readonly string ENVIROMENT_COLLISION_RULE_LAYER = "TODO";
		public static readonly string ENVIROMENT_COLLISION_RULE_LAYER_PRIORITY = "TODO";
		public static readonly string ENVIROMENT_COLLISION_RULE_PROCEDURES = "TODO";
		public static readonly string ENVIROMENT_COLLISION_BEHAVIOUR = "TODO";
		public static readonly string ENVIROMENT_COLLISION_INFLUENCES = "TODO";
		public static readonly string ENVIROMENT_COLLISION_AUDIO = "TODO";
		public static readonly string ENVIROMENT_COLLISION_EFFECT = "TODO";

		// ################################################################################
		// BEHAVIOUR
		// ################################################################################
		public static readonly string BEHAVIOUR = "While a Target represents a goal, Behaviours defines the way to reach it. The Behaviour settings " +
			"provides you to design and manage complex behaviour instructions and procedures, to reach your needs and goals and finally a realistic " +
			"and natural behaviour of your creature.";

		// ################################################################################
		// BEHAVIOUR MODE
		// ################################################################################
		public static readonly string BEHAVIOUR_MODE = "The behaviour of your creature is subdivided into single Behaviour Modes. Each of these " +
			"modes contains the instructions for specific situations and can be assigned to target-related or condition-based events. Furthermore " +
			"Behaviour Modes are not bounded to specific assignments and can generally be used for several targets and situations, in case they " +
			"are suitable for them.\n\nEach Behaviour Mode will have at least one Behaviour Rule, but to reach a more realistic behaviour can add " +
			"additional rules, which allows your creature to do things in different ways, break and resume running activities, run intermediate " +
			"animation sequences, start effects or to play audio files as well.";
		public static readonly string BEHAVIOUR_MODE_RENAME = "Renames allows you to change the key of the selected Behaviour Mode. Please note, " +
			"that renaming will remove all existing assignments.";
		public static readonly string BEHAVIOUR_MODE_FAVOURED = "The ‘Favoured’ flag allows you to block other targets and behaviours until the " +
			"defined conditions of the active mode are fulfilled. By using this feature you can force a specific behaviour independent of " +
			"higher-prioritised targets, which will normally determines the active behaviour. You can select several conditions, in this case the " +
			"selected ones will combined with OR, so that just one of them must be true. Please consider, that the active mode will in fact stay " +
			"active until the conditions are fulfilled, so please make sure, that your creature can fulfil the conditions, otherwise you will " +
			"provide a deadlock.";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_PRIORITY = "";

		public static readonly string BEHAVIOUR_MODE_FAVOURED_PERIOD = "";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_MOVE_POSITION_REACHED = "";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_TARGET_MOVE_POSITION_REACHED = "";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_TARGET = "";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_TARGET_POPUP = "";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_TARGET_RANGE = "";
		public static readonly string BEHAVIOUR_MODE_FAVOURED_DETOUR = "";

		// ################################################################################
		// BEHAVIOUR MODE RULE
		// ################################################################################
		public static readonly string BEHAVIOUR_MODE_RULE = "To provide a more realistic behaviour each mode can contains several different rules of " +
			"instructions at the same time, which allows your creature to do things in different ways, break and resume running activities, run " +
			"intermediate animation sequences, start effects or to play audio files as well.";

		public static readonly string BEHAVIOUR_MODE_RULE_WEIGHT = "Weight value for WEIGHTEDRANDOM Sequence Type, relative to other rules weight value.";

		public static readonly string BEHAVIOUR_MODE_RULES_ORDER = "Adapt the order type to define the desired sequence order of the given rules.";
		public static readonly string BEHAVIOUR_MODE_RULE_ADD = "Adds a new rule for the selected behaviour mode.";

		public static readonly string BEHAVIOUR_ANIMATION = "Here you can define the desired animation you want use for the selected rule. " +
			"Simply choose the desired type and adapt the required settings.";
		public static readonly string BEHAVIOUR_ANIMATION_NONE = "Animations are optional and not obligatory required, so you can " +
			"control also each kind of unanimated objects, such as dummies for testing and prototyping, simple bots and turrets or " +
			"movable waypoints.  ";
		public static readonly string BEHAVIOUR_ANIMATION_ANIMATOR = "By choosing the ANIMATOR ICECreatureControl will using the Animator " +
			"Interface to control Unity’s powerful Mecanim animation system. To facilitate setup and handling, ICECreatureControl provide " +
			"three different options to working with Mecanim: \n\n" +
				" - DIRECT – similar to the legacy animation handling \n" +
				" - CONDITIONS – triggering by specified values (float, integer, Boolean and trigger) \n" +
				" - ADVANCED – similar to CONDITIONS with additional settings for IK (ALPHA) \n";

		public static readonly string BEHAVIOUR_ANIMATION_ANIMATOR_CONTROL_TYPE_DIRECT = "DIRECT - similar to the legacy animation handling";
		public static readonly string BEHAVIOUR_ANIMATION_ANIMATOR_CONTROL_TYPE_CONDITIONS = "CONDITIONS – triggering by specified values (float, integer, Boolean and trigger)";
		public static readonly string BEHAVIOUR_ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED = "ADVANCED – similar to CONDITIONS with additional settings for IK (ALPHA)";
		public static readonly string BEHAVIOUR_ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED_INFO = "COMING SOON!";

		public static readonly string BEHAVIOUR_ANIMATION_ANIMATOR_ERROR_NO_CLIPS = "There are no clips available. Please check your Animator Controller!";
		public static readonly string BEHAVIOUR_ANIMATION_ANIMATION = "Working with legacy animations is the easiest and fastest way " +
			"to get nice-looking results. Simply select the desired animation, set the correct WrapMode and go.";
		public static readonly string BEHAVIOUR_ANIMATION_CLIP = "The direct use of animation clips is inadvisable and here only " +
			"implemented for the sake of completeness and for some single cases it could be helpful to have it. But apart from that it " +
			"works like the animation list. Simply assign the desired animation clip, set the correct WrapMode and go.";
		public static readonly string BEHAVIOUR_ANIMATION_CUSTOM = "";



		public static readonly string BEHAVIOUR_LENGTH = "Here you can define the play length of a rule by setting the ‘min’ and " +
			"‘max’ range. If both values are identical, the rule will be playing exactly for the specified time-span, otherwise " +
			"the length will be randomized based on the given values. Please note, that these settings are only available, if your " +
			"selected ‘Behaviour Mode’ contains more than one rule. If you ignore the play length settings while you have more " +
			"than one rule, the control tries to use the animation length but this could originate unlovely results and is " +
			"inadvisable.";
		public static readonly string BEHAVIOUR_LOOK = "TODO";
		public static readonly string BEHAVIOUR_AUDIO = "TODO";
		public static readonly string BEHAVIOUR_EFFECT = "TODO";
		public static readonly string BEHAVIOUR_INVENTORY = "TODO";

		public static readonly string BEHAVIOUR_EVENTS = "TODO";
		public static readonly string BEHAVIOUR_METHOD = "TODO";




		public static readonly string BEHAVIOUR_INVENTORY_TYPE = "";
		public static readonly string BEHAVIOUR_INVENTORY_TYPE_COLLECT = "When using Collect Active Item, the creature will add its " +
			"active target to its inventory. Dependent to the given behaviour and inventory settings, the original GameObject will be " +
			"removed from scene and the creature will looking directly for a new target. If there are no delays defined, this will be " +
			"done in the next frame, which could be too fast for the animator to change the animations correctly, so its advisable to " +
			"define a delay by using the start options or by adapting the desired retaining time in the Favoured options.";
		public static readonly string BEHAVIOUR_INVENTORY_TYPE_DISTRIBUTE = "";
		public static readonly string BEHAVIOUR_INVENTORY_TYPE_DISTRIBUTE_INTERVAL = "";
		public static readonly string BEHAVIOUR_INVENTORY_TYPE_EQUIP = "";
		public static readonly string BEHAVIOUR_INVENTORY_ITEM = "";
		public static readonly string BEHAVIOUR_INVENTORY_ITEM_PARENT = "";
	

		public static readonly string BEHAVIOUR_INFLUENCES = "Each Behaviour Rule can have an impact to your creature, these impacts " +
			"could be positive, such as the recreation (reducing of damage) while sleeping or eating, or negative, such as the increase " +
			"of damage while your creature is fighting. ";
		public static readonly string BEHAVIOUR_LINK = "Link provides the forwarding to a specific Rule or another Behaviour Mode as well.";
		public static readonly string BEHAVIOUR_LINK_SELECT = "";
		public static readonly string BEHAVIOUR_LINK_MODE = "";
		public static readonly string BEHAVIOUR_LINK_RULE = "";

		public static readonly string BEHAVIOUR_MOVEMENTS = "Additional to the Default Move, which you can adapt in the Essential section, each " +
			"Behaviour Rule provides enhanced Movement Options to customize the spatial movements of your creature according to the selected Animation, " +
			"the desired behaviour or other needs and requirements.\n\nIn difference to the Default Move settings, the Behaviour Movements contains in " +
			"addition to the known move specifications, further settings to define advanced movements, the viewing direction and the velocity, which " +
			"is absolutely essential if a desired behaviour is to be provided spatial position changes. In such cases it’s indispensable to adapt the " +
			"velocity settings.";
		public static readonly string BEHAVIOUR_MOVE_VELOCITY = "TODO";
		public static readonly string BEHAVIOUR_MOVE_VELOCITY_FORWARDS = "Velocity defines the desired speed of your creature in z-direction. Please note, that " +
			"the adjustment of the velocity is absolutely essential for all spatial movements. By activating the AUTO function, your creature will adjusts " +
			"its velocity according to the given target. The NEG flag allows you to use negative velocity values. Make sure that the velocity values are " +
			"suitable to the defined animation, otherwise your creature will do a moonwalk and please consider, that a zero value means no move. \n\n" +
			"Please note: When using root motions, the the forward speed will be controlled by the animation, therefore the associated control element is disabled.";
		public static readonly string BEHAVIOUR_MOVE_VELOCITY_SIDEWARDS = "";
		public static readonly string BEHAVIOUR_MOVE_VELOCITY_DRIFT = "";
		public static readonly string BEHAVIOUR_MOVE_VELOCITY_VERTICAL = "If vertical movements enabled you can use the Vertical Velocity to define the desired " +
			"speed along the y-axis. Your creature will use this velocity to reach its given altitude (Operating Level).";
		public static readonly string BEHAVIOUR_MOVE_ALTITUDE = "In addition to the Vertical Velocity you have to define also the desired operating level " +
			"which represents the desired altitude your creature will try to reach. If you define different minimum and maximum values, the final altitude " +
			"will be randomized if the rule becomes active. By default the altitude represents the absolute level above zero, by activating the ‘GND’ " +
			"button (Ground) the creature will use the altitude above the given ground level and will follow the contour map.";

		public static readonly string BEHAVIOUR_MOVE_VELOCITY_VARIANCE = "Use the Velocity Variance Multiplier to randomize the Forward Velocity Vector " +
			"during the runtime, to force non-uniform movements of your creature (this will be helpful while using several instances of your creature)";

		public static readonly string BEHAVIOUR_MOVE_VELOCITY_INERTIA = "The Inertia value will be used to simulate the mass inertia to avoid abrupt " +
			"movements while the speed value changed.";

		public static readonly string BEHAVIOUR_MOVE_VIEWING_DIRECTION = "Viewing Direction defines the direction your creature will look at while the " +
			"behaviour is active. By default your creature will look at the move direction, but in some cases it can be helpful to force a specific direction " +
			"independent of the move direction.";

		public static readonly string BEHAVIOUR_MOVE_ANGULAR_VELOCITY = "Angular Velocity defines the desired rotational speed of your creature around its " +
			"y axis. This value affects the turning radius of your creature – the smaller the value, the larger the radius and vice versa. For a realistic " +
			"behaviour, this value should be consider the given physical facts and therefore suitable to the specified speed and the naturally to the animation " +
			"and the kind of creature as well.";

		public static readonly string BEHAVIOUR_MOVE_DEFAULT = "By default ICECreatureControl will use the Default Move for all standard situations, which describes " +
			"a direct manoeuvre form the current transform position to the TargetMovePosition. This manoeuvre will be sufficient in the majority of cases, but " +
			"it is less helpful if your creature have to veer away from a target, such as in an escape situation.";

		public static readonly string BEHAVIOUR_MOVE_RANDOM = "";
		public static readonly string BEHAVIOUR_MOVE_ORBIT = "Orbit Move defines an orbital move around the TargetMovePosition. You can adjust the initial " +
			"radius, a positive or negative shift value, so that your creature will following a spirally path and the associated minimum and maximum distances, " +
			"which specifies the end of the move. Please consider, that an orbital move with a zero shift value will not have a logical end, so you should make " +
			"sure that your creature will be not circling around the target infinitely. You could do this, for example, by setting a limited play length of the rule.";
		public static readonly string BEHAVIOUR_MOVE_DETOUR = "";
		public static readonly string BEHAVIOUR_MOVE_COVER = "";
		public static readonly string BEHAVIOUR_MOVE_ESCAPE = "By using the Escape Move, your creature will move away from the target in the opposite direction of " +
			"the initial sighting line. You can randomize this escape direction by adapt the RandomEscapeAngle. The EscapeDistance defines the desired move distance, " +
			"which will added to the given SelectionRange of the target. Please consider, that you can affect the Escape behaviour by adjust the angular restriction " +
			"settings of the Target Selection Criteria and/or the Field Of View of your creature.";
		public static readonly string BEHAVIOUR_MOVE_ESCAPE_DISTANCE = "";
		public static readonly string BEHAVIOUR_MOVE_ESCAPE_ANGLE = "";
		public static readonly string BEHAVIOUR_MOVE_CUSTOM = "";
		public static readonly string BEHAVIOUR_MOVE_AVOID = "By using the Avoid Move, your creature will try to avoid the target by moving to the side, left or right " +
			"being based on the initial sighting line. Please consider, that you can affect the Avoid behaviour by adjust the angular restriction settings of the Target " +
			"Selection Criteria and/or the Field Of View of your creature.";
		public static readonly string BEHAVIOUR_MOVE_AVOID_DISTANCE = "";

		//public static readonly string REGISTER = "TODO";
		public static readonly string REGISTER_OPTIONS = "Options contains several optional features which could be helpful to you to organize your project and to reach the " +
			"desired goals without custom scripts, but in any case you are free to implement also your own solutions to handle these functions.";
		public static readonly string REGISTER_OPTIONS_DEFAULT = "";
		public static readonly string REGISTER_OPTIONS_GROUPS = "By using the Hierarchy Management the ICECreatureRegister makes sure that your scene stay clean and tidy " +
			"during the runtime. While the Hierarchy Management is enabled all spawned Objects will be sorted according to the given structure. You are free to modify the given " +
			"structure as desired to adapt it to your project. ";
		public static readonly string REGISTER_OPTIONS_GROUPS_ROOT = "By default the root node will be the CreatureRegister element but you can define also your own object " +
			"or deactivate this node to arrange all groups to the top level of your scene hierarchy.";
		public static readonly string REGISTER_OPTIONS_GROUPS_CREATURES = "The Creatures node contains all GameObjects who using the ICECreatureControl script for their " +
			"registration (see also ICECreatureControl).";
		public static readonly string REGISTER_OPTIONS_GROUPS_ITEMS = "The Items node contains all GameObjects who using the ICECreatureItem script for their registration " +
			"(see also ICECreatureItem).";
		public static readonly string REGISTER_OPTIONS_GROUPS_LOCATIONS = "The Locations node contains all GameObjects who using the ICECreatureLocation script for their " +
			"registration (see also ICECreatureLocation).";
		public static readonly string REGISTER_OPTIONS_GROUPS_WAYPOINTS = "The Waypoints node contains all GameObjects who using the ICECreatureWaypoint script for their " +
			"registration (see also ICECreatureWaypoint).";
		public static readonly string REGISTER_OPTIONS_GROUPS_MARKERS = "The Markers node contains all GameObjects who using the ICECreatureMarker script for their registration " +
			"(see also ICECreatureMarker).";
		public static readonly string REGISTER_OPTIONS_GROUPS_TARGETS = "The Targets node contains all GameObjects who using the ICECreatureTarget script for their registration " +
			"(see also ICECreatureTarget).";
		public static readonly string REGISTER_OPTIONS_GROUPS_OTHER = "The Other node contains all other GameObjects without specific ICE scripts.";
		public static readonly string REGISTER_OPTIONS_GROUPS_PLAYER = "The Players node contains all GameObjects who using the ICECreaturePlayer script for their registration " +
			"(see also ICECreaturePlayer).";

		public static readonly string REGISTER_OPTIONS_POOL_MANAGEMENT = "By using the Pool Management the Register can handle the population of your creatures and all your other " +
			"related objects such as locations, waypoints and items etc. as well. While UsePoolManagement is flagged you can activate the POOL functions for each reference " +
			"object to define the desired spawn and respawn settings. The Pool Management is an optional feature, you are free to handle it also by your own scripts or " +
			"third party products. ";
		public static readonly string REGISTER_OPTIONS_POOL_MANAGEMENT_GROUND_CHECK = "Use Ground Check to define how the ground level will be detected during a spawning " +
			"process. ";
		public static readonly string REGISTER_OPTIONS_POOL_MANAGEMENT_WATER_CHECK = "Use Water Check to define how the water areas will be detected during a spawning " +
			"process. ";
		public static readonly string REGISTER_OPTIONS_POOL_MANAGEMENT_OBSTACLE_CHECK = "Use Obstacle Check to define the obstacle layers which should be avoided during a " +
			"spawning process. ";
		public static readonly string REGISTER_OPTIONS_POOL_MANAGEMENT_COROUTINE = "Runs the respan procedures of the pool management within a coroutine";
		public static readonly string REGISTER_OPTIONS_POOL_MANAGEMENT_INTERVAL = "The 'Default Spawn Interval' defines the time interval in seconds, in which the pool " +
			"management triggers the spawning routines of all enabled reference groups to create new objects. By default the interval is adjusted to 0.5 secs. If the interval " +
			"is adjusted to zero the spawning routines will be called in each frame, but especially while using the Pool Management with a larg number of groups and objects it " +
			"is adviced to increase the respawn interval. Please note that deactivating the pool management also deactivates the option 'Default Spawn Interval'.";

		public static readonly string REGISTER_OPTIONS_SCENE_MANAGEMENT = "";
		public static readonly string REGISTER_OPTIONS_DONTDESTROYONLOAD = "While DontDestroyOnLoad is flagged the CreatureRegister will not be destroyed automatically when " +
			"loading a new scene.\n\nWhen loading a new level all objects in the scene are destroyed, then the objects in the new level are loaded. In order to preserve an " +
			"object during level loading call DontDestroyOnLoad on it. If the object is a component or game object then its entire transform hierarchy will not be destroyed " +
			"either.";
		public static readonly string REGISTER_OPTIONS_CUSTOM_GARBAGE_COLLECTION = "Beside of the Automatic Memory Management, it can be advantageous in some cases to request " +
			"a garbage collection at a regular frame interval. This will generally make collections happen more often than strictly necessary but they will be processed quickly " +
			"and with minimal effect on gameplay. The 'Custom Garbage Collection' feature allows you to run such collects according to the specified interval. You can use the " +
			"profiler to adapt the best interval for your scenario.";
		public static readonly string REGISTER_OPTIONS_RANDOMSEED = "Random Seed defines the seed for the random number generator." +
			"The random number generator is not truly random but produces numbers in a preset sequence (the values in the sequence " +
			"'jump' around the range in such a way that they appear random for most purposes)." +
			"The point in the sequence where a particular run of pseudo-random values begins is selected using an integer called the " +
			"seed value. The seed is normally set from some arbitrary value like the system clock before the random number functions are " +
			"used. This prevents the same run of values from occurring each time a game is played and thus avoids predictable gameplay. " +
			"However, it is sometimes useful to produce the same run of pseudo-random values on demand by setting the seed yourself.";
		public static readonly string REGISTER_OPTIONS_RANDOMSEED_CUSTOM = "You might set your own seed to make sure that the same 'random'" +
			"pattern is produced each time the game is played.";

		public static readonly string REGISTER_OPTIONS_DEBUG = "Activate the Debug feature to show the Reference and SpawnPoint Gizmos.";
		public static readonly string REGISTER_OPTIONS_DEBUG_GIZMOS_MODE = "While ‘Draw Selected Only’ is flagged the Gizmos will be only " +
			"drawn when their GameObjects are selected.";
		public static readonly string REGISTER_OPTIONS_DEBUG_REFERENCES = "Use the Reference settings to adapt the colour of the REFERENCE " +
			"gizmos, also you can activate/deactivate TEXT to display or hide the description and use ENABLED to activate or deactivate " +
			"the REFERENCE gizmos.";
		public static readonly string REGISTER_OPTIONS_DEBUG_CLONES = "Use the Clones settings to adapt the colour of the CLONE gizmos, " +
			"also you can activate/deactivate TEXT to display or hide the description and use ENABLED to activate or deactivate the CLONE " +
			"gizmos.";
		public static readonly string REGISTER_OPTIONS_DEBUG_SPAWNPOINTS = "Use the SpawnPoints settings to adapt the colour of the SPAWNPOINT " +
			"gizmos, also you can activate/deactivate TEXT to display or hide the description and use ENABLED to activate or deactivate " +
			"the SPAWNPOINT gizmos.";

		public static readonly string REGISTER_OPTIONS_DEBUG_PLAYERS = "";

		public static readonly string REGISTER_REFERENCE_OBJECTS = "Reference Objects represents a list with all different types of GameObjects " +
			"your creatures should interact with it during the runtime (e.g. your Player, other creatures and NPCs, locations and waypoints, loot " +
			"items etc.). In Editor Mode this list provides a Popup with all object names while using the target access by name, also you can use the " +
			"internal pool management of the register to adapt the spawning and population management. During the runtime the list contains all spawned " +
			"objects, provides a quick and performance friendly access to the superior groups and also to each single element, handles the population " +
			"management and the communication between groups and objects. Therefore you should add at least one reference object of each GameObject " +
			"which you want to use as potential target or interaction object. Basically you can add each desired GameObject (scene objects or prefabs) " +
			"as a reference, which will be listed also as potential target in the selection popup while using the target access by name but please " +
			"consider that all objects have to handle their registration and deregistration during the runtime alone, otherwise it could be that a target " +
			"will not detect correctly and your creatures will ignore it. ICE provides several target scripts (e.g. Player, Location, Waypoints, Marker " +
			"and Items etc.) to handle these registration procedures and you should classify all unknown objects by assigning one of these scripts to your " +
			"object according to its function (Tip: you can use the 'C' buttons to add or remove the desired script)";

		public static readonly string REGISTER_REFERENCE_CAT = "The ICE World is subdivided into various entity groups with specific characteristics. To " +
			"get a better overview in the register, all reference objects can be sorted according to their entity groups.";

		public static readonly string REGISTER_REFERENCE_OBJECTS_ADD = "Use ‘Add Reference Object’ to add a new reference object to the register. Each " +
			"Reference Object represents a group of objects with the same characteristics, such as a specific species or item classes etc.";

		public static readonly string REGISTER_REFERENCE_OBJECT = "The ‘Reference Object’ represents the prototype and will be used as original for all runtime " +
			"initialized clones. The ‘Reference Object’ should be a Prefab, to make sure that it will not be destroyed during the runtime but you are free to use " +
			"also scene objects, but please consider that this could trigger a couple of problems if it got lost during the runtime.";
		public static readonly string REGISTER_REFERENCE_OBJECT_GROUP = "A ‘Reference Object Group’ represents a group of objects with the same characteristics, such " +
			"as a specific species, an item class or a location etc. which should be used as potential target during the runtime. Basically a Reference Object Group " +
			"based on a single reference object which will be used as original for all runtime initialized clones. Such a reference object could be each GameObject " +
			"but the ICE framework provides several types of harmonised objects to increase the functionality and to optimize the interplay but also to simplify the " +
			"usability.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SPAWN_MAX = "Adapt 'Max. Spawn Objects’ to defines the maximum number of objects which should be " +
			"spawn during the runtime. Activate ‘INITIAL’ to spawn all objects on start, otherwise they will spawn according to the given spawn interval.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SPAWN_INTERVAL = "Adapt the ‘Spawn Interval’ values to define the minimum and maximum range in " +
			"seconds in which the objects should be spawn.";// Please note, whenever the ‘Spawn Interval’ is adjusted to zero it could be that ";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SPAWN_WAVE = "Adapt the ‘Wave Size’ to define the minimum and maximum amount of spawn cycles per wave.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SPAWN_PRIORITY = "If INITIAL is flagged you can adjust the ‘Spawn Priority’ to specify in which " +
			"order the given reference groups should spawn their clones. This could be important to make sure that locations and waypoints are already exists before " +
			"spawning your creatures.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_RANDOM_SIZE = "To force a more natural scenario you can activate ‘Random Size’ " +
			"to randomize the size of your initialized objects. ";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_RANDOM_SIZE_VARIANCE = "Use ‘Size Variance’ to define the minimum and maximum limits.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SOFTRESPAWN = "Activate ‘Soft Respawn’ to reuse already initialized objects " +
			"during the runtime. If ‘Soft Respawn’ is flagged already initialized but unused objects will be reset and reused without destroying " +
			"and creating new instances. ";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_GROUP_USE = "Activate ‘Use Hierarchy Group’ to use an own group node for " +
			"all spawned instances of the reference object.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_GROUP_CUSTOM = "By default the Hierarchy Group Node will be a child of the " +
			"given reference node, but you can also define a custom GameObject as group node, in this case the instances will only initialized if " +
			"the defined node is active and available inside your scene and they will be also hidden or removed if the custom group node will be " +
			"deleted or deactivated. You can use this feature for your zone management to steering the visibility of initialized items, just by " +
			"activate or deactivate or load or unload the custom group node.";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_GROUP_INFO = "While using the GROUP option without a custom group object the register " +
			"will use the default group.";

		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SPAWN_CONDITIONS = "TODO ";
		public static readonly string REGISTER_REFERENCE_OBJECT_POOL_SPAWN_CONDITIONS_MAIN_CAMERA_DISTANCE = "Main Camera Distances defines the required minimum " +
			"and maximum distances in relation of the viewing direction of the current main camera, so positive values represents the distance within the given field " +
			"of view, while negative values defines the distance within invisible areas behind the camera.";

		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINTS = "SpawnPoint defines the centre of the spawn area, which could be adapt by " +
			"modifying the Random Range values. By default your creatures will using its HOME location as spawn point, but basically a SpawnPoint could also " +
			"be each static or movable GameObject (e.g. a static building or your player as well).";
		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINT = "SpawnPoint defines the centre of the spawn area, which could be adapt by " +
			"modifying the Random Range values. By default your creatures will using its HOME location as spawn point, but basically a SpawnPoint could also " +
			"be each static or movable GameObject (e.g. a static building or your player as well).";
		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINT_REFERENCE = "SpawnPoint defines the centre of the spawn area, which could be adapt by " +
			"modifying the Random Range values. By default your creatures will using its HOME location as spawn point, but basically a SpawnPoint could also " +
			"be each static or movable GameObject (e.g. a static building or your player as well).";
		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINT_RANGE = "RandomRange could be used to define a minimum and maximum radius around " +
			"the given spawn point to modifify the final spawn area. If both RandomRange values (minimum/maximum) are adjusted to zero the spawn position will " +
			"be the transform position of the given spawn point.";
		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINT_RECT = "Random Rect could be used to define a rectable area around " +
			"the given spawn point to modifify the final spawn area.";
		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINT_LEVEL = "LevelDifference defines the maximum altitude difference between " +
			"the centre position and all potential spawn points within the given Random Range. Centre Position (y) + Level Difference will be the origin " +
			"of the ray to detect the correct ground level, so LevelDifference will be only visible if the GroundCheckType is adjusted to RAYCAST.\n\n" +
			"Tip: If a spawn point is within a covered area, with one or more level, such as a cave or another closed room, the level difference should be lower " +
			"than the height of the room, otherwise it could be that the object will be spawn on the wrong level.";
		public static readonly string REGISTER_REFERENCE_OBJECT_SPAWN_POINT_LEVEL_OFFSET = "Level Offset defines an addtional vertical offset which will be used for" +
			"the spawning process.";

		// ################################################################################
		// LOCATION
		// ################################################################################
		public static readonly string LOCATION_NAME = "";
		public static readonly string ITEM_NAME = "";




		public static readonly string LOOK_INVISIBILITY = "";
		public static readonly string LOOK_INVISIBILITY_TYPE = "";

		public static readonly string ATTRIBUTE_ADD = "Attributes represents components which provides an entity additional features.";

		public static readonly string SENSORIA = "Sensoria represents the perceptive capabilities of your creatures. If you activate this " +
			"feature your creature can use its senses to explore its surrounding environment or rather to detect relevant targets. Please note, " +
			"if Sensoria is disabled, the Sensoria features of the Target Selection Criteria will be disabled as well.";

		public static readonly string SENSORIA_FOV = "The Field Of View represents the maximum horizontal angle your creature can sense the surrounding environment. " +
			"By default this value is adjusted to zero, which suspends the FOV restrictions and allows sensing in 360 degrees, alternative you could set the " +
			"value also directly to 360 degrees, this will have the same effect except that the FOV still active and you can see the FOV gizmos. Please note, " +
			"that the FOV settings will not automatically use to sense (select) a target. You have to activate the FOV flag of the Target Selection Criteria " +
			"to use this feature. That’s because to provide more flexibility.";
		public static readonly string SENSORIA_FOV_VISUAL_RANGE = "Visual Range defines the maximum sighting distance of your creature. By adjusting this value to zero, " +
			"the sighting distance will be infinite.";
		public static readonly string SENSORIA_FOV_VISUAL_OFFSET = "The Visual Sensor Position defines the origin of the visual field or with other word the eye " +
			"position. You can define a fixed offset position (relating to your creatures transform position) or you could activate the DYN button to use the local " +
			"position of a subordinated transform (e.g. head or eyes). In this cases the Visual Sensor Position will be bounded to the specified transform and will " +
			"follow all movements automatically.";
		public static readonly string SENSORIA_FOV_VISUAL_HORIZONTAL_OFFSET = "While the Visual Sensor Position defines an exact point as origin of the visual field, " +
			"the Visual Horizontal Offset defines the radius of an additional circular range around this point, to relocate the origin out of a critical range and to " +
			"avoid conflicts with additional child objects or rather their colliders (e.g. helmet or other additional outfits), which could mask or interfere the visual " +
			"field.";
		public static readonly string SENSORIA_USE_SPHERECAST = "If 'Use Spherical Cast' is activated, the Visibility " +
			"Check is performed with a SphereCast instead of a LineCast. Radius defines the radius of the spherical cast " +
			"and thus the minimum size of the visible area.";

		public static readonly string SENSORIA_ATTRIBUTES = "Sensoria attributes represents the general perceptive capabilities of your creatures. By default all values are " +
			"adjusted to 100 % but you can adapt the values manual to restrict specific senses. Also you can adjust the multiplier to restrict senses dynamically during " +
			"the runtime.";
		public static readonly string SENSORIA_ATTRIBUTES_AUDITORY = "";
		public static readonly string SENSORIA_ATTRIBUTES_VISUAL = "";
		public static readonly string SENSORIA_ATTRIBUTES_OLFACTORY = "";
		public static readonly string SENSORIA_ATTRIBUTES_GUSTATORY = "";
		public static readonly string SENSORIA_ATTRIBUTES_TACTILE = "";

		public static readonly string LASER = "TODO";
		public static readonly string FLASHLIGHT = "TODO";

		public static readonly string FIRE = "TODO";
		public static readonly string FIRE_LIGHT = "Light object which represents the light of the fire.";
		public static readonly string FIRE_INTENSITY = "Random min/max intensity range of the fire light.";

		public static readonly string TURRET = "TODO";
		public static readonly string TURRET_SCAN_RANGE = "TODO";
		public static readonly string TURRET_ANGULAR_DEVIATION = "TODO";
		public static readonly string TURRET_VERTICAL_ADJUSTMENT = "TODO";

		public static readonly string TURRET_PIVOT_POINT = "TODO";
		public static readonly string TURRET_PIVOT_AXIS_YAW = "TODO";
		public static readonly string TURRET_PIVOT_AXIS_PITCH = "TODO";
		public static readonly string PRIMARY_WEAPON = "TODO";
		public static readonly string SECONDARY_WEAPON = "TODO";

		public static readonly string MELEE_WEAPON = "TODO";



		public static readonly string RANGED_WEAPON = "TODO";

		public static readonly string RANGED_WEAPON_MUZZLE_FLASH = "TODO";
		public static readonly string RANGED_WEAPON_MUZZLE_FLASH_SCALE = "TODO";

		public static readonly string RANGED_WEAPON_AMMO = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_TYPE = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_DAMAGE_METHOD = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_DAMAGE_VALUE = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_DAMAGE_FORCE = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_PROJECTILE = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_PROJECTILE_SCALE = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_PROJECTILE_SPAWN_POINT = "TODO";
		public static readonly string RANGED_WEAPON_AMMO_PROJECTILE_MUZZLE_VELOVITY = "TODO";

		public static readonly string RANGED_WEAPON_SHELL = "TODO";
		public static readonly string RANGED_WEAPON_SHELL_REFERENCE = "TODO";
		public static readonly string RANGED_WEAPON_SHELL_SPAWNPOINT = "TODO";

		public static readonly string PLAYER_PLAYER = "TODO";
		public static readonly string PLAYER_PLAYER_DEATHCAM = "TODO";
		public static readonly string PLAYER_PLAYER_USEMOUSEPOSITIONTOAIM = "TODO";
		public static readonly string PLAYER_EVENTS = "TODO";
		public static readonly string PLAYER_EVENT = "TODO";
		public static readonly string PLAYER_EVENT_INPUT = "TODO";

		public static readonly string INVENTORY = "The Inventory represents a list of items your creature (but also other ICE objects) can have with it. You can " +
			"define an empty list, your creature can fill during the runtime or you can also define default items, your creature can lose while looting by another creature " +
			"or your player but also while distribute items (e.g. sow seed or deliver a newspaper or pizza etc.)";
		public static readonly string INVENTORY_SLOTS = "Slots represents repositories to store items. To adapt the desired slots just set the maximum number and increase or " +
			"decrease the slider or press the CLR Button to remove all slots.";
		public static readonly string INVENTORY_SLOT = "A slot represents a repository for an item type. By default a slot and especially the deposited items are " +
			"virtual constructs without existing GameObjects, in this case the given amount is just a theoretical value which represents imaginary items. But you can also " +
			"specify a parent object by using the Slot Popup which represents the creatures’ hierarchy, so that your creature can use the defined parent as a real handle " +
			"(e.g. hand, holster, chest holder etc.) and as long as the item amount is larger than zero the defined item will be represented as an instantiated object, " +
			"assigned to the given handle and visible and detectable for other creatures. This allows you to equip or re-equip your creature during the runtime by using " +
			"the inventory settings of the behaviour rules. ";
		public static readonly string INVENTORY_SLOT_ITEM = "Item represents the type of a stored object. Basically an item can be each GameObject or Prefab but in " +
			"each case it must be listed as reference in the Creature Register and requires the ICECreatureItem script, if both is given the item will be listed in the Item " +
			"Popup. Furthermore you can activate the EXCL Button, to mark the item as exclusive and reserved for the given slot. By default EXCL is deactivated and the slot " +
			"is open for each kind of items. If you assign now an item to such an open slot the amount will automatically increase to 1 and the slot will be open again if " +
			"the amount will reset to zero. By activate the exclusive flag the slot stays in each case reserved for the assigned item type independent of the amount. If the " +
			"DOD Button is flagged all available items will be dropped in cases the inventory owner dies or its object will be destroyed.";
		public static readonly string INVENTORY_SLOT_AMOUNT = "Amount represents the current number of items. You can adjust the maximum number to restrict the " +
			"capacity. ";
		public static readonly string INVENTORY_SLOT_ITEM_DROP_RANGE = "Radius around the creature the detached item will be droped";
		public static readonly string INVENTORY_DROP_RANGE = "Default radius around the creature detached items will be droped";


		public static readonly string DEFAULT = "";

		// ################################################################################
		// BODYPARTMOTION
		// ################################################################################

		public static readonly string BODYPARTMOTION = "TODO";
		public static readonly string BODYPARTMOTION_ROTATIONSPEED = "TODO";
		public static readonly string BODYPARTMOTION_USERELATIVETOROOT = "TODO";



	}

}

