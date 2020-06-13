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
using System.Security.Policy;

namespace IngameScript
{
    partial class Program
    {
        public class PID
        {
            public float SetPoint;
            public Func<float, float> Actuator;

            readonly float KP;
            readonly float bi;
            readonly float bd;
            readonly float br;
            readonly float RefWeight;

            float Ival = 0.0f;
            float? oldPV;

            public PID(float proportional, float integrative, float derivative,
                       float period, float refWeight, float windupTime, Func<float, float> actuator)
            {
                KP = proportional;
                bi = integrative * period;
                bd = derivative / period;
                RefWeight = refWeight;
                br = period / windupTime;
                Actuator = actuator;
            }

            public float run(float PV)
            {
                var Pval = KP * (RefWeight * SetPoint - PV);
                var Dval = oldPV.HasValue ? bd * (PV - oldPV.Value) : 0f;
                var value = Pval + Ival + Dval;

                var command = Actuator(value);

                Ival = Ival + bi * (SetPoint - PV) + br * (command - value);
                oldPV = PV;

                return command;
            }
        }
    }
}
