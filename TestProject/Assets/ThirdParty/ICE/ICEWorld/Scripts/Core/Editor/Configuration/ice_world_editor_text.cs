// ##############################################################################
//
// ice_world_editor_text.cs | Info
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

namespace ICE.World.EditorInfos
{
	public class Info : ICEEditorInfo
	{
		public static string Version = "1.4.0";

		public static readonly string BASE_OFFSET = "Base Offset defines the relative vertical displacement of the owning GameObject.";

		public static readonly string ANIMATION = "Here you can define the desired animation you want use for the selected rule. " +
			"Simply choose the desired type and adapt the required settings.";

		public static readonly string ANIMATION_NAME = "TODO";
		public static readonly string ANIMATION_WRAP_MODE = "TODO";
		public static readonly string ANIMATION_SPEED = "TODO";
		public static readonly string ANIMATION_TRANSITION = "TODO";
		public static readonly string ANIMATION_NONE = "Animations are optional and not obligatory required, so you can " +
			"control also each kind of unanimated objects, such as dummies for testing and prototyping, simple bots and turrets or " +
			"movable waypoints.  ";
		public static readonly string ANIMATION_ANIMATOR = "By choosing the ANIMATOR ICECreatureControl will using the Animator " +
			"Interface to control Unity’s powerful Mecanim animation system. To facilitate setup and handling, ICECreatureControl provide " +
			"three different options to working with Mecanim: \n\n" +
			" - DIRECT – similar to the legacy animation handling \n" +
			" - CONDITIONS – triggering by specified values (float, integer, Boolean and trigger) \n" +
			" - ADVANCED – similar to CONDITIONS with additional settings for IK (ALPHA) \n";

		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_DIRECT = "DIRECT - similar to the legacy animation handling";
		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_CONDITIONS = "CONDITIONS – triggering by specified values (float, integer, Boolean and trigger)";
		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED = "ADVANCED – similar to CONDITIONS with additional settings for IK (ALPHA)";
		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED_INFO = "COMING SOON!";
		public static readonly string ANIMATION_ANIMATOR_POPUP = "Use the Animation Popup to select the desired animation, or use the arrow keys to navigate to the " +
			"previous or next animation. With the SEL (Select) button you can open the settings of the animation. Please note: in cases the selected animation is " +
			"stored as a child-object of a model, you need to open the Animation Import Settings of the model to modify the settings. In order to avoid problems with " +
			"incorrect adapted animation, it is recommended to check and adjust all motions and settings at an early stage.";
		public static readonly string ANIMATION_ANIMATOR_WRAPMODE = "";

		public static readonly string ANIMATION_ANIMATOR_ERROR_NO_CLIPS = "There are no clips available. Please check your Animator Controller!";
		public static readonly string ANIMATION_ANIMATION = "Working with legacy animations is the easiest and fastest way " +
			"to get nice-looking results. Simply select the desired animation, set the correct WrapMode and go.";
		public static readonly string ANIMATION_CLIP = "The direct use of animation clips is inadvisable and here only " +
			"implemented for the sake of completeness and for some single cases it could be helpful to have it. But apart from that it " +
			"works like the animation list. Simply assign the desired animation clip, set the correct WrapMode and go.";
		public static readonly string ANIMATION_CUSTOM = "While using CUSTOM animation you could handle the desired animation for this behaviour rule by your own " +
			"script. OnCustomAnimation and OnCustomAnimationUpdate ";
		public static readonly string ANIMATION_EVENTS = "AnimationEvent lets you call a script function similar to SendMessage as part of playing back an " +
			"animation. Animation events support functions that take zero or one parameter. The parameter can be a float, an integer or a string. In cases " +
			"you would like to use also events with an object reference or an AnimationEvent you have to define such events directly in the Animation Window. \n\n" +
			"Please note that Animation Events calls their methods on MonoBehaviours of the ‘animated’ GameObject only, if you want to call a method within " +
			"one of its children you could use the Methods feature instead. Also please consider that AnimationEvents will be assigned directly to the " +
			"associated animation and will try to call their defined methods on each GameObject which used these animation, so please be careful by " +
			"defining such events.";
		public static readonly string ANIMATION_EVENTS_METHOD = "Use the Event section to define as much events as you want. For this simply enable the " +
			"section and ADD a new event. Disabling the section will deactivate all listed events, which means that all defined events will be activated in " +
			"the list and removed from the animation. To reset the complete Event section press the RES button, this removes all listed events completely.";
		public static readonly string ANIMATION_EVENTS_METHOD_POPUP = "The Event Popup provides you a preselected list with available methods. These methods " +
			"represents registered PublicMethods which are directly related to the game play and suitable to steering movements and/or behaviour (see also: ICE World, PublicMethods).\n\n" +
			"In addition to the listed methods, you can activate CUSTOM to enter arbitrary function names, in doing so you can define each available function you want. \n\n" +
			"By default a new created event will be inactive and not assigned to the animation, so you have to press ACTIVE first to assign an event to the selected " +
			"animation, also deactivate the ACTION flag to remove an event from the animation. To remove obsolete events completely from list press " +
			"the X button, this will removes both the listed event template and the assigned AnimationEvent.";
		public static readonly string ANIMATION_EVENTS_METHOD_TIME = "The time at which the event will be fired off.";


