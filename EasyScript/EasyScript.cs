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
	partial class Program : MyGridProgram
	{
		abstract class MyMainScript
		{
			protected IMyProgrammableBlock Me { get; private set; }
			protected IMyGridProgramRuntimeInfo Runtime { get; private set; }
			protected IMyIntergridCommunicationSystem IGC { get; private set; }
			public Action<string> Echo;

			// Actions
			public virtual bool LoadConfig(MyIni config) { return true; }
			public virtual void ScanBlocks(IMyGridTerminalSystem grid) { }

			// Events
			public virtual void onLoad(MyIni data) { }
			public virtual void onSave(MyIni data) { }
			public virtual void onIGCMessage() { }
			public virtual void onTick(UpdateType source) { }

			public virtual void onCommand(MyCommandLine args)
			{
				var key = args.Argument(0).ToLower();
				Action<MyCommandLine> handler;
				if (!m_commands.TryGetValue(key, out handler)) {
					var text = new StringBuilder();
					text.Append("Commands: ");
					text.Append(String.Join(", ", m_commands.Keys));
					Echo(text.ToString());
					return;
				}
				handler(args);
			}

			// Internal API
			Dictionary<string, Action<MyCommandLine>> m_commands = new Dictionary<string, Action<MyCommandLine>>();

			protected void AddCommand(string name, Action<MyCommandLine> handler)
			{
				m_commands.Add(name.ToLower(), handler);
			}

			// Internals
			internal void _setContext(Program main)
			{
				Me = main.Me;
				Runtime = main.Runtime;
				IGC = main.IGC;
			}
		}

		readonly MyCommandLine m_command = new MyCommandLine();
		MyMainScript m_instance;

		public Program() { Runtime.UpdateFrequency = UpdateFrequency.Once; }

		bool Init()
        {
			m_instance = new MainScript() { Echo = Echo };
			m_instance._setContext(this);
			if (!LoadConfig()) { m_instance = null; return false; }

			var saved = new MyIni();
			saved.TryParse(Storage);
			m_instance.onLoad(saved);

			m_instance.ScanBlocks(GridTerminalSystem);
			return true;
		}

		bool LoadConfig()
        {
			var config = new MyIni();
			MyIniParseResult result;
			if (!config.TryParse(Me.CustomData, out result)) {
				Echo($"Cannot parse configuration {result}");
				return false;
			}
			return m_instance.LoadConfig(config);
		}

		public void Main(string argument, UpdateType source)
		{
			if (m_instance == null && !Init()) { return; }

			if ((source & (UpdateType.Mod | UpdateType.Script | UpdateType.Terminal | UpdateType.Trigger)) != 0) {
				if (m_command.TryParse(argument)) {
					m_instance.onCommand(m_command);
				} else {
					Echo("Rescanning");
					m_instance.ScanBlocks(GridTerminalSystem);	// with on args, rescan blocks
				}
			}
			if ((source & (UpdateType.IGC)) != 0) {	m_instance.onIGCMessage(); }
			if ((source & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0) {
				m_instance.onTick(source);
			}
		}

		public void Save()
		{
			var ini = new MyIni();
			m_instance.onSave(ini);
			Storage = ini.ToString();
		}
	}
}
