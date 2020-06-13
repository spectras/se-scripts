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
		class MainScript : MyMainScript
		{
			// Savegames
			static readonly string SaveVersion = "1";
			static readonly MyIniKey CounterKey = new MyIniKey(SaveVersion, "counter");

			// State
			int Counter = 0;
			List<IMyTextPanel> Screens = new List<IMyTextPanel>();

			// Event handlers
			public override void onLoad(MyIni data) {
				Counter = data.Get(CounterKey).ToInt32();
				Runtime.UpdateFrequency = UpdateFrequency.Update100;
			}

			public override void ScanBlocks(IMyGridTerminalSystem grid)
			{
				grid.GetBlocksOfType<IMyTextPanel>(Screens, Block.OnGridOf(Me));
			}

			public override void onTick(UpdateType source)
			{
				Counter += 1;
				foreach (var screen in Screens) {
					screen.ContentType = ContentType.TEXT_AND_IMAGE;
					screen.WriteText(Counter.ToString());
				}
			}

			public override void onCommand(MyCommandLine args)
			{
				if (args.Argument(0) == "reset") { Counter = 0; }
			}

			public override void onSave(MyIni data)
			{
				data.Set(CounterKey, Counter);
			}
		}
	}
}