		public static readonly string AUDIO = "TODO";
		public static readonly string AUDIO_CLIP = "TODO";
		public static readonly string AUDIO_CLIP_ADD = "TODO";
		public static readonly string AUDIO_PITCH = "Define the lowest and highest pitch to use a randomized pitch during the runtime.";
		public static readonly string AUDIO_VOLUME = "TODO";
		public static readonly string AUDIO_DISTANCE = "Within the Min distance the AudioSource will cease to grow louder in volume. Outside the min distance " +
			"the volume starts to attenuate and dependent to the RolloffMode, the MaxDistance is the distance where the sound is completely inaudible.";
		public static readonly string AUDIO_BREAK = "Check BREAK to interrupt an playing audio file to start the current file again or to change the clip while using multiple audio clips.";
		public static readonly string AUDIO_STOP = "Check STOP to interrupt an playing audio at the end of an action.";
		public static readonly string AUDIO_LOOP = "TODO";
		public static readonly string AUDIO_ROLLOFF = "TODO";
		public static readonly string AUDIO_MIXER_GROUP = "TODO";

		public static readonly string ODOUR = "Odour represents volatilized chemical compounds that other creature could perceive by their sense " +
			"of olfaction.";
		public static readonly string ODOUR_INTENSITY = "";
		public static readonly string ODOUR_RANGE = "";
		public static readonly string ODOUR_MARKER = "";
		public static readonly string ODOUR_MARKER_INTERVAL = "";
		public static readonly string ODOUR_EFFECT = "";

		public static readonly string CORPSE = "If Use Corpse is flagged you can assign a GameObject which will be used if your creature dies " +
			"(e.g. a Ragdoll Object of your creature which will be used instead of the original model). The corpse object have to be a prefab which will be " +
			"instantiate automatically if your creature dies.";
		public static readonly string CORPSE_REFERENCE = "The Corpse Prefab defines the prefab which will be used to replace the original object. Activate SCALE to allow" +
			"scaling the corpse according to the original object, but please note that this could results funny effects while scaling ragdolls.";
		public static readonly string CORPSE_REMOVING_DELAY = "Corpse Removing Delay defines the delay time in seconds until the spawned corpse object will " +
			"be removed from scene. You can adjust this value to zero to handle the removing process by using external scripts or to keep the spawned corpse durable " +
			"into your scene.";
		public static readonly string CORPSE_REMOVING_DELAY_VARIANCE = "The Variance Multiplier defines the threshold variance value, which will be used to " +
			"randomize the associated delay time during the runtime.";
		
		public static readonly string INPUT_POPUP = "TODO";

		public static readonly string EVENTS = "TODO";
		public static readonly string EVENT = "TODO";
		public static readonly string EVENT_POPUP = "The Event Popup displays all available Behaviour Events ";
		public static readonly string EVENT_PARAMETER_BOOLEAN = "Boolean parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_INTEGER = "Int parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_FLOAT = "Float parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_STRING = "String parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_OBJECT = "Object parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_NAME = "TODO";

		public static readonly string EFFECT = "TODO";
		public static readonly string EFFECT_REFERENCE = "TODO";
		public static readonly string EFFECT_MOUNTPOINT = "TODO";
		public static readonly string EFFECT_OFFSET_TYPE = "TODO";
		public static readonly string EFFECT_OFFSET_POSITION = "TODO";
		public static readonly string EFFECT_OFFSET_RADIUS = "TODO";
		public static readonly string EFFECT_DESTROY_DELAY = "TODO";

		public static readonly string STATUS_MASS = "TODO";

