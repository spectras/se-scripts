using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
	partial class Program
	{
#if false
		public interface IDisplay
	    {
			List<MySprite> Render();
	    }

		public class DisplayManager : SubProgram {
			public readonly Dictionary<string, IDisplay> Displays = new Dictionary<string, IDisplay>();

			public DisplayManager(ProgramHost host) : base(host)
            {
				UpdateFrequency = UpdateFrequency.Update100;
            }

			public override void Main(string argument, UpdateType updateSource)
            {
				if ((updateSource & UpdateType.Update100) != 0) { Render(); }
			}

			void Render()
            {

            }
		}
#endif
	}
}
