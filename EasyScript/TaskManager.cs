#define DEBUG 1

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
using ParallelTasks;

namespace IngameScript
{
	partial class Program
	{
		class Task
		{
			public enum ActionType { Call, Yield, Wait }

			public struct Action {
				public ActionType Type;
				public int Arg1;
				public IEnumerable<Task.Action> SubTask;
			}

			public static Action Call(IEnumerable<Task.Action> subTask)
			{
				return new Action() { Type = ActionType.Call, SubTask = subTask };
			}
			public static Action Wait(int ms) { return new Action() { Type = ActionType.Wait, Arg1 = ms }; }
		}


		public IEnumerable<Task.Action> Blink(IMyLightingBlock light)
		{
			while (true) {
				light.Enabled = !light.Enabled;
				yield return Task.Wait(1000);
			}
		}


		class TaskManager
		{
			struct Entry
			{
				public IEnumerator<Task.Action> Enumerator;
			}

			List<Entry> m_tasks = new List<Entry>();

			public void Add(IEnumerable<Task.Action> task)
			{
				var enumerator = task.GetEnumerator();
				if (enumerator.MoveNext()) {
					m_tasks.Add(new Entry() { Enumerator = enumerator });
				}
			}

			public void onTick(UpdateType updateSource)
			{
				var currentTasks = m_tasks;
				m_tasks = m_tasksCache;
				m_tasksCache = currentTasks;
				foreach (var task in currentTasks) {
					if (task.Run(argument, updateSource)) { m_tasks.Add(task); }
				}
				m_tasksCache.Clear();
			}
		}
	}
}