		public static readonly string IMPULSE_TIMER = "TODO";
		public static readonly string IMPULSE_TIMER_TIME = "The Start Time defines the timespan in seconds from the initial activation of a feature " +
			"to firing the first impulse. If Start Time is adjusted to zero, the impulse will be fired directly at the activation of a feature or rather " +
			"while END button is flagged at the end of an active feature. In both cases there will be a single impulse only. If you want more than one " +
			"impulse you can activate the Interval Function by pressing the INT button. By using the TRG button the timer can be triggered by an external " +
			"event, such as an AnimationEvent etc.";
		public static readonly string IMPULSE_TIMER_INTERVAL = "The Impulse Interval defines the minimum and maximum timespan between two impulses. If " +
			"minimum and maximum are different, the interval will be randomized within the specified range, otherwise the interval will be used as defined. " +
			"You could use the RND button to generate randomized settings or the D button to adjust the values back to zero. Please note, if the Impulse " +
			"Interval is adjusted to zero, the impulse will be fired in each frame. ";
		public static readonly string IMPULSE_TIMER_LIMITS = "The Impulse Limit defines the maximum number of impulses. If you define different minimum and " +
			"maximum values the limit will be randomized, otherwise the limit will be used as defined. You could use the RND button to generate randomized " +
			"values or the D button to adjust the values back to zero. Adjusting the limit to zero will disable the limit.";
		public static readonly string IMPULSE_TIMER_SEQUENCE_LIMITS = "While the Impulse Limit defines the total number of impulses, the Sequence Limit " +
			"specifies the number of sequenced impulses until the next break. Sequence Limit will be available only as long as the Impulse Interval is not " +
			"adjusted to zero. If you define different minimum and maximum values the limit will be randomized, otherwise the limit will be used as defined. " +
			"You could use the RND button to generate randomized values or the D button to adjust the values back to zero. Adjusting the limit to zero will " +
			"disable the Sequence Limit. Please consider to adjust also the Break Length if you define the Sequence Limit, otherwise the Sequence Limit will " +
			"not work as expected if the Break Length is adjusted to zero.";
		public static readonly string IMPULSE_TIMER_SEQUENCE_BREAK_LENGTH = "While the Sequence Limit defines the number of sequenced impulses until the next " +
			"break, the Break Length defines the timespan in seconds of the subsequent interruption. The Break Length option will be available only if Sequence " +
			"Limit is not adjusted to zero. If you define different minimum and maximum values the timespan will be randomized, otherwise the Break Length will " +
			"be used as defined. You could use the RND button to generate randomized values or the D button to adjust the values back to zero. Adjusting the " +
			"Break Length to zero will disable this option. ";

		public static readonly string DURABILITY_INITIAL = "Initial Durability defines the fundamental capability of resistance of a creature in terms of " +
			"physical integrity and its vital fitness. The durability will be affected by several influences (e.g. damage, age etc. ) during the runtime and the " +
			"creature will die as soon as its durability is exhausted. By default the Initial Durability is adjusted to 100 but you are free to define a suitable " +
			"value according to your needs and requirements; the lower the value, the greater the impact of several influences and vice versa (e.g. increase this " +
			"value to 1000 for your level boss or decrease it to 10 for homely antagonists). Please note that changing the durability value is ineffective while " +
			"using influence values in percent, in such a case a damage of 50% for example will also reduce the durability value to 50%, independent of the defined " +
			"initial durability value. \n\nThe Initial Durability based on a minimum and maximum value, which allows you to define a random range. If you prefer to " +
			"define a fixed value, simply set minimum and maximum to the same value, also you can adapt the third field to modify the range of the slider. ";

		public static readonly string BODYPART = "TODO";
		public static readonly string BODYPART_DAMAGE_TRANSFER = "TODO";

		public static readonly string DURABILITY_PERCENT = "TODO";

		public static readonly string RIGIDBODY_AND_COLLIDER = "This kind of entity represents a moveable and collectable object, therefore it’s advisable to add a " +
			"Rigidbody and a Collider to it.";
		public static readonly string TRIGGER_COLLIDER = "This kind of entity requires a trigger collider.";

		public static readonly string RIGIDBODY = "TODO";
		public static readonly string RIGIDBODY_INFO = "TODO";

		public static readonly string COLLIDER = "TODO";
		public static readonly string COLLIDER_INFO = "TODO";

		public static readonly string TRIGGER = "TODO";
		public static readonly string TRIGGER_INFO = "TODO";



		//public static readonly string ESSENTIALS_SYSTEM_GROUND_ORIENTATION = "";
		public static readonly string GROUND_CHECK = "Here you can define the desired method to handle ground related checks and " +
			"movements.";
		public static readonly string GROUND_CHECK_CUSTOM = "Use CustomGroundLevel to specify the ground level as desired";

		public static readonly string WATER_CHECK = "Here you can define the desired method to handle water related checks. By default" +
			"ICE will use Unitys Water Layer, but you are free to define custom water layer if required.";

