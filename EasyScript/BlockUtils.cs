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
using Sandbox;

namespace IngameScript
{
    partial class Program
    {
        class Block
        {
            public struct Predicate
            {
                Func<IMyTerminalBlock, bool> Evaluator;

                public Predicate(Func<IMyTerminalBlock, bool> evaluator) { Evaluator = evaluator; }
                public static implicit operator Func<IMyTerminalBlock, bool>(Predicate p) => p.Evaluator;
                public static Predicate operator !(Predicate p)
                    => new Predicate(block => !p.Evaluator(block));
                public static Predicate operator &(Predicate lhs, Predicate rhs)
                    => new Predicate(block => lhs.Evaluator(block) && rhs.Evaluator(block));
                public static Predicate operator |(Predicate lhs, Predicate rhs)
                    => new Predicate(block => lhs.Evaluator(block) || rhs.Evaluator(block));
            }

            static public Predicate Facing(Base6Directions.Direction direction, IMyTerminalBlock reference)
            {
                var transformed = reference.Orientation.TransformDirection(direction);
                return new Predicate(subject => subject.Orientation.Forward == transformed);
            }
            static public Predicate OnGridOf(IMyTerminalBlock block)
                => new Predicate(subject => subject.IsSameConstructAs(block));
            static public readonly Predicate WithInventory
                = new Predicate(subject => subject.InventoryCount > 0);
            static public Predicate WithSection(string name)
                => new Predicate(subject => MyIni.HasSection(subject.CustomData, name));
        }
    }
}
