﻿using System;
using UnityEngine;

namespace Verse.Sound
{
	
	public class AudioSourcePool
	{
		
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		
		public AudioSource GetSource(bool onCamera)
		{
			if (onCamera)
			{
				return this.sourcePoolCamera.GetSourceCamera();
			}
			return this.sourcePoolWorld.GetSourceWorld();
		}

		
		public AudioSourcePoolCamera sourcePoolCamera;

		
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