		public static readonly string OBSTACLE_CHECK = "Here you can define the desired method to handle water related checks. By default" +
			"ICE will use Unitys Water Layer, but you are free to define custom water layer if required.";


		// ################################################################################
		// RUNTIME BEHAVIOUR
		// ################################################################################

		public static readonly string RUNTIME_BEHAVIOURS = "The Runtime Behaviours contains optional settings to define and/or optimize the general behaviour of " +
			"an entity during the runtime. ";
		public static readonly string RUNTIME_CULLING_CONDITIONS = "TODO";
		public static readonly string RUNTIME_CULLING_CONDITIONS_MAIN_CAMERA_DISTANCE = "Main Camera Distances defines the required minimum and maximum distances " +
			"in relation of the viewing direction of the current main camera, so positive values represents the distance within the given field of view, while " +
			"negative values defines the distance within invisible areas behind the camera. Whenever the creature leaves this specified range it will be removed " +
			"according to the given pool management rules of its species.";
		public static readonly string RUNTIME_FIXED_UPDATE_MOVE = "";
		public static readonly string RUNTIME_COROUTINE = "ICE is using coroutines to handle all sense and preparing procedures separated " +
			"from Unity’s frame update. You can deactivate the coroutines for debugging or adjusting your creature settings. ";
		public static readonly string RUNTIME_NAME = "Adapts the name of the GameObject during the runtime.";
		public static readonly string USE_HIERARCHY_MANAGEMENT = "Allows the Hierarchy Management to reassigns the parent if required.";
		public static readonly string RUNTIME_DONTDESTROYONLOAD = "Makes that your creature will not be destroyed automatically when loading " +
			"a new scene.";
		public static readonly string RUNTIME_APPLICATIONFOCUS = "Makes that your creature will break while the application loss the fokus (e.g. pause mode etc.)";
		public static readonly string RUNTIME_BASE_OFFSET = "The Base Offset defines the vertical level offset related to the given transform.position." +
			"You should adapt this value whenever the given transform.position doesn't represent the base point of your entity.";

		// ################################################################################
		// LIFESPAN
		// ################################################################################

		public static readonly string LIFESPAN = "Lifespan defines a default runtime limit (in seconds). If this limit is reached the object will remove itself.";
		public static readonly string LIFESPAN_INTERVAL = "TODO";
		public static readonly string LIFESPAN_DETACH = "TODO";


		public static readonly string AGING = "The ‘Use Aging’ flag activates the aging process, which will limited the life-cycle of your creature and " +
			"will have additional influence to the Fitness as well. Please note, that a limited life-cycle consequently means that your creature will die at the " +
			"end of the cycle. ";
		public static readonly string AGING_AGE = "Current Age represents the age of your creature at runtime. You can adjust this value to define an " +
			"initial age or you can modify the value also during the runtime. Please note, that the effective time data are in seconds, the use of minutes is " +
			"for the editor mask only.";
		public static readonly string AGING_MAXAGE = "Maximum Age defines the maximum length of the life-cycle and consequently the time of death as well. " +
			"Please note, that the effective time data are in seconds, the use of minutes is for the editor mask only.";

		public static readonly string LAYERS = "Use layers to define the required and/or desired layer. Use ADD LAYER to add a new one or the 'X' button to remove the selected one.";


		// ################################################################################
		// IMPACT
		// ################################################################################

