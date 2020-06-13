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
using Sandbox.Common.ObjectBuilders;
using System.Linq.Expressions;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        class MainScript : MyMainScript
        {
            // INI keys
            const string MYNAME = "easy";
            static readonly MyIniKey MessageKey = new MyIniKey(MYNAME, "message");
            static readonly MyIniKey NameKey = new MyIniKey(MYNAME, "name");

            // State
            string Message;
            string Name;
            List<IMyTextPanel> Screens = new List<IMyTextPanel>();

            public MainScript()
            {
                AddCommand("SetName", SetName);
            }

            public override bool LoadConfig(MyIni config)
            {
                Message = config.Get(MessageKey).ToString("Hello, {0}!");
                return true;
            }

            public override void ScanBlocks(IMyGridTerminalSystem grid)
            {
                grid.GetBlocksOfType<IMyTextPanel>(
                    Screens,
                    Block.Facing(Base6Directions.Direction.Left, Me) & Block.OnGridOf(Me)
                );
                updateMessage();
            }

            public override void onLoad(MyIni data)
            {
                Name = data.Get(NameKey).ToString("anonymous");
            }

            public override void onSave(MyIni data)
            {
                data.Set(NameKey, Name);
            }

            void SetName(MyCommandLine args)
            {
                if (args.ArgumentCount != 2) { Echo("Usage: setname <name>"); return; }
                Name = args.Argument(1);
                updateMessage();
            }

            void updateMessage()
            {
                var message = string.Format(Message, Name);
                foreach (var surface in Screens) {
                    surface.ContentType = ContentType.TEXT_AND_IMAGE;
                    surface.WriteText(message);
                }
            }
        }
    }
}
