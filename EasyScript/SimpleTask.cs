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
using VRageRender.Messages;

namespace IngameScript
{
	partial class Program
	{
		sealed class SimpleDispatcher
		{
			public delegate bool Task();

			Task m_tasks1;
			Task m_tasks10;
			Task m_tasks100;

			public UpdateFrequency UpdateFrequency {
				get {
					return UpdateFrequency.None
						| (m_tasks1 != null ? UpdateFrequency.Update1 : 0)
						| (m_tasks10 != null ? UpdateFrequency.Update10 : 0)
						| (m_tasks100 != null ? UpdateFrequency.Update100 : 0);
				}
			}

			public void Add(Task task, UpdateFrequency frequency)
			{
				if ((frequency & UpdateFrequency.Update1) != 0) { m_tasks1 += task; }
				if ((frequency & UpdateFrequency.Update10) != 0) { m_tasks10 += task; }
				if ((frequency & UpdateFrequency.Update100) != 0) { m_tasks100 += task; }
			}

			public void Run(UpdateType source)
			{
				if ((source & UpdateType.Update1) != 0 && m_tasks1 != null) { m_tasks1(); }
				if ((source & UpdateType.Update10) != 0 && m_tasks10 != null) { m_tasks10(); }
				if ((source & UpdateType.Update100) != 0 && m_tasks100 != null) { m_tasks100(); }
			}
		}
	}
}