		public static readonly string IMPACT = "Impact contains the settings to define the consequences for other entities while colliding with an Item or a BodyPart " +
			"object in your scene. In addition to the Damage and Force values you can define Sounds and an Effect which will be used during the impact process.";
		public static readonly string IMPACT_TYPE = "The Damage Transfer Type defines the mode how the impact damage will be sent to the involved GameObjects.";
		public static readonly string IMPACT_TYPE_DIRECT = "While the Direct Mode is selected the specified damage and force values will be sent by calling the " +
			"AddDamage method of an ICEWorldEntity class directly.";
		public static readonly string IMPACT_TYPE_MESSAGE = "While the Message Mode is selected the damage value will be sent by using SendMessageUpwards with the " +
			"specified method name and float value.";
		public static readonly string IMPACT_TYPE_DIRECT_OR_MESSAGE = "While the DirectOrMessage Mode is selected, the impact handler will try to call the AddDamage " +
			"method and will use SendMessageUpwards method only in cases the first procedure failed.";
		public static readonly string IMPACT_TYPE_DIRECT_AND_MESSAGE = "While the DirectAndMessage Mode is selected, the impact handler will try to call the " +
			"AddDamage method and in addition also the SendMessageUpwards method. Please note, that this will results in a double damage for each affected ICEWorldEntity " +
			"object while using ‘ApplyDamage’ as method name.";
		public static readonly string IMPACT_DAMAGE_METHOD = "Damage Method defines the desired method name which shall be used to send the damage data.";
		public static readonly string IMPACT_DAMAGE_VALUE = "Damage Value defines the desired damage value.";
		public static readonly string IMPACT_FORCE_TYPE = "The Force Type specifies the kind of the impact forces.";
		public static readonly string IMPACT_FORCE_ENERGY = "Energy specifies the desired minimum and maximum energy range of the force.";
		public static readonly string IMPACT_EXPLOSION_RADIUS = "Explosion Radius defines the radius in which objects will be affected.";
		public static readonly string IMPACT_AUDIO = "Sound defines the desired audio clips which shall be used during an impact.";
		public static readonly string IMPACT_EFFECT = "Effect defines the desired effect which shall be used during an impact.";
		public static readonly string IMPACT_BEHAVIOUR = "The Behaviour section contains optional settings to optimize and adapt the impact according to the given object " +
			"or rather your individual needs and requirements. ";
		public static readonly string IMPACT_BEHAVIOUR_LAYER = "The Layer Mask allows you to restrict the impacts to one or more specified layer.";
		public static readonly string IMPACT_BEHAVIOUR_ALLOW_OWN_IMPACTS = "By default an impact will not effect objects within the own hierarchy but you can enable 'AllowOwnImpacts' to allow such impacts.";
		public static readonly string IMPACT_BEHAVIOUR_USE_IMPACT_DELAY = "TODO";
		public static readonly string IMPACT_BEHAVIOUR_IMPACT_DELAY = "TODO";
		public static readonly string IMPACT_BEHAVIOUR_USE_ATTACH = "If AttachOnHit is enabled the entity will be attached to its target subsequent to processing the impact procedure.";
		public static readonly string IMPACT_BEHAVIOUR_USE_HIDE = "If HideOnHit is enabled the entity will be hide subsequent to processing the impact procedure.";
		public static readonly string IMPACT_BEHAVIOUR_USE_DESTROY = "If DestroyOnHit is enabled the entity will be destroyed subsequent to processing the impact procedure.";
		public static readonly string IMPACT_BEHAVIOUR_DESTROY_DELAY = "Delay defines the delay time to destroy the object.";
		public static readonly string IMPACT_BEHAVIOUR_TRIGGERING_HITS = "TODO";


		// ################################################################################
		// EXPLOSIVE
		// ################################################################################

		public static readonly string EXPLOSIVE = "TODO";
		public static readonly string EXPLOSIVE_DETONATE_ON_CONTACT = "TODO";
		public static readonly string EXPLOSIVE_DETONATE_ON_DESTROYED = "TODO";
		public static readonly string EXPLOSIVE_DETONATE_ON_COUNTDOWN = "TODO";

		// ################################################################################
		// TOOL
		// ################################################################################

		public static readonly string TOOL = "TODO";
		public static readonly string TOOL_BEHAVIOUR = "TODO";
		public static readonly string TOOL_BEHAVIOUR_AUDIO = "TODO";
		public static readonly string TOOL_BEHAVIOUR_EFFECT = "TODO";

		public static readonly string WIZARD_TERRAIN_SIZE = "TODO";
		public static readonly string WIZARD_TERRAIN_HEIGHT = "TODO";
		public static readonly string WIZARD_TERRAIN_BASE = "TODO";
		public static readonly string WIZARD_TERRAIN_BUMPS = "TODO";
		public static readonly string WIZARD_TERRAIN_SMOOTHING = "TODO";
		public static readonly string WIZARD_TERRAIN_HILLS = "TODO";
		public static readonly string WIZARD_TERRAIN_TREES = "TODO";
		public static readonly string WIZARD_TERRAIN_GRAS = "TODO";
		public static readonly string WIZARD_TERRAIN_BUSHES = "TODO";
		public static readonly string WIZARD_TERRAIN_TURRETS = "TODO";
		public static readonly string WIZARD_TERRAIN_LOCATIONS = "TODO";
		public static readonly string WIZARD_TERRAIN_SHELTERS = "TODO";
		public static readonly string WIZARD_TERRAIN_WAYPOINTS = "TODO";
		public static readonly string WIZARD_TERRAIN_OBSTACLES = "TODO";
		public static readonly string WIZARD_TERRAIN_VALLEYS = "TODO";
		public static readonly string WIZARD_TERRAIN_PLATEAUS = "TODO";
	}
}

