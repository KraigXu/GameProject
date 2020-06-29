using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	
	public class SubSoundDef : Editable
	{
		
		public virtual void TryPlay(SoundInfo info)
		{
			if (this.resolvedGrains.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot play ",
					this.parentDef,
					" (subSound ",
					this,
					"_: No resolved grains."
				}), false);
				return;
			}
			if (!Find.SoundRoot.oneShotManager.CanAddPlayingOneShot(this.parentDef, info))
			{
				return;
			}
			if (Current.Game != null && !this.gameSpeedRange.Includes((int)Find.TickManager.CurTimeSpeed))
			{
				return;
			}
			ResolvedGrain resolvedGrain = this.RandomizedResolvedGrain();
			ResolvedGrain_Clip resolvedGrain_Clip = resolvedGrain as ResolvedGrain_Clip;
			if (resolvedGrain_Clip != null)
			{
				if (SampleOneShot.TryMakeAndPlay(this, resolvedGrain_Clip.clip, info) == null)
				{
					return;
				}
				SoundSlotManager.Notify_Played(this.parentDef.slot, resolvedGrain_Clip.clip.length);
			}
			if (this.distinctResolvedGrainsCount > 1)
			{
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					while (this.recentlyPlayedResolvedGrains.Count >= this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Dequeue();
					}
					if (this.recentlyPlayedResolvedGrains.Count < this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Enqueue(resolvedGrain);
						return;
					}
				}
				else if (this.repeatMode == RepeatSelectMode.NeverTwice)
				{
					this.lastPlayedResolvedGrain = resolvedGrain;
				}
			}
		}

		
		public ResolvedGrain RandomizedResolvedGrain()
		{
			ResolvedGrain chosenGrain = null;
			for (;;)
			{
				chosenGrain = this.resolvedGrains.RandomElement<ResolvedGrain>();
				if (this.distinctResolvedGrainsCount <= 1)
				{
					break;
				}
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					if (!(from g in this.recentlyPlayedResolvedGrains
					where g.Equals(chosenGrain)
					select g).Any<ResolvedGrain>())
					{
						break;
					}
				}
				else if (this.repeatMode != RepeatSelectMode.NeverTwice || !chosenGrain.Equals(this.lastPlayedResolvedGrain))
				{
					break;
				}
			}
			return chosenGrain;
		}

		
		public float RandomizedVolume()
		{
			return this.volumeRange.RandomInRange / 100f;
		}

		
		public override void ResolveReferences()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.resolvedGrains.Clear();
				foreach (AudioGrain audioGrain in this.grains)
				{
					foreach (ResolvedGrain item in audioGrain.GetResolvedGrains())
					{
						this.resolvedGrains.Add(item);
					}
				}
				this.distinctResolvedGrainsCount = this.resolvedGrains.Distinct<ResolvedGrain>().Count<ResolvedGrain>();
				this.numToAvoid = Mathf.FloorToInt((float)this.distinctResolvedGrainsCount / 2f);
				if (this.distinctResolvedGrainsCount >= 6)
				{
					this.numToAvoid++;
				}
			});
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.resolvedGrains.Count == 0)
			{
				yield return "No grains resolved.";
			}
			if (this.sustainAttack + this.sustainRelease > this.sustainLoopDurationRange.TrueMin)
			{
				yield return "Attack + release < min loop duration. Sustain samples will cut off.";
			}
			if (this.distRange.min > this.distRange.max)
			{
				yield return "Dist range min/max are reversed.";
			}
			if (this.gameSpeedRange.max == 0)
			{
				yield return "gameSpeedRange should have max value greater than 0";
			}
			if (this.gameSpeedRange.min > this.gameSpeedRange.max)
			{
				yield return "gameSpeedRange min/max are reversed.";
			}
			foreach (SoundParameterMapping soundParameterMapping in this.paramMappings)
			{
				if (soundParameterMapping.inParam == null || soundParameterMapping.outParam == null)
				{
					yield return "At least one parameter mapping is missing an in or out parameter.";
					break;
				}
				if (soundParameterMapping.outParam != null)
				{
					Type neededFilter = soundParameterMapping.outParam.NeededFilterType;
					if (neededFilter != null && !(from fil in this.filters
					where fil.GetType() == neededFilter
					select fil).Any<SoundFilter>())
					{
						yield return "A parameter wants to modify the " + neededFilter.ToString() + " filter, but this sound doesn't have it.";
					}
				}
			}
			List<SoundParameterMapping>.Enumerator enumerator = default(List<SoundParameterMapping>.Enumerator);
			yield break;
			yield break;
		}

		
		public override string ToString()
		{
			return this.name;
		}

		
		[Description("A name to help you identify the sound.")]
		[DefaultValue("UnnamedSubSoundDef")]
		[MayTranslate]
		public string name = "UnnamedSubSoundDef";

		
		[Description("Whether this sound plays on the camera or in the world.\n\nThis must match what the game expects from the sound Def with this name.")]
		[DefaultValue(false)]
		public bool onCamera;

		
		[Description("Whether to mute this subSound while the game is paused (either by the pausing in play or by opening a menu)")]
		[DefaultValue(false)]
		public bool muteWhenPaused;

		
		[Description("Whether this subSound's tempo should be affected by the current tick rate.")]
		[DefaultValue(false)]
		public bool tempoAffectedByGameSpeed;

		
		[Description("The sound grains used for this sample. The game will choose one of these randomly when the sound plays. Sustainers choose one for each sample as it begins.")]
		public List<AudioGrain> grains = new List<AudioGrain>();

		
		[EditSliderRange(0f, 100f)]
		[Description("This sound will play at a random volume inside this range.\n\nSustainers will choose a different random volume for each sample.")]
		[DefaultFloatRange(50f, 50f)]
		public FloatRange volumeRange = new FloatRange(50f, 50f);

		
		[EditSliderRange(0.05f, 2f)]
		[Description("This sound will play at a random pitch inside this range.\n\nSustainers will choose a different random pitch for each sample.")]
		[DefaultFloatRange(1f, 1f)]
		public FloatRange pitchRange = FloatRange.One;

		
		[EditSliderRange(0f, 200f)]
		[Description("This sound will play max volume when it is under minDistance from the camera.\n\nIt will fade out linearly until the camera distance reaches its max.")]
		[DefaultFloatRange(25f, 70f)]
		public FloatRange distRange = new FloatRange(25f, 70f);

		
		[Description("When the sound chooses the next grain, you may use this setting to have it avoid repeating the last grain, or avoid repeating any of the grains in the last X played, X being half the total number of grains defined.")]
		[DefaultValue(RepeatSelectMode.NeverLastHalf)]
		public RepeatSelectMode repeatMode = RepeatSelectMode.NeverLastHalf;

		
		[Description("Mappings between game parameters (like fire size or wind speed) and properties of the sound.")]
		[DefaultEmptyList(typeof(SoundParameterMapping))]
		public List<SoundParameterMapping> paramMappings = new List<SoundParameterMapping>();

		
		[Description("The filters to be applied to this sound.")]
		[DefaultEmptyList(typeof(SoundFilter))]
		public List<SoundFilter> filters = new List<SoundFilter>();

		
		[Description("A range of possible times between when this sound is triggered and when it will actually start playing.")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange startDelayRange = FloatRange.Zero;

		
		[Description("A range of game speeds this sound can be played on.")]
		public IntRange gameSpeedRange = new IntRange(0, 999);

		
		[Description("If true, each sample in the sustainer will be looped and ended only after sustainerLoopDurationRange. If not, the sounds will just play once and end after their own length.")]
		[DefaultValue(true)]
		public bool sustainLoop = true;

		
		[EditSliderRange(0f, 10f)]
		[Description("The range of durations that individual looped samples in the sustainer will have. Each sample ends after a time randomly chosen in this range.\n\nOnly used if the sustainer is looped.")]
		[DefaultFloatRange(9999f, 9999f)]
		public FloatRange sustainLoopDurationRange = new FloatRange(9999f, 9999f);

		
		[EditSliderRange(-2f, 2f)]
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainInterval")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange sustainIntervalRange = FloatRange.Zero;

		
		[EditSliderRange(0f, 2f)]
		[Description("The fade-in time of each sample. The sample will start at 0 volume and fade in over this number of seconds.")]
		[DefaultValue(0f)]
		public float sustainAttack;

		
		[Description("Skip the attack on the first sustainer sample.")]
		[DefaultValue(true)]
		public bool sustainSkipFirstAttack = true;

		
		[EditSliderRange(0f, 2f)]
		[Description("The fade-out time of each sample. At this number of seconds before the sample ends, it will start fading out. Its volume will be zero at the moment it finishes fading out.")]
		[DefaultValue(0f)]
		public float sustainRelease;

		
		[Unsaved(false)]
		public SoundDef parentDef;

		
		[Unsaved(false)]
		private List<ResolvedGrain> resolvedGrains = new List<ResolvedGrain>();

		
		[Unsaved(false)]
		private ResolvedGrain lastPlayedResolvedGrain;

		
		[Unsaved(false)]
		private int numToAvoid;

		
		[Unsaved(false)]
		private int distinctResolvedGrainsCount;

		
		[Unsaved(false)]
		private Queue<ResolvedGrain> recentlyPlayedResolvedGrains = new Queue<ResolvedGrain>();
	}
}
