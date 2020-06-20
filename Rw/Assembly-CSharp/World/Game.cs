using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{

    public class Game 
    {
		private List<Map> _maps;

		public List<Map> Maps
		{
			get
			{
				return this._maps;
			}
		}


		public void UpdatePlay()
		{
			//this.tickManager.TickManagerUpdate();
			//	this.letterStack.LetterStackUpdate();
			//	this.World.WorldUpdate();
			MapUpdate();
		//	this.Info.GameInfoUpdate();
		//	GameComponentUtility.GameComponentUpdate();
		//	this.signalManager.SignalManagerUpdate();
		}

		public void MapUpdate()
        {
			for (int i = 0; i < this._maps.Count; i++)
			{
				this._maps[i].MapUpdate();
			}
		}



	}
}
